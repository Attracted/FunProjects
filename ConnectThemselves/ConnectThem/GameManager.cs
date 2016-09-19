using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using ConnectThemselves.Properties;

namespace ConnectThemselves
{
   public enum Direction
   {
      None = 0,
      Up = 1,
      Down = 2,
      Left = 3,
      Right = 4,
   }
   public enum Players
   {
      None = 0,
      Red = 1,
      Blue = 2
   }

   public enum Chips
   {
      EmptyChip = 0,
      EmptyChipWin = 1,
      RedChip = 2,
      RedChipWin = 3,
      BlueChip = 4,
      BlueChipWin = 5
   }

   public enum QueueItemType
   {
      NextMove = 0,
      UndoMove = 1,
      YourMove = 2,
      ResetGame = 3,
      NextMoveAuto = 4,
      ResetGameAuto = 5,
      UndoMoveOnce = 6
   }

   public partial class GameManager : Form
   {
      private static Bitmap _EmptyChip = Resources.EmptyChip;
      private static Bitmap _EmptyChipWin = Resources.EmptyChipWin;
      private static Bitmap _RedChip = Resources.RedChip;
      private static Bitmap _RedChipWin = Resources.RedChipWin;
      private static Bitmap _BlueChip = Resources.BlueChip;
      private static Bitmap _BlueChipWin = Resources.BlueChipWin;

      private PictureBox _PictureBox;
      private QueueManager _QueueManager;
      private Cell[,] _SolutionMap;
      private Button[] _ButtonArray;
      private List<string> _CheckWinningLine;
      private List<string> _WinningLine;
      private Players _StartingPlayer;
      private Players _ActivePlayer;
      private Players _WinningPlayer;
      private bool _GameFinished;
      private bool _Checking;
      private bool _WinningMove;
      private List<Player> _Players;
      private int _Row;
      private int _Col;
      private int _Win;
      private int _GameCount;
      private int _MaxGames;
      private bool _Logging;
      private string _FileToSaveTo;

      public int RedWins { get; private set; }
      public int BlueWins { get; private set; }
      public int Ties { get; private set; }

      public Panel GamePanel
      {
         get
         {
            return gamePanel;
         }
      }
      public bool AutoPlayChecked
      {
         get
         {
            return autoPlay.Checked;
         }
      }
      public bool AutoStopChecked
      {
         get
         {
            return autoStop.Checked;
         }

      }
      public bool NormalModeChecked
      {
         get
         {
            return normalMode.Checked;
         }

      }
      public int AutoSliderValue
      {
         get
         {
            return autoSlider.Value;
         }

      }

      public delegate void AutoPlayChangedHandler(object sender, EventArgs e);
      public delegate void NextMoveButtonClickedHandler(object sender, EventArgs e);
      public delegate void UndoMoveButtonClickedHandler(object sender, EventArgs e);
      public delegate void AutoSliderChangedHandler(object sender, EventArgs e);
      public delegate void AutoStopChangedHandler(object sender, EventArgs e);
      public delegate void NormalModeChangedHandler(object sender, EventArgs e);
      public event AutoPlayChangedHandler OnAutoPlayChanged;
      public event NextMoveButtonClickedHandler OnNextMoveButtonClicked;
      public event UndoMoveButtonClickedHandler OnUndoMoveButtonClicked;
      public event AutoSliderChangedHandler OnAutoSliderChanged;
      public event AutoStopChangedHandler OnAutoStopChanged;
      public event NormalModeChangedHandler OnNormalModeChanged;

      public GameManager()
      {
         InitializeComponent();
         this.Icon = Resources.ConnectThem2;
         RedWins = 0;
         BlueWins = 0;
         Ties = 0;

         this.StartPosition = FormStartPosition.CenterScreen;
         this.Show();

         _QueueManager = new QueueManager();
         _QueueManager.OnQueueTimerElapsed += new QueueManager.QueueTimerElapsedHandler(QueueTimerElapsed_Event);

         this.OnAutoPlayChanged += new GameManager.AutoPlayChangedHandler(AutoPlayChanged_Event);
         this.OnNextMoveButtonClicked += new GameManager.NextMoveButtonClickedHandler(NextMoveButtonClicked_Event);
         this.OnUndoMoveButtonClicked += new GameManager.UndoMoveButtonClickedHandler(UndoMoveButtonClicked_Event);
         this.OnAutoSliderChanged += new GameManager.AutoSliderChangedHandler(AutoSliderChanged_Event);
         this.OnAutoStopChanged += new GameManager.AutoStopChangedHandler(AutoStopChanged_Event);
         this.OnNormalModeChanged += new GameManager.NormalModeChangedHandler(NormalModeChanged_Event);

         _CheckWinningLine = new List<string>(_Win);
         _WinningLine = new List<string>(_Win);
      }

      public GameManager(string save)
         : this()
      {
         LoadGameState(save);
      }
      
      public GameManager(int row, int col, int win, int max)
         : this()
      {
         this.StartPosition = FormStartPosition.CenterScreen;
         this.Padding = new Padding(0);
         this.Show();

         _Row = row;
         _Col = col;
         _Win = win;
         _MaxGames = max;

         _Players = new List<Player>(2);

         _Players.Add(new Player(){
            ThisOpponent = Players.Blue,
            ThisPlayer = Players.Red,
            MoveHistory = new Stack<Cell>(),
            CurrentMove = new Cell(_Row, _Col)
         });

         _Players.Add(new Player()
         {
            ThisOpponent = Players.Red,
            ThisPlayer = Players.Blue,
            MoveHistory = new Stack<Cell>(),
            CurrentMove = new Cell(_Row, _Col)
         });

         _GameCount = 1;
         _GameFinished = false;

         _PictureBox = new PictureBox();
         _PictureBox.Size = new Size(_Col * 30, _Row * 30);
         _PictureBox.Visible = true;
         GamePanel.Controls.Add(_PictureBox);

         _SolutionMap = new Cell[_Row, _Col];
         _ButtonArray = new Button[_Col];
         MakeGrid();

         _StartingPlayer = Players.Red;
         _ActivePlayer = _StartingPlayer;
         if (AutoPlayChecked)
         {
            EnqueueItem(new QueueItem()
            {
               QueueItemType = QueueItemType.NextMove
            });
         }
      }

