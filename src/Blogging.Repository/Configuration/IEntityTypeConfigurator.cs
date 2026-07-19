using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blogging.Repository.Configuration;

/// <summary>
/// Applies one or more entity mappings to an EF Core model.
/// </summary>
public interface IEntityTypeConfigurator
{
}

/// <summary>
/// Marks an EF Core entity configuration for assembly scanning.
/// </summary>
/// <typeparam name="TEntity">The entity type being configured.</typeparam>
public interface IEntityTypeConfigurator<TEntity> :
    IEntityTypeConfigurator,
    IEntityTypeConfiguration<TEntity>
    where TEntity : class
{
}
