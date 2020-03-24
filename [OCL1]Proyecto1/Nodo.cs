using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OCL1_Proyecto1
{
    class Nodo: ICloneable
    {
        public String data;
        public Nodo left;
        public Nodo right;

        public Nodo(String data)
        {
            this.data = data;
            left = null;
            right = null;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
