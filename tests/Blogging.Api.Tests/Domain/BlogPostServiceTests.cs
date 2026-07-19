using Blogging.Domain.Posts;
using Moq;

namespace Blogging.Api.Tests;

/// <summary>
/// Verifies the Domain use cases for blog posts.
/// </summary>
[TestClass]
public sealed class BlogPostServiceTests
{
    /// <summary>
    /// Confirms that listing delegates to the post repository.
    /// </summary>
    [TestMethod]
    public async Task ListAsyncReturnsRepositoryResultsAsync()
    {
        var expected = new[] { new BlogPostSummary(1, "A title", 2) };
        var repository = new Mock<IBlogPostRepository>();
        repository.Setup(item => item.ListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        var service = new BlogPostService(repository.Object);

        var result = await service.ListAsync(CancellationToken.None).ConfigureAwait(false);

        CollectionAssert.AreEqual(expected, result.ToArray());
    }

    /// <summary>
    /// Confirms that valid post values are delegated to persistence.
    /// </summary>
    [TestMethod]
    public async Task CreateAsyncDelegatesValidValuesAsync()
    {
        var command = new CreateBlogPostCommand { Title = "A title", Content = "Content" };
        var expected = new BlogPostSummary(1, command.Title, 0);
        var repository = new Mock<IBlogPostRepository>();
        repository.Setup(item => item.CreateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        var service = new BlogPostService(repository.Object);

        var result = await service.CreateAsync(command, CancellationToken.None)
            .ConfigureAwait(false);

        Assert.AreEqual(expected, result);
        repository.Verify(item => item.CreateAsync(command, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Confirms that whitespace-only values are rejected before persistence.
    /// </summary>
    [TestMethod]
    public void CreateAsyncRejectsWhitespaceValues()
    {
        var repository = new Mock<IBlogPostRepository>();
        var service = new BlogPostService(repository.Object);
        var command = new CreateBlogPostCommand { Title = " ", Content = "Content" };

        Assert.ThrowsExactly<ArgumentException>(
            () => service.CreateAsync(command, CancellationToken.None));
        repository.VerifyNoOtherCalls();
    }
}
