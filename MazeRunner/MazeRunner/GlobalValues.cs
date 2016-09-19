using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MazeRunner
{
   public static class GlobalValues
   {
      public const int WINDOW_WIDTH = 30;
      public const int WINDOW_HEIGHT = 37;

      public const int PROMPT_LINE = 33;
      public const int CHOICES_LINE = 34;

      public static Rectangle BORDER_LOCATION = new Rectangle(1, 1, 29, 31);
   }
}
