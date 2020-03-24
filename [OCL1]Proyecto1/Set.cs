using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OCL1_Proyecto1
{
    class Set
    {
        public string name;
        public LinkedList<char> characters; 

        public Set()
        {
            this.name = "";
            this.characters = new LinkedList<char>();
        }
    }
}
