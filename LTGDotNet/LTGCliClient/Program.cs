using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LTGSimulator;
using System.IO;
using log4net;
using log4net.Repository;
using log4net.Appender;

namespace LTGCliClient
{
    class Program
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(Program));

        // adapted from http://geekswithblogs.net/wpeck/archive/2009/10/08/setting-log4net-fileappender.file-at-runtime.aspx
        public static void InitializeLogFile(string logFileName)
        {            
            ILoggerRepository repository = LogManager.GetRepository();         
            IAppender[] appenders = repository.GetAppenders();
            
            foreach (IAppender appender in (from iAppender in appenders
                                            where iAppender is FileAppender
                                            select iAppender))
            {
                FileAppender fileAppender = appender as FileAppender;              
                fileAppender.File = logFileName;               
                fileAppender.ActivateOptions();
            }
        }

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            
            if (args.Count() != 3)
            {     
                log.Fatal("wrong number of arguments, exiting");
                return;
            }

            InitializeLogFile(String.Format("player{0}.log", args[2]));

            log.Debug("----------- app starting ------------");
            log.Debug("Command line: " + String.Join(" ", args));

            var rwType = Type.GetType("LTGSimulator." + args[0] + ", LTGSimulator");
            var cType = Type.GetType("LTGSimulator." + args[1] + ", LTGSimulator");

            if (rwType == null)
            {
                log.Fatal("Unable to load type: " + args[0]);
                return;
            }

            if (cType == null)
            {
                log.Fatal("Unable to load type: " + args[1]);
                return;
            }

            var ltgReaderWriter = (LTGReaderWriter)Activator.CreateInstance(rwType);
            var ltgController = (LTGController) Activator.CreateInstance(cType);

            Stream standardInput = Console.OpenStandardInput();
            Stream standardOutput = Console.OpenStandardOutput();

            ltgReaderWriter.SetStreams(standardInput, standardOutput);
            ltgController.ReaderWriter = ltgReaderWriter;

            ltgController.PlayGame(args[2] == "0");
        }
    }
}
