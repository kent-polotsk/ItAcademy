using DAL_CQS_.Queries;
using DataConvert.DTO;
using EFDatabase;
using EFDatabase.Entities;
using GNA.Services.Abstractions;
using GNA.Services.Implementations;
using Mappers.Mappers;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace GNA.Services.Tests.Tests
{
    public class ArticleServiceTests
    {
        private readonly Article[] _positiveArticlesCollection =
        [
            new Article
            {
                Id=Guid.Parse("92B39566-E75D-4AF0-967D-14A19B781713"),
                PositivityRate = -2,
                Source = new Source
                {
                    Id = 1,
                    Name= "1"
                } },
            new Article { PositivityRate = -4,
                Source = new Source
                {
                    Id = 2,
                    Name= "1"
                } },
            new Article { PositivityRate = -1,
                Source = new Source
                {
                    Id = 1,
                    Name= "1"
                } },
            new Article { PositivityRate = -3,
                Source = new Source
                {
                    Id = 2,
                    Name= "1"
                }},
            new Article { PositivityRate = -5,
                Source = new Source
                {
                    Id = 3,
                    Name= "1"
                }},
            new Article { PositivityRate = 5    ,
                Source = new Source
                {
                    Id = 2,
                    Name= "1"
                } },
            new Article { PositivityRate = 4,
                Source = new Source
                {
                    Id = 3,
                    Name= "1"
                } },
            new Article { PositivityRate = 3,
                Source = new Source
                {
                    Id = 1,
                    Name= "1"
                } },
            new Article { PositivityRate = 2,
                Source = new Source
                {
                    Id = 3,
                    Name= "1"
                } },
            new Article { PositivityRate = 1,
                Source = new Source
                {
                    Id = 1,
                    Name= "1"
                } },
        ];

        private IMediator mediatorMock;
        private ILogger<AccountService> loggerMock;
        private IArticleService articleService;
        //private ArticleMapper mapperMock;
        private CancellationToken cancellationToken;



        private void Prepare()
        {
            mediatorMock = Substitute.For<IMediator>();
            loggerMock = Substitute.For<ILogger<AccountService>>();
            cancellationToken = default;
            //var mapperMock = Substitute.For<ArticleMapper>();
        }

        [Fact]
        public async Task GetAllPositiveAsync_CorrectPageAndPageSize_ReturnCollection()
        {

            var minRate = -5.2;
            var pageNumber = 1;
            var pageSize = 10;

            Prepare();

            //mediatorMock.Send(Arg.Any<GetPositiveArticlesWithPaginationQuery>, Arg.Any<CancellationToken>())
            //    .Returns(Task.FromResult(_positiveArticlesCollection));

            mediatorMock.Send(Arg.Is<GetPositiveArticlesWithPaginationQuery>(q => q.PositivityRate == minRate && q.Page == pageNumber && q.PageSize == pageSize),
            Arg.Any<CancellationToken>()).Returns(Task.FromResult(_positiveArticlesCollection
                    //.Where(article => article.PositivityRate == null || article.PositivityRate >= minRate)
                    //.Skip((pageNumber - 1) * pageSize)
                    //.Take(pageSize)
                    //.ToArray()
                    ));

            var articleService = new ArticleService(loggerMock, mediatorMock, new ArticleMapper());


            var result = await articleService.GetAllPositiveAsync(minRate, pageNumber, pageSize, cancellationToken);


            Assert.NotNull(result);
            Assert.Equal(10, result.Length);
            Assert.True(result[0].PositivityRate <= 5 && result[0].PositivityRate >= -5);
        }

        [Fact]
        public async Task GetArticleById_CorrectId_ReturnsArticle()
        {
            Prepare();
            var id = Guid.Parse("92B39566-E75D-4AF0-967D-14A19B781713");

            mediatorMock.Send(Arg.Is<GetArticleByIdQuery>(query => query.Id == id), Arg.Any<CancellationToken>()).Returns(Task.FromResult(new ArticleDto { Id = id }));
            var articleService = new ArticleService(loggerMock, mediatorMock, new ArticleMapper());

            var result = await articleService.GetByIdAsync(id, cancellationToken);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
        }

        [Fact]
        public async Task CountArticleById_CorrectId_ReturnsInt()
        {
            Prepare();
            var minRate = -5.1;
            
            mediatorMock.Send(Arg.Is<CountArticlesQuery>(query=>query.MinRate==minRate), Arg.Any<CancellationToken>()).Returns(Task.FromResult(_positiveArticlesCollection.Count(a=>a.PositivityRate>=minRate)));
            
            var articleService = new ArticleService(loggerMock, mediatorMock, new ArticleMapper());

            var result = await articleService.CountAsync(minRate, cancellationToken);

            Assert.NotNull(result);
            Assert.True(result>0);
            Assert.True(result<=10);
        }
    }
}