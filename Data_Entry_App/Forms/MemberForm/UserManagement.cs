using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using Data_Entry_App.Properties;
using Data_Entry_App.Data.BusinessServices;


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

        }
        private void InitializeDataGridViewStyle()
        {
            // Setting the style of the DataGridView control
            dataGridViewMembers.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9, FontStyle.Bold, GraphicsUnit.Point);
            dataGridViewMembers.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.ControlDark;
            dataGridViewMembers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewMembers.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewMembers.DefaultCellStyle.Font = new Font("Tahoma", 8, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewMembers.DefaultCellStyle.BackColor = Color.Empty;
            dataGridViewMembers.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.Info;
            dataGridViewMembers.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridViewMembers.GridColor = SystemColors.ControlDarkDark;
        }

        private void InitializeResourceString()
        {

        }

        private void InitializeDropDownList()
        {

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

        private void BtnClearData_Click(object sender, EventArgs e)
        {
            ResetRegistration();
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

            //try
            //{
            //    var table = (DataTable)dataGridViewMembers.DataSource;

            //    Microsoft.Office.Interop.Excel.ApplicationClass ExcelApp
            //        = new Microsoft.Office.Interop.Excel.ApplicationClass();

            //    ExcelApp.Application.Workbooks.Add(true);

            //    int columnIndex = 0;

            //    foreach (DataColumn col in table.Columns)
            //    {
            //        columnIndex++;
            //        ExcelApp.Cells[1, columnIndex] = col.ColumnName;
            //    }

            //    int rowIndex = 0;

            //    foreach (DataRow row in table.Rows)
            //    {
            //        rowIndex++;
            //        columnIndex = 0;
            //        foreach (DataColumn col in table.Columns)
            //        {
            //            columnIndex++;
            //            if (columnIndex == 4 || columnIndex == 5 || columnIndex == 6)
            //            {
            //                if (columnIndex == 4)
            //                {
            //                    ExcelApp.Cells[rowIndex + 1, columnIndex]
            //                        = Enum.GetName(typeof(UserRole), row[col.ColumnName]);
            //                }

            //                if (columnIndex == 5)
            //                {
            //                    ExcelApp.Cells[rowIndex + 1, columnIndex]
            //                        = Enum.GetName(typeof(MaritalStatus), row[col.ColumnName]);
            //                }

            //                if (columnIndex == 6)
            //                {
            //                    ExcelApp.Cells[rowIndex + 1, columnIndex]
            //                        = Enum.GetName(typeof(HealthStatus), row[col.ColumnName]);
            //                }
            //            }
            //            else
            //            {
            //                ExcelApp.Cells[rowIndex + 1, columnIndex] = row[col.ColumnName].ToString();
            //            }
            //        }
            //    }
            //    ExcelApp.Columns.AutoFit();
            //    ExcelApp.Visible = true;
            //    Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelApp.ActiveSheet;
            //    worksheet.Activate();
            //}
            //catch (Exception ex)
            //{
            //    this.ShowErrorMessage(ex);
            //}
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
                this.ShowErrorMessage(ex);
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
                this.ShowErrorMessage(ex);
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
                this.ShowErrorMessage(ex);
            }
        }
    }
}
