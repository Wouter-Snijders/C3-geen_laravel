using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using voetbalgok.Models;

namespace voetbalgok.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Team> Teams { get; set; }
        public DbSet<Wedstrijd> Wedstrijden { get; set; }
        public DbSet<Gebruiker> Gebruikers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;" +
                "port=3306;" +
                "user=root;" +
                "password=;" +
                "database=VoetbalGok",
                ServerVersion.Parse("8.0.30")
            );

        }
    }
}
