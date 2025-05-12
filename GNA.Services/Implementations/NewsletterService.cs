using DAL_CQS_.Commands;
using DAL_CQS_.Queries;
using GNA.Services.Abstractions;
using Mappers.Mappers;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.ML.OnnxRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNA.Services.Implementations
{
    public class NewsletterService : INewsletterService
    {
        private readonly ILogger<NewsletterService> _logger;
        private readonly IMediator _mediator;
        //private readonly ArticleMapper _articleMapper;
        private readonly IEmailService _emailService;


        public NewsletterService(ILogger<NewsletterService> logger, IMediator mediator, ArticleMapper articleMapper, IEmailService emailService)
        {
            _logger = logger;
            _mediator = mediator;
            //_articleMapper = articleMapper;
            _emailService = emailService;
        }


        public async Task SendEmailsAsync()
        {

            try
            {
                const string Adress = "https://localhost:7080/Articles/Details/";
                //1 set new articles
                var articleDtos = await _mediator.Send(new GetNewsForSendingQuery());

                //2 set users
                Dictionary<string, double> userData = await _mediator.Send(new GetUsersDataForNewsletterQuery());

                //3 sort & send
                if (articleDtos != null && articleDtos.Length>0 && userData != null && userData.Count > 0)
                {
                    foreach (var user in userData)
                    {
                        try
                        {
                            int count = 0;
                            string mailMessage = "Подборка новостей для вас!\n";
                            foreach (var article in articleDtos)
                                if (article.PositivityRate >= user.Value)
                                {
                                    mailMessage += $"{count + 1}) {article.Description}\n{Adress + article.Id}\n\n";
                                    count++;
                                }
                            if (count > 0)
                                await _emailService.SendNewsletterAsync(user.Key, mailMessage);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Unable to send newsletters to {user.Key} : {ex.Message}");
                            continue;
                        }
                    }

                    await _mediator.Send(new MarkSentNewsCommand() {ArticleIds  =  articleDtos.Select(a=>a.Id).ToArray() });

                    _logger.LogInformation("NewslettersService successfully sent new articles");
                }
                else
                {
                    _logger.LogInformation("No articles or users to send newsletters");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while sending newsletters: " + ex.Message);
            }

        }
    }
}
