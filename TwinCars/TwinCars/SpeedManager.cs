using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwinCars
{
   public class SpeedManager
   {
      public int BlockCreateSpeed { get; set; }

      public SpeedManager(int createSpeed)
      {
         BlockCreateSpeed = createSpeed;
      }

      public void SetSpeeds(int s)
      {
         BlockCreateSpeed = 50 - 5 * (s / 25);
      }

      public float BlockMoveSpeed(int s)
      {
         return 3 + s / 25;
      }
   }
}
