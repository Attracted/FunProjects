using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using JumpMan.Properties;
using System.Diagnostics;

namespace JumpMan
{  
   public enum Direction
   {
      None,
      Right,
      Left,
      Up
   }
   
   public class Player
   {
      Color DeepBlue = Color.FromArgb(0, 10, 25);

      private CircularList<float> Angles;
      private float Angle;

      public Direction TurningDirection { get; private set; }

      private Bitmap Image;
      private Bitmap TurningImage;

      private Rectangle Shape = new Rectangle(0, 0, GlobalValues.PLAYER_WIDTH, GlobalValues.PLAYER_WIDTH);



      public Point Location { get; set; }
      public bool OntopObstacle { get; set; }
      public bool EnableTurn { get; set; }

      private bool _EnableJump;
      public bool EnableJump 
      {
         get
         {
            return _EnableJump;
         } 
         set
         {
            _EnableJump = value;

            if (_EnableJump)
            {
               _JumpWatch.Start();
            }
            else
            {
               _JumpWatch.Reset();
            }
         }
      }

      private bool _EnableFall;
      public bool EnableFall
      {
         get
         {
            return _EnableFall;
         }
         set
         {
            _EnableFall = value;

            if (_EnableFall)
            {
               _JumpWatch.Start();
            }
            else
            {
               _JumpWatch.Reset();
            }
         }
      }

      private Stopwatch _JumpWatch;

      public Player()
      {
         EnableTurn = false;

         Angles = new CircularList<float>();
         Angles.Add(0);
         Angles.Add(45);
         Angles.Add(90);
         Angles.Add(135);
         Angles.Add(180);
         Angles.Add(225);
         Angles.Add(270);
         Angles.Add(315);

         Angle = 0f;

         TurningDirection = Direction.None;

         Image = Resources.Circle;
         TurningImage = Image;

         Location = new Point(GlobalValues.PLAYER_START_X, GlobalValues.PLAYER_START_Y);

         _JumpWatch = new Stopwatch();
      }

      public void UpdateTurn(bool enableLeft, bool enableRight)
      {
         if (!EnableTurn)
         {
            return;
         }

         TurningImage = RotateImage();

         if (!ContinueTurn(enableLeft, enableRight))
         {
            //SetNextAngle(TurningDirection);
            TurningDirection = Direction.None;
            EnableTurn = false;
         }

         //if (Angle == GetNextAngle(TurningDirection))
         //{
         //   SetNextAngle(TurningDirection);
         //   TurningDirection = Direction.None;
         //   EnableTurn = false;
         //}
      }

      public void UpdateJump()
      {
         if (EnableJump || EnableFall)
         {
            var time = _JumpWatch.ElapsedMilliseconds;
            var timepow2 = Math.Pow(time, 2) / 10000;
            var timeroot = Math.Sqrt(time);

            if (EnableJump)
            {
               //Location = new Point(Location.X, 150 - Convert.ToInt32(75 * Math.Sin(Math.PI * _JumpWatch.ElapsedMilliseconds / 750)));

               Location = new Point(Location.X, GlobalValues.PLAYER_START_Y - Convert.ToInt32(7 * timeroot - (9.81 * timepow2)));
            }
            else //if (EnableFall)
            {
               Location = new Point(Location.X, GlobalValues.PLAYER_START_Y - Convert.ToInt32(- 9.81 * timepow2));
            }

            // TODO: Collision detection here
            if (Location.Y > GlobalValues.PLAYER_START_Y)
            {
               EnableJump = false;
               EnableFall = false;
               Location = new Point(Location.X, GlobalValues.PLAYER_START_Y);
            }
         }
      }

      private float GetNextAngle(Direction direction)
      {
         if (direction == Direction.Left)
         {
            return Angles.PeekPrev();
         }
         else if (direction == Direction.Right)
         {
            return Angles.PeekNext();
         }
         else
         {
            return Angles.Current();
         }
      }

      private void SetNextAngle(Direction direction)
      {
         if (direction == Direction.Left)
         {
            Angles.GetPrev();
         }
         else if (direction == Direction.Right)
         {
            Angles.GetNext();
         }
      }

      public void ChangeLanes(Direction dir)
      {
         if (!EnableTurn)
         {
            return;
         }

         if (TurningDirection == Direction.None)
         {
            TurningDirection = dir;
         }
      }

      public Bitmap GetImage(out int x, out int y)
      {
         x = Shape.X + Location.X;
         y = Shape.Y + Location.Y;
         return TurningImage;
      }

      public Bitmap RotateImage()
      {
         if (TurningDirection == Direction.Left)
         {
            Angle -= GlobalValues.PLAYER_ANGLE_INCREMENT;

            if (Angle < 0)
            {
               Angle += 360;
            }
         }
         else if (TurningDirection == Direction.Right)
         {
            Angle += GlobalValues.PLAYER_ANGLE_INCREMENT;

            if (Angle >= 360)
            {
               Angle -= 360;
            }
         }

         //return GameForm.RotateImg(Image, Angle, DeepBlue);
         return GameForm.RotateImage(Image, Angle);
         //return GameForm.RotateImage(Image, Angle, false, true, DeepBlue);
      }

      private bool ContinueTurn(bool enableLeft, bool enableRight)
      {
         return (enableLeft && TurningDirection == Direction.Left)
            || (enableRight && TurningDirection == Direction.Right);
      }
   }
}