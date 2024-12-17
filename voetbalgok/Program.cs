using Microsoft.EntityFrameworkCore;
using voetbalgok.Classes;
using voetbalgok.Data;
using voetbalgok.Models;

namespace voetbalgok
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new AppDbContext())
            {
                // Voeg een nieuwe gebruiker toe (als er nog geen gebruiker is)
                if (!context.Gebruikers.Any())
                {
                    var gebruiker = new Gebruiker { Naam = "Jan" };
                    context.Gebruikers.Add(gebruiker);
                    context.SaveChanges();
                    Console.WriteLine($"Gebruiker {gebruiker.Naam} geregistreerd met {gebruiker.Punten} punten.");
                }

                // Haal de gebruiker op
                var huidigeGebruiker = context.Gebruikers.First();

                Console.WriteLine($"Welkom {huidigeGebruiker.Naam}! Je huidige punten: {huidigeGebruiker.Punten}");
                Console.WriteLine("Start het toernooi!\n");

                // Haal alle teams uit de database
                var teams = context.Teams.ToList();

                if (teams.Count < 2)
                {
                    Console.WriteLine("Niet genoeg teams in de database om een toernooi te starten.");
                    return;
                }

                // Maak toernooi schema
                var toernooiWedstrijden = MaakToernooiSchema(teams);

                // Toon alle wedstrijden voordat voorspellingen worden gedaan
                Console.WriteLine("--- Toernooi Wedstrijden ---");
                foreach (var (team1, team2) in toernooiWedstrijden)
                {
                    Console.WriteLine($"{team1.Naam} vs {team2.Naam}");
                }

                Console.WriteLine("\nNu kun je voorspellingen plaatsen voor elke wedstrijd.");

                var weddenschappen = new Dictionary<(Team, Team), Team>(); // Opslaan van voorspellingen
                var resultaten = new List<(Team Team1, Team Team2, Team GewonnenTeam, int Score1, int Score2)>();

                // Voorspellingen plaatsen
                foreach (var (team1, team2) in toernooiWedstrijden)
                {
                    Console.WriteLine($"\nWie wint deze wedstrijd?");
                    Console.WriteLine($"1: {team1.Naam}");
                    Console.WriteLine($"2: {team2.Naam}");

                    string keuze = Console.ReadLine();
                    Team voorspelling = keuze == "1" ? team1 : team2;
                    weddenschappen[(team1, team2)] = voorspelling;
                }

                // Simuleer de wedstrijden
                foreach (var (team1, team2) in toernooiWedstrijden)
                {
                    var (gewonnenTeam, score1, score2) = SimuleerWedstrijd(team1, team2);
                    resultaten.Add((team1, team2, gewonnenTeam, score1, score2));
                }

                // Toon resultaten en bereken punten
                Console.WriteLine("\n--- Toernooi Resultaten ---");
                foreach (var (team1, team2, gewonnenTeam, score1, score2) in resultaten)
                {
                    Console.WriteLine($"{team1.Naam} ({score1}) vs {team2.Naam} ({score2}) - Winnaar: {gewonnenTeam.Naam}");

                    if (weddenschappen[(team1, team2)] == gewonnenTeam)
                    {
                        huidigeGebruiker.Punten += 1;
                        Console.WriteLine($"Je voorspelde correct! +1 punt.");
                    }
                    else
                    {
                        huidigeGebruiker.Punten -= 1;
                        Console.WriteLine($"Je voorspelde verkeerd. -1 punt.");
                    }
                }

                // Sla de nieuwe punten op
                context.SaveChanges();

                // Toon eindpunten
                Console.WriteLine($"\nToernooi afgelopen! Je eindstand: {huidigeGebruiker.Punten} punten.");
                Console.WriteLine("Bedankt voor het spelen!");
            }
        }

        static List<(Team, Team)> MaakToernooiSchema(List<Team> teams)
        {
            Random random = new Random();
            var beschikbareTeams = new List<Team>(teams);
            var wedstrijden = new List<(Team, Team)>();

            while (beschikbareTeams.Count >= 2)
            {
                var team1 = beschikbareTeams[random.Next(beschikbareTeams.Count)];
                beschikbareTeams.Remove(team1);

                var team2 = beschikbareTeams[random.Next(beschikbareTeams.Count)];
                beschikbareTeams.Remove(team2);

                wedstrijden.Add((team1, team2));
            }

            return wedstrijden;
        }

        static (Team gewonnenTeam, int scoreTeam1, int scoreTeam2) SimuleerWedstrijd(Team team1, Team team2)
        {
            // Genereer willekeurige scores
            Random random = new Random();
            int score1 = random.Next(0, 6);
            int score2 = random.Next(0, 6);

            // Zorg dat er geen gelijkspel is
            while (score1 == score2)
            {
                score1 = random.Next(0, 6);
                score2 = random.Next(0, 6);
            }

            Team winnaar = score1 > score2 ? team1 : team2;
            return (winnaar, score1, score2);
        }
    }
}
