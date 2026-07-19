using Blogging.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blogging.Repository.Configuration;

internal sealed class BlogPostEntityConfigurator : IEntityTypeConfigurator
{
    public void Configure(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<BlogPost>();

        entity.ToTable("blog_posts");
        entity.HasKey(post => post.Id);
        entity.Property(post => post.Id).ValueGeneratedOnAdd();
        entity.Property(post => post.Title).IsRequired().HasMaxLength(200);
        entity.Property(post => post.Content).IsRequired().HasMaxLength(10000);
        entity.HasMany(post => post.Comments)
            .WithOne(comment => comment.Post)
            .HasForeignKey(comment => comment.PostId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
