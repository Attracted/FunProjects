using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WordFinder
{
	public class SearchManager
	{		
		public SearchManager()
		{
		}

		public void FindWord(string pattern)
		{
         var FD = new OpenFileDialog();
         FD.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
         FD.DefaultExt = ".txt";
         FD.InitialDirectory = @"~\";
         if (FD.ShowDialog() != DialogResult.OK)
         {
            MessageBox.Show("Invalid text file");
            return;   
         }

         var output = new StringWriter();

         using (StreamReader input = File.OpenText(FD.FileName))
			{
				int index = 1;
				while (!input.EndOfStream)
				{
					var line = input.ReadLine();
					var isMatch = MatchWildcardString(pattern, line);
					if (isMatch)
					{
						output.WriteLine(line + "[" + index + "]");
					}

               index++;
				}
			}

         MessageBox.Show(output.ToString());
		}
		public Boolean MatchWildcardString(String pattern, String input)
		{
			if (String.Compare(pattern, input) == 0)
			{
				return true;
			}
			else if (String.IsNullOrEmpty(input))
			{
				if (String.IsNullOrEmpty(pattern.Trim(new Char[1] { '*' })))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else if (pattern.Length == 0)
			{
				return false;
			}
			else if (pattern[0] == '?')
			{
				return MatchWildcardString(pattern.Substring(1), input.Substring(1));
			}
			else if (pattern[pattern.Length - 1] == '?')
			{
				return MatchWildcardString(pattern.Substring(0, pattern.Length - 1), input.Substring(0, input.Length - 1));
			}
			else if (pattern[0] == '*')
			{
				if (MatchWildcardString(pattern.Substring(1), input))
				{
					return true;
				}
				else
				{
					return MatchWildcardString(pattern, input.Substring(1));
				}
			}
			else if (pattern[pattern.Length - 1] == '*')
			{
				if (MatchWildcardString(pattern.Substring(0, pattern.Length - 1), input))
				{
					return true;
				}
				else
				{
					return MatchWildcardString(pattern, input.Substring(0, input.Length - 1));
				}
			}
			else if (pattern[0] == input[0])
			{
				return MatchWildcardString(pattern.Substring(1), input.Substring(1));
			}
			return false;
		}
	}
}
