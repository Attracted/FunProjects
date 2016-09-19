using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnoTennis
{
   public class Ball : ScreenObject
   {
      public double XPosition;
      public double YPosition;
      public double XVelocity;
      public double YVelocity;
      public Location? PieceToDelete;

      public Ball(ScreenObject bp)
         : base(bp)
      {
         InitializeProperties();
      }
      
      public Ball(Location a, Char c, ConsoleColor colour)
         : base(a, c, colour)
      {
         InitializeProperties();
      }
      public Ball(int x, int y, Char c, ConsoleColor colour)
         : base(x, y, c, colour)
      {
         InitializeProperties();
      }
      
      private void InitializeProperties()
      {
         XPosition = 0;
         YPosition = 0;
         XVelocity = 10;
         YVelocity = 10;
      }
   }
}
