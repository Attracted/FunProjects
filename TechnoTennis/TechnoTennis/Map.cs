using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TechnoTennis.Properties;

namespace TechnoTennis
{    
   public class Map : IEnumerable<char>
   {
      public string[] LetterMap { get; private set; }
      public SquareGrid Grid { get; private set; }
      public Location BoundsIndex { get; private set; }
      public Location ManualPlayerStartLoc { get; private set; }

      public Map(string[] textmap)
      {
         LetterMap = textmap;
         BoundsIndex = FindMapBoundsAndPad();
         Grid = new SquareGrid(BoundsIndex.X, BoundsIndex.Y);

         for (var i = 0; i < BoundsIndex.X; i++)
         {
            for (var j = 0; j < BoundsIndex.Y; j++)
            {
               var character = this[i, j];

               if (character != ' ' &&
                  character != SpecialChars.Player_Start)
               {
                  Grid.Walls.Add(new Location(i, j));
               }
               else if (character == SpecialChars.Player_Start)
               {
                  ManualPlayerStartLoc = new Location(i, j);
                  this[i, j] = ' ';
               }
            }
         }
      }

      public Map(Map map)
      {
         LetterMap = map.LetterMap.ToArray();
         BoundsIndex = map.BoundsIndex;
         Grid = new SquareGrid(map.Grid);
         ManualPlayerStartLoc = map.ManualPlayerStartLoc;
      }

      public char this[int i, int j]
      {
         get
         {
            var row = LetterMap[j];
            return row[i];
         }
         set
         {
            StringBuilder str = new StringBuilder(LetterMap[j]);
            str[i] = value;
            LetterMap[j] = str.ToString();
         }
      }

      public override string ToString()
      {
         //return base.ToString();

         StringBuilder sb = new StringBuilder();

         for (int i = 0; i < BoundsIndex.Y; i++)
         {
            sb.AppendLine(LetterMap[i]);
         }
         
         return sb.ToString();
      }

      public void UpdateMapAfterMove(int i, int j)
      {
         if (this[i, j] != SpecialChars.Portal)
         {
            this[i, j] = ' ';
         }
      }

      public void UpdateMap(Location loc, char c)
      {
         this[loc.X, loc.Y] = c;
      }

      private Location FindMapBoundsAndPad()
      {
         int largestCol = 0;

         for (int i = 0; i < LetterMap.Length; i++)
         {
            int len = LetterMap[i].Length;

            if (len > largestCol)
            {
               largestCol = len;
            }
         }

         for (int i = 0; i < LetterMap.Length; i++)
         {
            if (LetterMap[i].Length < largestCol)
            {
               LetterMap[i] = LetterMap[i].PadRight(largestCol);
            }
         }

         return new Location(largestCol, LetterMap.Length);
      }

      #region IEnumerable<char> Members

      public IEnumerator<char> GetEnumerator()
      {
         return (IEnumerator<char>)LetterMap.GetEnumerator();
      }

      #endregion

      #region IEnumerable Members

      System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
      {
         return (IEnumerator<char>)LetterMap.GetEnumerator();
      }

      #endregion
   }
}
