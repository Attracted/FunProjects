using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MazeRunner.Properties;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace MazeRunner
{
   public class GameManager
   {
      const int TIMER_INTERVAL = 250;
      const int TIMER_INTERVAL_SLOW = 350;
      const int TIMER_DELAY = 1000;

      protected Mew3 Mew3;
      //private Player Ghost;
      private List<Ghost> Ghosts = new List<Ghost>();
      private Map GameMap;
      private System.Timers.Timer MoveGhostsTimer;
      private Thread GhostsThread;
      private Dictionary<Location, ConsoleColor> PortalColours;
      private bool CameFromPortal;
      private Map Map;
      private bool Started = false;

      public bool AdvanceCampaign { get; private set; }
      public int Score { get; set; }
      public int Lives { get; set; }

      public GameManager(string[] map)
      {
         Console.CursorVisible = false;
         Console.Clear();
         this.Map = new Map(map);
         Initialize();
      }

      public GameManager(Map map, int score = 0, int lives = 1)
      {
         Console.CursorVisible = false;
         Console.Clear();
         this.Map = map;
         Score = score;
         Lives = lives;
         Initialize();
      }

      private void Initialize(bool newGame = true)
      {
         AdvanceCampaign = false;

         if (newGame)
         {
            GameMap = new Map(Map);
         }

            Mew3 = new Mew3(GameMap.Mew3StartLoc, SpecialChars.Mew3_Right);
            Mew3.Colour = ConsoleColor.Yellow;
            Ghosts.Clear();

         foreach (Location ghostStartLoc in GameMap.GhostStartLocs)
         {
            var ghost = new Ghost(ghostStartLoc, SpecialChars.Ghost);
            ghost.Immune = true;
            ghost.Colour = ConsoleColor.Red;

            Ghosts.Add(ghost);
         }

         if (newGame)
         {
            PortalColours = new Dictionary<Location, ConsoleColor>();
            int numPortals = 1;

            foreach (var portal in GameMap.Grid.Portals)
            {
               Location portalLoc = portal.Item2;

               if (!PortalColours.ContainsKey(portalLoc))
               {
                  var buddyLoc = GameMap.Grid.GetOtherPortalOrDefault(portalLoc);
                  if (buddyLoc != portalLoc)
                  {
                     PortalColours.Add(portalLoc, Colours.ColoursList[numPortals]);
                     PortalColours.Add(buddyLoc, Colours.ColoursList[numPortals]);
                     numPortals++;
                     if (numPortals == 10)
                     {
                        numPortals = 0;
                     }
                  }
               }
            }
         }

         ReadyUserAndPrintGame(750);

         StartGhostsThread(TIMER_DELAY);
      }

      private void ReadyUserAndPrintGame(int delays = 0)
      {        
         Console.Clear();
         ConsoleMethods.DrawAppBorder();

         PrintToConsole();

         if (delays > 0)
         {
            ConsoleMethods.CentreText("Get Ready!", GlobalValues.PROMPT_LINE);
            Thread.Sleep(delays);

            ConsoleMethods.ClearPromptLines();

            Console.ForegroundColor = ConsoleColor.Red;

            ConsoleMethods.CentreText("- GO! -", GlobalValues.PROMPT_LINE);

            Console.ForegroundColor = ConsoleColor.Gray;

            Thread.Sleep(delays);
            PrintToConsole();
         }
      }

      protected virtual void StartGhostsThread(int delay)
      {
         GhostsThread = new Thread(thread =>
         {
            MoveGhostsTimer = new System.Timers.Timer();
            MoveGhostsTimer.Elapsed += OnTimerTick;
            MoveGhostsTimer.Interval = TIMER_INTERVAL;
            Thread.Sleep(delay);
            MoveGhostsTimer.Start();
         });

         GhostsThread.Start();
      }

      protected virtual void StopGhostsThread()
      {
         MoveGhostsTimer.Stop();
         MoveGhostsTimer.Elapsed -= OnTimerTick;
         MoveGhostsTimer.Dispose();

         GhostsThread.Abort();
      }

      private void OnTimerTick(object e, EventArgs args)
      {
         if (!Started)
         {
            ConsoleMethods.WriteScoreAndLives(Score, Lives);
            Started = true;
         }

         MoveGhosts();
         UpdateConsole();
      }

      public bool Go()
      {
         if (!CheckContinue())
         {
            return false;
         }
         
         if (!Console.KeyAvailable)
         {
            return true;
         }
         ConsoleKey key = Console.ReadKey(true).Key;

         if (key == ConsoleKey.NoName)
         {
            return true;
         }

         if (key == ConsoleKey.Enter)
         {
            return PauseGame();
         }

         switch (key)
         {
            case ConsoleKey.LeftArrow:
               //Mew3.Character = '3';
               Mew3.Character = SpecialChars.Mew3_Left;
               MakeMove(-1, 0);
               break;
            case ConsoleKey.RightArrow:
               //Mew3.Character = 'E';
               Mew3.Character = SpecialChars.Mew3_Right;
               MakeMove(1, 0);
               break;
            case ConsoleKey.UpArrow:
               //Mew3.Character = 'W';
               Mew3.Character = SpecialChars.Mew3_Up;
               MakeMove(0, -1);
               break;
            case ConsoleKey.DownArrow:
               //Mew3.Character = 'M';
               Mew3.Character = SpecialChars.Mew3_Down;
               MakeMove(0, 1);
               break;
            default:
               break;
         }

         UpdateConsole();

         if (!CheckContinue())
         {
            return false;
         }

         return true;
      }

      private bool PauseGame()
      {
         StopGhostsThread();

         while (true)
         {
            var choicesList = new List<string>() { "Continue", "Exit" };
            var choice = ConsoleMethods.PromptHorizontalSelection("Game paused.", choicesList);
            if (choice == "Continue")
            {
               ReadyUserAndPrintGame();
               StartGhostsThread(250);
               return true;
            }
            else if (choice == "Exit")
            {
               choicesList = new List<string>() { "Yes", "No" };

               choice = ConsoleMethods.PromptHorizontalSelection("Exit: Are you sure?", choicesList);
               if (choice == "Yes")
               {
                  return false;
               }
               else if (choice == "No")
               {
               }
            }
         }
      }

      private void MakeMove(int xDiff, int yDiff)
      {
         int x = Mew3.Col + xDiff;
         int y = Mew3.Row + yDiff;

         var nextLoc = new Location(x, y);
         var nextPortalLoc = GameMap.Grid.GetOtherPortalOrDefault(nextLoc);

         if (nextPortalLoc != nextLoc && !CameFromPortal) 
         {
            //var newLoc = new Location(nextPortalLoc.X + xDiff, nextPortalLoc.Y + yDiff);
            //var oppositeLoc = new Location(nextPortalLoc.X - xDiff, nextPortalLoc.Y - yDiff);

            //// Try "jumping out" of the portal first, then just land on it otherwise.
            //if (GameMap.Grid.InBounds(newLoc) 
            //   && GameMap.Grid.Passable(newLoc) 
            //   && GameMap[newLoc.X, newLoc.Y] != SpecialChars.Portal)
            //{
            //   Mew3.UpdateLocation(newLoc);
            //   GameMap.UpdateMapAfterMove(newLoc.X, newLoc.Y);

            //   CameFromPortal = false;
            //}
            //else if (GameMap.Grid.InBounds(oppositeLoc) 
            //      && GameMap.Grid.Passable(oppositeLoc) 
            //      && GameMap[oppositeLoc.X, oppositeLoc.Y] != SpecialChars.Portal)
            //{
            //   Mew3.UpdateLocation(oppositeLoc);
            //   Mew3.UpdateCharacter(-xDiff, -yDiff);
            //   GameMap.UpdateMapAfterMove(oppositeLoc.X, oppositeLoc.Y);

            //   CameFromPortal = false; 
            //}

            if (GameMap.Grid.InBounds(nextLoc) 
                  && GameMap.Grid.Passable(nextLoc))
            {
               Mew3.UpdateLocation(nextPortalLoc);
               CameFromPortal = true;
            }
            else
            {
               CameFromPortal = false;
            }
         }
         else
         {
            CameFromPortal = false;

            if (GameMap.Grid.InBounds(nextLoc) && GameMap.Grid.Passable(nextLoc))
            {
               var tmp = GameMap[x, y];
               Mew3.UpdateLocation(x, y);
               UpdateScore(x, y);

               if (tmp == 'o')
               {
                  MoveGhostsTimer.Interval = TIMER_INTERVAL_SLOW;

                  foreach (Ghost ghost in Ghosts)
                  {
                     ghost.Immune = false;
                  }
               }
            }
         }
      }

      private void UpdateScore(int x, int y, string bonus = "")
      {
         char character = GameMap[x, y];

         if (character == SpecialChars.Bit || character == SpecialChars.Power)
         {
            Score++;

            GameMap.UpdateMapAfterMove(x, y);

            ConsoleMethods.WriteScoreAndLives(Score, Lives);
         }
         else if (bonus != "")
         {
            Score += 10;
            ConsoleMethods.WriteScoreAndLives(Score, Lives, bonus);
         }
      }

      private void MoveGhosts()
      {
         foreach (Ghost ghost in Ghosts)
         {
            if (ghost.PenaltyCounter == 0)
            {
               var ghostloc = ghost.CurrentLoc;
               var mew3loc = Mew3.CurrentLoc;
               var startloc = ghost.StartLoc;

               var newloc = ghostloc;

               AddGhostWalls(GameMap.Grid, ghost);

               if (!ghost.Immune)
               {
                  var grid = GameMap.Grid;

                  grid.Walls.Add(mew3loc);

                  var astar = new AStarSearch(grid, startloc, ghostloc);
                  if (astar.cameFrom.ContainsKey(ghostloc))                                                     
                  {
                     newloc = astar.cameFrom[ghostloc];
                  }

                  grid.Walls.Remove(mew3loc);
               }
               else
               {
                  var astar = new AStarSearch(GameMap.Grid, mew3loc, ghostloc);
                  if (astar.cameFrom.ContainsKey(ghostloc))
                  {
                     newloc = astar.cameFrom[ghostloc];
                  }
               }

               RemoveGhostWalls(GameMap.Grid);

               ghost.UpdateLocation(newloc);

               if (newloc == ghost.StartLoc && !ghost.Immune)
               {
                  ghost.Immune = true;
                  MoveGhostsTimer.Interval = TIMER_INTERVAL;
               }
            }
            else
            {
               ghost.DecrementCounter();
               ConsoleMethods.WriteTextInBorder(ghost.PenaltyCounter.ToString(), ghost.CurrentLoc);
            }
         }
      }

      private void AddGhostWalls(SquareGrid grid, Ghost ghost)
      {
         if (ghost.Immune)
         {
            List<Ghost> otherGhosts = Ghosts.FindAll(g => g.CurrentLoc != ghost.CurrentLoc && g.Immune);

            foreach (Ghost otherGhost in otherGhosts)
            {
               grid.Walls.Add(otherGhost.CurrentLoc);
            }
         }
      }

      private void RemoveGhostWalls(SquareGrid grid)
      {
         foreach (Ghost ghost in Ghosts)
         {
            grid.Walls.Remove(ghost.CurrentLoc);
         }
      }

      private bool CheckContinue()
      {
         bool cont = true;
         bool win = false;

         if (GameMap.CountDots() == 0)
         {
            win = true;
            cont = false;
         }
         else
         {
            Ghost ghost = Ghosts.FirstOrDefault(g => g.CurrentLoc == Mew3.CurrentLoc);
            
            if (ghost != null)
            {
               if (!ghost.Immune)
               {
                  if (ghost.CurrentLoc != ghost.StartLoc)
                  {
                     UpdateScore(ghost.Col, ghost.Row, "Nom!");
                  }

                  MoveGhostsTimer.Interval = TIMER_INTERVAL;
                  
                  ghost.ResetLocation();
                  ghost.SetCounter();
               }
               else
               {
                  cont = false;
               }
            } 
         }

         //var ghostsAtStart = Ghosts.FindAll(g => g.CurrentLoc == g.StartLoc && g.PenaltyCounter == 0);

         //foreach (Ghost ghost in ghostsAtStart)
         //{
         //   ghost.Immune = true;
         //}

         if (!cont && CheckRetry(win))
         {
            cont = true;
         }

         return cont;
      }

      protected virtual bool CheckRetry(bool win)
      {
         StopGhostsThread();

         DeathMessage(win);

         bool cont;

         if (win)
         {
            AdvanceCampaign = true;
            cont = false;
         }
         else if (Lives > 1)
         {
            Lives--;

            var choicesList = new List<string>() { "Continue!", "Exit" };
            string choice = ConsoleMethods.PromptHorizontalSelection("Continue?", choicesList);
            if (choice == "Continue!")
            {
               Initialize(false);
               cont = true;
            }
            else //if (choice == "Exit")
            {
               cont = false;
            }
         }
         else
         {
            Lives = 0;
            cont = false;
         }

         return cont;
      }

      protected void DeathMessage(bool win)
      {
         int deathCol = Mew3.Col;
         int deathRow = Mew3.Row;

         Thread.Sleep(1000);

         string message = win ? "You won!" : "Nice try!";
         int messageLen = message.Length;

         int messageCol = 0;

         if (deathCol + messageLen < Console.WindowWidth)
         {
            messageCol = deathCol + 1;
         }
         else if (deathCol - messageLen >= 0)
         {
            messageCol = deathCol - messageLen;
         }

         ConsoleMethods.WriteTextInBorder(message, messageCol, deathRow, ConsoleColor.Gray, ConsoleColor.DarkRed);

         Thread.Sleep(1000);
      }

      private static void DrawGrid(SquareGrid grid, AStarSearch astar)
      {
         // Print out the cameFrom array
         for (var y = 0; y < grid.Height; y++)
         {
            for (var x = 0; x < grid.Width; x++)
            {
               Location id = new Location(x, y);
               Location ptr = id;
               if (!astar.cameFrom.TryGetValue(id, out ptr))
               {
                  ptr = id;
               }
               if (grid.Walls.Contains(id)) { Console.Write("#"); }
               else if (ptr.X == x + 1) { Console.Write("\u2192"); }
               else if (ptr.X == x - 1) { Console.Write("\u2190"); }
               else if (ptr.Y == y + 1) { Console.Write("\u2193"); }
               else if (ptr.Y == y - 1) { Console.Write("\u2191"); }
               else { Console.Write("*"); }
            }
            Console.WriteLine();
         }
      }

      [STAThread]
      static void PrintToConsole(Map map, Mew3 mew3, List<Ghost> ghosts, Dictionary<Location, ConsoleColor> colours)
      {
         int xMew3 = mew3.Col;
         int yMew3 = mew3.Row;

         ConsoleColor color;
         Char character;

         for (int x = 0; x < map.Bounds.X; x++)
         {
            for (int y = 0; y < map.Bounds.Y; y++)
            {
               Ghost currGhost = ghosts.FirstOrDefault(g => g.Col == x && g.Row == y);

               if (currGhost != null)
               {
                  if (currGhost.CurrentLoc == mew3.CurrentLoc)
                  {
                     color = ConsoleColor.Red;
                     character = 'X';
                  }
                  else
                  {
                     color = currGhost.Colour;
                     character = currGhost.Character;
                  }
               }
               else if (x == mew3.Col && y == mew3.Row)
               {
                  color = mew3.Colour;
                  character = mew3.Character;
               }
               else if (map[x, y] == SpecialChars.Bit || map[x, y] == SpecialChars.Power)
               {
                  color = ConsoleColor.Yellow;
                  character = map[x, y];
               }
               else if (map[x, y] == SpecialChars.Portal)
               {
                  color = colours[new Location(x, y)];
                  character = SpecialChars.Portal;
               }
               else
               {
                  color = ConsoleColor.DarkCyan;
                  character = map[x, y];
               }

               ConsoleMethods.WriteText(character, x, y, color);
            }
         }
      }

      public void UpdateConsole()
      {
         PlayerToConsole(Mew3, ' ');

         Location mew3Loc = Mew3.CurrentLoc;

         foreach (Ghost ghost in Ghosts)
         {
            Location ghostLoc = ghost.CurrentLoc;
            Location ghostPrevLoc = ghost.PrevLoc;
            char ghostPrevLocChar = GameMap[ghostPrevLoc.X, ghostPrevLoc.Y];

            if (ghost.PenaltyCounter == 0)
            {
               PlayerToConsole(ghost, ghostPrevLocChar, ConsoleColor.Yellow);

               if (ghostLoc == mew3Loc)
               {
                  ConsoleMethods.WriteTextInBorder('X', ghostLoc, ConsoleColor.Red);
               }
            }
         }
      }

      private void PlayerToConsole(Player player, char replaceChar, ConsoleColor replaceColor = ConsoleColor.Black)
      {
         Location location = player.CurrentLoc;
         Location prevLocation = player.PrevLoc;
         char prevLocationChar = GameMap[prevLocation.X, prevLocation.Y];

         if (prevLocationChar == SpecialChars.Portal)
         {
            ConsoleMethods.WriteTextInBorder(SpecialChars.Portal, prevLocation, PortalColours[prevLocation]);
         }
         else if (replaceChar == ' ')
         {
            ConsoleMethods.WriteTextInBorder(' ', prevLocation);
         }
         else
         {
            ConsoleMethods.WriteTextInBorder(replaceChar, prevLocation, replaceColor);
         }

         ConsoleMethods.WriteTextInBorder(player.Character, location, player.Colour);
      }

      public void PrintToConsole()
      {   
         int xMew3 = Mew3.Col;
         int yMew3 = Mew3.Row;

         ConsoleColor color;
         Char character;

         for (int x = 0; x < GameMap.Bounds.X; x++)
         {
            for (int y = 0; y < GameMap.Bounds.Y; y++)
            {
               Ghost currGhost = Ghosts.FirstOrDefault(g => g.Col == x && g.Row == y);
               
               if (currGhost != null)
               {
                  if (currGhost.CurrentLoc == Mew3.CurrentLoc)
                  {
                     color = ConsoleColor.Red;
                     character = 'X';
                  }
                  else
                  {
                     color = currGhost.Colour;
                     character = currGhost.Character;
                  }
               }
               else if (x == Mew3.Col && y == Mew3.Row)
               {
                  color = Mew3.Colour;
                  character = Mew3.Character;
               }
               else if (GameMap[x, y] == SpecialChars.Bit || GameMap[x, y] == SpecialChars.Power)
               {
                  color = ConsoleColor.Yellow;
                  character = GameMap[x, y];
               }
               else if (GameMap[x, y] == SpecialChars.Portal)
               {
                  color = PortalColours[new Location(x, y)];
                  character = SpecialChars.Portal;
               }
               else
               {
                  color = ConsoleColor.DarkCyan;
                  character = GameMap[x, y];
               }

               ConsoleMethods.WriteTextInBorder(character, x, y, color);
            }
         }
         
         
         //ConsoleMethods.WriteText(map.ToString(), 0, 0, ConsoleColor.DarkCyan);

         //ConsoleMethods.WriteText(mew3.Character, xMew3, yMew3, mew3.Colour);
         //if (map[xMew3, yMew3] != SpecialChars.Portal)
         //{
         //   map[xMew3, yMew3] = ' ';
         //}

         //foreach (Ghost ghost in ghosts)
         //{
         //   if (ghost.CurrentLoc == mew3.CurrentLoc)
         //   {
         //      ConsoleMethods.WriteText('X', ghost.Col, ghost.Row, ConsoleColor.Red);
         //   }
         //   else
         //   {
         //      ConsoleMethods.WriteText(ghost.Character, ghost.Col, ghost.Row, ghost.Colour);
         //   }
         //}




         //else if (x == mew3.Col && y == mew3.Row)
         //{
         //   color = mew3.Colour;
         //   character = Convert.ToByte(mew3.Character);
         //   //if (map[x, y] != '@')
         //   if (map[x, y] != SpecialChars.Portal)
         //   {
         //      map[x, y] = ' ';
         //   }
         //}
         //else if (map[x, y] == '.' || map[x, y] == 'o')
         //{
         //   color = Colours.Yellow;
         //   character = Convert.ToByte(map[x, y]);
         //}
         ////else if (map[x, y] == '@')
         //else if (map[x, y] == SpecialChars.Portal)
         //{
         //   color = colours[new Location(x, y)];
         //   //character = Convert.ToByte('@');
         //   character = Convert.ToByte(SpecialChars.Portal);
         //}
         //else
         //{
         //   //Console.ForegroundColor = ConsoleColor.Cyan;
         //   //Console.Write(map[x, y]);
         //   color = Colours.Cyan;
         //   character = Convert.ToByte(map[x, y]);
         //}
      }

      //[STAThread]
      //static void PrintToConsole(Map map, Player mew3, List<Ghost> ghosts, Dictionary<Location, short> colours)
      //{
      //   SafeFileHandle h = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

      //   if (!h.IsInvalid)
      //   {
      //      short xBound = Convert.ToInt16(map.Bounds.X);
      //      short yBound = Convert.ToInt16(map.Bounds.Y);

      //      CharInfo[] buf = new CharInfo[xBound * yBound];
      //      SmallRect rect = new SmallRect() { Left = 0, Top = 0, Right = xBound, Bottom = yBound };

      //      int x = 0;
      //      int y = 0;
      //      byte character = 0;
      //      short color = Colours.Cyan;

      //      for (int i = 0; i < buf.Length; ++i)
      //      {
      //         Ghost currGhost = ghosts.FirstOrDefault(g => g.Col == x && g.Row == y);
               
      //         if (currGhost != null)
      //         {
      //            if (currGhost.CurrentLoc == mew3.CurrentLoc)
      //            {
      //               color = Colours.Red;
      //               character = Convert.ToByte('X');
      //            }
      //            else
      //            {
      //               color = currGhost.Colour;
      //               character = Convert.ToByte(currGhost.Character);
      //            }
      //         }
      //         else if (x == mew3.Col && y == mew3.Row)
      //         {
      //            color = mew3.Colour;
      //            character = Convert.ToByte(mew3.Character);
      //            //if (map[x, y] != '@')
      //            if (map[x, y] != SpecialChars.Portal)
      //            {
      //               map[x, y] = ' ';
      //            }
      //         }
      //         else if (map[x, y] == '.' || map[x, y] == 'o')
      //         {
      //            color = Colours.Yellow;
      //            character = Convert.ToByte(map[x, y]);
      //         }
      //         //else if (map[x, y] == '@')
      //         else if (map[x, y] == SpecialChars.Portal)
      //         {
      //            color = colours[new Location(x, y)];
      //            //character = Convert.ToByte('@');
      //            character = Convert.ToByte(SpecialChars.Portal);
      //         }
      //         else
      //         {
      //            //Console.ForegroundColor = ConsoleColor.Cyan;
      //            //Console.Write(map[x, y]);
      //            color = Colours.Cyan;
      //            character = Convert.ToByte(map[x, y]);
      //         }

      //         buf[i].Attributes = color;
      //         buf[i].Char.AsciiChar = character;

      //         x++;
      //         if (x == xBound)
      //         {
      //            x = 0;
      //            y++;
      //         }
      //      }

      //      bool b = WriteConsoleOutput(h, buf,
      //         new Coord() { X = xBound, Y = yBound },
      //         new Coord() { X = 0, Y = 0 },
      //         ref rect);
      //   }
      //}

      #region Console Writing Members
      [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
      static extern SafeFileHandle CreateFile(
          string fileName,
          [MarshalAs(UnmanagedType.U4)] uint fileAccess,
          [MarshalAs(UnmanagedType.U4)] uint fileShare,
          IntPtr securityAttributes,
          [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
          [MarshalAs(UnmanagedType.U4)] int flags,
          IntPtr template);
      
      [DllImport("kernel32.dll", SetLastError = true)]
      static extern bool WriteConsoleOutput(
         SafeFileHandle hConsoleOutput, 
         CharInfo[] lpBuffer, 
         Coord dwBufferSize, 
         Coord dwBufferCoord, 
         ref SmallRect lpWriteRegion);
      
      [StructLayout(LayoutKind.Sequential)]
      public struct Coord
      {
         public short X;
         public short Y;
         
         public Coord(short X, short Y)
         {
            this.X = X;
            this.Y = Y;
         }
      };
      
      [StructLayout(LayoutKind.Explicit)]
      public struct CharUnion
      {
         [FieldOffset(0)] public char UnicodeChar;
         [FieldOffset(0)] public byte AsciiChar;
      }
      
      [StructLayout(LayoutKind.Explicit)]
      public struct CharInfo
      {
         [FieldOffset(0)] public CharUnion Char;
         [FieldOffset(2)] public short Attributes;
      }
      
      [StructLayout(LayoutKind.Sequential)]
      public struct SmallRect
      {
         public short Left;
         public short Top;
         public short Right;
         public short Bottom;
      }
      
#endregion 

      #region Key Capture Members
      [DllImport("user32.dll")]
      [return: MarshalAs(UnmanagedType.Bool)]
      static extern bool GetKeyboardState(byte [] lpKeyState);

      public static List<int> GetKeysState()
      {
         byte[] keys = new byte[256];
         List<int> pressedKeys = new List<int>();

         //Get pressed keys
         if (!GetKeyboardState(keys))
         {
            return null;
         }

         for (int i = 0; i < 256; i++)
         {
            byte key = keys[i];

            //Logical 'and' so we can drop the low-order bit for toggled keys, else that key will appear with the value 1!
            if ((key & 0x80) != 0)
            {
               pressedKeys.Add((int)key);
            }
         }
         return pressedKeys;
      }
      
      public ConsoleKey GetPressedKey()
      {
         var pressedKeys = GetKeysState();

         if (pressedKeys == null || pressedKeys.Count == 0)
         {
            return ConsoleKey.NoName;
         }

         var left = (int)ConsoleKey.LeftArrow;
         var right = (int)ConsoleKey.RightArrow;
         var up = (int)ConsoleKey.UpArrow;
         var down = (int)ConsoleKey.DownArrow;

         foreach (var key in pressedKeys)
         {
            if (key == left)
            {
               return ConsoleKey.LeftArrow;
            }

            if (key == right)
            {
               return ConsoleKey.RightArrow;
            }

            if (key == up)
            {
               return ConsoleKey.UpArrow;
            }

            if (key == down)
            {
               return ConsoleKey.DownArrow;
            }
         }

         return ConsoleKey.NoName;
      }

      public static byte GetVirtualKeyCode(ConsoleKey key) {
        int value = (int)key;
        return (byte)(value & 0xFF);
      }
      #endregion
   }      
}
