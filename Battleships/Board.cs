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
            setTilesUser1 = 7;
            setTilesUser2 = 7;
        }

        public void generateTiles(User user1, User user2)
        {
            battleshipContext context = new battleshipContext();
            Tile t = null;
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                {
                    t = new Tile(user1,i,j);
                    context.Tiles.Add(t);

                    context.SaveChanges();
                    t = new Tile(user2,i,j);
                    context.Tiles.Add(t);

                    context.SaveChanges();
               }
        }

        public int Id { get; set; }
        public int setTilesUser1 { get; set; }
        public int setTilesUser2 { get; set; }
        public int Size { get; set; }
        public List<Tile> Tiles { get; set; }
    }
}
