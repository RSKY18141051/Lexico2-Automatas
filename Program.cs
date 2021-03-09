using System;

namespace Lexico2
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (Lexico l = new Lexico())
                {
                    while (!l.FinDeArchivo())
                    {
                        l.NextToken();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
