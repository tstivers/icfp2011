using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LTGSimulator
{
    class NoopController : LTGController
    {
        public override LTGTurn GetTurn()
        {
            log.Debug("sending noop");
            return new LTGTurn(0, LTGTurn.Cards.I);
        }
    }
}
