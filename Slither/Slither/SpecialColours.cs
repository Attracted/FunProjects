using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slither
{
   public static class SpecialColours
   {
      public const ConsoleColor Player = ConsoleColor.Black;
      public const ConsoleColor Death = ConsoleColor.Red;
      public const ConsoleColor Walls = ConsoleColor.DarkCyan;
      public const ConsoleColor Bit = ConsoleColor.Yellow;
             
             
      public const ConsoleColor Blue         =  ConsoleColor.Blue;
      public const ConsoleColor DarkBlue     =  ConsoleColor.DarkBlue;
             
      public const ConsoleColor Cyan         =  ConsoleColor.Cyan;
      public const ConsoleColor DarkCyan     =  ConsoleColor.DarkCyan;
             
      public const ConsoleColor Green        =  ConsoleColor.Green;
      public const ConsoleColor DarkGreen    =  ConsoleColor.DarkGreen;
             
      public const ConsoleColor Magenta      =  ConsoleColor.Magenta;
      public const ConsoleColor DarkMagenta  =  ConsoleColor.DarkMagenta;
             
      public const ConsoleColor Red          =  ConsoleColor.Red;
      public const ConsoleColor DarkRed      =  ConsoleColor.DarkRed;
             
      public const ConsoleColor Yellow       =  ConsoleColor.Yellow;
      public const ConsoleColor DarkYellow   =  ConsoleColor.DarkYellow;
             
      public const ConsoleColor White        =  ConsoleColor.White;
      public const ConsoleColor Gray         =  ConsoleColor.Gray;
      public const ConsoleColor Black        =  ConsoleColor.Black;
             
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