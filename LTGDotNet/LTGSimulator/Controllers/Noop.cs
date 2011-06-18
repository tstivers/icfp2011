using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LtgSimulator.Controllers
{
    class Noop : LtgControllerBase
    {
        public override LtgTurn GetTurn()
        {            
            // sneaky
            return new LtgTurn(129, Cards.I);
        }
    }
}
