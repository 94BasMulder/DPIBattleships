using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    [Serializable]
    public class Response
    {
        public bool gotHit { get; set; }
        public bool Continue {get;set;}
        public Response(bool hit, bool Continue = true) { this.gotHit = hit;this.Continue = Continue; }

        public string displayMessage()
        {
            if (gotHit)
                return "U heeft iets geraakt!";
            else return "Gemist!";
        }

    }
}
