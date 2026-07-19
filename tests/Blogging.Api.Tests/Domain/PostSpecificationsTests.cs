using Blogging.Domain.Posts;
using Blogging.Domain.Specifications;

namespace Blogging.Api.Tests;

/// <summary>
/// Verifies specification composition metadata for post filters.
/// </summary>
[TestClass]
[TestCategory("Domain")]
public sealed class PostSpecificationsTests
{
    /// <summary>
    /// Confirms that filters produce paging and multiple ordering clauses.
    /// </summary>
    [TestMethod]
    public void FromFilterBuildsPagingAndOrderingMetadata()
    {
        var specification = PostSpecifications.FromFilter(new PostFilter
        {
            Page = 2,
            PageSize = 10,
            Sorts =
            [
                new PostSort("title", false),
                new PostSort("commentCount", true)
            ]
        });

        Assert.AreEqual(2, specification.PageNumber);
        Assert.AreEqual(10, specification.PageSize);
        Assert.HasCount(2, specification.Orderings);
    }

    /// <summary>
    /// Confirms that an empty filter uses stable identifier ordering.
    /// </summary>
    [TestMethod]
    public void FromFilterUsesIdentifierOrderingByDefault()
    {
        var specification = PostSpecifications.FromFilter(new PostFilter());

        Assert.HasCount(1, specification.Orderings);
        Assert.IsFalse(specification.Orderings[0].Descending);
    }
}
