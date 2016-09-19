using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Drawing.Imaging;
using System.Windows.Forms;
using JumpMan.Properties;
using System.Windows.Input;

namespace JumpMan
{  
   public partial class GameForm : Form
   {
      //Bitmap bm = new Bitmap(200, 200);
      Bitmap bm = new Bitmap(Resources.Hills);

      private Timer _Timer;

      private Player _Player;
      private Background _Background;

      Color DeepBlue = Color.FromArgb(0, 10, 25);

      private bool _GameOver;

      private bool _FirstLoad = true;

      private Random _Seed;

      private Queue<Direction> _TurnQueue;
      
      public GameForm()
      {
         InitializeComponent();
      }

      private void GameForm_Load(object sender, EventArgs e)
      {
         this.DoubleBuffered = true;

         //using (Graphics gfx = Graphics.FromImage(bm))
         //using (SolidBrush brush = new SolidBrush(DeepBlue))
         //{
         //   gfx.FillRectangle(brush, 0, 0, 200, 200);
         //   //gfx.DrawArc(new Pen(Color.Red), 0, 0, 150, 150, 30, 30);
         //}
      }

      private void startBar_Click(object sender, EventArgs e)
      {
         _Player = new Player();
         _Background = new Background();

         if (_FirstLoad)
         {
            //HookManager.KeyDown += HookManager_KeyDown;
            this.Paint += new PaintEventHandler(Form1_Paint);
            _FirstLoad = false;
         }

         RemoveStartControls();

         _Seed = new Random();
         _TurnQueue = new Queue<Direction>(2);

         _GameOver = false;

         _Timer = new Timer()
         {
            Interval = 5
         };
         _Timer.Tick += new EventHandler(_Timer_Tick);

         _Timer.Start();

      }

      private void _Timer_Tick(object sender, EventArgs e)
      {
         _Timer.Stop();

         if (_GameOver)
         {
            AddStartControls();
            return;
         }

         Invalidate();

         _Timer.Start();
      }

      private void CheckKeysPressed()
      {         
         if (Keyboard.IsKeyDown(Key.Up))
         {
            UpPressed();
         }
         if (Keyboard.IsKeyDown(Key.Left))
         {
            LeftPressed();
         }
         if (Keyboard.IsKeyDown(Key.Right))
         {
            RightPressed();
         }
      }

     
      private void UpPressed()
      {
         if (!_Player.EnableJump)
         {
            _Player.EnableJump = true;
         }
      }

      private void LeftPressed()
      {
         LeftOrRightPressed(Direction.Left);
         
         //var loc = _Player.Location;
         //loc.Offset(-10, 0);
         //_Player.Location = loc;
      }

      private void RightPressed()
      {
         LeftOrRightPressed(Direction.Right);
         
         //var loc = _Player.Location;
         //loc.Offset(10, 0);
         //_Player.Location = loc;
      }

      private void LeftOrRightPressed(Direction dir)
      {
         _Player.EnableTurn = true;
         _Background.EnableOffset = true;

         if (_TurnQueue.Count < 2)
         {
            _TurnQueue.Enqueue(dir);
         }
      }

      //private void KeyPressed(Direction dir)
      //{

      //}

      private void FinishGame()
      {
         CleanUp();

         AddStartControls();
      }

