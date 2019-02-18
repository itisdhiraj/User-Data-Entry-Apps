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


namespace Data_Entry_App
{
    public partial class Admin : Form
    {
       

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

        private void btn_AddUser_Click(object sender, EventArgs e)
        {
            
            try
            {
                if ( File.Exists("C:\\Temp\\Users.accdb"))
                {

                }
                else
                {
                    try
                    {
                        ADOX.Catalog catalog = new Catalog();
                        catalog.Create(@"Provider = Microsoft.ACE.OLEDB.12.0; Data source = C:\Temp\Users.accdb");

                        string DataFile = "C:\\Temp\\Users.accdb";
                        string myConnection = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\" + DataFile;

                        Table table = new Table();
                        table.Name = "UsersData";

                        // Column 1 (id)
                        ADOX.Column idCol = new ADOX.Column();
                        idCol.Name = "Id"; // The name of the column
                        idCol.ParentCatalog = catalog;
                        idCol.Type = ADOX.DataTypeEnum.adInteger; // Indicates a four byte signed integer.
                        idCol.Properties["AutoIncrement"].Value =
                            true; // Enable the auto increment property for this column.
                        
                        // Column 2 (Name)
                        ADOX.Column firstnameCol = new ADOX.Column();
                        firstnameCol.Name = "Firstname"; // The name of the column
                        firstnameCol.ParentCatalog = catalog;
                        firstnameCol.Type = ADOX.DataTypeEnum.adVarWChar; // Indicates a string value type.
                        firstnameCol.DefinedSize = 60; // 60 characters max.


                        // Column 3 (Surname)
                        ADOX.Column lastnameCol = new ADOX.Column();
                        lastnameCol.Name = "Lastname"; // The name of the column
                        lastnameCol.ParentCatalog = catalog;
                        lastnameCol.Type = ADOX.DataTypeEnum.adVarWChar; // Indicates a string value type.
                        lastnameCol.DefinedSize = 60; // 60 characters max.

                        // Column 4 (Username)
                        ADOX.Column usernameCol = new ADOX.Column();
                        usernameCol.Name = "Username"; // The name of the column
                        usernameCol.ParentCatalog = catalog;
                        usernameCol.Type = ADOX.DataTypeEnum.adVarWChar; // Indicates a string value type.
                        usernameCol.DefinedSize = 60; // 60 characters max.

                        // Column 5 (Password)
                        ADOX.Column passwordCol = new ADOX.Column();
                        passwordCol.Name = "Password"; // The name of the column
                        passwordCol.ParentCatalog = catalog;
                        passwordCol.Type = ADOX.DataTypeEnum.adVarWChar; // Indicates a string value type.
                        passwordCol.DefinedSize = 60; // 60 characters max.

                        //Column 5 (Email)
                        ADOX.Column emailIdCol = new ADOX.Column();
                        emailIdCol.Name = "EmailId"; // The name of the column
                        emailIdCol.ParentCatalog = catalog;
                        emailIdCol.Type = ADOX.DataTypeEnum.adVarWChar; // Indicates a string value type.
                        emailIdCol.DefinedSize = 60; // 60 characters max.

                        //Column 6 (DOB)
                        ADOX.Column dobCol = new ADOX.Column();
                        dobCol.Name = "DateOfBirth"; // The name of the column
                        dobCol.ParentCatalog = catalog;
                        dobCol.Type = ADOX.DataTypeEnum.adVarWChar; // Indicates a string value type.
                        dobCol.DefinedSize = 20; // 60 characters max.

                        table.Columns.Append(idCol); // Add the Id column to the table.
                        table.Columns.Append(firstnameCol); // Add the First Name column to the table.
                        table.Columns.Append(lastnameCol); // Add the Last Name column to the table.
                        table.Columns.Append(usernameCol); // Add the Username column to the table.
                        table.Columns.Append(passwordCol); // Add the Password column to the table.
                        table.Columns.Append(emailIdCol); // Add the Email-Id column to the table.
                        table.Columns.Append(dobCol); // Add the DOB column to the table.


                        catalog.Tables.Append(table); // Add the table to our database.             

                        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(table);
                        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(catalog.Tables);
                        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(catalog.ActiveConnection);
                        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(catalog);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }


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
                fdlg.InitialDirectory = @"c:\";
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
