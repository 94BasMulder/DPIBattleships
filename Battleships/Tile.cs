using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    public class Tile
    {
        public int id { get; set; }
        public bool isHit { get; set; }
        public Piece piece {get;set;}
    }
}
