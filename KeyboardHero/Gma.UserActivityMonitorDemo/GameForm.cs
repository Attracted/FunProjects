using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using Gma.UserActivityMonitor;

namespace Gma.UserActivityMonitorDemo
{   
   public partial class GameForm : Form
   {
      private string _SongString;
      private string[] _SongMap;
      private int _SongMapLength;

      private int _LineNumber;
      private int _SpeedNumber;
      private int _Score;
      private string _HighestScore;
      private int _MaxScore;
      private int _Speed;

      private Timer _Timer;
      
      public GameForm()
        {
            InitializeComponent();
        }

      private void Form1_Load(object sender, EventArgs e)
        {
        }

      private void start_Click_1(object sender, EventArgs e)
      {
         MessageBox.Show("Load a song file.");
         var FD = new OpenFileDialog();
         FD.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
         FD.DefaultExt = ".txt";
         FD.InitialDirectory = @"~\";

         if (FD.ShowDialog() == DialogResult.OK)
         {
            string fileToLoadFrom = FD.FileName;
            using (StreamReader reader = new StreamReader(fileToLoadFrom))
            {
               _SongString = reader.ReadToEnd();
            }
         }
         else
         {
            MessageBox.Show("Invalid File");
            return;
         }

         _SongMap = _SongString.Split('\n');
         _SongMapLength = _SongMap.Length;
         _LineNumber = 0;
         _SpeedNumber = 0;
         _Speed = speedSlider.Value;
         _Score = 50;
         _MaxScore = 50;
         SetScoreBarLocation();
         _HighestScore = "-9999";
         if (File.Exists(@"~HighestScore.txt"))
         {
            _HighestScore = File.ReadAllText(@"~HighestScore.txt");
         }
         
         
         for (int i = 0; i < _SongString.Length; i++)
         {
            if (_SongString[i] == '.')
            {
               _MaxScore++;
            }
         }

         _MaxScore = _MaxScore * 15 + 50;

         //this.Controls.Remove(this.banner);
         RemoveStartControls();

         HookManager.KeyDown += HookManager_KeyDown;
         HookManager.KeyUp += HookManager_KeyUp;

         _Timer = new Timer()
         {
            Interval = _Speed
         };
         _Timer.Tick += new EventHandler(_Timer_Tick);
         _Timer.Start();
      }

      private Label[] CreateLabelRow(string line)
        {
           var labels = new Label[4];

           for (int i = 0; i < 4; i++)
           {
              var image = line[i];

              if (image == '-')
              {
                 labels[i] = null;
              }
              else if (image == '.')
              {
                 labels[i] = new Label()
                 {
                    Name = "SongBeat",
                    Size = new Size(60, 60),
                    Location = new Point(i * 70, -60),
                    BorderStyle = BorderStyle.Fixed3D,
                    BackColor = ColToColor(i),
                    Visible = true
                 };
              }
           }

           return labels;
        }

      private Color ColToColor(int col)
        {
           switch (col)
           {
              case 0: return Color.Red;
              case 1: return Color.Green;
              case 2: return Color.Blue;
              case 3: return Color.Orange;
              default: return Color.Black;
           }
        }

      private Color InvertColor(Color color)
      {
         if (color == Color.Red)
         {
            return Color.DarkRed;
         }
         if (color == Color.Green)
         {
            return Color.DarkGreen;
         }
         if (color == Color.Blue)
         {
            return Color.DarkBlue;
         }
         if (color == Color.Orange)
         {
            return Color.DarkOrange;
         }
         if (color == Color.DarkRed)
         {
            return Color.Red;
         }
         if (color == Color.DarkGreen)
         {
            return Color.Green;
         }
         if (color == Color.DarkBlue)
         {
            return Color.Blue;
         }
         if (color == Color.DarkOrange)
         {
            return Color.Orange;
         }
         return Color.Black;
      }

      private void _Timer_Tick(object sender, EventArgs e)
       {
          _Timer.Stop();
          _SpeedNumber++;

          var labels = this.Controls.Find("SongBeat", false);

          foreach (Label label in labels)
          {
             var location = label.Location;
             if (location.Y >= 800)
             {
                this.Controls.Remove(label);
                _Score -= 15;
                SetScoreBarLocation();
                //this.scoreBar.Update();
             }
             else
             {
                label.Location = new Point(location.X, location.Y + 10);
             }

          }

          if (_SpeedNumber >= 20)
          {
             if (_LineNumber < _SongMapLength)
             {
                this.Controls.AddRange(CreateLabelRow(_SongMap[_LineNumber]));
             }
             else if (labels.Length == 0)
             {
                FinishGame();
                return;
             }
             _LineNumber++;
             _SpeedNumber = 0;
          }

          _Timer.Start();
       }

