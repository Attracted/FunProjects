using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slither
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
      public List<ScreenObject> BodyParts { get; private set; }
      public ScreenObject Head { get; private set; }
      public Location? TailPrevLoc { get; private set; }
      public Location HeadPrevLoc { get; private set; }

      public bool Collision { get; set; }

      public Player(Location headLoc, Direction dir)
      {
         Head = new ScreenObject(headLoc, SpecialChars.Player_Start, SpecialColours.Player);
         TailPrevLoc = headLoc;
         HeadPrevLoc = headLoc;

         MovementDirection = dir;

         BodyParts = new List<ScreenObject>();
         BodyParts.Add(Head);
      }

      public void MoveTo(int col, int row, char headChar)
      {
         Location newLoc = new Location(col, row);
         Location nextLoc = Head.Location;
         Head.Location = newLoc;
         Head.Character = headChar;

         TailPrevLoc = nextLoc;
         HeadPrevLoc = nextLoc;

         for (int i = 1; i < BodyParts.Count; i++)
         {
            TailPrevLoc = BodyParts[i].Location;
            BodyParts[i].Location = nextLoc;
            nextLoc = TailPrevLoc.Value;
         }
      }

      private void AddBlock(Location loc, char character)
      {
         BodyParts.Add(new ScreenObject(loc, character, SpecialColours.Player));
      }

      public void AddBlock()
      {
         if (TailPrevLoc != null)
         {
            BodyParts.Add(new ScreenObject(TailPrevLoc.Value, SpecialChars.Player_Body, SpecialColours.Player));
            TailPrevLoc = null;
         }
      }
      public void RemoveBlock()
      {
         int countIndex = BodyParts.Count - 1;

         if (countIndex > 0)
         {
            TailPrevLoc = BodyParts[countIndex].Location;
            BodyParts.RemoveAt(countIndex);
         }
      }
   }
}
