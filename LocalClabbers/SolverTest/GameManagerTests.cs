//using System;
//using System.Text;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using ClabbersTest.Properties;
//using Clabbers;
//using System.IO;
//using System.Windows.Forms;
//using System.Text.RegularExpressions;

//namespace ClabbersTest
//{
//   [TestClass]
//   public class GameManagerTests
//   {
//      public GameManagerTests()
//      {
//      }

//      private TestContext testContextInstance;

//      public TestContext TestContext
//      {
//         get
//         {
//            return testContextInstance;
//         }
//         set
//         {
//            testContextInstance = value;
//         }
//      }

//      #region Words text file
//      private static string _Words = String.Join(
//         Environment.NewLine,
//         "APPEL",
//         "APPLE",
//         "PEPLA",
//         "ACT",
//         "CAT",
//         "AC",
//         "TA",
//         "MAPLE",
//         "PAN",
//         "MAP",
//         "ME",
//         "AL",
//         "PI",
//         "PARENTS",
//         "PELICAN",
//         "NA",
//         "AN");
//      #endregion

//      #region Tiles text file
//      private static string _Tiles = String.Join(
//         Environment.NewLine,
//         "*=2",
//         "E=12,A=9,I=9,O=8,N=6,R=6,T=6,L=4,S=4,U=4",
//         "D=4,G=3",
//         "B=2,C=2,M=2,P=2",
//         "F=2,H=2,V=2,W=2,Y=2",
//         "K=1",
//         "",
//         "",
//         "J=1,X=1",
//         "Q=1,Z=1");
//      #endregion

//      #region Map text file
//      private static string _Map = String.Join(
//         Environment.NewLine,
//         "15,15",
//         "WXXlXXXWXXXlXXW",
//         "XwXXXLXXXLXXXwX",
//         "XXwXXXlXlXXXwXX",
//         "lXXwXXXlXXXwXXl",
//         "XXXXwXXXXXwXXXX",
//         "XLXXXLXXXLXXXLX",
//         "XXlXXXlXlXXXlXX",
//         "WXXlXXX*XXXlXXW",
//         "XXlXXXlXlXXXlXX",
//         "XLXXXLXXXLXXXLX",
//         "XXXXwXXXXXwXXXX",
//         "lXXwXXXlXXXwXXl",
//         "XXwXXXlXlXXXwXX",
//         "XwXXXLXXXLXXXwX",
//         "WXXlXXXWXXXlXXW");

//      #endregion

//      #region Blank save text file
//      private static string _BlankSave = String.Join(
//         Environment.NewLine,
//         "...............",
//         "...............",
//         "...............",
//         "...............",
//         "...............",
//         "...............",
//         "...............",
//         "...............",
//         "...............",
//         "...............",
//         "...............",
//         "...............",
//         "...............",
//         "...............",
//         "...............",
//         "7",
//         "2",
//         "MAP*LES",
//         "YYKJXQZ");
//      #endregion

//      #region Save text file
//      private static string _Save = String.Join(
//         Environment.NewLine,
//         "...............",
//         "...............",
//         "...............",
//         "...............",
//         "...............",
//         "...............",
//         "...............",
//         "....PELICAN....",
//         "....a.....AN...",
//         "....R..........",
//         "....E..........",
//         "....N..........",
//         "....T..........",
//         "...............",
//         "...............",
//         "7",
//         "2",
//         "MAP*LES",
//         "YYKJXQZ");
//      #endregion

//      #region Additional test attributes
//      //
//      // You can use the following additional attributes as you write your tests:
//      //
//      // Use ClassInitialize to run code before running the first test in the class
//      // [ClassInitialize()]
//      // public static void MyClassInitialize(TestContext testContext) { }
//      //
//      // Use ClassCleanup to run code after all tests in a class have run
//      // [ClassCleanup()]
//      // public static void MyClassCleanup() { }
//      //
//      // Use TestInitialize to run code before running each test 
//      // [TestInitialize()]
//      // public void MyTestInitialize() { }
//      //
//      // Use TestCleanup to run code after each test has run
//      // [TestCleanup()]
//      // public void MyTestCleanup() { }
//      //
//      #endregion

