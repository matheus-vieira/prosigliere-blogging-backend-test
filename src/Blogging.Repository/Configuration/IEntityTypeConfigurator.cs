using Microsoft.EntityFrameworkCore;

namespace Blogging.Repository.Configuration;

/// <summary>
/// Applies one or more entity mappings to an EF Core model.
/// </summary>
public interface IEntityTypeConfigurator
{
    /// <summary>
    /// Configures entity mappings in the supplied model builder.
    /// </summary>
    /// <param name="modelBuilder">The EF Core model builder.</param>
    void Configure(ModelBuilder modelBuilder);
}
