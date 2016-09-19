using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MEW3.Properties;

namespace MEW3
{    
   public class Map : IEnumerable<char>
   {
      public string[] GameMap { get; private set; }
      public SquareGrid Grid { get; private set; }
      public Location Bounds { get; private set; }
      public Location Mew3StartLoc { get; private set; }
      public List<Location> GhostStartLocs { get; private set; }

      public Map(string[] textmap)
      {
         GameMap = textmap;
         Bounds = FindMapBoundsAndPad();
         Grid = new SquareGrid(Bounds.X, Bounds.Y);
         GhostStartLocs = new List<Location>();

         for (var i = 0; i < Bounds.X; i++)
         {
            for (var j = 0; j < Bounds.Y; j++)
            {
               var character = this[i, j];

               int num = 0;
               bool isNumber = int.TryParse(character.ToString(), out num);

               if (character != ' ' && character != '.' && character != 'o' &&
                  //character != '!' && character != 'M' && !isNumber)
                  character != '!' && character != 'E' && !isNumber)
               {
                  Grid.Walls.Add(new Location(i, j));
               }
               else if (isNumber)
               {
                  Grid.Portals.Add(Tuple.Create(character, new Location(i, j)));
                  //this[i, j] = '@';
                  this[i, j] = SpecialChars.Portal;
               }
               else if (character == 'E')
               {
                  Mew3StartLoc = new Location(i, j);
                  this[i, j] = ' ';
               }
               else if (character == '!')
               {
                  //GhostStartLoc = new Location(i, j);

                  GhostStartLocs.Add(new Location(i, j));
                  this[i, j] = ' ';
               }
            }
         }
      }

      public Map(Map map)
      {
         GameMap = map.GameMap.ToArray();
         Bounds = map.Bounds;
         Grid = new SquareGrid(map.Grid);
         GhostStartLocs = map.GhostStartLocs;
         Mew3StartLoc = map.Mew3StartLoc;
      }

      public Location FindMapBoundsAndPad()
      {
         int largestCol = 0;

         for (int i = 0; i < GameMap.Length; i++)
         {
            int len = GameMap[i].Length;

            if(len > largestCol)
            {
               largestCol = len;
            }
         }

         for (int i = 0; i < GameMap.Length; i++)
         {
            if (GameMap[i].Length < largestCol)
            {
               GameMap[i] = GameMap[i].PadRight(largestCol);
            }
         }

         return new Location(largestCol, GameMap.Length);
      }

      public int CountDots()
      {
         int count = 0;
         
         foreach (var row in GameMap)
         {
            for (int col = 0; col < row.Length; col++)
            {
               if (row[col] == '.')
               {
                  count++;
               }
            }
         }

         return count;
      }

      public void ReplaceAt(int i, int j, char value)
      {
         StringBuilder str = new StringBuilder(GameMap[j]);
         str[i] = value;
         GameMap[j] = str.ToString();
      }

      public char this[int i, int j]
      {
         get
         {
            var row = GameMap[j];
            return row[i];
         }
         set
         {
            StringBuilder str = new StringBuilder(GameMap[j]);
            str[i] = value;
            GameMap[j] = str.ToString();
         }
      }

      #region IEnumerable<char> Members

      public IEnumerator<char> GetEnumerator()
      {
         return (IEnumerator<char>)GameMap.GetEnumerator();
      }

      #endregion

      #region IEnumerable Members

      System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
      {
         return (IEnumerator<char>)GameMap.GetEnumerator();
      }

      #endregion
   }
}
