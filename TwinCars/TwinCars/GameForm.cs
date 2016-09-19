using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using TwinCars.Properties;

namespace TwinCars
{  
   public partial class GameForm : Form
   {
      Bitmap bm = new Bitmap(200, 375);

      private Timer _Timer;

      private Car RedCar;
      private Car BlueCar;

      Color DeepBlue = Color.FromArgb(0, 10, 25);

      private string _HighestScore;
      private int Score
      {
         get
         {
            return int.Parse(this.score.Text);
         }
         set
         {
            this.score.Text = value.ToString();
         }
      }

      private int _TicksUntilNextRedBlock;
      private int _TicksUntilNextBlueBlock;

      private SpeedManager _SpeedManager;

      private bool _GameOver;

      private bool _FirstLoad = true;

      private List<Block> Blocks;
      private List<Particle> Particles;

      private Random _Seed;

      private Queue<Lane> _MoveQueue;
      
      public GameForm()
      {
         InitializeComponent();
      }

      private void GameForm_Load(object sender, EventArgs e)
      {
         this.DoubleBuffered = true;

         using (Graphics gfx = Graphics.FromImage(bm))
         using (SolidBrush brush = new SolidBrush(DeepBlue))
         {
            gfx.FillRectangle(brush, 0, 0, 200, 375);
            gfx.DrawLine(new Pen(Color.DeepSkyBlue, 3), 100, 0, 100, 375);
            gfx.DrawLine(new Pen(Color.DeepSkyBlue, 1), 50, 0, 50, 375);
            gfx.DrawLine(new Pen(Color.DeepSkyBlue, 1), 150, 0, 150, 375);
         }
      }

      private void startBar_Click(object sender, EventArgs e)
      {
         RedCar = new Car(Lane.Left);
         BlueCar = new Car(Lane.Right);

         _HighestScore = "0";
         if (File.Exists(@"~HighestScore.txt"))
         {
            _HighestScore = File.ReadAllText(@"~HighestScore.txt");
         }

         if (_FirstLoad)
         {
            HookManager.KeyDown += HookManager_KeyDown;
            this.Paint += new PaintEventHandler(Form1_Paint);
            _FirstLoad = false;
         }

         RemoveStartControls();

         _Seed = new Random();
         _MoveQueue = new Queue<Lane>(2);

         _TicksUntilNextRedBlock = 0;
         _TicksUntilNextBlueBlock = _Seed.Next(1, 5);

         _SpeedManager = new SpeedManager(30);

         _GameOver = false;
         Score = 0;

         Blocks = new List<Block>();
         Particles = new List<Particle>();
         _Timer = new Timer()
         {
            Interval = 5
         };
         _Timer.Tick += new EventHandler(_Timer_Tick);

         _Timer.Start();

         //this.Refresh();
      }

      private void _Timer_Tick(object sender, EventArgs e)
      {
         _Timer.Stop();

         if (_GameOver)
         {
            return;
         }

         _TicksUntilNextRedBlock++;
         _TicksUntilNextBlueBlock++;

         if (_TicksUntilNextRedBlock >= _SpeedManager.BlockCreateSpeed)
         {
            var rand = _Seed.Next(4);
            switch (rand)
            {
               case 0: // red circle + nothing
                  Blocks.Add(new Block(Resources.RedCircle, 11, 0, BlockType.Circle));
                  break;
               case 1: // red square + nothing
                  Blocks.Add(new Block(Resources.RedSquare, 11, 0, BlockType.Square));
                  break;
               case 2: // nothing + red circle
                  Blocks.Add(new Block(Resources.RedCircle, 61, 0, BlockType.Circle));
                  break;
               case 3: // nothing + red square
                  Blocks.Add(new Block(Resources.RedSquare, 61, 0, BlockType.Square));
                  break;
               default:
                  break;
            }
            _TicksUntilNextRedBlock = 0;
         }

         if (_TicksUntilNextBlueBlock >= _SpeedManager.BlockCreateSpeed)
         {
            var rand = _Seed.Next(4);
            switch (rand)
            {
               case 0: // blue circle + nothing
                  Blocks.Add(new Block(Resources.BlueCircle, 111, 0, BlockType.Circle));
                  break;
               case 1: // blue square + nothing
                  Blocks.Add(new Block(Resources.BlueSquare, 111, 0, BlockType.Square));
                  break;
               case 2: // nothing + blue circle
                  Blocks.Add(new Block(Resources.BlueCircle, 161, 0, BlockType.Circle));
                  break;
               case 3: // nothing + blue square
                  Blocks.Add(new Block(Resources.BlueSquare, 161, 0, BlockType.Square));
                  break;
               default:
                  break;
            }
            _TicksUntilNextBlueBlock = 0;
         }

         if(_TicksUntilNextBlueBlock % 5 == 0)
         {
            CreateParticles(Lane.Left);
            CreateParticles(Lane.Right);
         }

         Invalidate();

         _SpeedManager.SetSpeeds(Score);

         _Timer.Start();
      }

