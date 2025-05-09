using GNAggregator.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GNAggregator.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {

        private static List<ArticleModel> Articles = new List<ArticleModel>()
        {
            new ArticleModel
            {
                Id = Guid.NewGuid(),
                Title = "Title1",
                Description = "D1",
                Content = "1234566",
                Created = DateTime.UtcNow,
                Url = "http",
                ImageUrl = "",

                PositivityRate = 1,

                SourceId  = 1,
                SourceName ="s1"
            },
            new ArticleModel
            {
                Id = Guid.NewGuid(),
                Title = "Title2",
                Description = "D2",
                Content = "2221234566",
                Created = DateTime.UtcNow,
                Url = "http2",
                ImageUrl = "",

                PositivityRate = 2,

                SourceId  = 2,
                SourceName ="s2"
            }
        };

        [HttpGet]
        public IActionResult Get() 
        {
            return Ok(Articles);
        }

        [HttpPost]
        public IActionResult AddArticle(ArticleModel articleModel)
        {
            Articles.Add(articleModel);
            return Ok(Articles);
         
        }
    }
}
