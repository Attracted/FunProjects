namespace Clabbers
{
	partial class StartForm
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
			this.submit = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.mapTextBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.tilesTextBox = new System.Windows.Forms.TextBox();
			this.handSizeTextBox = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.numOppTextBox = new System.Windows.Forms.TextBox();
			this.wordsTextBox = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.loadGame = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe Print", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(252, 85);
			this.label1.TabIndex = 0;
			this.label1.Text = "Clabbers!";
			// 
			// submit
			// 
			this.submit.Location = new System.Drawing.Point(27, 293);
			this.submit.Name = "submit";
			this.submit.Size = new System.Drawing.Size(100, 35);
			this.submit.TabIndex = 1;
			this.submit.Text = "Submit";
			this.submit.UseVisualStyleBackColor = true;
			this.submit.Click += new System.EventHandler(this.submit_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(24, 94);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(91, 13);
			this.label3.TabIndex = 3;
			this.label3.Text = "Map File Location";
			// 
			// mapTextBox
			// 
			this.mapTextBox.Location = new System.Drawing.Point(27, 110);
			this.mapTextBox.Name = "mapTextBox";
			this.mapTextBox.Size = new System.Drawing.Size(212, 20);
			this.mapTextBox.TabIndex = 4;
			this.mapTextBox.Text = "Use Default";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(24, 211);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(103, 13);
			this.label2.TabIndex = 10;
			this.label2.Text = "Maximum Hand Size";
			// 
			// tilesTextBox
			// 
			this.tilesTextBox.Location = new System.Drawing.Point(27, 150);
			this.tilesTextBox.Name = "tilesTextBox";
			this.tilesTextBox.Size = new System.Drawing.Size(212, 20);
			this.tilesTextBox.TabIndex = 9;
			this.tilesTextBox.Text = "Use Default";
			// 
			// handSizeTextBox
			// 
			this.handSizeTextBox.Location = new System.Drawing.Point(26, 227);
			this.handSizeTextBox.Name = "handSizeTextBox";
			this.handSizeTextBox.Size = new System.Drawing.Size(213, 20);
			this.handSizeTextBox.TabIndex = 8;
			this.handSizeTextBox.Text = "7";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(24, 134);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(92, 13);
			this.label5.TabIndex = 7;
			this.label5.Text = "Tiles File Location";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(24, 250);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(81, 13);
			this.label6.TabIndex = 12;
			this.label6.Text = "# of Opponents";
			// 
			// numOppTextBox
			// 
			this.numOppTextBox.Location = new System.Drawing.Point(27, 266);
			this.numOppTextBox.Name = "numOppTextBox";
			this.numOppTextBox.Size = new System.Drawing.Size(212, 20);
			this.numOppTextBox.TabIndex = 11;
			this.numOppTextBox.Text = "0";
			// 
			// wordsTextBox
			// 
			this.wordsTextBox.Location = new System.Drawing.Point(27, 189);
			this.wordsTextBox.Name = "wordsTextBox";
			this.wordsTextBox.Size = new System.Drawing.Size(212, 20);
			this.wordsTextBox.TabIndex = 14;
			this.wordsTextBox.Text = "Use Default";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(24, 173);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(101, 13);
			this.label4.TabIndex = 13;
			this.label4.Text = "Words File Location";
			// 
			// loadGame
			// 
			this.loadGame.Location = new System.Drawing.Point(139, 292);
			this.loadGame.Name = "loadGame";
			this.loadGame.Size = new System.Drawing.Size(100, 35);
			this.loadGame.TabIndex = 15;
			this.loadGame.Text = "Load Game";
			this.loadGame.UseVisualStyleBackColor = true;
			this.loadGame.Click += new System.EventHandler(this.loadGame_Click);
			// 
			// StartForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(274, 350);
			this.Controls.Add(this.loadGame);
			this.Controls.Add(this.wordsTextBox);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.numOppTextBox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.tilesTextBox);
			this.Controls.Add(this.handSizeTextBox);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.mapTextBox);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.submit);
			this.Controls.Add(this.label1);
			this.Name = "StartForm";
			this.Text = "Clabbers!";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button submit;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox mapTextBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tilesTextBox;
		private System.Windows.Forms.TextBox handSizeTextBox;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox numOppTextBox;
		private System.Windows.Forms.TextBox wordsTextBox;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button loadGame;
	}
}

