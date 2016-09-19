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
   public delegate void MoveChangedEventHandler(object sender, MoveChangedEventArgs e);
   public delegate void TileChangedEventHandler(object sender, TileChangedEventArgs e);

   public class MoveChangedEventArgs : EventArgs
   {
      public bool AddMove { get; set; }
      public Cell CellAffected { get; set; }

      public MoveChangedEventArgs()
      {
      }
   }

   public class TileChangedEventArgs : EventArgs
   {
      public Cell CellAffected { get; set; }

      public TileChangedEventArgs()
      {
      }
   }
   
   public class Cell : Label
   {
      public event MoveChangedEventHandler OnMoveChanged;
      public event TileChangedEventHandler OnTileChanged;
      
      public CellInfoForm CellInfoPage { get; set; } 

      public bool Used { get; set; }
      public int Row { get; set; }
      public int Col { get; set; }
      public CellType Type { get; set; }
      private Tile _Tile;
      public Tile Tile 
      {
         get
         {
            return _Tile;
         }
         set
         {
            _Tile = value;
            if (_Tile != null)
            {
               this.Image = _Tile.Image;
            }
            else
            {
               this.Image = null;
            }
         }
      }

      public Cell(int row, int col, CellType type, Players player, bool real = true)
      {
         Row = row;
         Col = col;
         Type = type;
         
         Location = new Point(30 + col * Globals.ButtonSideLength, 30 + row * Globals.ButtonSideLength);
         Size = new Size(Globals.ButtonSideLength, Globals.ButtonSideLength);
         Name = row.ToString() + "," + col.ToString() + "," + player.ToString();
         BorderStyle = BorderStyle.Fixed3D;
         Tile = null;
         AllowDrop = true;
         Enabled = true;
         Visible = true;

         switch (type)
         {
            case (CellType.Empty):
               BackColor = Color.LightGray;
               Tag = "";
               Text = "";
               break;
            case (CellType.DoubleLetter):
               BackColor = Color.LightBlue;
               Tag = "DL";
               Text = "DL";
               break;
            case (CellType.DoubleWord):
               BackColor = Color.Red;
               ForeColor = Color.White;
               Tag = "DW";
               Text = "DW";
               break;
            case (CellType.TripleLetter):
               BackColor = Color.Green;
               ForeColor = Color.White;
               Tag = "TL";
               Text = "TL";
               break;
            case (CellType.TripleWord):
               BackColor = Color.Orange;
               Tag = "TW";
               Text = "TW";
               break;
            case (CellType.StartTile):
               BackColor = Color.Red;
               ForeColor = Color.White;
               Tag = "*";
               Text = "*";
               break;
            case (CellType.Hand):
               BackColor = Color.BurlyWood;
               Tag = "";
               Text = "";
               break;
            case (CellType.None):
            default:
               BackColor = Color.BurlyWood;
               break;
         }

         if (real)
         {
            DragDrop += new DragEventHandler(cell_DragDrop);
            DragEnter += new DragEventHandler(cell_DragEnter);
            MouseDown += new MouseEventHandler(cell_MouseDown);
            MouseUp += new MouseEventHandler(cell_MouseUp);
         }
      }

      public static void SwapTiles(Cell cell1, Cell cell2)
      {
         var tempTile = cell2.Tile;
         cell2.Tile = cell1.Tile;
         cell1.Tile = tempTile;
      }

      #region Events
      public void cell_DragDrop(object sender, DragEventArgs e)
      {
         Cell toCell = (Cell)sender;
         Cell fromCell = (Cell)e.Data.GetData(typeof(Cell));
         //Cell fromCell = this;
         //toCell.Text = "";

         if (toCell.Used || fromCell.Used)
         {
            return;
         }

         if (toCell.Name == fromCell.Name && toCell.Tile.IsBlankTile)
         {
            if (OnTileChanged == null) return;
            OnTileChanged(this, new TileChangedEventArgs() { CellAffected = toCell });
            return;
         }

         bool addMove = false;
         bool remMove = false;

         if (toCell.Tile == null)
         {
            if (toCell.Type != CellType.Hand)
            {
               addMove = true;
            }
            if (fromCell.Type != CellType.Hand)
            {
               remMove = true;
            }
         }

         SwapTiles(fromCell, toCell);

         if (toCell.Tile != null)
         {
            toCell.Text = "";
         }

         if (fromCell.Tile != null)
         {
            fromCell.Text = "";
         }

         if (!addMove && !remMove) 
         {
            if (OnMoveChanged == null) return;
            OnMoveChanged(this, new MoveChangedEventArgs() { AddMove = false, CellAffected = null });
         }
         else
         {
            if (addMove)
            {
               if (OnMoveChanged == null) return;
               OnMoveChanged(this, new MoveChangedEventArgs() { AddMove = true, CellAffected = toCell });
            }
            if (remMove)
            {
               if (OnMoveChanged == null) return;
               OnMoveChanged(this, new MoveChangedEventArgs() { AddMove = false, CellAffected = fromCell });
            }
         }

         //SwapTiles(fromCell, toCell);

         // Trigger move update
         //UpdateYourCurrentMove();
      }

      public void cell_DragEnter(object sender, DragEventArgs e)
      {
         e.Effect = DragDropEffects.Move;
      }

      public void cell_MouseDown(object sender, MouseEventArgs e)
      {
         Cell cell = (Cell)sender;
         if (e.Button == MouseButtons.Right)
         {
            int x = Cursor.Position.X - e.X;
            int y = Cursor.Position.Y - e.Y;

            var rowCol = cell.Name.Split(',');
            int row = int.Parse(rowCol[0]);
            int col = int.Parse(rowCol[1]);

            CellInfoPage = new CellInfoForm(this, x, y);
            CellInfoPage.Show();

         }
         else if (e.Button == MouseButtons.Left)
         {
            if (cell.Image == null || cell.Used)
            {
               return;
            }

            //var dataObj = new DataObject(cell);
            //dataObj.SetData("Cell", cell);
            cell.Text = cell.Tag.ToString();
            cell.DoDragDrop(this, DragDropEffects.Move);
         }
      }

      public void cell_MouseUp(object sender, MouseEventArgs e)
      {
         if (e.Button == MouseButtons.Right && CellInfoPage != null)
         {
            CellInfoPage.Close();
         }
      }
      #endregion
   }
}
