using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LtgSimulator.GameState;

namespace LtgSimulator.Controllers
{
    class RutgerHauerBase : LtgControllerBase
    {
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

        protected void Copy(int destSlot, int srcSlot)
        {
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

            if (value == 1)
                return;

            int dbls = (int)Math.Floor(Math.Log(value, 2.0));
            int incs = value - (int)Math.Pow(2, dbls);
            for (var i = 0; i < dbls; i++)
                Play(Cards.dbl, slot);
            for (var i = 0; i < incs; i++)
                Play(Cards.succ, slot);
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
                var dbls = (int)Math.Floor(Math.Log(value, 2.0));
                int incs = value - (int)Math.Pow(2, dbls);

                for (var i = 0; i < incs; i++)
                    Compose(slot, Cards.succ);

                for (int i = 0; i < dbls; i++)
                    Compose(slot, Cards.dbl);
                
                Compose(slot, Cards.succ);                
            }

            // materialize the value
            if(materialize)
                Play(slot, Cards.zero);
        }
    }
}
