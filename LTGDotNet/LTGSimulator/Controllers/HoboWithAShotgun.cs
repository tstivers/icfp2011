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

        public override void PlayGame()
        {
            GenerateSlotValue(1, 4096 * 2); // buckshot hurts
            GenerateSlotValue(2, 4096); // double tap
            GenerateSlotValue(3, 0); // src1
            GenerateSlotValue(4, 128); // src2         

            Play(5, Cards.attack);
            ComposeGet(5, 3, false);

            Play(6, Cards.attack);
            ComposeGet(6, 4, false);

            Play(7, Cards.get);
            ComposeValue(7, 3, false);

            Play(0, Cards.S);
            ComposeGet(0, 5);
            ComposeGet(0, 7);
            
            for (int i = 0; i < 128; i++)
            {
                Copy(8, 0); // copy the payload
                Play(8, Cards.zero); // materialize it
                ComposeGet(8, 1); // execute it for massive damage
                Play(Cards.succ, 3); // move to the next target
            }

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
