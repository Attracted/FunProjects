using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Clabbers
{
	public partial class CellInfoForm : Form
	{
		public CellInfoForm()
		{
			InitializeComponent();
		}

		public CellInfoForm(Cell cell, int x, int y)
		{
			InitializeComponent();
			this.row.Text = cell.Row.ToString();
			this.col.Text = cell.Col.ToString();
			if (cell.Tile != null)
			{
				this.letter.Text = cell.Tile.Letter.ToString();
				this.score.Text = cell.Tile.Score.ToString();
				this.image.Image = cell.Label.Image;
			}			
			this.used.Text = (cell.Used) ? "True" : "False";

			string type;
			switch (cell.Type)
			{
				case (CellType.Empty):
					type = "Empty";
					break;
				case (CellType.DoubleLetter):
					type = "DoubleLetter";
					break;
				case (CellType.DoubleWord):
					type = "DoubleWord";
					break;
				case (CellType.TripleLetter):
					type = "TripleLetter";
					break;
				case (CellType.TripleWord):
					type = "TripleWord";
					break;
				case (CellType.StartTile):
					type = "StartTile";
					break;
				case (CellType.Hand):
					type = "Hand";
					break;
				default:
					type = "";
					break;
			}
			this.tileType.Text = type;
			this.value.Text = cell.Value.ToString();

			this.StartPosition = FormStartPosition.Manual;
			this.Location = new Point(x, y);
		}

		private void Form3_Load(object sender, EventArgs e)
		{
		}
	}
}
