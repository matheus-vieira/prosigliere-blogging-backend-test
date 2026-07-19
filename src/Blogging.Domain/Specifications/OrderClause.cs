using System.Linq.Expressions;

namespace Blogging.Domain.Specifications;

/// <summary>
/// Describes one ordered field in a composed specification.
/// </summary>
/// <typeparam name="TEntity">The entity being ordered.</typeparam>
public sealed record OrderClause<TEntity>(
    Expression<Func<TEntity, object>> KeySelector,
    bool Descending)
    where TEntity : class;
