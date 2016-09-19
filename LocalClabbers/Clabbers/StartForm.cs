using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Clabbers
{
   public enum CustomDialogResult
   {
      Submit = 0,
      Load = 1,
      Failed = 2,
      Default = 3
   }
   
   public partial class StartForm : Form
   {
      public string Map { get; private set; }
      public string Tiles { get; private set; }
      public string Words { get; private set; }
      public int HandSize { get; private set; }
      public int NumOpp { get; private set; }
      public string Save { get; private set; }
      public CustomDialogResult Action { get; private set; }

      public StartForm()
      {
         InitializeComponent();
         this.StartPosition = FormStartPosition.CenterScreen;
         Action = CustomDialogResult.Default;
      }

      private void submit_Click(object sender, EventArgs e)
      {		
         if(Map == null) Map = mapTextBox.Text;
         if (Tiles == null) Tiles = tilesTextBox.Text;
         if (Words == null) Words = wordsTextBox.Text;
         int handSize;
         int numOpp;
         bool handWorked = int.TryParse(handSizeTextBox.Text, out handSize);
         bool oppWorked = int.TryParse(numOppTextBox.Text, out numOpp);
         oppWorked = oppWorked & numOpp < 4;

         if (handWorked && oppWorked)
         {
            HandSize = handSize;
            NumOpp = numOpp;
            Action = CustomDialogResult.Submit;
         }
         else
         {
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
            string fileToOpen = FD.FileName;
            using (StreamReader reader = new StreamReader(fileToOpen))
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


      public void mapTextBox_Click(object sender, EventArgs e)
      {
         Map = LoadFile();
      }
      public void tilesTextBox_Click(object sender, EventArgs e)
      {
         Tiles = LoadFile();
      }
      public void wordsTextBox_Click(object sender, EventArgs e)
      {
         Words = LoadFile();
      }

      private string LoadFile()
      {
         var FD = new OpenFileDialog();
         FD.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
         FD.DefaultExt = ".txt";
         FD.InitialDirectory = @"~\";

         string ret = "Load Failed...";

         if (FD.ShowDialog() == DialogResult.OK)
         {
            string fileToOpen = FD.FileName;
            using (StreamReader reader = new StreamReader(fileToOpen))
            {
               //etc
               ret = reader.ReadToEnd();
            }
         }
         
         return ret;
      }
   }
}
