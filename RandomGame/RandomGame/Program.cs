using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RandomGame
{
   class Program
   {
      private enum Direction
      {
         w = 0,
         s = 1,
         a = 2,
         d = 3
      }
      
      static void Main(string[] args)
      {
         var seed = new Random();
         int count = 0;

         string input = "";
         while(true)
         {
            WriteToScreen("Enter a direction (w, s, a, d): ");

            input = ReadFromScreen();
            input = input.ToLower();

            if (input != "w" && input != "s" && input != "a" && input != "d")
            {
               WriteToScreen("Hey dummy, enter a valid direction..");
               continue;
            }

            if (input == ((Direction)seed.Next(4)).ToString())
            {
               WriteToScreen("You lose! :D" + Environment.NewLine +
                             "...but hey! You got like, {0} point(s)!", count.ToString());

               string path = @"HighScore.txt";
               int highscore = count;

               if (File.Exists(path))
               {
                  highscore = int.Parse(File.ReadAllText(path));
               }

               if (highscore >= count)
               {
                  File.WriteAllText(path, highscore.ToString());
                  WriteToScreen("*High Score = {0}", highscore.ToString());
               }
               else
               {
                  File.WriteAllText(path, count.ToString());
                  WriteToScreen("***NEW HIGH SCORE = {0}", count.ToString());
               }

               WriteToScreen("Try your hand at another?? (y/n or a direction) ");

               string res = ReadFromScreen();
               res = res.ToLower();

               if (res == "y" || res == "yes" || res == "w" || res == "s" || res == "a" || res == "d")
               {
                  count = 0;
                  continue;
               }

               break; 
            }

            count++;
         }
      }

      private static void WriteToScreen(string text, params object[] args)
      {
         Console.WriteLine();
         Console.WriteLine(string.Format(text, args));
      }

      private static string ReadFromScreen()
      {
         Console.Write("> ");
         return Console.ReadLine();
      }
   }
}
