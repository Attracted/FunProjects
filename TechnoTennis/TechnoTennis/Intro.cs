using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
using TechnoTennis.Properties;
using System.Drawing;

namespace TechnoTennis
{
   public enum Direction
   {
      None,
      Up,
      Down,
      Left,
      Right
   }
   
   public class Intro : IDisposable
   {
      private int HighScore;
      
      private Player Snake;
      private ScreenObject CurrentBit;

      private int RespawnTimer = 0;

      private System.Timers.Timer MoveTimer;
      public bool InProgress { get; private set; }

      public Intro(int highScore)
      {
         HighScore = highScore;
         
         MakeSnake();
         MakeBit();

         InitializeWindow();

         CreateTimer();
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

      private void MakeSnake()
      {
         Snake = new Player(new Location(GlobalValues.PATH_LOCATION.Left,
            GlobalValues.PATH_LOCATION.Top + GlobalValues.PATH_LOCATION.Height / 2), 4);

         Snake.LeftMostPiece.Colour = SpecialColours.Player;
      }
      private void MakeBit()
      {
         CurrentBit = new ScreenObject(new Location(GlobalValues.PATH_LOCATION.Right,
            GlobalValues.PATH_LOCATION.Top + GlobalValues.PATH_LOCATION.Height / 2), SpecialChars.Bit, SpecialColours.Bit);
      }
      private void OnTimerTick(object e, EventArgs args)
      {
         MovePlayer();
         UpdateConsole();
      }

      private void UpdateConsole()
      {
         if (CurrentBit != null)
         {
            ConsoleMethods.WriteTextInBorder(CurrentBit.Character, CurrentBit.Location, SpecialColours.Yellow);

         }

         if (Snake.PieceToAdd.HasValue)
         {
            Location addLoc = Snake.PieceToAdd.Value;
            ConsoleMethods.WriteTextInBorder(Snake.LeftMostPiece.Character, addLoc, SpecialColours.Player);
         }

         if (Snake.PieceToDelete.HasValue)
         {
            Location delLoc = Snake.PieceToDelete.Value;
            ConsoleMethods.WriteTextInBorder(' ', delLoc);
         }
      }
      private void MovePlayer()
      {
         int col = Snake.LeftMostPiece.Location.X;
         int row = Snake.LeftMostPiece.Location.Y;
         
         switch (Snake.MovementDirection)
         {
            case Direction.Up:
               Snake.Move();
               if (row == GlobalValues.PATH_LOCATION.Top)
               {
                  Snake.MovementDirection = Direction.Left;
               }
               break;
            case Direction.Left:
               Snake.Move();
               if (col == GlobalValues.PATH_LOCATION.Left)
               {
                  Snake.MovementDirection = Direction.Down;
               }
               break;
            case Direction.Down:
               Snake.Move();
               if (row == GlobalValues.PATH_LOCATION.Bottom)
               {
                  Snake.MovementDirection = Direction.Right;
               }
               break;
            case Direction.Right:
               Snake.Move();
               if (col == GlobalValues.PATH_LOCATION.Right)
               {
                  Snake.MovementDirection = Direction.Up;
               }
               break;
         }

         if (CurrentBit != null && Snake.LeftMostPiece.Location == CurrentBit.Location)
         {
            CurrentBit = null;
            RespawnTimer = 5;
         }
         else if (RespawnTimer == 0)
         {
            MakeBit();
         }
         else
         {
            RespawnTimer--;
         }
      }
      private void DrawTitleLogo()
      {
         ConsoleMethods.DrawAppBorder();

         var body = new string[] {
            @" _____",
            @"|     |",
            @"|     |",
            @"|     |",
            @"|     |",
            @"|     |",
            @"|_____|"
         };

         var snake = new string[] {            
            @"|\",
            @"| \",
            @"|  \",
            @"|   \",
            @"|   /",
            @"|  /",
            @"| /",
            @"|/"
         };

         ConsoleMethods.WriteTextInBorder(body, new Location(GlobalValues.PATH_LOCATION.Left + 1, GlobalValues.PATH_LOCATION.Top + 1), SpecialColours.Player);
         ConsoleMethods.WriteTextInBorder(snake, new Location(GlobalValues.PATH_LOCATION.Left + 8, GlobalValues.PATH_LOCATION.Top + 1), SpecialColours.Player);
      }
      private void InitializeWindow()
      {
         ConsoleMethods.SetConsoleSize(GlobalValues.WINDOW_WIDTH, GlobalValues.WINDOW_HEIGHT);
         DrawTitleLogo();
         ConsoleMethods.CentreTextInBorder(string.Format("High Score: {0}", HighScore.ToString()), 1, ConsoleColor.Gray);
      }
      private void CreateTimer()
      {
         MoveTimer = new System.Timers.Timer();
         MoveTimer.Elapsed += OnTimerTick;
         MoveTimer.Interval = 100;
         MoveTimer.Start();
         InProgress = true;
      }

      private void DisposeTimer()
      {
         MoveTimer.Stop();
         MoveTimer.Elapsed -= OnTimerTick;
         MoveTimer.Dispose();
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