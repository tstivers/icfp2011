using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LTGSimulator;
using System.IO;

namespace LTGCliClient
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() != 2)
            {
                
                return;
            }                

            var ltgReaderWriter = (LTGReaderWriter)Activator.CreateInstance(Type.GetType(args[1]));
            var ltgController = (LTGController) Activator.CreateInstance(Type.GetType(args[2]));

            Stream standardInput = Console.OpenStandardInput();
            Stream standardOutput = Console.OpenStandardOutput();

            ltgReaderWriter.SetStreams(standardInput, standardOutput);
            ltgController.ReaderWriter = ltgReaderWriter;
        }
    }
}
