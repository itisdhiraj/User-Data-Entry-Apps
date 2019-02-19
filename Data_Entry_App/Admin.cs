using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ADOX;
using ADODB;
using DataTypeEnum = ADOX.DataTypeEnum;


namespace Data_Entry_App
{
    public partial class Admin : Form
    {
        private const string ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data source = C:\\Temp\\UsersData.accdb;  Persist Security Info=False;";

        public Admin()
        {
            InitializeComponent();
        }
       

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("This will Logout from the Application. Confirm?", "Admin Panel", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Main main = new Main();
                    this.Close();
                    this.Dispose();
                    main.Show();
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

        private void BtnHome_Click(object sender, EventArgs e)
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
                throw;
            }
        }
        private Catalog OpenDatabase()
        {
            Catalog catalog = new Catalog();
            Connection connection = new Connection();

            try
            {
                connection.Open(ConnectionString);
                catalog.ActiveConnection = connection;
            }
            catch (Exception)
            {
                catalog.Create(ConnectionString);
            }
            return catalog;
        }

        private void CreateDatabase()
        {

           string cn = @"Provider = Microsoft.ACE.OLEDB.12.0; Data source = C:\\Temp\\UsersData.accdb;  Persist Security Info=False;";
                //cn += AppDomain.CurrentDomain.BaseDirectory.ToString() + "test.mdb"; 
                try
                {
                    OleDbConnection connection = new OleDbConnection(cn);
                    object[] objArrRestrict;
                    objArrRestrict = new object[] { null, null, null, "TABLE" };
                    connection.Open();
                    DataTable schemaTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, objArrRestrict);
                    connection.Close();
                    string[] list;
                    if (schemaTable.Rows.Count > 0)
                    {
                        list = new string[schemaTable.Rows.Count];
                        int i = 0;
                        foreach (DataRow row in schemaTable.Rows)
                        {
                            list[i++] = row["TABLE_NAME"].ToString();
                        }
                        for (i = 0; i < list.Length; i++)
                        {
                            if (list[i] == "USERDATA")
                            {
                                string deletedl = "DROP TABLE TEMP";
                                using (OleDbConnection conn = new OleDbConnection(cn))
                                {
                                    using (OleDbCommand cmd = new OleDbCommand(deletedl, conn))
                                    {

                                        conn.Open();
                                        cmd.ExecuteNonQuery();
                                        conn.Close();
                                    }
                                }
                                break;
                            }
                        }
                    }
                    string ddl = "CREATE TABLE USERDATA (USERID INTEGER NOT NULL,[FIRSTNAME] TEXT(20), [LASTNAME] TEXT(20), [USERNAME] TEXT(20), [USEREMAIL] TEXT(20), [PASSWORD] TEXT(20), [USERDOB] TEXT(20))";
                    using (OleDbConnection conn = new OleDbConnection(cn))
                    {
                        using (OleDbCommand cmd = new OleDbCommand(ddl, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                    MessageBox.Show("A new Data Table is successfully Created");
                }
                catch (System.Exception e)
                {
                    MessageBox.Show(e.Message);
                }
          

            
        }
        private void btn_AddUser_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (File.Exists("C:\\Temp\\UsersData.accdb"))
                {
                    CreateDatabase();
                   // MessageBox.Show("File is already exist");
                }
                else
                {
                    CreateDatabase();

                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Failed to open DB due:\n" + ex.Message);
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

        private void BtnDisplay_Click(object sender, EventArgs e)
        {
            
            try
            {
                
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
