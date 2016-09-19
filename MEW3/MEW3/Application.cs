using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using MEW3.Properties;
using System.IO;
using System.Windows.Forms;

namespace MEW3
{
   public class Application
   {
      private static Dictionary<string, Map> EasyMaps = new Dictionary<string, Map>()
      {
         { "Classic",              new Map(SplitMap(Resources.DefaultIntentMap))    },
         { "Large Classic",        new Map(SplitMap(Resources.LargeMap))            },
         { "Bases",                new Map(SplitMap(Resources.BasesMap))            },
         { "Portals",              new Map(SplitMap(Resources.ErraticMap))          },
         { "X's",                  new Map(SplitMap(Resources.ExesMap))             },
      };

      private static Dictionary<string, Map> HardMaps = new Dictionary<string, Map>()
      {
         { "Tough Classic",        new Map(SplitMap(Resources.TwoGhostsDefaultMap)) },
         { "Tough Large Classic",  new Map(SplitMap(Resources.TwoGhostsLargeMap))   },
         { "Four Bases",           new Map(SplitMap(Resources.ManyGhostsBasesMap))  },
         { "Tough Portals",        new Map(SplitMap(Resources.TwoGhostsErraticMap)) },
         { "Tough X's",            new Map(SplitMap(Resources.TwoGhostsExesMap))    },
      };
      
      public Application()
      {
         int windowHeight = Console.WindowHeight;
         int windowWidth = Console.WindowWidth;
         int bufferHeight = Console.BufferHeight;
         int bufferWidth = Console.BufferWidth;
         bool cursorVisible = Console.CursorVisible;
         Encoding encoding = Console.OutputEncoding;

         Console.CursorVisible = false;

         ConsoleWindow.AttachConsoleWindow();
         ConsoleWindow.SetWindowText("!E\u00b7");
         //ConsoleWindow.SizeWindow(new Rectangle(200, 100, 170, 270));
         ConsoleWindow.SetConsoleIcon(Resources.MEW3);
         //ConsoleWindow.SetTaskbarIcon(Resources.MEW3);
         ConsoleWindow.ShowConsoleWindow();

         //Console.OutputEncoding = Encoding.UTF8;

         //Console.WriteLine("\u0123");
         //Console.ReadLine();

         ApplicationRun();

         //Console.OutputEncoding = encoding;

         Console.CursorVisible = cursorVisible;

         Console.WindowHeight = windowHeight;
         Console.WindowWidth = windowWidth;
         Console.BufferHeight = bufferHeight;
         Console.BufferWidth = bufferWidth;
      }

