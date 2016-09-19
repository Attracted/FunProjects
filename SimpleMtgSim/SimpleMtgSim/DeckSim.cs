using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SimpleMtgSim
{
   public partial class DeckSim : Form
   {
      private List<string> CardNames;

      private CardControl CardWindow;
      private TutorControl TutorWindow;

      private Random Seed;

      public DeckSim()
      {
         InitializeComponent();
         Seed = new Random();
         SetDeck();
      }

      private void SetDeck()
      {
         if (CardNames == null)
         {
            CardNames = new List<string>();
         }
         else
         {
            CardNames.Clear();
         }

         CardNames.AddRange(this.deckList.Items.Cast<string>());
         this.remainingCards.Text = CardNames.Count.ToString();

         this.numberCards.Value = 7;
      }

      private void draw_Click(object sender, EventArgs e)
      {
         if (CardWindow == null)
         {
            CreateCardWindow();
         }
         
         for (int i = 0; i < this.numberCards.Value; i++)
         {
            int cardCount = CardNames.Count;
            if (cardCount > 0)
            {
               int cardIndex = Seed.Next(0, cardCount);
               int x = (i % 7) * 100;
               int y = (i - (i % 7)) * (50 / 7);

               CreateCardLabelFromDeck(cardIndex, new Point(x, y));
            }
         }

         this.numberCards.Value = 1;
         this.remainingCards.Text = CardNames.Count.ToString();
      }

      private void CreateCardWindow()
      {
         CardWindow = new CardControl();
         CardWindow.FormClosed += cardWindow_Closed;
         CardWindow.Show();
      }

      private void CreateCardLabelFromDeck(int cardIndex, Point spawnLocation)
      {
         Label newLabel = new Label();

         newLabel.Text = CardNames[cardIndex];
         CardNames.RemoveAt(cardIndex);
         newLabel.MouseDown += label_MouseDown;
         newLabel.MouseMove += label_MouseMove;

         newLabel.Size = new System.Drawing.Size(100, 50);
         newLabel.Location = spawnLocation;

         newLabel.BorderStyle = BorderStyle.Fixed3D;
         newLabel.BackColor = Color.DarkGray;
         newLabel.ForeColor = Color.White;
         this.CardWindow.Controls.Add(newLabel);
      }

      private void tutor_Click(object sender, EventArgs e)
      {
         if (CardWindow == null)
         {
            CreateCardWindow();
         }

         if (TutorWindow == null)
         {
            TutorWindow = new TutorControl();
            TutorWindow.FormClosed += tutorWindow_Closed;
            TutorWindow.OnSubmitTarget += onSubmitTarget;
         }

         foreach (string cardName in CardNames.OrderBy(c => c))
         {
            TutorWindow.AddItem(cardName);
         }

         TutorWindow.Show();
      }

      private void onSubmitTarget(object sender, TutorEventArgs e)
      {
         if (CardWindow == null)
         {
            CreateCardWindow();
         }
         string cardName = e.TutorTarget;

         int cardIndex = CardNames.FindIndex(s => s == cardName);

         CreateCardLabelFromDeck(cardIndex, new Point(0, 0));
         TutorWindow.RemoveItem(cardName);

         this.remainingCards.Text = CardNames.Count.ToString();
      }

      private void resetDeck_Click(object sender, EventArgs e)
      {
         SetDeck();
      }
      private void loadDeck_Click(object sender, EventArgs e)
      {
         var FD = new OpenFileDialog();
         FD.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
         FD.DefaultExt = ".txt";
         if (FD.ShowDialog() == DialogResult.OK)
         {
            string fileToOpen = FD.FileName;
            this.deckList.Items.Clear();
            this.deckList.Items.AddRange(File.ReadAllLines(fileToOpen));
            SetDeck();
         }
      }

      private void cardWindow_Closed(object sender, EventArgs e)
      {
         CardWindow = null;
      }
      private void tutorWindow_Closed(object sender, EventArgs e)
      {
         TutorWindow = null;
      }

      private Point MouseDownLocation;
      private void label_MouseDown(object sender, MouseEventArgs e)
      {         
         if (e.Button == System.Windows.Forms.MouseButtons.Left)
         {
            Label movedObj = (Label)sender;
            movedObj.BringToFront();

            MouseDownLocation = e.Location;
         }
      }
      private void label_MouseMove(object sender, MouseEventArgs e)
      {
         Label movedObj = (Label)sender;
         
         if (e.Button == System.Windows.Forms.MouseButtons.Left)
         {
            movedObj.Left = e.X + movedObj.Left - MouseDownLocation.X;
            movedObj.Top = e.Y + movedObj.Top - MouseDownLocation.Y;
         }
      }
   }
}
