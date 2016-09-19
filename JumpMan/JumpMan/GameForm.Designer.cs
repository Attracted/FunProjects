namespace JumpMan
{
    partial class GameForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameForm));
         this.startBar = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // startBar
         // 
         this.startBar.BackColor = System.Drawing.Color.DimGray;
         this.startBar.ForeColor = System.Drawing.SystemColors.ControlLight;
         this.startBar.Location = new System.Drawing.Point(168, 83);
         this.startBar.Name = "startBar";
         this.startBar.Size = new System.Drawing.Size(150, 40);
         this.startBar.TabIndex = 1;
         this.startBar.Text = "Start";
         this.startBar.UseVisualStyleBackColor = false;
         this.startBar.Click += new System.EventHandler(this.startBar_Click);
         // 
         // GameForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.Black;
         this.ClientSize = new System.Drawing.Size(494, 197);
         this.Controls.Add(this.startBar);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.Name = "GameForm";
         this.ShowIcon = false;
         this.Text = "JumpMan!";
         this.Load += new System.EventHandler(this.GameForm_Load);
         this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button startBar;

    }
}