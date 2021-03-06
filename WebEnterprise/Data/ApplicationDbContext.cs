using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebEnterprise.Models;
using WebEnterprise.Models.DTO;

namespace WebEnterprise.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Idea> Ideas { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CustomUser> CustomUsers { get; set; }
        public DbSet<Documment> Documments { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Like> Likes { get; set; }  
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContext)
        {
            if(!dbContext.IsConfigured)
            {
                dbContext.UseSqlServer("Server=ADMIN\\SQLEXPRESS;Database=WebEnterprise;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            this.CreateAdmin(builder);
            this.SeedRoles(builder);
            this.SeedUserRoles(builder);
        }
        private void CreateAdmin(ModelBuilder builder)
        {
            var passwordHaser = new PasswordHasher<CustomUser>();
            var admin = new CustomUser()
            {
                Id = "1",
                UserName = "Admin",
                Email = "admin@gmail.com",
                NormalizedUserName = "admin",
                PasswordHash = passwordHaser.HashPassword(null, "Abc@12345"),
                LockoutEnabled = true,
                EmailConfirmed = true,
            };
            builder.Entity<CustomUser>().HasData(admin);
        }
        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Id = "1", Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "admin" },
                new IdentityRole() { Id = "2", Name = "Assurance", ConcurrencyStamp = "2", NormalizedName = "assurance" },
                new IdentityRole() { Id = "3", Name = "Coordinator", ConcurrencyStamp = "3", NormalizedName = "coordinator" },
                new IdentityRole() { Id = "4", Name = "Staff", ConcurrencyStamp = "4", NormalizedName = "staff" }
                );
        }
        private void SeedUserRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string> { RoleId = "1", UserId = "1", });
        }
    }
}