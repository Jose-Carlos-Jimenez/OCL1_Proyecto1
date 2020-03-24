using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace _OCL1_Proyecto1
{
    class AFD
    {
        public static int MAX_STATES = 512;
        public static int START = 1;
        public static int TRAP = 0;

        int[,] transTable;//Tabla de transiciones.
        List<HashSet<State>> DEstados;//Estados del AFD, que son conjuntos de estados del AFN.
        List<int> posicionesAceptadas;//Posiciones en la que estan los estados de aceptación.
        LinkedList<string> alfabeto;
        public LinkedList<string> cadenas;
        public Hashtable sets;
        String id;

        public AFD(Automoton NFA)
        {
            this.DEstados = new List<HashSet<State>>();
            this.posicionesAceptadas = new List<int>();
            convertirAFN(NFA);
        }

        private HashSet<State> E_cerradura(State s)//ɛ-closure(S)
        {
            HashSet<State> eClosure = new HashSet<State>();//Declaro el conjunto que deseo devolver.

            Stack<State> stack = new Stack<State>();//Declaro una pila auxiliar para el método.

            stack.Push(s);//Lo añado para inicializar el método
            eClosure.Add(s);
            while (stack.Any())
            {
                State tempState = stack.Pop();

                foreach (Transition tran in tempState.transitions)
                {
                    if (tran.character == "ɛ" && !eClosure.Contains(tran.state))
                    {
                        eClosure.Add(tran.state);
                        stack.Push(tran.state);
                    }
                }
            }
            return eClosure;
        }

        private HashSet<State> E_cerradura(HashSet<State> s)
        {
            /*Inicializar variables pila y conjunto a devolver*/
            Stack<State> pila = new Stack<State>();
            HashSet<State> ECerradura = new HashSet<State>();
            /*Fin de inicialización.*/

            /*Inicializar ecerradura de T con T, además meter todos los estados en la pila*/
            foreach (State estado in s)
            {
                pila.Push(estado);
                ECerradura.Add(estado);
            }

            while (pila.Any())
            {
                State t = pila.Pop();
                foreach (Transition tran in t.transitions)
                {
                    if (tran.character == "ɛ")
                    {
                        State st = tran.state;
                        if (!contenidoEn(ECerradura, st))
                        {
                            ECerradura.Add(st);
                            pila.Push(st);
                        }
                    }
                }
            }
            return ECerradura;
        }

        /*Método para verificar si el estado está contenido en una cerradura que se está verificando*/
        private bool contenidoEn(HashSet<State> ECerradura, State st)
        {
            foreach (State es in ECerradura)
            {
                if (st.Equals(es))
                {
                    return true;
                }
            }
            return false;
        }

        //Método para verificar si un estado debe ser de aceptación.
        private bool esAceptado(HashSet<State> DFAstate, State accept)
        {
            foreach (State state in DFAstate)
            {
                if (state == accept)
                    return true;
            }
            return false;
        }

        /*Método para verificar hasta donde llega un conjunto de estados con un simbolo*/
        private HashSet<State> Move(HashSet<State> T, String x)
        {
            HashSet<State> result = new HashSet<State>();
            foreach (State s in T)
            {
                foreach (Transition t in s.transitions)
                {
                    if (t.character == x && !result.Contains(t.state))
                    {
                        result.Add(t.state);
                    }
                }
            }
            return result;
        }

        /*Método para graficar la tabla de transiciones*/
        public String toString()
        {
            String s = "States |  \t";

            for (int i = 0; i < alfabeto.Count; i++)
            {
                String alpha = alfabeto.ElementAt(alfabeto.Count - i - 1);
                s += alpha + "  ";
            }
            s += "\n-------|-----------------------------------------------------------------------" + "\n";
            for (int i = 0; i <= DEstados.Count(); i++)
            {
                s += i + "     |\t";

                for (int j = 0; j < alfabeto.Count; j++)
                {
                    s += transTable[i, j] + "  ";
                }
                if (posicionesAceptadas.Contains(i))
                    s += "\t ACCEPT STATE";
                if (i == 1)
                    s += "\t START";
                else if (i == 0)
                    s += "\t TRAP";
                s += "\n";
            }
            return s;
        }

        public void getTable()
        {
            String table = "digraph A{\nSD [shape=none, margin= 0, label=\n";
            table += " <<TABLE BORDER=\"0\" CELLBORDER=\"1\" CELLSPACING=\"0\" CELLPADDING=\"4\">\n";
            table += " <TR><TD bgcolor=\"black\"><font color=\"limegreen\">Estados</font></TD>";
            for (int i = 0; i < this.alfabeto.Count; i++)
            {
                String simbolo = alfabeto.ElementAt(alfabeto.Count - i - 1).Trim('\"');
                if (simbolo == ">")
                {
                    simbolo = "MAYOR_QUE";
                }
                else if (simbolo == "<")
                {
                    simbolo = "MENOR_QUE";
                }
                else if (simbolo == "" || simbolo == " ")
                {
                    simbolo = "Space";
                }
                table += "<TD bgcolor=\"black\"><font color=\"magenta\">" + simbolo + "</font></TD>";

            }
            table += "</TR>\n";

            for (int i = 1; i <= this.DEstados.Count; i++)
            {
                table += " <TR><TD bgcolor=\"black\"><font color=\"yellow\">" + i + "</font></TD>";
                for (int j = 0; j < this.alfabeto.Count; j++)
                {
                    if (transTable[i, j] != 0)
                    {
                        table += "<TD bgcolor=\"black\"><font color=\"white\">" + transTable[i, j] + "</font></TD>";
                    }
                    else
                    {
                        table += "<TD bgcolor=\"black\"><font color= \"white\"> --</font> </TD>";
                    }
                }
                table += "</TR>\n";
            }

            table += "</TABLE>\n >];\n}";

            String rutapng = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + this.id + "_TABLE.png";
            String rutadot = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + this.id + "_TABLE.dot";
            System.IO.File.WriteAllText(rutadot, table);
            String commandoDot = "dot.exe -Tpng " + rutadot + " -o " + rutapng;
            var comando = string.Format(commandoDot);
            var procStart = new System.Diagnostics.ProcessStartInfo("cmd", "/c" + comando);
            var proc = new System.Diagnostics.Process();
            procStart.UseShellExecute = false;
            procStart.CreateNoWindow = true;
            procStart.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            proc.StartInfo = procStart;
            proc.Start();
            proc.WaitForExit();

        }

        /*Método para validar un string de entrada en el automata.*/
        public bool validarLexema(String s)
        {

            //Use the transTable, going through each character at a time
            int state = START;
            for (int i = 0; i < s.Length; i++)
            {
                char entry = s[i];
                try
                {
                    string c = entry.ToString();
                    if(perteneceACadena(c))
                    {
                        string aux = getNextPos(s, i);
                        if (aux == "")
                        {

                        }
                        else
                        {
                            c = aux;
                            i += c.Length - 1;
                        }

                    }
                    int indexOf = this.indexOf(c);
                    state = transTable[state, indexOf];  //Get next state to go to after reading a character 
                }
                catch (Exception e)
                {
                    return false;  //Alphabet in string not in alphabet array thus can't be accepted by DFA
                }
                //No coming out of a TRAP state, return false
                if (state == TRAP)
                    return false;
            }
            //At this point, string has finished reading in and we're not in a TRAP state
            if (posicionesAceptadas.Contains(state))        //Check if we're in an ACCEPT state		
                return true;
            else
                return false;                       //Else we're in a non-ACCEPT state(not the TRAP state)
        }

        /*Obtiene la posición de un string en el alfabeto*/
        public int indexOf(string s)
        {

            foreach (var Set in sets.Values)
            {
                Set set = (Set)Set;
                foreach (char value in set.characters)
                {
                    if (s == value.ToString())
                    {
                        s = set.name;
                        break;
                    }
                }
            }

            int res = 0;
            for (int i = 0; i < this.alfabeto.Count(); i++)
            {
                String aux = alfabeto.ElementAt(alfabeto.Count - i - 1).Trim('\"');
                if (aux == s.ToString())
                    return res;
                res++;
            }
            return res;
        }

        /*Método para verificar si un subconjunto ya fue ingresado a DEstados*/
        private bool estaEnDEstados(HashSet<State> U, List<HashSet<State>> DEstados)
        {
            foreach (HashSet<State> s in DEstados)
            {
                if (HashSet<State>.CreateSetComparer().Equals(s, U))
                {
                    return true;
                }
            }
            return false;

        }

        /*Método que convierte un AFN en un AFD*/
        public void convertirAFN(Automoton AFN)
        {
            /*Inicalización de variables*/
            alfabeto = AFN.alphabet;
            id = AFN.id;
            Queue pila = new Queue();
            transTable = new int[MAX_STATES, alfabeto.Count];
            /*Fin de inicialización de variables*/

            HashSet<State> S = E_cerradura(AFN.initialState);//1) S= e-cerradura(S0)
            DEstados.Add(S);//Añado a DEstados el estado inicial.
            if (esAceptado(S, AFN.finalState))
            {
                this.posicionesAceptadas.Add(DEstados.Count);
            }
            pila.Enqueue(S);//Lo añado a los estados sin marcar.
            int estado = START;
            while (pila.Count > 0)//Mientras hay un estado sin marcar en DEstados
            {
                HashSet<State> T = (HashSet<State>)pila.Dequeue();//Marcar estado T.
                for (int i = 0; i < alfabeto.Count; i++)//Para cada simbolo de entrada hacer mov
                {
                    String simbolo = alfabeto.ElementAt(alfabeto.Count - i - 1);
                    HashSet<State> U = E_cerradura(Move(T, simbolo));//U = e-cerradura(mover(T,a));
                    if (U.Count <= 0)
                    {
                        transTable[estado, i] = TRAP;

                    }
                    else if (!estaEnDEstados(U, DEstados))
                    {
                        pila.Enqueue(U);
                        DEstados.Add(U);
                        transTable[estado, i] = DEstados.Count;
                        if (esAceptado(U, AFN.finalState))
                        {
                            this.posicionesAceptadas.Add(DEstados.Count);
                        }
                    }
                    else
                    {
                        transTable[estado, i] = getPos(DEstados, U) + 1;
                    }
                }
                estado++;
            }
        }

        private int getPos(List<HashSet<State>> lista, HashSet<State> estado)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                if (HashSet<State>.CreateSetComparer().Equals(estado, lista.ElementAt(i)))
                {
                    return i;
                }
            }
            return 0;
        }

        public void graphViz()
        {
            String dot = "digraph D{\n";
            dot += "color= green;\ngraph[bgcolor = black];\nnode[style = dashed\ncolor = yellow fontcolor = white]\nedge[color = red fontcolor = white]\n";
            dot += "\tstyle=filled;\n\trankdir=LR;\n\tinicio -> 1;\n\tinicio[shape = point];\n";
            dot += "\tordering=in\n\tranksep=.7\n\tnodesep=.6\n";
            for (int i = 1; i <= DEstados.Count; i++)
            {
                for (int j = 0; j < alfabeto.Count; j++)
                {
                    if (transTable[i, j] != 0)
                    {
                        string simbolo = alfabeto.ElementAt(alfabeto.Count - j - 1).Trim('\"');
                        dot += "\t" + i + " -> " + transTable[i, j].ToString() + "[label=\"" + simbolo + "\"];\n";
                    }
                }
            }

            for (int i = 1; i <= DEstados.Count; i++)
            {

                dot += "\t" + i + "[shape=circle];\n";
                if (posicionesAceptadas.Contains(i))
                {
                    dot += "\t" + i + "[shape=doublecircle];\n";
                }
            }
            dot += "}";

            String rutapng = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + this.id + "_AFD.png";
            String rutadot = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + this.id + "_AFD.dot";
            System.IO.File.WriteAllText(rutadot, dot);
            String commandoDot = "dot.exe -Tpng " + rutadot + " -o " + rutapng;
            var comando = string.Format(commandoDot);
            var procStart = new System.Diagnostics.ProcessStartInfo("cmd", "/c" + comando);
            var proc = new System.Diagnostics.Process();
            procStart.UseShellExecute = false;
            procStart.CreateNoWindow = true;
            procStart.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            proc.StartInfo = procStart;
            proc.Start();
            proc.WaitForExit();

            //Console.WriteLine(dot);
        }

        public bool perteneceACadena(String s)
        {
            foreach (string cadena in this.cadenas)
            {
                if (s == cadena[0].ToString())
                {
                    return true;
                }
            }
            return false;
        }

        public string  getNextPos(String s, int i)
        {
            foreach(string cadena in this.cadenas)
            {
                int contador = 0;
                bool match = true;
                for(int j = i; j < i+cadena.Length; j++)
                {
                    if (cadena[contador] != s[j])
                    {
                        match = false;
                        break;
                    }
                    contador++;
                }
                if(match)
                {

                    return cadena;
                }
            }
            return "";
        }
    }
}
