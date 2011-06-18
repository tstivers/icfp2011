using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using log4net;

namespace LTGSimulator
{
    public class LTGReaderWriter
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

        public virtual LTGTurn GetOpponentTurn()
        {

            String p1 = _iStream.ReadLine();
            String p2 = _iStream.ReadLine();
            String p3 = _iStream.ReadLine();

            if (p1 == null || p2 == null || p3 == null)
            {
                throw new GameOverException();
            }

            return p1 == "1" ?
                new LTGTurn((LTGTurn.Cards)Enum.Parse(typeof(LTGTurn.Cards), p2), int.Parse(p3)) :
                new LTGTurn(int.Parse(p2), (LTGTurn.Cards)Enum.Parse(typeof(LTGTurn.Cards), p3));
        }

       
    }

    internal class GameOverException : Exception
    {
    }
}