      private void CreateParticles(Lane lane)
      {
         var angle = _Seed.Next(45);
         var resize = _Seed.Next(5, 10);
         var turning = (_Seed.Next(2) == 1) ? Turning.Right : Turning.Left;
         var xDrift = _Seed.Next(-1, 2);

         Bitmap img;
         float xPart = 10f;
         float yPart = 343f;
         if (lane == Lane.Left)
         {
            if (RedCar.TurningEnum == Turning.Left)
            {
               xPart += 5;
            }
            xPart += RedCar.GetXAxis();
            img = new Bitmap(Resources.RedParticle, resize, resize);
         }
         else
         {
            if (BlueCar.TurningEnum == Turning.Left)
            {
               xPart += 5;
            }
            xPart += BlueCar.GetXAxis();
            img = new Bitmap(Resources.BlueParticle, resize, resize);
         }

         Particles.Add(new Particle(img, xPart, yPart, xDrift, angle, turning));
      }

      private void ScoreKeyDown(int column)
      {
         if (column == 0)
         {
            // Add to queue
            RedCar.EnableTurn = true;
            if (_MoveQueue.Count < 2)
            {
               _MoveQueue.Enqueue(Lane.Left);
            }
         }
         else if (column == 1)
         {
            // Add to queue
            BlueCar.EnableTurn = true;
            if (_MoveQueue.Count < 2)
            {
               _MoveQueue.Enqueue(Lane.Right);
            }
         }
      }

      private void FinishGame()
      {
         CleanUp();
         if(Score > int.Parse(_HighestScore))
         {
            _HighestScore = Score.ToString();
            MessageBox.Show(string.Format("Amazing!\nScore : {0}, NEW! *High Score: {1}*", Score, _HighestScore));
         }
         else
         {
            MessageBox.Show(string.Format("Good run!\nScore : {0}, High Score: {1}", Score, _HighestScore));
         }
         
         File.WriteAllText(@"~HighestScore.txt", _HighestScore);
         AddStartControls();
      }

      private void CleanUp()
      {
         Blocks.Clear();
         Particles.Clear();
         _Timer.Stop();
         _Timer.Dispose();
         _GameOver = true;
      }

      private void RemoveStartControls()
      {
         this.Controls.Remove(this.startBar);
      }

      private void AddStartControls()
      {
         this.Controls.Add(this.startBar);
      }

      private bool CheckHitSquare(Block block, Bitmap bm)
      {
         int x = (int)block.XAxis;
         int y = (int)block.YAxis;
         
         if (bm.GetPixel(x, y) != DeepBlue)
         {
            return true;
         }

         if(bm.GetPixel(x + 28,  y) != DeepBlue)
         {
            return true;
         }

         if(y + 28 < 343 && bm.GetPixel(x,  y + 28) != DeepBlue)
         {
            return true;
         }

         if (y + 28 < 343 && bm.GetPixel(x + 28, y + 28) != DeepBlue)
         {
            return true;
         }

         return false;
      }

      private bool CheckHitCircle(Block block, Bitmap bm)
      {
         int x = (int)block.XAxis;
         int y = (int)block.YAxis;

         if (bm.GetPixel(x + 14, y) != DeepBlue)
         {
            return true;
         }

         if (y + 28 < 343 && bm.GetPixel(x, y + 28) != DeepBlue)
         {
            return true;
         }

         if (y + 14 < 343 && bm.GetPixel(x + 14, y + 14) != DeepBlue)
         {
            return true;
         }

         if (y + 14 < 343 && bm.GetPixel(x + 28, y + 14) != DeepBlue)
         {
            return true;
         }

         return false;
      }

