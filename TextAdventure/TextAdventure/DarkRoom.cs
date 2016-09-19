using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextAdventure
{
   public class DarkRoom : Scene
   {
      private Dictionary<GenericCommands, string> _CommandToText;
      public override Dictionary<GenericCommands, string> CommandToText { get { return _CommandToText; } }
      
      public Dictionary<StateTransition, GameState> _Transitions;
      public override Dictionary<StateTransition, GameState> Transitions { get { return _Transitions; } }

      public List<Item> _Items;
      public override List<Item> Items { get { return _Items; } }
      
      public DarkRoom()
      {
         _CommandToText = new Dictionary<GenericCommands, string>
         {   
            { GenericCommands.Default, SceneText },
            { GenericCommands.Help, HelpText },
            { GenericCommands.North, NorthText },
            { GenericCommands.East, EastText },
            { GenericCommands.South, SouthText },
            { GenericCommands.West, WestText },
            { GenericCommands.Look, LookText },
            { GenericCommands.Use, UseText },
         };

         _Transitions = new Dictionary<StateTransition, GameState>
         {   
            { new StateTransition(GameState.DarkRoom, GenericCommands.South), GameState.Cliffside },
            { new StateTransition(GameState.DarkRoom, GenericCommands.Inventory), GameState.Inventory },
         };

         _Items = new List<Item>
         {
            new Item("SHIMMER", "A light glimmers NORTH"),
            new Item("ROCK", "A shiny rock", true)
         };

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
            Item foundItem = _Items.FirstOrDefault(i => i.Name == modifiers[1].ToUpper());

            if (foundItem != null)
            {
               ScreenText = foundItem.Description;
            }
         }
         else if (command == GenericCommands.Get)
         {
            Item foundItem = _Items.FirstOrDefault(i => i.Name == modifiers[1].ToUpper());

            if (foundItem != null && foundItem.Tangible)
            {
               AddOrRemoveItem(foundItem, true);
               ScreenText = string.Format("{0} Get!", foundItem.Name);
               _Items.Remove(foundItem);
            }
         }
         else if (command == GenericCommands.Use)
         {
            //ScreenText = _Items.Find(i => i.Name == modifier).Description;
         }
      }

      public override GameState GameState
      {
         get { return GameState.DarkRoom; }
      }

      private string _ScreenText;
      public override string ScreenText
      {
         get { return _ScreenText; }
         set { _ScreenText = value; }
      }
      public string SceneText
      {
         get 
         { 
            return "You find yourelf in a dark room." + Environment.NewLine +
            "Faint wisps of light are scattered across the grime-coated gravel beneath your feet." + Environment.NewLine + 
            "A shiny ROCK sits next to you."; 
         }
      }
      public string HelpText
      {
         get { return ""; }
      }
      public string NorthText
      {
         get { return ""; }
      }
      public string EastText
      {
         get { return ""; }
      }
      public string SouthText
      {
         get { return ""; }
      }
      public string WestText
      {
         get { return ""; }
      }
      public string LookText
      {
         get
         {
            return "You look closer." + Environment.NewLine +
               "A tiny SHIMMER is echoing light in front of you.";
         }
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