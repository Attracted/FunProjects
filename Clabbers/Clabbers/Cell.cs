using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using Clabbers.Properties;

namespace Clabbers
{
   public class Cell
   {
      public bool Used { get; set; }
      public int Row { get; set; }
      public int Col { get; set; }
      public CellType Type { get; set; }
      public Label Label { get; set; }
      public Tile Tile { get; set; }
      public int Value { get; set; }

      public Cell()
      {
      }
   }
}
