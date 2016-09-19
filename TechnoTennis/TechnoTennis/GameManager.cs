using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnoTennis.Properties;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace TechnoTennis
{
   public class GameManager
   {
      const int TIMER_INTERVAL = 50;
      const int TIMER_INTERVAL_SLOW = 350;
      const int TIMER_DELAY = 0;

      private Map GameMap;
      private System.Timers.Timer MoveTimer;
      private Thread TimerThread;
      private Map Map;
      private Player ManualPlayer;
      private Player AutoPlayer;
      private bool Started = false;
      private bool Continue = true;

      private Ball CurrentBit;
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
         ManualPlayer = new Player(Map.ManualPlayerStartLoc, 4);
         CurrentBit = new Ball(new Location(0,0), SpecialChars.Bit, SpecialColours.Bit);

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


      private static readonly object TimerTickLock = new object();
      private void OnTimerTick(object e, EventArgs args)
      {
         if (!Started)
         {
            ConsoleMethods.WriteScore(Score);
            Started = true;
         }

         lock (TimerTickLock)
         {
            if (Console.KeyAvailable)
            {
               ConsoleKey key = Console.ReadKey(true).Key;

               if (key == ConsoleKey.LeftArrow)
               {
                  ManualPlayer.MovementDirection = Direction.Left;
               }
               else if (key == ConsoleKey.RightArrow)
               {
                  ManualPlayer.MovementDirection = Direction.Right;
               }
               else
               {
                  ManualPlayer.MovementDirection = Direction.None;

                  if (key == ConsoleKey.Enter)
                  {
                     Continue = PauseGame();
                  }
               }
            }

            MovePieces();
         }
      }

      public bool Go()
      {
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

      private void MovePieces()
      {
         ManualPlayer.Move();
         UpdateGameMap();

         MoveBit();
         UpdateConsole();
      }

      public void MoveBit()
      {
         CurrentBit.XPosition += CurrentBit.XVelocity / 10;
         CurrentBit.YPosition += CurrentBit.YVelocity / 10;

         int x = (int)Math.Round(CurrentBit.XPosition);
         int y = (int)Math.Round(CurrentBit.YPosition);

         if (x < 0 || x > GameMap.BoundsIndex.X - 1)
         {
            CurrentBit.XPosition -= CurrentBit.XVelocity / 10;
            CurrentBit.XVelocity = 0 - CurrentBit.XVelocity;
            CurrentBit.XPosition += CurrentBit.XVelocity / 10;
            x = (int)Math.Round(CurrentBit.XPosition);
            y = (int)Math.Round(CurrentBit.YPosition);
         }
         // TEMPORARY
         if (y < 0 || y > GameMap.BoundsIndex.Y - 1)
         {
            CurrentBit.YPosition -= CurrentBit.YVelocity / 10;
            CurrentBit.YVelocity = 0 - CurrentBit.YVelocity;
            CurrentBit.YPosition += CurrentBit.YVelocity / 10;
            x = (int)Math.Round(CurrentBit.XPosition);
            y = (int)Math.Round(CurrentBit.YPosition);
         }

         if (GameMap[x, y] == ' ')
         {
            CurrentBit.PieceToDelete = CurrentBit.Location;
            CurrentBit.Location = new Location(x, y);
         }
         else
         {
            CurrentBit.YVelocity = 0 - CurrentBit.YVelocity;
            CurrentBit.XPosition -= CurrentBit.XVelocity / 10;
            MoveBit();
         }
      }

      private void UpdateGameMap()
      {         
         if (ManualPlayer.PieceToAdd.HasValue)
         {
            Location addLoc = ManualPlayer.PieceToAdd.Value;
            GameMap[addLoc.X, addLoc.Y] = ManualPlayer.LeftMostPiece.Character;
         }

         if (ManualPlayer.PieceToDelete.HasValue)
         {
            Location delLoc = ManualPlayer.PieceToDelete.Value;
            GameMap[delLoc.X, delLoc.Y] = ' ';
         }

         Location newLoc = CurrentBit.Location;
         GameMap[newLoc.X, newLoc.Y] = CurrentBit.Character;

         if (CurrentBit.PieceToDelete.HasValue)
         {
            Location delLoc = CurrentBit.PieceToDelete.Value;
            GameMap[delLoc.X, delLoc.Y] = ' ';
         }
      }

      private bool CheckContinue()
      {
         //bool cont = true;

         //if (ManualPlayer.Collision) 
         //{
         //   if (!CheckRetry())
         //   {
         //      cont = false;
         //   }
         //}

         return Continue;
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
         if (ManualPlayer.PieceToAdd.HasValue)
         {
            Location addLoc = ManualPlayer.PieceToAdd.Value;
            ConsoleMethods.WriteTextInBorder(ManualPlayer.LeftMostPiece.Character, addLoc, SpecialColours.Player);
         }

         if (ManualPlayer.PieceToDelete.HasValue)
         {
            Location delLoc = ManualPlayer.PieceToDelete.Value;
            ConsoleMethods.WriteTextInBorder(' ', delLoc);
         }

         if (CurrentBit.PieceToDelete.HasValue)
         {
            Location delLoc = CurrentBit.PieceToDelete.Value;
            ConsoleMethods.WriteTextInBorder(' ', delLoc);
         }

         ConsoleMethods.WriteTextInBorder(CurrentBit.Character, CurrentBit.Location, CurrentBit.Colour);
      }

      public void PrintToConsole()
      {   
         int xLoc = ManualPlayer.LeftMostPiece.Location.X;
         int yLoc = ManualPlayer.LeftMostPiece.Location.Y;

         for (int i = 0; i < ManualPlayer.LengthIndex + 1; i++)
         {
            ConsoleMethods.WriteTextInBorder(ManualPlayer.LeftMostPiece.Character, new Location(xLoc + i, yLoc), ManualPlayer.LeftMostPiece.Colour);
         }
         //ConsoleMethods.WriteTextInBorder(ManualPlayer.LeftMostPiece.Character, ManualPlayer.LeftMostPiece.Location, ManualPlayer.LeftMostPiece.Colour);
         ConsoleMethods.WriteTextInBorder(CurrentBit.Character, CurrentBit.Location, CurrentBit.Colour);
         //ConsoleMethods.WriteTextInBorder(LastMine.Character, LastMine.Location, LastMine.Colour);
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
