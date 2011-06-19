using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LtgSimulator.Controllers
{
    class HoboWithAShotgun : RutgerHauerBase
    {
        public void BuildShotgun(int src1, int src2, int target)
        {
            
        }

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
            int dmgSlot = 4;
            int targetSlot = 1;
            int getSlot = 3;
            int heal1Slot = 8;
            int heal0Slot = 16;
            int healAmtSlot = 7;
            int attackSlot = 5;

            GenerateHealer(heal1Slot, 0, 1, healAmtSlot, new[] { 0, 1 });
            GenerateHealer(heal0Slot, 1, 0, healAmtSlot, new[] { 0, 1 });
            GenerateAttacker(attackSlot, targetSlot, dmgSlot, new int[] { 0, 1, 2 });
                       
            Play(heal1Slot, Cards.zero);
            Play(Cards.dbl, healAmtSlot);

            for (int i = 0; i < 10; i++)
            {
                Play(heal0Slot, Cards.zero);
                Play(heal1Slot, Cards.zero);
            }

            Play(Cards.dbl, healAmtSlot);

            Play(5, Cards.attack);
            Play(5, Cards.zero);
            ComposeGet(5, targetSlot, false);

            ComposeGet(6, dmgSlot, false);

            Play(Cards.put, 0);
            Play(0, Cards.S);
            ComposeGet(0, 5);
            ComposeGet(0, 6);
            
            // slots 0, 1, 2, 3 are available at this point

            Play(Cards.put, getSlot);
            ComposeGet(getSlot, 5, false);
           
            Play(Cards.put, 5);
            Play(5, Cards.S);
            ComposeGet(5, 0);
            ComposeGet(5, getSlot);            

            for (int i = 0; i < 256; i++ )
            {
                Play(attackSlot, Cards.zero);

                Play(Cards.succ, targetSlot);

                for (int j = 0; j < 5; j++)
                {
                    Play(heal0Slot, Cards.zero);
                    Play(heal1Slot, Cards.zero);
                }
            }

            var rng = new Random();
            // now just spin ressing slots until the game is over
            while (true)
            {
                for (int i = 0; i < 256; i++)
                    ResSlot(rng.Next(1, 255), i);
            }

            return;

            GenerateSlotValue(2, 4096 * 2); // buckshot hurts
            GenerateSlotValue(3, 4096); // double tap
            GenerateSlotValue(4, 0); // src1
            GenerateSlotValue(5, 128); // src2         

            Play(6, Cards.attack);
            ComposeGet(6, 4, false);

            Play(7, Cards.attack);
            ComposeGet(7, 5, false);

            Play(8, Cards.get);
            ComposeValue(8, 4, false);

            Play(0, Cards.S);
            ComposeGet(0, 6);
            ComposeGet(0, 8);

            Play(1, Cards.S);
            ComposeGet(1, 7);
            ComposeGet(1, 8);
            
            for (int i = 0; i < 128; i++)
            {
                Copy(9, 0); // copy the payload
                Play(9, Cards.zero); // materialize it
                ComposeGet(9, 2); // execute it for massive damage

                Copy(9, 1);
                Play(9, Cards.zero);
                ComposeGet(9, 3); // double tap

                Play(Cards.succ, 4); // move to the next target
                Play(Cards.succ, 5);
            }

            while(true)
                Play(Cards.succ, 0);

            return;

            Play(Cards.put, 0);
            Play(0, Cards.get);
            ComposeValue(0, 5, false);

            Play(Cards.put, 120);
            Play(120, Cards.get);
            ComposeGet(120, 0);
            Play(120, Cards.zero);

            return;

            GenerateSlotValue(0, 4096 * 2); // buckshot hurts
            GenerateSlotValue(1, 0); // src1
            GenerateSlotValue(2, 128); // src2
            GenerateSlotValue(3, 0); // target

            for (int i = 0; i < 128; i++)
            {
                // barrel one
                Play(4, Cards.attack);
                ComposeGet(4, 1);
                ComposeGet(4, 3);

                // barrel two
                Play(5, Cards.attack);
                ComposeGet(5, 2);
                ComposeGet(5, 3);

                // double barrel
                Play(6, Cards.S);
                ComposeGet(6, 4);
                ComposeGet(6, 5);
                ComposeGet(6, 0);

                // inc everything
                for(int j = 1; j < 4; j++)
                    Play(Cards.succ, j);

                // clear everything
                for(int j = 4; j < 7; j++)
                    Play(Cards.put, j);
            }
        }
    }
}
