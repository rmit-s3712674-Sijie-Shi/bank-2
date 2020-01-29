using Microsoft.EntityFrameworkCore;
using assignment2.Models;

namespace assignment2.Data
{
    public class Logincontext : DbContext
    {
        public Logincontext(DbContextOptions<Logincontext> options)
            : base(options)
        {
        }

        public DbSet<Login> LoginID { get; set; }
        public DbSet<Login> PasswordHashed { get; set; }
        public DbSet<Login> CustomerID { get; set; }
    }
}
