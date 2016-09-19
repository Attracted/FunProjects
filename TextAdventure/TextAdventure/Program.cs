using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextAdventure
{
   public class Program
   {
      static void Main(string[] args)
      {
         var gameManager = new GameManager();

         do
         {
            Console.Clear();
            //Console.WriteLine();
            Console.WriteLine(gameManager.CurrentStateText);
            Console.Write(">");
            string r = Console.ReadLine();

            gameManager.HandleReadLine(r);
         } while (true);
      }
   }
}