      public GameManager(int row, int col, int win, int max, string fileToSaveTo)
         : this(row, col, win, max)
      {
         _Logging = true;
         _FileToSaveTo = fileToSaveTo;
      }

      private void MakeGrid()
      {
         for (int i = 0; i < _Row; i++)
         {
            for (int j = 0; j < _Col; j++)
            {
               if (i == _Row - 1)
               {
                  _ButtonArray[j] = new Button();
                  DisplayButton(_ButtonArray[j], j);
               }
               _SolutionMap[i, j] = new Cell(i, j);
               DisplayBitmap(Chips.EmptyChip, i, j);
            }
         }
      }

      private void DisplayBitmap(Chips chip, int row, int col)
      {
         _SolutionMap[row, col].Chip = chip;
         if (chip == Chips.EmptyChip)
         {
            _SolutionMap[row, col].Used = false;
            _SolutionMap[row, col].Owner = Players.None;
            _SolutionMap[row, col].Winning = false;
            _SolutionMap[row, col].Value = 0;
         }

         Bitmap bitmap = ChipToBitmap(chip);
         _PictureBox.Paint += (sender, e) =>
         {
            e.Graphics.DrawImage(bitmap, col * 30, row * 30);
         };
      }

      private void DisplayButton(Button button, int col)
      {
         button.Location = new Point(col * 30, _Row * 30);
         button.Size = new Size(30, 30);
         button.Name = col.ToString();
         button.Click += new EventHandler(Button_Click);
         button.Visible = true;
         this.GamePanel.Controls.Add(button);
      }

      private void SetupMakeAndCheckMove(Player player, Chips chip, Action<List<int>> action)
      {
         if (player.MoveHistory.Count > 0)
         {
            var lastMove = player.MoveHistory.Peek();
            DisplayBitmap(chip, lastMove.Row, lastMove.Col);
         }
         
         List<int> movesLeft = CheckMovesLeft();

         if (!_GameFinished && movesLeft.Count == 0)
         {
            Tie();
            return;
         }

         action(movesLeft);

         if (_GameFinished)
         {
            return;
         }

         if (CheckMovesLeft().Count == 0)
         {
            Tie();
            return;
         }
      }
      
      private void YourMove(Player player, Chips chip, int row, int col)
      {         
         player.CurrentMove.Row = row;
         player.CurrentMove.Col = col;

         SetupMakeAndCheckMove(player, chip, (movesLeft) => 
         {
            MakeMove(player);
         });
      }

      private void PlayerMove(Player player, Chips chip)
      {
         SetupMakeAndCheckMove(player, chip, (movesLeft) =>
         {
            if (UsingFindWinningMove(player, player.ThisPlayer, movesLeft)) 
            {
               MakeMove(player);
            }
            else if (UsingFindWinningMove(player, player.ThisOpponent, movesLeft))
            {
               MakeMove(player);
            }
            else
            {
               var smartMovesLeft = CheckSmartMovesLeft(player, movesLeft);

               if (FindSmartMove(player, smartMovesLeft))
               {
                  MakeMove(player);
               }
               else
               {
                  CloseOrRandomMove(player, movesLeft);
                  MakeMove(player);
               }
            }
         });
      }

      private void UndoPlayerMove(Player player, Chips chip)
      {         
         if (_GameFinished)
         {
            DisplayWinningLine(Players.None, _WinningLine);
            RemoveWin(_WinningPlayer);
            _WinningLine.Clear();
            _GameFinished = false;
         }

         if (player.MoveHistory.Count > 0)
         {
            var lastMove = player.MoveHistory.Pop();
            DisplayBitmap(Chips.EmptyChip, lastMove.Row, lastMove.Col);
         }

         if (player.MoveHistory.Count > 0)
         {
            var lastMove = player.MoveHistory.Peek();
            DisplayBitmap(chip, lastMove.Row, lastMove.Col);
         }
         
         player.CurrentMove = new Cell(_Row, _Col);

         Refresh();
      }

      private void StartYourMove(int row, int col)
      {
         switch (_ActivePlayer)
         {
            case Players.Red:
               _ActivePlayer = Players.Blue;
               YourMove(_Players[0], Chips.RedChip, row, col);
               break;
            case Players.Blue:
               _ActivePlayer = Players.Red;
               YourMove(_Players[1], Chips.BlueChip, row, col);
               break;
            default:
               break;
         }
      }
      
      private void StartNextMove()
      {         
         switch (_ActivePlayer)
         {
            case Players.Red:
               PlayerMove(_Players[0], Chips.RedChip);
               _ActivePlayer = Players.Blue;
               break;
            case Players.Blue:
               PlayerMove(_Players[1], Chips.BlueChip);
               _ActivePlayer = Players.Red;
               break;
            default:
               break;
         }
      }

      private void StartUndoMove()
      {        
         switch (_ActivePlayer)
         {
            case Players.Red:
               _ActivePlayer = Players.Blue;
               UndoPlayerMove(_Players[1], Chips.BlueChipWin);
               break;
            case Players.Blue:
               _ActivePlayer = Players.Red;
               UndoPlayerMove(_Players[0], Chips.RedChipWin);
               break;
            default:
               break;
         }
      }

      private void MakeMove(Player player)
      {
         var currentMove = player.CurrentMove;
         int row = currentMove.Row;
         int col = currentMove.Col;
         
         if (player.ThisPlayer == Players.Red)
         {
            DisplayBitmap(Chips.RedChipWin, row, col);
         }
         else if (player.ThisPlayer == Players.Blue)
         {
            DisplayBitmap(Chips.BlueChipWin, row, col);
         }

         _SolutionMap[row, col].Used = true;
         _SolutionMap[row, col].Owner = player.ThisPlayer;
         player.MoveHistory.Push(_SolutionMap[row, col]);
         player.CurrentMove = new Cell(_Row, _Col);

         Refresh();
         CheckProgress(player);
      }

      private int GetNextRow(int col)
      {
         int ret = _Row;
         for (int i = _Row - 1; i >= 0; i--)
         {
            if (!_SolutionMap[i, col].Used)
            {
               ret = i;
               break;
            }
         }
         return ret;
      }

      private bool UsingFindWinningMove(Player player, Players playerEnum, List<int> movesLeft)
      {
         _Checking = true;
         var result = FindWinningMove(player, playerEnum, movesLeft);
         _Checking = false;
         return result;
      }
      
