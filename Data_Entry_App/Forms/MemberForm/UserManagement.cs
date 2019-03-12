using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Data_Entry_App.Properties;
using Data_Entry_App.Data.BusinessServices;
using Data_Entry_App.Data.DataModel;
using Data_Entry_App.Data.Enums;


namespace Data_Entry_App.Forms.MemberForm
{
    public partial class UserManagement : Form
    {
        /// <summary>
        /// Instance of DataGridViewPrinter
        /// </summary>
        private DataGridViewPrinter dataGridViewPrinter;

        private int memberId ;
        private readonly IUserDataService UserDataService;

        public UserManagement()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
            this.UserDataService = new UserDataService();
            this.ResetRegistration();
            this.ResetSearch();
            this.InitializeDropDownList();
            this.InitializeDataGridViewStyle();
            this.InitializeResourceString();
            MaximizeBox = false;
            MinimizeBox = false;
            ControlBox = false;
            
        }
        private void InitializeDataGridViewStyle()
        {
            // Setting the style of the DataGridView control
            dataGridViewMembers.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold, GraphicsUnit.Point);
            dataGridViewMembers.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.ControlDark;
            dataGridViewMembers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewMembers.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewMembers.DefaultCellStyle.Font = new Font("Arial", 8, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewMembers.DefaultCellStyle.BackColor = Color.Empty;
            dataGridViewMembers.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.Info;
            dataGridViewMembers.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridViewMembers.GridColor = SystemColors.ControlDarkDark;
        }

        private void InitializeResourceString()
        {
            // Registration Tab
            lblFirstNameReg.Text = Resources.Registration_Name_Label_Text;
            lblLastNameReg.Text = Resources.Registration_Name_Label_Text;
            lblDOBReg.Text = Resources.Registration_DateOfBirth_Label_Text;
            lblEmailIDReg.Text = Resources.Registration_Email_Label_Text;
            lblUserNameReg.Text = Resources.Registration_UserName_Label_Text;
            lblUserRoleReg.Text = Resources.Registration_Role_Label_Text;
            lblMaritalStatusReg.Text = Resources.Registration_MaritalStatus_Label_Text;
            lblPasswordReg.Text = Resources.Registration_Password_Label_Text;
            lblHealthStatusReg.Text = Resources.Registration_HealthStatus_Label_Text;
            BtnAddUser.Text = Resources.Registration_Register_Button_Text;
            BtnClearDataReg.Text = Resources.Registration_Clear_Button_Text;


            // View, Search, Delete, Update Tab
      /*    lblFirstName.Text = Resources.Registration_Name_Label_Text;
            lblLastName.Text = Resources.Registration_Name_Label_Text;
            lblDOBUser.Text = Resources.Registration_DateOfBirth_Label_Text;
            lblEmailID.Text = Resources.Registration_Email_Label_Text;
            lblUserName.Text = Resources.Registration_UserName_Label_Text;
            lblUserRole.Text = Resources.Registration_Role_Label_Text;
            lblMaritalStatus.Text = Resources.Registration_MaritalStatus_Label_Text;
            lblPassword.Text = Resources.Registration_Password_Label_Text;
            lblHealthStatus.Text = Resources.Registration_HealthStatus_Label_Text;   */
            BtnDelete.Text = Resources.Delete_Button_Text;
            btnUpdate.Text = Resources.Update_Button_Text;
            btnRefresh.Text = Resources.Search_Refresh_Button_Text;
            btnSearch.Text = Resources.Search_Search_Button_Text;
            btnPrint.Text = Resources.Print_Print_Button_Text;
            btnPrintPreview.Text = Resources.Print_PrintPreview_Button_Text;
            btnExport.Text = Resources.Print_PrintPreview_Button_Text;



        }

