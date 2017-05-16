using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    public class Board
    {
        public int id { get; set; }
        public int size { get; set; }
        public List<Tile> tiles { get; set; }
    }
}
