using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OCL1_Proyecto1
{
    class State
    {
        public LinkedList<Transition> transitions;
        public String id;
        public bool passed;
        public bool printed;
        public int pos;

        public State()
        {
            this.transitions = new LinkedList<Transition>();
            this.id = "";
            this.passed = false;
            this.printed = false;
        }

        public void AddTransition(Transition edge)
        {
            this.transitions.AddLast(edge);
        }
 
        public string getId()
        {
            string n = "";
            foreach(char c in this.id)
            {
                if(c != '\"')
                {
                    n += c;
                }
            }
            return n;
        }
    }
}
