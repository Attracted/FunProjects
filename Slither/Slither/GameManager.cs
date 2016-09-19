using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slither.Properties;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Slither
{
   public class GameManager
   {
      const int TIMER_INTERVAL = 150;
      const int TIMER_INTERVAL_SLOW = 350;
      const int TIMER_DELAY = 0;

      private Map GameMap;
      private System.Timers.Timer MoveTimer;
      private Thread TimerThread;
      private Map Map;
      private Player Snake;
      private bool Started = false;

      private ScreenObject CurrentBit;
      private ScreenObject LastMine;
      private Random Seed;

      public int Score { get; set; }

      public GameManager()
      {
         Console.CursorVisible = false;
         Console.Clear();
         Initialize();
      }

      private void Initialize(bool newGame = true)
      {
         if (newGame)
         {
            Map = new Map(Application.SplitString(Resources.GameMap));
            Seed = new Random();
         }

         GameMap = new Map(Map);

         Score = 0;
         Snake = new Player(Map.SnakeStartLoc, Direction.Right);
         CurrentBit = null;
         MakeBitAndMine();

         ReadyUserAndPrintGame(750);

         StartTimerThread(TIMER_DELAY);
      }

      private void ReadyUserAndPrintGame(int delays = 0)
      {        
         ConsoleMethods.DrawAppBorder();

         ConsoleMethods.ClearPromptLines();

         PrintToConsole();

         if (delays > 0)
         {
            ConsoleMethods.CentreText("Get Ready!", GlobalValues.PROMPT_LINE);
            Thread.Sleep(delays);

            ConsoleMethods.ClearPromptLines();
         }
      }

      protected virtual void StartTimerThread(int delay)
      {
         TimerThread = new Thread(thread =>
         {
            MoveTimer = new System.Timers.Timer();
            MoveTimer.Elapsed += OnTimerTick;
            MoveTimer.Interval = TIMER_INTERVAL;
            Thread.Sleep(delay);
            MoveTimer.Start();
         });

         TimerThread.Start();
      }

      protected virtual void StopTimerThread()
      {
         MoveTimer.Stop();
         MoveTimer.Elapsed -= OnTimerTick;
         MoveTimer.Dispose();

         TimerThread.Abort();
      }

      private void OnTimerTick(object e, EventArgs args)
      {
         if (!Started)
         {
            ConsoleMethods.WriteScore(Score);
            Started = true;
         }

         MovePlayer();
         MakeBitAndMine();
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

         switch (key)
         {
            case ConsoleKey.LeftArrow:
               Snake.MovementDirection = Direction.Left;
               break;
            case ConsoleKey.RightArrow:
               Snake.MovementDirection = Direction.Right;
               break;
            case ConsoleKey.UpArrow:
               Snake.MovementDirection = Direction.Up;
               break;
            case ConsoleKey.DownArrow:
               Snake.MovementDirection = Direction.Down;
               break;
            //case ConsoleKey.Insert:
            //   Snake.AddBlock();
            //   break;
            //case ConsoleKey.Delete:
            //   Snake.RemoveBlock();
            //   break;
            case ConsoleKey.NoName:
               return true;
            case ConsoleKey.Enter:
               return PauseGame();
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
         StopTimerThread();

         while (true)
         {
            var choicesList = new List<string>() { "Continue", "Exit" };
            var choice = ConsoleMethods.PromptHorizontalSelection("Game paused.", choicesList);
            if (choice == "Continue")
            {
               ReadyUserAndPrintGame();
               StartTimerThread(250);
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

      private void AteBit()
      {
         Score++;
         Snake.AddBlock();
         CurrentBit = null;
         ConsoleMethods.WriteScore(Score);
      }

      private void MakeBitAndMine()
      {
         if (CurrentBit == null)
         {
            List<Location> available = new List<Location>();
            
            for (int y = 0; y < GameMap.BoundsIndex.Y; y++)
            {
               for (int x = 0; x < GameMap.BoundsIndex.X; x++)
               {
                  char thisChar = GameMap[x, y];
                  if(thisChar == ' ')
                  {
                     available.Add(new Location(x, y));
                  }
               }
            }

            int seedIndex = Seed.Next(0, available.Count);
            Location newBitLoc = available[seedIndex];
            CurrentBit = new ScreenObject(newBitLoc, SpecialChars.Bit, SpecialColours.Yellow);
            GameMap[newBitLoc.X, newBitLoc.Y] = SpecialChars.Bit;
            
            available.RemoveAt(seedIndex);

            seedIndex = Seed.Next(0, available.Count);
            Location newMineLoc = available[seedIndex];
            LastMine = new ScreenObject(newMineLoc, SpecialChars.Mine, SpecialColours.Red);
            GameMap[LastMine.Location.X, LastMine.Location.Y] = SpecialChars.Mine;
         }
      }
      private void MovePlayer()
      {
         int col = Snake.Head.Location.X;
         int row = Snake.Head.Location.Y;
         
         switch (Snake.MovementDirection)
         {
            case Direction.Up:
               if (row <= 0)
               {
                  Snake.MoveTo(col, GameMap.BoundsIndex.Y - 1, SpecialChars.Player_Up);
               }
               else
               {
                  Snake.MoveTo(col, --row, SpecialChars.Player_Up);
               }
               break;
            case Direction.Down:
               if (row >= GameMap.BoundsIndex.Y - 1)
               {
                  Snake.MoveTo(col, 0, SpecialChars.Player_Down);
               }
               else
               {
                  Snake.MoveTo(col, ++row, SpecialChars.Player_Down);
               }
               break;
            case Direction.Left:
               if (col <= 0)
               {
                  Snake.MoveTo(GameMap.BoundsIndex.X - 1, row, SpecialChars.Player_Left);
               }
               else
               {
                  Snake.MoveTo(--col, row, SpecialChars.Player_Left);
               }
               break;
            case Direction.Right:
               if (col >= GameMap.BoundsIndex.X - 1)
               {
                  Snake.MoveTo(0, row, SpecialChars.Player_Right);
               }
               else
               {
                  Snake.MoveTo(++col, row, SpecialChars.Player_Right);
               }
               break;
            default:
               break;
         }

         UpdateGameMap();

         if(Snake.Head.Location == CurrentBit.Location)
         {
            AteBit();
         }
      }

      private void UpdateGameMap()
      {         
         int xHead = Snake.Head.Location.X;
         int yHead = Snake.Head.Location.Y;
         
         char headLocChar = GameMap[xHead, yHead];

         Location? tailLoc = Snake.TailPrevLoc;

         if (headLocChar == SpecialChars.Mine)
         {
            Snake.Collision = true;

            GameMap[Snake.HeadPrevLoc.X, Snake.HeadPrevLoc.Y] = SpecialChars.Player_Body;
            if (tailLoc != null)
            {
               GameMap[tailLoc.Value.X, tailLoc.Value.Y] = ' ';
            }
         }
         else if (headLocChar == SpecialChars.Player_Body)
         {
            Snake.Collision = true;

            if (tailLoc != null)
            {
               GameMap[tailLoc.Value.X, tailLoc.Value.Y] = ' ';
            }
            GameMap[Snake.HeadPrevLoc.X, Snake.HeadPrevLoc.Y] = SpecialChars.Player_Body;
            GameMap[xHead, yHead] = Snake.Head.Character;
         }
         else
         {
            GameMap[xHead, yHead] = Snake.Head.Character;
            GameMap[Snake.HeadPrevLoc.X, Snake.HeadPrevLoc.Y] = SpecialChars.Player_Body;

            if (tailLoc != null)
            {
               GameMap[tailLoc.Value.X, tailLoc.Value.Y] = ' ';
            }
         }
      }

      private bool CheckContinue()
      {
         bool cont = true;

         if (Snake.Collision) 
         {
            if (!CheckRetry())
            {
               cont = false;
            }
         }

         return cont;
      }

      protected virtual bool CheckRetry()
      {
         StopTimerThread();

         ConsoleMethods.CentreText("Nice Try!", GlobalValues.CHOICES_LINE + 1);
         Thread.Sleep(2000);

         return false;
      }

      private static void DrawGrid(SquareGrid grid, AStarSearch astar)
      {
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

      public void UpdateConsole()
      {
         if (CurrentBit != null)
         {
            ConsoleMethods.WriteTextInBorder(GameMap[CurrentBit.Location.X, CurrentBit.Location.Y], CurrentBit.Location, SpecialColours.Yellow);

         }

         ConsoleMethods.WriteTextInBorder(GameMap[Snake.HeadPrevLoc.X, Snake.HeadPrevLoc.Y], Snake.HeadPrevLoc, SpecialColours.Player);

         if (Snake.TailPrevLoc != null)
         {
            Location tailPrevLoc = Snake.TailPrevLoc.Value;
            if (GameMap[tailPrevLoc.X, tailPrevLoc.Y] == ' ')
            {
               ConsoleMethods.WriteTextInBorder(Map[tailPrevLoc.X, tailPrevLoc.Y], tailPrevLoc);
            }
         }         

         ScreenObject head = Snake.Head;
         if (!Snake.Collision)
         {
            ConsoleMethods.WriteTextInBorder(GameMap[head.Location.X, head.Location.Y], head.Location, head.Colour);
         }
         else
         {
            ConsoleMethods.WriteTextInBorder(GameMap[head.Location.X, head.Location.Y], head.Location, SpecialColours.Death);
         }

         if (LastMine != null)
         {
            ConsoleMethods.WriteTextInBorder(GameMap[LastMine.Location.X, LastMine.Location.Y], LastMine.Location, LastMine.Colour);
         }
      }

      public void PrintToConsole()
      {   
         for (int x = 0; x < GameMap.BoundsIndex.X; x++)
         {
            for (int y = 0; y < GameMap.BoundsIndex.Y; y++)
            {
               ConsoleMethods.WriteTextInBorder(GameMap[x, y], new Location(x, y), SpecialColours.Walls);
            }
         }

         ConsoleMethods.WriteTextInBorder(Snake.Head.Character, Snake.Head.Location, Snake.Head.Colour);
         ConsoleMethods.WriteTextInBorder(CurrentBit.Character, CurrentBit.Location, CurrentBit.Colour);
         ConsoleMethods.WriteTextInBorder(LastMine.Character, LastMine.Location, LastMine.Colour);
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
