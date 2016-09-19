using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clabbers
{
   public class OpponentsTurnEventArgs : EventArgs
   {
      public Player Player { get; private set; }

      public OpponentsTurnEventArgs(Player player)
      {
         Player = player;
      }
   }
}
