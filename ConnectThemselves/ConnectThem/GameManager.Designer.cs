namespace ConnectThemselves
{
    partial class GameManager
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
         this.label1 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.redWins = new System.Windows.Forms.Label();
         this.blueWins = new System.Windows.Forms.Label();
         this.ties = new System.Windows.Forms.Label();
         this.label5 = new System.Windows.Forms.Label();
         this.gamePanel = new System.Windows.Forms.Panel();
         this.autoPlay = new System.Windows.Forms.CheckBox();
         this.nextMove = new System.Windows.Forms.Button();
         this.undoMove = new System.Windows.Forms.Button();
         this.autoSlider = new System.Windows.Forms.TrackBar();
         this.autoStop = new System.Windows.Forms.CheckBox();
         this.normalMode = new System.Windows.Forms.CheckBox();
         this.saveGame = new System.Windows.Forms.Button();
         ((System.ComponentModel.ISupportInitialize)(this.autoSlider)).BeginInit();
         this.SuspendLayout();
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label1.Location = new System.Drawing.Point(4, 9);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(38, 15);
         this.label1.TabIndex = 0;
         this.label1.Text = "Wins";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.label2.Location = new System.Drawing.Point(7, 62);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(33, 15);
         this.label2.TabIndex = 1;
         this.label2.Text = "Blue:";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.label3.Location = new System.Drawing.Point(7, 33);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(32, 15);
         this.label3.TabIndex = 2;
         this.label3.Text = "Red:";
         // 
         // redWins
         // 
         this.redWins.AutoSize = true;
         this.redWins.BackColor = System.Drawing.Color.Salmon;
         this.redWins.Location = new System.Drawing.Point(45, 35);
         this.redWins.Name = "redWins";
         this.redWins.Size = new System.Drawing.Size(13, 13);
         this.redWins.TabIndex = 3;
         this.redWins.Text = "0";
         // 
         // blueWins
         // 
         this.blueWins.AutoSize = true;
         this.blueWins.BackColor = System.Drawing.Color.SkyBlue;
         this.blueWins.Location = new System.Drawing.Point(46, 62);
         this.blueWins.Name = "blueWins";
         this.blueWins.Size = new System.Drawing.Size(13, 13);
         this.blueWins.TabIndex = 4;
         this.blueWins.Text = "0";
         // 
         // ties
         // 
         this.ties.AutoSize = true;
         this.ties.BackColor = System.Drawing.Color.LemonChiffon;
         this.ties.Location = new System.Drawing.Point(46, 89);
         this.ties.Name = "ties";
         this.ties.Size = new System.Drawing.Size(13, 13);
         this.ties.TabIndex = 6;
         this.ties.Text = "0";
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.label5.Location = new System.Drawing.Point(7, 89);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(27, 15);
         this.label5.TabIndex = 5;
         this.label5.Text = "Tie:";
         // 
         // gamePanel
         // 
         this.gamePanel.AutoSize = true;
         this.gamePanel.Location = new System.Drawing.Point(100, 39);
         this.gamePanel.Name = "gamePanel";
         this.gamePanel.Size = new System.Drawing.Size(90, 119);
         this.gamePanel.TabIndex = 7;
         // 
         // autoPlay
         // 
         this.autoPlay.AutoSize = true;
         this.autoPlay.Location = new System.Drawing.Point(10, 139);
         this.autoPlay.Name = "autoPlay";
         this.autoPlay.Size = new System.Drawing.Size(68, 17);
         this.autoPlay.TabIndex = 8;
         this.autoPlay.Text = "AutoPlay";
         this.autoPlay.UseVisualStyleBackColor = true;
         this.autoPlay.CheckedChanged += new System.EventHandler(this.autoPlay_CheckedChanged);
         // 
         // nextMove
         // 
         this.nextMove.Location = new System.Drawing.Point(49, 185);
         this.nextMove.Name = "nextMove";
         this.nextMove.Size = new System.Drawing.Size(30, 21);
         this.nextMove.TabIndex = 9;
         this.nextMove.Text = ">>";
         this.nextMove.UseVisualStyleBackColor = true;
         this.nextMove.Click += new System.EventHandler(this.nextMoveButton_Click);
         // 
         // undoMove
         // 
         this.undoMove.Location = new System.Drawing.Point(7, 185);
         this.undoMove.Name = "undoMove";
         this.undoMove.Size = new System.Drawing.Size(30, 21);
         this.undoMove.TabIndex = 10;
         this.undoMove.Text = "<<";
         this.undoMove.UseVisualStyleBackColor = true;
         this.undoMove.Click += new System.EventHandler(this.undoMoveButton_Click);
         // 
         // autoSlider
         // 
         this.autoSlider.Location = new System.Drawing.Point(7, 212);
         this.autoSlider.Maximum = 1000;
         this.autoSlider.Minimum = 1;
         this.autoSlider.Name = "autoSlider";
         this.autoSlider.Size = new System.Drawing.Size(72, 45);
         this.autoSlider.TabIndex = 11;
         this.autoSlider.TickStyle = System.Windows.Forms.TickStyle.None;
         this.autoSlider.Value = 1;
         this.autoSlider.Scroll += new System.EventHandler(this.autoSlider_Scroll);
         // 
         // autoStop
         // 
         this.autoStop.AutoSize = true;
         this.autoStop.Location = new System.Drawing.Point(10, 162);
         this.autoStop.Name = "autoStop";
         this.autoStop.Size = new System.Drawing.Size(70, 17);
         this.autoStop.TabIndex = 12;
         this.autoStop.Text = "AutoStop";
         this.autoStop.UseVisualStyleBackColor = true;
         this.autoStop.CheckedChanged += new System.EventHandler(this.autoStop_CheckedChanged);
         // 
         // normalMode
         // 
         this.normalMode.AutoSize = true;
         this.normalMode.Location = new System.Drawing.Point(10, 116);
         this.normalMode.Name = "normalMode";
         this.normalMode.Size = new System.Drawing.Size(86, 17);
         this.normalMode.TabIndex = 13;
         this.normalMode.Text = "NormalMode";
         this.normalMode.UseVisualStyleBackColor = true;
         this.normalMode.CheckedChanged += new System.EventHandler(this.normalMode_CheckedChanged);
         // 
         // saveGame
         // 
         this.saveGame.Location = new System.Drawing.Point(100, 12);
         this.saveGame.Name = "saveGame";
         this.saveGame.Size = new System.Drawing.Size(90, 21);
         this.saveGame.TabIndex = 14;
         this.saveGame.Text = "Save Game";
         this.saveGame.UseVisualStyleBackColor = true;
         this.saveGame.Click += new System.EventHandler(this.saveGame_Click);
         // 
         // GameManager
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.AutoSize = true;
         this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
         this.ClientSize = new System.Drawing.Size(202, 243);
         this.Controls.Add(this.saveGame);
         this.Controls.Add(this.normalMode);
         this.Controls.Add(this.autoStop);
         this.Controls.Add(this.autoSlider);
         this.Controls.Add(this.undoMove);
         this.Controls.Add(this.nextMove);
         this.Controls.Add(this.autoPlay);
         this.Controls.Add(this.gamePanel);
         this.Controls.Add(this.ties);
         this.Controls.Add(this.label5);
         this.Controls.Add(this.blueWins);
         this.Controls.Add(this.redWins);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.label1);
         this.Name = "GameManager";
         this.Text = "ConnectThemselves";
         this.Load += new System.EventHandler(this.Form2_Load);
         ((System.ComponentModel.ISupportInitialize)(this.autoSlider)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label redWins;
        private System.Windows.Forms.Label blueWins;
        private System.Windows.Forms.Label ties;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel gamePanel;
        private System.Windows.Forms.CheckBox autoPlay;
        private System.Windows.Forms.Button nextMove;
        private System.Windows.Forms.Button undoMove;
        private System.Windows.Forms.TrackBar autoSlider;
        private System.Windows.Forms.CheckBox autoStop;
        private System.Windows.Forms.CheckBox normalMode;
        private System.Windows.Forms.Button saveGame;
    }
}