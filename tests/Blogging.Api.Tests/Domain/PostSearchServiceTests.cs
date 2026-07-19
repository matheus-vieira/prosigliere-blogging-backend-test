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

    /// <summary>
    /// Confirms that all optional filters can be composed together.
    /// </summary>
    [TestMethod]
    public async Task SearchAsyncComposesAllFiltersAsync()
    {
        var repository = new Mock<IBlogPostRepository>();
        repository.Setup(item => item.SearchAsync(
                It.IsAny<Blogging.Domain.Specifications.ISpecification<Blogging.Domain.Entities.BlogPost>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<BlogPostSummary>([], 2, 10, 0));
        var service = new PostSearchService(repository.Object);

        var result = await service.SearchAsync(
                new PostFilter
                {
                    Title = " title ",
                    Content = " content ",
                    HasComments = true,
                    MinCommentCount = 1,
                    MaxCommentCount = 5,
                    Page = 2,
                    PageSize = 10,
                    Sorts = [
                        new PostSort("content", false),
                        new PostSort("commentCount", true)
                    ]
                },
                CancellationToken.None)
            .ConfigureAwait(false);

        Assert.AreEqual(2, result.Page);
        repository.Verify(item => item.SearchAsync(
                It.IsAny<Blogging.Domain.Specifications.ISpecification<Blogging.Domain.Entities.BlogPost>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Confirms that negative comment counts and oversized pages are rejected.
    /// </summary>
    [TestMethod]
    public void SearchAsyncRejectsNegativeAndOversizedOptions()
    {
        var service = new PostSearchService(new Mock<IBlogPostRepository>().Object);

        Assert.ThrowsExactly<ArgumentException>(() => service.SearchAsync(
            new PostFilter { MinCommentCount = -1 },
            CancellationToken.None));
        Assert.ThrowsExactly<ArgumentException>(() => service.SearchAsync(
            new PostFilter { PageSize = 101 },
            CancellationToken.None));
    }
}
