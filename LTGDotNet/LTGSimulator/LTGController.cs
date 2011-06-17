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
            return Type == ApplicationType.Left ? "2\n" + Card.ToString() + "\n" + Slot + "\n" : "1\n" + Card.ToString() + "\n" + Slot + "\n";
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

            while (_gameInProgress)
            {
                if (moveFirst)
                {
                    LTGTurn turn = GetTurn();
                    ReaderWriter.ExecuteTurn(turn);
                    LTGTurn opponentTurn = ReaderWriter.GetOpponentTurn();
                }
                else
                {
                    LTGTurn opponentTurn = ReaderWriter.GetOpponentTurn();
                    LTGTurn turn = GetTurn();
                    ReaderWriter.ExecuteTurn(turn);
                }
                _currentTurn++;
                log.Debug("Done with turn " + _currentTurn);
            }
        }

        public abstract LTGTurn GetTurn();

    }
}
