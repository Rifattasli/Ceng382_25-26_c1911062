using Microsoft.EntityFrameworkCore;
using LabProject.Models;

namespace LabProject.Data
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }

        public DbSet<Class> Classes { get; set; }
    }
}