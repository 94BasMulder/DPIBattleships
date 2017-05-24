using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    public class Piece
    {
        public int id { get; set; }
        public string pieceType { get; set; }

        public Piece() { }
        public Piece(string pieceType) { this.pieceType = pieceType; }
    }
}
