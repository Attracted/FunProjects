using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Drawing;

namespace GetConsoleUnicodeCharacters
{
   [StructLayout(LayoutKind.Sequential, Pack = 1)]
   public struct ConsoleFont
   {
      public uint Index;
      public short SizeX, SizeY;
   }
   
   class Program
   {
      static void Main(string[] args)
      {
         // Set the window size and title
         Console.Title = "Code Page 437: MS-DOS ASCII Characters";

         for (byte b = 0; b < byte.MaxValue; b++)
         {
            char c = Encoding.GetEncoding(437).GetChars(new byte[] { b })[0];
            switch (b)
            {
               case 8: // Backspace
               case 9: // Tab
               case 10: // Line feed
               case 13: // Carriage return
                  c = '.';
                  break;
            }

            //Console.WriteLine("{0:000} {1}   ", b, c);
            //File.AppendAllText(@"C:\MyProjects\MEW3\Maps\Characters.txt", c.ToString());
            Console.Write(c);

            // 7 is a beep -- Console.Beep() also works
            //if (b == 7) Console.Write(" ");

            //if ((b + 1) % 8 == 0)
            //   Console.WriteLine();
         }
         Console.WriteLine();



         //Console.Clear();
         //Console.SetCursorPosition(0, 0);
         //Console.ForegroundColor = ConsoleColor.Magenta;
         //Console.WriteLine("o o        . .");
         //Console.WriteLine(" )          ) ");
         //Console.WriteLine("___        ###");
         //for (int i = 0; i < 20; i++)
         //{
         //   Console.MoveBufferArea(i + 11, i, 3, 3, i + 12, i + 1,
         //       'x', ConsoleColor.Red, ConsoleColor.White);
         //   Console.MoveBufferArea(i, i, 3, 3, i + 1, i + 1);
         //   Console.Beep((i + 10) * 100, 100);
         //}
         //Console.SetCursorPosition(0, 23);
         //Console.ResetColor();
      }
   }
}
