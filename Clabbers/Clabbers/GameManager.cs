using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using Clabbers.Properties;

namespace Clabbers
{
   public enum Players
   {
      You = 0,
      Opp1 = 1,
      Opp2 = 2,
      Opp3 = 3,
      None = 4
   }

   public enum CellType
   {
      Empty = 0,
      DoubleLetter = 1,
      DoubleWord = 2,
      TripleLetter = 3,
      TripleWord = 4,
      StartTile = 5,
      Hand = 6
   }

   public enum Alignment
   {
      Horizontal = 0,
      Vertical = 1,
      None = 2
   }

   public enum MoveErrorType
   {
      EmptyMove = 0,
      FirstMoveNotOnStartTile = 1,
      TilesNotAligned = 2,
      TilesNotConsecutive = 3,
      InvalidWord = 4,
      FirstMoveOneLetter = 5,
      None = 6
   }

   public class GameManager : GenericForm
   {
      public GenericForm _QueryPage;
      public CellInfoForm _CellInfoPage;
      public GenericForm _MovesPage;
      public List<Label> _ScorePopups;
      public Panel _InfoTab;
      public InfoPanel[] _InfoTabPanels;
      public GridMap _GridMap;
      public List<string> _TwoLetterWords;
      public List<string> _Words;
      public string _MapString;
      public string _TilesString;
      public Button _Submit;
      public Button _SwapTiles;
      public Button _RecallTiles;
      public Button _PassTurn;
      public Button _SaveGame;
      public Button _OpenTab;
      public Players _ActivePlayer;
      public Players _StartingPlayer;
      public Player[] _Players;
      public TileBag _TileBag;
      public bool _FirstMove;
      public int _HandSize; 
      public int _NumPlayers;
      public int _Turn;
      public const int _ButtonSideLength = 30;
      public const int _PlayerInfoWidth = 120;
      public const int _PlayerInfoHeight = 165;
      public delegate void OpponentsTurnHandler(object sender, OpponentsTurnEventArgs e);
      public event OpponentsTurnHandler OnOpponentsTurn;

      public void InitializeComponent()
      {
         this.SuspendLayout();
         // 
         // GameManager
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.ClientSize = new System.Drawing.Size(116, 0);
         this.Name = "GameManager";
         this.ResumeLayout(false);

      }

      public GameManager(string map, string tiles, string words, int handSize, int numOpp)
      {
         _HandSize = handSize;
         _NumPlayers = numOpp + 1;
         MakeWords(words);
         _MapString = map;
         _TilesString = tiles;

         MakeGame(false);
      }

      public GameManager(string saveState)
      {
         RestoreGameState(saveState);

         MakeGame(true);
      }

      public void MakeWords(string words)
      {
         _Words = new List<string>();
         _Words.AddRange(words.Split('\r', '\n').Distinct().ToList());
         _Words.Remove("");
      }
      
      public void MakeGame(bool fromSave)
      {       
         this.StartPosition = FormStartPosition.CenterScreen;
         this.Padding = new Padding(30);
         this.AutoSize = true;

         _TwoLetterWords = new List<string>();
         _TwoLetterWords.AddRange(_Words.FindAll(w => w.Length == 2));
         if (_NumPlayers > 1)
         {
            this.OnOpponentsTurn += new OpponentsTurnHandler(OpponentsTurn);
         }
         
         _ScorePopups = new List<Label>();

         if (!fromSave)
         {
            MakePlayers();
            MakeGrid(_MapString);
         }

         DisplayGridMap();
         MakeSubmitButton();
         MakeRecallButton();
         MakeSwapButton();
         MakePassButton();
         MakeSaveButton();
         MakeOpenTabButton();
         MakeInfoTabPanel();
         MakeTwoLetterWordsPanel();

         MakeTiles(_TilesString);

         for (int i = 0; i < _NumPlayers; i++)
         {
            MakePlayerInfo(_Players[i]);
            MakeHand(_Players[i]);
            _InfoTabPanels[(int)_Players[i].ThisPlayer].SetHand(_Players[i].HandMapToString());
         }

         this.Show();

         Random rand = new Random();
         var startingPlayer = rand.Next(0, _NumPlayers);
         _StartingPlayer = _Players[startingPlayer].ThisPlayer;

         _ActivePlayer = _StartingPlayer;
         _Turn = 1;
         _FirstMove = true;

         switch (_StartingPlayer)
         {
            case Players.You:
               MessageBox.Show("You start!");
               break;
            case Players.Opp1:
               MessageBox.Show("Opponent 1 starts!");
               StartOpponentsTurn(_Players[1]);
               break;
            case Players.Opp2:
               MessageBox.Show("Opponent 2 starts!");
               StartOpponentsTurn(_Players[2]);
               break;
            case Players.Opp3:
               MessageBox.Show("Opponent 3 starts!");
               StartOpponentsTurn(_Players[3]);
               break;
            default:
               MessageBox.Show("Something went very wrong!");
               return;
         }
      }

      public void MakePlayers()
      {
         _Players = new Player[_NumPlayers];

         for (int i = 0; i < _NumPlayers; i++)
         {
            _Players[i] = new Player()
            {
               HandMap = new Cell[_HandSize],
               TotalScore = 0,
               ThisPlayer = (Players)i,
               MoveHistory = new List<MoveData>(),
               MovesData = new List<MoveData>(),
               Move = new List<Cell>(),
               MoveScore = 0,
               MoveError = MoveErrorType.EmptyMove
            };
         }
      }
      
      public void MakeGrid(string map)
      {			
         List<Cell> startCells = new List<Cell>();

         using (StringReader input = new StringReader(map))
         {
            int row = 0;
            int col = 0;

            var line = input.ReadLine();

            var rowCol = line.Split(',');

            _GridMap = new GridMap(int.Parse(rowCol[0]), int.Parse(rowCol[1]));

            while (input.Peek() != -1)
            {
               line = input.ReadLine();

               col = 0;

               foreach(Char type in line)
               {			
                  switch (type)
                  {
                     case ('X'):
                        _GridMap.Set(row, col, new Cell() { Row = row, Col = col, Value = 100, Used = false, Type = CellType.Empty });
                        break;
                     case ('l'):
                        _GridMap.Set(row, col, new Cell() { Row = row, Col = col, Value = 100, Used = false, Type = CellType.DoubleLetter });
                        break;
                     case ('w'):
                        _GridMap.Set(row, col, new Cell() { Row = row, Col = col, Value = 100, Used = false, Type = CellType.DoubleWord });
                        break;
                     case ('L'):
                        _GridMap.Set(row, col, new Cell() { Row = row, Col = col, Value = 100, Used = false, Type = CellType.TripleLetter });
                        break;
                     case ('W'):
                        _GridMap.Set(row, col, new Cell() { Row = row, Col = col, Value = 100, Used = false, Type = CellType.TripleWord });
                        break;
                     case ('*'):
                        _GridMap.Set(row, col, new Cell() { Row = row, Col = col, Value = 100, Used = false, Type = CellType.StartTile });
                        startCells.Add(_GridMap.Get(row, col));
                        break;
                  }
                  col++;
               }
               row++;
            }
         }

         //foreach (Cell startCell in startCells)
         //{
         //   SetCellValues(startCell);
         //}
      }

      public void DisplayGridMap()
      {
         for (int row = 0; row < _GridMap.Row; row++)
         {
            for (int col = 0; col < _GridMap.Col; col++)
            {
               DisplayCell(_GridMap.Get(row, col));
               this.Controls.Add(_GridMap.Get(row, col).Label);
            }
         }
      }
      
      public void MakeSubmitButton()
      {
         _Submit = new Button()
         {
            Location = new Point(_ButtonSideLength, (_GridMap.Row + 3) * _ButtonSideLength),
            Size = new Size(_ButtonSideLength * 2, _ButtonSideLength),
            Text = "Submit",
            Name = "Submit",
            BackColor = Color.Red,
            ForeColor = Color.White
         };

         _Submit.MouseClick += new MouseEventHandler(_Submit_MouseClick);
         this.Controls.Add(_Submit);
      }

