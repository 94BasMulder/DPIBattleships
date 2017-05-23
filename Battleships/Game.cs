using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{ 
    public class Game
    {
        public int Id { get; set; }
        public User Turn { get; set; }
        public User User1 { get; set; }
        public User User2 { get; set; }
        public Board Board { get; set; }

        public Game() { }

        public Game(User user1, User user2, Board board)
        {
            battleshipContext context = new battleshipContext();
            this.User1 = user1;
            this.User2 = user2;
            this.Board = board;
            this.Turn = getStarter();
        }

        private User getStarter()
        {
            Random r = new Random();
            if(r.Next(0,2) == 1)
            {
                return User1;
            }
            return User2;
        }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
