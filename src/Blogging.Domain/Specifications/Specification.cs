using System.Linq.Expressions;

namespace Blogging.Domain.Specifications;

/// <summary>
/// Provides expression composition for entity specifications.
/// </summary>
/// <typeparam name="TEntity">The entity being filtered.</typeparam>
public abstract class Specification<TEntity> : ISpecification<TEntity>
    where TEntity : class
{
    public abstract Expression<Func<TEntity, bool>> Criteria { get; }

    public virtual IReadOnlyList<OrderClause<TEntity>> Orderings => [];

    public virtual int PageNumber => 1;

    public virtual int PageSize => int.MaxValue;
}
