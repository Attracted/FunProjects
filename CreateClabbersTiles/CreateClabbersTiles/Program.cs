using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using CreateClabbersTiles.Properties;

namespace CreateClabbersTiles
{
	public class Program
	{
		
		static void Main(string[] args)
		{
			// Update this if new tiles are needed
         MakeTiles(Resources.DefaultTiles);
		}

		static private void MakeTiles(string tiles)
		{
			using (StringReader input = new StringReader(tiles))
			{
				bool first = true;
				int score = -1;
				while (input.Peek() != -1)
				{
					if (first)
					{
						first = false;
						continue;
					}

					score++;
					var line = input.ReadLine();

					if (line == "")
					{
						continue;
					}

					string[] pairs;
					if (line.Contains(','))
					{
						pairs = line.Split(',');
					}
					else
					{
						pairs = new string[] { line };
					}

					foreach (string pair in pairs)
					{
						var letterCount = pair.Split('=');
						var letter = letterCount[0];
						string saveLetter;
						string drawScore;
						if (letter == "*")
						{
							saveLetter = "_";
							drawScore = "";
							letter = " ";
							
						}
						else
						{
							letter = letter.ToUpper();
							drawScore = score.ToString();
							saveLetter = letter;
						}
						// Create bitmap With large letter and small score
						using (var bmp = new Bitmap(Resources.Background)) 
						{
							using (Graphics g = Graphics.FromImage(bmp))
							{
								Font letterFont;
								int i = 10;
								SizeF size;
								
								do{
									i++;
									letterFont = new Font(FontFamily.GenericSansSerif, i, FontStyle.Bold);
									size = g.MeasureString(letter, letterFont);
								} 
								while (size.Width < 25.0);

								Font scoreFont;
								i = 2;

								do
								{
									i++;
									scoreFont = new Font(FontFamily.GenericSansSerif, i, FontStyle.Regular);
									size = g.MeasureString(letter, scoreFont);
								} while (size.Width < 7);

								g.DrawString(letter, letterFont, Brushes.Black, new Point(-5, -5));
								g.DrawString(drawScore, scoreFont, Brushes.Black, new Point(15, 14));
							}
							bmp.Save(string.Format(@"{0}{1}.bmp", saveLetter, score.ToString()));
						}
					}
				}
			}
		}
	}
}
