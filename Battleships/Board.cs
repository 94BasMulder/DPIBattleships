using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    public class Board
    {
        public Board() { }

        public Board(int size)
        {
            this.Size = size;
        }

        public void generateTiles(User user1, User user2)
        {
            battleshipContext context = new battleshipContext();
            Tile t = null;
            for(int i =0;i<Size * Size ;i++)
            {
                t = new Tile(user1);
                context.Tiles.Add(t);

                context.SaveChanges();
                t = new Tile(user2);
                context.Tiles.Add(t);

                context.SaveChanges();
            }
        }

        public int Id { get; set; }
        public int Size { get; set; }
        public List<Tile> Tiles { get; set; }
    }
}
