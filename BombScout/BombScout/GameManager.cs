using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace BombScout
{
    public class GameManager
    {
        public int _GridSize;
        public Button[,] _ButtonArray;
        public Cell[,] _SolutionMap;
        private int _NumberOfBombs;
        public bool _Start;
        Form2 _PlayPage;

        public GameManager(int gridSize)
        {
            _PlayPage = new Form2();
            _PlayPage.StartPosition = FormStartPosition.CenterScreen;
            _PlayPage.Show();

            _GridSize = gridSize;

            _SolutionMap = new Cell[_GridSize, _GridSize];
            MakeGrid();
        }

        public void MakeGrid()
        {
            _ButtonArray = new Button[_GridSize, _GridSize];
            for (int i = 0; i < _GridSize; i++)
            {
                for (int j = 0; j < _GridSize; j++)
                {
                    _ButtonArray[i, j] = new Button();
                    DisplayButton(_ButtonArray[i, j], i, j);
                }
            }
        }

        public void MakeSolution(int row, int col)
        {
            Random rand = new Random();
            for (int i = 0; i < _GridSize; i++)
            {
                for (int j = 0; j < _GridSize; j++)
                {
                    if (rand.Next(0, 8) == 0 && (i != row && j != col)) //difficulty based on second number in rand.Next(,)
                    {
                        _NumberOfBombs++;
                        _SolutionMap[i, j] = new Cell(true, _ButtonArray[i, j], i, j);
                    }

                    else
                    {
                        _SolutionMap[i, j] = new Cell(false, _ButtonArray[i, j], i, j);
                    }
                }
            }
        }

        public void MakeMap()
        {
            for (int i = 0; i < _GridSize; i++)
            {
                for (int j = 0; j < _GridSize; j++)
                {
                    FindSurroundingBombs(i, j);

                }
            }
        }

        public void FindSurroundingBombs(int row, int col)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {

                    if (_SolutionMap[row, col]._BombTile == true)
                    {
                        _SolutionMap[row, col]._SurroundingBombs = 9;
                        return;
                    }
                    if (row + i < 0 || row + i >= _GridSize || col + j < 0 || col + j >= _GridSize)
                    {
                    }
                    else if (_SolutionMap[row + i, col + j]._BombTile == true)
                    {
                        _SolutionMap[row, col]._SurroundingBombs++;
                    }
                }
            }
        }

        public void DisplayButton(Button button, int row, int col)
        {
            button.Location = new Point(30 + row * 30, 30 + col * 30);
            button.Visible = true;
            button.Size = new Size(30, 30);
            button.Name = row + "," + col;
            button.MouseDown += new MouseEventHandler(button_MouseDown);
            _PlayPage.Controls.Add(button);
        }

        public void AddText(int numberOfBombs)
        {
            Label label = new Label();
            label.Text = String.Format("Number of Bombs: {0}", numberOfBombs);
            label.Location = new Point(10, 10);
            label.Visible = true;
            label.Size = new Size(_PlayPage.Width, 30);
            _PlayPage.Controls.Add(label);
        }

        public void button_MouseDown(object sender, MouseEventArgs e)
        {
            Button button = (Button)sender;
            string[] coordinates = button.Name.Split(',');
            int row = int.Parse(coordinates[0]);
            int col = int.Parse(coordinates[1]);
            if (e.Button == MouseButtons.Right && _SolutionMap[row, col] != null)
            {
                FlagOrUnflagButton(row, col);
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (!_Start)
                {
                    StartGame(row, col);
                }
                MakeMove(row, col);
            }
            if (_Start)
            {
                CheckProgress();
            }

        }

        public void StartGame(int row, int col)
        {
            while (_NumberOfBombs == 0)
            {
                MakeSolution(row, col);
            }
            AddText(_NumberOfBombs);
            MakeMap();
            _Start = true;
        }

        public void MakeMove(int row, int col)
        {
            _SolutionMap[row, col].DisplayTile();
            if (_SolutionMap[row, col]._SurroundingBombs == 9)
            {
                Lose();
            }
            if (_SolutionMap[row, col]._SurroundingBombs == 0)
            {
                DisplaySurrounding(row, col);
            }
        }

        public void CheckProgress()
        {
            for (int i = 0; i < _GridSize; i++)
            {
                for (int j = 0; j < _GridSize; j++)
                {
                    if (_SolutionMap[i, j]._Correct == false)
                    {
                        return;
                    }
                }
            }
            Win();
        }

        public void FlagOrUnflagButton(int row, int col)
        {
            _SolutionMap[row, col].FlagOrUnflagButton();
        }

        public void DisplaySurrounding(int row, int col)
        {
            _SolutionMap[row, col].DisplayTile();

            if (_SolutionMap[row, col]._SurroundingBombs == 0)
            {
                _SolutionMap[row, col]._SurroundingBombs = 10;
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (row + i < 0 || row + i >= _GridSize || col + j < 0 || col + j >= _GridSize)
                        {
                        }
                        else
                        {
                            DisplaySurrounding(row + i, col + j);
                        }
                    }
                }
            }
        }

        public void DisplaySolution()
        {
            for (int i = 0; i < _GridSize; i++)
            {
                for (int j = 0; j < _GridSize; j++)
                {
                    _SolutionMap[i, j].DisplayTile();
                }
            }
        }

        public void Lose()
        {
            MessageBox.Show("You Lose!");
            DisplaySolution();
        }

        public void Win()
        {
            MessageBox.Show("You Win!");
            DisplaySolution();
        }
    }
}