      private void Form1_Paint(object sender, PaintEventArgs e)
      {
         if (_GameOver)
         {
            return;
         }

         if (_MoveQueue.Count > 0)
         {
            var move = _MoveQueue.Dequeue();

            if (move == Lane.Left)
            {
               RedCar.ChangeLanes();

            }
            if (move == Lane.Right)
            {
               BlueCar.ChangeLanes();
            }
         }

         if (RedCar.EnableTurn)
         {
            RedCar.UpdateLanes();
         }
         if (BlueCar.EnableTurn)
         {
            BlueCar.UpdateLanes();
         }

         var newBM = new Bitmap(bm);
         using (Graphics g = Graphics.FromImage(newBM))
         {
            int x = 0;
            int y = 0;
            var img = BlueCar.GetImage(out x, out y);
            g.DrawImage(img, x, y);

            x = 0;
            y = 0;
            img = RedCar.GetImage(out x, out y);
            g.DrawImage(img, x, y);

            #region Blocks
            var blocksToRemove = new List<Block>();
            foreach (var block in Blocks)
            {
               block.YAxis += _SpeedManager.BlockMoveSpeed(Score);

               if (block.YAxis >= 343)
               {
                  if (block.Type == BlockType.Circle)
                  {
                     //MISS
                     blocksToRemove.Clear();
                     FinishGame();
                     return;
                  }
                  blocksToRemove.Add(block);
               }
               else
               {
                  if (block.YAxis >= 262 && block.Type == BlockType.Circle)
                  {
                     if (CheckHitCircle(block, newBM))
                     {
                        // Hit Car
                        blocksToRemove.Add(block);
                        Score++;
                     }
                  }
                  else if (block.YAxis >= 262 && block.Type == BlockType.Square)
                  {
                     if (CheckHitSquare(block, newBM))
                     {
                        //Hit Car
                        FinishGame();
                        return;
                     }
                  }
               }

               float xAxis = 0;
               float yAxis = 0;
               img = block.GetImage(out xAxis, out yAxis);
               img.MakeTransparent(DeepBlue);
               g.DrawImage(img, xAxis, yAxis);
            }

            foreach (var block in blocksToRemove)
            {
               Blocks.Remove(block);
            }
            #endregion

            #region Particles
            var particlesToRemove = new List<Particle>();
            foreach (var particle in Particles)
            {
               particle.YAxis += 1;
               float xPart = 0;
               float yPart = 0;
               var image = particle.GetImage(out xPart, out yPart);

               g.DrawImage(image, xPart, yPart);

               if (yPart >= 360)
               { 
                  particlesToRemove.Add(particle); 
               }
            }

            foreach (var particle in particlesToRemove)
            {
               Particles.Remove(particle);
            }
            #endregion

            e.Graphics.DrawImage(newBM, 14, 88);
         }
      }

      public static Bitmap RotateImg(Bitmap bmp, float angle, Color bkColor)
      {
         angle = angle % 360;
         if (angle > 180)
            angle -= 360;

         System.Drawing.Imaging.PixelFormat pf = default(System.Drawing.Imaging.PixelFormat);
         if (bkColor == Color.Transparent)
         {
            pf = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
         }
         else
         {
            pf = bmp.PixelFormat;
         }

         float sin = (float)Math.Abs(Math.Sin(angle * Math.PI / 180.0)); // this function takes radians
         float cos = (float)Math.Abs(Math.Cos(angle * Math.PI / 180.0)); // this one too
         float newImgWidth = sin * bmp.Height + cos * bmp.Width;
         float newImgHeight = sin * bmp.Width + cos * bmp.Height;
         float originX = 0f;
         float originY = 0f;

         if (angle > 0)
         {
            if (angle <= 90)
               originX = sin * bmp.Height;
            else
            {
               originX = newImgWidth;
               originY = newImgHeight - sin * bmp.Width;
            }
         }
         else
         {
            if (angle >= -90)
               originY = sin * bmp.Width;
            else
            {
               originX = newImgWidth - sin * bmp.Height;
               originY = newImgHeight;
            }
         }

         Bitmap newImg = new Bitmap((int)newImgWidth, (int)newImgHeight, pf);
         Graphics g = Graphics.FromImage(newImg);
         g.Clear(bkColor);
         g.TranslateTransform(originX, originY); // offset the origin to our calculated values
         g.RotateTransform(angle); // set up rotate
         g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
         g.DrawImageUnscaled(bmp, 0, 0); // draw the image at 0, 0
         g.Dispose();
         newImg.MakeTransparent(bkColor);

         return newImg;
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
            case (Keys.Left):
               ScoreKeyDown(0);
               break;
            case (Keys.Right):
               ScoreKeyDown(1);
               break;
            default:
               break;
         }
      }

      //private void HookManager_KeyUp(object sender, KeyEventArgs e)
      //{
      //}

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