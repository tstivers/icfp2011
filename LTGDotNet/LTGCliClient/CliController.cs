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
    class CliController
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(CliController));

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
                fileAppender.File = Path.Combine(Directory.GetCurrentDirectory(), logFileName);
                fileAppender.ActivateOptions();
            }
        }

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            
            if (args.Count() < 2)
            {     
                log.Fatal("usage: LTGCliClient.exe <playernum> <controllerclass> [controller args]");
                return;
            }

            InitializeLogFile(String.Format("player{0}.log", args[0]));

            log.Debug("----------- app starting ------------");
            log.Debug("Command line: " + String.Join(" ", args));
            
            var cType = Type.GetType("LTGSimulator." + args[1] + ", LTGSimulator");
          
            var ltgReaderWriter = new LTGReaderWriter();
            var ltgController = (LTGControllerBase) Activator.CreateInstance(cType);
            ltgController.Init(args);

            Stream standardInput = Console.OpenStandardInput();
            Stream standardOutput = Console.OpenStandardOutput();

            ltgReaderWriter.SetStreams(standardInput, standardOutput);
            ltgController.ReaderWriter = ltgReaderWriter;

            ltgController.PlayGame(args[0] == "0");
        }
    }
}
