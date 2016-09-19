using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeRunner
{
   public class Mew3 : Player
   {      
      public Mew3(Location startLoc, Char character)
         :base(startLoc, character)
      {
      }

      public void UpdateCharacter(Direction dir)
      {
         switch (dir)
         {
            case Direction.Up:
               this.Character = SpecialChars.Mew3_Up;
               break;
            case Direction.Down:
               this.Character = SpecialChars.Mew3_Down;
               break;
            case Direction.Left:
               this.Character = SpecialChars.Mew3_Left;
               break;
            case Direction.Right:
               this.Character = SpecialChars.Mew3_Right;
               break;
            default:
               break;
         }
      }

      public void UpdateCharacter(int xDiff, int yDiff)
      {
         if (yDiff == -1)
         {
            UpdateCharacter(Direction.Up);
         }
         else if (yDiff == 1)
         {
            UpdateCharacter(Direction.Down);
         }
         else if (xDiff == -1)
         {
            UpdateCharacter(Direction.Left);
         }
         else if (xDiff == 1)
         {
            UpdateCharacter(Direction.Right);
         }
      }
   }
}
