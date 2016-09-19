using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using MazeRunner.Properties;

namespace MazeRunner
{
   public class CreateMode
   {
      private Map CreateMap;

      private Player CharCursor;

      private Dictionary<int, int> Portals;

      public CreateMode(string[] map)
      {
         CharCursor = new Player(new Location(GlobalValues.BORDER_LOCATION.Left, GlobalValues.BORDER_LOCATION.Top), 
            SpecialChars.Wall_Horizontal);
         CharCursor.Colour = ConsoleColor.Gray; 
         CreateMap = new Map(map);
         Portals = new Dictionary<int, int>() 
         {
            { 0, 0 },
            { 1, 0 },
            { 2, 0 },
            { 3, 0 },
            { 4, 0 },
            { 5, 0 },
            { 6, 0 },
            { 7, 0 },
            { 8, 0 },
            { 9, 0 }
         };

         InitializeWindow();
      }

      public bool Go()
      {
         if (!Console.KeyAvailable)
         {
            return true;
         }

         ConsoleKeyInfo keyInfo = Console.ReadKey(true);

         ConsoleKey key = keyInfo.Key;
         ConsoleModifiers keyMods = keyInfo.Modifiers;
         
         int prevRow = CharCursor.Row;
         int prevCol = CharCursor.Col;
         char prevChar = CreateMap[prevCol, prevRow];

         switch (key)
         {
            case ConsoleKey.D1:
            case ConsoleKey.NumPad1:
               CharCursor.Character = '1';
               break;
            case ConsoleKey.D2:
            case ConsoleKey.NumPad2:
               CharCursor.Character = '2';
               break;
            case ConsoleKey.D3:
            case ConsoleKey.NumPad3:
               CharCursor.Character = '3';
               break;
            case ConsoleKey.D4:
            case ConsoleKey.NumPad4:
               CharCursor.Character = '4';
               break;
            case ConsoleKey.D5:
            case ConsoleKey.NumPad5:
               CharCursor.Character = '5';
               break;
            case ConsoleKey.D6:
            case ConsoleKey.NumPad6:
               CharCursor.Character = '6';
               break;
            case ConsoleKey.D7:
            case ConsoleKey.NumPad7:
               CharCursor.Character = '7';
               break;
            case ConsoleKey.D8:
            case ConsoleKey.NumPad8:
               CharCursor.Character = '8';
               break;
            case ConsoleKey.D9:
            case ConsoleKey.NumPad9:
               CharCursor.Character = '9';
               break;
            case ConsoleKey.D0:
            case ConsoleKey.NumPad0:
               CharCursor.Character = '0';
               break;


            case ConsoleKey.Q:
               CharCursor.Character = SpecialChars.Wall_UpRight;
               break;
            case ConsoleKey.W:
               CharCursor.Character = SpecialChars.Wall_DownTee; 
               break;
            case ConsoleKey.E:
               CharCursor.Character = SpecialChars.Wall_UpLeft;
               break;
            case ConsoleKey.R:
               CharCursor.Character = SpecialChars.Wall_Horizontal;
               break;
            case ConsoleKey.T:
               CharCursor.Character = SpecialChars.Mew3_Start;
               break;

            case ConsoleKey.A:
               CharCursor.Character = SpecialChars.Wall_RightTee;
               break;
            case ConsoleKey.S:
               CharCursor.Character = SpecialChars.Wall_AllWay;
               break;
            case ConsoleKey.D:
               CharCursor.Character = SpecialChars.Wall_LeftTee;
               break;
            case ConsoleKey.F:
               CharCursor.Character = SpecialChars.Wall_Vertical;
               break;
            case ConsoleKey.G:
               CharCursor.Character = SpecialChars.Ghost;
               break;

            case ConsoleKey.Z:
               CharCursor.Character = SpecialChars.Wall_DownRight; 
               break;
            case ConsoleKey.X:
               CharCursor.Character = SpecialChars.Wall_UpTee;
               break;
            case ConsoleKey.C:
               CharCursor.Character = SpecialChars.Wall_DownLeft;
               break;
            case ConsoleKey.V:
               CharCursor.Character = SpecialChars.Bit;
               break;
            case ConsoleKey.B:
               CharCursor.Character = SpecialChars.Power;
               break;

            //case ConsoleKey.N:
            //   DrawCreateTools_Page1();
            //   break;
            //case ConsoleKey.M:
            //   DrawCreateTools_Page2();
            //   break;
            case ConsoleKey.Enter:
            case ConsoleKey.Insert:
            case ConsoleKey.Tab:
               PasteChar(CharCursor.Character);
               break;
            case ConsoleKey.Delete:
            case ConsoleKey.Backspace:
            case ConsoleKey.Oem3:
               DeleteChar();
               break;
            case ConsoleKey.LeftArrow:
               if (CharCursor.Col > 0)
               {
                  CharCursor.UpdateLocation(CharCursor.Col - 1, CharCursor.Row);
               }
               break;
            case ConsoleKey.RightArrow:
               if (CharCursor.Col < GlobalValues.BORDER_LOCATION.Width - 2)
               {
                  CharCursor.UpdateLocation(CharCursor.Col + 1, CharCursor.Row);
               }
               break;
            case ConsoleKey.UpArrow:
               if (CharCursor.Row > 0)
               {
                  CharCursor.UpdateLocation(CharCursor.Col, CharCursor.Row - 1);
               }
               break;
            case ConsoleKey.DownArrow:
               if (CharCursor.Row < GlobalValues.BORDER_LOCATION.Height - 1)
               {
                  CharCursor.UpdateLocation(CharCursor.Col, CharCursor.Row + 1);
               }
               break;

            case ConsoleKey.Escape:
               if (SaveMapToFile())
               {
                  return false;
               }
               break;
            default:
               return true;
         }

         UpdateConsole(prevCol, prevRow, prevChar);

         return true;
      }

