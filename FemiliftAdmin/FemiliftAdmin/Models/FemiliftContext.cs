using System.Data.Entity;

namespace FemiliftAdmin.Models
{
    public class FemiliftContext : DbContext
    {
        public FemiliftContext() : base("name=FemiliftDb")
        {
        }

        public DbSet<Branch> Branches { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
    }
}
