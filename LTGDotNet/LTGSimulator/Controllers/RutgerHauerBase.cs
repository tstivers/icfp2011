using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LtgSimulator.GameState;

namespace LtgSimulator.Controllers
{
    class RutgerHauerBase : LtgControllerBase
    {
        public void GenerateCompoundHealer(int destSlot, int slotA, int slotB, int amtSlot, int[] tempSlots)
        {
            Play(tempSlots[0], Cards.help);
            ComposeValue(tempSlots[0], slotA);
            ComposeValue(tempSlots[0], slotB);
            ComposeGet(tempSlots[0], amtSlot, false);

            Play(tempSlots[1], Cards.help);
            ComposeValue(tempSlots[1], slotB);
            ComposeValue(tempSlots[1], slotA);
            ComposeGet(tempSlots[1], amtSlot, false);

            Play(destSlot, Cards.S);
            ComposeGet(destSlot, tempSlots[0]);
            ComposeGet(destSlot, tempSlots[1]);

            for (int i = 0; i < 2; i++)
                Play(Cards.put, tempSlots[i]);
        }

        public void GenerateChain(int destSlot, int srcA, int srcB)
        {
            Play(destSlot, Cards.S);
            ComposeGet(destSlot, srcA);
            ComposeGet(destSlot, srcB);
        }

        public void GenerateRepeater(int destSlot, int srcSlot, int[] tempSlots)
        {
            Play(Cards.put, tempSlots[0]);
            ComposeGet(tempSlots[0], destSlot, false);

            Play(destSlot, Cards.S);
            ComposeGet(destSlot, srcSlot);
            ComposeGet(destSlot, tempSlots[0]);

            Play(Cards.put, tempSlots[0]);
        }

        // always attacks from slot 0
        public void GenerateAttacker(int destSlot, int targetSlotSlot, int amount, int[] tempSlots)
        {
            Play(tempSlots[0], Cards.attack);
            Play(tempSlots[0], Cards.zero);
            ComposeGet(tempSlots[0], targetSlotSlot, false);

            ComposeValue(tempSlots[1], amount, false);

            Play(destSlot, Cards.S);
            ComposeGet(destSlot, tempSlots[0]);
            ComposeGet(destSlot, tempSlots[1]);

            for (int i = 0; i < 3; i++)
                Play(Cards.put, tempSlots[i]);
        }

        public void GenerateAttacker(int destSlot, int srcSlot, int targetSlotSlot, int amount, int[] tempSlots)
        {
            Play(tempSlots[0], Cards.attack);
            ComposeValue(tempSlots[0], srcSlot);            
            ComposeGet(tempSlots[0], targetSlotSlot, false);

            ComposeValue(tempSlots[1], amount, false);

            Play(destSlot, Cards.S);
            ComposeGet(destSlot, tempSlots[0]);
            ComposeGet(destSlot, tempSlots[1]);

            for (int i = 0; i < 3; i++)
                Play(Cards.put, tempSlots[i]);
        }

        protected void Compose(int destSlot, Cards card)
        {
            Play(Cards.K, destSlot);
            Play(Cards.S, destSlot);
            Play(destSlot, card);
        }

        protected void ComposeGet(int destSlot, int srcSlot, bool materialize = true)
        {
            Compose(destSlot, Cards.get);
            ComposeValue(destSlot, srcSlot, materialize);
        }

        protected void Copy(int destSlot, int srcSlot, bool clear = true)
        {
            if(clear)
                Play(Cards.put, destSlot);
            Play(destSlot, Cards.get);
            ComposeValue(destSlot, srcSlot);
        }

        protected void Attack(int srcSlot, int targetSlot, int damage)
        {
            GenerateSlotValue(2, srcSlot);
            GenerateSlotValue(3, 255 - targetSlot);
            GenerateSlotValue(4, damage);

            Play(0, Cards.attack);
            ComposeGet(0, 2);
            ComposeGet(0, 3);
            ComposeGet(0, 4);
        }

        protected void Attack(int from, Slot srcSlotValue, Slot targetSlotValue, Slot damageValue)
        {
            Play(from, Cards.attack);
            ComposeGet(from, srcSlotValue.Index);
            ComposeGet(from, targetSlotValue.Index);
            ComposeGet(from, damageValue.Index);
        }

        protected void ResSlot(int from, int target)
        {
            GenerateSlotValue(from, target);
            Play(Cards.revive, from);
        }

        protected void GenerateSlotValue(int slot, int value)
        {
            Play(Cards.put, slot);
            Play(slot, Cards.zero);

            if (value == 0)
                return;

            Play(Cards.succ, slot);

            var ops = GetValueOps(value);

            while (ops.Count != 0)            
                Play(ops.Pop() ? Cards.dbl : Cards.succ, slot);            
        }      
                
        protected void ComposeValue(int slot, int value, bool materialize = true)
        {
            if (value == 0)
            {
            }
            else if (value == 1)
            {
                Compose(slot, Cards.succ);
            }
            else
            {
                var ops = GetValueOps(value);

                foreach (var op in ops.Reverse())                
                     Compose(slot, op ? Cards.dbl : Cards.succ);                
                           
                Compose(slot, Cards.succ);
            }

            // materialize the value
            if (materialize)
                Play(slot, Cards.zero);
        }

        private Stack<bool> GetValueOps(int value)
        {
            var ops = new Stack<bool>();

            while (value > 1)
            {
                if (value % 2 == 0)
                {
                    value /= 2;
                    ops.Push(true);
                }
                else
                {
                    value--;
                    ops.Push(false);
                }
            }
            return ops;
        }
    }
}
