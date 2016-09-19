using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextAdventure
{
   public class Bag : List<Item>
   {      
      public Bag()
      {
      }

      public void OnAddOrRemoveItem(object sender, BagEventArgs e)
      {  
         if (e.AddItem)
         {
            this.Add(e.BagItem);
         }
         else
         {
            this.Remove(e.BagItem);
         }
      }

      public override string ToString()
      {
         string ret = "Inventory:" + Environment.NewLine;

         foreach (Item item in this)
         {
            ret += " " + item.Name;
         }

         return ret;
      }
   }
}
