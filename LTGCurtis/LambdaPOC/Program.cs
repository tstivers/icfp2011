using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LambdaPOC
{
    class Program
    {
        
        delegate int intDelegate(int identity);
        delegate int dblDelegate(int dbl);
        delegate int succDelegate(int succ);
        


        const int zero = 0;

        static void M(string s)
        {
            Console.WriteLine(s);
        }

        static void Main(string[] args)
        {
                        // C# 3.0. A delegate can be initialized with
            // a lambda expression. The lambda also takes a string
            // as an input parameter (x). The type of x is inferred by the compiler.           
            intDelegate cardI = (x) => { return x; };
            dblDelegate cardDbl = (x) => { return x * 2; };
            succDelegate cardSucc = (x) => { return x + 1; };

            


            // Invoke the delegates.            
            Console.WriteLine(cardI(4));
            Console.WriteLine(cardDbl(4));
            Console.WriteLine(cardSucc(4));
            Console.WriteLine(cardDbl(cardDbl(cardDbl(cardSucc(0)))));

            // Keep console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
