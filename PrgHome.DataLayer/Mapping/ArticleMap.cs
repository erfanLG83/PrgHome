using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrgHome.DataLayer.Models;

namespace PrgHome.DataLayer.Mapping
{
    public class ArticleMap : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.ToTable("Articles")
                .HasKey(n => n.Id);
            builder.Property(n => n.Description)
                .HasMaxLength(350);
            builder.Property(n => n.CategoryId)
                .IsRequired(false);
            builder.Property(n => n.Image)
                .IsRequired(false);
            builder.Property(n => n.IsPublish)
                .HasDefaultValue(false);
            builder.Property(n => n.PublishDate)
                .IsRequired(false);
            builder.Property(n => n.Tags)
                .IsRequired(false);
            builder.Property(n => n.TimeToRead)
                .IsRequired(false);
            builder.Property(n => n.Title)
                .IsRequired()
                .HasMaxLength(250);
            builder.Property(n => n.View)
                .HasDefaultValue(0);
            builder.Property(n => n.Content)
                .IsRequired(false);

            //relations mapping
            builder.HasOne(n => n.Category)
                .WithMany(n => n.Articles)
                .HasForeignKey(n => n.CategoryId);

        }
    }
}
