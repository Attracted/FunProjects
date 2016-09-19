using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace Clabbers
{
   [Serializable()]
   public class TileBag
   {
      private List<Tile> _Tiles;
      private List<UsedLetter> _UsedLetters;
      private Dictionary<char, Bitmap> _BlankImages;
      private Dictionary<char, Bitmap> _Images;

      public TileBag()
      {
         _Tiles = new List<Tile>();
         _BlankImages = new Dictionary<char, Bitmap>();
         _Images = new Dictionary<char, Bitmap>();
         _UsedLetters = new List<UsedLetter>();
      }

      public void AddTile(Tile tile)
      {
         _Tiles.Add(tile);
         var usedLetter = _UsedLetters.FirstOrDefault(t => t.Letter == tile.Letter);

         if (usedLetter == null)
         {
            _UsedLetters.Add(new UsedLetter() {
               Letter = tile.Letter, 
               Score = tile.Score, 
               Frequency = 1 
            });
         }
         else
         {
            usedLetter.Frequency++;
         }
      }

      public List<UsedLetter> GetUsedLetters()
      {
         return _UsedLetters.ToList();
      }

      public void AddBlankImage(char key, Bitmap value)
      {
         _BlankImages.Add(key, value);
      }

      public void AddImage(char key, Bitmap value)
      {
         _Images.Add(key, value);
      }

      public Tile GetRandomTile(Random rand)
      {
         var tileIndex = rand.Next(0, _Tiles.Count);
         var tile = _Tiles[tileIndex];
         _Tiles.Remove(tile);
         tile.Image = GetImage(tile.Letter);

         return tile;
      }

      public Tile GetTile(char letter)
      {
         var tile = _Tiles.Find(t => t.Letter == letter);
         _Tiles.Remove(tile);
         tile.Image = GetImage(tile.Letter);

         return tile;
      }

      public Bitmap GetImage(char letter)
      {
         if (char.IsLower(letter))
         {
            return GetBlankImage(char.ToUpper(letter));
         }

         if (_Images.ContainsKey(letter))
         {
            return _Images[letter];
         }
         else
         {
            return null;
         }
      }

      public Bitmap GetBlankImage(char letter)
      {
         if (letter == '*')
         {
            return _BlankImages[' '];
         }
         return _BlankImages[letter];
      }

      public int Count()
      {
         return _Tiles.Count;
      }

      public string TilesToString()
      {
         string tiles = "";
         using (StringWriter output = new StringWriter())
         {
            int largestFrequency = _UsedLetters.OrderByDescending(s => s.Score).FirstOrDefault().Score;

            for (int i = 0; i < largestFrequency + 1; i++)
            {
               var usedLetters = _UsedLetters.FindAll(s => s.Score == i).ToList();

               for (int j = 0; j < usedLetters.Count; j++)
               {
                  UsedLetter usedLetter = usedLetters[j];
                  output.Write(char.ToUpper(usedLetter.Letter) + "=" + usedLetter.Frequency);

                  if(j < usedLetters.Count - 1)
                  {
                     output.Write(',');
                  }
               }

               if(i < largestFrequency)
               {
                  output.WriteLine();
               }
            }

            tiles = output.ToString();
         }

         return tiles;
      }
   }
}
