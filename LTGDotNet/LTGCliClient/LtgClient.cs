using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LtgSimulator;
using System.IO;
using log4net;
using log4net.Repository;
using log4net.Appender;
using LtgSimulator.Controllers;

namespace LTGCliClient
{
    class LtgClient
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(LtgClient));

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
             if (File.Exists("LTGCliClient.exe.config"))
                log4net.Config.XmlConfigurator.Configure();
            
            if (args.Count() < 2)
            {     
                log.Fatal("usage: LTGCliClient.exe <playernum> <controllerclass> [controller args]");
                return;
            }

            InitializeLogFile(String.Format("player{0}.log", args[0]));

            log.Debug("----------- app starting ------------");
            log.Debug("Command line: " + String.Join(" ", args));
            
            var cType = Type.GetType("LtgSimulator.Controllers." + args[1] + ", LTGSimulator");
          
            var ltgReaderWriter = new LtgReaderWriter();
            var ltgController = (LtgControllerBase)Activator.CreateInstance(cType);
            ltgController.Init(int.Parse(args[0]), args);

            Stream standardInput = Console.OpenStandardInput();
            Stream standardOutput = Console.OpenStandardOutput();

            ltgReaderWriter.SetStreams(standardInput, standardOutput);
            ltgController.ReaderWriter = ltgReaderWriter;

            try
            {
                ltgController.PlayGame();
            }
            catch (GameOverException)
            {                
            }
        }
    }
}