      public void MakeRecallButton()
      {
         _RecallTiles = new Button()
         {
            Location = new Point(_ButtonSideLength * 3, (_GridMap.Row + 3) * _ButtonSideLength),
            Size = new Size(_ButtonSideLength * 2, _ButtonSideLength),
            Text = "Recall",
            Name = "Recall",
            BackColor = Color.LightBlue,
            ForeColor = Color.Black
         };

         _RecallTiles.MouseClick += new MouseEventHandler(_RecallTiles_MouseClick);
         this.Controls.Add(_RecallTiles);
      }

      public void MakeSwapButton()
      {
         _SwapTiles = new Button()
         {
            Location = new Point(_ButtonSideLength * 5, (_GridMap.Row + 3) * _ButtonSideLength),
            Size = new Size(_ButtonSideLength * 2, _ButtonSideLength),
            Text = "Swap",
            Name = "Swap",
            BackColor = Color.Green,
            ForeColor = Color.White
         };

         _SwapTiles.MouseClick += new MouseEventHandler(_SwapTiles_MouseClick);
         this.Controls.Add(_SwapTiles);
      }

      public void MakePassButton()
      {
         _PassTurn = new Button()
         {
            Location = new Point(_ButtonSideLength * 7, (_GridMap.Row + 3) * _ButtonSideLength),
            Size = new Size(_ButtonSideLength * 2, _ButtonSideLength),
            Text = "Pass",
            Name = "Pass",
            BackColor = Color.Orange,
            ForeColor = Color.Black
         };

         _PassTurn.MouseClick += new MouseEventHandler(_PassTurn_MouseClick);
         this.Controls.Add(_PassTurn);
      }

      public void MakeSaveButton()
      {
         _SaveGame = new Button()
         {
            Location = new Point(_ButtonSideLength * 9, (_GridMap.Row + 3) * _ButtonSideLength),
            Size = new Size(_ButtonSideLength * 2, _ButtonSideLength),
            Text = "Save",
            Name = "Save",
            BackColor = Color.BlanchedAlmond,
            ForeColor = Color.Black
         };

         _SaveGame.MouseClick += new MouseEventHandler(_SaveGame_MouseClick);
         this.Controls.Add(_SaveGame);
      }

      public void MakeOpenTabButton()
      {
         _OpenTab = new Button()
         {
            Location = new Point(7 + (_GridMap.Col + 1) * _ButtonSideLength, 30 + (_GridMap.Row + 1) * _ButtonSideLength / 3),
            Size = new Size(17, (_GridMap.Row + 1) * _ButtonSideLength / 3),
            Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold),
            BackColor = Color.DarkGray,
            ForeColor = Color.White,
            Text = ">",
            Name = "OpenTab"
         };
         _OpenTab.MouseClick += new MouseEventHandler(_OpenTab_MouseClick);

         this.Controls.Add(_OpenTab);
      }

      public void MakeInfoTabPanel()
      {
         _InfoTab = new Panel()
         {
            Location = new Point((_GridMap.Col + 2) * _ButtonSideLength, _ButtonSideLength),
            MinimumSize = new Size(_PlayerInfoWidth, _PlayerInfoHeight * 2),
            Padding = new Padding(10),
            BackColor = Color.DarkGray,
            Name = "Closed",
            AutoSize = true,
            MaximumSize = new Size((_PlayerInfoWidth + _ButtonSideLength) * 2, (_PlayerInfoHeight + _ButtonSideLength) * 2),
            Visible = false
         };

         _InfoTabPanels = new InfoPanel[_NumPlayers];

         this.Controls.Add(_InfoTab);
      }

      public void MakeTwoLetterWordsPanel()
      {			
         StringBuilder twoLetterWordsBuilder = new StringBuilder();
         twoLetterWordsBuilder.AppendLine("Two Letter Words: ");
         foreach (string word in _TwoLetterWords)
         {
            twoLetterWordsBuilder.Append(word[0]);
            twoLetterWordsBuilder.Append(word.ToLower()[1] + " ");
         }

         int x = 10;
         int y;
         int w;
         int h = _PlayerInfoHeight;
         switch (_NumPlayers)
         {
            case 1:
               y = _PlayerInfoHeight + _ButtonSideLength;
               w = _PlayerInfoWidth;
               break;
            case 2:
               y = _PlayerInfoHeight + _ButtonSideLength;
               w = (_PlayerInfoWidth + _ButtonSideLength) * 2;
               break;
            case 3:
               y = (_PlayerInfoHeight + _ButtonSideLength) * 2;
               w = (_PlayerInfoWidth + _ButtonSideLength) * 2;
               break;
            case 4:
               y = (_PlayerInfoHeight + _ButtonSideLength) * 2;
               w = (_PlayerInfoWidth + _ButtonSideLength) * 2;
               break;
            default:
               y = _PlayerInfoHeight + _ButtonSideLength;
               w = _PlayerInfoWidth;
               break;
         }

         TextBox textBox = new TextBox()
         {
            Location = new Point(x, y),
            Size = new Size(w, h),
            Text = twoLetterWordsBuilder.ToString(),
            BackColor = Color.Gray,
            ForeColor = Color.LightBlue,
            BorderStyle = BorderStyle.Fixed3D,
            Font = new Font(FontFamily.GenericMonospace, 10, FontStyle.Bold),
            ScrollBars = ScrollBars.Vertical,
            Multiline = true,
            ReadOnly = true,
            AutoSize = true,
            Visible = true
         };

         _InfoTab.Controls.Add(textBox);
      }

      public void MakePlayerInfo(Player player)
      {
         int x = (int)player.ThisPlayer % 2;
         int y = ((int)player.ThisPlayer < 2) ? 0 : 1;
         InfoPanel panel = new InfoPanel(_HandSize)
         {
            Location = new Point(10 + x * 120, 10 + y * 165),
            Name = player.ThisPlayer.ToString(),
            Visible = true,
         };

         panel.PlayerName = player.ThisPlayer;
         panel.MoveHistoryButton.MouseClick += new MouseEventHandler(MoveHistoryButton_MouseClick);
            
         _InfoTab.Controls.Add(panel);
         _InfoTabPanels[(int)player.ThisPlayer] = panel;

         string s = new String(' ', _HandSize);

         panel.SetHand(s);
         panel.Score = player.TotalScore;
         panel.LastMove = player.MoveHistory.LastOrDefault();
      }

      public void MakeTiles(string tiles)
      {
         _TileBag = new TileBag();
         
         using (StringReader input = new StringReader(tiles))
         {
            int score = -1;

            while (input.Peek() != -1)
            {
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
                  pairs = new string[]{line};
               }

               foreach(string pair in pairs)
               {
                  var letterCount = pair.Split('=');
                  var letter = letterCount[0];
                  var frequency = int.Parse(letterCount[1]);
                  Tile tile = new Tile() { Letter = char.Parse(letter), Score = score, IsBlankTile = (letter == "*") ? true : false };
                  string saveLetter;
                  string drawScore;

                  for (int i = 0; i < frequency; i++)
                  {
                     _TileBag.AddTile(tile);
                  }
            
                  if (letter == "*")
                  {
                     saveLetter = "_";
                     letter = " ";
                  }
                  else
                  {
                     saveLetter = letter;
                  }

                  if (score == 0)
                  {
                     drawScore = "";
                  }
                  else
                  {
                     drawScore = score.ToString();
                  }
                  // Create bitmap With large letter and small score
                  var bmp = new Bitmap(Resources.Background);
                  var blankbmp = new Bitmap(Resources.Background);
                  using (Graphics g = Graphics.FromImage(bmp))
                  {
                     Font letterFont;
                     int i = 10;
                     SizeF size;

                     do
                     {
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

                     using (Graphics blankg = Graphics.FromImage(blankbmp))
                     {
                        blankg.DrawString(letter, letterFont, Brushes.DarkRed, new Point(-5, -5));
                     }
                  }
                  _TileBag.AddImage(tile.Letter, bmp);
                  _TileBag.AddBlankImage(char.Parse(letter), blankbmp);
                  //bmp.Save(string.Format(@"C:\Clabbers\Tiles\{0}{1}.bmp", saveLetter, score.ToString()));
               }
            }
         }
      }

