using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrgHome.DataLayer.IdentityClasses;
using PrgHome.DataLayer.Mapping;
using PrgHome.DataLayer.Models;

namespace PrgHome.DataLayer
{
    public class PrgHomeContext
        : IdentityDbContext
        <
            AppUser,
            AppRole, string,
            IdentityUserClaim<string>,
            AppUserRole,
            IdentityUserLogin<string>,
            IdentityRoleClaim<string>,
            IdentityUserToken<string>
         >
    {
        public PrgHomeContext(DbContextOptions<PrgHomeContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.UseSqlServer(@"Server=.;Database=PrgHomeDB;trusted_Connection=True");
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new ArticleMap());
            builder.ApplyConfiguration(new CommentMap());
            builder.Entity<Category>()
                .Property(n => n.Title)
                .IsRequired()
                .HasMaxLength(125);
            builder.Entity<File>()
                .ToTable("Files")
                .HasKey(n => n.Name);
            builder.Entity<File>()
                .Property(n => n.Name)
                .IsUnicode(false)
                .HasMaxLength(300);
            builder.Entity<File>()
                .Property(n => n.Size)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();
            builder.Entity<File>()
                .Property(n => n.Type)
                .HasMaxLength(50)
                .IsRequired()
                .IsUnicode(false);
            #region Identity Tables Mapping

            builder.Entity<AppUser>().ToTable("Users");
            builder.Entity<AppRole>().ToTable("Roles");
            builder.Entity<AppUserRole>().ToTable("UsersRole");
            builder.Entity<AppUser>()
                .Property(b => b.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
            builder.Entity<AppUserRole>()
                .HasOne(n => n.Role)
                .WithMany(n => n.UserRoles)
                .HasForeignKey(n => n.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<AppUserRole>()
                .HasOne(n => n.User)
                .WithMany(n => n.UserRoles)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion
        }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<File> Files { get; set; }

    }
}
