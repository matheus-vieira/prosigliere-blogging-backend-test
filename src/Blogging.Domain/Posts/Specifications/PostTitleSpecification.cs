using System.Linq.Expressions;
using Blogging.Domain.Entities;
using Blogging.Domain.Specifications;

namespace Blogging.Domain.Posts.Specifications;

internal sealed class PostTitleSpecification(string value) : Specification<BlogPost>
{
    // ToLower is translated to SQL LOWER by the relational provider.
#pragma warning disable CA1304, CA1311, CA1862
    public override Expression<Func<BlogPost, bool>> Criteria =>
        post => post.Title.ToLower().Contains(value.ToLower());
#pragma warning restore CA1304, CA1311, CA1862
}