      private bool FindWinningMove(Player player, Players playerEnum, List<int> movesLeft)
      {
         var currentMove = player.CurrentMove;
         int row = _Row;
         int col = _Col;
         
         foreach (int column in movesLeft)
         {
            col = column;
            row = GetNextRow(col);

            currentMove.Row = row;
            currentMove.Col = col;

            if (row != _Row) 
            { 
               _SolutionMap[row, col].Used = true;
               _SolutionMap[row, col].Owner = playerEnum;

               CheckWin(CheckVertical(playerEnum, row, col, 0), playerEnum);
               if (_WinningMove) 
               {
                  _WinningMove = false;
                  _SolutionMap[row, col].Owner = Players.None;
                  _SolutionMap[row, col].Used = false;
                  return true; 
               }
               CheckWin(CheckRight(playerEnum, row, col, CheckLeft(playerEnum, row, col, 0) - 1), playerEnum);
               if (_WinningMove)
               {
                  _WinningMove = false;
                  _SolutionMap[row, col].Owner = Players.None;
                  _SolutionMap[row, col].Used = false;
                  return true;
               }
               CheckWin(CheckRightDiagonalUp(playerEnum, row, col, CheckLeftDiagonalDown(playerEnum, row, col, 0) - 1), playerEnum);
               if (_WinningMove)
               {
                  _WinningMove = false;
                  _SolutionMap[row, col].Owner = Players.None;
                  _SolutionMap[row, col].Used = false;
                  return true;
               }
               CheckWin(CheckRightDiagonalDown(playerEnum, row, col, CheckLeftDiagonalUp(playerEnum, row, col, 0) - 1), playerEnum);
               if (_WinningMove)
               {
                  _WinningMove = false;
                  _SolutionMap[row, col].Owner = Players.None;
                  _SolutionMap[row, col].Used = false;
                  return true;
               }
               _SolutionMap[row, col].Owner = Players.None;
               _SolutionMap[row, col].Used = false;
            }
         }
         return false;
      }

      private bool FindSmartMove(Player player, List<int> movesLeft)
      {
         int row = _Row;
         int col = _Col;

         if (movesLeft.Count == 0)
         {
            return false;
         }
         if (_Players[0].MoveHistory.Count == 0 && _Players[1].MoveHistory.Count == 0)
         {
            return false;
         }
         else if (FindDefensiveMove(player, movesLeft))
         {
            return true;
         }
         else if (FindOffensiveMove(player, movesLeft))
         {
            return true;
         }

         return false;
      }

      private List<int> CheckSmartMovesLeft(Player player, List<int> movesLeft)
      {
         List<int> smartMoves = new List<int>();
         foreach (int column in movesLeft)
         {
            int col = column;
            int row = GetNextRow(col);
            if (row != _Row)
            {
               Player tempPlayer = new Player()
               {
                  ThisPlayer = player.ThisOpponent,
                  ThisOpponent = player.ThisPlayer,
                  CurrentMove = new Cell(player.CurrentMove)
               };

               // Stop them from giving away potential wins.

               _SolutionMap[row, col].Used = true;
               _SolutionMap[row, col].Owner = player.ThisPlayer;
               if (!UsingFindWinningMove(tempPlayer, tempPlayer.ThisPlayer, movesLeft))
               {
                  smartMoves.Add(col);
               }
               _SolutionMap[row, col].Owner = Players.None;
               _SolutionMap[row, col].Used = false;
            }
         }
         return smartMoves;
      }

      private bool FindDefensiveMove(Player player, List<int> movesLeft)
      {
         var currentMove = player.CurrentMove;
         currentMove.Row = _Row;
         currentMove.Col = _Col;
         Random rand = new Random();

         List<Cell> smartMoves = new List<Cell>();

         foreach (int tryCol in movesLeft)
         {
            int tryRow = GetNextRow(tryCol);
            // count the # of non-blocked peices that are in this diag + col + row

            if (CountConsecutive(tryRow, tryCol, Direction.Left, Direction.Up, player.ThisOpponent))
            {
               smartMoves.Add(new Cell(tryRow, tryCol));
            }
            if (CountConsecutive(tryRow, tryCol, Direction.Left, Direction.None, player.ThisOpponent))
            {
               smartMoves.Add(new Cell(tryRow, tryCol));
            }
            if (CountConsecutive(tryRow, tryCol, Direction.Left, Direction.Down, player.ThisOpponent))
            {
               smartMoves.Add(new Cell(tryRow, tryCol));
            }
            if (CountConsecutive(tryRow, tryCol, Direction.Right, Direction.Up, player.ThisOpponent))
            {
               smartMoves.Add(new Cell(tryRow, tryCol));
            }
            if (CountConsecutive(tryRow, tryCol, Direction.Right, Direction.None, player.ThisOpponent))
            {
               smartMoves.Add(new Cell(tryRow, tryCol));
            }
            if (CountConsecutive(tryRow, tryCol, Direction.Right, Direction.Down, player.ThisOpponent))
            {
               smartMoves.Add(new Cell(tryRow, tryCol));
            }
         }

         if (smartMoves.Count == 0)
         {
            return false;
         }

         var smartMove = smartMoves[rand.Next(0, smartMoves.Count)];

         currentMove.Row = smartMove.Row;
         currentMove.Col = smartMove.Col;

         return true;
      }
      
