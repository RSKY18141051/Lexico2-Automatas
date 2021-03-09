using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Lexico2
{
    class Lexico : Token, IDisposable
    {
        private StreamReader archivo;
        private StreamWriter bitacora;

        public Lexico()
        {
            Console.WriteLine("Compilando prueba.txt");

            if (File.Exists("C:\\Archivos\\prueba.txt"))
            {
                archivo = new StreamReader("C:\\Archivos\\prueba.txt");
                bitacora = new StreamWriter("C:\\Archivos\\prueba.log");
                bitacora.AutoFlush = true;

                bitacora.WriteLine("Archivo: prueba.txt");
                bitacora.WriteLine("Directorio: C:\\Archivos");
            }
            else
            {
                throw new Exception("El archivo prueba.txt no existe.");
            }

        }
        //~Lexico() Destructor xd
        public void Dispose()
        {
            Console.WriteLine("Finaliza compilacion de prueba.txt");
            CerrarArchivos();
        }

        private void CerrarArchivos()
        {
            archivo.Close();
            bitacora.Close();
        }

        public void NextToken()
        {
            char c;
            string palabra = "";
            int estado = 0;
            const int F = -1;
            const int E = -2;

            while (estado >= 0)
            {
                c = (char)archivo.Peek();

                switch (estado)
                {
                    case 0:
                        if (char.IsWhiteSpace(c))
                        {
                            estado = 0;
                        }
                        else if(char.IsLetter(c))
                        {
                            estado = 1;
                        }
                        else if (char.IsDigit(c))
                        {
                            estado = 2;
                        }
                        else if (c == '=')
                        {
                            estado = 8;
                        }
                        else if (c == ':')
                        {
                            estado = 9;
                        }
                        else if (c == ';')
                        {
                            estado = 11;
                        }
                        else if (c == '&')
                        {
                            estado = 12;
                        }
                        else if (c == '|')
                        {
                            estado = 13;
                        }
                        else if (c == '!')
                        {
                            estado = 14;
                        }
                        else if (c == '>')
                        {
                            estado = 17;
                        }
                        else if (c == '<')
                        {
                            estado = 18;
                        }
                        else if (c == '+')
                        {
                            estado = 19;
                        }
                        else if (c == '-')
                        {
                            estado = 20;
                        }
                        else if (c == '\"')
                        {
                            estado = 24;
                        }
                        else if (c == '\'')
                        {
                            estado = 26;
                        }
                        else if (c == '%' || c == '/' || c == '*')
                        {
                            estado = 22;
                        }
                        else if (c == '?')
                        {
                            estado = 28;
                        }
                        else
                        {
                            estado = 29;
                        }
                        break;
                    case 1:
                        setClasificacion(clasificaciones.identificador);
                        if (char.IsLetterOrDigit(c))
                        {
                            estado = 1;
                        }
                        else
                        {
                            estado = F;
                        }
                        /* FORMA ALTERNATIVA
                        if(!char.IsLetterOrDigit(c))
                        {
                            estado = F;
                        }
                        */
                        break;
                    case 2:
                        setClasificacion(clasificaciones.numero);
                        if (char.IsDigit(c))
                        {
                            estado = 2;
                        }
                        else if (c == '.')
                        {
                            estado = 3;
                        }
                        else if (char.ToLower(c) == 'e')
                        {
                            estado = 5;
                        }
                        else
                        {
                            estado = F;
                        }
                        break;
                    case 3:
                        if (char.IsDigit(c))
                        {
                            estado = 4;
                        }
                        else
                        {
                            throw new Exception("Error lexico: Se espera un digito");
                        }
                        break;
                    case 4:
                        if (char.IsDigit(c))
                        {
                            estado = 4;
                        }
                        else if (char.ToLower(c) == 'e')
                        {
                            estado = 5;
                        }
                        else
                        {
                            estado = F;
                        }
                        break;
                    case 5:
                        if (c == '+' || c == '-')
                        {
                            estado = 6;
                        }
                        else if (char.IsDigit(c))
                        {
                            estado = 7;
                        }
                        else
                        {
                            throw new Exception("Error lexico: Se espera un digito");
                        }
                        break;
                    case 6:
                        if (char.IsDigit(c))
                        {
                            estado = 7;
                        }
                        else
                        {
                            throw new Exception("Error lexico: Se espera un digito");
                        }
                        break;
                    case 7:
                        if (!char.IsDigit(c))
                        {
                            estado = F;
                        }
                        break;
                    case 8:
                        setClasificacion(clasificaciones.asignacion);
                        if (c == '=')
                        {
                            estado = 16;
                        }
                        else
                        {
                            estado = F;
                        }
                        break;
                    case 9:
                        setClasificacion(clasificaciones.caracter);
                        if (c == '=')
                        {
                            estado = 10;
                        }
                        else
                        {
                            estado = F;
                        }
                        break;
                    case 10:
                        setClasificacion(clasificaciones.inicializacion);
                        estado = F;
                        break;
                    case 11:
                        setClasificacion(clasificaciones.finSentencia);
                        estado = F;
                        break;
                    case 12:
                        setClasificacion(clasificaciones.caracter);
                        if (c == '&')
                        {
                            estado = 15;
                        }
                        else
                        {
                            estado = F;
                        }
                        break;
                    case 13:
                        setClasificacion(clasificaciones.caracter);
                        if (c == '|')
                        {
                            estado = 15;
                        }
                        else
                        {
                            estado = F;
                        }
                        break;
                    case 14:
                        setClasificacion(clasificaciones.operadorLogico);
                        if (c == '=')
                        {
                            estado = 16;
                        }
                        else
                        {
                            estado = F;
                        }
                        break;
                    case 15:
                        setClasificacion(clasificaciones.operadorLogico);
                        estado = F;
                        break;
                    case 16:
                        setClasificacion(clasificaciones.operadorRelacional);
                        estado = F;
                        break;
                    case 17:
                        setClasificacion(clasificaciones.operadorRelacional);
                        if (c == '=')
                        {
                            estado = 16;
                        }
                        else
                        {
                            estado = F;
                        }
                        break;
                    case 18:
                        setClasificacion(clasificaciones.operadorRelacional);
                        if (c == '>' || c == '=')
                        {
                            estado = 16;
                        }
                        else
                        {
                            estado = F;
                        }
                        break;
                    case 19:
                        setClasificacion(clasificaciones.operadorRelacional);
                        if(c == '+' || c == '=')
                        {
                            estado = 21;
                        }
                        else
                        {
                            estado = F;
                        }
                        break;
                    case 20:
                        setClasificacion(clasificaciones.operadorTermino);
                        if (c == '-' || c == '=')
                        {
                            estado = 21;
                        }
                        else
                        {
                            estado = F;
                        }
                        break;
                    case 21:
                        setClasificacion(clasificaciones.incrementoTermino);
                        estado = F;
                        break;
                    case 22:
                        setClasificacion(clasificaciones.operadorFactor);
                        if (c == '=')
                        {
                            estado = 23;
                        }
                        else
                        {
                            estado = F;
                        }
                        break;
                    case 23:
                        setClasificacion(clasificaciones.incrementoFactor);
                        estado = F;
                        break;
                    case 24:
                        setClasificacion(clasificaciones.cadena);
                        if (c == '\"')
                        {
                            estado = 25;
                        }
                        else if(FinDeArchivo())
                        {
                            throw new Exception("Error lexico: se esperan comillas");
                        }
                        else
                        {
                            estado = 24;
                        }
                        break;
                    case 25:
                        estado = F;
                        break;
                    case 26:
                        setClasificacion(clasificaciones.cadena);
                        if (c == '\'')
                        {
                            estado = 27;
                        }
                        else if (FinDeArchivo())
                        {
                            throw new Exception("Error lexico: se espera comilla");
                        }
                        else
                        {
                            estado = 26;
                        }
                        break;
                    case 27:
                        estado = F;
                        break;
                    case 28:
                        setClasificacion(clasificaciones.operadorTernario);
                        estado = F;
                        break;
                    case 29:
                        setClasificacion(clasificaciones.caracter);
                        estado = F;
                        break;
                }

                if (estado >= 0)
                {
                    archivo.Read();

                    if (estado > 0)
                    {
                        palabra += c;
                    }
                }
                /*
                if (estado == E)
                {
                    throw new Exception("ERROR LEXICO.");
                }
                */
            }

            setContenido(palabra);

            switch (palabra)
            {
                case "char":
                case "int":
                case "float":
                    setClasificacion(clasificaciones.tipoDato);
                    break;

                case "private":
                case "public":
                case "protected":
                    setClasificacion(clasificaciones.zona);
                    break;

                case "if":
                case "else":
                case "switch":
                    setClasificacion(clasificaciones.condicion);
                    break;

                case "for":
                case "while":
                case "do":
                    setClasificacion(clasificaciones.ciclo);
                    break;
            }
            bitacora.WriteLine("Token = " + getContenido());
            bitacora.WriteLine("Clasificacion = " + getClasificacion());
        }

        public bool FinDeArchivo()
        {
            return archivo.EndOfStream;
        }
    }
}
