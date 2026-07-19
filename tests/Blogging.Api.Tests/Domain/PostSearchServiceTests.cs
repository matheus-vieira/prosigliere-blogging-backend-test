using Blogging.Domain.Posts;
using Moq;

namespace Blogging.Api.Tests;

/// <summary>
/// Verifies post search validation and specification delegation.
/// </summary>
[TestClass]
[TestCategory("Domain")]
public sealed class PostSearchServiceTests
{
    /// <summary>
    /// Confirms that valid filters are delegated as a specification.
    /// </summary>
    [TestMethod]
    public async Task SearchAsyncDelegatesValidFilterAsync()
    {
        var expected = new PagedResult<BlogPostSummary>(
            [new BlogPostSummary(1, "A title", 0)],
            1,
            20,
            1);
        var repository = new Mock<IBlogPostRepository>();
        repository.Setup(item => item.SearchAsync(
                It.IsAny<Blogging.Domain.Specifications.ISpecification<Blogging.Domain.Entities.BlogPost>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        var service = new PostSearchService(repository.Object);

        var result = await service.SearchAsync(
                new PostFilter { Title = "api", Page = 1, PageSize = 20 },
                CancellationToken.None)
            .ConfigureAwait(false);

        Assert.AreEqual(expected, result);
        repository.Verify(item => item.SearchAsync(
                It.IsAny<Blogging.Domain.Specifications.ISpecification<Blogging.Domain.Entities.BlogPost>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Confirms that invalid pagination and ranges are rejected.
    /// </summary>
    [TestMethod]
    public void SearchAsyncRejectsInvalidOptions()
    {
        var repository = new Mock<IBlogPostRepository>();
        var service = new PostSearchService(repository.Object);

        Assert.ThrowsExactly<ArgumentException>(() => service.SearchAsync(
            new PostFilter { Page = 0 },
            CancellationToken.None));
        Assert.ThrowsExactly<ArgumentException>(() => service.SearchAsync(
            new PostFilter { MinCommentCount = 3, MaxCommentCount = 1 },
            CancellationToken.None));
        repository.VerifyNoOtherCalls();
    }
}
