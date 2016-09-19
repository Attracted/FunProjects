using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TechnoTennis
{
   public class IntroGameManager : GameManager
   {
      public IntroGameManager()
         : base()
      {
      }

      protected override bool CheckRetry()
      {
         StopTimerThread();
         //DeathMessage();

         return false;
      }
   }
}
