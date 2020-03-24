using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OCL1_Proyecto1
{
    class Automoton : ICloneable
    {
        public State initialState;
        public State finalState;
        public int count;
        public string graphviz;
        public string id;
        public AFD AFD;
        public LinkedList<String> alphabet;

        public Automoton()
        {
            AFD = null;
            initialState = new State();
            finalState = new State();
            this.alphabet = new LinkedList<string>();
            count = 0;
            this.graphviz = "digraph D{\n";
            this.graphviz += "color= green;\ngraph[bgcolor = black];\nnode[style = dashed\ncolor = yellow fontcolor = white]edge[color = red fontcolor = white]\n";
            this.graphviz += "\tstyle=filled;\n\trankdir=LR";
        }

        public void setStatesId(State init)
        {
            if (init.id == "")
            {
                init.pos = count;
                init.id = "S" + count;
                count++;

            }
            if(!init.passed)
            {
                init.passed = true;
                foreach (Transition t in init.transitions)
                {
                    setStatesId(t.state);
                }
            }
        }

        public void printAutomaton(State init)
        {
            if (!init.printed)
            {
                init.printed = true;
                foreach (Transition t in init.transitions)
                {
                    graphviz += "\n\t" + init.getId() + " -> " + t.state.getId() + "[label = \""+t.getChar()+"\"];";
                    printAutomaton(t.state);
                }
                graphviz += "\n\t"+init.id + "[shape=circle];";
            }
        }

        public void print()
        {
            this.graphviz += "\n\tp -> " + initialState.getId() + ";";
            this.graphviz += "\n\tp[shape=point];";
            this.printAutomaton(initialState);
            this.graphviz += "\n\t" + finalState.getId() + "[shape = doublecircle];";
            graphviz += "\n}";
            int n = 949;
            string epsilon = Convert.ToChar(n).ToString();
        }

        public void generarDot()
        {
            String rutapng = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + this.id + ".png";
            String rutadot = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + this.id + ".dot";
            System.IO.File.WriteAllText(rutadot, this.graphviz);
            String commandoDot = "dot.exe -Tpng " + rutadot + " -o " + rutapng;
            var comando = string.Format(commandoDot);
            var procStart = new System.Diagnostics.ProcessStartInfo("cmd", "/c" + comando);
            procStart.UseShellExecute = false;
            procStart.CreateNoWindow = true;
            procStart.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            var proc = new System.Diagnostics.Process();
            proc.StartInfo = procStart;
            proc.Start();
            proc.WaitForExit();
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