      public void MakeHand(Player player)
      {
         Random rand = new Random();
         for (int i = 0; i < _HandSize; i++)
         {
            if (player.HandMap[i] == null)
            {
               player.HandMap[i] = new Cell() { Row = _GridMap.Row + 1, Col = i, Value = 0, Used = false, Type = CellType.Hand, Tile = _TileBag.GetRandomTile(rand) };
               if (player.ThisPlayer == Players.You) 
               { 
                  DisplayCell(player.HandMap[i]); 
               }
            }
            else if (player.HandMap[i].Tile == null)
            {
               player.HandMap[i].Tile = _TileBag.GetRandomTile(rand);
               player.HandMap[i].Label.Image = _TileBag.GetImage(player.HandMap[i].Tile.Letter);
            }
         }

         if (player.ThisPlayer == Players.You)
         {
            _InfoTabPanels[(int)player.ThisPlayer].SetHand(player.HandMapToString());
         }
      }

      public void DisplayHand(Player player)
      {
         for (int i = 0; i < _HandSize; i++)
         {
            if (player.ThisPlayer == Players.You)
            {
               DisplayCell(player.HandMap[i]);
            }
         }
      }

      public static Alignment GetAlignment(List<Cell> move)
      {
         bool first = true;
         int row = int.MaxValue;
         bool rowAligned = true;
         int col = int.MaxValue;
         bool colAligned = true;

         if (move.Count == 1)
         {
            return Alignment.Horizontal;
         }

         foreach(Cell cell in move)
         {
            if (first)
            {
               first = false;
               row = cell.Row;
               col = cell.Col;
            }
            if (cell.Row != row)
            {
               rowAligned = false;
            }
            if (cell.Col != col)
            {
               colAligned = false;
            }
         }

         if (rowAligned)
         {
            return Alignment.Horizontal;
         }
         if (colAligned)
         {
            return Alignment.Vertical;
         }

         return Alignment.None;
      }

      public List<MoveData> GetMovesDataForWord(List<Cell> move, Alignment wordDirection)
      {
         List<MoveData> movesData = new List<MoveData>();
         Alignment otherDirection = InvertAlignment(wordDirection);
         
         MoveData startMoveData = new MoveData()
         {
            Move = new List<Cell>(),
            Score = 0,
            ValidWord = false,
            Turn = _Turn,
            Word = "",
            Alignment = wordDirection
         };

         Cell startCell = move.FirstOrDefault();

         startMoveData.Move.AddRange(GetConsecutiveMove(startCell, wordDirection));
         if (startMoveData.Move.Count >= 2)
         {
            startMoveData.SetWordToString();
            if (CheckWord(startMoveData.Word))
            {
               startMoveData.ValidWord = true;
            }
         }

         movesData.Add(startMoveData);

         foreach (Cell cell in move) 
         {
            MoveData moveData = new MoveData()
            {
               Move = new List<Cell>(),
               Score = 0,
               ValidWord = false,
               Turn = _Turn,
               Word = "",
               Alignment = otherDirection
            };

            moveData.Move.AddRange(GetConsecutiveMove(cell, otherDirection));
            if (moveData.Move.Count >= 2)
            {
               moveData.SetWordToString();
               if (CheckWord(moveData.Word))
               {
                  moveData.ValidWord = true;
               }
            }

            movesData.Add(moveData);
         }

         movesData.RemoveAll(m => m.Word == "");

         return movesData;
      }

      public List<string> GetInvalidWords(List<MoveData> movesData)
      {
         if (movesData.Count == 0)
         {
            return null;
         }

         // Create list of all invalid words
         List<string> words = new List<string>();

         foreach (MoveData moveData in movesData)
         {
            if (moveData.ValidWord == false)
            {
               words.Add(moveData.Word);
            }
         }

         return words;
      }

      public List<Cell> GetConsecutiveMove(Cell startCell, Alignment alignment)
      {
         List<Cell> move = new List<Cell>();
         int row = startCell.Row;
         int col = startCell.Col;

         if (alignment == Alignment.Horizontal)
         {
            // Search for used cells going right
            int max = col;
            while (max < _GridMap.Col && _GridMap.Get(row, max).Tile != null)
            {
               move.Add(_GridMap.Get(row, max));
               max++;
            }
            // Search for used cells going left
            int min = col - 1;
            while (min >= 0 && _GridMap.Get(row, min).Tile != null)
            {
               move.Add(_GridMap.Get(row, min));
               min--;
            }

            move.Sort((x, y) => x.Col.CompareTo(y.Col));
         }
         else if (alignment == Alignment.Vertical)
         {
            // Search for used cells going down
            int max = row;
            while (max < _GridMap.Row && _GridMap.Get(max, col).Tile != null)
            {
               move.Add(_GridMap.Get(max, col));
               max++;
            }
            // Search for used cells going up
            int min = row - 1;
            while (min >= 0 && _GridMap.Get(min, col).Tile != null)
            {
               move.Add(_GridMap.Get(min, col));
               min--;
            }

            move.Sort((x, y) => x.Row.CompareTo(y.Row));
         }

         return move;
      }

      public bool CheckWord(string word)
      {
         string wordString = word.ToUpper();
         return _Words.Contains(wordString);
      }

      public bool CheckConsecutive(List<Cell> move, Alignment alignment)
      {
         List<Cell> checkMove = new List<Cell>(move);
         
         if (alignment == Alignment.Horizontal)
         {
            checkMove.Sort((x, y) => x.Col.CompareTo(y.Col));
            int row = checkMove.First().Row;				
            int col = checkMove.First().Col;

            int checkCol = col;
            while (checkCol < _GridMap.Col && _GridMap.Get(row, checkCol).Tile != null)
            {
               if (checkMove.Count > 0 && checkMove[0].Col == checkCol)
               {
                  checkMove.RemoveAt(0);
               }
               else if (!_GridMap.Get(row, checkCol).Used)
               {
                  return false;
               }
               checkCol++;
            }
         }
         else if (alignment == Alignment.Vertical)
         {
            checkMove.Sort((x, y) => x.Row.CompareTo(y.Row));
            int row = checkMove.First().Row;				
            int col = checkMove.First().Col;

            int checkRow = row;
            while (checkRow < _GridMap.Row && _GridMap.Get(checkRow, col).Tile != null)
            {
               if (checkMove.Count == 0)
               {
                  return false;
               }

               if(checkMove[0].Row == checkRow)
               {
                  checkMove.RemoveAt(0);
               }
               else if (!_GridMap.Get(checkRow, col).Used)
               {
                  return false;
               }
               checkRow++;
            }
         }
         
         if (checkMove.Count > 0)
         {
            return false;
         }

         return true;
      }
      
      public static int CalculateScore(List<MoveData> movesData)
      {
         int score = 0;
         foreach (MoveData moveData in movesData)
         {
            int wordMult = 1;
            foreach (Cell cell in moveData.Move)
            {
               if (cell.Used)
               {
                  moveData.Score += cell.Tile.Score;
               }
               else
               {
                  switch (cell.Type)
                  {
                     case (CellType.Empty):
                        moveData.Score += cell.Tile.Score;
                        break;
                     case (CellType.DoubleLetter):
                        moveData.Score += cell.Tile.Score * 2;
                        break;
                     case (CellType.DoubleWord):
                        moveData.Score += cell.Tile.Score;
                        wordMult *= 2;
                        break;
                     case (CellType.TripleLetter):
                        moveData.Score += cell.Tile.Score * 3;
                        break;
                     case (CellType.TripleWord):
                        moveData.Score += cell.Tile.Score;
                        wordMult *= 3;
                        break;
                     case (CellType.StartTile):
                        moveData.Score += cell.Tile.Score;
                        wordMult *= 2;
                        break;
                  }
               }
            }
            moveData.Score *= wordMult;

            score += moveData.Score;
         }

         return score;
      }

