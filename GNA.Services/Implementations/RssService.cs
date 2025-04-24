using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using EFDatabase.Entities;
using GNA.Services.Abstractions;

namespace GNA.Services.Implementations
{
    public class RssService : IRssService
    {

        public async Task<Article[]> GetRssDataAsync(Source rss, CancellationToken token)
        {
            try
            {
                if (rss == null)
                {
                    throw new ArgumentNullException(nameof(rss));
                }

                using (var xmlReader = XmlReader.Create(rss.RSSURL))
                {
                    var feed = SyndicationFeed.Load(xmlReader);
                    var items = feed.Items.Select(item => GetArticleFromSyndicationItem(item, rss.Id))
                        .ToArray();
                    return items;
                }
            }
            catch
            {
                throw;
            }
        }


        //Filling article info from rss feed
        public Article GetArticleFromSyndicationItem(SyndicationItem item, int sourceId)
        {
            var (imageUrl, content) = GetImageUrlAndContent(item);

            var article = new Article()
            {
                Id = Guid.NewGuid(),
                Title = item.Title.Text,
                Description = content,
                ImageUrl = imageUrl,
                Created = item.PublishDate.UtcDateTime,
                Url = item.Id,
                SourceId = sourceId
            };
            return article;
        }


        //Getting image and text
        public (string, string) GetImageUrlAndContent(SyndicationItem item)
        {
            var content = item.Summary?.Text ?? item.Content?.ToString() ?? string.Empty;
            var imageUrl = string.Empty;
            if (content != null)
            {
                var match = Regex.Match(content, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    imageUrl = match.Groups[1].Value;
                }
            }
            return (imageUrl, content);
        }
    }
}
