using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace BombScout
{
    public class Cell
    {
        public bool _BombTile;
        public bool _Flagged;
        public bool _Revealed;
        public int _SurroundingBombs;
        public Button _Button;
        public bool _Correct;
        public int _Row;
        public int _Col;

        public Cell(bool bombTile, Button button, int row, int col)
        {
            _BombTile = bombTile;
            _Button = button;
            _Row = row;
            _Col = col;
        }

        public void DisplayTile()
        {
            _Revealed = true;
            
            if (!_BombTile) 
            { 
                _Correct = true; 
            }

            UnflagButton();
            if (_BombTile == true)
            {
                _Button.Text = "X";
            }
            else if (_SurroundingBombs == 0 || _SurroundingBombs == 10)
            {
                _Button.Text = "";
            }
            else
            {
                _Button.Text = _SurroundingBombs.ToString();
            }

            ChangeColour();
        }
        public void ChangeColour()
        {
            if (_SurroundingBombs == 0)
            {
                _Button.BackColor = Color.LightGray;
            }
            if (_SurroundingBombs == 1)
            {
                _Button.BackColor = Color.LightCyan;
            }
            if (_SurroundingBombs == 2)
            {
                _Button.BackColor = Color.LightBlue;
            }
            if (_SurroundingBombs == 3)
            {
                _Button.BackColor = Color.LightGreen;
            }
            if (_SurroundingBombs == 4)
            {
                _Button.BackColor = Color.Yellow;
            }
            if (_SurroundingBombs == 5)
            {
                _Button.BackColor = Color.Orange;
            }
            if (_SurroundingBombs == 6)
            {
                _Button.BackColor = Color.Salmon;
            }
            if (_SurroundingBombs == 7)
            {
                _Button.BackColor = Color.DarkSalmon;
            }
            if (_SurroundingBombs == 8)
            {
                _Button.BackColor = Color.RosyBrown;
            }
            if (_SurroundingBombs == 9)
            {
                _Button.BackColor = Color.Red;
                _Button.ForeColor = Color.White;
            }
            if (_SurroundingBombs == 10)
            {
                _Button.BackColor = Color.LightGray;
            }
        }
        public void FlagOrUnflagButton()
        {
            if (_Flagged)
            {
                UnflagButton();
            }
            else
            {
                FlagButton();
            }
        }
        public void FlagButton(){
            if (!_Revealed)
            {
                _Flagged = true;
                _Button.ForeColor = Color.Red;
                _Button.Text = "!!";

                if (_BombTile)
                {
                    _Correct = true;
                }
            }
        }
        public void UnflagButton()
        {
            _Flagged = false;
            _Button.ForeColor = Color.Black;

            if (_Correct == true && (_SurroundingBombs > 0 && _SurroundingBombs < 9))
            {
                _Button.Text = _SurroundingBombs.ToString();
            }
            else
            {
                _Button.Text = "";
            }

            if (_BombTile)
            {
                _Correct = false;
            }
            
        }

    }
}
