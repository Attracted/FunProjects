using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using TechnoTennis.Properties;

namespace TechnoTennis
{
   public static class ConsoleMethods
   {
      private static readonly object ConsoleWriterLock = new object();

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

         int choiceIndex = 0;
         string ret = "";

         while (true)
         {
            ClearLine(lineNum);

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

            // Centre text assuming no arrow
            int startLoc = (GlobalValues.WINDOW_WIDTH - promptLine.Length - 2) / 2;
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

      public static string PromptHorizontalSelection(string prompt, List<string> choices)
      {
         int choicesLen = choices.Count;
         if (choicesLen == 0)
         {
            return "";
         }

         int lastChoice = choicesLen - 1;
         int choiceIndex = 0;
         string ret = "";

         while (true)
         {
            ret = choices[choiceIndex];

            string[] lines = ParseChoiceLines(prompt, choices, choiceIndex);

            ClearPromptLines();
            CentreChoices(prompt, lines[0], lines[1]);

            while (Console.KeyAvailable)
            {
               Console.ReadKey(true);
            }
            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.UpArrow || key == ConsoleKey.PageUp)
            {
               if (choicesLen > 2)
               {
                  if (choiceIndex == 2 || choiceIndex == 3)
                  {
                     choiceIndex -= 2;
                  }
               }
            }
            else if (key == ConsoleKey.LeftArrow)
            {
               if (choiceIndex == 1 || choiceIndex == 3)
               {
                  choiceIndex -= 1;
               }
            }
            else if (key == ConsoleKey.DownArrow || key == ConsoleKey.PageDown)
            {
               if (choicesLen > 2)
               {
                  if (choiceIndex == 0 || choiceIndex == 1)
                  {
                     choiceIndex += 2;
                  }
               }
            }
            else if (key == ConsoleKey.RightArrow)
            {
               if (choiceIndex == 0 || choiceIndex == 2)
               {
                  choiceIndex += 1;
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
            else if (key == ConsoleKey.Escape)
            {
               ret = null;
               break;
            }
         }

         return ret;
      }

      private static string ParseChoiceLine(string prompt, List<string> choices, int choiceIndex)
      {
         return CreateLineText(choices, choiceIndex, 0, choices.Count);
      }
      private static string[] ParseChoiceLines(string prompt, List<string> choices, int choiceIndex)
      {
         int longestLen = choices[0].Length;
         
         if (choices.Count > 2)
         {
            longestLen = Math.Max(choices[0].Length, choices[2].Length);
         }

         string topChoicesLine = CreateLineText(choices[0], choices[1], longestLen, choiceIndex, 0);
         string bottomChoicesLine = "";

         if (choices.Count > 2)
         {
            bottomChoicesLine = CreateLineText(choices[2], choices[3], longestLen, choiceIndex, 2);
         }

         return new string[] { topChoicesLine, bottomChoicesLine };
      }

      private static string CreateLineText(List<string> choices, int choiceIndex, int startIndex, int finishIndex)
      {
         string choicesLine = "";

         for (int i = startIndex; i < finishIndex; i++)
         {
            string choice = choices[i];

            string prefix = "";

            if (i > startIndex)
            {
               prefix += "  ";
            }

            if (choiceIndex == i)
            {
               choicesLine += string.Format("{0}> {1}", prefix, choice);
            }
            else
            {
               choicesLine += string.Format("{0}  {1}", prefix, choice);
            }
         }

         return choicesLine;
      }

      private static string CreateLineText(string choice1, string choice2, int minFirstChoiceLen, int choiceIndex, int startIndex)
      {
         string choicesLine = "";

         if (choiceIndex == startIndex)
         {
            choicesLine += string.Format("> {0}", choice1);
         }
         else
         {
            choicesLine += string.Format("  {0}", choice1);
         }

         if (minFirstChoiceLen > choice1.Length)
         {
            choicesLine += new string(' ', minFirstChoiceLen - choice1.Length);
         }

         choicesLine += "  ";

         if (choiceIndex == startIndex + 1)
         {
            choicesLine += string.Format("> {0}", choice2);
         }
         else
         {
            choicesLine += string.Format("  {0}", choice2);
         }

         return choicesLine;
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

      public static void WriteText(string text, int line)
      {
         lock (ConsoleWriterLock)
         {
            Console.SetCursorPosition(0, line);
            Console.Write(text);
         }
      }
      public static void WriteText(string text, int column, int line)
      {
         lock (ConsoleWriterLock)
         {
            Console.SetCursorPosition(column, line);
            Console.Write(text);
         }
      }
      public static void WriteText(string[] textArray, int column, int line)
      {
         for (int i = 0; i < textArray.Length; i++)
         {
            WriteText(textArray[i], column, line + i);
         }
      }
      public static void WriteText(string[] textArray, int column, int line, ConsoleColor color)
      {
         lock (ConsoleWriterLock)
         {
            ConsoleColor temp = Console.ForegroundColor;
            Console.ForegroundColor = color;

            for (int i = 0; i < textArray.Length; i++)
            {
               WriteText(textArray[i], column, line + i);
            }

            Console.ForegroundColor = temp;
         }
      }
      public static void WriteText(string[] textArray, int column, int line, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
      {
         lock (ConsoleWriterLock)
         {
            ConsoleColor foreTemp = Console.ForegroundColor;
            ConsoleColor backTemp = Console.BackgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;

            for (int i = 0; i < textArray.Length; i++)
            {
               WriteText(textArray[i], column, line + i);
            }

            Console.ForegroundColor = foreTemp;
            Console.BackgroundColor = backTemp;
         }
      }
      public static void WriteText(string[] textArray, Location loc, ConsoleColor color)
      {
         lock (ConsoleWriterLock)
         {
            ConsoleColor temp = Console.ForegroundColor;
            Console.ForegroundColor = color;

            for (int i = 0; i < textArray.Length; i++)
            {
               WriteText(textArray[i], loc.X, loc.Y + i);
            }

            Console.ForegroundColor = temp;
         }
      }
      public static void WriteText(string text, int column, int line, ConsoleColor color)
      {
         lock (ConsoleWriterLock)
         {
            ConsoleColor temp = Console.ForegroundColor;
            Console.ForegroundColor = color;

            Console.SetCursorPosition(column, line);
            Console.Write(text);

            Console.ForegroundColor = temp;
         }
      }
      public static void WriteText(string text, int column, int line, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
      {
         lock (ConsoleWriterLock)
         {
            ConsoleColor foreTemp = Console.ForegroundColor;
            ConsoleColor backTemp = Console.BackgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;

            Console.SetCursorPosition(column, line);
            Console.Write(text);

            Console.ForegroundColor = foreTemp;
            Console.BackgroundColor = backTemp;
         }
      }
      public static void WriteText(char character, int column, int line)
      {
         lock (ConsoleWriterLock)
         {
            Console.SetCursorPosition(column, line);
            Console.Write(character);
         }
      }
      public static void WriteText(char character, Location loc)
      {
         lock (ConsoleWriterLock)
         {
            Console.SetCursorPosition(loc.X, loc.Y);
            Console.Write(character);
         }
      }
      public static void WriteText(char character, int column, int line, ConsoleColor color)
      {
         lock (ConsoleWriterLock)
         {
            ConsoleColor temp = Console.ForegroundColor;
            Console.ForegroundColor = color;

            Console.SetCursorPosition(column, line);
            Console.Write(character);

            Console.ForegroundColor = temp;
         }
      }
      public static void WriteText(char character, Location loc, ConsoleColor color)
      {
         lock (ConsoleWriterLock)
         {
            ConsoleColor temp = Console.ForegroundColor;
            Console.ForegroundColor = color;

            Console.SetCursorPosition(loc.X, loc.Y);
            Console.Write(character);

            Console.ForegroundColor = temp;
         }
      }

      public static void WriteTextInBorder(string text, Location loc,
         ConsoleColor foregroundColor = GlobalValues.BORDER_FORECOLOUR,
         ConsoleColor backgroundColor = GlobalValues.BORDER_BACKCOLOUR)
      {
         WriteText(text, GlobalValues.BORDER_LOCATION.X + loc.X, GlobalValues.BORDER_LOCATION.Y + loc.Y, foregroundColor, backgroundColor);
      }

      public static void WriteTextInBorder(char character, Location loc,
         ConsoleColor foregroundColor = GlobalValues.BORDER_FORECOLOUR,
         ConsoleColor backgroundColor = GlobalValues.BORDER_BACKCOLOUR)
      {
         WriteTextInBorder(character.ToString(), loc, foregroundColor, backgroundColor);
      }

      public static void WriteTextInBorder(string[] textArray, Location loc,
         ConsoleColor foregroundColor = GlobalValues.BORDER_FORECOLOUR,
         ConsoleColor backgroundColor = GlobalValues.BORDER_BACKCOLOUR)
      {
         for (int i = 0; i < textArray.Length; i++)
         {
            WriteTextInBorder(textArray[i], new Location(loc.X, loc.Y + i), foregroundColor, backgroundColor);
         }
      }

      public static void CentreTextInBorder(string text, int line,
         ConsoleColor foregroundColor = GlobalValues.BORDER_FORECOLOUR,
         ConsoleColor backgroundColor = GlobalValues.BORDER_BACKCOLOUR)
      {
         int startLoc = (GlobalValues.BORDER_LOCATION.Width - text.Length) / 2;
         WriteTextInBorder(text, new Location(startLoc, line), foregroundColor, backgroundColor);
      }


      public static void WriteScore(int score,string bonus = "")
      {
         ClearPromptLines();
         CentreText(string.Format("Score: {0}", score), GlobalValues.PROMPT_LINE);
         if (bonus != "")
         {
            CentreText(bonus, GlobalValues.CHOICES_LINE);
         }
      }
      public static void WriteScoreAndLives(int score, int lives, string bonus = "")
      {
         ClearPromptLines();
         CentreText(string.Format("Score: {0}", score), GlobalValues.PROMPT_LINE);
         CentreText(string.Format("Lives: {0}", new string('☻', lives)), GlobalValues.CHOICES_LINE);
         if (bonus != "")
         {
            CentreText(bonus, GlobalValues.CHOICES_LINE + 1);
         }
      }

      public static void CentreText(string text, int line, int offset = 0)
      {
         int startLoc = (Console.WindowWidth - text.Length + offset) / 2;

         WriteText(text, startLoc, line);
      }
      public static void CentreText(string text, int line, ConsoleColor foregroundColor, ConsoleColor backgroundColor, int offset = 0)
      {
         int startLoc = (Console.WindowWidth - text.Length + offset) / 2;

         WriteText(text, startLoc, line, foregroundColor, backgroundColor);
      }
      public static void CentrePrompt(string prompt, string choices)
      {
         ClearPromptLines();
         CentreText(prompt, GlobalValues.PROMPT_LINE);
         CentreText(choices, GlobalValues.CHOICES_LINE, -2);
      }
      public static void CentreChoices(string prompt, string choices1, string choices2)
      {
         ClearPromptLines();
         CentreText(prompt, GlobalValues.PROMPT_LINE);
         int startLoc = (Console.WindowWidth - choices1.Length - 2) / 2;
         WriteText(choices1, startLoc, GlobalValues.CHOICES_LINE);
         WriteText(choices2, startLoc, GlobalValues.CHOICES_LINE + 1);
      }
      public static void CentreError(string problem, string solution)
      {
         ClearPromptLines();

         ConsoleColor foregroundColor = ConsoleColor.Yellow;
         ConsoleColor backgroundColor = ConsoleColor.Red;

         CentreText(problem, GlobalValues.PROMPT_LINE, foregroundColor, backgroundColor);
         CentreText(solution, GlobalValues.CHOICES_LINE, foregroundColor, backgroundColor);
         CentreText("Press any key...", GlobalValues.CHOICES_LINE + 1, foregroundColor, backgroundColor);
         Console.ReadKey(true);
      }

      public static void ClearLine(int line)
      {
         string blankLine = new string(' ', GlobalValues.WINDOW_WIDTH);

         WriteText(blankLine, line);
      }
      public static void ClearPromptLines()
      {
         ClearLine(GlobalValues.PROMPT_LINE);
         ClearLine(GlobalValues.CHOICES_LINE);
         ClearLine(GlobalValues.CHOICES_LINE + 1);
      }
      public static void ClearInsideBorder()
      {
         for (int i = GlobalValues.BORDER_LOCATION.Top; i < GlobalValues.BORDER_LOCATION.Bottom; i++)
         {
            ClearLine(i);
         }

         DrawAppBorder();
      }

      public static void SetConsoleSize(int width, int height)
      {
         try
         {
            lock (ConsoleWriterLock)
            {
               Console.SetWindowSize(width, height);
               Console.SetBufferSize(width, height);
            }
         }
         catch (IOException)
         {
         }
      }
      public static void SetBufferSize(int width, int height)
      {
         try
         {
            lock (ConsoleWriterLock)
            {
               int windowWidth = Console.WindowWidth;
               int windowHeight = Console.WindowHeight;
               Console.SetBufferSize(Math.Max(width, windowWidth), Math.Max(height, windowHeight));
            }
         }
         catch (IOException)
         {
         }
      }
      public static void DrawAppBorder()
      {
         WriteTextInBorder(Application.SplitString(Resources.ScreenBorder), new Location(0, 0));
      }
   }
}
