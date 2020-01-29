using Microsoft.EntityFrameworkCore;
using assignment2.Models;

namespace assignment2.Data
{
    public class Registercontext : DbContext
    {
        public Registercontext(DbContextOptions<Registercontext> options)
            : base(options)
        {
        }

        public DbSet<Register> LoginID { get; set; }
        public DbSet<Register> PasswordHashed { get; set; }
        public DbSet<Register> CustomerID { get; set; }
    }
}