      public void DisplayCell(Cell cell)
      {
         CellType type = cell.Type;
         Tile tile = cell.Tile;
         int row = cell.Row;
         int col = cell.Col;

         Label label = new Label()
         {
            Location = new Point(30 + col * _ButtonSideLength, 30 + row * _ButtonSideLength),
            Size = new Size(_ButtonSideLength, _ButtonSideLength),
            Name = row.ToString() + "," + col.ToString(),
            BorderStyle = BorderStyle.Fixed3D,
            AllowDrop = true,
            Enabled = true,
            Visible = true
         };
         label.DragDrop += new DragEventHandler(label_DragDrop);
         label.DragEnter += new DragEventHandler(label_DragEnter);
         label.MouseDown += new MouseEventHandler(label_MouseDown);
         label.MouseUp += new MouseEventHandler(label_MouseUp);

         switch (type)
         {
            case (CellType.Empty):
               label.BackColor = Color.LightGray;
               label.Tag = "";
               label.Text = "";
               break;
            case (CellType.DoubleLetter):
               label.BackColor = Color.LightBlue;
               label.Tag = "DL";
               label.Text = "DL";
               break;
            case (CellType.DoubleWord):
               label.BackColor = Color.Red;
               label.ForeColor = Color.White;
               label.Tag = "DW";
               label.Text = "DW";
               break;
            case (CellType.TripleLetter):
               label.BackColor = Color.Green;
               label.ForeColor = Color.White;
               label.Tag = "TL";
               label.Text = "TL";
               break;
            case (CellType.TripleWord):
               label.BackColor = Color.Orange;
               label.Tag = "TW";
               label.Text = "TW";
               break;
            case (CellType.StartTile):
               label.BackColor = Color.Red;
               label.ForeColor = Color.White;
               label.Tag = "*";
               label.Text = "*";
               break;
            case (CellType.Hand):
               label.BackColor = Color.BurlyWood;
               label.Tag = "";
               label.Text = "";
               break;
         }

         if (tile != null)
         {
            label.Image = _TileBag.GetImage(tile.Letter);
            if (type != CellType.Hand)
            {
               label.Name += ",disabled";
               label.Text = "";
            }
         }

         this.Controls.Add(label);

         cell.Label = label;
      }

      public void DisplayWords(string title, List<string> words)
      {
         StringBuilder s = new StringBuilder();
         s.AppendLine(title);
         foreach (string word in words)
         {
            s.AppendLine(word);
         }
         MessageBox.Show(s.ToString());
      }

      public void SetTile(Cell toCell)
      {
         this.Enabled = false;
         
         _QueryPage = new GenericForm()
         {
            StartPosition = FormStartPosition.CenterScreen,
            FormBorderStyle = FormBorderStyle.None,
            Size = new Size(0, 0),
            Padding = new Padding(15, 30, 15, 15)
         };
         
         Label text = new Label()
         {
            Location = new Point(15, 7),
            Name = "Text",
            Text = "Select a Tile:",
            Visible = true
         };
         _QueryPage.Controls.Add(text);
         
         int row = 0;
         int col = 0;

         var usedLetters = _TileBag.GetUsedLetters();
         foreach (UsedLetter usedLetter in usedLetters)
         {
            Label label = new Label()
            {
               Image = _TileBag.GetBlankImage(usedLetter.Letter),
               Location = new Point(15 + col * _ButtonSideLength, 30 + row * _ButtonSideLength),
               Size = new Size(_ButtonSideLength, _ButtonSideLength),
               Name = usedLetter.Letter.ToString(),
               BorderStyle = BorderStyle.Fixed3D,
               Tag = toCell,
               Enabled = true,
               Visible = true
            };

            label.Click += new EventHandler(label_Click);
            _QueryPage.Controls.Add(label);
            col++;
            if (col > Math.Sqrt(usedLetters.Count))
            {
               row++;
               col = 0;
            }
         }
         _QueryPage.Show();
      }

      public void SwapTiles(Cell cell1, Cell cell2)
      {
         var tempTile = cell2.Tile;
         cell2.Tile = cell1.Tile;
         cell1.Tile = tempTile;

         var tempImage = cell2.Label.Image;
         cell2.Label.Image = cell1.Label.Image;
         cell1.Label.Image = tempImage;
      }

      public bool SwapTilesWithBag(Player player, List<int> cells)
      {
         List<Tile> tiles = new List<Tile>();
         foreach (int i in cells)
         {
            if (player.HandMap[i].Tile == null)
            {
               return false;
            }
         }
         foreach (int i in cells)
         {
            Tile tile = player.HandMap[i].Tile;
            Image image = null;
            if (player.HandMap[i].Tile.IsBlankTile)
            {
               tile.Letter = '*';
               image = _TileBag.GetBlankImage('*');
            }

            tiles.Add(tile);
            player.HandMap[i].Tile = null;
            player.HandMap[i].Label.Image = image;
         }
         MakeHand(player);
         foreach (Tile tile in tiles)
         {
            _TileBag.AddTile(tile);
         }
         _InfoTabPanels[(int)player.ThisPlayer].SetHand(player.HandMapToString());
         return true;
      }

      public void OpenSwapTilePopup()
      {
         this.Enabled = false;
         
         _QueryPage = new GenericForm()
         {
            StartPosition = FormStartPosition.CenterScreen,
            FormBorderStyle = FormBorderStyle.None,
            Size = new Size(0, 0),
            Padding = new Padding(15, 30, 15, 15)
         };

         Label text = new Label()
         {
            Location = new Point(15, 7),
            AutoSize = true,
            Name = "Text",
            Text = "Select Tiles to Swap:",
            Visible = true
         };
         _QueryPage.Controls.Add(text);

         int row = 0;
         int col = 0;
         for (int i = 0; i < _HandSize; i++)
         {
            Cell cell = _Players[(int)Players.You].HandMap[i];
            
            Label label = new Label()
            {
               Image = (cell.Label.Image == null) ? null : new Bitmap(cell.Label.Image),
               Location = new Point(15 + col * _ButtonSideLength, 30 + row * _ButtonSideLength),
               Size = new Size(_ButtonSideLength, _ButtonSideLength),
               Name = "",
               Tag = i,
               BackColor = Color.SaddleBrown,
               BorderStyle = BorderStyle.Fixed3D,
               Enabled = true,
               Visible = true
            };

            label.Click += new EventHandler(labelSwap_Click);
            _QueryPage.Controls.Add(label);
            col++;
            if (col > Math.Sqrt(_TileBag.GetUsedLetters().Count))
            {
               row++;
               col = 0;
            }
         }

         Button button = new Button()
         {
            Location = new Point(15, 30 + (row + 1) * _ButtonSideLength),
            Size = new Size(2 * _ButtonSideLength, _ButtonSideLength),
            Text = "Confirm",
            Enabled = true,
            Visible = true
         };
         button.Click += new EventHandler(buttonSwap_Click);
         _QueryPage.Controls.Add(button);

         Button button1 = new Button()
         {
            Location = new Point(15 + 2 * _ButtonSideLength, 30 + (row + 1) * _ButtonSideLength),
            Size = new Size(2 * _ButtonSideLength, _ButtonSideLength),
            Text = "Cancel",
            Enabled = true,
            Visible = true
         };
         button1.Click += new EventHandler(buttonCancel_Click);
         _QueryPage.Controls.Add(button1);

         _QueryPage.Show();
      }

      public static Alignment InvertAlignment(Alignment alignment)
      {
         switch (alignment)
         {
            case (Alignment.Horizontal):
               return Alignment.Vertical;
            case (Alignment.Vertical):
               return Alignment.Horizontal;
            default:
               return Alignment.None;
         }
      }

