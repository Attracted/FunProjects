using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextAdventure
{
   public class StateTransition
   {
      readonly GameState CurrentGameState;
      readonly GenericCommands Command;

      public StateTransition(GameState currentGameState, GenericCommands command)
      {
         CurrentGameState = currentGameState;
         Command = command;
      }

      public override int GetHashCode()
      {
         return 17 + 31 * CurrentGameState.GetHashCode() + 31 * Command.GetHashCode();
      }

      public override bool Equals(object obj)
      {
         StateTransition other = obj as StateTransition;
         return other != null && this.CurrentGameState == other.CurrentGameState && this.Command == other.Command;
      }
   }
}
