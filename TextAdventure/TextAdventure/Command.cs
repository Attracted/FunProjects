using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextAdventure
{
   public class Command
   {      
      public string ScreenText { get; private set; }

      public Command(string screenText)
      {
         ScreenText = screenText;
      }
   }
}
