using DataConvert.DTO;
using EFDatabase.Entities;
using Riok.Mapperly.Abstractions;

namespace Mappers.Mappers
{
    [Mapper]
    public partial class SourceMapper
    {
        public partial SourceDto SourceToSourceDto(Source? source);
    }
}
