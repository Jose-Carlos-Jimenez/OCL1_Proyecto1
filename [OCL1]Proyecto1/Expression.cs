using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OCL1_Proyecto1
{
    class Expression
    {
        public string name;
        public LinkedList<Token> values;
        public ASTTree tree;
        public LinkedList<string> alphabet;
        public LinkedList<string> cadenas;
        public Hashtable sets;

        public Expression()
        {
            this.name = "";
            this.values = new LinkedList<Token>();
            this.alphabet = new LinkedList<string>();
            this.tree = new ASTTree(this.name);
            this.cadenas = new LinkedList<string>();
            this.sets = new Hashtable();
        }

        public void setTree()
        {
            Stack<Nodo> aux = new Stack<Nodo>();
            LinkedListNode<Token> node = values.Last;
            for(int i = values.Count; i > 0; i--)
            { 
                if(node.Value.type == Token.Type.KLEENE_CLOSURE || node.Value.type == Token.Type.BINARY_CLOSURE)
                {
                    Nodo left = aux.Pop();
                    Nodo nuevo = new Nodo(node.Value.lexem);
                    nuevo.left = left;
                    aux.Push(nuevo);
                    this.tree.root = nuevo;
                }
                else if(node.Value.type == Token.Type.POSITIVE_CLOSURE)
                {
                    Nodo left = aux.Pop();//Saco terminal
                    Nodo copyOf = (Nodo)left.Clone();//Copia del terminal
                    Nodo kleene = new Nodo("*");//Creo el nodo para la cerradura de kleene
                    Nodo concat = new Nodo(".");//Creo el nodo concat

                    kleene.left = copyOf;
                    concat.right = kleene;
                    concat.left = left;
                    aux.Push(concat);
                    this.tree.root = concat;
                }
                else if(node.Value.type == Token.Type.CONCAT || node.Value.type == Token.Type.OR)
                {
                    Nodo right = aux.Pop();
                    Nodo left = aux.Pop();                    
                    Nodo nuevo = new Nodo(node.Value.lexem);
                    nuevo.left = left;
                    nuevo.right = right;
                    aux.Push(nuevo);
                    this.tree.root = nuevo;
                }
                else
                {
                    if(node.Value.type != Token.Type.INTRO && node.Value.type != Token.Type.TABULATION && node.Value.type != Token.Type.SPECIAL_DOUBLE_COM &&
                        node.Value.type != Token.Type.SPECIAL_SIMPLE_COM)
                    {
                        if (node.Value.type == Token.Type.SET && !alphabet.Contains(node.Value.lexem.Trim('[').Trim(']').Trim(':')))
                        {
                            string auxs = node.Value.lexem.Trim('[').Trim(']').Trim(':');
                            this.alphabet.AddLast(auxs);
                            Nodo n = new Nodo(auxs);
                            aux.Push(n);
                        }
                        else if (!alphabet.Contains(node.Value.lexem))
                        {
                            this.alphabet.AddLast(node.Value.lexem);
                            Nodo n = new Nodo(node.Value.lexem);
                            aux.Push(n);
                        }
                        else
                        {
                            Nodo n = new Nodo(node.Value.lexem);
                            aux.Push(n);
                        }
                    }
                }
                node = node.Previous;
            }
            this.tree.printPostorder(this.tree.root);
            this.tree.id = this.name;
            this.tree.alphabet = this.alphabet;
            this.tree.sets = this.sets;
            this.tree.cadenas = cadenas;
        }

    }
}