      private void CleanUp()
      {
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

      private void Form1_Paint(object sender, PaintEventArgs e)
      {
         if (_GameOver)
         {
            return;
         }

         CheckKeysPressed();

         if (_TurnQueue.Count > 0)
         {
            var move = _TurnQueue.Dequeue();
            _Player.ChangeLanes(move);
            _Background.ChangeLanes(move);
         }

         var enableLeft = Keyboard.IsKeyDown(Key.Left);
         var enableRight = Keyboard.IsKeyDown(Key.Right);

         _Player.UpdateTurn(enableLeft, enableRight);
         _Player.UpdateJump();

         _Background.UpdateOffset(enableLeft, enableRight);

         var result = _Background.CheckObstacleCollision(_Player.Location);

         if (result != null)
         {
            if (result.Value == true)
            {
               _GameOver = true;
            }
            else
            {
               // NOTE: Player is on top of an object

               _Player.Location = new Point(_Player.Location.X, GlobalValues.PLAYER_START_Y - GlobalValues.PLAYER_HEIGHT + 1);
               _Player.EnableJump = false;
               _Player.OntopObstacle = true;
            }
         }
         else
         {
            if (_Player.OntopObstacle)
            {
               _Player.EnableFall = true;
            }
         }

         this.Text = string.Format("JumpMap! ~ Score: {0} ~", _Background.PassedObstacles);
         
         //var newBM = new Bitmap(bm);
         var newBM = _Background.GetImage();
         using (Graphics g = Graphics.FromImage(newBM))
         {
            int x = 0;
            int y = 0;
            var img = _Player.GetImage(out x, out y);
            g.DrawImage(img, x, y);

            e.Graphics.DrawImage(newBM, 0, 0);
         }
      }

      public static Bitmap RotateImage(Bitmap bmp, float angle)
      {
         //create an empty Bitmap image
         //Bitmap bmp = new Bitmap(bmp.Width, bmp.Height);

         //turn the Bitmap into a Graphics object
         //Graphics gfx = Graphics.FromImage(bmp);

         //float sin = (float)Math.Abs(Math.Sin(angle * Math.PI / 180.0)); // this function takes radians
         //float cos = (float)Math.Abs(Math.Cos(angle * Math.PI / 180.0)); // this one too
         //float newImgWidth = sin * bmp.Height + cos * bmp.Width;
         //float newImgHeight = sin * bmp.Width + cos * bmp.Height;

         System.Drawing.Imaging.PixelFormat pf = default(System.Drawing.Imaging.PixelFormat);
         pf = bmp.PixelFormat;

         //Bitmap newImg = new Bitmap((int)newImgWidth, (int)newImgHeight, pf);
         Bitmap newImg = new Bitmap((int)bmp.Width, (int)bmp.Height);
         Graphics g = Graphics.FromImage(newImg);

         //now we set the rotation point to the center of our image
         g.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

         //now rotate the image
         g.RotateTransform(angle);

         g.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

         //set the InterpolationMode to HighQualityBicubic so to ensure a high
         //quality image once it is transformed to the specified size
         g.InterpolationMode = InterpolationMode.HighQualityBicubic;

         //now draw our new image onto the graphics object
         g.DrawImage(bmp, new Point(0,0));

         //dispose of our Graphics object
         g.Dispose();

         //return the image
         return newImg;
      }

      //public static Bitmap RotateImg(Bitmap bmp, float angle, Color bkColor)
      //{
      //   angle = angle % 360;
      //   if (angle > 180)
      //   {
      //      angle -= 360;
      //   }
      //   else if (angle < -180)
      //   {
      //      angle += 360;
      //   }

      //   System.Drawing.Imaging.PixelFormat pf = default(System.Drawing.Imaging.PixelFormat);
      //   if (bkColor == Color.Transparent)
      //   {
      //      pf = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
      //   }
      //   else
      //   {
      //      pf = bmp.PixelFormat;
      //   }

      //   float sin = (float)Math.Abs(Math.Sin(angle * Math.PI / 180.0)); // this function takes radians
      //   float cos = (float)Math.Abs(Math.Cos(angle * Math.PI / 180.0)); // this one too
      //   float newImgWidth = sin * bmp.Height + cos * bmp.Width;
      //   float newImgHeight = sin * bmp.Width + cos * bmp.Height;
      //   float originX = 0f;
      //   float originY = 0f;

      //   if (angle > 0)
      //   {
      //      if (angle <= 90)
      //         originX = sin * bmp.Height;
      //      else
      //      {
      //         originX = newImgWidth;
      //         originY = newImgHeight - sin * bmp.Width;
      //      }
      //   }
      //   else
      //   {
      //      if (angle >= -90)
      //         originY = sin * bmp.Width;
      //      else
      //      {
      //         originX = newImgWidth - sin * bmp.Height;
      //         originY = newImgHeight;
      //      }
      //   }

      //   Bitmap newImg = new Bitmap((int)newImgWidth, (int)newImgHeight, pf);
      //   Graphics g = Graphics.FromImage(newImg);
      //   g.Clear(bkColor);
      //   g.TranslateTransform(originX, originY); // offset the origin to our calculated values
      //   g.RotateTransform(angle); // set up rotate
      //   g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
      //   g.DrawImageUnscaled(bmp, 0, 0); // draw the image at 0, 0
      //   g.Dispose();
      //   newImg.MakeTransparent(bkColor);

      //   return newImg;
      //}

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

      //private void HookManager_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
      //{
      //   switch (e.KeyCode)
      //   {
      //      case (Keys.Left):
      //         KeyPressed(Direction.Left);
      //         break;
      //      case (Keys.Right):
      //         KeyPressed(Direction.Right);
      //         break;
      //      case (Keys.Up):
      //         KeyPressed(Direction.Up);
      //         break;
      //      default:
      //         break;
      //   }
      //}

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