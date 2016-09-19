using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MEW3
{
   public class Ghost : Player
   {
      private const int _PenaltyTime = 10;

      public int PenaltyCounter { get; private set; }
      private bool _Immune;
      public bool Immune 
      {
         get
         {
            return _Immune;
         }
         set
         {
            _Immune = value;
            if (_Immune)
            {
               Colour = Colour = Colours.Red;
            }
            else
            {
               Colour = Colours.Blue;
            }
         }
      }
      
      public Ghost(Location startLoc, char character)
         : base(startLoc, character)
      {
         PenaltyCounter = 0;
      }

      public void SetCounter()
      {
         PenaltyCounter = _PenaltyTime;
      }
      
      public int DecrementCounter()
      {
         return --PenaltyCounter;
      }
   }
   
   public class Player
   {
      private int _Row;
      public int Row
      {
         get
         {
            return _Row;
         }
         set
         {
            _Row = value;
            UpdateLocation();
         }
      }
      private int _Col;
      public int Col 
      { 
         get
         {
            return _Col;
         }
         set
         {
            _Col = value;
            UpdateLocation();
         } 
      }

      public char Character { get; set; }
      public short Colour { get; set; }
      public Location StartLoc { get; private set; }
      public Location CurrentLoc { get; private set; }

      public Player(Location startLoc, char character)
      {
         StartLoc = startLoc;
         Col = startLoc.X;
         Row = startLoc.Y;
         Character = character;
      }

      public void ResetLocation()
      {
         Col = StartLoc.X;
         Row = StartLoc.Y;
      }

      public void UpdateLocation()
      {
         CurrentLoc = new Location(Col, Row);
      }
   }
}
