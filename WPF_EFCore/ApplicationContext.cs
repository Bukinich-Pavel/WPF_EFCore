
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_EFCore.Model;

namespace WPF_EFCore
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<DeposBankAccount> DeposBankAccount { get; set; }
        public DbSet<DontDeposBankAccount> DontDeposBankAccount { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=WPF_EfCore1;Trusted_Connection=True;");
        }
    }
}
