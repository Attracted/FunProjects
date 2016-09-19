namespace SimpleMtgSim
{
   partial class TutorControl
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.tutorTargets = new System.Windows.Forms.GroupBox();
         this.close = new System.Windows.Forms.Button();
         this.select = new System.Windows.Forms.Button();
         this.targetsList = new System.Windows.Forms.ListBox();
         this.tutorTargets.SuspendLayout();
         this.SuspendLayout();
         // 
         // tutorTargets
         // 
         this.tutorTargets.Controls.Add(this.close);
         this.tutorTargets.Controls.Add(this.select);
         this.tutorTargets.Controls.Add(this.targetsList);
         this.tutorTargets.Location = new System.Drawing.Point(0, 12);
         this.tutorTargets.Name = "tutorTargets";
         this.tutorTargets.Size = new System.Drawing.Size(286, 218);
         this.tutorTargets.TabIndex = 9;
         this.tutorTargets.TabStop = false;
         this.tutorTargets.Text = "tutorTargets";
         // 
         // close
         // 
         this.close.Location = new System.Drawing.Point(146, 172);
         this.close.Name = "close";
         this.close.Size = new System.Drawing.Size(132, 37);
         this.close.TabIndex = 9;
         this.close.Text = "Close!";
         this.close.UseVisualStyleBackColor = true;
         this.close.Click += new System.EventHandler(this.close_Click);
         // 
         // select
         // 
         this.select.Location = new System.Drawing.Point(6, 172);
         this.select.Name = "select";
         this.select.Size = new System.Drawing.Size(134, 37);
         this.select.TabIndex = 8;
         this.select.Text = "Select!";
         this.select.UseVisualStyleBackColor = true;
         this.select.Click += new System.EventHandler(this.select_Click);
         // 
         // targetsList
         // 
         this.targetsList.FormattingEnabled = true;
         this.targetsList.Location = new System.Drawing.Point(6, 19);
         this.targetsList.Name = "targetsList";
         this.targetsList.Size = new System.Drawing.Size(272, 147);
         this.targetsList.TabIndex = 7;
         // 
         // TutorControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(289, 240);
         this.Controls.Add(this.tutorTargets);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
         this.Name = "TutorControl";
         this.Text = "TutorControl";
         this.tutorTargets.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.GroupBox tutorTargets;
      private System.Windows.Forms.Button select;
      private System.Windows.Forms.ListBox targetsList;
      private System.Windows.Forms.Button close;
   }
}