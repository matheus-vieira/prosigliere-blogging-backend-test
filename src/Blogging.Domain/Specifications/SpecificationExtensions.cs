using System.Linq.Expressions;

namespace Blogging.Domain.Specifications;

/// <summary>
/// Provides reusable operations for composing specifications.
/// </summary>
public static class SpecificationExtensions
{
    /// <summary>
    /// Combines two specifications using logical AND.
    /// </summary>
    /// <typeparam name="TEntity">The entity being filtered.</typeparam>
    /// <param name="left">The first specification.</param>
    /// <param name="right">The second specification.</param>
    /// <returns>A composed specification.</returns>
    public static Specification<TEntity> And<TEntity>(
        this Specification<TEntity> left,
        Specification<TEntity> right)
        where TEntity : class
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return new ComposedSpecification<TEntity>(
            ExpressionComposer.And(left.Criteria, right.Criteria));
    }

    /// <summary>
    /// Adds ordering and pagination metadata to a specification.
    /// </summary>
    /// <typeparam name="TEntity">The entity being queried.</typeparam>
    /// <param name="specification">The base specification.</param>
    /// <param name="orderings">The ordered fields.</param>
    /// <param name="pageNumber">The one-based page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A configured specification.</returns>
    public static Specification<TEntity> WithQueryOptions<TEntity>(
        this Specification<TEntity> specification,
        IReadOnlyList<OrderClause<TEntity>> orderings,
        int pageNumber,
        int pageSize)
        where TEntity : class
    {
        ArgumentNullException.ThrowIfNull(specification);
        return new ConfiguredSpecification<TEntity>(
            specification.Criteria,
            orderings,
            pageNumber,
            pageSize);
    }

    private sealed class ComposedSpecification<TEntity>(
        Expression<Func<TEntity, bool>> criteria) : Specification<TEntity>
        where TEntity : class
    {
        public override Expression<Func<TEntity, bool>> Criteria => criteria;
    }

    private sealed class ConfiguredSpecification<TEntity>(
        Expression<Func<TEntity, bool>> criteria,
        IReadOnlyList<OrderClause<TEntity>> orderings,
        int pageNumber,
        int pageSize) : Specification<TEntity>
        where TEntity : class
    {
        public override Expression<Func<TEntity, bool>> Criteria => criteria;

        public override IReadOnlyList<OrderClause<TEntity>> Orderings => orderings;

        public override int PageNumber => pageNumber;

        public override int PageSize => pageSize;
    }
}

internal static class ExpressionComposer
{
    public static Expression<Func<TEntity, bool>> And<TEntity>(
        Expression<Func<TEntity, bool>> left,
        Expression<Func<TEntity, bool>> right)
    {
        var parameter = Expression.Parameter(typeof(TEntity), "entity");
        var leftBody = new ParameterReplacer(left.Parameters[0], parameter)
            .Visit(left.Body)!;
        var rightBody = new ParameterReplacer(right.Parameters[0], parameter)
            .Visit(right.Body)!;

        return Expression.Lambda<Func<TEntity, bool>>(
            Expression.AndAlso(leftBody, rightBody),
            parameter);
    }

    private sealed class ParameterReplacer(
        ParameterExpression source,
        ParameterExpression target) : ExpressionVisitor
    {
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == source ? target : base.VisitParameter(node);
        }
    }
}
