using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextAdventure
{
   public enum GameState
   {
      Intro,
      Inventory,
      DarkRoom,
      Cliffside,
   }

   public delegate void BagEventsHandler(object sender, BagEventArgs e);

   public class BagEventArgs : EventArgs
   {
      public Item BagItem;
      public bool AddItem;

      public BagEventArgs()
      {
      }

      public BagEventArgs(Item item, bool addItem)
      {
         BagItem = item;
         AddItem = addItem;
      }
   }

   public class GameManager
   {      
      Dictionary<GameState, Scene> ScenesInScope = new Dictionary<GameState, Scene>();

      public string CurrentStateText { get; private set; }
      public GameState CurrentGameState { get; private set; }

      Inventory InventoryScene;

      public GameManager()
      {
         CurrentGameState = GameState.Intro;
         Intro intro = new Intro();
         CurrentStateText = intro.SceneText;

         ScenesInScope.Add(GameState.Intro, intro);

         InventoryScene = new Inventory();
         intro.OnBagContentsChange += InventoryScene.GameBag.OnAddOrRemoveItem;

         ScenesInScope.Add(GameState.Inventory, InventoryScene);
      }

      public void HandleReadLine(string line)
      {
         Scene currentScene = ScenesInScope[CurrentGameState];

         GenericCommands command = GenericCommands.Default;
         
         List<string> modifiers = SplitLine(line);

         if (modifiers == null || modifiers.Count < 2)
         {
            command = GenericInputDecoder.Decode(line);
         }
         else
         {
            command = GenericInputDecoder.Decode(modifiers[0]);
         }      
         
         if (MoveToNextGameState(line, currentScene, command))
         {
            switch (CurrentGameState)
            {
               case GameState.DarkRoom:
                  if (!ScenesInScope.ContainsKey(GameState.DarkRoom))
                  {
                     DarkRoom darkroom = new DarkRoom();
                     ScenesInScope.Add(GameState.DarkRoom, darkroom);
                     darkroom.OnBagContentsChange += InventoryScene.GameBag.OnAddOrRemoveItem;
                  }
                  break;
               case GameState.Cliffside:
                  if (!ScenesInScope.ContainsKey(GameState.Cliffside))
                  {
                     Cliffside cliffside = new Cliffside();
                     ScenesInScope.Add(GameState.Cliffside, cliffside);
                     cliffside.OnBagContentsChange += InventoryScene.GameBag.OnAddOrRemoveItem;
                  }
                  break;
               default:
                  break;
            }

            currentScene = ScenesInScope[CurrentGameState];
            CurrentStateText = currentScene.ScreenText;
         }
         else if (command == GenericCommands.Inventory)
         {
            InventoryScene.DoCommand(command, modifiers);
            CurrentStateText = InventoryScene.ScreenText;
         }
         else
         {
            currentScene.DoCommand(command, modifiers);
            CurrentStateText = currentScene.ScreenText;
         }
      }

      private List<string> SplitLine(string line)
      {
         List<string> ret = null;

         ret = SplitLine(line, ret, "look", new string[] { "at", "a", "an", "the", "towards" });
         if (ret != null) 
         {
            return ret; 
         }
         
         ret = SplitLine(line, ret, "get", new string[] { "the", "a", "an" });
         if (ret != null) 
         {
            return ret; 
         }
         
         ret = SplitLine(line, ret, "grab", new string[] { "the", "a", "an" });
         if (ret != null) 
         {
            return ret; 
         }

         ret = SplitLine(line, ret, "take", new string[] { "the", "a", "an" });
         if (ret != null) 
         {
            return ret; 
         }

         ret = SplitLine(line, ret, "use", new string[] { "the", "a", "an" });
         if (ret != null)
         {
            return ret;
         }

         return null;
      }

      private List<string> SplitLine(string line, List<string> ret, string splitWord, params string[] avoidWords)
      {
         List<string> temp = null;
         
         if (line.StartsWith(splitWord))
         {
            temp = new List<string>();
            
            string[] splitString = line.Split(' ');

            for (int i = 0; i < splitString.Length; i++)
            {
               string word = splitString[i];

               bool addWord = true;
               for (int j = 0; j < avoidWords.Length; j++)
               {
                  if (word == avoidWords[i])
                  {
                     addWord = false;
                     break;
                  }
               }

               //if (addWord && word == splitWord)
               //{
               //   addWord = false;
               //}

               if (addWord)
               {
                  temp.Add(word);
               }
            }
         }

         if (temp != null)
         {
            return temp;
         }

         return ret;
      }

      private bool MoveToNextGameState(string line, Scene currentScene, GenericCommands command)
      {
         bool transitioned = false;

         StateTransition transition = new StateTransition(CurrentGameState, command);
         GameState nextState;
         if (currentScene.Transitions.TryGetValue(transition, out nextState))
         {
            InventoryScene.PreviousGameState = CurrentGameState;
            CurrentGameState = nextState;
            transitioned = true;
         }

         return transitioned;
      }
   }
}
