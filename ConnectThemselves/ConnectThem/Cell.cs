using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace ConnectThemselves
{
   public class Cell
   {
      public bool Used { get; set; }
      public Players Owner { get; set; }
      public int Row { get; set; }
      public int Col { get; set; }
      public int Value { get; set; }
      public bool Winning { get; set; }
      public Chips Chip { get; set; }


      public Cell(Cell cell)
      { 
         Used     = cell.Used;
         Owner    = cell.Owner;
         Row      = cell.Row;
         Col      = cell.Col;
         Value    = cell.Value;
         Winning  = cell.Winning;
         Chip     = cell.Chip;
      } 
      
      public Cell(int row, int col)
         :this()
      {
         Row = row;
         Col = col;
      }

      public Cell()
      {
         Owner = Players.None;
         Winning = false;
         Chip = Chips.EmptyChip;
      }
   }
}
