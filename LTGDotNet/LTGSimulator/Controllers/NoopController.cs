using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LTGSimulator
{
    class NoopController : LTGControllerBase
    {
        public override LTGTurn GetTurn()
        {            
            return new LTGTurn(0, LTGTurn.Cards.I);
        }
    }
}
