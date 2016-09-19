using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Slither
{
   public static class GlobalValues
   {
      public const int WINDOW_WIDTH = 19;
      public const int WINDOW_HEIGHT = 21;

      public const int PROMPT_LINE = 16;
      public const int CHOICES_LINE = 17;

      public static Rectangle BORDER_LOCATION = new Rectangle(1, 1, 17, 15);
      public static Rectangle PATH_LOCATION = new Rectangle(1, 3, 14, 9); //Printed inside border

      public const ConsoleColor BORDER_FORECOLOUR = SpecialColours.Walls;
      public const ConsoleColor BORDER_BACKCOLOUR = ConsoleColor.DarkGreen;

   }
}
