using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    class Inventory
    {
        List<Item> inventory = new List<Item>();
        Item[] items;

        //Konstruktor
        public Inventory(Item[] items)
        {
            this.items = items;
        }


        //Kombinerar föremål i inventoryn
        public void combineItem(Item item1, Item item2)
        {
            //Kan båda kombineras?
            if (item1.isCombinable && item2.isCombinable)
            {
                //Har de samma komineringsnummer?
                if (item1.combinedItemInt == item2.combinedItemInt)
                {
                    //Tar bort de föremål som man kombinerar och lägger till det nya föremålet
                    addItem(items[item1.combinedItemInt]);
                    removeItem(item1);
                    removeItem(item2);
                }
            }
        }
        //Lägger till föremål i inventory
        public void addItem(Item item)
        {
            if (item.isPickable)
                inventory.Add(item);
        }

        //Tar bort föremål från inventory
        public void removeItem(Item item)
        {
            inventory.Remove(item);
        }

    }
}
