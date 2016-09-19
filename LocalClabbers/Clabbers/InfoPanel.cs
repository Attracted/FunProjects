using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Clabbers
{
   public partial class InfoPanel : UserControl
   {		
      public InfoPanel()
      {
         InitializeComponent();
      }

      public InfoPanel(int handSize)
      {
         InitializeComponent();

         _HandLabels = new Label[handSize];
         for (int i = 0; i < _HandLabels.Length; i++)
         {
            _HandLabels[i] = new Label()
            {
               Location = new Point(5 + i * 15, 30),
               Size = new Size(14, 14),
               BorderStyle = BorderStyle.Fixed3D,
               BackColor = Color.BurlyWood
            };
            this.Controls.Add(_HandLabels[i]);
         }
      }

      public Players PlayerName
      {
         get {	return (Players)Enum.Parse(typeof(Players), playerName.Text); }
         set { playerName.Text = value.ToString(); }
      }

      public int Score
      {
         get { return int.Parse(score.Text); }
         set { score.Text = value.ToString(); }
      }

      public MoveData LastMove
      {
         get 
         {
            return new MoveData() { Word = lastMove.Text, Score = int.Parse(lastScore.Text) }; 
         }
         set 
         {
            if (value == null)
            {
               return;
            }
            lastMove.Text = value.Word;
            lastScore.Text = value.Score.ToString();
         }
      }

      public Button MoveHistoryButton
      {
         get { return moveHistory; }
         set { moveHistory = value; }
      }

      private Label[] _HandLabels;

      public void SetHand(string handLetters)
      {
         if (_HandLabels.Length != handLetters.Length)
         {
            return;
         }

         for (int i = 0; i < handLetters.Length; i++)
         {
            _HandLabels[i].Text = handLetters[i].ToString();
         }
      }
   }
}