      public void UpdateYourCurrentMove()
      {
         Player you = _Players[(int)Players.You];
         you.MovesData.Clear();
         you.MoveScore = 0;
         foreach (Label label in _ScorePopups)
         {
            this.Controls.Remove(label);
         }
         _ScorePopups.Clear();

         if (you.Move.Count == 0)
         {
            you.MoveError = MoveErrorType.EmptyMove;
            return;
         }

         if (_FirstMove == true && !you.Move.Any(cell => cell.Type == CellType.StartTile))
         {
            you.MoveError = MoveErrorType.FirstMoveNotOnStartTile;
            return;
         }

         Alignment wordAlignment = GetAlignment(you.Move);

         if (wordAlignment == Alignment.None)
         {
            you.MoveError = MoveErrorType.TilesNotAligned;
            return;
         }

         if (!CheckConsecutive(you.Move, wordAlignment))
         {
            you.MoveError = MoveErrorType.TilesNotConsecutive;
            return;
         }

         var movesData = GetMovesDataForWord(you.Move, wordAlignment);
         if (movesData.Count == 0)
         {
            you.MoveError = MoveErrorType.FirstMoveOneLetter;
            return;
         }

         you.MovesData.AddRange(movesData);
         you.MoveError = MoveErrorType.None;

         var score = CalculateScore(you.MovesData);
         you.MoveScore = score;

         var lastTile = you.MovesData.First().Move.Last();
         var scoreLabel = new Label()
         {
            Location = new Point(30 + _ButtonSideLength - 5 + lastTile.Col * _ButtonSideLength, 30 + lastTile.Row * _ButtonSideLength),
            Size = new Size(15, 15),
            Name = score.ToString(),
            TextAlign = ContentAlignment.MiddleCenter,
            AutoSize = true,
            Font = new Font(FontFamily.GenericMonospace, 8, FontStyle.Bold),
            BackColor = Color.Red,
            ForeColor = Color.White,
            Text = score.ToString(),
            Visible = true
         };
         _ScorePopups.Add(scoreLabel);
         this.Controls.Add(scoreLabel);
         scoreLabel.BringToFront();
         
         // Displays all individual moves
         //foreach (MoveData moveData in you.MovesData)
         //{
         //   var lastTile = moveData.Move.LastOrDefault();
         //   var label = new Label()
         //   {
         //      Location = new Point(60 + lastTile.Col * 30, 30 + lastTile.Row * 30),
         //      Size = new Size(15, 15),
         //      Name = moveData.Score.ToString(),
         //      TextAlign = ContentAlignment.MiddleCenter,
         //      AutoSize = true,
         //      Font = new Font(FontFamily.GenericMonospace, 8, FontStyle.Bold),
         //      BackColor = Color.Red,
         //      ForeColor = Color.White,
         //      Text = moveData.Score.ToString(),
         //      Visible = true
         //   };
         //   _ScorePopups.Add(label);
         //   _PlayPage.Controls.Add(label);
         //   label.BringToFront();
         //}
      }

      public void ResolveMove(Player player)
      {
         _FirstMove = false;

         player.TotalScore += player.MoveScore;
         player.MoveHistory.AddRange(player.MovesData);

         foreach (Cell cell in player.Move)
         {
            cell.Used = true;
            cell.Label.Name += ",disabled";
         }

         //SetAlignedCellValues(player.MovesData.First());

         MessageBox.Show("Total score is: " + player.MoveScore);

         _InfoTabPanels[(int)player.ThisPlayer].Score = player.TotalScore;
         _InfoTabPanels[(int)player.ThisPlayer].LastMove = player.MovesData.LastOrDefault();
         player.Move.Clear();
         player.MovesData.Clear();
         player.MoveScore = 0;
         player.MoveError = MoveErrorType.EmptyMove;

         //_GridMap.CopyGridMapToWordMap();

         MakeHand(player);

         PassTurn();
      }

      public void StartOpponentsTurn(Player player)
      {
         if (OnOpponentsTurn == null) return;

         OpponentsTurnEventArgs args = new OpponentsTurnEventArgs(player);
         OnOpponentsTurn(this, args);
      }

      public void PassTurn()
      {
         if ((int)_ActivePlayer == _Players.Length - 1)
         {
            _ActivePlayer = 0;
         }
         else
         {
            _ActivePlayer++;
         }

         if (_ActivePlayer == _StartingPlayer)
         {
            _Turn++;
         }

         switch (_ActivePlayer)
         {
            case Players.You:
               MessageBox.Show("Your turn!");
               this.Enabled = true;
               return;
            case Players.Opp1:
               MessageBox.Show("Opponent 1 turn!");
               this.Enabled = false;
               StartOpponentsTurn(_Players[1]);
               return;
            case Players.Opp2:
               MessageBox.Show("Opponent 2 turn!");
               this.Enabled = false;
               StartOpponentsTurn(_Players[2]);
               return;
            case Players.Opp3:
               MessageBox.Show("Opponent 3 turn!");
               this.Enabled = false;
               StartOpponentsTurn(_Players[3]);
               return;
            default:
               MessageBox.Show("Something went very wrong!");
               return;
         }
      }

      public bool ConfirmSelection()
      {
         DialogResult dialogResult = MessageBox.Show("Are you sure?", "Confirm Selection", MessageBoxButtons.YesNo);
         if (dialogResult == DialogResult.Yes)
         {
            return true;
         }
         else if (dialogResult == DialogResult.No)
         {
            return false;
         }
         return false;
      }

      public string ListToString(List<object> list)
      {
         StringBuilder sb = new StringBuilder();
         foreach (string member in list)
         {
            sb.AppendLine(member);
         }
         return sb.ToString();
      }

      #region AI Methods
      public void MakeAIMove(Player player)
      {
         // Check for highest value columns / rows
         // Match up highest valued hand tiles with highest valued tiles
         // Try to make those tiles fit there
         // Think about doing setup work to check for these values easier / 
         //    already have them set up when you take the turn
            // Have a list of all the highest valued tiles?
            // Have a list of all the reachable tile locations?

         // Usually the best move is a series of 2 letter words combined with one larger word holding them together
         // Try to put high valued pieces on Double/Triple letter/word and then build word toward other tiles?


         // Cant just find words from hand tiles. Have to place tiles on the board and find all words made
         // Make lightweight gridmap with just letters for faster checking? Performance differences?


         // if(_FirstMove){
         //    MakeFirstMove(_ActivePlayer);
         // }

      }

      public Dictionary<char, int> GetLetterFrequencies(string letters)
      {
         Dictionary<char, int> letterDictionary = new Dictionary<char, int>();
         
         for (int i = 0; i < letters.Length; i++)
         {
            var letter = letters[i];
            if (!letterDictionary.ContainsKey(letter))
            {
               letterDictionary.Add(letter, 1);
            }
            else
            {
               letterDictionary[letter]++;
            }
         }
         
         return letterDictionary;
      }

      public bool CheckPerfectAnagram(Dictionary<char, int> letterDictionary, string checkWord, int lettersLength)
      {
         if (checkWord.Length == lettersLength)
         {
            char letter;
            for (int i = 0; i < checkWord.Length; i++)
            {
               letter = checkWord[i];
               if ((!letterDictionary.ContainsKey(letter) && letter != '*') || checkWord.Count(e => e == letter) != letterDictionary[letter])
               {
                  return false;
               }
            }

            return true;
         }

         return false;
      }

      public bool CheckAnagram(Dictionary<char, int> letterDictionary, string checkWord, int lettersLength)
      {
         if (checkWord.Length <= lettersLength)
         {
            char letter;
            for (int i = 0; i < checkWord.Length; i++)
            {
               letter = checkWord[i];
               if ((!letterDictionary.ContainsKey(letter) && letter != '*') || checkWord.Count(e => e == letter) > letterDictionary[letter])
               {
                  return false;
               }
            }

            return true;
         }

         return false;
      }

      public List<string> FindPerfectAnagrams(string letters)
      {
         Dictionary<char, int> letterDictionary = GetLetterFrequencies(letters);

         List<string> foundAnagrams = new List<string>();
         foreach (string word in _Words)
         {
            if (CheckPerfectAnagram(letterDictionary, word, letters.Length))
            {
               foundAnagrams.Add(word);
            }
         }
         return foundAnagrams;
      }

      public List<string> FindAnagrams(string letters)
      {
         Dictionary<char, int> letterDictionary = GetLetterFrequencies(letters);

         List<string> foundAnagrams = new List<string>();
         foreach (string word in _Words)
         {
            if (CheckAnagram(letterDictionary, word, letters.Length))
            {
               foundAnagrams.Add(word);
            }
         }
         return foundAnagrams;
      }

