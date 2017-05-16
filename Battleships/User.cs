using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    public class User
    {
        public User(string userName, string password)
        {
            this.userName = userName;
            this.password = password;
        }

        public int id { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
    }
}
