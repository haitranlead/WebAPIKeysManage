using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using WebAPIKeysManage.Models;

namespace WebAPIKeysManage.Controllers
{
    public class AppDbContext: DbContext
    {
        public DbSet<LicenseKey> Licenses { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=licenses.db");
            }
        }
    }
}
