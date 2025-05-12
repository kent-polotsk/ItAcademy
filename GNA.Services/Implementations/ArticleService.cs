using DAL_CQS_.Commands;
using DAL_CQS_.Queries;
using DataConvert.DTO;
using EFDatabase.Entities;
using GNA.Services.Abstractions;
using HtmlAgilityPack;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using Mappers.Mappers;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace GNA.Services.Implementations
{
    public class ArticleService : IArticleService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly IMediator _mediator;
        private readonly ArticleMapper _articleMapper;
        private readonly InferenceSession _modelSession;

        public ArticleService(ILogger<AccountService> logger, IMediator mediator, ArticleMapper articleMapper)
        {
            _logger = logger;
            _mediator = mediator;
            _articleMapper = articleMapper;
            _modelSession = new InferenceSession(@"d:\C#\GNAggregator\GNAggregator\rubert_base_cased.onnx");
        }


        public async Task<ArticleDto?[]> GetAllPositiveAsync(double? minPositivityRate, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            try
            {
                var articleDtos =  (await _mediator.Send(new GetPositiveArticlesWithPaginationQuery()
                {
                    PositivityRate = minPositivityRate,
                    Page = pageNumber,
                    PageSize = pageSize
                }, cancellationToken))
                    .Select(article => _articleMapper.ArticleToArticleDto(article))
                    .ToArray();

                return articleDtos;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Articles getting error: {ex.Message}");
                throw;
            }
        }

        [HttpGet]
        public async Task<Article?[]> GetArticlesWithoutRate()
        {
            return await _mediator.Send(new GetArticlesWithoutRateQuery());
        }

        [HttpPost]
        public async Task<bool> RatingProcess(CancellationToken cancellationToken = default)
        {
            try
            {
                var articles = await GetArticlesWithoutRate();

                foreach (var a in articles)
                {
                    if (a.Content != null)
                    {
                        double? rate = PositivityRating(a.Content, cancellationToken);
                        if (rate != null)
                        {
                            a.PositivityRate = (double?)(Math.Round((decimal)rate, 2) * 10);
                        }
                    }
                }
                await _mediator.Send(new SaveRatedArticlesCommand() { Articles = articles });
                return true;
            }
            catch (Exception ex) 
            {
                _logger.LogError($"Error rating articles: {ex.Message}");
                return false;
            }
        }


        [HttpGet]
        public async Task<ArticleDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var article = await _mediator.Send(new GetArticleByIdQuery() { Id = id }, cancellationToken);
            return article;
        }



        public async Task<int> CountAsync(double minRate, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new CountArticlesQuery() { MinRate = minRate }, cancellationToken);
        }


        public async Task<string[]> GetUniqueArticlesUrls(CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetUniqueArticlesUrlsQuery() { }, cancellationToken);
        }


        [HttpPost]
        public async Task AddArticlesAsync(IEnumerable<Article> newUniqueArticles, CancellationToken cancellationToken)
        {
            await _mediator.Send(new AddArticlesCommand() { Articles = newUniqueArticles }, cancellationToken);
        }

        [HttpPost]
        public double? PositivityRating(string inputText, CancellationToken cancellationToken = default)
        {
            try
            {
                string tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");
                File.WriteAllText(tempFile, inputText);

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = @"C:\Users\KeNT\miniconda3\python.exe",
                    Arguments = $"\"D:\\C#\\GNAggregator\\GNaggregator\\tokenizer_script.py\" \"{tempFile}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                
                using Process process = Process.Start(psi);
                using StreamReader reader = process.StandardOutput;
                using StreamReader errorReader = process.StandardError;

                string result = reader.ReadToEnd().Trim();

                process.WaitForExit();

                if (string.IsNullOrEmpty(result))
                {
                    Console.WriteLine("StandardOutput is empty!");
                    return null;
                }

                long[] tokenIds = result.Split(' ').Select(long.Parse).ToArray();
                //Console.WriteLine($"Size of tokenIds: {tokenIds.Length}");

                const int maxLength = 512;
                Array.Resize(ref tokenIds, maxLength);

                var sentimentScore = RunModel(tokenIds);
                //_logger.LogInformation($"Rate after onnx model: sentimentScore {sentimentScore}");

                return (double)sentimentScore;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (double)0;
            }
        }

        private double RunModel(long[] tokens)
        {

            var inputs = new[] { NamedOnnxValue.CreateFromTensor("input.1", new DenseTensor<long>(tokens, new[] { 1, tokens.Length })) };
            var results = _modelSession.Run(inputs);
            //Console.WriteLine($"model result: {results.First()}");
            return results.First().AsEnumerable<float>().First();
        }



        public async Task UpdateTextForArticlesByWebScrappingAsync(CancellationToken cancellationToken = default)
        {
            var ids = await _mediator.Send(new GetIdsForArticlesWithNoTextQuery(), cancellationToken);
            var dictionary = new Dictionary<Guid, string>();

            foreach (var id in ids)
            {
                var article = await _mediator.Send(new GetArticleByIdQuery { Id = id }, cancellationToken);

                if (article == null || string.IsNullOrWhiteSpace(article.Url))
                {
                    _logger.LogWarning($"*ArticleService* Article {id} not found or URL doesn't exist");
                    continue;
                }

                var web = new HtmlWeb();
                var doc = await web.LoadFromWebAsync(article.Url, cancellationToken);

                if (doc == null)
                {
                    _logger.LogWarning($"*ArticleService* Unable to load document from {article.Url}");
                    continue;
                }

                HtmlNode articleNode = null;

                if (article.Url.Contains("onliner"))
                {
                    articleNode = doc.DocumentNode.SelectSingleNode("//div[@class='news-text']");
                }
                else if (article.Url.Contains("belta"))
                {
                    articleNode = doc.DocumentNode.SelectSingleNode("//div[@class='js-mediator-article']");
                }
                else
                {
                    articleNode = doc.DocumentNode.SelectSingleNode("//div[@class='article__text']");
                }

                if (articleNode == null)
                {
                    _logger.LogWarning($"*ArticleService* Unable to load data from {article.Url}");
                    continue;
                }

                string cleanedText = CleanHtmlText(articleNode);
                dictionary.Add(id, cleanedText);
            }

            await _mediator.Send(new UpdateTextForArticlesCommand()
            {
                Data = dictionary
            }, cancellationToken);
        }


        private string CleanHtmlText(HtmlNode node)
        {
            foreach (var script in node.SelectNodes("//script|//style") ?? Enumerable.Empty<HtmlNode>())
            {
                script.Remove();
            }

            string text = node.InnerText;

            return Regex.Replace(text, @"\s+", " ").Trim();
        }


        public async Task<bool> DeleteAsync(Guid Id, CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new DeleteArticleCommand() { id = Id }, cancellationToken);
        }

        public Task SaveChangedArticleAsync(ArticleDto atricleDto, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new SaveChangedArticleAsyncCommand() { articleDto = atricleDto }, cancellationToken);
        }
    
    }
}
