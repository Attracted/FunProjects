using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextAdventure
{
   public class Intro : Scene
   {
      private Dictionary<GenericCommands, string> _CommandToText;
      public override Dictionary<GenericCommands, string> CommandToText { get { return _CommandToText; } }
      
      public Dictionary<StateTransition, GameState> _Transitions;
      public override Dictionary<StateTransition, GameState> Transitions { get { return _Transitions; } }

      public List<Item> _Items;
      public override List<Item> Items { get { return _Items; } }

      public Intro()
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
            { new StateTransition(GameState.Intro, GenericCommands.Look), GameState.DarkRoom },
         };

         _Items = new List<Item>
         {
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
                  
         }
         else if (command == GenericCommands.Look)
         {

         }
         else if (command == GenericCommands.Get)
         {

         }
         else if (command == GenericCommands.Use)
         {

         }
      }

      public override GameState GameState
      {
         get { return GameState.Intro; }
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
            return "Welcome to TextAdventure" + Environment.NewLine +
               "LOOK around to start your adventure!";
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