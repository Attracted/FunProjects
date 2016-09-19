using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextAdventure
{
   public static class GenericInputDecoder
   {
      public static GenericCommands Decode(string line)
      {
         string lowerCase = line.ToLower();

         switch (lowerCase)
         {
            #region North
            case "north":
            case "go north":
            case "move north":
            case "head north":
               return GenericCommands.North;
            #endregion
            #region East
            case "east":
            case "go east":
            case "move east":
            case "head east":
               return GenericCommands.East;
            #endregion
            #region South
            case "south":
            case "go south":
            case "move south":
            case "head south":
               return GenericCommands.South;
            #endregion
            #region West
            case "west":
            case "go west":
            case "move west":
            case "head west":
               return GenericCommands.West;
            #endregion
            #region Get
            case "get":
            case "grab":
            case "take":
               return GenericCommands.Get;
            #endregion
            #region Use
            case "use":
               return GenericCommands.Use;
            #endregion
            #region Look
            case "look":
            case "look around":
            case "peek":
            case "see":
               return GenericCommands.Look;
            #endregion
            #region Back
            case "back":
            case "undo":
               return GenericCommands.Back;
            #endregion
            #region Inventory
            case "inventory":
            case "bag":
            case "stuff":
            case "things":
               return GenericCommands.Inventory;
            #endregion
            #region Help
            case "help":
            case "help!":
            case "/?":
               return GenericCommands.Help;
            #endregion
            default:
               return GenericCommands.Default;
         }
      }
   }
}
