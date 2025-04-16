using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFDatabase.Entities;

namespace GNA.Services.Abstractions
{
    public interface ISourceService
    {
        
        public Task<Source[]> GetSourceWithRssAsync(CancellationToken cancellationToken);
    }
}
