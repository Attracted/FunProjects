using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;

namespace MEW3
{
   public enum Direction
   {
      Up,
      Down,
      Left,
      Right
   }
   
   public class IntroSequence : IDisposable
   {
      private const int WINDOW_WIDTH = 20;
      private const int WINDOW_HEIGHT = 15;

      private Player Mew3;
      private Ghost Ghost;
      private System.Timers.Timer MoveTimer;
      public bool InProgress { get; private set; }
      Direction Mew3Direction = Direction.Down;
      Direction GhostDirection = Direction.Down;

      public IntroSequence()
      {
         //Mew3 = new Player(new Location(0, 0), 'M');
         //Ghost = new Ghost(new Location(0, -3), '!'); 
         Mew3 = new Player(new Location(0, 0), SpecialChars.Mew3_Down);
         Ghost = new Ghost(new Location(0, -3), SpecialChars.Ghost);

         InitializeWindow();

         CreateTimer();
      }

      private void InitializeWindow()
      {
         ConsoleMethods.SetConsoleSize(WINDOW_WIDTH, WINDOW_HEIGHT);
         DrawTitleLogo(2);
      }

      private void CreateTimer()
      {
         //MoveThread = new Thread(thread =>
         //{
            MoveTimer = new System.Timers.Timer();
            MoveTimer.Elapsed += OnTimerTick;
            MoveTimer.Interval = 100;
            MoveTimer.Start();
            InProgress = true;
         //});
      }

      private void OnTimerTick(object e, EventArgs args)
      {        
         if (Mew3.Row >= 0 && Mew3.Row < WINDOW_HEIGHT && Mew3.Col >= 0 && Mew3.Col < WINDOW_WIDTH)
         {
            ConsoleMethods.WriteText(" ", Mew3.Col, Mew3.Row);
         }
         if (Ghost.Row >= 0 && Ghost.Row < WINDOW_HEIGHT && Ghost.Col >= 0 && Ghost.Col < WINDOW_WIDTH)
         {
            ConsoleMethods.WriteText(" ", Ghost.Col, Ghost.Row);
         }

         MovePlayer(Mew3, ref Mew3Direction);
         MovePlayer(Ghost, ref GhostDirection);

         if (Mew3.Row >= 0 && Mew3.Row < WINDOW_HEIGHT && Mew3.Col >= 0 && Mew3.Col < WINDOW_WIDTH)
         {
            Console.ForegroundColor = ConsoleColor.Yellow;
            ConsoleMethods.WriteText(Mew3.Character, Mew3.Col, Mew3.Row);
         }
         if (Ghost.Row >= 0 && Ghost.Row < WINDOW_HEIGHT && Ghost.Col >= 0 && Ghost.Col < WINDOW_WIDTH)
         {
            Console.ForegroundColor = ConsoleColor.Red;
            ConsoleMethods.WriteText(Ghost.Character, Ghost.Col, Ghost.Row);
         }
         Console.ForegroundColor = ConsoleColor.Gray;
      }

      private void MovePlayer(Player player, ref Direction direction)
      {
         switch (direction)
         {
            case Direction.Up:
               player.Row--;
               if (player.Row == 1)
               {
                  direction = Direction.Left;
                  //if (player.Character == 'W')
                  //{
                  //   player.Character = '3';
                  //}
                  if (player.Character == SpecialChars.Mew3_Up)
                  {
                     player.Character = SpecialChars.Mew3_Left;
                  }
               }
               break;
            case Direction.Down:
               player.Row++;
               if (player.Row == 10)
               {
                  direction = Direction.Right;
                  //if (player.Character == 'M')
                  //{
                  //   player.Character = 'E';
                  //}
                  if (player.Character == SpecialChars.Mew3_Down)
                  {
                     player.Character = SpecialChars.Mew3_Right;
                  }
               }
               break;
            case Direction.Left:
               player.Col--;
               if (player.Col == 0)
               {
                  direction = Direction.Down;
                  //if (player.Character == '3')
                  //{
                  //   player.Character = 'M';
                  //}
                  if (player.Character == SpecialChars.Mew3_Left)
                  {
                     player.Character = SpecialChars.Mew3_Down;
                  }
               }
               break;
            case Direction.Right:
               player.Col++;
               if (player.Col == WINDOW_WIDTH - 1)
               {
                  direction = Direction.Up;
                  //if (player.Character == 'E')
                  //{
                  //   player.Character = 'W';
                  //}
                  if (player.Character == SpecialChars.Mew3_Right)
                  {
                     player.Character = SpecialChars.Mew3_Up;
                  }
               }
               break;
         }
      }

      private void DrawTitleLogo(int line)
      {

         var ghost = new string[] {
            " __ ",
            "/  \\",
            "\\  /",
            " )( ",
            " || ",
            " \\/ ",
            "    ",
            " () "
         };

         var mew3 = new string[] {
            "  _______",
            " / _____ \\",
            "| /     \\_)",
            "| \\___     _",
            "|  ___)   (_)",
            "| /      _",
            "| \\_____/ )",
            " \\_______/"
         };

         Console.SetCursorPosition(0, line);
         
         for (int i = 0; i < ghost.Length; i++)
         {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(string.Format(" {0} ", ghost[i]));
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(mew3[i]);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
         }
      }

      private void DisposeTimer()
      {
         MoveTimer.Stop();
         MoveTimer.Elapsed -= OnTimerTick;
         MoveTimer.Dispose();
      }

      public void Pause()
      {
         MoveTimer.Stop();
      }

      public void Continue()
      {
         Console.Clear();
         InitializeWindow();
         MoveTimer.Start();
      }

      #region IDisposable Members

      public void Dispose()
      {
         DisposeTimer();
         InProgress = false;
      }

      #endregion
   }
}