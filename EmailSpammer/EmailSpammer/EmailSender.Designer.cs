namespace EmailSpammer
{
   partial class EmailSender
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
         this.sendEmail = new System.Windows.Forms.Button();
         this.fromTextbox = new System.Windows.Forms.TextBox();
         this.toTextbox = new System.Windows.Forms.TextBox();
         this.smtpTextbox = new System.Windows.Forms.TextBox();
         this.fromLabel = new System.Windows.Forms.Label();
         this.toLabel = new System.Windows.Forms.Label();
         this.smtpLabel = new System.Windows.Forms.Label();
         this.setupGroup = new System.Windows.Forms.GroupBox();
         this.portTextbox = new System.Windows.Forms.TextBox();
         this.portLabel = new System.Windows.Forms.Label();
         this.loginGroup = new System.Windows.Forms.GroupBox();
         this.passwordLabel = new System.Windows.Forms.Label();
         this.usernameLabel = new System.Windows.Forms.Label();
         this.passwordTextbox = new System.Windows.Forms.TextBox();
         this.usernameTextbox = new System.Windows.Forms.TextBox();
         this.groupBox2 = new System.Windows.Forms.GroupBox();
         this.subjectTextbox = new System.Windows.Forms.TextBox();
         this.bodyLabel = new System.Windows.Forms.Label();
         this.subjectLabel = new System.Windows.Forms.Label();
         this.bodyTextbox = new System.Windows.Forms.TextBox();
         this.setupGroup.SuspendLayout();
         this.loginGroup.SuspendLayout();
         this.groupBox2.SuspendLayout();
         this.SuspendLayout();
         // 
         // sendEmail
         // 
         this.sendEmail.Location = new System.Drawing.Point(17, 355);
         this.sendEmail.Name = "sendEmail";
         this.sendEmail.Size = new System.Drawing.Size(382, 34);
         this.sendEmail.TabIndex = 6;
         this.sendEmail.Text = "Send Email!";
         this.sendEmail.UseVisualStyleBackColor = true;
         this.sendEmail.Click += new System.EventHandler(this.sendEmail_Click);
         // 
         // fromTextbox
         // 
         this.fromTextbox.Location = new System.Drawing.Point(86, 19);
         this.fromTextbox.Name = "fromTextbox";
         this.fromTextbox.Size = new System.Drawing.Size(282, 20);
         this.fromTextbox.TabIndex = 1;
         // 
         // toTextbox
         // 
         this.toTextbox.Location = new System.Drawing.Point(86, 45);
         this.toTextbox.Name = "toTextbox";
         this.toTextbox.Size = new System.Drawing.Size(282, 20);
         this.toTextbox.TabIndex = 2;
         // 
         // smtpTextbox
         // 
         this.smtpTextbox.Location = new System.Drawing.Point(86, 71);
         this.smtpTextbox.Name = "smtpTextbox";
         this.smtpTextbox.Size = new System.Drawing.Size(198, 20);
         this.smtpTextbox.TabIndex = 3;
         this.smtpTextbox.Text = "default@mailserver.com";
         this.smtpTextbox.TextChanged += new System.EventHandler(this.smtpTextbox_TextChanged);
         // 
         // fromLabel
         // 
         this.fromLabel.AutoSize = true;
         this.fromLabel.Location = new System.Drawing.Point(6, 22);
         this.fromLabel.Name = "fromLabel";
         this.fromLabel.Size = new System.Drawing.Size(74, 13);
         this.fromLabel.TabIndex = 4;
         this.fromLabel.Text = "From Address:";
         // 
         // toLabel
         // 
         this.toLabel.AutoSize = true;
         this.toLabel.Location = new System.Drawing.Point(11, 48);
         this.toLabel.Name = "toLabel";
         this.toLabel.Size = new System.Drawing.Size(64, 13);
         this.toLabel.TabIndex = 5;
         this.toLabel.Text = "To Address:";
         // 
         // smtpLabel
         // 
         this.smtpLabel.AutoSize = true;
         this.smtpLabel.Location = new System.Drawing.Point(10, 74);
         this.smtpLabel.Name = "smtpLabel";
         this.smtpLabel.Size = new System.Drawing.Size(65, 13);
         this.smtpLabel.TabIndex = 6;
         this.smtpLabel.Text = "SMTP Host:";
         // 
         // setupGroup
         // 
         this.setupGroup.Controls.Add(this.portTextbox);
         this.setupGroup.Controls.Add(this.portLabel);
         this.setupGroup.Controls.Add(this.loginGroup);
         this.setupGroup.Controls.Add(this.fromLabel);
         this.setupGroup.Controls.Add(this.smtpLabel);
         this.setupGroup.Controls.Add(this.fromTextbox);
         this.setupGroup.Controls.Add(this.toLabel);
         this.setupGroup.Controls.Add(this.toTextbox);
         this.setupGroup.Controls.Add(this.smtpTextbox);
         this.setupGroup.Location = new System.Drawing.Point(17, 12);
         this.setupGroup.Name = "setupGroup";
         this.setupGroup.Size = new System.Drawing.Size(382, 181);
         this.setupGroup.TabIndex = 7;
         this.setupGroup.TabStop = false;
         this.setupGroup.Text = "Setup";
         // 
         // portTextbox
         // 
         this.portTextbox.Location = new System.Drawing.Point(325, 71);
         this.portTextbox.Name = "portTextbox";
         this.portTextbox.Size = new System.Drawing.Size(43, 20);
         this.portTextbox.TabIndex = 4;
         this.portTextbox.Text = "25";
         // 
         // portLabel
         // 
         this.portLabel.AutoSize = true;
         this.portLabel.Location = new System.Drawing.Point(290, 74);
         this.portLabel.Name = "portLabel";
         this.portLabel.Size = new System.Drawing.Size(29, 13);
         this.portLabel.TabIndex = 8;
         this.portLabel.Text = "Port:";
         // 
         // loginGroup
         // 
         this.loginGroup.Controls.Add(this.passwordLabel);
         this.loginGroup.Controls.Add(this.usernameLabel);
         this.loginGroup.Controls.Add(this.passwordTextbox);
         this.loginGroup.Controls.Add(this.usernameTextbox);
         this.loginGroup.Location = new System.Drawing.Point(31, 97);
         this.loginGroup.Name = "loginGroup";
         this.loginGroup.Size = new System.Drawing.Size(326, 76);
         this.loginGroup.TabIndex = 5;
         this.loginGroup.TabStop = false;
         this.loginGroup.Text = "Login Credentials";
         // 
         // passwordLabel
         // 
         this.passwordLabel.AutoSize = true;
         this.passwordLabel.Location = new System.Drawing.Point(8, 48);
         this.passwordLabel.Name = "passwordLabel";
         this.passwordLabel.Size = new System.Drawing.Size(56, 13);
         this.passwordLabel.TabIndex = 6;
         this.passwordLabel.Text = "Password:";
         // 
         // usernameLabel
         // 
         this.usernameLabel.AutoSize = true;
         this.usernameLabel.Location = new System.Drawing.Point(6, 19);
         this.usernameLabel.Name = "usernameLabel";
         this.usernameLabel.Size = new System.Drawing.Size(58, 13);
         this.usernameLabel.TabIndex = 5;
         this.usernameLabel.Text = "Username:";
         // 
         // passwordTextbox
         // 
         this.passwordTextbox.Location = new System.Drawing.Point(70, 45);
         this.passwordTextbox.Name = "passwordTextbox";
         this.passwordTextbox.PasswordChar = '*';
         this.passwordTextbox.Size = new System.Drawing.Size(246, 20);
         this.passwordTextbox.TabIndex = 1;
         // 
         // usernameTextbox
         // 
         this.usernameTextbox.Location = new System.Drawing.Point(70, 19);
         this.usernameTextbox.Name = "usernameTextbox";
         this.usernameTextbox.Size = new System.Drawing.Size(246, 20);
         this.usernameTextbox.TabIndex = 0;
         // 
         // groupBox2
         // 
         this.groupBox2.Controls.Add(this.subjectTextbox);
         this.groupBox2.Controls.Add(this.bodyLabel);
         this.groupBox2.Controls.Add(this.subjectLabel);
         this.groupBox2.Controls.Add(this.bodyTextbox);
         this.groupBox2.Location = new System.Drawing.Point(17, 199);
         this.groupBox2.Name = "groupBox2";
         this.groupBox2.Size = new System.Drawing.Size(382, 150);
         this.groupBox2.TabIndex = 8;
         this.groupBox2.TabStop = false;
         this.groupBox2.Text = "Mail Composition";
         // 
         // subjectTextbox
         // 
         this.subjectTextbox.Location = new System.Drawing.Point(58, 19);
         this.subjectTextbox.Name = "subjectTextbox";
         this.subjectTextbox.Size = new System.Drawing.Size(310, 20);
         this.subjectTextbox.TabIndex = 5;
         // 
         // bodyLabel
         // 
         this.bodyLabel.AutoSize = true;
         this.bodyLabel.Location = new System.Drawing.Point(6, 48);
         this.bodyLabel.Name = "bodyLabel";
         this.bodyLabel.Size = new System.Drawing.Size(34, 13);
         this.bodyLabel.TabIndex = 6;
         this.bodyLabel.Text = "Body:";
         // 
         // subjectLabel
         // 
         this.subjectLabel.AutoSize = true;
         this.subjectLabel.Location = new System.Drawing.Point(6, 22);
         this.subjectLabel.Name = "subjectLabel";
         this.subjectLabel.Size = new System.Drawing.Size(46, 13);
         this.subjectLabel.TabIndex = 5;
         this.subjectLabel.Text = "Subject:";
         // 
         // bodyTextbox
         // 
         this.bodyTextbox.Location = new System.Drawing.Point(58, 45);
         this.bodyTextbox.Multiline = true;
         this.bodyTextbox.Name = "bodyTextbox";
         this.bodyTextbox.Size = new System.Drawing.Size(310, 99);
         this.bodyTextbox.TabIndex = 5;
         // 
         // EmailSender
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(413, 399);
         this.Controls.Add(this.groupBox2);
         this.Controls.Add(this.setupGroup);
         this.Controls.Add(this.sendEmail);
         this.Name = "EmailSender";
         this.Text = "Email Sender";
         this.setupGroup.ResumeLayout(false);
         this.setupGroup.PerformLayout();
         this.loginGroup.ResumeLayout(false);
         this.loginGroup.PerformLayout();
         this.groupBox2.ResumeLayout(false);
         this.groupBox2.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Button sendEmail;
      private System.Windows.Forms.TextBox fromTextbox;
      private System.Windows.Forms.TextBox toTextbox;
      private System.Windows.Forms.TextBox smtpTextbox;
      private System.Windows.Forms.Label fromLabel;
      private System.Windows.Forms.Label toLabel;
      private System.Windows.Forms.Label smtpLabel;
      private System.Windows.Forms.GroupBox setupGroup;
      private System.Windows.Forms.GroupBox groupBox2;
      private System.Windows.Forms.TextBox subjectTextbox;
      private System.Windows.Forms.Label bodyLabel;
      private System.Windows.Forms.Label subjectLabel;
      private System.Windows.Forms.TextBox bodyTextbox;
      private System.Windows.Forms.GroupBox loginGroup;
      private System.Windows.Forms.Label passwordLabel;
      private System.Windows.Forms.Label usernameLabel;
      private System.Windows.Forms.TextBox passwordTextbox;
      private System.Windows.Forms.TextBox usernameTextbox;
      private System.Windows.Forms.TextBox portTextbox;
      private System.Windows.Forms.Label portLabel;
   }
}

