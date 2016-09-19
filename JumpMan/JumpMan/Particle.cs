using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace JumpMan
{
   class Particle
   {
      Color DeepBlue = Color.FromArgb(0, 10, 25);

      public Bitmap Image { get; set; }
      public float XAxis { get; set; }
      public float YAxis { get; set; }
      public float XDrift { get; set; }
      public float Angle { get; set; }
      public Direction Spin { get; set; }

      public Particle(Bitmap image, float x, float y, float xDrift, float angle, Direction spin)
      {
         Image = image;
         XAxis = x;
         YAxis = y;
         Angle = angle;
         Spin = spin;
         XDrift = xDrift;
      }

      public Bitmap GetImage(out float x, out float y)
      {
         x = XAxis + XDrift;
         y = YAxis;
         if (Spin == Direction.Left)
         {
            Angle -= 5;
         }
         else if (Spin == Direction.Right)
         {
            Angle += 5;
         }
         //return GameForm.RotateImg(Image, Angle, DeepBlue);
         return GameForm.RotateImage(Image, Angle);
      }
   }
}
