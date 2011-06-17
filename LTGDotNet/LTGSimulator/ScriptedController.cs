using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LTGSimulator
{
    public class ScriptedController : LTGController
    {
        private LTGTurn[] _script;

        public ScriptedController()
        {            
        }

        public override LTGTurn GetTurn()
        {
          return new LTGTurn();
        }
    }
}
