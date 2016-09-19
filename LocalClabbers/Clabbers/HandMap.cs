using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using Clabbers.Properties;

namespace Clabbers
{
   public class HandMap : IEnumerable<Cell>
   {     
      private List<Cell> _HandMap;
      private int _HandSize;
      private int _HandRow;
      private TileBag _TileBag;

      public HandMap(int handRow, int handSize, TileBag tileBag, Players player, 
         MoveChangedEventHandler moveHandler, TileChangedEventHandler tileHandler)
      {
         _HandMap = new List<Cell>(handSize);
         _HandSize = handSize;
         _HandRow = handRow;
         _TileBag = tileBag;

         MakeHandMap(player, moveHandler, tileHandler);
      }

      public Cell this[int index]
      {
         get
         {
            return _HandMap[index];
         }
         set
         {
            _HandMap[index] = value;
         }
      }

      public IEnumerator<Cell> GetEnumerator()
      {
         return (IEnumerator<Cell>) _HandMap.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return _HandMap.GetEnumerator();
      }

      public void RefillHand(Random seed)
      {
         foreach (Cell cell in _HandMap)
         {
            if (cell.Tile == null)
            {
               cell.Tile = GetRandomTile(seed);
            }
         }
      }

      private void MakeHandMap(Players player, MoveChangedEventHandler moveHandler, TileChangedEventHandler tileHandler)
      {
         for (int i = 0; i < _HandSize; i++)
         {
            _HandMap.Add(new Cell(_HandRow, i, CellType.Hand, player));
            _HandMap[i].OnMoveChanged += new MoveChangedEventHandler(moveHandler);
            _HandMap[i].OnTileChanged += new TileChangedEventHandler(tileHandler);
         }         
      }

      private Tile GetRandomTile(Random seed)
      {
         return _TileBag.GetRandomTile(seed);
      }
   }
}
