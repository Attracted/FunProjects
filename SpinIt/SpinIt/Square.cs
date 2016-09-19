using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using SpinIt.Properties;

namespace SpinIt
{  
   public enum Direction
   {
      None,
      Right,
      Left
   }
   
   public class Square
   {
      Color DeepBlue = Color.FromArgb(0, 10, 25);

      private CircularList<float> Angles;
      private float Angle;

      private Direction TurningDirection;
      private Bitmap Image;
      private Bitmap TurningImage;

      private Rectangle Shape = new Rectangle(0, 0, 28, 28);

      public bool EnableTurn { get; set; }

      public Square()
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

         Image = Resources.Square;
         TurningImage = Image;
      }

      public void UpdateLanes()
      {
         if (!EnableTurn)
         {
            return;
         }

         TurningImage = RotateImage();

         if (Angle == GetNextAngle(TurningDirection))
         {
            SetNextAngle(TurningDirection);
            TurningDirection = Direction.None;
            EnableTurn = false;
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
         x = Shape.X;
         y = Shape.Y;
         return TurningImage;
      }

      public Bitmap RotateImage()
      {
         if (TurningDirection == Direction.Left)
         {
            Angle -= 15;

            if (Angle < 0)
            {
               Angle += 360;
            }
         }
         else if (TurningDirection == Direction.Right)
         {
            Angle += 15;

            if (Angle >= 360)
            {
               Angle -= 360;
            }
         }

         //return GameForm.RotateImg(Image, Angle, DeepBlue);
         return GameForm.RotateImage(Image, Angle);
         //return GameForm.RotateImage(Image, Angle, false, true, DeepBlue);
      }
   }
}