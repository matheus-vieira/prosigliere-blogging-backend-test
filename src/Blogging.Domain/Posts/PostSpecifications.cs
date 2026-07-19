using Blogging.Domain.Entities;
using Blogging.Domain.Posts.Specifications;
using Blogging.Domain.Specifications;

namespace Blogging.Domain.Posts;

/// <summary>
/// Creates composable specifications for post queries.
/// </summary>
public static class PostSpecifications
{
    /// <summary>
    /// Creates a specification from all supplied post filters.
    /// </summary>
    /// <param name="filter">The optional post filter.</param>
    /// <returns>A composed post specification.</returns>
    public static Specification<BlogPost> FromFilter(PostFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        Specification<BlogPost> specification = new AllPostsSpecification();

        if (filter.Id is not null)
        {
            specification = specification.And(new PostIdSpecification(filter.Id.Value));
        }

        if (!string.IsNullOrWhiteSpace(filter.Title))
        {
            specification = specification.And(
                new PostTitleSpecification(filter.Title.Trim()));
        }

        if (!string.IsNullOrWhiteSpace(filter.Content))
        {
            specification = specification.And(
                new PostContentSpecification(filter.Content.Trim()));
        }

        if (filter.HasComments is not null)
        {
            specification = specification.And(
                new HasCommentsSpecification(filter.HasComments.Value));
        }

        if (filter.MinCommentCount is not null)
        {
            specification = specification.And(
                new MinimumCommentCountSpecification(filter.MinCommentCount.Value));
        }

        if (filter.MaxCommentCount is not null)
        {
            specification = specification.And(
                new MaximumCommentCountSpecification(filter.MaxCommentCount.Value));
        }

        var orderings = filter.Sorts.Select(CreateOrderClause).ToList();
        if (orderings.Count == 0)
        {
            orderings.Add(new OrderClause<BlogPost>(post => (object)post.Id, false));
        }

        return specification.WithQueryOptions(orderings, filter.Page, filter.PageSize);
    }

    private static OrderClause<BlogPost> CreateOrderClause(PostSort sort)
    {
        var field = sort.Field.Trim().ToLowerInvariant();

        return field switch
        {
            "id" => new OrderClause<BlogPost>(post => (object)post.Id, sort.Descending),
            "title" => new OrderClause<BlogPost>(post => (object)post.Title, sort.Descending),
            "content" => new OrderClause<BlogPost>(post => (object)post.Content, sort.Descending),
            "commentcount" => new OrderClause<BlogPost>(
                post => (object)post.Comments.Count,
                sort.Descending),
            _ => throw new ArgumentException($"Unknown sort field: {sort.Field}.", nameof(sort))
        };
    }
}
