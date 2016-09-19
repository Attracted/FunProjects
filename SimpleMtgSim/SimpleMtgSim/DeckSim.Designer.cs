namespace SimpleMtgSim
{
   partial class DeckSim
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
         this.draw = new System.Windows.Forms.Button();
         this.card = new System.Windows.Forms.Label();
         this.resetDeck = new System.Windows.Forms.Button();
         this.numberCards = new System.Windows.Forms.NumericUpDown();
         this.remainingCards = new System.Windows.Forms.TextBox();
         this.deckControls = new System.Windows.Forms.GroupBox();
         this.deckList = new System.Windows.Forms.ListBox();
         this.deckCreate = new System.Windows.Forms.GroupBox();
         this.loadDeck = new System.Windows.Forms.Button();
         this.tutor = new System.Windows.Forms.Button();
         ((System.ComponentModel.ISupportInitialize)(this.numberCards)).BeginInit();
         this.deckControls.SuspendLayout();
         this.deckCreate.SuspendLayout();
         this.SuspendLayout();
         // 
         // draw
         // 
         this.draw.Location = new System.Drawing.Point(9, 44);
         this.draw.Name = "draw";
         this.draw.Size = new System.Drawing.Size(107, 37);
         this.draw.TabIndex = 0;
         this.draw.Text = "Draw!";
         this.draw.UseVisualStyleBackColor = true;
         this.draw.Click += new System.EventHandler(this.draw_Click);
         // 
         // card
         // 
         this.card.AutoSize = true;
         this.card.Location = new System.Drawing.Point(59, 21);
         this.card.Name = "card";
         this.card.Size = new System.Drawing.Size(75, 13);
         this.card.TabIndex = 2;
         this.card.Text = "card(s), out of:";
         // 
         // resetDeck
         // 
         this.resetDeck.BackColor = System.Drawing.Color.DarkRed;
         this.resetDeck.ForeColor = System.Drawing.SystemColors.ControlLightLight;
         this.resetDeck.Location = new System.Drawing.Point(202, 18);
         this.resetDeck.Name = "resetDeck";
         this.resetDeck.Size = new System.Drawing.Size(79, 63);
         this.resetDeck.TabIndex = 3;
         this.resetDeck.Text = "Reset";
         this.resetDeck.UseVisualStyleBackColor = false;
         this.resetDeck.Click += new System.EventHandler(this.resetDeck_Click);
         // 
         // numberCards
         // 
         this.numberCards.Location = new System.Drawing.Point(14, 19);
         this.numberCards.Name = "numberCards";
         this.numberCards.Size = new System.Drawing.Size(39, 20);
         this.numberCards.TabIndex = 4;
         this.numberCards.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
         // 
         // remainingCards
         // 
         this.remainingCards.Enabled = false;
         this.remainingCards.Location = new System.Drawing.Point(140, 18);
         this.remainingCards.Name = "remainingCards";
         this.remainingCards.Size = new System.Drawing.Size(39, 20);
         this.remainingCards.TabIndex = 5;
         // 
         // deckControls
         // 
         this.deckControls.Controls.Add(this.tutor);
         this.deckControls.Controls.Add(this.remainingCards);
         this.deckControls.Controls.Add(this.numberCards);
         this.deckControls.Controls.Add(this.resetDeck);
         this.deckControls.Controls.Add(this.card);
         this.deckControls.Controls.Add(this.draw);
         this.deckControls.Location = new System.Drawing.Point(12, 236);
         this.deckControls.Name = "deckControls";
         this.deckControls.Size = new System.Drawing.Size(290, 93);
         this.deckControls.TabIndex = 6;
         this.deckControls.TabStop = false;
         this.deckControls.Text = "deckControls";
         // 
         // deckList
         // 
         this.deckList.FormattingEnabled = true;
         this.deckList.Location = new System.Drawing.Point(9, 62);
         this.deckList.Name = "deckList";
         this.deckList.Size = new System.Drawing.Size(272, 147);
         this.deckList.TabIndex = 7;
         // 
         // deckCreate
         // 
         this.deckCreate.Controls.Add(this.loadDeck);
         this.deckCreate.Controls.Add(this.deckList);
         this.deckCreate.Location = new System.Drawing.Point(12, 12);
         this.deckCreate.Name = "deckCreate";
         this.deckCreate.Size = new System.Drawing.Size(290, 218);
         this.deckCreate.TabIndex = 8;
         this.deckCreate.TabStop = false;
         this.deckCreate.Text = "deckCreate";
         // 
         // loadDeck
         // 
         this.loadDeck.Location = new System.Drawing.Point(9, 19);
         this.loadDeck.Name = "loadDeck";
         this.loadDeck.Size = new System.Drawing.Size(272, 37);
         this.loadDeck.TabIndex = 8;
         this.loadDeck.Text = "Load Deck!";
         this.loadDeck.UseVisualStyleBackColor = true;
         this.loadDeck.Click += new System.EventHandler(this.loadDeck_Click);
         // 
         // tutor
         // 
         this.tutor.Location = new System.Drawing.Point(122, 44);
         this.tutor.Name = "tutor";
         this.tutor.Size = new System.Drawing.Size(74, 37);
         this.tutor.TabIndex = 6;
         this.tutor.Text = "Tutor!";
         this.tutor.UseVisualStyleBackColor = true;
         this.tutor.Click += new System.EventHandler(this.tutor_Click);
         // 
         // DeckSim
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(312, 341);
         this.Controls.Add(this.deckCreate);
         this.Controls.Add(this.deckControls);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
         this.Name = "DeckSim";
         this.Text = "Deck Sim";
         ((System.ComponentModel.ISupportInitialize)(this.numberCards)).EndInit();
         this.deckControls.ResumeLayout(false);
         this.deckControls.PerformLayout();
         this.deckCreate.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Button draw;
      private System.Windows.Forms.Label card;
      private System.Windows.Forms.Button resetDeck;
      private System.Windows.Forms.NumericUpDown numberCards;
      private System.Windows.Forms.TextBox remainingCards;
      private System.Windows.Forms.GroupBox deckControls;
      private System.Windows.Forms.ListBox deckList;
      private System.Windows.Forms.GroupBox deckCreate;
      private System.Windows.Forms.Button loadDeck;
      private System.Windows.Forms.Button tutor;
   }
}

