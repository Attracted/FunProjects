using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clabbers
{
	public class Tile
	{
		public char Letter { get; set; }
		public int Score { get; set; }
		public bool IsBlankTile { get; set; }

		public Tile()
		{
		}
		public Tile(Tile tile)
		{
			Letter = tile.Letter;
			Score = tile.Score;
			IsBlankTile = tile.IsBlankTile;
		}
	}
}