      private static void ApplicationRun()
      {
         bool goToTitle = true;
         
         while (true)
         {
            if (goToTitle && !IntroSequence())
            {
               return;
            }

            Map map;

            #region Select Map
            while (true)
            {
               var choices = new List<string>() { 
                  "Easy", 
                  "Hard", 
                  "Use my own!", 
                  "Back" 
               };

               var selection = ConsoleMethods.PromptSelection("Choose a difficulty:", choices);

               if (selection == "Easy")
               {
                  List<string> easyMaps = new List<string>(EasyMaps.Keys.ToList());
                  easyMaps.Add("Back");
                  var choice = ConsoleMethods.PromptSelection("Choose an easy map:", easyMaps);

                  if (choice == "Back")
                  {
                     continue;
                  }

                  if (EasyMaps.TryGetValue(choice, out map))
                  {
                     break;
                  }
               }
               else if (selection == "Hard")
               {
                  List<string> hardMaps = new List<string>(HardMaps.Keys.ToList());
                  hardMaps.Add("Back");
                  var choice = ConsoleMethods.PromptSelection("Choose a hard map:", hardMaps);

                  if (choice == "Back")
                  {
                     continue;
                  }
                  if (HardMaps.TryGetValue(choice, out map))
                  {
                     break;
                  }
               }
               else if (selection == "Use my own!")
               {
                  string[] tempMap = LoadFile();

                  if (tempMap[0] == "Load Failed...")
                  {
                     Console.WriteLine("Load Failed...");
                     Thread.Sleep(2000);
                  }
                  else
                  {
                     List<string> errorText = VerifyMap(tempMap);
                     if (errorText != null)
                     {
                        WriteErrorText(errorText);
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
                  WriteErrorText("Invalid choice...");
               }
            }
            #endregion

            if (map != null)
            {
               GameManager game = new GameManager(map);

               while (true)
               {
                  if (!game.Go())
                  {
                     Thread.Sleep(100);
                     break;
                  }
               }

               var choicesList = new List<string>() { "New map!", "Exit (to Title)" };

               string input = ConsoleMethods.PromptSelection("Choose new map?", choicesList);
               goToTitle = input == "Exit (to Title)";
            }
         }
      }
      private static bool IntroSequence()
      {
         Console.Clear();
         var seq = new IntroSequence();

         while (seq.InProgress)
         {
            Thread.Sleep(100);
            string choice = ConsoleMethods.PromptSelection(12, new List<string> { "Play!", "Try!" });

            switch (choice)
            {
               case "Play!":
                  seq.Dispose();
                  break;
               case "Try!":
                  seq.Pause();
                  IntroGameManager intro = new IntroGameManager(SplitMap(Resources.IntroScreen));
                  while (true)
                  {
                     if (!intro.Go())
                     {
                        Thread.Sleep(100);
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

      private static List<string> VerifyMap(string[] map)
      {
         List<string> ret = null;
         
         Dictionary<int, int> portalsCount = new Dictionary<int, int>();

         for (int i = 0; i < map.Length; i++)
         {
            for (int j = 0; j < map[i].Length; j++)
            {
               char thisChar = map[i][j];
               int parseNum = 0;
               bool isNumber = int.TryParse(map[i][j].ToString(), out parseNum);
               
               if (isNumber)
               {                 
                  if(portalsCount.ContainsKey(parseNum))
                  {
                     portalsCount[parseNum]++;
                  }
                  else
                  {
                     portalsCount.Add(parseNum, 1);
                  }
               }
               //else if (thisChar == '@')
               else if (thisChar == SpecialChars.Portal)
               {
                  //ret = CreateErrorText("Maps cannot contain the symbol '@'.", "Use pairs of numbers for portals instead!");
                  ret = CreateErrorText(string.Format("Maps cannot contain the symbol '{0}'.", SpecialChars.Portal), "Use pairs of numbers for portals instead!");
                  return ret;
               }
            }
         }

         for (int i = 0; i < 10; i++)
         {
            if (portalsCount.ContainsKey(i) && portalsCount[i] != 2)
            {
               ret = CreateErrorText(string.Format("Invalid number of portals for '{0}'", i), "Must have exactly two portals for each number");
            }
         }

         return ret;
      }

      private static List<string> CreateErrorText(string problem, string solution)
      {
         return new List<string>() 
         { 
            string.Format("Problem: {0}", problem), 
            string.Format("Solution: {0}", solution) 
         };
      }
      private static void WriteErrorText(string errorText)
      {
         var height = Console.WindowHeight;
         var width = Console.WindowWidth;

         Console.ForegroundColor = ConsoleColor.Red;

         List<string> completeText = new List<string>() { errorText, "Press any key to continue..." };
         ConsoleMethods.FitConsoleToText(completeText);

         WriteConsoleMessage(completeText, 1000);
         Console.ReadKey(true);

         Console.ForegroundColor = ConsoleColor.White;
         ConsoleMethods.SetConsoleSize(width, height);
      }

      private static void WriteErrorText(List<string> errorText)
      {
         var height = Console.WindowHeight;
         var width = Console.WindowWidth;

         Console.ForegroundColor = ConsoleColor.Red;

         List<string> completeText = new List<string>(errorText);
         completeText.Add("Press any key to continue...");
         ConsoleMethods.FitConsoleToText(completeText);

         WriteConsoleMessage(completeText, 1000);
         Console.ReadKey(true);

         Console.ForegroundColor = ConsoleColor.Gray;
         ConsoleMethods.SetConsoleSize(width, height);
      }

      private static void WriteConsoleMessage(string message, int sleepSeconds)
      {
         Console.WriteLine();
         Console.WriteLine(message);
         Thread.Sleep(sleepSeconds);
      }
      private static void WriteConsoleMessage(List<string> messages, int sleepSeconds)
      {
         Console.WriteLine();
         for (int i = 0; i < messages.Count; i++)
         {
            Console.WriteLine(messages[i]);
         }
         Thread.Sleep(sleepSeconds);
      }

      private static string[] OpenAndReadFileIfExists(string path)
      {
         string[] ret = { "" };
         
         if (File.Exists(path))
         {
            ret = File.ReadAllLines(path);
         }

         return ret;
      }

      private static string[] LoadFile()
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

      private static string[] SplitMap(string map)
      {
         return map.Split(new string[] { "\r\n" }, StringSplitOptions.None);
      }
   }
}
