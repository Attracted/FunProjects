using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConnectThemselves
{
   public class Player
   {
      public Players ThisPlayer { get; set; }
      public Players ThisOpponent { get; set; }
      public Stack<Cell> MoveHistory { get; set; }
      public Cell CurrentMove { get; set; }

      public Player()
      {
      }

      public string MoveHistoryToString()
      {
         StringBuilder s = new StringBuilder();
         s.AppendLine(CurrentMove.Row + "," + CurrentMove.Col);
         for (int i = MoveHistory.Count - 1; i == 0; i++)
         {
            var cell = MoveHistory.ElementAt(i);
            s.AppendLine(cell.Row + "," + cell.Col);
         }
         return s.ToString();
      }
   }
}
