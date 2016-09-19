using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace IsogramFinder
{
   class Program
   {
      static void Main(string[] args)
      {
         //var uniqueWords = File.ReadAllLines(@"C:\MyProjects\Clabbers\Words\OrderedUniqueWords.txt");

         //var orderedWords = uniqueWords.OrderByDescending(e => e.Length).ToList();

         //StringBuilder sb = new StringBuilder();

         //foreach (var word in orderedWords)
         //{
         //   sb.AppendLine(word);
         //}

         //File.WriteAllText(@"C:\MyProjects\Clabbers\Words\OrderedUniqueWordsByName.txt", sb.ToString());

         //****var uniqueWords = File.ReadAllLines(@"C:\MyProjects\Clabbers\Words\RemainingOrderedUniqueWords.txt");
         //var uniqueWords = File.ReadAllLines(@"C:\MyProjects\Clabbers\Words\TestIsoGrams.txt");

         var pangrams = File.ReadAllLines(@"C:\MyProjects\Clabbers\Words\SCRABBLE_PANGRAMS!.txt");


         using (StreamWriter sw = File.AppendText(@"C:\MyProjects\Clabbers\Words\Pangrams_no_QOPH_or_CWM.txt"))
         {
            for (int i = 0; i < pangrams.Length; i++)
            {
               if (!pangrams[i].Contains("QOPH") && !pangrams[i].Contains("CWM"))
               {
                  sw.WriteLine(pangrams[i]);
               }
            }
         }


         //***int count = uniqueWords.Length;

         //var stopWatch = new Stopwatch();
         //stopWatch.Start();
         //CreateIsogram(uniqueWords, new List<string>());
         //CreateIsogram(uniqueWords, "");
         //****CreateIsogram(uniqueWords, count, 0, "", 0, 0);
         //stopWatch.Stop();

         //Console.WriteLine(stopWatch.ElapsedTicks);
      }

      static void CreateIsogram(string[] words, List<string> savedWords)
      {
         for (int i = 0; i < words.Length; i++)
         {
            var word = words[i];

            var newIso = string.Join("", savedWords) + word;
            if (UniqueLetters(newIso))
            {
               var newList = new List<string>(savedWords);
               newList.Add(word);
               
               var len = newIso.Length;
               if (len == 26)
               {
                  using (var sw = File.AppendText(@"C:\MyProjects\Clabbers\Words\IsoGrams.txt"))
                  {
                     sw.WriteLine(string.Join(" ", newList));
                  }

                  continue;
               }

               CreateIsogram(words.Skip(i).ToArray(), newList);
            }
         }
         return;
      }

      static void CreateIsogram(string[] words, string savedWords)
      {
         for (int i = 0; i < words.Length; i++)
         {
            var word = words[i];

            var newIso = savedWords + ' ' + word;
            if (UniqueLetters(newIso))
            {
               if (Count(newIso) == 26)
               {
                  using (var sw = File.AppendText(@"C:\MyProjects\Clabbers\Words\IsoGrams.txt"))
                  {
                     sw.WriteLine(newIso);
                  }

                  continue;
               }

               CreateIsogram(words.Skip(i).ToArray(), newIso);
            }
         }
         return;
      }

      static void CreateIsogram(string[] words, int length, int skip, string iso, int count, int level)
      {
         for (int i = skip; i < length; i++)
         {
            string word = words[i];

            string newIso = iso + ' ' + word;
            if (UniqueLetters(newIso))
            {
               int newCount = count + word.Length;

               if (newCount == 26)
               {
                  using (StreamWriter sw = File.AppendText(@"C:\MyProjects\Clabbers\Words\IsoGrams.txt"))
                  {
                     sw.WriteLine(newIso);
                  }

                  continue;
               }

               if (level == 0)
               {
                  //Console.WriteLine(words[i]);
                  using (StreamWriter sw = File.AppendText(@"C:\MyProjects\Clabbers\Words\Isogram_Progress.txt"))
                  {
                     sw.WriteLine(newIso);
                  }
               }
               else if (level == 1)
               {
                  //Console.WriteLine(">" + words[i]);
                  using (StreamWriter sw = File.AppendText(@"C:\MyProjects\Clabbers\Words\Isogram_Progress.txt"))
                  {
                     sw.WriteLine(newIso);
                  }
               }

               level++;
               CreateIsogram(words, length, i + 1, newIso, newCount, level);
               level--;
            }
         }
         return;
      }

      static double Weight(string word)
      {
         double count = 0.0;

         foreach (var letter in word.ToLower())
         {
            switch (letter)
            {
               case 'a':
               case 'e':
               case 'i':
               case 'o':
               case 'u':
               case 'y':
                  count = count + 1.0;
                  break;
               default:
                  break;
            }
         }

         return word.Length / count;
      }

      static short Count(string word)
      {
         short count = 0;

         for (int i = 0; i < word.Length; i++)
         {
            if (word[i] != ' ')
            {
               count++;
            }
         }

         return count;
      }

      static bool UniqueLetters(string word)
      {
         bool[] array = new bool[256]; // or larger for Unicode

         for (int i = 0; i < word.Length; i++)
         {
            char value = word[i];

            if (value == ' ') { }
            else if (array[(int)value])
               return false;
            else
               array[(int)value] = true;
         }

         return true;
      }

      static bool UniqueLetters(params string[] words)
      {
         bool[] array = new bool[256]; // or larger for Unicode

         foreach (string word in words)
         {
            foreach (char value in word)
               if (value == ' ') { }
               else if (array[(int)value])
                  return false;
               else
                  array[(int)value] = true;
         }

         return true;
      }

      static bool UniqueLetters(List<string> words)
      {
         bool[] array = new bool[256]; // or larger for Unicode

         foreach (string word in words)
         {
            foreach (char value in word)
               if (value == ' ') { }
               else if (array[(int)value])
                  return false;
               else
                  array[(int)value] = true;
         }

         return true;
      }
   }
}
