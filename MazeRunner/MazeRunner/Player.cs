using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeRunner
{
   public class Player
   {
      //private int _Row;
      public int Row
      {
         get
         {
            return CurrentLoc.Y;
         }
      }
      //private int _Col;
      public int Col 
      { 
         get
         {
            return CurrentLoc.X;
         }
      }

      public char Character { get; set; }
      public ConsoleColor Colour { get; set; }
      public Location StartLoc { get; private set; }
      public Location CurrentLoc { get; private set; }
      public Location PrevLoc { get; private set; }

      public Player(Location startLoc, char character)
      {
         StartLoc = startLoc;
         CurrentLoc = startLoc;
         PrevLoc = startLoc;
         Character = character;
      }

      public void ResetLocation()
      {
         CurrentLoc = StartLoc;
      }

      public void UpdateLocation(int col, int row)
      {
         PrevLoc = CurrentLoc;
         CurrentLoc = new Location(col, row);
      }
      public void UpdateLocation(Location loc)
      {
         PrevLoc = CurrentLoc;
         CurrentLoc = loc;
      }
   }
}
