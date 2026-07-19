namespace Blogging.Repository.Options;

/// <summary>
/// Controls the database connection and startup migration behavior.
/// </summary>
public sealed class BlogDatabaseOptions
{
    public const string SectionName = "BlogDatabase";

    /// <summary>
    /// Gets or sets the SQLite connection string.
    /// </summary>
    public string ConnectionString { get; set; } = "Data Source=blogging.db";

}