      private void PasteChar(char c)
      {
         if (!OverwriteAnyPortals(c))
         {
            return;
         }
         
         CreateMap.UpdateMap(CharCursor.CurrentLoc, c);
      }
      private void DeleteChar()
      {         
         PasteChar(' ');
      }

      private bool OverwriteAnyPortals(char c)
      {
         int num;
         if (int.TryParse(c.ToString(), out num))
         {
            int portals;

            if (Portals.TryGetValue(num, out portals))
            {
               if (portals == 2)
               {
                  return false;
               }

               Portals[num] = portals + 1;
            }
         }
         
         char mapChar = CreateMap[CharCursor.Col, CharCursor.Row];

         if (int.TryParse(mapChar.ToString(), out num))
         {
            int portals;

            if (Portals.TryGetValue(num, out portals))
            {
               if (portals > 0)
               {
                  Portals[num] = portals - 1;
               }
            }
         }

         return true;
      }

      private bool SaveMapToFile()
      {
         for (int i = 0; i < Portals.Count; i++)
         {
            int portal = Portals[i];
            if (portal > 2)
            {
               ConsoleMethods.CentreError(string.Format("Too many portals for '{0}'", portal), "Need exactly two portals");
               DrawCreateTools_Page1();
               return false;
            }
            if (portal == 1)
            {
               ConsoleMethods.CentreError(string.Format("Not enough portals for '{0}'", portal), "Need exactly two portals");
               DrawCreateTools_Page1();
               return false;
            }
         }
         
         File.WriteAllLines(@"CustomMap.txt", CreateMap.GameMap);

         return true;
      }

      private void UpdateConsole(int prevCol, int prevRow, char prevChar)
      {
         ConsoleMethods.WriteText(GetChar(prevChar),
            prevCol + GlobalValues.BORDER_LOCATION.Left,
            prevRow + GlobalValues.BORDER_LOCATION.Top,
            GetColour(prevChar));
         ConsoleMethods.WriteText(GetChar(CharCursor.Character), 
            CharCursor.Col + GlobalValues.BORDER_LOCATION.Left, 
            CharCursor.Row + GlobalValues.BORDER_LOCATION.Top,
            GetColour(CharCursor.Character));
      }

      private char GetChar(char c)
      {
         char portal = c;

         int num;

         if (int.TryParse(c.ToString(), out num))
         {
            portal = SpecialChars.Portal;
         }

         return portal;
      }

      private ConsoleColor GetColour(char c)
      {
         ConsoleColor colour = CharCursor.Colour; 

         int num;

         if(int.TryParse(c.ToString(), out num))
         {
            colour = Colours.ColoursList[num];
         }
         else if (c == SpecialChars.Wall_UpRight    ||
                  c == SpecialChars.Wall_DownTee    ||
                  c == SpecialChars.Wall_UpLeft     ||
                  c == SpecialChars.Wall_RightTee   ||
                  c == SpecialChars.Wall_Horizontal ||
                  c == SpecialChars.Wall_AllWay     ||
                  c == SpecialChars.Wall_LeftTee    ||
                  c == SpecialChars.Wall_Vertical   ||
                  c == SpecialChars.Wall_DownRight  ||
                  c == SpecialChars.Wall_UpTee      ||
                  c == SpecialChars.Wall_DownLeft
            )
         {
            colour = ConsoleColor.DarkCyan;
         }
         else if (c == SpecialChars.Mew3_Start ||
                  c == SpecialChars.Bit ||
                  c == SpecialChars.Power)
         {
            colour = ConsoleColor.Yellow;
         }
         else if (c == SpecialChars.Ghost){
            colour = ConsoleColor.Red;
         }

         return colour;
      }

      private void InitializeWindow()
      {
         ConsoleMethods.DrawCreateBorder();

         DrawCreateTools_Page1();
         
         ConsoleMethods.WriteText(GetChar(CharCursor.Character),
            CharCursor.Col + GlobalValues.BORDER_LOCATION.Left,
            CharCursor.Row + GlobalValues.BORDER_LOCATION.Top,
            GetColour(CharCursor.Character));
      }

