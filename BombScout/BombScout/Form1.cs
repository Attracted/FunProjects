using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace BombScout
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Icon = BombScout.Properties.Resources.Bomb2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int gridSize;

            var worked = int.TryParse(textBox1.Text, out gridSize);

            if (!worked || gridSize < 1 || gridSize > 99)
            {
                return;
            }
            
            GameManager game = new GameManager(gridSize);

        }
    }
}
