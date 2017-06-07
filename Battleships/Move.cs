using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    [Serializable]
    public class Move
    {
        public int x { get; set; }
        public int y { get; set; }


        public Move() { }

        public Move(int x, int y) {
            this.x = x;
            this.y = y;
        }


    }
}