      public List<string> FindWordsThatContainString(string letters)
      {
         string letterString = letters.ToUpper();
         _Words.FindAll(w => w.Contains(letterString)).ToList();
         return _Words.FindAll(w => w.Contains(letterString)).ToList();
      }

      public void PlayWord(Player player, string word)
      {
         Cell[] wordCells = HandStringToOrderedCells(player, word);

      }

      public Cell[] HandStringToOrderedCells(Player player, string word)
      {
         var wordCells = new Cell[word.Length];
         var playerCells = player.HandMap;

         for( int i = 0; i < word.Length; i++)
         {
            char letter = word[i];

            Cell foundCell = playerCells.FirstOrDefault(e => e.Tile.Letter == letter);

            //if (foundCell == null)
            //{
            //   foundCell = playerCells.FirstOrDefault(e => e.Tile.IsBlankTile == true);
            //}

            if (foundCell == null)
            {
               continue;
            }

            wordCells[i] = foundCell;
         }
         return wordCells;
      }
      
      public void SetCellValues(Cell startCell)
      {
         int startRow = startCell.Row;
         int startCol = startCell.Col;

         for (int i = -_HandSize + 1; i < _HandSize; i++)
         {
            int row = startRow + i;
            int col = startCol;

            if (row >= 0 && row < _GridMap.Row)
            {
               SetCellValue(_GridMap.Get(row, col), Alignment.Vertical);
            }
         }

         for (int j = -_HandSize + 1; j < _HandSize; j++)
         {
            int row = startRow;
            int col = startCol + j;

            if (col >= 0 && col < _GridMap.Col)
            {
               SetCellValue(_GridMap.Get(row, col), Alignment.Horizontal);
            }
         }
      }

      public void SetAlignedCellValues(MoveData moveData)
      {
         Alignment alignment = moveData.Alignment;

         if (alignment == Alignment.Horizontal)
         {
            int row = moveData.Move.First().Row;

            int startCol = moveData.Move.First().Col - _HandSize;
            int endCol = moveData.Move.Last().Col + _HandSize + 1;

            // Set cells in row of move
            for (int i = startCol; i < endCol; i++)
            {
               if (i >= 0 && i < _GridMap.Col)
               {
                  SetCellValue(_GridMap.Get(row, i), Alignment.Horizontal);
               }
            }

            int startRow = row - _HandSize;
            int endRow = row + _HandSize + 1;

            // Set cells in same col of cells in move
            foreach (Cell cell in moveData.Move)
            {
               int col = cell.Col;

               for (int i = startRow; i < endRow; i++)
               {
                  if (i >= 0 && i < _GridMap.Row && i != row)
                  {
                     SetCellValue(_GridMap.Get(i, col), Alignment.Vertical);
                  }
               }
            }
         }
         else if (alignment == Alignment.Vertical)
         {
            int col = moveData.Move.First().Col;

            int startRow = moveData.Move.First().Row - _HandSize;
            int endRow = moveData.Move.Last().Row + _HandSize + 1;

            // Set cells in col of move
            for (int i = startRow; i < endRow; i++)
            {
               if (i >= 0 && i < _GridMap.Row)
               {
                  SetCellValue(_GridMap.Get(i, col), Alignment.Vertical);
               }
            }

            // Set cells in same row of cells in move
            foreach (Cell cell in moveData.Move)
            {
               int row = cell.Row;

               int startCol = col - _HandSize;
               int endCol = col + _HandSize + 1;

               for (int i = startCol; i < endCol; i++)
               {
                  if (i >= 0 && i < _GridMap.Col && i != col)
                  {
                     SetCellValue(_GridMap.Get(row, i), Alignment.Horizontal);
                  }
               }
            }
         }
      }

      public void SetCellValue(Cell middleCell, Alignment alignment)
      {
         int row = middleCell.Row;
         int col = middleCell.Col;

         int cellBonus = GetCellTypeBonus(middleCell);

         // XXX
         // XOX
         // XXX
         middleCell.Value = cellBonus;

         if (_GridMap.Get(row, col).Used)
         {
            return;
         }

         if (alignment == Alignment.Horizontal || alignment == Alignment.None)
         {
            // XXX
            // OXX
            // XXX
            if (col > 0)
            {
               middleCell.Value += GetCellTypeBonus(_GridMap.Get(row, col - 1)) / 2;
            }

            // XXX
            // XXO
            // XXX
            if (col < _GridMap.Col - 1)
            {
               middleCell.Value += GetCellTypeBonus(_GridMap.Get(row, col + 1)) / 2;
            }
         }

         if (alignment == Alignment.Vertical || alignment == Alignment.None)
         {
            // XOX
            // XXX
            // XXX
            if (row > 0)
            {
               middleCell.Value += GetCellTypeBonus(_GridMap.Get(row - 1, col)) / 2;
            }

            // XXX
            // XXX
            // XOX
            if (row < _GridMap.Row - 1)
            {
               middleCell.Value += GetCellTypeBonus(_GridMap.Get(row + 1, col)) / 2;
            }
         }
      }

      public int GetCellTypeBonus(Cell cell)
      {
         if (cell.Used)
         {
            return cell.Tile.Score;
         }

         switch (cell.Type)
         {
            case (CellType.Empty):
               return 1;
            case (CellType.DoubleLetter):
               return 3;
            case (CellType.DoubleWord):
               return 5;
            case (CellType.TripleLetter):
               return 5;
            case (CellType.TripleWord):
               return 10;
            case (CellType.StartTile):
               return 5;
            default:
               return 1;
         }
      }
      #endregion

      #region Save Game Methods
      public string SaveGameState(params string[] objectsToSave)
      {
         string saveGameState;

         using (StringWriter output = new StringWriter())
         {
            foreach (string objectToSave in objectsToSave)
            {
               switch (objectToSave)
               {
                  case "words":
                     output.WriteLine("~WORDS");
                     foreach (string word in _Words)
                     {
                        output.WriteLine(word);
                     }
                     output.WriteLine("~ENDWORDS");
                     break;
                  case "tiles":
                     output.WriteLine("~TILES");
                     output.WriteLine(_TileBag.TilesToString());
                     output.WriteLine("~ENDTILES");
                     break;
                  case "map":
                     output.WriteLine("~MAP");
                     output.WriteLine(_GridMap.MapToString());
                     output.WriteLine("~ENDMAP");
                     break;
                  case "savemap":
                     output.WriteLine("~SAVEMAP");
                     output.WriteLine(_GridMap.UsedToString());
                     output.WriteLine("~ENDSAVEMAP");
                     break;
                  case "handsize":
                     output.WriteLine("~HANDSIZE");
                     output.WriteLine(_HandSize.ToString());
                     output.WriteLine("~ENDHANDSIZE");
                     break;
                  case "players":
                     output.WriteLine("~PLAYERS");
                     output.WriteLine(_Players.Length + "," + _StartingPlayer.ToString());
                     foreach (Player player in _Players)
                     {
                        output.WriteLine("~PLAYER");
                        if (player.ThisPlayer == _ActivePlayer)
                        {
                           output.WriteLine("Y," + _Turn);
                        }
                        else
                        {
                           output.WriteLine("N");
                        }
                        output.WriteLine(player.HandMapToStringForSave());
                        var moveHistory = player.MoveHistoryToStringForSave();
                        if (moveHistory != string.Empty)
                        {
                           output.WriteLine(moveHistory);
                        }
                        output.WriteLine("~ENDPLAYER");
                     }
                     output.WriteLine("~ENDPLAYERS");
                     break;
                  default:
                     break;
               }
            }

            saveGameState = output.ToString();
         }

         return saveGameState;
      }

      public string SaveGameState()
      {
         return SaveGameState("words", "tiles", "map", "savemap", "handsize", "players");
      }

