using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using TechnoTennis.Properties;
using System.IO;
using System.Windows.Forms;

namespace TechnoTennis
{
   public class Application
   {
      private const string FilePath = @"hs.txt";

      private int HighScore;
      
      public Application()
      {
         #region Save Console State
         int windowHeight = Console.WindowHeight;
         int windowWidth = Console.WindowWidth;
         int bufferHeight = Console.BufferHeight;
         int bufferWidth = Console.BufferWidth;
         bool cursorVisible = Console.CursorVisible;
         #endregion

         #region Set Console State
         Console.CursorVisible = false;
         ConsoleWindow.AttachConsoleWindow();
         ConsoleWindow.SetWindowText("§");
         ConsoleWindow.SetConsoleIcon(Resources.Slither);
         ConsoleWindow.ShowConsoleWindow();
         #endregion

         ApplicationRun();

         #region Restore Console State
         Console.CursorVisible = cursorVisible;
         Console.WindowHeight = windowHeight;
         Console.WindowWidth = windowWidth;
         Console.BufferHeight = bufferHeight;
         Console.BufferWidth = bufferWidth;
         #endregion
      }

      private void ApplicationRun()
      {
         HighScore = 0;
         if (File.Exists(FilePath))
         {
            int.TryParse(File.ReadAllText(FilePath), out HighScore);
         }
                 
         while (true)
         {
            //if (!IntroSequence())
            //{
            //   return;
            //}

            int runningScore = 0;

            GameManager game = MakeAndRunGame();
            runningScore += game.Score;

            if (runningScore > HighScore)
            {
               File.WriteAllText(FilePath, runningScore.ToString());
               HighScore = runningScore;
            }
         }
      }

      private GameManager MakeAndRunGame()
      {
         GameManager game = new GameManager();

         while (true)
         {
            if (!game.Go())
            {
               Thread.Sleep(100);
               break;
            }
         }

         return game;
      }

      private bool IntroSequence()
      {
         Console.Clear();
         var seq = new Intro(HighScore);

         bool ret = true;

         while (seq.InProgress)
         {
            Thread.Sleep(200);

            string choice = ConsoleMethods.PromptHorizontalSelection("", new List<string> { "Play!", "Exit!" });

            switch (choice)
            {
               case "Play!":
                  seq.Dispose();
                  break;
               case "Exit!":
                  seq.Dispose();
                  Console.Clear();
                  ret = false;
                  break;
               default:
                  ret = false;
                  break;
            }
         }

         return ret;
      }

      private string[] VerifyMap(string[] map)
      {
         int heightInsideBorder = GlobalValues.BORDER_LOCATION.Height;
         int widthInsideBorder = GlobalValues.BORDER_LOCATION.Width;

         if (map.Length > heightInsideBorder)
         {
            return CreateErrorText(string.Format("Too tall! ({0})", map.Length), string.Format("Must be <= {0}.", heightInsideBorder)); ;
         }
         
         Dictionary<int, int> portalsCount = new Dictionary<int, int>();

         for (int i = 0; i < map.Length; i++)
         {
            int lineLen = map[i].Length;

            for (int j = 0; j < lineLen; j++)
            {
               if (lineLen > widthInsideBorder)
               {
                  return CreateErrorText(string.Format("Too wide! ({0})", map.Length), string.Format("Must be <= {0}.", heightInsideBorder));
               }

               char thisChar = map[i][j];
               int parseNum = 0;
               bool isNumber = int.TryParse(map[i][j].ToString(), out parseNum);
               
               if (isNumber)
               {                 
                  if (portalsCount.ContainsKey(parseNum))
                  {
                     portalsCount[parseNum]++;
                  }
                  else
                  {
                     portalsCount.Add(parseNum, 1);
                  }
               }
               else if (thisChar == SpecialChars.Portal)
               {
                  return CreateErrorText(string.Format("Invalid: '{0}'", SpecialChars.Portal), "Use Numbers!");
               }
            }
         }

         for (int i = 0; i < 10; i++)
         {
            if (portalsCount.ContainsKey(i))
            {
               if (portalsCount[i] > 2)
               {
                  return CreateErrorText(string.Format("Too many: '{0}'", i), "Two or None");
               }
               if(portalsCount[i] < 2)
               {
                  return CreateErrorText(string.Format("Too few: '{0}'", i), "Two or None");
               }
            }
         }

         return null;
      }

      private string[] CreateErrorText(string problem, string solution)
      {
         return new string[] 
         { 
            problem, 
            solution
         };
      }

      private string[] OpenAndReadFileIfExists(string path)
      {
         string[] ret = { "" };
         
         if (File.Exists(path))
         {
            ret = File.ReadAllLines(path);
         }

         return ret;
      }

      private string[] LoadFile()
      {
         var FD = new OpenFileDialog();
         FD.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
         FD.DefaultExt = ".txt";
         FD.InitialDirectory = @"~\";

         string[] ret = new string[] { "Load failed" };

         if (FD.ShowDialog() == DialogResult.OK)
         {
            string fileToOpen = FD.FileName;

            ret = File.ReadAllLines(fileToOpen);
         }

         return ret;
      }

      public static string[] SplitString(string text)
      {
         return text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
      }
   }
}
