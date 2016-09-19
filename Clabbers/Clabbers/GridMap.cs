using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Clabbers
{
   public class GridMap
   {
      private Cell[,] _GridMap;
      private Char[,] _WordMap;

      private int _Row;
      public int Row 
      { 
         get { return _Row; } 
      }

      private int _Col;
      public int Col 
      {
         get { return _Col; } 
      }

      public GridMap(int row, int col)
      {
         _Row = row;
         _Col = col;
         _GridMap = new Cell[row, col];
         //_WordMap = new Char[row, col];
      }

      public void Set(int row, int col, Cell cell)
      {
         _GridMap[row, col] = cell;
      }

      public Cell Get(int row, int col)
      {
         return _GridMap[row, col];
      }

      public void CopyGridMapToWordMap()
      {
         for (int i = 0; i < _Row; i++)
         {
            for (int j = 0; j < _Col; j++)
            {
               var tile = _GridMap[i, j].Tile;
               if (tile != null)
               {
                  _WordMap[i, j] = tile.Letter;
               }
            }
         }
      }

      public string MapToString()
      {
         string map = "";
         using (StringWriter output = new StringWriter())
         {
            output.WriteLine(_Row + "," + _Col);
            for (int row = 0; row < _Row; row++)
            {
               for (int col = 0; col < _Col; col++)
               {
                  Cell cell = _GridMap[row, col];
                  switch (cell.Type)
                  {
                     case (CellType.Empty):
                        output.Write('X');
                        break;
                     case (CellType.DoubleLetter):
                        output.Write('l');
                        break;
                     case (CellType.DoubleWord):
                        output.Write('w');
                        break;
                     case (CellType.TripleLetter):
                        output.Write('L');
                        break;
                     case (CellType.TripleWord):
                        output.Write('W');
                        break;
                     case (CellType.StartTile):
                        output.Write('*');
                        break;
                  }
               }
               if (row < _Row - 1)
               {
                  output.WriteLine();
               }
            }
            map = output.ToString();
         }
         return map;
      }

      public string UsedToString()
      {
         string used = "";
         using (StringWriter output = new StringWriter())
         {
            for (int row = 0; row < _Row; row++)
            {
               for (int col = 0; col < _Col; col++)
               {
                  Cell cell = _GridMap[row, col];
                  if (!cell.Used)
                  {
                     output.Write('.');
                  }
                  else if (cell.Tile.IsBlankTile)
                  {
                     output.Write(char.ToLower(cell.Tile.Letter));
                  }
                  else
                  {
                     output.Write(cell.Tile.Letter);
                  }
               }
               if (row < _Row - 1)
               {
                  output.WriteLine();
               }
            }
            used = output.ToString();
         }
         return used;
      }
   }
}
