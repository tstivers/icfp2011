using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using log4net;

namespace LTGSimulator
{
    public abstract class LTGReaderWriter
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(LTGReaderWriter));

        protected StreamReader _iStream;
        protected StreamWriter _oStream;

        public void SetStreams(Stream inputStream, Stream outputStream)
        {
            _iStream = new StreamReader(inputStream);
            _oStream = new StreamWriter(outputStream);
            _oStream.AutoFlush = false;
        }

        public virtual void ExecuteTurn(LTGTurn turn)
        {                   
            _oStream.Write(turn.ToCommandString());
            _oStream.Flush();
        }

        public abstract LTGTurn GetOpponentTurn();

    }
}
