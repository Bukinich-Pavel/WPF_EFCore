using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_EFCore.Model;

namespace WPF_EFCore
{
    class ApplicationContext : DbContext
    {
        public DbSet<Client> Ckients { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=wpf_efcore;Trusted_Connection=True;");
        }
    }
}
