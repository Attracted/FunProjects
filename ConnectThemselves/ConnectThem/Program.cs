using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ConnectThemselves
{
   static class Program
   {
      [STAThread]
      static void Main()
      {
         int row = int.MinValue;
         int col = int.MinValue;
         int win = int.MinValue;
         int max = int.MinValue;
         string save = "";

         CustomDialogResult result = CustomDialogResult.Default;

         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         using (var selectionForm = new StartForm())
         {
            selectionForm.ShowDialog();

            result = selectionForm.Action;

            switch (result)
            {
               case CustomDialogResult.Load:
                  save = selectionForm.Save;
                  break;
               case CustomDialogResult.Logging:
                  row = selectionForm.Row;
                  col = selectionForm.Col;
                  win = selectionForm.Win;
                  max = selectionForm.Max;
                  save = selectionForm.Save;
                  break;
               case CustomDialogResult.Submit:
                  row = selectionForm.Row;
                  col = selectionForm.Col;
                  win = selectionForm.Win;
                  max = selectionForm.Max;
                  break;
               case CustomDialogResult.Failed:
                  MessageBox.Show("One or more invalid parameters");
                  return;
               default:
                  return;
            }
         }

         switch (result)
            {
               case CustomDialogResult.Load:
                  Application.Run(new GameManager(save));
                  break;
               case CustomDialogResult.Logging:
                  Application.Run(new GameManager(row, col, win, max, save));
               break;
               case CustomDialogResult.Submit:
                  Application.Run(new GameManager(row, col, win, max));
                  break;
               default:
                  return;
            }

         //GameManager game = new GameManager(map, tiles, words, handSize, numOpp);
         //game.MakeGame();
         //while(true);
      }
   }
}
