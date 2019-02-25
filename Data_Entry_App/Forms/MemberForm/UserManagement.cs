using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Data_Entry_App.Properties;

namespace Data_Entry_App.Forms.MemberForm
{
    public partial class UserManagement : Form
    {
        public UserManagement()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("This will Logout from the Application. Confirm?", "User Management", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Main main = new Main();
                    this.Close();
                    this.Dispose();
                    System.Windows.Forms.Application.Exit();
                    //main.Show();
                }
                else
                {
                    this.Activate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnCheck_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fdlg = new OpenFileDialog();
                fdlg.InitialDirectory = @"c:\Temp";
                fdlg.Filter = "Access 2000-2003 (*.mdb)|*.mdb|Access 2007 or Later(*.accdb)| *.accdb|CSV files (*.csv)| *.csv";
                fdlg.FilterIndex = 2;
                fdlg.CheckFileExists = true;
                fdlg.RestoreDirectory = true;
                fdlg.ReadOnlyChecked = true;
                fdlg.ShowReadOnly = true;
                if (fdlg.ShowDialog() == DialogResult.OK)
                {
                    TxtCheckFile.Text = fdlg.FileName;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private string errorMessage;
        private void AddErrorMessage(string error)
        {
            if (this.errorMessage == string.Empty)
            {
                this.errorMessage = Resources.Error_Message_Header + "\n\n";
            }

            this.errorMessage += error + "\n";
        }
        private bool ValidateRegistration()
        {
            errorMessage = string.Empty;

            if (TxtFirstName.Text.Trim() == string.Empty)
            {
                AddErrorMessage(Resources.Registration_Name_Required_Text);
            }

            if (TxtLastName.Text.Trim() == string.Empty)
            {
                this.AddErrorMessage(Resources.Registration_Name_Required_Text);
            }

            if (TxtEmail.Text.Trim() == String.Empty)
            {
                AddErrorMessage(Resources.Registration_Email_Required_Text);
            }

            if (CMBUserRole.SelectedIndex == -1)
            {
                this.AddErrorMessage(Resources.Registration_Role_Select_Text);
            }

            if (cmbMaritalStatus.SelectedIndex == -1)
            {
                this.AddErrorMessage(Resources.Registration_MaritalStatus_Select_Text);
            }

            if (cmbHealthStatus.SelectedIndex == -1)
            {
                this.AddErrorMessage(Resources.Registration_HealthStatus_Select_Text);
            }

            return this.errorMessage != string.Empty ? false : true;
        }
        private void btn_AddUser_Click(object sender, EventArgs e)
        {

        }
    }
}
