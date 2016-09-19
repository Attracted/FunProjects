using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SimpleMtgSim
{
   public delegate void TutorTargetHandler(object sender, TutorEventArgs e);

   public partial class TutorControl : Form
   {
      public event TutorTargetHandler OnSubmitTarget;
      
      public TutorControl()
      {
         InitializeComponent();
         this.targetsList.MouseDoubleClick += new MouseEventHandler(targetsList_MouseDoubleClick);
      }

      public void AddItem(string tutorTarget)
      {
         this.targetsList.Items.Add(tutorTarget);
      }

      public void RemoveItem(string tutorTarget)
      {
         this.targetsList.Items.Remove(tutorTarget);
      }

      private void select_Click(object sender, EventArgs e)
      {
         SubmitTarget(sender);
      }

      private void close_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      private void targetsList_MouseDoubleClick(object sender, MouseEventArgs e)
      {
         SubmitTarget(sender);
      }

      private void SubmitTarget(object sender)
      {
         if (OnSubmitTarget == null || this.targetsList.SelectedItem == null)
         {
            return;
         }


         string tutorTarget = (string)this.targetsList.SelectedItem;

         TutorEventArgs args = new TutorEventArgs(tutorTarget);

         OnSubmitTarget(sender, args);
      }
   }

   public class TutorEventArgs : EventArgs
   {
      public string TutorTarget;

      public TutorEventArgs()
      {
      }

      public TutorEventArgs(string tutorTarget)
      {
         TutorTarget = tutorTarget;
      }
   }
}
