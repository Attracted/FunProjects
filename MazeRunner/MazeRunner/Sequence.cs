using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
using MazeRunner.Properties;
using System.Drawing;

namespace MazeRunner
{
   public enum Direction
   {
      Up,
      Down,
      Left,
      Right
   }
   
   public abstract class Sequence : IDisposable
   {
      private int HighScore;
      
      private Player Mew3;
      private Ghost Ghost;
      private System.Timers.Timer MoveTimer;
      public bool InProgress { get; private set; }
      Direction Mew3Direction = Direction.Right;
      Direction GhostDirection = Direction.Right;

      public static Rectangle PathRectangle = new Rectangle(4, 10, 21, 12);

      public Sequence(int highScore)
      {
         HighScore = highScore;
         
         Mew3 = MakeMew3();
         Ghost = MakeGhost();

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

      protected abstract Mew3 MakeMew3();
      protected abstract Ghost MakeGhost();
      protected abstract void MovePlayer(Player player, ref Direction direction);
      protected abstract void DrawTitleLogo();

      private void InitializeWindow()
      {
         ConsoleMethods.SetConsoleSize(GlobalValues.WINDOW_WIDTH, GlobalValues.WINDOW_HEIGHT);
         DrawTitleLogo();
         ConsoleMethods.CentreText(string.Format("High Score: {0}", HighScore.ToString()), GlobalValues.BORDER_LOCATION.Top + 3);
      }
      private void CreateTimer()
      {
         MoveTimer = new System.Timers.Timer();
         MoveTimer.Elapsed += OnTimerTick;
         MoveTimer.Interval = 100;
         MoveTimer.Start();
         InProgress = true;
      }
      private void OnTimerTick(object e, EventArgs args)
      {
         if (Mew3.Row >= 0 && Mew3.Row < GlobalValues.WINDOW_HEIGHT && Mew3.Col >= 0 && Mew3.Col < GlobalValues.WINDOW_WIDTH)
         {
            ConsoleMethods.WriteText(" ", Mew3.Col, Mew3.Row);
         }
         if (Ghost.Row >= 0 && Ghost.Row < GlobalValues.WINDOW_HEIGHT && Ghost.Col >= 0 && Ghost.Col < GlobalValues.WINDOW_WIDTH)
         {
            ConsoleMethods.WriteText(" ", Ghost.Col, Ghost.Row);
         }

         MovePlayer(Mew3, ref Mew3Direction);
         MovePlayer(Ghost, ref GhostDirection);

         if (Mew3.Row >= 0 && Mew3.Row < GlobalValues.WINDOW_HEIGHT && Mew3.Col >= 0 && Mew3.Col < GlobalValues.WINDOW_WIDTH)
         {
            ConsoleMethods.WriteText(Mew3.Character, Mew3.Col, Mew3.Row, Mew3.Colour);
         }
         if (Ghost.Row >= 0 && Ghost.Row < GlobalValues.WINDOW_HEIGHT && Ghost.Col >= 0 && Ghost.Col < GlobalValues.WINDOW_WIDTH)
         {
            ConsoleMethods.WriteText(Ghost.Character, Ghost.Col, Ghost.Row, Ghost.Colour);
         }
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