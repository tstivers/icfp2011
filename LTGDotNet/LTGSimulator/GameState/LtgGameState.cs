using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LtgSimulator.Controllers;

namespace LtgSimulator.GameState
{
    public class Slot
    {
        private int _vitality = 10000;
        public int Vitality
        {
            get
            {
                return _vitality;
            }
            set
            {
                _vitality = value;
            }
        }

        public int Index { get; private set; }

        public bool IsAlive
        {
            get
            {
                return _vitality > 0;
            }
        }

        public void Damage(int hp)
        {
            _vitality -= hp;
            if (_vitality < -1) _vitality = -1;
        }

        public void Heal(int hp)
        {
            if (_vitality <= 0)
                return;
            _vitality += hp;
        }

        public int Value { get; set; }

        // String if/until someone writes the emulator 
        public string Function { get; set; }

        public bool IsValue { get { return !IsFunction; } }

        public bool IsFunction { get { return Function != null; } }

        public Slot(int index)
        {
            Index = index;
        }
    }

    public class LtgGameState
    {
        protected int ProponentId;
        protected int OpponentId;
        private readonly Slot[][] _state = new Slot[2][];
        private readonly LtgTurn[] _lastTurn = new LtgTurn[2];

        public LtgTurn LastOpponentTurn { get { return _lastTurn[OpponentId]; } }      
        public Slot[] ProponentSlot { get { return _state[ProponentId]; } }
        public Slot[] OpponentSlot { get { return _state[OpponentId]; } }
        public int Turn { get; private set; }

        public LtgGameState(int proponentId)
        {
            ProponentId = proponentId;
            OpponentId = proponentId == 0 ? 1 : 0;
            
            for (int x = 0; x < 2; x++)
            {
                _state[x] = new Slot[256];
                for (int i = 0; i < 256; i++)
                    _state[x][i] = new Slot(i);
            }
        }
       
        public void ApplyProponentTurn(LtgTurn turn)
        {
            _lastTurn[ProponentId] = turn;
            ApplyTurn(turn, ProponentSlot, OpponentSlot);
            if (ProponentId == 1)
            {
                Turn++;
                log4net.GlobalContext.Properties["turn"] = Turn;
            }
        }

        public void ApplyOpponentTurn(LtgTurn turn)
        {
            _lastTurn[OpponentId] = turn;
            ApplyTurn(turn, OpponentSlot, ProponentSlot);
            if (OpponentId == 1)
            {
                Turn++;
                log4net.GlobalContext.Properties["turn"] = Turn;
            }
        }

        protected void ApplyTurn(LtgTurn turn, Slot[] proponent, Slot[] opponent)
        {
            // TODO: emulator goes here :)
        }
    }
}
