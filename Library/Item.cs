using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Library
{
    class Item
    {
        private string searchName;
        private bool isPickable;
        public string name;
        private string description;
        List<string> information = new List<string>();
        
        //Konstruktor
        public Item(String searchName)
        {
            this.searchName = searchName;
            getInformation();

            name = information[0];
            description = information[1];
            isPickable = bool.Parse(information[2]);
            //Osv.
            
        }

        //Lägger in all text i den önskade filen på rätt plats
        private void getInformation()
        {
            StreamReader open = new StreamReader(@"../../../../ProjektArbeteContent/Data/object.fil");
            List<string> data = new List<string>();
            string input = null;
            while ((input = open.ReadLine()) != null)
            {
                data.Add(input);
            }
            open.Close();

            if (stringSearch(data) != -1)
            {
                information.Add(data[stringSearch(data) + 1]);
                //Ändra vilken stränga som ska läggas till /\ när det bestämms
                //Lägg till hur många olika strängar som ska finnas

            }
        }

        private int stringSearch(List<string> data)
        {
            int pos = -1;
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i] == searchName)
                {
                    pos = i;
                }
            }

            return pos;
        }

    }
}
