using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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

        private void btn_AddUser_Click(object sender, EventArgs e)
        {

        }
    }
}