      private bool FindOffensiveMove(Player player, List<int> movesLeft)
      {
         var currentMove = player.CurrentMove;
         currentMove.Row = _Row;
         currentMove.Col = _Col;

         List<Cell> smartMoves = new List<Cell>();

         foreach (int tryCol in movesLeft)
         {
            int tryRow = GetNextRow(tryCol);
            // count the # of non-blocked peices that are in this diag + col + row

            int wins = 0;
            if (CountDirection(tryRow, tryCol, Direction.Left, Direction.Up, Players.Blue) +
               CountDirection(tryRow, tryCol, Direction.Right, Direction.Down, Players.Blue) >= _Win)
            {
               wins++;
            }
            if (CountDirection(tryRow, tryCol, Direction.Left, Direction.None, Players.Blue) + 
                CountDirection(tryRow, tryCol, Direction.Right, Direction.None, Players.Blue) >= _Win)
            {
               wins++;
            }
            if (CountDirection(tryRow, tryCol, Direction.Up, Direction.None, Players.Blue) +
               CountDirection(tryRow, tryCol, Direction.Down, Direction.None, Players.Blue) >= _Win)
            {
               wins++;
            }
            if (CountDirection(tryRow, tryCol, Direction.Left, Direction.Down, Players.Blue) +
                  CountDirection(tryRow, tryCol, Direction.Right, Direction.Up, Players.Blue) >= _Win)
            {
               wins++;
            }
            if (CountDirection(tryRow, tryCol, Direction.Left, Direction.Up, Players.Red) +
               CountDirection(tryRow, tryCol, Direction.Right, Direction.Down, Players.Red) >= _Win)
            {
               wins++;
            }
            if (CountDirection(tryRow, tryCol, Direction.Left, Direction.None, Players.Red) +
                CountDirection(tryRow, tryCol, Direction.Right, Direction.None, Players.Red) >= _Win)
            {
               wins++;
            }
            if (CountDirection(tryRow, tryCol, Direction.Up, Direction.None, Players.Red) +
               CountDirection(tryRow, tryCol, Direction.Down, Direction.None, Players.Red) >= _Win)
            {
               wins++;
            }
            if (CountDirection(tryRow, tryCol, Direction.Left, Direction.Down, Players.Red) +
                  CountDirection(tryRow, tryCol, Direction.Right, Direction.Up, Players.Red) >= _Win)
            {
               wins++;
            }

            if (wins > 1)
            {
               smartMoves.Add(new Cell(tryRow, tryCol) { Value = wins });
            }
         }
         Cell smartMove = smartMoves.OrderByDescending(e => e.Value).FirstOrDefault();

         if (smartMove == null)
         {
            return false;
         }

         Random rand = new Random();
         List<Cell> smartestMoves = smartMoves.FindAll(e => e.Value == smartMove.Value);
         smartMove = smartestMoves[rand.Next(0, smartestMoves.Count)];

         currentMove.Row = smartMove.Row;
         currentMove.Col = smartMove.Col;

         return true;
      }

      private int CountDirection(int row, int col, Direction dir1, Direction dir2, Players player)
      {
         int vert1, hori1;
         int vert2, hori2;
         int count = 0;
         int win = 0;

         EnumToDirection(dir1, out vert1, out hori1);
         EnumToDirection(dir2, out vert2, out hori2);

         int vert = vert1 + vert2;
         int hori = hori1 + hori2;

         int checkRow = row;
         int checkCol = col;

         while (checkRow >= 0 && 
            checkRow < _Row && 
            checkCol >= 0   && 
            checkCol < _Col && 
            (_SolutionMap[checkRow, checkCol].Owner == Players.None || _SolutionMap[checkRow, checkCol].Owner == player))
         {
            if (_SolutionMap[checkRow, checkCol].Used && _SolutionMap[checkRow, checkCol].Owner == player)
            {
               count++;
            }

            checkRow += vert;
            checkCol += hori;
            win++;
         }
         return win;
      }

      private bool CountConsecutive(int row, int col, Direction dir1, Direction dir2, Players player)
      {
         int vert1, hori1;
         int vert2, hori2;
         int playerCount = 0;
         int unUsedCount = 0;

         if (!WinningDirection(dir1, row, col) || !WinningDirection(dir2, row, col))
         {
            return false;
         }

         EnumToDirection(dir1, out vert1, out hori1);
         EnumToDirection(dir2, out vert2, out hori2);

         int vert = vert1 + vert2;
         int hori = hori1 + hori2;

         int checkRow = row;
         int checkCol = col;

         // Only 5 if _Win = 4
         List<Cell> line = new List<Cell>(_Win + 1);

         for (int i = 0; i <= _Win; i++)
         {
            var thisCell = _SolutionMap[checkRow, checkCol];

            if (thisCell.Owner != Players.None && thisCell.Owner != player)
            {
               return false;
            }

            line.Add(thisCell);
            checkRow += vert;
            checkCol += hori;
            if (!thisCell.Used)
            {
               unUsedCount++;
            }
            else if (thisCell.Owner == player)
            {
               playerCount++;
            }
         }

         // OXOXO

         // OXXO0

         // 0OXXO

         if (unUsedCount == 3 &&
            playerCount == 2 && 
            !line[0].Used && 
            !line[_Win].Used && 
            !(col == line[0].Col && !line[1].Used) && 
            !(col == line[_Win].Col && !line[_Win - 1].Used))
         {
            var checkValidCells = line.FindAll(e => e.Owner == Players.None);
            foreach (Cell cell in checkValidCells)
            {
               if (GetNextRow(cell.Col) != cell.Row)
               {
                  return false;
               }
            }
            return true;
         }
         else
         {
            return false;
         }
      }

      private bool WinningDirection(Direction dir, int row, int col)
      {
         switch (dir)
         {
            case Direction.Up:
               if (row <= _Win)
               {
                  return false;
               }
               break;
            case Direction.Down:
               if (row >= _Row - _Win)
               {
                  return false;
               }
               break;
            case Direction.Left:
               if (col <= _Win)
               {
                  return false;
               }
               break;
            case Direction.Right:
               if (col >= _Col - _Win)
               {
                  return false;
               }
               break;
            default:
               break;
         }
         return true;
      }
      
      private void CloseOrRandomMove(Player player, List<int> movesLeft)
      {
         var currentMove = player.CurrentMove;
         currentMove.Row = _Row;
         currentMove.Col = _Col;
         Random rand = new Random();
         while (currentMove.Row == _Row)
         {
            if (movesLeft.Count <= 0)
            {
               currentMove.Row = rand.Next(0, _Col);
               currentMove.Col = GetNextRow(currentMove.Col);
            }
            else
            {
               int lastCol = (player.MoveHistory.Count == 0) ? _Col : player.MoveHistory.Peek().Col;

               List<int> closeMoves = new List<int>();

               int left = (lastCol == 0) ? lastCol : lastCol - 1;
               int right = (lastCol == _Col - 1) ? lastCol : lastCol + 1;
               if (movesLeft.Contains(left))
               {
                  closeMoves.Add(left);
               }
               if (movesLeft.Contains(lastCol))
               {
                  closeMoves.Add(lastCol);
               }
               if (movesLeft.Contains(right))
               {
                  closeMoves.Add(right);
               }

               if (closeMoves.Count < 2)
               {
                  int index = rand.Next(movesLeft.Count);
                  currentMove.Col = movesLeft[index];
               }
               else
               {
                  int index = rand.Next(closeMoves.Count);
                  currentMove.Col = closeMoves[index];
               }

               if (movesLeft.Contains(currentMove.Col))
               {
                  currentMove.Row = GetNextRow(currentMove.Col);
               }
            }
         }
      }

