using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using assignment2.Data;
using System;
using System.Linq;


namespace assignment2.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new Logincontext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<Logincontext>>()))
            {
                // Look for any movies.
                if (context.CustomerID.Any())
                {
                    return;   // DB has been seeded
                }

                context.LoginID.AddRange(
                    new Login
                    {
                        LoginID =" ",
                        PasswordHashed = "",
                        CustomerID = " ",
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
