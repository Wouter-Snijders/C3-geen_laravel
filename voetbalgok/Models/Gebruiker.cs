using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace voetbalgok.Models
{
    public class Gebruiker
    {
        public int Id { get; set; }

        public string Naam { get; set; }
        public int Punten { get; set; } = 50;
    }
}
