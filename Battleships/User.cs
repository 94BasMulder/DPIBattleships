using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    [Serializable]
    public class User
    {
        public User() { }

        public User(string userName, string password)
        {
            this.UserName = userName;
            this.Password = password;
        }

        public int Id { get; set; }
        
        public string UserName { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return UserName;
        }
    }
}
