using Blogging.Domain.Entities;
using Blogging.Repository.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Blogging.Api.Tests;

/// <summary>
/// Verifies EF Core mappings, migrations, and SQLite relationship behavior.
/// </summary>
[TestClass]
[TestCategory("Repository")]
public sealed class PersistenceTests
{
    /// <summary>
    /// Confirms that the configured model exposes the expected relational schema.
    /// </summary>
    [TestMethod]
    public void ModelUsesExpectedTablesAndConstraints()
    {
        using var connection = new SqliteConnection("Data Source=:memory:");
        var options = new DbContextOptionsBuilder<BlogDbContext>()
            .UseSqlite(connection)
            .Options;
        using var context = new BlogDbContext(options);

        var postEntity = context.Model.FindEntityType(typeof(BlogPost));
        var commentEntity = context.Model.FindEntityType(typeof(Comment));

        Assert.IsNotNull(postEntity);
        Assert.IsNotNull(commentEntity);
        Assert.AreEqual("blog_posts", postEntity.GetTableName());
        Assert.AreEqual("comments", commentEntity.GetTableName());
        Assert.AreEqual(200, postEntity.FindProperty(nameof(BlogPost.Title))!.GetMaxLength());
        Assert.AreEqual(2000, commentEntity.FindProperty(nameof(Comment.Content))!.GetMaxLength());
        Assert.AreEqual(DeleteBehavior.Cascade, commentEntity.GetForeignKeys().Single().DeleteBehavior);
    }

    /// <summary>
    /// Confirms that a post and its comment persist through the SQLite provider.
    /// </summary>
    [TestMethod]
    public async Task SavingPostAndCommentPersistsTheirRelationshipAsync()
    {
        using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync().ConfigureAwait(false);
        var options = new DbContextOptionsBuilder<BlogDbContext>()
            .UseSqlite(connection)
            .Options;
        using var context = new BlogDbContext(options);

        await context.Database.MigrateAsync().ConfigureAwait(false);
        var post = new BlogPost("A title", "Post content");
        context.BlogPosts.Add(post);
        await context.SaveChangesAsync().ConfigureAwait(false);

        context.Comments.Add(new Comment(post.Id, "A comment"));
        await context.SaveChangesAsync().ConfigureAwait(false);

        var persistedPost = await context.BlogPosts
            .Include(item => item.Comments)
            .SingleAsync()
            .ConfigureAwait(false);

        Assert.AreEqual("A title", persistedPost.Title);
        Assert.AreEqual(1, persistedPost.Comments.Count);
        Assert.AreEqual("A comment", persistedPost.Comments.Single().Content);
    }

    /// <summary>
    /// Confirms that the foreign key prevents orphan comments.
    /// </summary>
    [TestMethod]
    public async Task SavingCommentForUnknownPostFailsAsync()
    {
        using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync().ConfigureAwait(false);
        var options = new DbContextOptionsBuilder<BlogDbContext>()
            .UseSqlite(connection)
            .Options;
        using var context = new BlogDbContext(options);

        await context.Database.MigrateAsync().ConfigureAwait(false);
        context.Comments.Add(new Comment(999, "Orphan comment"));

        await Assert.ThrowsExactlyAsync<DbUpdateException>(
            () => context.SaveChangesAsync()).ConfigureAwait(false);
    }
}
