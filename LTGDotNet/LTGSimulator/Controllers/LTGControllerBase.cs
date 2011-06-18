using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using LtgSimulator.GameState;

namespace LtgSimulator.Controllers
{
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

    public struct LtgTurn
    {
        public enum ApplicationType
        {
            Left,
            Right
        }

        public ApplicationType Type;
        public Cards Card;
        public int Slot;

        public LtgTurn(Cards card, int slot)
        {
            Type = ApplicationType.Left;
            Card = card;
            Slot = slot;
        }

        public LtgTurn(int slot, Cards card)
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

        public static LtgTurn Parse(string s)
        {
            int slot;
            var args = s.Split(new char[] { ' ' });
            return int.TryParse(args[0], out slot) ?
                new LtgTurn(slot, (Cards)Enum.Parse(typeof(Cards), args[1])) :
                new LtgTurn((Cards)Enum.Parse(typeof(Cards), args[0]), int.Parse(args[1]));
        }
    }

    public abstract class LtgControllerBase
    {
        public LtgReaderWriter ReaderWriter { get; set; }
        private static readonly ILog Log = LogManager.GetLogger(typeof(LtgControllerBase));
        protected LtgGameState State;
        protected int ProponentId;

        public void Play(Cards card, int slot)
        {
            Play(new LtgTurn(card, slot));
        }

        public void Play(int slot, Cards card)
        {
            Play(new LtgTurn(slot, card));
        }

        public void Play(LtgTurn turn)
        {
            if (ProponentId == 0) // we move first
            {
                State.ApplyProponentTurn(turn);
                ReaderWriter.ExecuteTurn(turn);
                Log.DebugFormat("      we played: {0}", turn);

                var opponentTurn = ReaderWriter.GetOpponentTurn();
                State.ApplyOpponentTurn(opponentTurn);
                Log.DebugFormat("opponent played: {0}", opponentTurn);
            }
            else // opponent goes first
            {
                var opponentTurn = ReaderWriter.GetOpponentTurn();
                State.ApplyOpponentTurn(opponentTurn);
                Log.DebugFormat("opponent played: {0}", opponentTurn);

                ReaderWriter.ExecuteTurn(turn);
                State.ApplyProponentTurn(turn);
                Log.DebugFormat("      we played: {0}", turn);
            }
        }

        // implementation for dump controllers that don't track their own state
        public virtual void PlayGame()
        {  
            while (true)
                Play(GetTurn());
        }

        // move generator for controllers that don't track state
        public virtual LtgTurn GetTurn()
        {
            throw new NotImplementedException();
        }

        // should be a constructor but we're limited by the dynamic creation of the class
        public virtual void Init(int playerId, string[] args)
        {
            ProponentId = playerId;
            State = new LtgGameState(playerId);
        }
    }
}
