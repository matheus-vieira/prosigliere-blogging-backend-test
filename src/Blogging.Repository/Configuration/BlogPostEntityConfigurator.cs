using Blogging.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blogging.Repository.Configuration;

internal sealed class BlogPostEntityConfigurator : IEntityTypeConfigurator<BlogPost>
{
    public void Configure(EntityTypeBuilder<BlogPost> entity)
    {
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
