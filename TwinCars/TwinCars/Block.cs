using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace TwinCars
{
   public enum BlockType
   {
      Circle,
      Square
   }
   
   public class Block
   {
      public Bitmap Image { get; set; }
      public BlockType Type { get; set; }
      public float YAxis { get; set; }
      public float XAxis { get; set; }


      public Block(Bitmap image, float x, float y, BlockType type)
      {
         Image = image;
         XAxis = x;
         YAxis = y;
         Type = type;
      }

      public Bitmap GetImage(out float x, out float y)
      {
         x = XAxis;
         y = YAxis;
         return Image;
      }

   }
}
