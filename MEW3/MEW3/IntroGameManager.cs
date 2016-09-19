using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MEW3
{
   public class IntroGameManager : GameManager
   {
      public IntroGameManager(string[] map)
         : base(map)
      {
      }
      
      public IntroGameManager(Map map)
         :base(map)
      {
      }

      protected override bool CheckRetry(bool win)
      {
         StopGhostsThread();

         DeathMessage(win);

         return false;
      }
   }
}
