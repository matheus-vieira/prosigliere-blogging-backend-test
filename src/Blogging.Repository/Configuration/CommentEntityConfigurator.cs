using Blogging.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blogging.Repository.Configuration;

internal sealed class CommentEntityConfigurator : IEntityTypeConfigurator
{
    public void Configure(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Comment>();

        entity.ToTable("comments");
        entity.HasKey(comment => comment.Id);
        entity.Property(comment => comment.Id).ValueGeneratedOnAdd();
        entity.Property(comment => comment.Content).IsRequired().HasMaxLength(2000);
        entity.HasIndex(comment => comment.PostId);
    }
}
