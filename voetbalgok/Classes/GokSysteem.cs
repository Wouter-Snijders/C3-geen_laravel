using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using voetbalgok.Data;

namespace voetbalgok.Classes
{
    public class GokSysteem
    {
        private readonly AppDbContext _context;
        private readonly Random _random;

        public GokSysteem(AppDbContext context)
        {
            _context = context;
            _random = new Random();
        }

        public void StartGokspel()
        {
            // Kies willekeurig 2 teams
            var teams = _context.Teams.ToList();
            var team1 = teams[_random.Next(teams.Count)];
            var team2 = teams[_random.Next(teams.Count)];

            // Zorg ervoor dat de teams verschillend zijn
            while (team1 == team2)
            {
                team2 = teams[_random.Next(teams.Count)];
            }

            Console.WriteLine($"Kies het winnende team: \n1. {team1.Naam}\n2. {team2.Naam}");

            // Gebruiker kiest welk team wint
            string keuze = Console.ReadLine();
            int gekozenTeamId = keuze == "1" ? team1.Id : team2.Id;

            // Genereer willekeurig een winnaar
            int winnaarId = _random.Next(2) == 0 ? team1.Id : team2.Id;
            Console.WriteLine($"Het winnende team is: {(_context.Teams.First(t => t.Id == winnaarId)).Naam}");

            // Verkrijg de gebruiker en update hun punten
            var gebruiker = _context.Gebruikers.First();

            if (gekozenTeamId == winnaarId)
            {
                Console.WriteLine("Gefeliciteerd! Je hebt het juiste team gekozen. Je krijgt 1 punt.");
                gebruiker.Punten += 1;
            }
            else
            {
                Console.WriteLine("Helaas, je had het verkeerde team gekozen. Je verliest 1 punt.");
                gebruiker.Punten -= 1;
            }

            // Sla de wijzigingen op in de database
            _context.SaveChanges();
        }
    }

}