      private List<int> CheckMovesLeft()
      {
         List<int> movesLeft = new List<int>();
         for (int i = 0; i < _Col; i++)
         {
            if (!_SolutionMap[0, i].Used)
            {
               movesLeft.Add(i);
            }
         }
         return movesLeft;
      }

      private void CheckProgress(Player player)
      {
         var currentMove = player.MoveHistory.Peek();
         int row = currentMove.Row;
         int col = currentMove.Col;
         Players playerEnum = player.ThisPlayer;

         CheckWin(CheckVertical(playerEnum, row, col, 0), playerEnum);
         CheckWin(CheckRight(playerEnum, row, col, CheckLeft(playerEnum, row, col, 0) - 1), playerEnum);
         CheckWin(CheckRightDiagonalUp(playerEnum, row, col, CheckLeftDiagonalDown(playerEnum, row, col, 0) - 1), playerEnum);
         CheckWin(CheckRightDiagonalDown(playerEnum, row, col, CheckLeftDiagonalUp(playerEnum, row, col, 0) - 1), playerEnum);
      }

      private int CheckVertical(Players player, int row, int col, int consecutive)
      {
         if (row < _Row && _SolutionMap[row, col].Used && _SolutionMap[row, col].Owner == player)
         {
            AddToWinningLine(row, col);
            consecutive++;
            consecutive = CheckVertical(player, row + 1, col, consecutive);
         }
         return consecutive;
      }

      private int CheckLeft(Players player, int row, int col, int consecutive)
      {
         if (col >= 0 && _SolutionMap[row, col].Used && _SolutionMap[row, col].Owner == player)
         {
            AddToWinningLine(row, col);
            consecutive++;
            consecutive = CheckLeft(player, row, col - 1, consecutive);
         }
         return consecutive;
      }

      private int CheckRight(Players player, int row, int col, int consecutive)
      {
         if (col < _Col && _SolutionMap[row, col].Used && _SolutionMap[row, col].Owner == player)
         {
            AddToWinningLine(row, col);
            consecutive++;
            consecutive = CheckRight(player, row, col + 1, consecutive);
         }
         return consecutive;
      }

      private int CheckRightDiagonalUp(Players player, int row, int col, int consecutive)
      {
         if (row >= 0 && col < _Col && _SolutionMap[row, col].Used && _SolutionMap[row, col].Owner == player)
         {
            AddToWinningLine(row, col);
            consecutive++;
            consecutive = CheckRightDiagonalUp(player, row - 1, col + 1, consecutive);
         }
         return consecutive;
      }

      private int CheckRightDiagonalDown(Players player, int row, int col, int consecutive)
      {
         if (row < _Row && col < _Col && _SolutionMap[row, col].Used && _SolutionMap[row, col].Owner == player)
         {
            AddToWinningLine(row, col);
            consecutive++;
            consecutive = CheckRightDiagonalDown(player, row + 1, col + 1, consecutive);
         }
         return consecutive;
      }

      private int CheckLeftDiagonalUp(Players player, int row, int col, int consecutive)
      {
         if (row >= 0 && col >= 0 && _SolutionMap[row, col].Used && _SolutionMap[row, col].Owner == player)
         {
            AddToWinningLine(row, col);
            consecutive++;
            consecutive = CheckLeftDiagonalUp(player, row - 1, col - 1, consecutive);
         }
         return consecutive;
      }

      private int CheckLeftDiagonalDown(Players player, int row, int col, int consecutive)
      {
         if (row < _Row && col >= 0 && _SolutionMap[row, col].Used && _SolutionMap[row, col].Owner == player)
         {
            AddToWinningLine(row, col);
            consecutive++;
            consecutive = CheckLeftDiagonalDown(player, row + 1, col - 1, consecutive);
         }
         return consecutive;
      }

      private void CheckWin(int win, Players player)
      {
         if (win >= _Win)
         {
            if (_Checking)
            {
               _WinningMove = true;
            }
            else
            {
               _WinningLine.AddRange(_CheckWinningLine);
               Win(player);
            }
         }
         _CheckWinningLine.Clear();
      }

      private void Win(Players player)
      {         
         DisplayWinningLine(player, _CheckWinningLine);

         if (!_GameFinished)
         {
            AddWin(player);
            _WinningPlayer = player;
         }

         _GameFinished = true;

         if (AutoPlayChecked)
         {
            EnqueueItem(new QueueItem()
            {
               QueueItemType = QueueItemType.ResetGameAuto
            });
         }

         if (_Logging)
         {
            SaveGameState();
         }
      }

      private void Tie()
      {
         if (!_GameFinished)
         {
            AddWin(Players.None);
            _WinningPlayer = Players.None;
         }

         _GameFinished = true;

         if (AutoPlayChecked)
         {
            EnqueueItem(new QueueItem()
            {
               QueueItemType = QueueItemType.ResetGameAuto
            });
         }

         if (_Logging)
         {
            SaveGameState();
         }
      }
      
      private void AddToWinningLine(int row, int col)
      {
         string name = (row).ToString() + "," + (col).ToString();

         if (!_CheckWinningLine.Contains(name))
         {
            _CheckWinningLine.Add(name);
         }       
      }

      private void DisplayWinningLine(Players player, List<string> line)
      {
         for (int i = 0; i < line.Count; i++)
         {
            string[] coordinates;
            coordinates = line[i].Split(',');
            int row = int.Parse(coordinates[0]);
            int col = int.Parse(coordinates[1]);

            _SolutionMap[row, col].Winning = true;

            switch (player)
            {
               case Players.Red:
                  DisplayBitmap(Chips.RedChipWin, row, col);
                  break;
               case Players.Blue:
                  DisplayBitmap(Chips.BlueChipWin, row, col);
                  break;
               case Players.None:
                  Chips chip = Chips.EmptyChip;
                  if (_SolutionMap[row, col].Chip == Chips.RedChipWin)
                  {
                     chip = Chips.RedChip;
                  }
                  else if (_SolutionMap[row, col].Chip == Chips.BlueChipWin)
                  {
                     chip = Chips.BlueChip;
                  }
                  DisplayBitmap(chip, row, col);
                  break;
            }
         }
         Refresh();
      }

