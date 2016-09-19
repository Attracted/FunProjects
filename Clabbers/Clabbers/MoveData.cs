using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clabbers
{
   public class MoveData
   {
      public string Word { get; set; }
      public int Score { get; set; }
      public List<Cell> Move { get; set; }
      public bool ValidWord { get; set; }
      public int Turn { get; set; }
      public Alignment Alignment { get; set; }

      public MoveData()
      {
      }

      public void SetWordToString()
      {
         StringBuilder wordBuilder = new StringBuilder();

         foreach (Cell cell in Move)
         {
            wordBuilder.Append(cell.Tile.Letter);
         }

         Word = wordBuilder.ToString();
      }
   }
}
