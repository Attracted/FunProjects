using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace Clabbers
{
   public class Player
   {     
      public List<MoveData> MoveHistory { get; set; }
      public HandMap HandMap { get; set; }
      public List<Cell> Move { get; set; }
      public List<MoveData> MovesData { get; set; }
      public Players ThisPlayer { get; set; }
      public int TotalScore { get; set; }
      public int MoveScore { get; set; }
      public MoveErrorType MoveError { get; set; }

      public Player()
      {
      }

      public void MakeHandMap(int handRow, int handSize, TileBag tilebag, 
         MoveChangedEventHandler moveHandler, TileChangedEventHandler tileHandler)
      {
         HandMap = new HandMap(handRow, handSize, tilebag, ThisPlayer, moveHandler, tileHandler);
      }

      public string MoveHistoryToString()
      {		
         StringBuilder moveHistory = new StringBuilder();

         int turn = 0;
         for (int i = 0; i < MoveHistory.Count; i++)
         {
            if (turn != MoveHistory[i].Turn)
            {
               turn = MoveHistory[i].Turn;
               moveHistory.AppendLine(turn.ToString() + ".\t" + MoveHistory[i].Word + "\t - " + MoveHistory[i].Score);
            }
            else
            {
               moveHistory.AppendLine("\t" + MoveHistory[i].Word + "\t - " + MoveHistory[i].Score);
            }
         }

         return moveHistory.ToString();
      }

      public string MoveHistoryToStringForSave()
      {
         StringBuilder moveHistory = new StringBuilder();

         int turn = 0;
         for (int i = 0; i < MoveHistory.Count; i++)
         {
            turn = MoveHistory[i].Turn;
            moveHistory.AppendLine(turn.ToString() + "," + MoveHistory[i].Word + "," + MoveHistory[i].Score);
         }
         return moveHistory.ToString().TrimEnd('\r', '\n');
      }

      public string HandMapToString()
      {
         StringBuilder handMap = new StringBuilder();

         for (int i = 0; i < HandMap.Count(); i++)
         {
            //handMap.Append(HandCells[i].Tile.Letter);
            handMap.Append(HandMap[i].Tile.Letter);
         }
         return handMap.ToString();
      }

      public string HandMapToStringForSave()
      {
         StringBuilder handMap = new StringBuilder();

         for (int i = 0; i < HandMap.Count(); i++)
         {
            if (HandMap[i] == null)
            {
               handMap.Append(" ");
               continue;
            }

            Tile tile = HandMap[i].Tile;
            char letter;
            if (tile.IsBlankTile)
            {
               letter = '*';
            }
            else
            {
               letter = tile.Letter;
            }
            handMap.Append(letter);
         }
         return handMap.ToString();
      }
   }
}
