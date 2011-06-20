using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LtgSimulator.Controllers
{
    class RandomHobo : RutgerHauerBase
    {
        private bool[] _killed = new bool[256];

        public override void PlayGame()
        {
            bool punt = true;
            while (true)
            {
                Random r = new Random();

                int offset = r.Next(0, 16);
                int startOffset = offset;
                int healSlot = offset++;
                int attackSlot = offset++;
                int attackTargetSlot = offset++;
                int healAmtSlot = offset++;
                int healSrc = offset++;
                int healTgt = offset++;
                int healer = offset++;
                int attacker = offset++;

                int[] chains = new int[6];
                for (int i = 0; i < 6; i++)
                    chains[i] = offset++;

                int[] tmps = new int[3];
                for (int i = 0; i < 3; i++)
                    tmps[i] = offset++;

                GenerateCompoundHealer(healSlot, healSrc, healTgt, healAmtSlot, tmps);
                GenerateAttacker(attackSlot, healSrc, attackTargetSlot, 4096*4, tmps);

                GenerateChain(chains[0], healSlot, healSlot);
                GenerateChain(chains[1], chains[0], chains[0]);
                GenerateChain(chains[2], chains[1], chains[1]);
                GenerateChain(chains[3], chains[2], chains[2]);

                GenerateRepeater(healer, chains[3], tmps);

                GenerateChain(chains[4], attackSlot, chains[3]);
                GenerateRepeater(attacker, chains[4], tmps);

                GenerateSlotValue(healAmtSlot, 4096*2);

                for (int i = 0; i < 5; i++)
                    Play(healer, Cards.zero);

                Play(Cards.dbl, healAmtSlot);
                Play(Cards.dbl, healAmtSlot);
         
                if(punt)
                {
                    punt = false;
                    continue;
                }

                for (int i = 0; i < 256; i++)
                {
                    int targetSlot = State.LastOpponentTurn.Slot;
                    if (_killed[targetSlot]) // we've already killed that slot
                        for (targetSlot = 0; targetSlot < 256 && _killed[targetSlot]; targetSlot++)
                            ; // find the lowest living slot

                    GenerateSlotValue(attackTargetSlot, 255 - targetSlot);

                    Play(attacker, Cards.zero);
                    _killed[targetSlot] = true;
                }

                // now res slots and start over
                for (int i = 0; i < 256; i++)
                {
                    ResSlot(r.Next(17, 255), i);
                    _killed[i] = false;
                    Play(Cards.put, i);
                } 
            }
        }
    }
}
