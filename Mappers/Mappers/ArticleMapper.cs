using EFDatabase.Entities;
using Riok.Mapperly.Abstractions;
using DataConvert.DTO;
using System.Linq;

namespace Mappers.Mappers
{
    [Mapper]
    public partial class ArticleMapper
    {
        [MapProperty($"{nameof(Article.Source)}.{nameof(Article.Source.Name)}", nameof(ArticleDto.SourceName))]
        public partial ArticleDto ArticleToArticleDto(Article? article);

        public partial ArticleModel ArticleDtoToArticleModel(ArticleDto? atricleDto);

        public partial ArticleDto ArticleModelToArticleDto(ArticleModel? articleModel);

        [MapperIgnoreTarget(nameof(Article.Source))]
        [MapperIgnoreTarget(nameof(Article.Comments))]
        [MapperIgnoreTarget(nameof(Article.SourceId))]
        [MapperIgnoreTarget(nameof(Article.Url))]
        [MapperIgnoreTarget(nameof(Article.ImageUrl))]
        public partial void UpdateArticleFromDto(ArticleDto sourceDto, Article targetArticle);

    }
}
