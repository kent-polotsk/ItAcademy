using DAL_CQS_.Queries;
using EFDatabase.Entities;
using GNA.Services.Abstractions;
using MediatR;

namespace GNA.Services.Implementations
{
    public class SourceService : ISourceService
    {
        private readonly IMediator _mediator;

        public SourceService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Source[]> GetSourceWithRssAsync(CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new GetAllSourcesWithRssQuery(),cancellationToken);
        }
    }
}
