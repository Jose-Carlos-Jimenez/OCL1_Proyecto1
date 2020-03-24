using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OCL1_Proyecto1
{
    class ASTTree
    {
        public Nodo root;
        public string graphviz;
        public LinkedList<Nodo> postfix;
        public Automoton AFN;
        public AFD AFD;
        public LinkedList<String> cadenas;
        public string id;
        public LinkedList<string> alphabet;
        public Hashtable sets;
        
        public ASTTree(String id)
        {
            root = null;
            postfix = new LinkedList<Nodo>();
            this.id = id;
        }

        public void graph()
        {
            printPreorder(root);
        }

        void printPreorder(Nodo node)
        {
            if (node == null)
                return;

            /* first print data of node */
            Console.Write(node.data + " ");

            /* then recur on left sutree */
            printPreorder(node.left);

            /* now recur on right subtree */
            printPreorder(node.right);
        }

        void printInorder(Nodo node)
        {
            if (node == null)
                return;

            /* first recur on left child */
            printInorder(node.left);

            /* then print the data of node */
            Console.Write(node.data + " ");

            /* now recur on right child */
            printInorder(node.right);
        }

        void bst_print_dot_aux(Nodo node)
        {

            if (node.left != null)
            {
                this.graphviz += "     " + node.data + " -> " + node.left.data + ";\n";
                bst_print_dot_aux(node.left);
            }
            if (node.right != null)
            {
                this.graphviz += "     " + node.data + " -> " + node.right.data + ";\n";
                bst_print_dot_aux(node.right);
            }                
        }

        public void printPostorder(Nodo node)
        {
            if (node == null)
                return;

            // first recur on left subtree 
            printPostorder(node.left);

            // then recur on right subtree 
            printPostorder(node.right);

            // now deal with the node 
            this.postfix.AddLast(node);
        }

        public void getAFN()
        {
            Stack<Automoton> automatonCons = new Stack<Automoton>();
            Automoton auto = new Automoton();
            foreach (Nodo n in this.postfix)
            {
                if (n.left == null && n.right == null)//SI ES SIMBOLO TERMINAL
                {
                    //AGREGAR A LA PILA EL AUTOMATA PARA UN TERMINAL, ES DECIR UN NODO.
                    Automoton part = new Automoton();//Creo un nuevo automata.

                    State first = new State();//Creo el primer estado.

                    State last = new State();//Creo el último estado e indico que será el final.

                    Transition niu = new Transition();//Creo la transición.
                    niu.character = n.data;//Asigno el caracter con el cual funcionará.
                    niu.state = last;//Asigno un estado al cual debe moverse.
                    first.AddTransition(niu);

                    part.initialState = first;//Le asigno al automata su estado inicial.
                    part.finalState = last;//Le asigno su estado final

                    auto = part;
                    automatonCons.Push(part);//Añado el automata a la pila.
                }
                else
                {
                    if (n.data == "?")
                    {
                        Automoton aux1 = automatonCons.Pop();
                        Automoton nuevo = new Automoton();
                        State s1 = new State();
                        State s2 = new State();
                        State s3 = new State();
                        State s4 = new State();

                        Transition t1 = new Transition();
                        Transition t2 = new Transition();
                        Transition t3 = new Transition();
                        Transition t4 = new Transition();
                        Transition t5 = new Transition();

                        t1.character = "ɛ";
                        t2.character = "ɛ";
                        t3.character = "ɛ";
                        t4.character = "ɛ";
                        t5.character = "ɛ";

                        t1.state = s2;
                        t2.state = aux1.initialState;
                        s1.AddTransition(t1);
                        s1.AddTransition(t2);

                        t3.state = s3;
                        s2.AddTransition(t3);

                        t4.state = s4;
                        s3.AddTransition(t4);

                        t5.state = s4;
                        aux1.finalState.AddTransition(t5);

                        nuevo.initialState = s1;
                        nuevo.finalState = s4;
                        auto = nuevo;
                        automatonCons.Push(nuevo);
                    }
                    else if (n.data == "*")
                    {
                        Automoton aux1 = automatonCons.Pop();
                        Automoton nuevo = new Automoton();

                        State i = new State();
                        State f = new State();

                        Transition t1 = new Transition();
                        Transition t2 = new Transition();
                        Transition t3 = new Transition();
                        Transition t4 = new Transition();

                        t1.character = "ɛ";
                        t2.character = "ɛ";
                        t3.character = "ɛ";
                        t4.character = "ɛ";

                        t1.state = aux1.initialState;
                        t4.state = f;
                        i.AddTransition(t4);
                        i.AddTransition(t1);

                        t2.state = aux1.initialState;
                        aux1.finalState.AddTransition(t2);

                        t3.state = f;
                        aux1.finalState.AddTransition(t3);

                        nuevo.initialState = i;
                        nuevo.finalState = f;
                        auto = nuevo;
                        automatonCons.Push(nuevo);
                    }
                    else if (n.data == "+")
                    {
                        Automoton aux1 = automatonCons.Pop();//Para concatenar.
                        Automoton aux2 = (Automoton)aux1.Clone();//Para cerradura de Kleene.
                        Automoton nuevo = new Automoton();//El que será el nuevo

                        State f = new State();

                        Transition t1 = new Transition();
                        Transition t2 = new Transition();
                        Transition t3 = new Transition();
                        Transition t4 = new Transition();

                        t1.character = "ɛ";
                        t2.character = "ɛ";
                        t3.character = "ɛ";
                        t4.character = "ɛ";

                        t1.state = aux2.initialState;
                        t2.state = f;
                        aux1.finalState.AddTransition(t1);
                        aux1.finalState.AddTransition(t2);

                        t3.state = aux2.initialState;
                        t4.state = f;
                        aux2.finalState.AddTransition(t3);
                        aux2.finalState.AddTransition(t4);

                        nuevo.initialState = aux1.initialState;
                        nuevo.finalState = f;

                        automatonCons.Push(nuevo);
                        auto = nuevo;
                    }
                    else if (n.data == ".")
                    {
                        Automoton aux1 = automatonCons.Pop();
                        Automoton aux2 = automatonCons.Pop();//Saco 2 automatas de la pila

                        Automoton nuevo = new Automoton();//Genero uno nuevo

                        aux1.finalState.transitions = aux2.initialState.transitions;//Se fusionan el final de uno y el inicial del otro.
                        
                        nuevo.initialState = aux1.initialState;
                        nuevo.finalState = aux2.finalState;

                        automatonCons.Push(nuevo);
                        auto = nuevo;
                    }
                    else if (n.data == "|")
                    {
                        Automoton aux1 = automatonCons.Pop();
                        Automoton aux2 = automatonCons.Pop();
                        Automoton nuevo = new Automoton();
                        State initial = new State();
                        State final = new State();

                        Transition t1 = new Transition();
                        t1.character = "ɛ";
                        t1.state = aux1.initialState;
                        initial.AddTransition(t1);

                        Transition t2 = new Transition();
                        t2.character = "ɛ";
                        t2.state = aux2.initialState;
                        initial.AddTransition(t2);

                        Transition t3 = new Transition();
                        t3.character = "ɛ";
                        t3.state = final;
                        aux1.finalState.AddTransition(t3);

                        Transition t4 = new Transition();
                        t4.character = "ɛ";
                        t4.state = final;
                        aux2.finalState.AddTransition(t4);

                        nuevo.initialState = initial;
                        nuevo.finalState = final;
                        automatonCons.Push(nuevo);
                        auto = nuevo;
                    }
                    else
                    {
                        Console.WriteLine("Error en el token " + n.data);
                    }
                }
            }
            auto.alphabet = this.alphabet;
            auto.setStatesId(auto.initialState);
            this.AFN = auto;
            this.AFN.id = this.id;
            this.AFD = new AFD(this.AFN);
            this.AFD.sets = this.sets;
            this.AFD.cadenas = this.cadenas;
            this.AFD.getTable();
            //Console.WriteLine(this.AFD.toString());
            this.AFD.graphViz();
        }

    }
}
