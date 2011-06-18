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
            return new LtgTurn(255, Cards.I);
        }
    }
}
