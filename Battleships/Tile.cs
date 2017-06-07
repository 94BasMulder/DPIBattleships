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
        public virtual Piece Piece {get;set;}
        public virtual User Owner { get; set; }
        public int x { get; set; }
        public int y { get; set; }

        public Tile() { }

        public Tile(User user,int x, int y) {
            this.Owner = user;
            this.x = x;
            this.y = y;
        }
    }
}
