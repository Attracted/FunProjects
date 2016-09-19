namespace ConnectThemselves
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
         this.button1 = new System.Windows.Forms.Button();
         this.rowsTextBox = new System.Windows.Forms.TextBox();
         this.colsTextBox = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.winTextBox = new System.Windows.Forms.TextBox();
         this.label4 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.label5 = new System.Windows.Forms.Label();
         this.withLogging = new System.Windows.Forms.Button();
         this.label7 = new System.Windows.Forms.Label();
         this.maxGamesTextBox = new System.Windows.Forms.TextBox();
         this.loadGame = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // button1
         // 
         this.button1.Location = new System.Drawing.Point(25, 163);
         this.button1.Name = "button1";
         this.button1.Size = new System.Drawing.Size(115, 44);
         this.button1.TabIndex = 0;
         this.button1.Text = "Submit";
         this.button1.UseVisualStyleBackColor = true;
         this.button1.Click += new System.EventHandler(this.button1_Click);
         // 
         // rowsTextBox
         // 
         this.rowsTextBox.Location = new System.Drawing.Point(133, 57);
         this.rowsTextBox.Name = "rowsTextBox";
         this.rowsTextBox.Size = new System.Drawing.Size(38, 20);
         this.rowsTextBox.TabIndex = 2;
         this.rowsTextBox.Text = "6";
         // 
         // colsTextBox
         // 
         this.colsTextBox.Location = new System.Drawing.Point(133, 83);
         this.colsTextBox.Name = "colsTextBox";
         this.colsTextBox.Size = new System.Drawing.Size(38, 20);
         this.colsTextBox.TabIndex = 3;
         this.colsTextBox.Text = "7";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(77, 86);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(50, 13);
         this.label2.TabIndex = 4;
         this.label2.Text = "Columns:";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(90, 60);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(37, 13);
         this.label3.TabIndex = 5;
         this.label3.Text = "Rows:";
         // 
         // winTextBox
         // 
         this.winTextBox.Location = new System.Drawing.Point(133, 109);
         this.winTextBox.Name = "winTextBox";
         this.winTextBox.Size = new System.Drawing.Size(37, 20);
         this.winTextBox.TabIndex = 6;
         this.winTextBox.Text = "4";
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(51, 112);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(76, 13);
         this.label4.TabIndex = 7;
         this.label4.Text = "Win Condition:";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(176, 112);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(44, 13);
         this.label1.TabIndex = 8;
         this.label1.Text = "in a row";
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Font = new System.Drawing.Font("Harlow Solid Italic", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label5.Location = new System.Drawing.Point(30, 24);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(218, 30);
         this.label5.TabIndex = 9;
         this.label5.Text = "Connect Themselves";
         // 
         // withLogging
         // 
         this.withLogging.Location = new System.Drawing.Point(25, 213);
         this.withLogging.Name = "withLogging";
         this.withLogging.Size = new System.Drawing.Size(115, 29);
         this.withLogging.TabIndex = 10;
         this.withLogging.Text = "Submit With Logging";
         this.withLogging.UseVisualStyleBackColor = true;
         this.withLogging.Click += new System.EventHandler(this.withLogging_Click);
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.Location = new System.Drawing.Point(51, 140);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(66, 13);
         this.label7.TabIndex = 12;
         this.label7.Text = "Max Games:";
         // 
         // maxGamesTextBox
         // 
         this.maxGamesTextBox.Location = new System.Drawing.Point(133, 137);
         this.maxGamesTextBox.Name = "maxGamesTextBox";
         this.maxGamesTextBox.Size = new System.Drawing.Size(37, 20);
         this.maxGamesTextBox.TabIndex = 11;
         this.maxGamesTextBox.Text = "5000";
         // 
         // loadGame
         // 
         this.loadGame.Location = new System.Drawing.Point(146, 163);
         this.loadGame.Name = "loadGame";
         this.loadGame.Size = new System.Drawing.Size(115, 44);
         this.loadGame.TabIndex = 13;
         this.loadGame.Text = "Load Game";
         this.loadGame.UseVisualStyleBackColor = true;
         this.loadGame.Click += new System.EventHandler(this.loadGame_Click);
         // 
         // Form1
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(284, 255);
         this.Controls.Add(this.loadGame);
         this.Controls.Add(this.label7);
         this.Controls.Add(this.maxGamesTextBox);
         this.Controls.Add(this.withLogging);
         this.Controls.Add(this.label5);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.label4);
         this.Controls.Add(this.winTextBox);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.colsTextBox);
         this.Controls.Add(this.rowsTextBox);
         this.Controls.Add(this.button1);
         this.Name = "Form1";
         this.Text = "ConnectThemselves";
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox rowsTextBox;
        private System.Windows.Forms.TextBox colsTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox winTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button withLogging;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox maxGamesTextBox;
        private System.Windows.Forms.Button loadGame;

    }
}

