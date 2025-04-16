using EFDatabase.Entities;
using Riok.Mapperly.Abstractions;
using DataConvert.DTO;

namespace WebAppGNAggregator.Mappers
{
    [Mapper]
    public partial class ArticleMapper
    {
        [MapProperty($"{nameof(Article.Source)}.{nameof(Article.Source.Name)}", nameof(ArticleDto.SourceName))]
        public partial ArticleDto ArticleToArticleDto(Article? article);


        public partial ArticleModel ArticleDtoToArticleModel(ArticleDto? atricleDto);


    }
}
