using CourseManagerAPI.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Identity.Client;
using System.Data;
using System.Net.Http.Headers;

namespace CourseManagerAPI.Context
{
    public class courseDbContext : IdentityDbContext<UserEntity>
    {
        public courseDbContext(DbContextOptions<courseDbContext> options) : base(options) { }
        public DbSet<CourseEntity> Courses { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<TeachingRequest> TeachingRequests { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //1
            modelBuilder.Entity<UserEntity>(e =>
            {
                e.ToTable("Users");
            }
          );
            //2
            modelBuilder.Entity<IdentityUserClaim<string>>(e =>
            {
                e.ToTable("UserClaims");
            }
            );
            //3
            modelBuilder.Entity<IdentityUserLogin<string>>(e =>
            {
                e.ToTable("UserLogins");
            }
            );
            //4
            modelBuilder.Entity<IdentityUserToken<string>>(e =>
            {
                e.ToTable("UserToken");
            }
           );
            //5
            modelBuilder.Entity<IdentityRole>(e =>
            {
                e.ToTable("Roles");
            }
           );
            //6
            modelBuilder.Entity<IdentityRoleClaim<string>>(e =>
            {
                e.ToTable("RoleClaims");
            }
           );
            //7
            modelBuilder.Entity<IdentityUserRole<string>>(e =>
            {
                e.ToTable("UserRoles");
            }
           );
            base.OnModelCreating(modelBuilder);
        }
    }
}