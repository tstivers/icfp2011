using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace LTGSimulator
{
    class TestReaderWriter : LTGReaderWriter
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(LTGReaderWriter));

        public override LTGTurn GetOpponentTurn()
        {           
            String p1 = _iStream.ReadLine();
            String p2 = _iStream.ReadLine();
            String p3 = _iStream.ReadLine();

            log.Debug(String.Format("read {0} {1} {2}", p1, p2, p3));

            return new LTGTurn(LTGTurn.Cards.I, 0);
        }
    }
}
