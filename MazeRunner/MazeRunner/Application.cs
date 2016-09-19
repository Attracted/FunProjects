using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using MazeRunner.Properties;
using System.IO;
using System.Windows.Forms;

namespace MazeRunner
{
   public class Application
   {
      private static List<Map> Maps = new List<Map>()
      {
         //{ new Map(SplitString(Resources.IntroScreen)) },   
         { new Map(SplitString(Resources.Stage1)) },
         { new Map(SplitString(Resources.Stage2)) },
         { new Map(SplitString(Resources.Stage3)) },
         { new Map(SplitString(Resources.Stage4)) },
         { new Map(SplitString(Resources.Stage5)) },
         { new Map(SplitString(Resources.Stage6)) }
      };
      private const string FilePath = @"hs.txt";

      private bool CampaignMode;
      private bool CreateMode;
      private bool WonGame;
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
         ConsoleWindow.SetWindowText("!E\u00b7");
         ConsoleWindow.SetConsoleIcon(Resources.MEW3);
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
         
         bool goToTitle = true;
         
         while (true)
         {
            if (goToTitle)
            {
               if (WonGame)
               {
                  if (!OutroSequence())
                  {
                     return;
                  }
               }
               else
               {
                  if (!IntroSequence())
                  {
                     return;
                  }
               }
            }

            Map map = null;

            while (true)
            {
               var choices = new List<string>() { 
                  "Campaign",
                  "Create",
                  "Use my own!",
                  "Back"
               };

               string selection = ConsoleMethods.PromptHorizontalSelection("Choose a mode:", choices);

               if (selection == "Campaign")
               {
                  CampaignMode = true;
                  break;
               }
               else if (selection == "Create")
               {
                  map = null;
                  CreateMode = true;
                  break;
               }
               else if (selection == "Use my own!")
               {
                  string[] tempMap = LoadFile();

                  if (tempMap[0] == "Load Failed...")
                  {
                     ConsoleMethods.CentreError("Load Failed...", "");
                     Thread.Sleep(2000);
                  }
                  else
                  {
                     string[] errorText = VerifyMap(tempMap);
                     if (errorText != null)
                     {
                        ConsoleMethods.CentreError(errorText[0], errorText[1]);
                     }
                     else
                     {
                        map = new Map(tempMap);
                        break;
                     }
                  }
               }
               else if (selection == "Back")
               {
                  map = null;
                  goToTitle = true;
                  break;
               }
               else if (selection == null)
               {
                  return;
               }
               else
               {
                  ConsoleMethods.CentreError("Invalid choice...", "");
               }
            }

            if (map != null)
            {
               int lives = 5;
               MakeAndRunGame(map, 0, lives);
            }
            else if (CampaignMode)
            {
               bool completed = true;
               int runningScore = 0;
               int runningLives = 5;

               for (int i = 0; i < Maps.Count; i++)
               {
                  map = Maps[i];
                  //ConsoleMethods.SetBufferSize(map.Bounds.X + 1, map.Bounds.Y + 1);

                  GameManager game = MakeAndRunGame(map, runningScore, runningLives);
                  if (!game.AdvanceCampaign)
                  {
                     completed = false;
                     break;
                  }

                  runningScore += game.Score;
                  runningLives = game.Lives;

                  while (Console.KeyAvailable)
                  {
                     Console.ReadKey(true);
                  }
               }

               if (completed)
               {
                  WonGame = true;
               }

               if (runningScore > HighScore)
               {
                  File.WriteAllText(FilePath, runningScore.ToString());
                  HighScore = runningScore;
               }
            }
            else if (CreateMode)
            {
               StartCreateMode();
            }
         }
      }

