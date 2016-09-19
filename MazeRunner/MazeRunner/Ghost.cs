using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeRunner
{
   public class Ghost : Player
   {
      private const int _PenaltyTime = 19;
      private int _InternalCounter = 0;

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
               Colour = ConsoleColor.Red;
            }
            else
            {
               Colour = ConsoleColor.Blue;
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
         _InternalCounter = _PenaltyTime;
         PenaltyCounter = _InternalCounter / 2;
      }

      public int DecrementCounter()
      {
         _InternalCounter--;
         PenaltyCounter = _InternalCounter / 2;

         if (PenaltyCounter == 0)
         {
            Immune = true;
         }

         return PenaltyCounter;
      }
   }
}