      private void ScoreKeyDown(int column)
      {
         var labels = this.Controls.Find("SongBeat", false);

         foreach (Control label in labels)
         {
            if (label.Location.X != column * 70)
            {
               continue;
            }
            var y = label.Location.Y;
            int y_target = 0;

            switch (column)
            {
               case 0:
                  y_target = h_label.Location.Y;
                  break;
               case 1:
                  y_target = j_label.Location.Y;
                  break;
               case 2:
                  y_target = k_label.Location.Y;
                  break;
               case 3:
                  y_target = l_label.Location.Y;
                  break;
            }

            var y_diff = Math.Abs(y - y_target);

            if (y_diff >= 60)
            {
               continue;
            }
            else
            {
               if (y_diff >= 50 && y_diff < 60)
               {
                  // awful
                  _Score -= 10;
               }
               else if (y_diff >= 40 && y_diff < 50)
               {
                  // bad
                  _Score -= 5;
               }
               else if (y_diff >= 30 && y_diff < 40)
               {
                  // ok
                  //_Score += 0;
               }
               else if (y_diff >= 20 && y_diff < 30)
               {
                  // good
                  _Score += 5;
               }
               else if (y_diff >= 10 && y_diff < 20)
               {
                  // great
                  _Score += 10;
               }
               else if (y_diff >= 0 && y_diff < 10)
               {
                  // perfect
                  _Score += 15;
               }
               this.Controls.Remove(label);
               SetScoreBarLocation();
               //this.scoreBar.Update();
            }
         }
      }

      private void SetScoreBarLocation()
      {
         this.scoreBar.Location = new Point(0, GetHeight(_Score));
      }

      private int GetHeight(int score)
      {
         // Must subtract because 0 => top of panel
         int height = 580 * score / _MaxScore;
         return (score > 0) ? 580 - height : 580;
      }

      private void FinishGame()
      {
         if(_Score > int.Parse(_HighestScore))
         {
            _HighestScore = _Score.ToString();
            MessageBox.Show(string.Format("Amazing!\nScore : {0}, NEW! *High Score: {1}*", _Score, _HighestScore));
         }
         else
         {
            MessageBox.Show(string.Format("Good run!\nScore : {0}, High Score: {1}", _Score, _HighestScore));
         }
         
         File.WriteAllText(@"~HighestScore.txt", _HighestScore);
         AddStartControls();
      }

      private void RemoveStartControls()
      {
         this.Controls.Remove(this.startBar);
         this.Controls.Remove(this.speedSlider);
         this.Controls.Remove(this.speedText);
      }

      private void AddStartControls()
      {
         this.Controls.Add(this.startBar);
         this.Controls.Add(this.speedSlider);
         this.Controls.Add(this.speedText);
      }

      //##################################################################
      #region Check boxes to set or remove particular event handlers.

        //private void checkBoxOnMouseMove_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (checkBoxOnMouseMove.Checked)
        //    {
        //        HookManager.MouseMove += HookManager_MouseMove;
        //    }
        //    else
        //    {
        //        HookManager.MouseMove -= HookManager_MouseMove;
        //    }
        //}

        //private void checkBoxOnMouseClick_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (checkBoxOnMouseClick.Checked)
        //    {
        //        HookManager.MouseClick += HookManager_MouseClick;
        //    }
        //    else
        //    {
        //        HookManager.MouseClick -= HookManager_MouseClick;
        //    }
        //}

        //private void checkBoxOnMouseUp_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (checkBoxOnMouseUp.Checked)
        //    {
        //        HookManager.MouseUp += HookManager_MouseUp;
        //    }
        //    else
        //    {
        //        HookManager.MouseUp -= HookManager_MouseUp;
        //    }
        //}

        //private void checkBoxOnMouseDown_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (checkBoxOnMouseDown.Checked)
        //    {
        //        HookManager.MouseDown += HookManager_MouseDown;
        //    }
        //    else
        //    {
        //        HookManager.MouseDown -= HookManager_MouseDown;
        //    }
        //}

