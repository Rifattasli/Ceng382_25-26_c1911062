using Microsoft.EntityFrameworkCore;
using LabProject.Models;

namespace LabProject.Data
{
    
    public class AppDbContext : DbContext
    {
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<ClassInformationModel> Classes { get; set; }
    }
}