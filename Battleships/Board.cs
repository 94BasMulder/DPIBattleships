using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    public class Board
    {
        public Board()
        {
            this.Tiles = new List<Tile>();
        }

        public Board(int size)
        {
            this.Size = size;
            setTilesUser1 = 7;
            setTilesUser2 = 7;
            this.Tiles = new List<Tile>();
        }

        public void generateTiles(User u1, User u2)
        {
            battleshipContext context = new battleshipContext();

            //Create new users because of persistence.
            User user1, user2;
            user1 = context.Users.Where(r => r.Id == u1.Id).First();
            user2 = context.Users.Where(r => r.Id == u2.Id).First();

            Tile t = null;
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                {
                    t = new Tile(user1,i,j);
                    this.Tiles.Add(t);
                    context.Tiles.Add(t);

                    context.SaveChanges();
                    t = new Tile(user2,i,j);
                    this.Tiles.Add(t);
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
