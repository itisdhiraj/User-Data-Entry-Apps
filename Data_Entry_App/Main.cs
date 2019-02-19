using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Data_Entry_App
{
    public partial class Main : Form
    {
        
        public Main()
        {
            InitializeComponent();
           
        }
        //
        
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                Home home = new Home();
                Hide();
                home.Show();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
         
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
          TxtUserName.Clear();
          TxtPassword.Clear();
        }

        private void BtnExit_Click(object sender, EventArgs e)
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

        private void LblUserName_Click(object sender, EventArgs e)
        {

        }
    }
}
