using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    public class Game
    {
        public int id { get; set; }
        public User turn { get; set; }
        public List<User> usersInGame {get;set;}
        public Board board { get; set; }
        public string name { get; set; }

    }
}
