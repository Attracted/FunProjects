using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeRunner
{
   public static class Colours
   {
      public static ConsoleColor Blue         =  ConsoleColor.Blue;
      public static ConsoleColor DarkBlue     =  ConsoleColor.DarkBlue;

      public static ConsoleColor Cyan         =  ConsoleColor.Cyan;
      public static ConsoleColor DarkCyan     =  ConsoleColor.DarkCyan;

      public static ConsoleColor Green        =  ConsoleColor.Green;
      public static ConsoleColor DarkGreen    =  ConsoleColor.DarkGreen;

      public static ConsoleColor Magenta      =  ConsoleColor.Magenta;
      public static ConsoleColor DarkMagenta  =  ConsoleColor.DarkMagenta;

      public static ConsoleColor Red          =  ConsoleColor.Red;
      public static ConsoleColor DarkRed      =  ConsoleColor.DarkRed;

      public static ConsoleColor Yellow       =  ConsoleColor.Yellow;
      public static ConsoleColor DarkYellow   =  ConsoleColor.DarkYellow;

      public static ConsoleColor White        =  ConsoleColor.White;
      public static ConsoleColor Gray         =  ConsoleColor.Gray;
      public static ConsoleColor Black        =  ConsoleColor.Black;

      public static List<ConsoleColor> ColoursList = new List<ConsoleColor>() {
         DarkBlue,
         Green,
         Magenta,
         DarkYellow,
         White,
         DarkGreen,
         DarkMagenta,
         Gray,
         DarkRed,
         DarkCyan,
      };
   }
}
