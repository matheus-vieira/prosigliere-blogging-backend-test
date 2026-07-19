using System.Linq.Expressions;
using Blogging.Domain.Entities;
using Blogging.Domain.Specifications;

namespace Blogging.Domain.Posts.Specifications;

internal sealed class MinimumCommentCountSpecification(int value) : Specification<BlogPost>
{
    public override Expression<Func<BlogPost, bool>> Criteria =>
        post => post.Comments.Count >= value;
}
