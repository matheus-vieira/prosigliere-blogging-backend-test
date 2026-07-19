using System.Linq.Expressions;
using Blogging.Domain.Entities;
using Blogging.Domain.Specifications;

namespace Blogging.Domain.Posts.Specifications;

internal sealed class AllPostsSpecification : Specification<BlogPost>
{
    public override Expression<Func<BlogPost, bool>> Criteria => post => true;
}
