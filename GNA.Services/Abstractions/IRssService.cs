using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using EFDatabase.Entities;

namespace GNA.Services.Abstractions
{
    public interface IRssService
    {
        public Task<Article[]> GetRssDataAsync(Source rss, CancellationToken token = default);
        public (string, string) GetImageUrlAndContent(SyndicationItem item);
        public Article GetArticleFromSyndicationItem(SyndicationItem item, int sourceId);
    }
}
