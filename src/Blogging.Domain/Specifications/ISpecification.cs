using System.Linq.Expressions;

namespace Blogging.Domain.Specifications;

/// <summary>
/// Defines a composable query criterion for an entity.
/// </summary>
/// <typeparam name="TEntity">The entity being filtered.</typeparam>
public interface ISpecification<TEntity>
    where TEntity : class
{
    /// <summary>
    /// Gets the expression executed by the persistence provider.
    /// </summary>
    Expression<Func<TEntity, bool>> Criteria { get; }

    IReadOnlyList<OrderClause<TEntity>> Orderings { get; }

    int PageNumber { get; }

    int PageSize { get; }
}
