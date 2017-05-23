using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    public class Tile
    {
        public int Id { get; set; }
        public bool IsHit { get; set; }
        public Piece Piece {get;set;}
        public User Owner { get; set; }

        public Tile() { }

        public Tile(User user) { this.Owner = user; }
    }
}
