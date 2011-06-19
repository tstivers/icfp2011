using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using LtgSimulator.GameState;

namespace LtgSimulator.Controllers
{    
    class BlindFury : RutgerHauerBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LtgControllerBase));
        private bool[] _killed = new bool[256];

        public BlindFury()
        {
            // don't think this is necessary in c# but meh
            for (int i = 0; i < 256; i++)
                _killed[i] = false;
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
