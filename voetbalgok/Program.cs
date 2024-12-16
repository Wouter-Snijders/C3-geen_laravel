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

                bool doorgaan = true;

                while (doorgaan)
                {
                    // Toon de gebruiker en zijn punten
                    Console.WriteLine($"\nHuidige gebruiker: {huidigeGebruiker.Naam} - Punten: {huidigeGebruiker.Punten}");

                    // Haal willekeurige teams op uit de database
                    var teams = context.Teams.ToList();
                    if (teams.Count < 2)
                    {
                        Console.WriteLine("Niet genoeg teams in de database om een wedstrijd te starten.");
                        break;
                    }

                    // Kies twee willekeurige teams
                    Random random = new Random();
                    var team1 = teams[random.Next(teams.Count)];
                    var team2 = teams[random.Next(teams.Count)];

                    // Zorg ervoor dat het niet hetzelfde team is
                    while (team1 == team2)
                    {
                        team2 = teams[random.Next(teams.Count)];
                    }

                    // Toon de teams en vraag de gebruiker om een keuze
                    Console.WriteLine($"Kies een team om te winnen:");
                    Console.WriteLine($"1: {team1.Naam}");
                    Console.WriteLine($"2: {team2.Naam}");

                    string keuze = Console.ReadLine();

                    // Simuleer de wedstrijd met scores en bepaal een winnaar
                    var (gewonnenTeam, scoreTeam1, scoreTeam2) = SimuleerWedstrijd(team1, team2);

                    // Toon de scores
                    Console.WriteLine($"\n{team1.Naam} ({scoreTeam1}) vs {team2.Naam} ({scoreTeam2})");

                    if (scoreTeam1 == scoreTeam2)
                    {
                        Console.WriteLine($"De wedstrijd eindigt in een gelijkspel!");
                        Console.WriteLine("Geen punten toegevoegd of afgetrokken.");
                    }
                    else
                    {
                        Console.WriteLine($"Het winnende team was {gewonnenTeam.Naam}!");

                        // Update de punten van de gebruiker
                        if ((keuze == "1" && gewonnenTeam == team1) || (keuze == "2" && gewonnenTeam == team2))
                        {
                            huidigeGebruiker.Punten += 1;
                            Console.WriteLine("Je hebt gewonnen! 1 punt erbij.");
                        }
                        else
                        {
                            huidigeGebruiker.Punten -= 1;
                            Console.WriteLine("Je hebt verloren. 1 punt eraf.");
                        }

                        // Sla de wijziging op in de database
                        context.SaveChanges();

                        // Toon de nieuwe punten
                        Console.WriteLine($"Nieuwe punten: {huidigeGebruiker.Punten}");
                    }

                    // Vraag of de gebruiker door wil spelen
                    Console.WriteLine("\nWil je doorgaan? (j/n)");
                    string doorgaanKeuze = Console.ReadLine();
                    if (doorgaanKeuze?.ToLower() != "j")
                    {
                        doorgaan = false;
                        Console.WriteLine("Bedankt voor het spelen!");
                    }
                }
            }
        }

        static (Team gewonnenTeam, int scoreTeam1, int scoreTeam2) SimuleerWedstrijd(Team team1, Team team2)
        {
            // Genereer willekeurige scores
            Random random = new Random();
            int scoreTeam1 = random.Next(0, 6); // Scores van 0 tot 5
            int scoreTeam2 = random.Next(0, 6);

            // Bepaal de winnaar of een gelijkspel
            if (scoreTeam1 > scoreTeam2)
            {
                return (team1, scoreTeam1, scoreTeam2);
            }
            else if (scoreTeam2 > scoreTeam1)
            {
                return (team2, scoreTeam1, scoreTeam2);
            }
            else
            {
                return (null, scoreTeam1, scoreTeam2); // Gelijkspel
            }
        }
    }
}