//      //[ClassInitialize()]
//      //public static void MyClassInitialize(TestContext TestContext)
//      //{
//      //}

//      //[TestInitialize()]
//      //public void MyTestInitialize()
//      //{
//      //}

//      public GameManager MakeGame(int handSize, int numOpp)
//      {
//         GameManager game = new GameManager(_Map, _Tiles, _Words, handSize, numOpp, true);

//         game.MakePlayers();
//         Random rand = new Random();
//         var startingPlayer = rand.Next(0, game._NumPlayers);
//         game._StartingPlayer = game._Players[startingPlayer].ThisPlayer;
//         game.MakeGrid(game._MapString);
//         game.MakeTiles(game._TilesString);
//         game._ActivePlayer = game._StartingPlayer;
//         game._Turn = 1;
//         game._FirstMove = true;

//         return game;
//      }

//      [TestMethod]
//      public void MakePlayers_DesignIntent()
//      {
//         GameManager game = MakeGame(7, 0);
         
//         Player[] players = game._Players;

//         Assert.AreEqual(1, players.Length);
//         Assert.AreEqual(7, players[0].HandMap.Length);
//         Assert.AreEqual(Players.You, players[0].ThisPlayer);
//         Assert.AreEqual(0, players[0].TotalScore);
//         Assert.AreEqual(0, players[0].MoveHistory.Count);
//         Assert.AreEqual(0, players[0].MovesData.Count);
//         Assert.AreEqual(0, players[0].Move.Count);
//         Assert.AreEqual(0, players[0].MoveScore);
//         Assert.AreEqual(MoveErrorType.EmptyMove, players[0].MoveError);

//         GameManager newGame = MakeGame(4, 3);
//         Player[] newPlayers = newGame._Players;

//         Assert.AreEqual(4, newPlayers.Length);
//         Assert.AreEqual(4, newPlayers[0].HandMap.Length);
//         Assert.AreEqual(Players.You, newPlayers[0].ThisPlayer);
//         Assert.AreEqual(4, newPlayers[1].HandMap.Length);
//         Assert.AreEqual(Players.Opp1, newPlayers[1].ThisPlayer);
//         Assert.AreEqual(4, newPlayers[2].HandMap.Length);
//         Assert.AreEqual(Players.Opp2, newPlayers[2].ThisPlayer);
//         Assert.AreEqual(4, newPlayers[3].HandMap.Length);
//         Assert.AreEqual(Players.Opp3, newPlayers[3].ThisPlayer);
//      }

//      [TestMethod]
//      public void SaveGameState_DesignIntent()
//      {
//         GameManager game = MakeGame(7, 1);

//         string saveGameState = game.SaveGameState("savemap", "handsize", "players");

//         string expectedGameState = String.Join(
//         Environment.NewLine,
//         "~SAVEMAP",
//         "...............",
//         "...............",
//         "...............",
//         "...............",
//         "...............",
//         "...............",
//         "...............",
//         "...............",
//         "...............",
//         "...............",
//         "...............",
//         "...............",
//         "...............",
//         "...............",
//         "...............",
//         "~ENDSAVEMAP",
//         "~HANDSIZE",
//         "7",
//         "~ENDHANDSIZE",
//         "~PLAYERS",
//         "2",
//         "       ",
//         "       ",
//         "~ENDPLAYERS",
//         "");

//         Assert.AreEqual(expectedGameState, saveGameState);
//      }

//      //[TestMethod]
//      //public void RestoreSave_TestSave_DesignIntent()
//      //{
//      //   RestoreSave(_Map, _Tiles, _BlankSave);
//      //   Assert.AreEqual(72, _TileBag.Count());
//      //}

//      //[TestMethod]
//      //public void RestoreSave_BlankBoardSave_DesignIntent()
//      //{
//      //   RestoreSave(_Map, _Tiles, _BlankBoardSave);
//      //   Assert.AreEqual(86, _TileBag.Count());
//      //}

