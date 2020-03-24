using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OCL1_Proyecto1
{
    class Transition
    {
        public State state;
        public String character;

        public string getChar()
        {
            string n = "";
            foreach(char c in character)
            {
                if(c != '\"' && c!='{' && c!='}' )
                {
                    n += c;
                }
            }
            return n;
        }
    }
}
