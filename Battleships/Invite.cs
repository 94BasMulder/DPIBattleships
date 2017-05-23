using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    [Serializable]
    public class Invite
    {
        public Invite() { }

        public Invite(int size, string msg, User user)
        {
            this.Size = size;
            this.Message = msg;
            this.User = user;
        }

        public int Size { get; set; }
        public string Message { get; set; }
        public User User { get; set; }
    }
}