      private GameManager MakeAndRunGame(Map map, int startScore, int startLives)
      {
         GameManager game = new GameManager(map, startScore, startLives);

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
         var seq = new IntroSequence(HighScore);

         while (seq.InProgress)
         {
            Thread.Sleep(200);

            string choice = ConsoleMethods.PromptHorizontalSelection("", new List<string> { "Play!", "Try!" });

            switch (choice)
            {
               case "Play!":
                  seq.Dispose();
                  break;
               case "Try!":
                  seq.Pause();
                  IntroGameManager intro = new IntroGameManager(SplitString(Resources.IntroScreen));
                  while (true)
                  {
                     if (!intro.Go())
                     {
                        Thread.Sleep(200);
                        break;
                     }
                  }

                  seq.Continue();
                  break;
               default:
                  return false;
            }
         }

         return true;
      }
      private bool OutroSequence()
      {
         Console.Clear();
         var seq = new OutroSequence(HighScore);

         while (seq.InProgress)
         {
            Thread.Sleep(200);

            string choice = ConsoleMethods.PromptHorizontalSelection("", new List<string> { "Play!", "Challenge!" });

            switch (choice)
            {
               case "Play!":
                  seq.Dispose();
                  break;
               case "Challenge!":
                  seq.Pause();
                  IntroGameManager intro = new IntroGameManager(SplitString(Resources.OutroScreen));
                  while (true)
                  {
                     if (!intro.Go())
                     {
                        Thread.Sleep(200);
                        break;
                     }
                  }

                  seq.Continue();
                  break;
               default:
                  return false;
            }
         }

         return true;
      }

      private void StartCreateMode()
      {
         Console.Clear();
         var createMode = new CreateMode(SplitString(Resources.CreateBorder));

         while (true)
         {
            if (!createMode.Go())
            {
               break;
            }
         }
      }

      private string[] VerifyMap(string[] map)
      {
         int heightInsideBorder = GlobalValues.BORDER_LOCATION.Height;
         int widthInsideBorder = GlobalValues.BORDER_LOCATION.Width;

         if (map.Length > heightInsideBorder)
         {
            return CreateErrorText(string.Format("Map is too tall ({0} lines).", map.Length), string.Format("Map must be <= {0} lines.", heightInsideBorder));;
         }
         
         Dictionary<int, int> portalsCount = new Dictionary<int, int>();

         for (int i = 0; i < map.Length; i++)
         {
            int lineLen = map[i].Length;

            for (int j = 0; j < lineLen; j++)
            {
               if (lineLen > widthInsideBorder)
               {
                  return CreateErrorText(string.Format("Map is too wide ({0} lines).", map.Length), string.Format("Map must be <- {0} lines.", heightInsideBorder));
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
                  return CreateErrorText(string.Format("Maps cannot contain the symbol '{0}'.", SpecialChars.Portal), "Use pairs of numbers for portals instead!");
               }
            }
         }

         for (int i = 0; i < 10; i++)
         {
            if (portalsCount.ContainsKey(i))
            {
               if (portalsCount[i] > 2)
               {
                  return CreateErrorText(string.Format("Too many portals for '{0}'", i), "Need exactly two portals");
               }
               if(portalsCount[i] < 2)
               {
                  return CreateErrorText(string.Format("Not enough portals for '{0}'", i), "Need exactly two portals");
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
      private void WriteErrorText(string errorText)
      {
         var height = Console.WindowHeight;
         var width = Console.WindowWidth;

         Console.ForegroundColor = ConsoleColor.Red;

         List<string> completeText = new List<string>() { errorText, "Press any key to continue..." };
         //ConsoleMethods.FitConsoleToText(completeText);

         WriteConsoleMessage(completeText, 1000);
         Console.ReadKey(true);

         Console.ForegroundColor = ConsoleColor.White;
         //ConsoleMethods.SetConsoleSize(width, height);
      }

      private void WriteErrorText(List<string> errorText)
      {
         Console.ForegroundColor = ConsoleColor.Red;

         List<string> completeText = new List<string>(errorText);
         completeText.Add("Press any key to continue...");
         WriteConsoleMessage(completeText, 1000);

         Console.ForegroundColor = ConsoleColor.Gray;
         Console.ReadKey(true);


      }

      private void WriteConsoleMessage(string message, int sleepSeconds)
      {
         Console.WriteLine();
         Console.WriteLine(message);
         Thread.Sleep(sleepSeconds);
      }
      private void WriteConsoleMessage(List<string> messages, int sleepSeconds)
      {
         Console.WriteLine();
         for (int i = 0; i < messages.Count; i++)
         {
            Console.WriteLine(messages[i]);
         }
         Thread.Sleep(sleepSeconds);
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

         string[] ret = new string[] { "Load Failed..." };

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
