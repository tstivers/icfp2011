using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace LtgSimulator.Controllers
{
    class HoboWithAShotgun : RutgerHauerBase
    {
        private bool[] _killed = new bool[256];

        public void GenerateHealer(int destSlot, int srcSlot, int targetSlot, int amtSlot, int[] tempSlots)
        {
            Play(tempSlots[0], Cards.help);
            ComposeValue(tempSlots[0], srcSlot);
            ComposeValue(tempSlots[0], targetSlot);
            ComposeGet(tempSlots[0], amtSlot, false);

            ComposeGet(tempSlots[1], destSlot, false);

            Play(destSlot, Cards.S);
            ComposeGet(destSlot, tempSlots[0]);
            ComposeGet(destSlot, tempSlots[1]);

            Play(Cards.put, tempSlots[0]);
            Play(Cards.put, tempSlots[1]);
        }

       

        public override void PlayGame()
        {
            int attackTargetSlot = 1;
            int healAmtSlot = 2;
            int healSlot = 4;
            int attackSlot = 16;

            GenerateCompoundHealer(healSlot, 0, 1, healAmtSlot, new[] { 0, 1, 2 });
            
            GenerateChain(5, healSlot, healSlot);
            GenerateChain(6, 5, 5);
            GenerateChain(7, 6, 6);
            GenerateChain(8, 7, 7);
    
            GenerateRepeater(9, 8, new[] { 0 });

            GenerateAttacker(attackSlot, attackTargetSlot, 4096 * 4, new[] { 0, 1, 2 });
            GenerateChain(10, attackSlot, 8);
            GenerateRepeater(11, 10, new[] {0});

            GenerateSlotValue(healAmtSlot, 4096 * 2);


            for (int i = 0; i < 5; i++)            
                Play(9, Cards.zero);            

            Play(Cards.dbl, healAmtSlot);
            Play(Cards.dbl, healAmtSlot);

            var rng = new Random();

            while (true)
            {
                for (int i = 0; i < 256; i++)
                {
                    int targetSlot = State.LastOpponentTurn.Slot;
                    if (_killed[targetSlot]) // we've already killed that slot
                        for (targetSlot = 0; targetSlot < 256 && _killed[targetSlot]; targetSlot++)
                            ; // find the lowest living slot

                    GenerateSlotValue(attackTargetSlot, 255 - targetSlot);
                    Play(11, Cards.zero);
                    _killed[targetSlot] = true;
                }

                // now ress slots and start over
                for (int i = 0; i < 256; i++)
                {
                    ResSlot(rng.Next(17, 255), i);
                    _killed[i] = false;
                }
            }
        }
    }
}
