using System;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace TwinCars
{
   static class Program 
   {
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main() 
      {
         string resource1 = "TwinCars.TwinHook.dll";
         EmbeddedAssembly.Load(resource1, "TwinHook.dll");

         AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
         
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         Application.Run(new GameForm());
         //Application.Run(new TestFormComponent());
      }

      static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
      {
         return Load();
      }

      public static Assembly Load()
      {
         byte[] ba = null;
         string resource = "TwinCars.TwinHook.dll";
         Assembly curAsm = Assembly.GetExecutingAssembly();
         using (Stream stm = curAsm.GetManifestResourceStream(resource))
         {
            ba = new byte[(int)stm.Length];
            stm.Read(ba, 0, (int)stm.Length);

            return Assembly.Load(ba);
         }
      }
   }
}