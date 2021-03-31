using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrgHome.DataLayer.Models;

namespace PrgHome.DataLayer.Mapping
{
    public class CommentMap : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments").HasKey(n => n.Id);
            builder.Property(n => n.Date)
                .IsRequired();
            builder.Property(n => n.Name)
                .IsRequired()
                .HasMaxLength(125);
            builder.Property(n => n.Email)
                .IsRequired()
                .HasMaxLength(250);
            builder.Property(n => n.Text)
                .IsRequired()
                .HasMaxLength(500);
            builder.Property(n => n.ParentId)
                .IsRequired(false);
            builder.HasOne(n => n.ParentComment)
                .WithMany(n => n.ChildrenComments)
                .HasForeignKey(n => n.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(n => n.Article)
                .WithMany(n => n.Comments)
                .HasForeignKey(n => n.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
