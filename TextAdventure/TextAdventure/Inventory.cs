using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextAdventure
{
   public class Inventory : Scene
   {
      private Dictionary<GenericCommands, string> _CommandToText;
      public override Dictionary<GenericCommands, string> CommandToText { get { return _CommandToText; } }

      public Dictionary<StateTransition, GameState> _Transitions;
      public override Dictionary<StateTransition, GameState> Transitions { get { return _Transitions; } }

      public Bag GameBag;
      public override List<Item> Items { get { return GameBag; } }

      public GameState PreviousGameState { get; set; }

      public Inventory()
      {
         _CommandToText = new Dictionary<GenericCommands, string>
         {   
            { GenericCommands.Default, SceneText },
            { GenericCommands.Help, HelpText },
            { GenericCommands.Look, LookText },
            { GenericCommands.Use, UseText },
         };

         _Transitions = new Dictionary<StateTransition, GameState>
         {   
            { new StateTransition(GameState.Inventory, GenericCommands.Back), PreviousGameState },
         };

         GameBag = new Bag();

         ScreenText = SceneText;

      }
      public override void DoCommand(GenericCommands command)
      {
         base.DoCommand(command);
      }
      public override void DoCommand(GenericCommands command, List<string> modifiers)
      {
         if (modifiers == null || modifiers.Count < 2)
         {
            DoCommand(command);
         }
         else if (command == GenericCommands.Look)
         {

         }
         else if (command == GenericCommands.Use)
         {
            //ScreenText = _Items.Find(i => i.Name == modifier).Description;
         }
      }

      public override GameState GameState
      {
         get { return GameState.Inventory; }
      }

      public override string ScreenText
      {
         get 
         {
            string text = SceneText;
            foreach (Item item in GameBag)
            {
               text += Environment.NewLine + string.Format("{0} - {1}", item.Name, item.Description);
            }

            return text; 
         }
         set { }
      }
      public string SceneText
      {
         get
         {
            return "Inventory: ";
         }
      }
      public string HelpText
      {
         get { return ""; }
      }
      public string LookText
      {
         get { return ""; }
      }
      public string UseText
      {
         get { return ""; }
      }
      public string GetText
      {
         get { return ""; }
      }
   }
}