      private static void DrawCreateTools_Page1()
      {
         ConsoleMethods.ClearPromptLines();

         //                        0    5    10   15   20   25
         //                        V    V    V    V    V    V
         ConsoleMethods.WriteText("Tild)    Q  W  E   R  T", 1, GlobalValues.PROMPT_LINE, ConsoleColor.DarkGray);
         ConsoleMethods.WriteText("Tab)      A  S  D   F  G", 1, GlobalValues.CHOICES_LINE, ConsoleColor.DarkGray);
         ConsoleMethods.WriteText("Esc)       Z  X  C   V  B", 1, GlobalValues.CHOICES_LINE + 1, ConsoleColor.DarkGray);

         ConsoleMethods.WriteText("Del", 5 + 1, GlobalValues.PROMPT_LINE, ConsoleColor.White);
         ConsoleMethods.WriteText(SpecialChars.Wall_UpRight,      3 * 3 + 2, GlobalValues.PROMPT_LINE, ConsoleColor.White);
         ConsoleMethods.WriteText(SpecialChars.Wall_DownTee,      4 * 3 + 2, GlobalValues.PROMPT_LINE, ConsoleColor.White);
         ConsoleMethods.WriteText(SpecialChars.Wall_UpLeft,       5 * 3 + 2, GlobalValues.PROMPT_LINE, ConsoleColor.White);
         ConsoleMethods.WriteText(SpecialChars.Wall_Horizontal,   6 * 3 + 3, GlobalValues.PROMPT_LINE, ConsoleColor.White);
         ConsoleMethods.WriteText(SpecialChars.Mew3_Start,        7 * 3 + 3, GlobalValues.PROMPT_LINE, ConsoleColor.White);

         ConsoleMethods.WriteText("Drop", 4 + 1, GlobalValues.CHOICES_LINE, ConsoleColor.White);
         ConsoleMethods.WriteText(SpecialChars.Wall_RightTee,  3 * 3 + 3, GlobalValues.CHOICES_LINE, ConsoleColor.White);
         ConsoleMethods.WriteText(SpecialChars.Wall_AllWay,    4 * 3 + 3, GlobalValues.CHOICES_LINE, ConsoleColor.White);
         ConsoleMethods.WriteText(SpecialChars.Wall_LeftTee,   5 * 3 + 3, GlobalValues.CHOICES_LINE, ConsoleColor.White);
         ConsoleMethods.WriteText(SpecialChars.Wall_Vertical,  6 * 3 + 4, GlobalValues.CHOICES_LINE, ConsoleColor.White);
         ConsoleMethods.WriteText(SpecialChars.Ghost,          7 * 3 + 4, GlobalValues.CHOICES_LINE, ConsoleColor.White);

         ConsoleMethods.WriteText("Done", 4 + 1, GlobalValues.CHOICES_LINE + 1, ConsoleColor.White);
         ConsoleMethods.WriteText(SpecialChars.Wall_DownRight,    3 * 3 + 4, GlobalValues.CHOICES_LINE + 1, ConsoleColor.White);
         ConsoleMethods.WriteText(SpecialChars.Wall_UpTee,        4 * 3 + 4, GlobalValues.CHOICES_LINE + 1, ConsoleColor.White);
         ConsoleMethods.WriteText(SpecialChars.Wall_DownLeft,     5 * 3 + 4, GlobalValues.CHOICES_LINE + 1, ConsoleColor.White);
         ConsoleMethods.WriteText(SpecialChars.Bit,               6 * 3 + 5, GlobalValues.CHOICES_LINE + 1, ConsoleColor.White);
         ConsoleMethods.WriteText(SpecialChars.Power,             7 * 3 + 5, GlobalValues.CHOICES_LINE + 1, ConsoleColor.White);
      }

      private static void DrawCreateTools_Page2()
      {
         ConsoleMethods.ClearPromptLines();
         ConsoleMethods.ClearLine(GlobalValues.CHOICES_LINE + 2);

         ConsoleMethods.WriteText(string.Format("{0}){1}", "Del", "Delete"), 0 * 5 + 1, GlobalValues.CHOICES_LINE, ConsoleColor.White);
         ConsoleMethods.WriteText(string.Format("{0}){1}", "Enter", "Insert"), 1 * 5 + 1, GlobalValues.CHOICES_LINE, ConsoleColor.White);
         ConsoleMethods.WriteText(string.Format("{0}){1}", "Ctrl", "Clear"), 2 * 5 + 1, GlobalValues.CHOICES_LINE, ConsoleColor.White);
         ConsoleMethods.WriteText(string.Format("{0}){1}", "Arrows", "Move"), 3 * 5 + 1, GlobalValues.CHOICES_LINE, ConsoleColor.White);
         ConsoleMethods.WriteText(string.Format("{0}){1}", "N", "Page1"), 4 * 5 + 1, GlobalValues.CHOICES_LINE + 2);
      }
   }
}