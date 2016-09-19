using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MEW3
{
   public static class Colours
   {
      public static short Blue         =  Convert.ToInt16(ConsoleColor.Blue);
      public static short DarkBlue     =  Convert.ToInt16(ConsoleColor.DarkBlue);

      public static short Cyan         =  Convert.ToInt16(ConsoleColor.Cyan);
      public static short DarkCyan     =  Convert.ToInt16(ConsoleColor.DarkCyan);
      
      public static short Green        =   Convert.ToInt16(ConsoleColor.Green);
      public static short DarkGreen    =   Convert.ToInt16(ConsoleColor.DarkGreen);
      
      public static short Magenta      =  Convert.ToInt16(ConsoleColor.Magenta);
      public static short DarkMagenta  =  Convert.ToInt16(ConsoleColor.DarkMagenta);

      public static short Red          =  Convert.ToInt16(ConsoleColor.Red);
      public static short DarkRed      =  Convert.ToInt16(ConsoleColor.DarkRed);

      public static short Yellow       =  Convert.ToInt16(ConsoleColor.Yellow);
      public static short DarkYellow   =  Convert.ToInt16(ConsoleColor.DarkYellow);

      public static short White        =  Convert.ToInt16(ConsoleColor.White);
      public static short Gray         =  Convert.ToInt16(ConsoleColor.Gray);
      public static short Black        =  Convert.ToInt16(ConsoleColor.Black);

      public static List<short> ColoursList = new List<short>() {
         Green,
         Magenta,
         DarkYellow,
         White,
         DarkGreen,
         DarkMagenta,
         Gray,
         DarkRed,
         DarkCyan,
         DarkBlue,
      };
   }
}
