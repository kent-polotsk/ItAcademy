using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataConvert.DTO;
using EFDatabase.Entities;

namespace GNA.Services.Abstractions
{
    public interface ISourceService
    {
        public Task<Source[]> GetSourceWithRssAsync(CancellationToken cancellationToken);
        public Task<SourceDto[]> GetSourceDtosWithRssAsync(CancellationToken cancellationToken);
    }
}