      public void MakeGameFromSave()
      {
         this.StartPosition = FormStartPosition.CenterScreen;
         this.Padding = new Padding(30);
         this.AutoSize = true;

         _ScorePopups = new List<Label>();

         DisplayGridMap();
         MakeSubmitButton();
         MakeRecallButton();
         MakeSwapButton();
         MakePassButton();
         MakeSaveButton();
         MakeOpenTabButton();
         MakeInfoTabPanel();
         MakeTwoLetterWordsPanel();

         for (int i = 0; i < _NumPlayers; i++)
         {
            MakePlayerInfo(_Players[i]);
            DisplayHand(_Players[i]);
            _InfoTabPanels[(int)_Players[i].ThisPlayer].SetHand(_Players[i].HandMapToString());
         }

         this.Show();

         switch (_StartingPlayer)
         {
            case Players.You:
               MessageBox.Show("You start!");
               break;
            case Players.Opp1:
               MessageBox.Show("Opponent 1 starts!");
               StartOpponentsTurn(_Players[1]);
               break;
            case Players.Opp2:
               MessageBox.Show("Opponent 2 starts!");
               StartOpponentsTurn(_Players[2]);
               break;
            case Players.Opp3:
               MessageBox.Show("Opponent 3 starts!");
               StartOpponentsTurn(_Players[3]);
               break;
            default:
               MessageBox.Show("Something went very wrong!");
               return;
         }
      }

      public void MakeGridFromSave(string save)
      {
         using (StringReader input = new StringReader(save))
         {
            for (int row = 0; row < _GridMap.Row; row++)
            {
               var line = input.ReadLine();
               for (int col = 0; col < _GridMap.Col; col++)
               {
                  var letter = line[col];

                  if (letter != '.')
                  {
                     if (char.IsLower(letter))
                     {
                        _TileBag.GetTile('*');
                        _GridMap.Get(row, col).Tile = new Tile() { Letter = char.ToLower(letter), Score = 0, IsBlankTile = true };
                     }
                     else
                     {
                        _GridMap.Get(row, col).Tile = _TileBag.GetTile(letter);
                     }

                     _GridMap.Get(row, col).Used = true;
                     _FirstMove = false;
                  }
               }
            }
         }
      }

      public void MakePlayersFromSave(string save)
      {
         using (StringReader input = new StringReader(save))
         {
            string line = input.ReadLine();
            var playerInfo = line.Split(',');
            _NumPlayers = int.Parse(playerInfo[0]);
            _StartingPlayer = (Players)Enum.Parse(typeof(Players), playerInfo[1]);

            _Players = new Player[_NumPlayers];
            for (int i = 0; i < _NumPlayers; i++)
            {
               _Players[i] = new Player()
               {
                  HandMap = new Cell[_HandSize],
                  TotalScore = 0,
                  ThisPlayer = (Players)i,
                  MoveHistory = new List<MoveData>(),
                  MovesData = new List<MoveData>(),
                  Move = new List<Cell>(),
                  MoveScore = 0,
                  MoveError = MoveErrorType.EmptyMove
               };

               line = input.ReadLine();
               if (line != "~PLAYER")
               {
                  MessageBox.Show("Inproper formatting of ~PLAYERS");
                  this.Close();
                  return;
               }

               line = input.ReadLine();
               var turnData = line.Split(',');
               if (turnData[0] == "Y")
               {
                  _ActivePlayer = _Players[i].ThisPlayer;
                  _Turn = int.Parse(turnData[1]);
               }

               line = input.ReadLine();
               for (int j = 0; j < _HandSize; j++)
               {
                  _Players[i].HandMap[j] = new Cell() { Col = j, Row = _GridMap.Row + 1, Tile = _TileBag.GetTile(line[j]), Label = null, Type = CellType.Hand, Used = false };
               }

               line = input.ReadLine();
               while (line != "~ENDPLAYER")
               {
                  var moveData = line.Split(',');
                  var score = int.Parse(moveData[2]);

                  _Players[i].MoveHistory.Add(new MoveData()
                  {
                     Turn = int.Parse(moveData[0]),
                     Word = moveData[1],
                     Score = score
                  });

                  _Players[i].TotalScore += score;

                  line = input.ReadLine();
               }
            }
         }
      }

      public void RestoreGameState(string save)
      {
         using (StringReader input = new StringReader(save))
         {
            while (input.Peek() != -1)
            {
               string line = input.ReadLine();

               switch (line)
               {
                  case "~WORDS":
                     _Words = new List<string>();
                     line = input.ReadLine();
                     while (line != "~ENDWORDS")
                     {
                        _Words.Add(line.ToUpper());
                        line = input.ReadLine();
                     }
                     break;
                  case "~TILES":
                     StringBuilder tiles = new StringBuilder();
                     line = input.ReadLine();
                     while (line != "~ENDTILES")
                     {
                        tiles.AppendLine(line);
                        line = input.ReadLine();
                     }
                     MakeTiles(tiles.ToString());
                     break;
                  case "~MAP":
                     StringBuilder map = new StringBuilder();
                     line = input.ReadLine();
                     while (line != "~ENDMAP")
                     {
                        map.AppendLine(line);
                        line = input.ReadLine();
                     }
                     MakeGrid(map.ToString());
                     break;
                  case "~SAVEMAP":
                     StringBuilder saveMap = new StringBuilder();

                     line = input.ReadLine();
                     while (line != "~ENDSAVEMAP")
                     {
                        saveMap.AppendLine(line);
                        line = input.ReadLine();
                     }
                     MakeGridFromSave(saveMap.ToString());
                     break;
                  case "~HANDSIZE":
                     line = input.ReadLine();
                     while (line != "~ENDHANDSIZE")
                     {
                        _HandSize = int.Parse(line);
                        line = input.ReadLine();
                     }
                     break;
                  case "~PLAYERS":
                     StringBuilder handMap = new StringBuilder();
                     line = input.ReadLine();
                     while (line != "~ENDPLAYERS")
                     {
                        handMap.AppendLine(line);
                        line = input.ReadLine();
                     }
                     MakePlayersFromSave(handMap.ToString());
                     break;
                  default:
                     MessageBox.Show("Persisted variable " + line + " not recognized");
                     return;
               }
            }
         }
      }
      #endregion

      #region Events
      public void label_DragDrop(object sender, DragEventArgs e)
      {
         Label toLabel = (Label)sender;
         Label fromLabel = (Label)e.Data.GetData(typeof(Label));
         toLabel.Text = "";

         if(toLabel.Name.Contains("disabled") || fromLabel.Name.Contains("disabled"))
         {
            return;
         }

         var fromRowCol = fromLabel.Name.Split(',');
         int fromRow = int.Parse(fromRowCol[0]);
         int fromCol = int.Parse(fromRowCol[1]);
         Cell fromCell = (fromRow < _GridMap.Row + 1) ? _GridMap.Get(fromRow, fromCol) : _Players[(int)Players.You].HandMap[fromCol];

         var toRowCol = toLabel.Name.Split(',');
         int toRow = int.Parse(toRowCol[0]);
         int toCol = int.Parse(toRowCol[1]);
         Cell toCell = (toRow < _GridMap.Row + 1) ? _GridMap.Get(toRow, toCol) : _Players[(int)Players.You].HandMap[toCol];

         if (toLabel.Name == fromLabel.Name && toCell.Tile.IsBlankTile)
         {
            SetTile(toCell);
            return;
         }

         if (toLabel.Image != null)
         {
            fromLabel.Text = "";
         }
         else
         {
            if (toRow < _GridMap.Row + 1)
            {
               _Players[(int)Players.You].Move.Add(toCell);
            }
            if (fromRow < _GridMap.Row + 1)
            {
               _Players[(int)Players.You].Move.Remove(fromCell);
            }
         }

         SwapTiles(fromCell, toCell);

         UpdateYourCurrentMove();
      }

      public void label_DragEnter(object sender, DragEventArgs e)
      {
         e.Effect = DragDropEffects.Move;
      }

