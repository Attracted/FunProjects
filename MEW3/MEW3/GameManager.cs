using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MEW3.Properties;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace MEW3
{
   public class GameManager
   {
      const int TIMER_INTERVAL = 200;
      const int TIMER_DELAY = 1000;

      protected Player Mew3;
      //private Player Ghost;
      private List<Ghost> Ghosts = new List<Ghost>();
      private Map GameMap;
      private System.Timers.Timer MoveGhostsTimer;
      private Thread GhostsThread;

      private Dictionary<Location, short> PortalColours;

      private bool CameFromPortal;

      private Map Map;

      public GameManager(string[] map)
      {
         Console.CursorVisible = false;
         Console.Clear();
         this.Map = new Map(map);
         Initialize();
      }

      public GameManager(Map map)
      {
         Console.CursorVisible = false;
         Console.Clear();
         this.Map = map;
         Initialize();
      }

      private void Initialize()
      {
         GameMap = new Map(Map);

         //Mew3 = new Player(GameMap.Mew3StartLoc, 'M');
         Mew3 = new Player(GameMap.Mew3StartLoc, SpecialChars.Mew3_Right);
         Mew3.Colour = Colours.Yellow;
         Ghosts.Clear();

         foreach (Location ghostStartLoc in GameMap.GhostStartLocs)
         {
            //var ghost = new Ghost(ghostStartLoc, '!');
            var ghost = new Ghost(ghostStartLoc, SpecialChars.Ghost);
            ghost.Immune = true;
            ghost.Colour = Colours.Red;

            Ghosts.Add(ghost);
         }

         PortalColours = new Dictionary<Location, short>();
         int numPortals = 0;
         
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
               }
            }
         }

         //PrintToConsole(GameMap, Mew3, Ghost, PortalColours);
         //PrintToConsole(GameMap, Mew3, Ghosts, PortalColours);

         ReadyUserAndPrintGame();

         StartGhostsThread(TIMER_DELAY);
      }

      private void ReadyUserAndPrintGame()
      {
         int halfWidth = GameMap.Bounds.X / 2;
         int halfHeight = GameMap.Bounds.Y / 2;

         ConsoleMethods.SetConsoleSize(GameMap.Bounds.X, GameMap.Bounds.Y);
         Console.Clear();
         Console.SetCursorPosition(halfWidth - 5, halfHeight);
         Console.WriteLine("Get Ready!");
         Thread.Sleep(1000);
         Console.Clear();
         PrintToConsole(GameMap, Mew3, Ghosts, PortalColours);

         Console.SetCursorPosition(halfWidth - 2, halfHeight);
         
         Console.ForegroundColor = ConsoleColor.Red;
         Console.BackgroundColor = ConsoleColor.White;
         Console.WriteLine("|GO!|");
         Console.BackgroundColor = ConsoleColor.Black;
         Console.ForegroundColor = ConsoleColor.Gray;

         Console.SetCursorPosition(0, 0);
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
         MoveGhosts();
         PrintToConsole(GameMap, Mew3, Ghosts, PortalColours);
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
         var key = Console.ReadKey(true).Key;
         //var key = GetPressedKey();

         if (!CheckContinue())
         {
            return false;
         }

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

         //WriteToScreen(map, mew3.Character, x, y);
         //WriteToScreen();
         //PrintToConsole(GameMap, Mew3, Ghost, PortalColours);
         PrintToConsole(GameMap, Mew3, Ghosts, PortalColours);

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
            var choicesList = new List<string>() { "Continue", "Exit (to menu)" };
            var choice = ConsoleMethods.PromptSelection("Game paused.", choicesList);
            if (choice == "Continue")
            {
               Console.Clear();
               ReadyUserAndPrintGame();
               StartGhostsThread(250);
               return true;
            }
            else if (choice == "Exit (to menu)")
            {
               choicesList = new List<string>() { "Yes", "No" };

               choice = ConsoleMethods.PromptSelection("Are you sure you want to exit?", choicesList);
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

         var currentLoc = Mew3.CurrentLoc;
         var currentPortal = GameMap.Grid.GetOtherPortalOrDefault(currentLoc);
         var nextLoc = new Location(x, y);
         var nextPortal = GameMap.Grid.GetOtherPortalOrDefault(nextLoc);

         if (nextPortal != nextLoc && !CameFromPortal) 
            //&& (y > 0 && y < GameMap.Bounds.y - 1 && x > 0 && x < GameMap.Bounds.x - 1))
         {
            var newLoc = new Location(nextPortal.X + xDiff, nextPortal.Y + yDiff);

            if (GameMap.Grid.InBounds(newLoc) && GameMap.Grid.Passable(newLoc))
            {
               Mew3.Col = newLoc.X;
               Mew3.Row = newLoc.Y;
               CameFromPortal = false;
            }
            else if (GameMap.Grid.InBounds(nextLoc) && GameMap.Grid.Passable(nextLoc))
            {
               Mew3.Col = nextPortal.X;
               Mew3.Row = nextPortal.Y;
               CameFromPortal = true;
            }
            else
            {
               CameFromPortal = false;
            }
            //GameMap[y, x] = '@';
         }
         else
         {
            CameFromPortal = false;

            if (GameMap.Grid.InBounds(nextLoc) && GameMap.Grid.Passable(nextLoc))
            {
               var tmp = GameMap[x, y];
               Mew3.Col = x;
               Mew3.Row = y;

               if (tmp == 'o')
               {
                  //Ghost.Immune = false;
                  foreach (Ghost ghost in Ghosts)
                  {
                     ghost.Immune = false;
                  }
               }
            }
         }
      }

      //private void MoveGhost()
      //{
      //   var opploc = new Location(Ghost.Col, Ghost.Row);
      //   var mew3loc = new Location(Mew3.Col, Mew3.Row);
      //   var startloc = Ghost.StartLoc;

      //   var newloc = opploc;

      //   if (Mew3.Immune && !Ghost.Immune)
      //   {
      //      var grid = GameMap.Grid;

      //      grid.walls.Add(mew3loc);

      //      var astar = new AStarSearch(grid, startloc, opploc);
      //      if (astar.cameFrom.ContainsKey(opploc))
      //      {
      //         newloc = astar.cameFrom[opploc];
      //      }

      //      grid.walls.Remove(mew3loc);
      //   }
      //   else if (!Mew3.Immune && Ghost.Immune)
      //   {
      //      var astar = new AStarSearch(GameMap.Grid, mew3loc, opploc);
      //      if (astar.cameFrom.ContainsKey(opploc))
      //      {
      //         newloc = astar.cameFrom[opploc];
      //      }
      //   }

      //   Ghost.Col = newloc.x;
      //   Ghost.Row = newloc.y;
      //}

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

               ghost.Col = newloc.X;
               ghost.Row = newloc.Y;
            }
            else
            {
               ghost.DecrementCounter();
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

      private Location OppositeDir(Location currloc, Location runawayloc)
      {
         Location bestloc = currloc;
         int bestdiff = Location.Diff(currloc,runawayloc);
         
         foreach (var loc in GameMap.Grid.Neighbors(currloc))
         {
            var diff = Location.Diff(loc, runawayloc);
            if (diff > bestdiff) bestloc = loc;
         }
         
         return bestloc;
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
            Ghost ghost = Ghosts.FirstOrDefault(g => g.Row == Mew3.Row && g.Col == Mew3.Col);
            
            if (ghost != null)
            {
               if (!ghost.Immune)
               {
                  ghost.ResetLocation();
                  ghost.SetCounter();
                  ghost.Immune = true;

                  //MoveGhostsTimer.Change(TIMER_DELAY, TIMER_INTERVAL);
               }
               else
               {
                  cont = false;
               }
            } 
         }
         //else if (Ghost.Col == Ghost.StartLoc.x && Ghost.Row == Ghost.StartLoc.y && Mew3.Immune && !Ghost.Immune)
         //{
         //   Mew3.Immune = false;
         //   Ghost.Immune = true;
         //}
         foreach (Ghost ghost in Ghosts.FindAll(g => g.CurrentLoc == g.StartLoc && g.PenaltyCounter == 0))
         {
            ghost.Immune = true;
         }

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

         var choicesList = new List<string>() { "Again!", "Exit (to menu)" };
         string choice = ConsoleMethods.PromptSelection("Play again?", choicesList);
         if (choice == "Yes!")
         {
            Initialize();
            return true;
         }
         else if (choice == "Exit (to menu)")
         {
            return false;
         }

         return false;
      }

      protected void DeathMessage(bool win)
      {
         // Display Sweet Death Message;
         int deathCol = Mew3.Col;
         Console.SetCursorPosition(deathCol, Mew3.Row);
         Console.CursorVisible = true;
         Thread.Sleep(2000);
         Console.CursorVisible = false;

         string message = win ? "You won!" : "Nice try!";
         int messageLen = message.Length;

         if (deathCol + messageLen < Console.WindowWidth)
         {
            Console.SetCursorPosition(deathCol + 1, Mew3.Row);
         }
         else if (deathCol - messageLen >= 0)
         {
            Console.SetCursorPosition((deathCol - messageLen), Mew3.Row);
         }


         Console.BackgroundColor = ConsoleColor.DarkRed;
         Console.ForegroundColor = ConsoleColor.Gray;
         Console.Write(win ? "You won!" : "Nice try!");
         Console.BackgroundColor = ConsoleColor.Black;
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
      static void PrintToConsole(Map map, Player mew3, List<Ghost> ghosts, Dictionary<Location, short> colours)
      {
         SafeFileHandle h = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

         if (!h.IsInvalid)
         {
            short xBound = Convert.ToInt16(map.Bounds.X);
            short yBound = Convert.ToInt16(map.Bounds.Y);

            CharInfo[] buf = new CharInfo[xBound * yBound];
            SmallRect rect = new SmallRect() { Left = 0, Top = 0, Right = xBound, Bottom = yBound };

            int x = 0;
            int y = 0;
            byte character = 0;
            short color = Colours.Cyan;

            for (int i = 0; i < buf.Length; ++i)
            {
               Ghost currGhost = ghosts.FirstOrDefault(g => g.Col == x && g.Row == y);
               
               if (currGhost != null)
               {
                  if (currGhost.CurrentLoc == mew3.CurrentLoc)
                  {
                     color = Colours.Red;
                     character = Convert.ToByte('X');
                  }
                  else
                  {
                     color = currGhost.Colour;
                     character = Convert.ToByte(currGhost.Character);
                  }
               }
               else if (x == mew3.Col && y == mew3.Row)
               {
                  color = mew3.Colour;
                  character = Convert.ToByte(mew3.Character);
                  //if (map[x, y] != '@')
                  if (map[x, y] != SpecialChars.Portal)
                  {
                     map[x, y] = ' ';
                  }
               }
               else if (map[x, y] == '.' || map[x, y] == 'o')
               {
                  color = Colours.Yellow;
                  character = Convert.ToByte(map[x, y]);
               }
               //else if (map[x, y] == '@')
               else if (map[x, y] == SpecialChars.Portal)
               {
                  color = colours[new Location(x, y)];
                  //character = Convert.ToByte('@');
                  character = Convert.ToByte(SpecialChars.Portal);
               }
               else
               {
                  //Console.ForegroundColor = ConsoleColor.Cyan;
                  //Console.Write(map[x, y]);
                  color = Colours.Cyan;
                  character = Convert.ToByte(map[x, y]);
               }

               buf[i].Attributes = color;
               buf[i].Char.AsciiChar = character;

               x++;
               if (x == xBound)
               {
                  x = 0;
                  y++;
               }
            }

            bool b = WriteConsoleOutput(h, buf,
               new Coord() { X = xBound, Y = yBound },
               new Coord() { X = 0, Y = 0 },
               ref rect);
         }
      }

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
