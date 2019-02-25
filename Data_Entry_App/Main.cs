using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Data_Entry_App.External;
using Data_Entry_App.Forms.MemberForm;
using Data_Entry_App.Properties;


namespace Data_Entry_App
{
    public partial class Main : Form
    {
        
        public Main()
        {
            InitializeComponent();
            InitializeResourceString();
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            MinimizeBox = false;
            TopMost = true;

        }
       
        //
        private void InitializeResourceString()
        {
            LblUserName.Text = Resources.Login_Username_Label_Text;
            LblPassword.Text = Resources.Login_Password_Lable_Text;
            BtnLogin.Text = Resources.Login_Login_Button_Text;
            BtnClear.Text = Resources.Login_Clear_Button_Text;
        }
        Login login = new Login("admin", "1234");
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string user = TxtUserName.Text;
                string pass = TxtPassword.Text;
                    
                if (login.IsLoggedIn(user,pass))
                {
                    UserManagement userManagement = new UserManagement();
                    Hide();
                    userManagement.Show();
                }
               
                else
                {
                    MessageBox.Show(
                        Resources.Login_Validation_Message,
                        Resources.Login_Validation_Message_Title,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
               
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
         
        }

        private void ClearTexts(string user, string pass)
        {
            user = string.Empty;
            pass = string.Empty;
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
          TxtUserName.Clear();
          TxtPassword.Clear();
        }

        

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will close down the whole application. Confirm?", "Data Analyzer", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                this.Activate();
            }
        }

       
    }
}
