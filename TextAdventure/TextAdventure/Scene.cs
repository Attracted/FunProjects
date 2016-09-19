using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextAdventure
{
   abstract public class Scene
   {
      virtual public void DoCommand(GenericCommands command)
      {
         if (CommandToText.ContainsKey(command))
         {
            ScreenText = CommandToText[command];
         }
      }

      virtual public void DoCommand(GenericCommands command, List<string> modifiers)
      {
      }

      public event BagEventsHandler OnBagContentsChange;

      public void AddOrRemoveItem(Item bagItem, bool addItem)
      {
         if (OnBagContentsChange == null)
         {
            return;
         }

         BagEventArgs args = new BagEventArgs(bagItem, addItem);

         OnBagContentsChange(this, args);
      }

      abstract public Dictionary<GenericCommands, string> CommandToText { get; }
      abstract public Dictionary<StateTransition, GameState> Transitions { get; }
      abstract public List<Item> Items { get; }
      abstract public GameState GameState { get; }
      abstract public string ScreenText { get; set; }
   }
}
