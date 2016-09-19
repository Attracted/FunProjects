using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Clabbers
{
	public partial class TwoLetterWordsPanel : UserControl
	{
		public TwoLetterWordsPanel()
		{
			InitializeComponent();
		}

		public GridItem Grid { get; set; }
	}
}
