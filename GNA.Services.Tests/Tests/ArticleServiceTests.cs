using DAL_CQS_.Queries;
using EFDatabase;
using EFDatabase.Entities;
using GNA.Services.Implementations;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace GNA.Services.Tests.Tests
{
    public class ArticleServiceTests
    {
        private readonly Article[] _positiveArticlesCollection =
        [
            new Article { PositivityRate = -2},
            new Article { PositivityRate = -4},
            new Article { PositivityRate = -1},
            new Article { PositivityRate = -3},
            new Article { PositivityRate = -5},
            new Article { PositivityRate = 5},
            new Article { PositivityRate = 4},
            new Article { PositivityRate = 3},
            new Article { PositivityRate = 2},
            new Article { PositivityRate = 1},
        ];

        [Fact]
        public async Task GetAllPositiveAsync_CorrectPageAndPageSize_ReturnCollection()
        {
            var mediatorMock = Substitute.For<IMediator>();

            mediatorMock.Send(Arg.Any<GetPositiveArticlesWithPaginationQuery>, Arg.Any<CancellationToken>())
                .Returns(_positiveArticlesCollection.AsQueryable());
            var loggerMock = Substitute.For<ILogger<ArticleService>>();
            var dbContextMock = Substitute.For<GNAggregatorContext>();

            var articleService = new ArticleService(dbContextMock, loggerMock, mediatorMock);
            var minRate = -5;
            var pageNumber = 1;
            var pageSize = 10;

            CancellationToken cancellationToken = default;


            var result = await articleService.GetAllPositiveAsync(minRate, pageNumber, pageSize, cancellationToken);




            Assert.NotNull(result);
            Assert.Equal(10, result.Length);
            // Assert.InRange(result[0].PositivityRate, 0,5);
        }
    }
}