using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace API.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int, 
        IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, 
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<UserLike> Likes{get;set;}
        public DbSet<Message> Messages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<Photo> Photos{get;set;}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<AppUser>()
                    .HasMany(ur=>ur.UserRoles)
                    .WithOne(u=>u.User)
                    .HasForeignKey(ur=>ur.UserId)
                    .IsRequired();

                builder.Entity<AppRole>()
                    .HasMany(ur=>ur.UserRoles)
                    .WithOne(u=>u.Role)
                    .HasForeignKey(ur=>ur.RoleId)
                    .IsRequired();

            builder.Entity<UserLike>()
                    .HasKey(k => new {k.SourceUserId, k.LikedUserId});
            builder.Entity<UserLike>()
                    .HasOne(s => s.SourceUser)
                    .WithMany(l => l.LikedUsers)
                    .HasForeignKey(s => s.SourceUserId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserLike>()
                    .HasOne(s => s.LikedUser)
                    .WithMany(l => l.LikedByUsers)
                    .HasForeignKey(s => s.LikedUserId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>()
                    .HasOne(u => u.Recipient)
                    .WithMany(m => m.MessagesReceived)
                    .OnDelete(DeleteBehavior.Restrict);

             builder.Entity<Message>()
                    .HasOne(u => u.Sender)
                    .WithMany(m => m.MessagesSent)
                    .OnDelete(DeleteBehavior.Restrict);  
                      
            builder.Entity<Photo>().HasQueryFilter(p => p.IsApproved);
            builder.ApplyUtcDateTimeConverter();                             
        }
    }

    public static class UtcDateAnnotation
    {
        private const string IsUtcAnnotation = "IsUtc";

        public static PropertyBuilder<TProperty> IsUtc<TProperty>(this PropertyBuilder<TProperty> builder, bool isUtc = true)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.HasAnnotation(IsUtcAnnotation, isUtc);
        }

        public static bool IsUtc(this IMutableProperty property)
        {
            if (property is null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var attribute = property.PropertyInfo?.GetCustomAttribute<IsUtcAttribute>();
            if (attribute is not null && attribute.IsUtc)
            {
                return true;
            }

            return ((bool?)property.FindAnnotation(IsUtcAnnotation)?.Value) ?? true;
        }

        /// <summary>
        /// Make sure this is called after configuring all your entities.
        /// </summary>
        public static void ApplyUtcDateTimeConverter(this ModelBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (!property.IsUtc())
                    {
                        continue;
                    }

                    if (property.ClrType == typeof(DateTime) ||
                        property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(typeof(UtcValueConverter));
                    }
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class IsUtcAttribute : Attribute
    {
        public IsUtcAttribute(bool isUtc = true) => this.IsUtc = isUtc;
        public bool IsUtc { get; }
    }

     public class UtcValueConverter : ValueConverter<DateTime, DateTime>
    {
        public UtcValueConverter()
            : base(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
        {
        }
    }
}