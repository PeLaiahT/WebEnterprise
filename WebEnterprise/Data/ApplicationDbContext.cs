using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebEnterprise.Models;

namespace WebEnterprise.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Idea> Ideas { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}