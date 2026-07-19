using Blogging.Domain.Entities;
using Blogging.Domain.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blogging.Repository.Configuration;

internal sealed class CommentEntityConfigurator : IEntityTypeConfigurator<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> entity)
    {
        entity.ToTable("comments");
        entity.HasKey(comment => comment.Id);
        entity.Property(comment => comment.Id).ValueGeneratedOnAdd();
        entity.Property(comment => comment.Content)
            .IsRequired()
            .HasMaxLength(PostValidationRules.CommentContentMaxLength);
        entity.HasIndex(comment => comment.PostId);
    }
}
