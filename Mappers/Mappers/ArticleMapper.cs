using EFDatabase.Entities;
using Riok.Mapperly.Abstractions;
using DataConvert.DTO;
using WebAppGNAggregator.Models;

namespace WebAppGNAggregator.Mappers
{
    [Mapper]
    public partial class ArticleMapper
    {
        [MapProperty($"{nameof(Article.Source)}.{nameof(Article.Source.Name)}", nameof(ArticleDto.SourceName))]
        public partial ArticleDto ArticleToArticleDto(Article article);


        public partial ArticleModel ArticleDtoToArticleModel(ArticleDto atricleDto);


    }
}

//{
//   var articleModel = new ArticleModel()
//    {
//        Id = article.Id,
//        Title = article.Title,
//        Description = article.Description,
//        Source = article.Source.Name,
//        CreationDate = article.Created,
//        Content = article.Content,
//        Rate = article.PositivityRate ?? 0
//    };
//    return articleModel;
//}