      public void label_MouseDown(object sender, MouseEventArgs e)
      {
         Label label = (Label)sender;
         if (e.Button == MouseButtons.Right)
         {
            int x = Cursor.Position.X - e.X;
            int y = Cursor.Position.Y - e.Y;

            var rowCol = label.Name.Split(',');
            int row = int.Parse(rowCol[0]);
            int col = int.Parse(rowCol[1]);
            Cell cell = (row < _GridMap.Row + 1) ? _GridMap.Get(row, col) : _Players[(int)Players.You].HandMap[col];

            _CellInfoPage = new CellInfoForm(cell, x, y);
            _CellInfoPage.Show();

         }
         else if (e.Button == MouseButtons.Left)
         {
            if (label.Image == null || label.Name.Contains("disabled"))
            {
               return;
            }

            //var dataObj = new DataObject(label);
            //dataObj.SetData("Label", label);
            label.Text = label.Tag.ToString();
            label.DoDragDrop(label, DragDropEffects.Move);
         }
      }
      
      public void label_MouseUp(object sender, MouseEventArgs e)
      {
         if (e.Button == MouseButtons.Right && _CellInfoPage != null)
         {
            _CellInfoPage.Close();
         }
      }

      public void label_Click(object sender, EventArgs e)
      {
         Label label = (Label)sender;
         Cell cell = (Cell)label.Tag;
         cell.Label.Image = label.Image;
         cell.Tile.Letter = char.Parse(label.Name);
         
         _QueryPage.Close();
         this.Enabled = true;
         this.BringToFront();
      }

      public void labelSwap_Click(object sender, EventArgs e)
      {
         Label label = (Label)sender;
         if (label.Name != "Chosen")
         {
            label.Name = "Chosen";
            label.BackColor = Color.Yellow;
         }
         else
         {
            label.Name = "";
            label.BackColor = Color.SaddleBrown;
         }
      }
      
      public void buttonSwap_Click(object sender, EventArgs e)
      {
         var labels = _QueryPage.Controls.Find("Chosen", true);
         List<int> handIndexesToSwap = new List<int>();
         foreach (Control control in labels)
         {
            handIndexesToSwap.Add((int)control.Tag);
         }
         if (SwapTilesWithBag(_Players[(int)Players.You], handIndexesToSwap))
         {
            PassTurn();
         }
         _QueryPage.Close();
         this.Enabled = true;
         this.BringToFront();
      }

      public void buttonCancel_Click(object sender, EventArgs e)
      {
         _QueryPage.Close();
         this.Enabled = true;
         this.BringToFront();
      }

      public void _Submit_MouseClick(object sender, MouseEventArgs e)
      {
         if (!ConfirmSelection())
         {
            return;
         }
         
         switch (_Players[(int)Players.You].MoveError)
         {
            case (MoveErrorType.EmptyMove):
               MessageBox.Show("Error: Empty move!");
               return;
            case (MoveErrorType.FirstMoveNotOnStartTile):
               MessageBox.Show("Error: First move not on start tile!");
               return;
            case(MoveErrorType.TilesNotAligned):
               MessageBox.Show("Error: Tiles not aligned!");
               return;
            case(MoveErrorType.TilesNotConsecutive):
               MessageBox.Show("Error: Tiles not consecutive!");
               return;
            case(MoveErrorType.FirstMoveOneLetter):
               MessageBox.Show("Error: First move one letter!");
               return;
         }
         
         var invalidWords = GetInvalidWords(_Players[(int)Players.You].MovesData);

         if (invalidWords == null)
         {
            MessageBox.Show("Error: Move not valid!");
            return;
         }
         
         if (invalidWords.Count > 0)
         {
            DisplayWords("Invalid words: ", invalidWords);
            return;
         }

         ResolveMove(_Players[(int)Players.You]);
      }

      public void _SwapTiles_MouseClick(object sender, MouseEventArgs e)
      {
         OpenSwapTilePopup();
      }

      public void _RecallTiles_MouseClick(object sender, MouseEventArgs e)
      {
         Player you = _Players[(int)Players.You];
         for (int i = 0; i < _HandSize; i++)
         {
            if (you.HandMap[i].Tile == null && _TileBag.Count() > 0)
            {
               Cell cell = you.Move.First();

               SwapTiles(cell, you.HandMap[i]);

               you.Move.Remove(cell);
            }
         }
      }

      public void _PassTurn_MouseClick(object sender, MouseEventArgs e)
      {
         if (ConfirmSelection())
         {
            PassTurn();
         }
      }

      public void _SaveGame_MouseClick(object sender, MouseEventArgs e)
      {
         string save = SaveGameState();

         var FD = new SaveFileDialog();
         FD.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
         FD.DefaultExt = ".txt";
         FD.InitialDirectory = @"~\";
         if (FD.ShowDialog() == DialogResult.OK)
         {
            string fileToOpen = FD.FileName;
            File.WriteAllText(fileToOpen, save);
            MessageBox.Show("Save Complete");
         }
      }

      public void _OpenTab_MouseClick(object sender, MouseEventArgs e)
      {
         if (_InfoTab.Name == "Closed")
         {
            _InfoTab.Size = new Size(100, _GridMap.Row * _ButtonSideLength);
            _InfoTab.Visible = true;
            _OpenTab.Text = "<";
            _InfoTab.Name = "Open";
         }
         else if (_InfoTab.Name == "Open")
         {
            _InfoTab.Size = new Size(1, _GridMap.Row * _ButtonSideLength);
            _InfoTab.Visible = false;
            _OpenTab.Text = ">";
            _InfoTab.Name = "Closed";
         }
      }

      public void MoveHistoryButton_MouseClick(object sender, MouseEventArgs e)
      {
         Button button = (Button)sender;
         InfoPanel infoPanel = (InfoPanel)button.Tag;
         MessageBox.Show(_Players[(int)infoPanel.PlayerName].MoveHistoryToString(), infoPanel.PlayerName.ToString() + " Move History");
      }

      public void OK_Click(object sender, EventArgs e)
      {
         Button button = (Button)sender;
         Form form = (Form)button.Tag;
         form.DialogResult = DialogResult.OK;
         form.Close();
      }

      public void Cancel_Click(object sender, EventArgs e)
      {
         Button button = (Button)sender;
         Form form = (Form)button.Tag;
         form.DialogResult = DialogResult.Cancel;
         form.Close();
      }

      public void OpponentsTurn(object sender, OpponentsTurnEventArgs e)
      {
         MessageBox.Show("Player " + e.Player.ThisPlayer.ToString());

         _MovesPage = new GenericForm()
         {
            StartPosition = FormStartPosition.CenterScreen,
            Size = new Size(0, 0),
            Padding = new Padding(15, 30, 15, 15),
            AutoSize = true
         };

         var listBox = new ListBox()
         {
            Size = new Size(300, 300),
            Location = new Point(10, 10),
            Tag = e.Player
         };
         listBox.MultiColumn = true;
         listBox.BeginUpdate();

         var anagramList = FindAnagrams(e.Player.HandMapToString());
         foreach(string anagram in anagramList)
         {
            listBox.Items.Add(anagram);
         }
         listBox.EndUpdate();
         listBox.MouseDoubleClick += new MouseEventHandler(listBox_MouseDoubleClick);

         var buttonCancel = new Button()
         {
            Size = new Size(_ButtonSideLength * 2, _ButtonSideLength),
            Location = new Point(10 + _ButtonSideLength, 10 + 300),
            Tag = _MovesPage,
            Text = "Cancel"
         };
         buttonCancel.Click += new EventHandler(Cancel_Click);

         _MovesPage.Controls.Add(listBox);
         _MovesPage.Controls.Add(buttonCancel);

         _MovesPage.CancelButton = buttonCancel;

         var result = _MovesPage.ShowDialog();

         if (result == DialogResult.Cancel)
         {
            _MovesPage.Close();
         }

         //MessageBox.Show(listBox.SelectedItem as string);

         // MessageBox.Show(ListToString(anagramList));

         PassTurn();
      }

      public void listBox_MouseDoubleClick(object sender, MouseEventArgs e)
      {
         ListBox listBox = (ListBox)sender;
         Player player = (Player)listBox.Tag;
         int index = listBox.IndexFromPoint(e.Location);

         if (index != ListBox.NoMatches)
         {
            MessageBox.Show(listBox.SelectedItem as string);
            PlayWord(player, listBox.SelectedItem as string);
         }
         _MovesPage.Close();
      }
      #endregion
   }
}
