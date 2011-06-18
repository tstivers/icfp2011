using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using log4net;
using LtgSimulator.Controllers;

namespace LtgSimulator
{
    public class LtgReaderWriter
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(LtgReaderWriter));

        protected StreamReader _iStream;
        protected StreamWriter _oStream;

        public void SetStreams(Stream inputStream, Stream outputStream)
        {
            _iStream = new StreamReader(inputStream);
            _oStream = new StreamWriter(outputStream);
            _oStream.AutoFlush = false;
        }

        public virtual void ExecuteTurn(LtgTurn turn)
        {                   
            _oStream.Write(turn.ToCommandString());
            _oStream.Flush();
        }

        public virtual LtgTurn GetOpponentTurn()
        {

            String p1 = _iStream.ReadLine();
            String p2 = _iStream.ReadLine();
            String p3 = _iStream.ReadLine();

            if (p1 == null || p2 == null || p3 == null)
            {
                throw new GameOverException();
            }

            return p1 == "1" ?
                new LtgTurn((Cards)Enum.Parse(typeof(Cards), p2), int.Parse(p3)) :
                new LtgTurn(int.Parse(p2), (Cards)Enum.Parse(typeof(Cards), p3));
        }

       
    }

    public class GameOverException : Exception
    {
    }
}
