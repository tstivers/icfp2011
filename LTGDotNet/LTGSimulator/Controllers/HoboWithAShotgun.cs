using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        // always attacks from slot 0
        public void GenerateAttacker(int destSlot, int targetSlotSlot, int amtSlot, int[] tempSlots)
        {
            Play(tempSlots[0], Cards.attack);
            Play(tempSlots[0], Cards.zero);
            ComposeGet(tempSlots[0], targetSlotSlot, false);

            ComposeGet(tempSlots[1], amtSlot, false);

            Play(tempSlots[2], Cards.S);
            ComposeGet(tempSlots[2], tempSlots[0]);
            ComposeGet(tempSlots[2], tempSlots[1]);

            Play(Cards.put, tempSlots[0]);
            ComposeGet(tempSlots[0], destSlot, false);

            Play(destSlot, Cards.S);
            ComposeGet(destSlot, tempSlots[2]);
            ComposeGet(destSlot, tempSlots[0]);

            for(int i = 0; i < 3; i++)
                Play(Cards.put, i);
        }

        public override void PlayGame()
        {
            int attackDmgSlot = 0;
            int attackTargetSlot = 1;
            int healAmtSlot = 2;
            int heal1Slot = 4;
            int heal0Slot = 8;            
            int attackSlot = 16;

            GenerateHealer(heal1Slot, 0, 1, healAmtSlot, new[] {0, 1});
            GenerateHealer(heal0Slot, 1, 0, healAmtSlot, new[] {0, 1});
            GenerateAttacker(attackSlot, attackTargetSlot, attackDmgSlot, new int[] {0, 1, 2});

            GenerateSlotValue(attackTargetSlot, 0);
            GenerateSlotValue(attackDmgSlot, 4096*4);
            GenerateSlotValue(healAmtSlot, 4096*2);

            Play(heal1Slot, Cards.zero);
            Play(Cards.dbl, healAmtSlot);

            for (int i = 0; i < 10; i++)
            {
                Play(heal0Slot, Cards.zero);
                Play(heal1Slot, Cards.zero);
            }

            Play(Cards.dbl, healAmtSlot);

            for (int i = 0; i < 256; i++)
            {
                int targetSlot = State.LastOpponentTurn.Slot;
                if (_killed[targetSlot]) // we've already killed that slot
                    for (targetSlot = 0; targetSlot < 256 && _killed[targetSlot]; targetSlot++) ; // find the lowest living slot
                
                GenerateSlotValue(attackTargetSlot, 255 - targetSlot);
                Play(attackSlot, Cards.zero);
                _killed[targetSlot] = true;

                for (int j = 0; j < 5; j++)
                {
                    Play(heal0Slot, Cards.zero);
                    Play(heal1Slot, Cards.zero);
                }
            }

            // now just spin ressing slots until the game is over
            var rng = new Random();
            while (true)
            {
                for (int i = 0; i < 256; i++)
                    ResSlot(rng.Next(1, 255), i);
            }
        }
    }
}
