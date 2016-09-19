using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using ConnectThemselves.Properties;

namespace ConnectThemselves
{
   public enum CustomDialogResult
   {
      Default = 0,
      Submit = 1,
      Load = 2,
      Failed = 3,
      Logging = 4
   } 
  
   public partial class StartForm : Form
   {
      public int Row { get; private set; }
      public int Col { get; private set; }
      public int Win { get; private set; }
      public int Max { get; private set; }
      public string Save { get; private set; }
      public CustomDialogResult Action { get; private set; }  
     
      public StartForm()
      {
          InitializeComponent();
          this.StartPosition = FormStartPosition.CenterScreen;
          this.Icon = Resources.ConnectThem2;
          Action = CustomDialogResult.Default;
      }

      private void button1_Click(object sender, EventArgs e)
      {
         int row;
         int col;
         int win;
         int max;

         var rowWorked = int.TryParse(rowsTextBox.Text, out row);
         var colWorked = int.TryParse(colsTextBox.Text, out col);
         var winWorked = int.TryParse(winTextBox.Text, out win);
         var maxWorked = int.TryParse(maxGamesTextBox.Text, out max);

         if (!rowWorked || row < 1 || row > 99 || 
            !colWorked || col < 1 || col > 99 || 
            !winWorked || win < 1 || win > 99 || 
            !maxWorked || max < 1)
         {
            Action = CustomDialogResult.Failed;
         }
         else
         {
            Row = row;
            Col = col;
            Win = win;
            Max = max;
            
            Action = CustomDialogResult.Submit;
         }
         
         this.Close();
      }

      private void withLogging_Click(object sender, EventArgs e)
      {
         int row;
         int col;
         int win;
         int max;

         var rowWorked = int.TryParse(rowsTextBox.Text, out row);
         var colWorked = int.TryParse(colsTextBox.Text, out col);
         var winWorked = int.TryParse(winTextBox.Text, out win);
         var maxWorked = int.TryParse(maxGamesTextBox.Text, out max);

         if (!rowWorked || row < 1 || row > 99 ||
            !colWorked || col < 1 || col > 99 ||
            !winWorked || win < 1 || win > 99 ||
            !maxWorked || max < 1)
         {
            Action = CustomDialogResult.Failed;
         }
         else
         {
            Row = row;
            Col = col;
            Win = win;
            Max = max;
            
            var FD = new SaveFileDialog();
            FD.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            FD.DefaultExt = ".txt";
            FD.InitialDirectory = @"~\";

            if (FD.ShowDialog() == DialogResult.OK)
            {
               Save = FD.FileName;
               Action = CustomDialogResult.Logging;
               this.Close();
               return;
            }
            Action = CustomDialogResult.Failed;
         }

         this.Close(); 
      }

      private void loadGame_Click(object sender, EventArgs e)
      {
         var FD = new OpenFileDialog();
         FD.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
         FD.DefaultExt = ".txt";
         FD.InitialDirectory = @"~\";

         if (FD.ShowDialog() == DialogResult.OK)
         {
            string fileToLoadFrom = FD.FileName;
            using (StreamReader reader = new StreamReader(fileToLoadFrom))
            {
               //etc
               Save = reader.ReadToEnd();
            }

            Action = CustomDialogResult.Load;
            this.Close();
            return;
         }
         Action = CustomDialogResult.Failed;
      }
   }
}
