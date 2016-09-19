using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnoTennis
{
   public class ScreenObject
   {
      public Location Location;
      public Char Character;
      public ConsoleColor Colour;

      public ScreenObject(ScreenObject bp)
      {
         Location = bp.Location;
         Character = bp.Character;
         Colour = bp.Colour;
      }
      public ScreenObject(Location a, Char c, ConsoleColor colour)
      {
         Location = a;
         Character = c;
         Colour = colour;
      }
      public ScreenObject(int x, int y, Char c, ConsoleColor colour)
      {
         Location = new Location(x, y);
         Character = c;
         Colour = colour;
      }

      public override bool Equals(object obj)
      {
         if (obj == null)
         {
            return false;
         }

         ScreenObject a = (ScreenObject)obj;
         return Location == a.Location && Character == a.Character && Colour == a.Colour;
      }

      public static bool operator ==(ScreenObject a, ScreenObject b)
      {
         if (System.Object.ReferenceEquals(a, b))
         {
            return true;
         }

         object obj_a = (object)a;
         object obj_b = (object)b;

         if (obj_a == null || obj_b == null)
         {
            return false;
         }

         return a.Location == b.Location && a.Character == b.Character && a.Colour == b.Colour;
      }

      public static bool operator !=(ScreenObject a, ScreenObject b)
      {
         return !(a == b);
      }

      public override int GetHashCode()
      {
         return base.GetHashCode();
      }
   }

   public class Player
   {
      public Direction MovementDirection { get; set; }
      public int LengthIndex { get; private set; }
      public ScreenObject LeftMostPiece { get; private set; }
      public Location? PieceToDelete { get; private set; }
      public Location? PieceToAdd { get; private set; }

      public Player(Location headLoc, int length)
      {
         LeftMostPiece = new ScreenObject(headLoc, SpecialChars.Player_Body, SpecialColours.Player);
         MovementDirection = Direction.None;
         LengthIndex = length - 1;
      }

      public void Move()
      {
         int x = LeftMostPiece.Location.X;
         int y = LeftMostPiece.Location.Y;

         if (MovementDirection == Direction.Left && x > 0)
         {
            LeftMostPiece.Location = new Location(x - 1, y);
            PieceToAdd = new Location(x - 1, y);
            PieceToDelete = new Location(x + LengthIndex, y);
         }
         else if (MovementDirection == Direction.Right && (x + LengthIndex) < GlobalValues.BORDER_LOCATION.Right - 2)
         {
            LeftMostPiece.Location = new Location(x + 1, y);
            PieceToAdd = new Location(x + 1 + LengthIndex, y);
            PieceToDelete = new Location(x, y);
         }
         else
         {
            PieceToAdd = null;
            PieceToDelete = null;
         }
      }
   }
}
