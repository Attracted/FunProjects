namespace TwinCars
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
         this.banner = new System.Windows.Forms.Label();
         this.startBar = new System.Windows.Forms.Button();
         this.label1 = new System.Windows.Forms.Label();
         this.score = new System.Windows.Forms.Label();
         this.SuspendLayout();
         // 
         // banner
         // 
         this.banner.AutoSize = true;
         this.banner.Font = new System.Drawing.Font("Segoe Print", 32F, System.Drawing.FontStyle.Bold);
         this.banner.ForeColor = System.Drawing.SystemColors.ControlLight;
         this.banner.Location = new System.Drawing.Point(-1, 0);
         this.banner.Name = "banner";
         this.banner.Size = new System.Drawing.Size(235, 76);
         this.banner.TabIndex = 0;
         this.banner.Text = "TwinCars";
         // 
         // startBar
         // 
         this.startBar.BackColor = System.Drawing.Color.DimGray;
         this.startBar.ForeColor = System.Drawing.SystemColors.ControlLight;
         this.startBar.Location = new System.Drawing.Point(12, 449);
         this.startBar.Name = "startBar";
         this.startBar.Size = new System.Drawing.Size(206, 40);
         this.startBar.TabIndex = 1;
         this.startBar.Text = "Start";
         this.startBar.UseVisualStyleBackColor = false;
         this.startBar.Click += new System.EventHandler(this.startBar_Click);
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label1.ForeColor = System.Drawing.SystemColors.ControlLight;
         this.label1.Location = new System.Drawing.Point(9, 67);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(72, 18);
         this.label1.TabIndex = 6;
         this.label1.Text = "SCORE:";
         // 
         // score
         // 
         this.score.AutoSize = true;
         this.score.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.score.ForeColor = System.Drawing.SystemColors.ControlLight;
         this.score.Location = new System.Drawing.Point(87, 67);
         this.score.Name = "score";
         this.score.Size = new System.Drawing.Size(17, 18);
         this.score.TabIndex = 7;
         this.score.Text = "0";
         // 
         // GameForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.Black;
         this.ClientSize = new System.Drawing.Size(232, 501);
         this.Controls.Add(this.score);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.startBar);
         this.Controls.Add(this.banner);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.Name = "GameForm";
         this.Text = "KeyboardHero!";
         this.Load += new System.EventHandler(this.GameForm_Load);
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label banner;
        private System.Windows.Forms.Button startBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label score;

    }
}