      private void ResetGame()
      {
         _GameFinished = false;
         _Checking = false;
         _WinningMove = false;

         if (_GameCount >= _MaxGames)
         {
            DisplayGameStatistics();
            return;
         }
         _GameCount++;
         
         foreach (Player player in _Players)
         {
            player.MoveHistory.Clear();
         }

         _CheckWinningLine.Clear();
         _CheckWinningLine.AddRange(new List<string>(_Win));
         _WinningLine.Clear();
         _WinningLine.AddRange(new List<string>(_Win));
         _SolutionMap = null;
         _SolutionMap = new Cell[_Row, _Col];
         _ButtonArray = null;
         _ButtonArray = new Button[_Col];

         MakeGrid();

         Refresh();

         _StartingPlayer = Players.Red;
         _ActivePlayer = _StartingPlayer;
      }

      private void DisplayGameStatistics()
      {
         MessageBox.Show(string.Format("Player1 Wins: {0}, Player2 Wins: {1}, Ties: {2}", RedWins, BlueWins, Ties));
      }

      #region Save Functions
      private void saveGame_Click(object sender, EventArgs e)
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
      
      private string SolutionMapToString()
      {
         StringBuilder s = new StringBuilder();
         for (int i = 0; i < _Row; i++)
         {
            for (int j = 0; j < _Col; j++)
            {
               if (_SolutionMap[i, j].Used)
               {
                  if (_SolutionMap[i, j].Owner == Players.Red)
                  {
                     var lastMove = _Players[0].MoveHistory.Peek();
                     if (_SolutionMap[i, j].Winning || lastMove.Row == i && lastMove.Col == j)
                     {
                        s.Append("R");
                     }
                     else
                     {
                        s.Append("r");
                     }
                  }
                  else if (_SolutionMap[i, j].Owner == Players.Blue)
                  {
                     var lastMove = _Players[1].MoveHistory.Peek();
                     if (_SolutionMap[i, j].Winning || lastMove.Row == i && lastMove.Col == j)
                     {
                        s.Append("U");
                     }
                     else
                     {
                        s.Append("u");
                     }
                  }
               }
               else
               {
                  s.Append("-");
               }

            }
            if (i < _Row - 1) 
            { 
               s.AppendLine(); 
            }
         }
         return s.ToString();
      }

