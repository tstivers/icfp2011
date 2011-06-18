using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LTGSimulator.GameState
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
    }

    public class GameState
    {
        protected int ProponentId;        
        protected int OpponentId;
        private readonly Slot[][] _state = new Slot[2][];
        private readonly LTGTurn[] _lastTurn = new LTGTurn[2];

        public LTGTurn LastOpponentTurn { get { return _lastTurn[OpponentId]; } }      
        public Slot[] ProponentSlot { get { return _state[ProponentId]; } }
        public Slot[] OpponentSlot { get { return _state[OpponentId]; } }

        public GameState(int proponentId)
        {
            ProponentId = proponentId;
            OpponentId = proponentId == 0 ? 1 : 0;
            _state[0] = new Slot[256];
            _state[1] = new Slot[256];
        }
       
        public virtual void ApplyProponentTurn(LTGTurn turn)
        {
            _lastTurn[ProponentId] = turn;
            ApplyTurn(turn, ProponentSlot, OpponentSlot);
        }

        public virtual void ApplyOpponentTurn(LTGTurn turn)
        {
            _lastTurn[OpponentId] = turn;
            ApplyTurn(turn, OpponentSlot, ProponentSlot);
        }

        protected virtual void ApplyTurn(LTGTurn turn, Slot[] proponent, Slot[] opponent)
        {
            // TODO: emulator goes here :)
        }
    }
}