        private void InitializeDropDownList()
        {
            /*
            cmbOccupation.DataSource = Enum.GetValues(typeof(Occupation));
            cmbMaritalStatus.DataSource = Enum.GetValues(typeof(MaritalStatus));
            cmbHealthStatus.DataSource = Enum.GetValues(typeof(HealthStatus));            

            cmbSearchOccupation.DataSource = Enum.GetValues(typeof(Occupation));
            cmbSearchMaritalStatus.DataSource = Enum.GetValues(typeof(MaritalStatus));
            cmbOperand.SelectedIndex = 0;
        */
            cmbHealthStatus.DataSource = Enum.GetValues(typeof(HealthStatus));
            cmbMaritalStatus.DataSource = Enum.GetValues(typeof(MaritalStatus));
            CMBUserRole.DataSource = Enum.GetValues(typeof(UserRole));

            cmbSearchUserRole.DataSource = Enum.GetValues(typeof(UserRole));
            cmbSearchMaritalStatus.DataSource = Enum.GetValues(typeof(MaritalStatus));
            cmbOperand.SelectedIndex = 0;

        }
        private void ResetSearch()
        {
            cmbSearchMaritalStatus.SelectedIndex = -1;
            cmbSearchUserRole.SelectedIndex = -1;
            cmbOperand.SelectedIndex = 0;
        }
        private void ResetRegistration()
        {
            TxtFirstName.Text = string.Empty;
            TxtLastName.Text = string.Empty;
            TxtPassword.Text = string.Empty;
            TxtEmail.Text = string.Empty;
            TxtUserName.Text = string.Empty;
            CMBUserRole.SelectedIndex = -1;
            cmbHealthStatus.SelectedIndex = -1;
            cmbMaritalStatus.SelectedIndex = -1;
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
                OpenFileDialog fdlg = new OpenFileDialog
                {
                    InitialDirectory = @"c:\Temp",
                    Filter =
                        "Access 2000-2003 (*.mdb)|*.mdb|Access 2007 or Later(*.accdb)| *.accdb|CSV files (*.csv)| *.csv",
                    FilterIndex = 2,
                    CheckFileExists = true,
                    RestoreDirectory = true,
                    ReadOnlyChecked = true,
                    ShowReadOnly = true
                };
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
        private void BtnAddUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ValidateRegistration())
                {
                    // Assign the values to the model
                    UserDataModel userDataModel = new UserDataModel()
                    {
                        UserId = 0,
                        FirstName = TxtFirstName.Text.Trim(),
                        LastName = TxtLastName.Text.Trim(),
                        UserName = TxtUserName.Text.Trim(),
                        DateOfBirth = DateTimePickerDOB.Value,
                        UserRole = (UserRole) CMBUserRole.SelectedValue,
                        HealthStatus = (HealthStatus) cmbHealthStatus.SelectedValue,
                        MaritalStatus = (MaritalStatus) cmbMaritalStatus.SelectedValue,
                        Password = TxtPassword.Text.Trim(),
                        UserEmail = TxtEmail.Text.Trim()

                    };

                    // Call the service method and assign the return status to variable
                    var success = this.UserDataService.AddUserData(userDataModel);

                    // if status of success variable is true then display a information else display the error message
                    if (success)
                    {
                        // display the message box
                        MessageBox.Show(
                            Resources.Registration_Successful_Message,
                            Resources.Registration_Successful_Message_Title,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        // Reset the screen
                        this.ResetRegistration();
                    }
                    else
                    {
                        // display the error messge
                        MessageBox.Show(
                            Resources.Registration_Error_Message,
                            Resources.Registration_Error_Message_Title,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Display the validation failed message
                    MessageBox.Show(
                            this.errorMessage,
                            Resources.Registration_Error_Message_Title,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        
                }
            }
            catch (Exception exception)
            {
                ShowErrorMessage(exception);

            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var flag = this.UserDataService.DeleteUserData(this.memberId);
               
                if (flag)
                {
                    DataTable data = this.UserDataService.GetAllUserData();
                    LoadDataGridView(data);

                    MessageBox.Show(
                        Resources.Delete_Successful_Message,
                        Resources.Delete_Successful_Message_Title,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }
       
        private void ShowErrorMessage(Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                //Resources.System_Error_Message, 
                Resources.System_Error_Message_Title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        private void LoadDataGridView(DataTable data)
        {
            // Data grid view column setting            
            dataGridViewMembers.DataSource = data;
            dataGridViewMembers.DataMember = data.TableName;
            dataGridViewMembers.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }
        /// <summary>
        /// Method to set up the printing
        /// </summary>
        /// <param name="isPrint">isPrint value</param>
        /// <returns>true or false</returns>
        private bool SetupPrinting(bool isPrint)
        {
            PrintDialog printDialog = new PrintDialog
            {
                AllowCurrentPage = false,
                AllowPrintToFile = false,
                AllowSelection = false,
                AllowSomePages = false,
                PrintToFile = false,
                ShowHelp = false,
                ShowNetwork = false
            };

            if (isPrint)
            {
                if (printDialog.ShowDialog() != DialogResult.OK)
                {
                    return false;
                }
            }

            this.PrintReport.DocumentName = "MembersReport";
            this.PrintReport.PrinterSettings = printDialog.PrinterSettings;
            this.PrintReport.DefaultPageSettings = printDialog.PrinterSettings.DefaultPageSettings;
            this.PrintReport.DefaultPageSettings.Margins = new Margins(40, 40, 40, 40);
            this.dataGridViewPrinter = new DataGridViewPrinter(dataGridViewMembers, PrintReport, true, true, Resources.Report_Header, new Font("Tahoma", 13, FontStyle.Bold, GraphicsUnit.Point), Color.Black, true);
            return true;
        }

        private void BtnClearDataReg_Click(object sender, EventArgs e)
        {
            ResetRegistration();
        }

        private bool ValidateUpdate()
        {
            this.errorMessage = string.Empty;

            if (txtFirstNameUpdate.Text.Trim() == string.Empty)
            {
                this.AddErrorMessage(Resources.Registration_Name_Required_Text);
            }

            if (txtLastNameUpdate.Text.Trim() == string.Empty)
            {
                this.AddErrorMessage(Resources.Registration_Name_Required_Text);
            }
            if (txtUsernameUpdate.Text.Trim() == string.Empty)
            {
                this.AddErrorMessage(Resources.Registration_Name_Required_Text);
            }
            if (txtPasswordUpdate.Text.Trim() == string.Empty)
            {
                this.AddErrorMessage(Resources.Registration_Password_Required_Text);
            }

            if (cmbUserRoleUpdate.SelectedIndex == -1)
            {
                this.AddErrorMessage(Resources.Registration_Role_Select_Text);
            }

            if (cmbMaritalStatusUpdate.SelectedIndex == -1)
            {
                this.AddErrorMessage(Resources.Registration_MaritalStatus_Select_Text);
            }

            if (cmbHealthStatusUpdate.SelectedIndex == -1)
            {
                this.AddErrorMessage(Resources.Registration_HealthStatus_Select_Text);
            }

            return this.errorMessage != string.Empty ? false : true;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.SetupPrinting(true))
                {
                    this.PrintReport.Print();
                }
            }
            catch (Exception ex)
            {
                this.ShowErrorMessage(ex);
            }
        }

        private void BtnPrintPreview_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.SetupPrinting(false))
                {
                    PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog {Document = this.PrintReport};
                    printPreviewDialog.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                this.ShowErrorMessage(ex);
            }
        }

        
        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                Microsoft.Office.Interop.Excel.ApplicationClass excel = new Microsoft.Office.Interop.Excel.ApplicationClass();
                excel.Visible = true;
                Microsoft.Office.Interop.Excel.Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
                Microsoft.Office.Interop.Excel.Worksheet sheet1 = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                int StartCol = 1;
                int StartRow = 1;
                int j = 0, i = 0;

                //Write Headers
                for (j = 0; j < dataGridViewMembers.Columns.Count; j++)
                {
                    Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[StartRow, StartCol + j];
                    myRange.Value2 = dataGridViewMembers.Columns[j].HeaderText;
                }

                StartRow++;

                //Write datagridview content
                for (i = 0; i < dataGridViewMembers.Rows.Count; i++)
                {
                    for (j = 0; j < dataGridViewMembers.Columns.Count; j++)
                    {
                        try
                        {
                            Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[StartRow + i, StartCol + j];
                            myRange.Value2 = dataGridViewMembers[j, i].Value == null ? "" : dataGridViewMembers[j, i].Value;
                        }
                        catch
                        {
                            ;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

           
        }
        /// <summary>
        /// Event to handle print page
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event data</param>
        private void PrintReport_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                bool hasMorePages = this.dataGridViewPrinter.DrawDataGridView(e.Graphics);

                if (hasMorePages == true)
                {
                    e.HasMorePages = true;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                ResetSearch();
                DataTable data = this.UserDataService.GetAllUserData();
                this.LoadDataGridView(data);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        private void UserManagement_Load(object sender, EventArgs e)
        {
            try
            {
                this.ResetSearch();
                DataTable data = this.UserDataService.GetAllUserData();
                this.LoadDataGridView(data);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        private void dataGridViewMembers_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            //UserID, FirstName, LastName, UserName, Password, UserEmail, UserDOB, HealthStatus, MaritalStatus, UserRole
            try
            {
                if (dgv.SelectedRows.Count > 0)
                {
                    string userMemberId = dgv.SelectedRows[0].Cells[0].Value.ToString();
                    memberId = int.Parse(userMemberId);

                    DataRow dataRow = this.UserDataService.GetUserDataById(memberId);

                    txtFirstNameUpdate.Text = dataRow["FirstName"].ToString();
                    txtLastNameUpdate.Text = dataRow["LastName"].ToString();
                    txtPasswordUpdate.Text = dataRow["Password"].ToString();
                    txtUsernameUpdate.Text = dataRow["Username"].ToString();
                    dateTimePickerDOBUpdate.Value = Convert.ToDateTime(dataRow["UserDOB"]);
                    cmbUserRoleUpdate.SelectedItem = (UserRole)dataRow["UserRole"];
                    cmbMaritalStatusUpdate.SelectedItem = (MaritalStatus)dataRow["MaritalStatus"];
                    cmbHealthStatusUpdate.SelectedItem = (HealthStatus)dataRow["HealthStatus"];
                    TxtEmailIDUpdate.Text = dataRow["UserEmail"].ToString();
                    
                }
            }
            catch (Exception ex)
            {
                this.ShowErrorMessage(ex);
            }
        }

        private void dataGridViewMembers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int currentRow = dataGridViewMembers.SelectedCells[0].RowIndex;
            MessageBox.Show("cell content click");
            try
            {
                string clubMemberId = dataGridViewMembers[0, currentRow].Value.ToString();
                memberId = int.Parse(clubMemberId);
            }
            catch (Exception ex)
            {

            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable data = this.UserDataService.SearchUserData(cmbSearchUserRole.SelectedValue, cmbSearchMaritalStatus.SelectedValue, cmbOperand.GetItemText(cmbOperand.SelectedItem));
                this.LoadDataGridView(data);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ValidateUpdate())
                {
                    UserDataModel userDataModel = new UserDataModel()
                    {
                        UserId = this.memberId,
                        FirstName = txtFirstNameUpdate.Text.Trim(),
                        LastName = txtLastNameUpdate.Text.Trim(),
                        UserName = txtUsernameUpdate.Text.Trim(),
                        UserEmail = TxtEmailIDUpdate.Text.Trim(),
                        DateOfBirth = dateTimePickerDOBUpdate.Value,
                        UserRole = (UserRole)cmbUserRoleUpdate.SelectedValue,
                        HealthStatus = (HealthStatus)cmbHealthStatusUpdate.SelectedValue,
                        MaritalStatus = (MaritalStatus)cmbMaritalStatusUpdate.SelectedValue,
                        Password = txtPasswordUpdate.Text.Trim(),
                        
                    };

                    var flag = this.UserDataService.UpdateUserData(userDataModel);

                    if (flag)
                    {
                        DataTable data = this.UserDataService.GetAllUserData();
                        this.LoadDataGridView(data);

                        MessageBox.Show(
                            Resources.Update_Successful_Message,
                            Resources.Update_Successful_Message_Title,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show(
                        this.errorMessage,
                        Resources.Registration_Error_Message_Title,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        private void InitializeUpdate()
        {
            cmbUserRoleUpdate.DataSource = Enum.GetValues(typeof(UserRole));
            cmbMaritalStatusUpdate.DataSource = Enum.GetValues(typeof(MaritalStatus));
            cmbHealthStatusUpdate.DataSource = Enum.GetValues(typeof(HealthStatus));
        }
        private void TabControlUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (TabControlUser.SelectedIndex == 1)
                {
                    DataTable data = this.UserDataService.GetAllUserData();
                    this.InitializeUpdate();
                    this.LoadDataGridView(data);
                }
            }
            catch (Exception ex)
            {
                this.ShowErrorMessage(ex);
            }
        }
    }
}
