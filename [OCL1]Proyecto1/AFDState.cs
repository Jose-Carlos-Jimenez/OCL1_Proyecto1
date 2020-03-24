using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OCL1_Proyecto1
{
    class AFDState
    {
        public List<AFDTransition> transitions;
        public HashSet<State> set;
    }
}
