using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using LtgSimulator.GameState;

namespace LtgSimulator.Controllers
{    
    class HoboWithAShotgun : LtgControllerBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LtgControllerBase));
        private bool[] _killed = new bool[256];

        public HoboWithAShotgun()
        {
            // don't think this is necessary in c# but meh
            for (int i = 0; i < 256; i++)
                _killed[i] = false;
        }

        protected void GenerateSlotValue(int slot, int value)
        {
            Play(Cards.put, slot);
            Play(slot, Cards.zero);

            if (value == 0)
                return;

            Play(Cards.succ, slot);

            if (value == 1)
                return;

            int dbls = (int)Math.Floor(Math.Log(value, 2.0));
            int incs = value - (int)Math.Pow(2, dbls);
            for(var i = 0; i < dbls; i++)
                Play(Cards.dbl, slot);
            for (var i = 0; i < incs; i++)
                Play(Cards.succ, slot);
            State.ProponentSlot[slot].Value = value;
        }

        // doesn't handle 0 or 1 correctly
        protected void GenValueGenerator(int slot, int value)
        {
            int generatedValue = 1;

            int dblCount = 0;
            while(generatedValue * 2 < value)
            {
                generatedValue *= 2;
                dblCount++;
            }

            while (generatedValue < value)
            {
                Play(Cards.K, slot);
                Play(Cards.S, slot);
                Play(slot, Cards.succ);
                generatedValue++;
            }

            for (int i = 0; i < dblCount; i++ )
            {
                Play(Cards.K, slot);
                Play(Cards.S, slot);
                Play(slot, Cards.dbl);
                generatedValue *= 2;
            }

            Play(Cards.K, slot);
            Play(Cards.S, slot);
            Play(slot, Cards.succ);
         
            // materialize the value
            Play(slot, Cards.zero);
        }

        protected void MaterializeSlotGetter(int destSlot, int srcSlot)
        {
            Play(Cards.K, destSlot);
            Play(Cards.S, destSlot);
            Play(destSlot, Cards.get);
            GenValueGenerator(destSlot, srcSlot);
        }

        protected void Attack(int srcSlot, int targetSlot, int damage)
        {
            GenerateSlotValue(2, srcSlot);
            GenerateSlotValue(3, 255 - targetSlot);
            GenerateSlotValue(4, damage);

            Play(0, Cards.attack);
            MaterializeSlotGetter(0, 2);
            MaterializeSlotGetter(0, 3);
            MaterializeSlotGetter(0, 4);
        }

        protected void Attack(int from, Slot srcSlotValue, Slot targetSlotValue, Slot damageValue)
        {
            Play(from, Cards.attack);
            MaterializeSlotGetter(from, srcSlotValue.Index);
            MaterializeSlotGetter(from, targetSlotValue.Index);
            MaterializeSlotGetter(from, damageValue.Index);
        }

        protected void ResSlot(int from, int target)
        {
            GenerateSlotValue(from, target);
            Play(Cards.revive, from);
        }

        public override void PlayGame()
        {
            // set up our constants
            GenerateSlotValue(1, 0);
            GenerateSlotValue(2, 128);
            GenerateSlotValue(3, 4096);
            GenerateSlotValue(4, 4096 * 2);

            var rng = new Random();

            // kill the opponnent's slots rarr
            for (int i = 0; i < 128; i++)
            {
                int targetSlot = State.LastOpponentTurn.Slot;
                if (_killed[targetSlot]) // we've already killed that slot
                    for (targetSlot = 0; _killed[targetSlot]; targetSlot++); // find the lowest living slot

                // kill the slot
                GenerateSlotValue(5, 255 - targetSlot);
                Attack(6, State.ProponentSlot[1], State.ProponentSlot[5], State.ProponentSlot[3]);
                Attack(6, State.ProponentSlot[2], State.ProponentSlot[5], State.ProponentSlot[4]);
                Play(Cards.succ, 1);
                Play(Cards.succ, 2);
                _killed[targetSlot] = true; // mark it dead

                // res our slots for next round
                for(var j = 1; j < 7; j++)
                    ResSlot(rng.Next(7, 255), j);
            }
         
            // now just spin ressing slots until the game is over
            while (true)
            {
                for (int i = 0; i < 256; i++ )
                    ResSlot(rng.Next(1, 255), i);
            }
        }
    }
}