//      //[TestMethod]
//      //public void CheckKnownWords()
//      //{
//      //   Assert.IsFalse(_Words.Contains("A"));
//      //   Assert.IsTrue(_Words.Contains("AA"));
//      //   Assert.AreEqual(1, _Words.Count(e => e == "AA"));
//      //}

//      //[TestMethod]
//      //public void FindPerfectAnagrams_DesignIntent()
//      //{
//      //   string hand = "PPLEA";

//      //   List<string> foundAnagrams = FindPerfectAnagrams(hand);

//      //   Assert.AreEqual(3, foundAnagrams.Count);
//      //   Assert.AreEqual("APPEL", foundAnagrams[0]);
//      //   Assert.AreEqual("APPLE", foundAnagrams[1]);
//      //   Assert.AreEqual("PEPLA", foundAnagrams[2]);
//      //}

//      //[TestMethod]
//      //public void FindAnagrams_DesignIntent()
//      //{
//      //   string hand = "CAT";
         
//      //   List<string> foundAnagrams = FindAnagrams(hand);

//      //   Assert.AreEqual(4, foundAnagrams.Count);
//      //   Assert.AreEqual("ACT", foundAnagrams[0]);
//      //   Assert.AreEqual("AT", foundAnagrams[1]);
//      //   Assert.AreEqual("CAT", foundAnagrams[2]);
//      //   Assert.AreEqual("TA", foundAnagrams[3]);
//      //}

//      //[TestMethod]
//      //public void CheckMove_VerticalOverlapWithHorizontalBranch_DesignIntent()
//      //{
//      //   RestoreSave(_Map, _Tiles, _TestSave);
//      //   Player you = _Players[0];

//      //   //M
//      //   _GridMap[6, 9].Tile = you.HandMap[0].Tile;
//      //   you.HandMap[0].Tile = null;
//      //   you.Move.Add(_GridMap[6, 9]);
         
//      //   //A

//      //   //P
//      //   _GridMap[8, 9].Tile = you.HandMap[2].Tile;
//      //   you.HandMap[2].Tile = null;
//      //   you.Move.Add(_GridMap[8, 9]);

//      //   //L
//      //   _GridMap[9, 9].Tile = you.HandMap[4].Tile;
//      //   you.HandMap[4].Tile = null;
//      //   you.Move.Add(_GridMap[9, 9]);

//      //   //E
//      //   _GridMap[10, 9].Tile = you.HandMap[5].Tile;
//      //   you.HandMap[5].Tile = null;
//      //   you.Move.Add(_GridMap[10, 9]);

//      //   Alignment alignment = GameManager.GetAlignment(you.Move);

//      //   Assert.AreEqual(Alignment.Vertical, alignment);

//      //   bool consecutive = CheckConsecutive(you.Move, alignment);

//      //   Assert.IsTrue(consecutive);

//      //   List<MoveData> movesData = GetMovesDataForWord(you.Move, alignment);

//      //   Assert.AreEqual(2, movesData.Count);
//      //   Assert.AreEqual("MAPLE", movesData[0].Word);
//      //   Assert.AreEqual("PAN", movesData[1].Word);

//      //   you.MovesData.AddRange(movesData);
//      //   you.MoveError = MoveErrorType.None;

//      //   int score = GameManager.CalculateScore(you.MovesData);

//      //   Assert.AreEqual(16, score);
//      //   Assert.AreEqual(11, movesData[0].Score);
//      //   Assert.AreEqual(5, movesData[1].Score);
//      //}

//      //[TestMethod]
//      //public void CheckMove_HorizontalClassicWithMultipleVerticalBranches_DesignIntent()
//      //{
//      //   RestoreSave(_Map, _Tiles, _TestSave);
//      //   Player you = _Players[0];

//      //   //M
//      //   _GridMap[6, 5].Tile = you.HandMap[0].Tile;
//      //   you.HandMap[0].Tile = null;
//      //   you.Move.Add(_GridMap[6, 5]);

//      //   //A
//      //   _GridMap[6, 6].Tile = you.HandMap[1].Tile;
//      //   you.HandMap[1].Tile = null;
//      //   you.Move.Add(_GridMap[6, 6]);