        //private void checkBoxMouseDoubleClick_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (checkBoxMouseDoubleClick.Checked)
        //    {
        //        HookManager.MouseDoubleClick += HookManager_MouseDoubleClick;
        //    }
        //    else
        //    {
        //        HookManager.MouseDoubleClick -= HookManager_MouseDoubleClick;
        //    }
        //}

        //private void checkBoxMouseWheel_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (checkBoxMouseWheel.Checked)
        //    {
        //        HookManager.MouseWheel += HookManager_MouseWheel;
        //    }
        //    else
        //    {
        //        HookManager.MouseWheel -= HookManager_MouseWheel;
        //    }
        //}

        //private void checkBoxKeyDown_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (checkBoxKeyDown.Checked)
        //    {
        //        HookManager.KeyDown += HookManager_KeyDown;
        //    }
        //    else
        //    {
        //        HookManager.KeyDown -= HookManager_KeyDown;
        //    }
        //}


        //private void checkBoxKeyUp_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (checkBoxKeyUp.Checked)
        //    {
        //        HookManager.KeyUp += HookManager_KeyUp;
        //    }
        //    else
        //    {
        //        HookManager.KeyUp -= HookManager_KeyUp;
        //    }
        //}

        //private void checkBoxKeyPress_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (checkBoxKeyPress.Checked)
        //    {
        //        HookManager.KeyPress += HookManager_KeyPress;
        //    }
        //    else
        //    {
        //        HookManager.KeyPress -= HookManager_KeyPress;
        //    }
        //}

        #endregion

      //##################################################################
      #region Event handlers of particular events. They will be activated when an appropriate checkbox is checked.

      private void HookManager_KeyDown(object sender, KeyEventArgs e)
      {
         switch (e.KeyCode)
         {
            case (Keys.H):
               h_label.BackColor = Color.Red;
               ScoreKeyDown(0);
               break;
            case (Keys.J):
               j_label.BackColor = Color.Green;
               ScoreKeyDown(1);
               break;
            case (Keys.K):
               k_label.BackColor = Color.Blue;
               ScoreKeyDown(2);
               break;
            case (Keys.L):
               l_label.BackColor = Color.Orange;
               ScoreKeyDown(3);
               break;
            default:
               break;
         }
      }

      private void HookManager_KeyUp(object sender, KeyEventArgs e)
        {
           switch (e.KeyCode)
           {
              case (Keys.H):
                 h_label.BackColor = Color.DarkRed;
                 break;
              case (Keys.J):
                 j_label.BackColor = Color.DarkGreen;
                 break;
              case (Keys.K):
                 k_label.BackColor = Color.DarkBlue;
                 break;
              case (Keys.L):
                 l_label.BackColor = Color.DarkOrange;
                 break;
              default:
                 break;
           }
        }

      //private void HookManager_KeyPress(object sender, KeyPressEventArgs e)
      //{
      //    textBoxLog.AppendText(string.Format("KeyPress - {0}\n", e.KeyChar));
      //    textBoxLog.ScrollToCaret();
      //} 


      //private void HookManager_MouseMove(object sender, MouseEventArgs e)
      //{
      //    labelMousePosition.Text = string.Format("x={0:0000}; y={1:0000}", e.X, e.Y);
      //}

      //private void HookManager_MouseClick(object sender, MouseEventArgs e)
      //{
      //    textBoxLog.AppendText(string.Format("MouseClick - {0}\n", e.Button));
      //    textBoxLog.ScrollToCaret();
      //}


      //private void HookManager_MouseUp(object sender, MouseEventArgs e)
      //{
      //    textBoxLog.AppendText(string.Format("MouseUp - {0}\n", e.Button));
      //    textBoxLog.ScrollToCaret();
      //}


      //private void HookManager_MouseDown(object sender, MouseEventArgs e)
      //{
      //    textBoxLog.AppendText(string.Format("MouseDown - {0}\n", e.Button));
      //    textBoxLog.ScrollToCaret();
      //}


      //private void HookManager_MouseDoubleClick(object sender, MouseEventArgs e)
      //{
      //    textBoxLog.AppendText(string.Format("MouseDoubleClick - {0}\n", e.Button));
      //    textBoxLog.ScrollToCaret();
      //}


      //private void HookManager_MouseWheel(object sender, MouseEventArgs e)
      //{
      //    labelWheel.Text = string.Format("Wheel={0:000}", e.Delta);
      //}

      #endregion
   }
}