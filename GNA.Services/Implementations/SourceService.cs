using DAL_CQS_.Queries;
using DataConvert.DTO;
using EFDatabase.Entities;
using GNA.Services.Abstractions;
using MediatR;
using Mappers.Mappers;
using Microsoft.EntityFrameworkCore;

namespace GNA.Services.Implementations
{
    public class SourceService : ISourceService
    {
        private readonly IMediator _mediator;
        private readonly SourceMapper _sourceMapper;

        public SourceService(IMediator mediator, SourceMapper sourceMapper)
        {
            _mediator = mediator;
            _sourceMapper = sourceMapper;
        }

        public async Task<Source[]> GetSourceWithRssAsync(CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new GetAllSourcesWithRssQuery(),cancellationToken);
        }

        public async Task<SourceDto[]> GetSourceDtosWithRssAsync(CancellationToken cancellationToken = default)
        {
            var sourceDtos  =  (await _mediator.Send(new GetAllSourcesWithRssQuery(), cancellationToken))
                .Select(s => _sourceMapper.SourceToSourceDto(s))
                .ToArray();
            return sourceDtos;
        }
    }
}
