using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace Clabbers
{
   static class Program
   {
      [STAThread]
      static void Main()
      {
         string map = "";
         string tiles = "";
         string words ="";
         string save = "";
         int handSize = int.MinValue;
         int numOpp = int.MinValue;

         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         using (var selectionForm = new StartForm())
         {
            selectionForm.ShowDialog();
            
            var result = selectionForm.Action;

            switch (result)
            {
               case CustomDialogResult.Load:
                  save = selectionForm.Save;
               break;
               case CustomDialogResult.Submit:
                  map = selectionForm.Map;
                  tiles = selectionForm.Tiles;
                  words = selectionForm.Words;
                  handSize = selectionForm.HandSize;
                  numOpp = selectionForm.NumOpp;
               break;
               case CustomDialogResult.Failed:
                  MessageBox.Show("Invalid hand size and/or number of opponents");
                  return;
               default:
                  return;
            }

         }

         if (map == "Use Default")
         {
            map = Clabbers.Properties.Resources.DefaultMap;
         }
         else
         {
            // Load file into a string
         }

         if (tiles == "Use Default")
         {
            tiles = Clabbers.Properties.Resources.DefaultTiles;
         }
         else
         {
            // Load file into a string
         }

         if (words == "Use Default")
         {
            words = Clabbers.Properties.Resources.DefaultWords;
         }
         else
         {
            // Load file into a string
            // Send to all caps
         }


         if (save == "")
         {
            Application.Run(new GameManager(map, tiles, words, handSize, numOpp));
         }
         else
         {
            Application.Run(new GameManager(save));
         }
         //GameManager game = new GameManager(map, tiles, words, handSize, numOpp);
         //game.MakeGame();
         //while(true);
      }
   }
}
