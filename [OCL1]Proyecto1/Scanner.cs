using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace _OCL1_Proyecto1
{
    class Scanner
    {
        public LinkedList<Token> outList;
        public Hashtable sets;  
        public Hashtable expressions;
        public Queue valuations;
        public LinkedList<string> names;
        int state;
        private string auxLex;
        private int row, column;
        public bool error;
        public string consoleOut;

        public Scanner()
        {
            this.outList = new LinkedList<Token>();
            this.sets = new Hashtable();
            this.expressions = new Hashtable();
            this.names = new LinkedList<string>();
            this.valuations = new Queue();
        }

        public void AddToken(Token.Type type)
        {
            if (type == Token.Type.ERROR)
            {
                outList.AddLast(new Token(type, auxLex, row, column, "Error léxico"));
                //Console.WriteLine("Error: \n" + auxLex + "\n");
                MessageBox.Show("Error en fila " + row + " y columna " + column + " con el lexema " + auxLex);
                error = true;
            }
            else
            {
                outList.AddLast(new Token(type, auxLex, row, column));
                //Console.WriteLine("El token es:\n" + auxLex + "\n");
            }
            auxLex = "";
            state = 0;
        }
        
        public LinkedList<Token> getTokens()
        {
            return outList;
        }

        public void analyze(String inString)
        {
            inString += "#";
            row = 1;
            outList = new LinkedList<Token>();
            state = 0;
            auxLex = "";
            char pointer;
            

            for(int i = 0; i < inString.Length; i++)
            {
                pointer = inString[i];
                column++;
                switch (state)
                {
                    case 0:
                        auxLex += pointer;
                        switch (pointer)
                        {
                            case '{':
                                AddToken(Token.Type.LEFT_KEY);
                                break;
                            case '}':
                                AddToken(Token.Type.RIGHT_KEY);
                                break;
                            case '.':
                                AddToken(Token.Type.CONCAT);
                                break;
                            case '|':
                                AddToken(Token.Type.OR);
                                break;
                            case '?':
                                AddToken(Token.Type.BINARY_CLOSURE);
                                break;
                            case '*':
                                AddToken(Token.Type.KLEENE_CLOSURE);
                                break;
                            case '+':
                                AddToken(Token.Type.POSITIVE_CLOSURE);
                                break;
                            case '~':
                                AddToken(Token.Type.VIRG);
                                break;
                            case ',':
                                AddToken(Token.Type.COLON);
                                break;
                            case ';':
                                AddToken(Token.Type.SEMICOLON);
                                break;
                            case '\"':
                                state = 13;
                                break;
                            case '<':
                                state = 1;
                                break;
                            case '/':
                                state = 4;
                                break;
                            case '\\':
                                state = 7;
                                break;
                            case '[':
                                state = 8;
                                break;
                            case '-':
                                state = 11;
                                break;
                            case '\n':
                                auxLex = "";
                                row++;
                                column = 0;
                                state = 0;
                                break;
                            case '\t':
                                auxLex = "";
                                state = 0;
                                break;
                            case ' ':
                                auxLex = "";
                                state = 0;
                                break;
                            case '\r':
                                auxLex = "";
                                state = 0;
                                break;
                            case ':':
                                AddToken(Token.Type.DOUBLE_DOT);
                                break;
                            case 'C':
                                state = 14;
                                break;
                            default:
                                if(Char.IsDigit(pointer))
                                {
                                    state = 12;
                                    break;
                                }
                                else if(Char.IsLetterOrDigit(pointer) || pointer == '_')
                                {
                                    state = 6;
                                    break;
                                }
                                else if(pointer.Equals('#') && i == inString.Length - 1)
                                {
                                    AddToken(Token.Type.END);
                                }
                                else if((int)pointer >= 32 && (int)pointer <= 125)
                                {
                                    AddToken(Token.Type.ASCII_CHAR);
                                }
                                else
                                {
                                    AddToken(Token.Type.ERROR);
                                }
                                state = 0;
                                break;
                        }
                        break;
                    case 1:
                        if(pointer == '!')
                        {
                            auxLex += pointer;
                            state = 2;
                        }
                        else
                        {
                            AddToken(Token.Type.ASCII_CHAR);
                            i--;
                        }
                        break;
                    case 2:
                        auxLex += pointer;
                        if(pointer == '!' )
                        {
                            state = 3;
                        }
                        if (pointer == '\n')
                        {
                            column = 0;
                            row++;
                        }
                        break;
                    case 3:
                        if (pointer != '>')
                        {
                            AddToken(Token.Type.PAR_COMMENT);
                            i--;
                        }
                        else
                        {
                            auxLex += pointer;
                        }
                        break;
                    case 4:
                        
                        if(pointer == '/')
                        {
                            auxLex += pointer;
                            state = 5;
                        }
                        else
                        {
                            AddToken(Token.Type.ASCII_CHAR);
                            i--;
                        }
                        break;
                    case 5:
                        
                        if(pointer == '\n' || (pointer == '#' && i + 1 == inString.Length))
                        {
                            AddToken(Token.Type.LINE_COMMENT);
                            i--;
                        }
                        else
                        {
                            auxLex += pointer;
                        }
                        break;
                    case 6:
                        if(Char.IsLetterOrDigit(pointer) || pointer == '_')
                        {
                            auxLex += pointer;
                        }
                        else
                        {
                            AddToken(Token.Type.ID);
                            i--;
                        }
                        break;
                    case 7:
                        if(pointer == '\'')
                        {
                            auxLex += pointer;
                            AddToken(Token.Type.SPECIAL_SIMPLE_COM);
                        }
                        else if(pointer == 'n')
                        {
                            auxLex += pointer;
                            AddToken(Token.Type.INTRO);
                        }
                        else if(pointer == 't')
                        {
                            auxLex += pointer;
                            AddToken(Token.Type.TABULATION);
                        }
                        else if( pointer == '\"')
                        {
                            auxLex += pointer;
                            AddToken(Token.Type.SPECIAL_DOUBLE_COM);
                        }
                        else
                        {
                            AddToken(Token.Type.ASCII_CHAR);
                            i--;
                        }
                        break;
                    case 8:
                        if(pointer == ':')
                        {
                            auxLex += pointer;
                            state = 9;
                        }
                        else
                        {
                            AddToken(Token.Type.ASCII_CHAR);
                            i--;
                        }
                        break;
                    case 9:
                        if(pointer == ':')
                        {
                            auxLex += pointer;
                            state = 10;
                        }
                        else
                        {
                            auxLex += pointer;

                        }
                        break;
                    case 10:
                        if(pointer == ']')
                        {
                            auxLex += pointer;
                            AddToken(Token.Type.SET);
                        }
                        else
                        {
                            auxLex += pointer;
                            AddToken(Token.Type.ERROR);
                        }
                        break;
                    case 11:
                        if(pointer == '>')
                        {
                            auxLex += pointer;
                            AddToken(Token.Type.LEFT_ARROW);
                        }
                        else
                        {
                            AddToken(Token.Type.ASCII_CHAR);
                            i--;
                        }
                        break;
                    case 12:
                        if(Char.IsDigit(pointer))
                        {
                            auxLex += pointer;
                        }
                        else
                        {
                            AddToken(Token.Type.NUMBER);
                            state = 0;
                            i--;
                        }
                        break;
                    case 13:
                        auxLex += pointer;
                        if (pointer == '\"')
                        {
                            AddToken(Token.Type.STRING);
                        }
                        break;
                    case 14:
                        auxLex += pointer;
                        if(pointer == 'O')
                        {
                            state = 15;
                        }
                        else
                        {
                            state = 6;
                        }
                        break;
                    case 15:
                        auxLex += pointer;
                        if (pointer == 'N')
                        {
                            state = 16;
                        }
                        else
                        {
                            state = 6;
                        }
                        break;
                    case 16:
                        auxLex += pointer;
                        if (pointer == 'J')
                        {
                            AddToken(Token.Type.RESERVED_CONJ);
                        }
                        else
                        {
                            state = 6;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public void getConj()
        {
            LinkedListNode<Token> aux = outList.First;
            while(aux != outList.Last)
            {
                if(aux.Value.type == Token.Type.RESERVED_CONJ)
                {
                    aux = obtainOneConj(aux);
                }
                aux = aux.Next;
            }
        }

        public LinkedListNode<Token> obtainOneConj(LinkedListNode<Token> aux)
        {
            Set conj = new Set();
            conj.name = aux.Next.Next.Value.lexem;

            if(aux.Next.Next.Next.Next.Next.Value.type == Token.Type.VIRG)
            {
                aux = aux.Next.Next.Next.Next;//Obtengo el token que guarda el primer caracter.
                int init = (int)Char.Parse(aux.Value.lexem);//Obtengo su código ASCII.
                aux = aux.Next.Next;//Me muevo al ultimo caracter del conjunto.
                int final = (int)Char.Parse(aux.Value.lexem);//Obtengo su código ASCII.

                for(int i = init; i <= final; i++ )//Desde el caracter inicio hasta el caracter final añado a la lista según su ASCII.
                {
                    conj.characters.AddLast((char)i);
                }
                sets.Add(conj.name, conj);//Añado el conjunto a la tabla hash de conjuntos.}
            }
            else
            {
                aux = aux.Next.Next.Next.Next;//Referencio a la primera letra.
                while(aux.Value.type != Token.Type.SEMICOLON)//Mientras no sea ';' siga
                {
                    if(aux.Value.type != Token.Type.COLON)//Si es distinto a ',' agregelo a la lista de caracteres del conjunto.
                    {
                        conj.characters.AddLast(Char.Parse(aux.Value.lexem));
                    }
                    aux = aux.Next;//Recorra el siguiente feca
                }
                sets.Add(conj.name, conj);
            }
            return aux;

        }

        public void getExpr()
        {
            LinkedListNode<Token> aux = outList.First;
            while(aux != outList.Last)
            {
                if(aux.Value.type == Token.Type.ID && aux.Previous.Value.type != Token.Type.DOUBLE_DOT && aux.Next.Value.type == Token.Type.LEFT_ARROW)
                {
                    aux = getOneExpresion(aux);
                }
                aux = aux.Next;
            }
        }

        public LinkedListNode<Token> getOneExpresion(LinkedListNode<Token> aux )
        {
            Expression newExp = new Expression();
            newExp.name = aux.Value.lexem;
            aux = aux.Next.Next;
            while(aux.Value.type != Token.Type.SEMICOLON)
            {
                if (aux.Value.type == Token.Type.LEFT_KEY)
                {
                    aux = aux.Next;
                    newExp.values.AddLast(aux.Value);
                    if (this.sets.Contains(aux.Value.lexem) && !newExp.sets.Contains(aux.Value.lexem))
                    {
                        newExp.sets.Add(aux.Value.lexem, this.sets[aux.Value.lexem]);
                    }
                    aux = aux.Next;
                }
                else
                {
                    newExp.values.AddLast(aux.Value);
                    if (aux.Value.type == Token.Type.STRING)
                    {
                        newExp.cadenas.AddLast(aux.Value.lexem.Trim('\"'));
                    }
                    else if (aux.Value.type == Token.Type.SET)
                    {
                        newExp.cadenas.AddLast(aux.Value.lexem.Trim(']').Trim('[').Trim(':'));
                    }

                }
                aux = aux.Next;
            }
            newExp.setTree();
            newExp.tree.getAFN();
            newExp.tree.AFN.setStatesId(newExp.tree.AFN.initialState);
            newExp.tree.AFN.print();
            newExp.tree.AFN.generarDot();
            expressions.Add(newExp.name, newExp);
            names.AddLast(newExp.name);
            return aux;
        }

        public string getValuations()
        {
            consoleOut = "";
            LinkedListNode<Token> aux = outList.First;
            while(aux !=outList.Last)
            {
                if(aux.Value.type == Token.Type.ID && aux.Next.Value.type == Token.Type.DOUBLE_DOT)
                {
                    Valuation v = new Valuation();
                    v.id = aux.Value.lexem;
                    v.value = aux.Next.Next.Value.lexem;
                    v.row = aux.Next.Next.Value.row;
                    v.column = aux.Next.Next.Value.column;
                    this.valuations.Enqueue(v);
                }
                aux = aux.Next;
            }

            while(valuations.Count > 0)
            {
                Valuation exp = (Valuation)valuations.Dequeue();
                string value = exp.value.Trim('\"');
                if(this.expressions.ContainsKey(exp.id))
                {
                    Expression ans = (Expression)expressions[exp.id];
                    if(ans.tree.AFD.validarLexema(value))
                    {
                        consoleOut += "El lexema " + value + " es aceptado.\n";
                    }
                    else
                    {
                        consoleOut += "El lexema " + value+" en la fila " +exp.row + " es rechazado.\n";
                    }
                }
                else
                {
                    consoleOut += "La expresión " + exp.id +" no ha sido declarada.\n ";
                }
            }
            return consoleOut;
        }
    }
}