      private string SaveGameState(params string[] objectsToSave)
      {
         string saveGameState;

         using (StringWriter output = new StringWriter())
         {
            foreach (string objectToSave in objectsToSave)
            {
               switch (objectToSave)
               {
                  case "map":
                     output.WriteLine("~MAP");
                     output.WriteLine(_Row + "," + _Col);
                     output.WriteLine(SolutionMapToString());
                     output.WriteLine("~ENDMAP");
                     break;
                  case "players":
                     output.WriteLine("~PLAYERS");
                     output.WriteLine(_ActivePlayer + "," + _StartingPlayer + "," + _Win + "," + _MaxGames + "," + _GameCount + "," + Ties);
                     foreach (Player player in _Players)
                     {
                        output.WriteLine("~PLAYER");
                        switch (player.ThisPlayer)
                        {
                           case Players.Red:
                              output.WriteLine(RedWins);
                              break;
                           case Players.Blue:
                              output.WriteLine(BlueWins);
                              break;
                        }
                        var moveHistory = player.MoveHistoryToString();
                        if (moveHistory != string.Empty && moveHistory != "")
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
      
      private string SaveGameState()
      {
         string fileName = string.Format(@"Logs\Game_{0}-Win_{1}.log", _GameCount, _WinningPlayer);

         return SaveGameState("players", "map");
      }

      private void LoadGameState(string save)
      {
         using (StringReader input = new StringReader(save))
         {
            while (input.Peek() != -1)
            {
               string line = input.ReadLine();

               switch (line)
               {
                  case "~MAP":
                     StringBuilder map = new StringBuilder();
                     line = input.ReadLine();
                     while (line != "~ENDMAP")
                     {
                        map.AppendLine(line);
                        line = input.ReadLine();
                     }
                     MakeGridFromSave(map.ToString());
                     break;
                  case "~PLAYERS":
                     StringBuilder players = new StringBuilder();
                     line = input.ReadLine();
                     while (line != "~ENDPLAYERS")
                     {
                        players.AppendLine(line);
                        line = input.ReadLine();
                     }
                     MakePlayersFromSave(players.ToString());
                     break;
                  default:
                     MessageBox.Show("Persisted variable " + line + " not recognized");
                     return;
               }
            }
         }
      }

      private void MakeGridFromSave(string save)
      {         
         using (StringReader input = new StringReader(save))
         {
            string line = input.ReadLine();
            var splitLine = line.Split(',');
            _Row = int.Parse(splitLine[0]);
            _Col = int.Parse(splitLine[1]);

            _SolutionMap = new Cell[_Row, _Col];
            _ButtonArray = new Button[_Col];

            _PictureBox = new PictureBox();
            _PictureBox.Size = new Size(_Col * 30, _Row * 30);
            _PictureBox.Visible = true;
            GamePanel.Controls.Add(_PictureBox);

            for (int i = 0; i < _Row; i++)
            {
               line = input.ReadLine();

               for (int j = 0; j < _Col; j++)
               {
                  if (i == _Row - 1)
                  {
                     _ButtonArray[j] = new Button();
                     DisplayButton(_ButtonArray[j], j);
                  }
                  
                  char letter = line[j];
                  Chips chip = Chips.EmptyChip;
                  switch (letter)
                  {
                     case 'R':
                        chip = PlayersToChip(Players.Red, true);
                        _SolutionMap[i, j] = new Cell()
                        {
                           Used = true,
                           Owner = Players.Red,
                           Row = i,
                           Col = j,
                           Value = 0,
                           Winning = (_WinningPlayer == Players.Red || _Players[0].MoveHistory.Count > 0) ? true : false,
                           Chip = chip
                        };
                        break;
                     case 'r':
                        chip = PlayersToChip(Players.Red, false);
                        _SolutionMap[i, j] = new Cell()
                        {
                           Used = true,
                           Owner = Players.Red,
                           Row = i,
                           Col = j,
                           Value = 0,
                           Winning = false,
                           Chip = chip
                        };
                        break;
                     case 'U':
                        chip = PlayersToChip(Players.Blue, true);
                        _SolutionMap[i, j] = new Cell()
                        {
                           Used = true,
                           Owner = Players.Blue,
                           Row = i,
                           Col = j,
                           Value = 0,
                           Winning = (_WinningPlayer == Players.Blue || _Players[1].MoveHistory.Count > 0) ? true : false,
                           Chip = chip
                        };
                        break;
                     case 'u':
                        chip = PlayersToChip(Players.Blue, false);
                        _SolutionMap[i, j] = new Cell()
                        {
                           Used = true,
                           Owner = Players.Blue,
                           Row = i,
                           Col = j,
                           Value = 0,
                           Winning = false,
                           Chip = chip
                        };
                        break;
                     case '-':
                        chip = PlayersToChip(Players.None, false);
                        _SolutionMap[i, j] = new Cell()
                        {
                           Used = false,
                           Owner = Players.None,
                           Row = i,
                           Col = j,
                           Value = 0,
                           Winning = false,
                           Chip = chip
                        };
                        break;
                     default:
                        MessageBox.Show(string.Format("Error loading game: Invalid letter {0}", letter));
                        break;
                  }

                  DisplayBitmap(chip, i, j);
               }
            }
         }
      }

      private void MakePlayersFromSave(string save)
      {
         using (StringReader input = new StringReader(save))
         {
            string line = input.ReadLine();
            //_ActivePlayer + "," + _StartingPlayer + "," + _Win + "," + _MaxGames + "," + _GameCount + "," + Ties;
            var gameData = line.Split(',');
            _ActivePlayer = (Players)Enum.Parse(typeof(Players), gameData[0]);
            _StartingPlayer = (Players)Enum.Parse(typeof(Players), gameData[1]);
            _Win           = int.Parse(gameData[2]);
            _MaxGames      = int.Parse(gameData[3]);
            _GameCount     = int.Parse(gameData[4]);
            Ties           = int.Parse(gameData[5]);

            _Players = new List<Player>(2);

            _Players.Add(new Player()
            {
               ThisOpponent = Players.Blue,
               ThisPlayer = Players.Red,
               MoveHistory = new Stack<Cell>(),
               CurrentMove = new Cell()
            });

            _Players.Add(new Player()
            {
               ThisOpponent = Players.Red,
               ThisPlayer = Players.Blue,
               MoveHistory = new Stack<Cell>(),
               CurrentMove = new Cell()
            });

            for (int i = 0; i < _Players.Count; i++)
            {
               line = input.ReadLine();
               if (line != "~PLAYER")
               {
                  MessageBox.Show("Inproper formatting of ~PLAYERS");
                  this.Close();
                  return;
               }

               line = input.ReadLine();
               switch (_Players[i].ThisPlayer)
               {
                  case Players.Red:
                     RedWins = int.Parse(line);
                     break;
                  case Players.Blue:
                     BlueWins = int.Parse(line);
                     break;
               }
               UpdateWins();

               line = input.ReadLine();
               var currentMove = line.Split(',');
               _Players[i].CurrentMove.Row = int.Parse(currentMove[0]);
               _Players[i].CurrentMove.Col = int.Parse(currentMove[1]);

               line = input.ReadLine();
               while (line != "~ENDPLAYER")
               {
                  var moveData = line.Split(',');
                  _Players[i].MoveHistory.Push(new Cell(int.Parse(moveData[0]), int.Parse(moveData[1])));

                  line = input.ReadLine();
               }
            }
         }
      }
      #endregion

      #region Helper Functions
      private void EnqueueItem(QueueItem queueItem)
      {
         _QueueManager.AddToQueue(queueItem);
      }

      private Bitmap ChipToBitmap(Chips chip)
      {
         switch (chip)
         {
            case Chips.EmptyChip:
               return _EmptyChip;
            case Chips.EmptyChipWin:
               return _EmptyChipWin;
            case Chips.RedChip:
               return _RedChip;
            case Chips.RedChipWin:
               return _RedChipWin;
            case Chips.BlueChip:
               return _BlueChip;
            case Chips.BlueChipWin:
               return _BlueChipWin;
            default:
               return null;
         }
      }

      private void EnumToDirection(Direction dir, out int vert, out int hori)
      {
         vert = 0;
         hori = 0;

         switch (dir)
         {
            case Direction.Up:
               vert = -1;
               break;
            case Direction.Down:
               vert = 1;
               break;
            case Direction.Left:
               hori = -1;
               break;
            case Direction.Right:
               hori = 1;
               break;
            default:
               break;
         }
      }

      private Chips PlayersToChip(Players player, bool win)
      {
         if (player == Players.None)
         {
            if (win)
            {
               return Chips.EmptyChipWin;
            }
            else
            {
               return Chips.EmptyChip;
            }
         }

         if (player == Players.Red)
         {
            if (win)
            {
               return Chips.RedChipWin;
            }
            else
            {
               return Chips.RedChip;
            }
         }

         if (player == Players.Blue)
         {
            if (win)
            {
               return Chips.BlueChipWin;
            }
            else
            {
               return Chips.BlueChip;
            }
         }

         return Chips.EmptyChip;
      }

      private Player GetOpposingPlayer(Player player)
      {
         switch (player.ThisOpponent)
         {
            case Players.Red:
               return _Players[0];
            case Players.Blue:
               return _Players[1];
            default:
               return null;
         }
      }
      #endregion

      #region Game Events
      public void Button_Click(object sender, EventArgs e)
      {
         if (!AutoPlayChecked)
         {
            if (_GameFinished)
            {
               EnqueueItem(new QueueItem()
               {
                  QueueItemType = QueueItemType.ResetGame
               });
            }
            else
            {
               Button button = (Button)sender;
               int col = int.Parse(button.Name);
               int row = GetNextRow(col);
               if (row < _Row)
               {
                  EnqueueItem(new QueueItem()
                  {
                     QueueItemType = QueueItemType.YourMove,
                     Cell = new Cell(row, col)
                  });
               }
            }
         }
      }
      public void AutoPlayChanged_Event(object sender, EventArgs e)
      {
         if (AutoPlayChecked)
         {
            _QueueManager.StopAutoMove = false;
            if (_GameFinished)
            {
               EnqueueItem(new QueueItem()
               {
                  QueueItemType = QueueItemType.ResetGameAuto
               });
            }
            else
            {
               EnqueueItem(new QueueItem()
               {
                  QueueItemType = QueueItemType.NextMoveAuto
               });
            }
         }
         else
         {
            _QueueManager.StopAutoMove = true;
         }
      }
      public void AutoStopChanged_Event(object sender, EventArgs e)
      {
         if (AutoStopChecked)
         {
            _QueueManager.StopAutoReset = true;
         }
         else
         {
            _QueueManager.StopAutoReset = false;
            if (AutoPlayChecked && _GameFinished)
            {
               EnqueueItem(new QueueItem()
               {
                  QueueItemType = QueueItemType.ResetGameAuto
               });
            }
         }
      }
      public void NormalModeChanged_Event(object sender, EventArgs e)
      {
         if (NormalModeChecked && _ActivePlayer == Players.Blue && !_GameFinished)
         {
            EnqueueItem(new QueueItem()
            {
               QueueItemType = QueueItemType.NextMove
            });
         }
      }
      public void NextMoveButtonClicked_Event(object sender, EventArgs e)
      {
         if (!AutoPlayChecked)
         {
            if (_GameFinished)
            {
               EnqueueItem(new QueueItem()
               {
                  QueueItemType = QueueItemType.ResetGame
               });
            }
            else
            {
               EnqueueItem(new QueueItem()
               {
                  QueueItemType = QueueItemType.NextMove
               });
            }
         }
         else
         {
            if (AutoStopChecked && _GameFinished)
            {
               _QueueManager.StopAutoReset = false;
               EnqueueItem(new QueueItem()
               {
                  QueueItemType = QueueItemType.ResetGameAuto
               });
               _QueueManager.StopAutoReset = true;
            }
         }
      }
      public void UndoMoveButtonClicked_Event(object sender, EventArgs e)
      {
         if (!AutoPlayChecked)
         {
            EnqueueItem(new QueueItem()
            {
               QueueItemType = QueueItemType.UndoMove
            });
         }
      }
      public void AutoSliderChanged_Event(object sender, EventArgs e)
      {
         if (AutoPlayChecked)
         {
            _QueueManager.SetTimerInterval(AutoSliderValue);
         }
      }
      public void QueueTimerElapsed_Event(object sender, QueueEventArgs e)
      {
         var item = e.Item;

         switch (item.QueueItemType)
         {
            case QueueItemType.NextMove:
               StartNextMove();
               break;
            case QueueItemType.UndoMove:
               StartUndoMove();
               if (NormalModeChecked)
               {
                  EnqueueItem(new QueueItem()
                  {
                     QueueItemType = QueueItemType.UndoMoveOnce
                  });
               }
               break;
            case QueueItemType.YourMove:
               StartYourMove(item.Cell.Row, item.Cell.Col);
               if (NormalModeChecked)
               {
                  if (!_GameFinished)
                  {
                     EnqueueItem(new QueueItem()
                     {
                        QueueItemType = QueueItemType.NextMove
                     });
                  }
               }
               break;
            case QueueItemType.ResetGame:
               ResetGame();
               break;
            case QueueItemType.NextMoveAuto:
               StartNextMove();
               if (!_GameFinished)
               {
                  EnqueueItem(new QueueItem()
                  {
                     QueueItemType = QueueItemType.NextMoveAuto
                  });
               }
               break;
            case QueueItemType.ResetGameAuto:
               ResetGame();
               EnqueueItem(new QueueItem()
               {
                  QueueItemType = QueueItemType.NextMoveAuto
               });
               break;
            case QueueItemType.UndoMoveOnce:
               StartUndoMove();
               break;
         }
      }
      #endregion

      #region Form Functions
      private void AddWin(Players player)
      {
         switch (player)
         {
            case Players.Red:
               RedWins++;
               break;
            case Players.Blue:
               BlueWins++;;
               break;
            case Players.None:
               Ties++;
               break;
         }
         UpdateWins();
      }
      private void RemoveWin(Players player)
      {
         switch (player)
         {
            case Players.Red:
               RedWins--;
               break;
            case Players.Blue:
               BlueWins--;
               break;
            case Players.None:
               Ties--;
               break;
         }
         UpdateWins();
      }
      private void UpdateWins()
      {
         redWins.Text = RedWins.ToString();
         blueWins.Text = BlueWins.ToString();
         ties.Text = Ties.ToString();
      }
      private void Form2_Load(object sender, EventArgs e)
      {

      }
      private void autoPlay_CheckedChanged(object sender, EventArgs e)
      {
         var autoPlay = (CheckBox)sender;

         if (autoPlay.Checked)
         {
            autoStop.Enabled = true;
            normalMode.Checked = false;
         }
         else
         {
            autoStop.Enabled = false;
         }

         if (OnAutoPlayChanged == null) return;

         EventArgs args = new EventArgs();
         OnAutoPlayChanged(this, args);
      }
      private void normalMode_CheckedChanged(object sender, EventArgs e)
      {
         if (OnNormalModeChanged == null) return;

         if (normalMode.Checked)
         {
            autoPlay.Checked = false;
         }

         EventArgs args = new EventArgs();
         OnNormalModeChanged(this, args);
      }
      private void nextMoveButton_Click(object sender, EventArgs e)
      {
         if (OnNextMoveButtonClicked == null) return;

         EventArgs args = new EventArgs();
         OnNextMoveButtonClicked(this, args);
      }
      private void undoMoveButton_Click(object sender, EventArgs e)
      {
         if (OnUndoMoveButtonClicked == null) return;

         EventArgs args = new EventArgs();
         OnUndoMoveButtonClicked(this, args);
      }
      private void autoSlider_Scroll(object sender, EventArgs e)
      {
         if (OnAutoSliderChanged == null) return;

         EventArgs args = new EventArgs();
         OnAutoSliderChanged(this, args);
      }
      private void autoStop_CheckedChanged(object sender, EventArgs e)
      {
         if (OnAutoStopChanged == null) return;

         EventArgs args = new EventArgs();
         OnAutoStopChanged(this, args);
      }
      #endregion
   }
}
