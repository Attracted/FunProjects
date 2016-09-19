namespace Clabbers
{
	partial class CellInfoForm
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
         this.image = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.label5 = new System.Windows.Forms.Label();
         this.label6 = new System.Windows.Forms.Label();
         this.label7 = new System.Windows.Forms.Label();
         this.row = new System.Windows.Forms.Label();
         this.col = new System.Windows.Forms.Label();
         this.letter = new System.Windows.Forms.Label();
         this.score = new System.Windows.Forms.Label();
         this.used = new System.Windows.Forms.Label();
         this.label13 = new System.Windows.Forms.Label();
         this.label14 = new System.Windows.Forms.Label();
         this.tileType = new System.Windows.Forms.Label();
         this.SuspendLayout();
         // 
         // image
         // 
         this.image.BackColor = System.Drawing.Color.BurlyWood;
         this.image.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.image.Location = new System.Drawing.Point(50, 9);
         this.image.Name = "image";
         this.image.Size = new System.Drawing.Size(30, 30);
         this.image.TabIndex = 0;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(12, 57);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(32, 13);
         this.label2.TabIndex = 1;
         this.label2.Text = "Row:";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(12, 70);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(25, 13);
         this.label3.TabIndex = 2;
         this.label3.Text = "Col:";
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(12, 83);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(37, 13);
         this.label5.TabIndex = 4;
         this.label5.Text = "Letter:";
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(12, 96);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(38, 13);
         this.label6.TabIndex = 5;
         this.label6.Text = "Score:";
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.Location = new System.Drawing.Point(12, 109);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(35, 13);
         this.label7.TabIndex = 6;
         this.label7.Text = "Used:";
         // 
         // row
         // 
         this.row.AutoSize = true;
         this.row.Location = new System.Drawing.Point(67, 57);
         this.row.Name = "row";
         this.row.Size = new System.Drawing.Size(10, 13);
         this.row.TabIndex = 7;
         this.row.Text = "-";
         // 
         // col
         // 
         this.col.AutoSize = true;
         this.col.Location = new System.Drawing.Point(67, 70);
         this.col.Name = "col";
         this.col.Size = new System.Drawing.Size(10, 13);
         this.col.TabIndex = 8;
         this.col.Text = "-";
         // 
         // letter
         // 
         this.letter.AutoSize = true;
         this.letter.Location = new System.Drawing.Point(67, 83);
         this.letter.Name = "letter";
         this.letter.Size = new System.Drawing.Size(10, 13);
         this.letter.TabIndex = 9;
         this.letter.Text = "-";
         // 
         // score
         // 
         this.score.AutoSize = true;
         this.score.Location = new System.Drawing.Point(68, 96);
         this.score.Name = "score";
         this.score.Size = new System.Drawing.Size(10, 13);
         this.score.TabIndex = 10;
         this.score.Text = "-";
         // 
         // used
         // 
         this.used.AutoSize = true;
         this.used.Location = new System.Drawing.Point(69, 109);
         this.used.Name = "used";
         this.used.Size = new System.Drawing.Size(10, 13);
         this.used.TabIndex = 11;
         this.used.Text = "-";
         // 
         // label13
         // 
         this.label13.AutoSize = true;
         this.label13.Location = new System.Drawing.Point(12, 26);
         this.label13.Name = "label13";
         this.label13.Size = new System.Drawing.Size(36, 13);
         this.label13.TabIndex = 12;
         this.label13.Text = "Label:";
         // 
         // label14
         // 
         this.label14.AutoSize = true;
         this.label14.Location = new System.Drawing.Point(12, 122);
         this.label14.Name = "label14";
         this.label14.Size = new System.Drawing.Size(51, 13);
         this.label14.TabIndex = 13;
         this.label14.Text = "TileType:";
         // 
         // tileType
         // 
         this.tileType.AutoSize = true;
         this.tileType.Location = new System.Drawing.Point(69, 122);
         this.tileType.Name = "tileType";
         this.tileType.Size = new System.Drawing.Size(10, 13);
         this.tileType.TabIndex = 14;
         this.tileType.Text = "-";
         // 
         // CellInfoForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.AutoSize = true;
         this.BackColor = System.Drawing.SystemColors.ControlLight;
         this.ClientSize = new System.Drawing.Size(117, 149);
         this.Controls.Add(this.tileType);
         this.Controls.Add(this.label14);
         this.Controls.Add(this.label13);
         this.Controls.Add(this.used);
         this.Controls.Add(this.score);
         this.Controls.Add(this.letter);
         this.Controls.Add(this.col);
         this.Controls.Add(this.row);
         this.Controls.Add(this.label7);
         this.Controls.Add(this.label6);
         this.Controls.Add(this.label5);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.image);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "CellInfoForm";
         this.Text = "Cell Info";
         this.Load += new System.EventHandler(this.Form3_Load);
         this.ResumeLayout(false);
         this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label image;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label row;
		private System.Windows.Forms.Label col;
		private System.Windows.Forms.Label letter;
		private System.Windows.Forms.Label score;
		private System.Windows.Forms.Label used;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
      private System.Windows.Forms.Label tileType;
	}
}