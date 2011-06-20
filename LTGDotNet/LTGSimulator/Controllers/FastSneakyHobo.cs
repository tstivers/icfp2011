using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace LtgSimulator.Controllers
{
    class FastSneakyHobo : RutgerHauerBase
    {                        
        public override void PlayGame()
        {
            int attackTargetSlot = 1;
            int healAmtSlot = 2;
            int healSlot = 4;
            int attackSlot = 16;

            GenerateCompoundHealer(healSlot, 0, 1, healAmtSlot, new[] {0, 1, 2});
            GenerateAttacker(attackSlot, attackTargetSlot, 4096*4, new[] {0, 1, 2});

            GenerateChain(5, healSlot, healSlot);
            GenerateChain(6, 5, 5);
            GenerateChain(7, 6, 6);
            GenerateChain(8, 7, 7);
            GenerateRepeater(9, 8, new[] {0});
            GenerateChain(10, 8, attackSlot);

            GenerateRepeater(11, 10, new[] {0});

            GenerateSlotValue(healAmtSlot, 4096*2);

            for (int i = 0; i < 5; i++)
                Play(9, Cards.zero);

            Play(Cards.dbl, healAmtSlot);
            Play(Cards.dbl, healAmtSlot);

            // sneaky: destroy whatever slot they are currently working with
            GenerateSlotValue(attackTargetSlot, 255 - State.LastOpponentTurn.Slot); 
            Play(11, Cards.zero);            

            // and then walk their entire stack
            GenerateSlotValue(attackTargetSlot, 0);            
            for (int i = 0; i < 256; i++)
            {
                Play(11, Cards.zero);
                Play(Cards.succ, attackTargetSlot);
            }

            var rng = new Random();
            while (true)
            {
                // now just spin ressing slots until the game is over             
                for (int i = 0; i < 256; i++)
                    ResSlot(rng.Next(17, 255), i);

                // sneaky: try to pop whatever they're working on now
                GenerateSlotValue(attackTargetSlot, 255 - State.LastOpponentTurn.Slot);
                Play(11, Cards.zero);   
            }
        }
    }
}
