using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using log4net;

namespace LtgSimulator.Controllers
{
    public class ScriptRunner : LtgControllerBase
    {
        private LtgTurn[] _script;
        private int _index = 0;
        private static readonly ILog Log = LogManager.GetLogger(typeof(ScriptRunner));

        public ScriptRunner()
        {
        }

        public override LtgTurn GetTurn()
        {
            if (_script == null)
                LoadScript(String.Format("player{0}.txt", ProponentId));
            if (_index >= _script.Length)
                _index = 0;
            return _script[_index++];
        }

        protected void LoadScript(String fileName)
        {
            Log.DebugFormat("loading script from file {0}", fileName);
            if (!File.Exists(fileName))
            {
                Log.FatalFormat("unable to load script file {0}", fileName);
                throw new GameOverException();               
            }

            var r = new StreamReader(fileName);
            var turns = new List<LtgTurn>();

            for(string line = r.ReadLine(); line != null; line = r.ReadLine())
            {
                if (line.StartsWith("#") || line.Trim().Length == 0) // ignore comments and blank lines
                    continue;

                turns.Add(LtgTurn.Parse(line));
                Log.DebugFormat("[{0}] {1}", turns.Count - 1, turns.Last());
            }

            _script = turns.ToArray();
        }
    }
}
