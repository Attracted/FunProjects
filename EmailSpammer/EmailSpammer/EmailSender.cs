using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;

namespace EmailSpammer
{
   public partial class EmailSender : Form
   {
      public EmailSender()
      {
         InitializeComponent();
      }

      private void sendEmail_Click(object sender, EventArgs e)
      {
         string from = this.fromTextbox.Text;
         string to = this.toTextbox.Text;
         string smtp = this.smtpTextbox.Text;
         string port = this.portTextbox.Text;

         string subject = this.subjectTextbox.Text;
         string body = this.bodyTextbox.Text;
         string username = this.usernameTextbox.Text;
         string password = this.passwordTextbox.Text;

         if (from != "" && to != "" && smtp != "" && port != "" && subject != "" && body != "")
         {
            try
            {
               MailMessage mail = new MailMessage();
               SmtpClient SmtpServer = new SmtpClient(smtp);

               mail.From = new MailAddress(from);
               mail.To.Add(to);
               mail.Subject = subject;
               mail.Body = body;

               SmtpServer.Port = int.Parse(port);
               if (username != "" && password != "")
               {
                  string[] splitUser = username.Split('\\');
                  int len = splitUser.Length;
                  if (len == 1)
                  {
                     SmtpServer.Credentials = new System.Net.NetworkCredential(username, password);
                  }
                  else if (len == 2)
                  {
                     SmtpServer.Credentials = new System.Net.NetworkCredential(splitUser[1], password, splitUser[0]);
                  }
                  else
                  {
                     throw new ArgumentException("Invalid 'Domain\\Username' format");
                  }
               }
               SmtpServer.EnableSsl = false;

               SmtpServer.Send(mail);
               MessageBox.Show("Mail Send Completed");
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.ToString());
            }
         }
      }

      private void smtpTextbox_TextChanged(object sender, EventArgs e)
      {

      }
   }
}
