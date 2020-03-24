using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OCL1_Proyecto1
{  
    class Token
    {
        public enum Type
        {
            LEFT_KEY, RIGHT_KEY, LINE_COMMENT, PAR_COMMENT, ID,
            CONCAT, OR, BINARY_CLOSURE, KLEENE_CLOSURE, POSITIVE_CLOSURE,
            VIRG, COLON, INTRO, DOUBLE_COM, TABULATION,
            EVERYTHING, RESERVED_CONJ, DOUBLE_DOT, LEFT_ARROW, SEMICOLON,
            SPECIAL_SIMPLE_COM, SPECIAL_DOUBLE_COM, STRING, ERROR, NUMBER,
            END,ASCII_CHAR, SET
        }
        public Type type;
        public String lexem;
        public int row, column;
        public String token;

        public Token(Type type, String lexem, int row, int column)
        {
            this.type = type;
            this.lexem = lexem;
            this.row = row;
            this.column = column;
        }

        public Token(Type type, string lexem, int row, int column, string token)
        {
            this.type = type;
            this.lexem = lexem;
            this.row = row;
            this.column = column;
            this.token = token;
        }

        public string  getLex()
        {
            return lexem;
        }

        public Type getType()
        {
            return type;
        }

        public int getRow()
        {
            return row;
        }

        public int getCol()
        {
            return column;
        }

        public string getToken()
        {
            return token;
        }

        public string getTypeDesc()
        {
            switch (type)
            {
                case Type.BINARY_CLOSURE:
                    return "Cerradura binaria";
                case Type.COLON:
                    return "Coma";
                case Type.CONCAT:
                    return "Concatenación";
                case Type.DOUBLE_COM:
                    return "Comilla doble";
                case Type.DOUBLE_DOT:
                    return "Dos puntos";
                case Type.ERROR:
                    return "Error";
                case Type.ID:
                    return "Identificador";
                case Type.INTRO:
                    return "Salto de linea";
                case Type.KLEENE_CLOSURE:
                    return "Cerradura de Kleene";
                case Type.LEFT_ARROW:
                    return "Flecha";
                case Type.LEFT_KEY:
                    return "Llave de apertura";
                case Type.LINE_COMMENT:
                    return "Comentario de linea";
                case Type.OR:
                    return "Alternancia";
                case Type.PAR_COMMENT:
                    return "Comentario multilinea";
                case Type.POSITIVE_CLOSURE:
                    return "Cerradura positiva";
                case Type.RESERVED_CONJ:
                    return "Reservada CONJ";
                case Type.RIGHT_KEY:
                    return "LLave de cierre";
                case Type.SEMICOLON:
                    return "Punto y coma";
                case Type.SPECIAL_DOUBLE_COM:
                    return "Caracter especial comilla doble";
                case Type.SPECIAL_SIMPLE_COM:
                    return "Caracter especial comilla simple";
                case Type.STRING:
                    return "Cadena";
                case Type.TABULATION:
                    return "Caracter especial tabulación";
                case Type.VIRG:
                    return "Virgulilla";
                default:
                    return "desconocido";
            }
        }
    }
}
