using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace LTGSimulator
{
    public struct LTGTurn
    {
        public enum ApplicationType
        {
            Left,
            Right
        }

        public enum Cards
        {
            I,
            zero,
            succ,
            dbl,
            get,
            put,
            S,
            K,
            inc,
            dec,
            attack,
            help,
            copy,
            revive,
            zombie
        }

        public ApplicationType Type;
        public Cards Card;
        public int Slot;

        public LTGTurn(Cards card, int slot)
        {
            Type = ApplicationType.Left;
            Card = card;
            Slot = slot;
        }

        public LTGTurn(int slot, Cards card)
        {
            Type = ApplicationType.Right;
            Slot = slot;
            Card = card;
        }

        public string ToCommandString()
        {
            return Type == ApplicationType.Left ?
                string.Format("1\n{0}\n{1}\n", Card, Slot) :
                string.Format("2\n{0}\n{1}\n", Slot, Card);

        }

        public override string ToString()
        {
            return Type == ApplicationType.Left ? 
                string.Format("{0} {1}", Card, Slot) : 
                string.Format("{0} {1}", Slot, Card);
        }
    }

    public abstract class LTGController
    {
        public LTGReaderWriter ReaderWriter { get; set; }
        private bool _gameInProgress;
        protected static readonly ILog log = LogManager.GetLogger(typeof(LTGController));
        protected int _currentTurn;

        public void PlayGame(bool moveFirst)
        {
            _gameInProgress = true;
            _currentTurn = 0;

            try
            {
                while (_gameInProgress)
                {
                    if (moveFirst)
                    {
                        var turn = GetTurn();
                        ReaderWriter.ExecuteTurn(turn);
                        log.DebugFormat("      we played: {0}", turn);

                        var opponentTurn = ReaderWriter.GetOpponentTurn();
                        log.DebugFormat("opponent played: {0}", opponentTurn);
                    }
                    else
                    {
                        var opponentTurn = ReaderWriter.GetOpponentTurn();
                        log.DebugFormat("opponent played: {0}", opponentTurn);
                        
                        var turn = GetTurn();
                        ReaderWriter.ExecuteTurn(turn);
                        log.DebugFormat("      we played: {0}", turn);
                    }
                    _currentTurn++;
                    log.DebugFormat("---- turn {0} ----", _currentTurn);
                }
            }
            catch(GameOverException)
            {
                _gameInProgress = false;
            }
        }

        public abstract LTGTurn GetTurn();

    }
}
