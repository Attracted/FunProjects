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
   public class IntroSequence : Sequence
   {
      public IntroSequence(int highScore)
         :base(highScore)
      {
      }

      protected override Mew3 MakeMew3()
      {
         Mew3 mew3 = new Mew3(new Location(PathRectangle.Left + 6, PathRectangle.Bottom), SpecialChars.Mew3_Right);
         mew3.Colour = ConsoleColor.Yellow;
         return mew3;
      }
      protected override Ghost MakeGhost()
      {
         Ghost ghost = new Ghost(new Location(PathRectangle.Left + 3, PathRectangle.Bottom), SpecialChars.Ghost);
         ghost.Colour = ConsoleColor.Red;
         return ghost;
      }

      protected override void MovePlayer(Player player, ref Direction direction)
      {         
         switch (direction)
         {
            case Direction.Up:
               player.UpdateLocation(player.Col, player.Row - 1);
               if (player.Row == PathRectangle.Top)
               {
                  direction = Direction.Left;
                  if (player.Character == SpecialChars.Mew3_Up)
                  {
                     player.Character = SpecialChars.Mew3_Left;
                  }
               }
               break;
            case Direction.Down:
               player.UpdateLocation(player.Col, player.Row + 1);
               if (player.Row == PathRectangle.Bottom)
               {
                  direction = Direction.Right;
                  if (player.Character == SpecialChars.Mew3_Down)
                  {
                     player.Character = SpecialChars.Mew3_Right;
                  }
               }
               break;
            case Direction.Left:
               player.UpdateLocation(player.Col - 1, player.Row);
               if (player.Col == PathRectangle.Left)
               {
                  direction = Direction.Down;
                  if (player.Character == SpecialChars.Mew3_Left)
                  {
                     player.Character = SpecialChars.Mew3_Down;
                  }
               }
               break;
            case Direction.Right:
               player.UpdateLocation(player.Col + 1, player.Row);
               if (player.Col == PathRectangle.Right)
               {
                  direction = Direction.Up;
                  if (player.Character == SpecialChars.Mew3_Right)
                  {
                     player.Character = SpecialChars.Mew3_Up;
                  }
               }
               break;
         }
      }
      protected override void DrawTitleLogo()
      {
         ConsoleMethods.DrawAppBorder();

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


         //ConsoleMethods.WriteText(ghost, GlobalValues.BORDER_LOCATION.X + 5, GlobalValues.BORDER_LOCATION.Y + 4, ConsoleColor.Red);
         ConsoleMethods.WriteText(ghost, PathRectangle.Left + 2, PathRectangle.Top + 2, ConsoleColor.Red);
         ConsoleMethods.WriteText(mew3, PathRectangle.Left + 7, PathRectangle.Top + 2, ConsoleColor.Yellow);
      }
   }
}