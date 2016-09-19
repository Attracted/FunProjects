using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextAdventure
{
   public class Item
   {
      public string Name { get; set; }
      public string Description { get; set; }
      public bool Tangible { get; set; }
      public GenericCommands Effect { get; set; }

      public Item() 
      { 
      }

      public Item(string name)
         : this()
      {
         Name = name;
      }

      public Item(string name, string description)
         : this(name)
      {
         Description = description;
      }

      public Item(string name, string description, bool tangible)
         : this(name, description)
      {
         Tangible = tangible;
      }

      public Item(string name, string description, bool tangible, GenericCommands effect)
         : this(name, description, tangible)
      {
         Effect = effect;
      }
   }
}
