using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LTGSimulator
{
    public class ScriptedController : LTGControllerBase
    {
        private LTGTurn[] _script;
        private int _index = 0;

        public ScriptedController()
        {
        }

        public override LTGTurn GetTurn()
        {
            if (_script == null)
                LoadScript(String.Format("player{0}.txt", _playerNum));
            if (_index >= _script.Length)
                _index = 0;
            return _script[_index++];
        }

        protected void LoadScript(String fileName)
        {
            log.DebugFormat("loading script from file {0}", fileName);
            if (!File.Exists(fileName))
            {
                log.FatalFormat("unable to load script file {0}", fileName);
                throw new GameOverException();               
            }

            var r = new StreamReader(fileName);
            var turns = new List<LTGTurn>();

            for(string line = r.ReadLine(); line != null; line = r.ReadLine())
            {
                if (line.StartsWith("#") || line.Trim().Length == 0) // ignore comments and blank lines
                    continue;

                turns.Add(LTGTurn.Parse(line));
                log.DebugFormat("[{0}] {1}", turns.Count - 1, turns.Last());
            }

            _script = turns.ToArray();
        }
    }
}