//      //   //P
//      //   _GridMap[6, 7].Tile = you.HandMap[2].Tile;
//      //   you.HandMap[2].Tile = null;
//      //   you.Move.Add(_GridMap[6, 7]);


//      //   Alignment alignment = GameManager.GetAlignment(you.Move);

//      //   Assert.AreEqual(Alignment.Horizontal, alignment);

//      //   bool consecutive = _Game.CheckConsecutive(you.Move, alignment);

//      //   Assert.IsTrue(consecutive);

//      //   List<MoveData> movesData = GetMovesDataForWord(you.Move, alignment);

//      //   Assert.AreEqual(4, movesData.Count);
//      //   Assert.AreEqual("MAP", movesData[0].Word);
//      //   Assert.AreEqual("ME", movesData[1].Word);
//      //   Assert.AreEqual("AL", movesData[2].Word);
//      //   Assert.AreEqual("PI", movesData[3].Word);


//      //   you.MovesData.AddRange(movesData);
//      //   you.MoveError = MoveErrorType.None;

//      //   int score = GameManager.CalculateScore(you.MovesData);

//      //   Assert.AreEqual(19, score);
//      //   Assert.AreEqual(8, movesData[0].Score);
//      //   Assert.AreEqual(4, movesData[1].Score);
//      //   Assert.AreEqual(3, movesData[2].Score);
//      //   Assert.AreEqual(4, movesData[3].Score);
//      //}

//      //[TestMethod]
//      //public void CheckMove_VerticalExtend_BlankTileHasNoScore_DesignIntent()
//      //{
//      //   RestoreSave(_Map, _Tiles, _TestSave);
//      //   Player you = _Players[0];

//      //   //S
//      //   _GridMap[13, 4].Tile = you.HandMap[6].Tile;
//      //   you.HandMap[6].Tile = null;
//      //   you.Move.Add(_GridMap[13, 4]);

//      //   Alignment alignment = GameManager.GetAlignment(you.Move);

//      //   //Default is horizontal
//      //   Assert.AreEqual(Alignment.Horizontal, alignment);

//      //   bool consecutive = CheckConsecutive(you.Move, alignment);

//      //   Assert.IsTrue(consecutive);

//      //   List<MoveData> movesData = GetMovesDataForWord(you.Move, alignment);

//      //   Assert.AreEqual(1, movesData.Count);
//      //   Assert.AreEqual("PARENTS", movesData[0].Word);

//      //   you.MovesData.AddRange(movesData);
//      //   you.MoveError = MoveErrorType.None;

//      //   int score = GameManager.CalculateScore(you.MovesData);

//      //   Assert.AreEqual(8, score);
//      //   Assert.AreEqual(8, movesData[0].Score);
//      //}

//      //[TestMethod]
//      //public void FindLargestScoringMoveWithinHand_DesignIntent()
//      //{
//      //   RestoreSave(_Map, _Tiles, _BlankBoardSave);
//      //   Player you = _Players[0];

//      //   List<string> foundAnagrams = FindAnagrams(you.HandMapToString());

//      //   foreach (string anagram in foundAnagrams)
//      //   {
//      //      //List<Cell> move = WordToMoves(anagram);
//      //   }

//      //   Alignment alignment = GameManager.GetAlignment(you.Move);

//      //   //Default is horizontal
//      //   Assert.AreEqual(Alignment.Horizontal, alignment);

//      //   bool consecutive = _Game.CheckConsecutive(you.Move, alignment);

//      //   Assert.IsTrue(consecutive);

//      //   List<MoveData> movesData = _Game.GetMovesDataForWord(you.Move, alignment);

//      //   Assert.AreEqual(1, movesData.Count);
//      //   Assert.AreEqual("PARENTS", movesData[0].Word);

//      //   you.MovesData.AddRange(movesData);
//      //   you.MoveError = MoveErrorType.None;

//      //   int score = GameManager.CalculateScore(you.MovesData);

//      //   Assert.AreEqual(8, score);
//      //   Assert.AreEqual(8, movesData[0].Score);
//      //}
//   }
//}
