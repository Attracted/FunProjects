namespace Gma.UserActivityMonitorDemo
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
         this.banner = new System.Windows.Forms.Label();
         this.startBar = new System.Windows.Forms.Button();
         this.h_label = new System.Windows.Forms.Label();
         this.j_label = new System.Windows.Forms.Label();
         this.k_label = new System.Windows.Forms.Label();
         this.l_label = new System.Windows.Forms.Label();
         this.scorePanel = new System.Windows.Forms.Panel();
         this.scoreBar = new System.Windows.Forms.Label();
         this.speedText = new System.Windows.Forms.Label();
         this.speedSlider = new System.Windows.Forms.TrackBar();
         this.scorePanel.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.speedSlider)).BeginInit();
         this.SuspendLayout();
         // 
         // banner
         // 
         this.banner.AutoSize = true;
         this.banner.Font = new System.Drawing.Font("Segoe Print", 36F, System.Drawing.FontStyle.Bold);
         this.banner.Location = new System.Drawing.Point(1, 9);
         this.banner.Name = "banner";
         this.banner.Size = new System.Drawing.Size(380, 85);
         this.banner.TabIndex = 0;
         this.banner.Text = "KeyboardHero";
         // 
         // startBar
         // 
         this.startBar.Location = new System.Drawing.Point(16, 148);
         this.startBar.Name = "startBar";
         this.startBar.Size = new System.Drawing.Size(259, 40);
         this.startBar.TabIndex = 1;
         this.startBar.Text = "Start";
         this.startBar.UseVisualStyleBackColor = true;
         this.startBar.Click += new System.EventHandler(this.start_Click_1);
         // 
         // h_label
         // 
         this.h_label.BackColor = System.Drawing.Color.DarkRed;
         this.h_label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.h_label.Font = new System.Drawing.Font("Segoe UI Symbol", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.h_label.Location = new System.Drawing.Point(0, 600);
         this.h_label.Margin = new System.Windows.Forms.Padding(0);
         this.h_label.Name = "h_label";
         this.h_label.Size = new System.Drawing.Size(60, 60);
         this.h_label.TabIndex = 2;
         this.h_label.Text = "H";
         this.h_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // j_label
         // 
         this.j_label.BackColor = System.Drawing.Color.DarkGreen;
         this.j_label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.j_label.Font = new System.Drawing.Font("Segoe UI Symbol", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.j_label.Location = new System.Drawing.Point(70, 600);
         this.j_label.Margin = new System.Windows.Forms.Padding(0);
         this.j_label.Name = "j_label";
         this.j_label.Size = new System.Drawing.Size(60, 60);
         this.j_label.TabIndex = 3;
         this.j_label.Text = "J";
         this.j_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // k_label
         // 
         this.k_label.BackColor = System.Drawing.Color.DarkBlue;
         this.k_label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.k_label.Font = new System.Drawing.Font("Segoe UI Symbol", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.k_label.Location = new System.Drawing.Point(140, 600);
         this.k_label.Margin = new System.Windows.Forms.Padding(0);
         this.k_label.Name = "k_label";
         this.k_label.Size = new System.Drawing.Size(60, 60);
         this.k_label.TabIndex = 4;
         this.k_label.Text = "K";
         this.k_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // l_label
         // 
         this.l_label.BackColor = System.Drawing.Color.DarkOrange;
         this.l_label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.l_label.Font = new System.Drawing.Font("Segoe UI Symbol", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.l_label.Location = new System.Drawing.Point(210, 600);
         this.l_label.Margin = new System.Windows.Forms.Padding(0);
         this.l_label.Name = "l_label";
         this.l_label.Size = new System.Drawing.Size(60, 60);
         this.l_label.TabIndex = 5;
         this.l_label.Text = "L";
         this.l_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // scorePanel
         // 
         this.scorePanel.BackColor = System.Drawing.SystemColors.ControlDark;
         this.scorePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.scorePanel.Controls.Add(this.scoreBar);
         this.scorePanel.Location = new System.Drawing.Point(281, 97);
         this.scorePanel.Name = "scorePanel";
         this.scorePanel.Size = new System.Drawing.Size(72, 580);
         this.scorePanel.TabIndex = 6;
         // 
         // scoreBar
         // 
         this.scoreBar.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
         this.scoreBar.ForeColor = System.Drawing.SystemColors.ControlText;
         this.scoreBar.Location = new System.Drawing.Point(-2, 646);
         this.scoreBar.Name = "scoreBar";
         this.scoreBar.Size = new System.Drawing.Size(72, 10);
         this.scoreBar.TabIndex = 7;
         // 
         // speedText
         // 
         this.speedText.AutoSize = true;
         this.speedText.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.speedText.Location = new System.Drawing.Point(20, 97);
         this.speedText.Name = "speedText";
         this.speedText.Size = new System.Drawing.Size(60, 18);
         this.speedText.TabIndex = 8;
         this.speedText.Text = "Speed:";
         // 
         // speedSlider
         // 
         this.speedSlider.BackColor = System.Drawing.SystemColors.ButtonShadow;
         this.speedSlider.Location = new System.Drawing.Point(86, 97);
         this.speedSlider.Maximum = 15;
         this.speedSlider.Minimum = 5;
         this.speedSlider.Name = "speedSlider";
         this.speedSlider.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.speedSlider.Size = new System.Drawing.Size(184, 45);
         this.speedSlider.TabIndex = 9;
         this.speedSlider.Value = 10;
         // 
         // GameForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(365, 681);
         this.Controls.Add(this.speedSlider);
         this.Controls.Add(this.speedText);
         this.Controls.Add(this.scorePanel);
         this.Controls.Add(this.l_label);
         this.Controls.Add(this.k_label);
         this.Controls.Add(this.j_label);
         this.Controls.Add(this.h_label);
         this.Controls.Add(this.startBar);
         this.Controls.Add(this.banner);
         this.Name = "GameForm";
         this.Text = "KeyboardHero!";
         this.Load += new System.EventHandler(this.Form1_Load);
         this.scorePanel.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.speedSlider)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label banner;
        private System.Windows.Forms.Button startBar;
        private System.Windows.Forms.Label h_label;
        private System.Windows.Forms.Label j_label;
        private System.Windows.Forms.Label k_label;
        private System.Windows.Forms.Label l_label;
        private System.Windows.Forms.Panel scorePanel;
        private System.Windows.Forms.Label scoreBar;
        private System.Windows.Forms.Label speedText;
        private System.Windows.Forms.TrackBar speedSlider;

    }
}