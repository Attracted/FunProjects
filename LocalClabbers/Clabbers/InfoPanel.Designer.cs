namespace Clabbers
{
	partial class InfoPanel
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.playerName = new System.Windows.Forms.Label();
			this.score = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.lastMove = new System.Windows.Forms.Label();
			this.moveHistory = new System.Windows.Forms.Button();
			this.lastScore = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// playerName
			// 
			this.playerName.AutoSize = true;
			this.playerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.playerName.Location = new System.Drawing.Point(4, 9);
			this.playerName.Name = "playerName";
			this.playerName.Size = new System.Drawing.Size(48, 18);
			this.playerName.TabIndex = 1;
			this.playerName.Text = "Opp1";
			// 
			// score
			// 
			this.score.AutoSize = true;
			this.score.Location = new System.Drawing.Point(69, 63);
			this.score.Name = "score";
			this.score.Size = new System.Drawing.Size(13, 13);
			this.score.TabIndex = 3;
			this.score.Text = "0";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 63);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(38, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Score:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(3, 76);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(60, 13);
			this.label4.TabIndex = 4;
			this.label4.Text = "Last Move:";
			// 
			// lastMove
			// 
			this.lastMove.AutoSize = true;
			this.lastMove.Location = new System.Drawing.Point(31, 89);
			this.lastMove.Name = "lastMove";
			this.lastMove.Size = new System.Drawing.Size(10, 13);
			this.lastMove.TabIndex = 5;
			this.lastMove.Text = "-";
			// 
			// moveHistory
			// 
			this.moveHistory.AutoSize = true;
			this.moveHistory.Location = new System.Drawing.Point(3, 124);
			this.moveHistory.Name = "moveHistory";
			this.moveHistory.Size = new System.Drawing.Size(90, 23);
			this.moveHistory.TabIndex = 6;
			this.moveHistory.Tag = this;
			this.moveHistory.Text = "Move History";
			this.moveHistory.UseVisualStyleBackColor = true;
			// 
			// lastScore
			// 
			this.lastScore.AutoSize = true;
			this.lastScore.Location = new System.Drawing.Point(69, 89);
			this.lastScore.Name = "lastScore";
			this.lastScore.Size = new System.Drawing.Size(13, 13);
			this.lastScore.TabIndex = 7;
			this.lastScore.Text = "0";
			// 
			// InfoPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this.lastScore);
			this.Controls.Add(this.moveHistory);
			this.Controls.Add(this.lastMove);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.score);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.playerName);
			this.Name = "InfoPanel";
			this.Size = new System.Drawing.Size(100, 150);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label playerName;
		private System.Windows.Forms.Label score;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lastMove;
		private System.Windows.Forms.Button moveHistory;
		private System.Windows.Forms.Label lastScore;
	}
}
