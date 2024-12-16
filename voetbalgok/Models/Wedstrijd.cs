using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace voetbalgok.Models
{
    public class Wedstrijd
    {
        public int Id { get; set; }
        public int Team1Id { get; set; }
        public int Team2Id { get; set; }
        public int WinnaarId { get; set; }
        public Team Team1 { get; set; }
        public Team Team2 { get; set; }
        public Team Winnaar { get; set; }
    }
}
