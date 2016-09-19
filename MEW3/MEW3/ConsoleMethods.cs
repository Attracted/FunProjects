using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MEW3
{
   public static class ConsoleMethods
   {
      public static string PromptSelection(string prompt, List<string> choices)
      {
         int choicesLen = choices.Count;
         int lastChoice = choicesLen - 1;

         if (choicesLen == 0)
         {
            return "";
         }

         if (choicesLen == 1)
         {
            return choices[0];
         }

         int maxLen = choices.Max(e => e.Length);
         int longestLen = prompt.Length > maxLen ? prompt.Length : maxLen;
         longestLen += 2;

         FitConsoleToText(choices, longestLen);

         int choiceLine = 0;
         string ret = "";

         while (true)
         {
            Console.Clear();
            Console.WriteLine(prompt);
            Console.WriteLine();
            for (int i = 0; i < choicesLen; i++)
            {
               if (i == choiceLine)
               {
                  ret = choices[i];
                  Console.WriteLine(string.Format("> {0}", choices[i]));
               }
               else
               {
                  Console.WriteLine(string.Format(choices[i]));
               }
            }

            while (Console.KeyAvailable)
            {
               Console.ReadKey(true);
            }
            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.UpArrow || key == ConsoleKey.LeftArrow)
            {
               if (choiceLine != 0)
               {
                  choiceLine--;
               }
            }
            else if (key == ConsoleKey.DownArrow || key == ConsoleKey.RightArrow)
            {
               if (choiceLine != lastChoice)
               {
                  choiceLine++;
               }
            }
            else if (key == ConsoleKey.Enter)
            {
               break;
            }
            else if (key == ConsoleKey.Backspace || key == ConsoleKey.Delete)
            {
               ret = choices[lastChoice];
               break;
            }
            else if (key == ConsoleKey.Home)
            {
               choiceLine = 0;
            }
            else if (key == ConsoleKey.End)
            {
               choiceLine = lastChoice;
            }
            else if (key == ConsoleKey.PageUp)
            {
               choiceLine = choiceLine <= 5 ? 0 : choiceLine - 5;
            }
            else if (key == ConsoleKey.PageDown)
            {
               choiceLine = choiceLine >= lastChoice - 5 ? lastChoice : choiceLine + 5;
            }
         }

         return ret;
      }
      public static string PromptSelection(int lineNum, List<string> choices)
      {
         int choicesLen = choices.Count;
         if (choicesLen == 0)
         {
            return "";
         }

         int realLen = (choicesLen - 1) * 4;
         int lastChoice = choicesLen - 1;

         for (int i = 0; i < choicesLen; i++)
         {
            realLen += choices[i].Length;
         }

         //int width = Console.WindowWidth;
         //int height = Console.WindowHeight;
         //SetConsoleSize(realLen > width ? realLen : width, lineNum > height ? lineNum : height);

         int choiceIndex = 0;
         string ret = "";

         while (true)
         {
            ConsoleMethods.WriteText(new string(' ', Console.WindowWidth), 0, lineNum);

            //string[] promptLine = new string[realLen];
            string promptLine = "";

            for (int i = 0; i < choicesLen; i++)
            {
               string prefix = "";

               if (i > 0)
               {
                  prefix += "  ";
               }

               if (i == choiceIndex)
               {
                  ret = choices[i];
                  promptLine += string.Format("{0}> {1}", prefix, choices[i]);
               }
               else
               {
                  promptLine += string.Format("{0}  {1}", prefix, choices[i]);
               }
            }

            int startLoc = (Console.WindowWidth - promptLine.Length - 2) / 2;

            WriteText(promptLine, startLoc, lineNum);

            while (Console.KeyAvailable)
            {
               Console.ReadKey(true);
            }
            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.UpArrow || key == ConsoleKey.LeftArrow)
            {
               if (choiceIndex != 0)
               {
                  choiceIndex--;
               }
            }
            else if (key == ConsoleKey.DownArrow || key == ConsoleKey.RightArrow)
            {
               if (choiceIndex != lastChoice)
               {
                  choiceIndex++;
               }
            }
            else if (key == ConsoleKey.Enter)
            {
               break;
            }
            else if (key == ConsoleKey.Backspace || key == ConsoleKey.Delete)
            {
               ret = choices[lastChoice];
               break;
            }
            else if (key == ConsoleKey.Home)
            {
               choiceIndex = 0;
            }
            else if (key == ConsoleKey.End)
            {
               choiceIndex = lastChoice;
            }
            else if (key == ConsoleKey.PageUp)
            {
               choiceIndex = choiceIndex <= 5 ? 0 : choiceIndex - 5;
            }
            else if (key == ConsoleKey.PageDown)
            {
               choiceIndex = choiceIndex >= lastChoice - 5 ? lastChoice : choiceIndex + 5;
            }
            else if (key == ConsoleKey.Escape)
            {
               ret = null;
               break;
            }
         }

         return ret;
      }

      public static void FitConsoleToText(string text)
      {
         ConsoleMethods.SetConsoleSize(text.Length + 2, 3);
      }
      public static void FitConsoleToText(string[] text, int minLength = 0)
      {
         for (int i = 0; i < text.Length; i++)
         {
            int len = text[i].Length;

            if (len > minLength)
            {
               minLength = len;
            }
         }

         ConsoleMethods.SetConsoleSize(minLength + 2, text.Length + 3);
      }
      public static void FitConsoleToText(List<string> text, int minLength = 0)
      {
         for (int i = 0; i < text.Count; i++)
         {
            int len = text[i].Length;

            if (len > minLength)
            {
               minLength = len;
            }
         }

         ConsoleMethods.SetConsoleSize(minLength + 2, text.Count + 3);
      }

      public static void CentreText(string text, int line)
      {
         int startLoc = (Console.WindowWidth - text.Length) / 2;

         WriteText(text, startLoc, line);
      }
      public static void CentreText(string text)
      {
         int startLoc = (Console.WindowWidth - text.Length) / 2;

         WriteText(text, startLoc, Console.CursorTop);
      }
      public static void WriteText(string text, int column, int line)
      {
         Console.SetCursorPosition(column, line);
         Console.WriteLine(text);
      }
      public static void WriteText(char character, int column, int line)
      {
         Console.SetCursorPosition(column, line);
         Console.Write(character);
      }

      public static void SetConsoleSize(int width, int height)
      {
         try
         {
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);
         }
         catch (IOException)
         {
         }
      }
   }
}
