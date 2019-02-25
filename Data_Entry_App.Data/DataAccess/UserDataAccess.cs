using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using Data_Entry_App.Data.DataModel;
using Data_Entry_App.Data.SQL;

namespace Data_Entry_App.Data.DataAccess
{
    public class UserDataAccess : ConnectionConfiguration, IUserDataAccess
    {
        /// <summary>
        /// Method to get all Users Data
        /// </summary>
        /// <returns>Data table</returns>
        public DataTable GetAllUserData()
        {
            DataTable dataTable = new DataTable();

            using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter())
            {
                // Create the command and set its properties
                oleDbDataAdapter.SelectCommand = new OleDbCommand();
                oleDbDataAdapter.SelectCommand.Connection = new OleDbConnection(this.ConnectionString);
                oleDbDataAdapter.SelectCommand.CommandType = CommandType.Text;

                // Assign the SQL to the command object
                oleDbDataAdapter.SelectCommand.CommandText = Scripts.SqlGetAllUserData;

                // Fill the datatable from adapter
                oleDbDataAdapter.Fill(dataTable);
            }

            return dataTable;
        }

        /// <summary>
        /// Method to get User Data by Id
        /// </summary>
        /// <param name="UserID">member id</param>
        /// <returns>Data row</returns>
        public DataRow GetUserDataById(int UserID)
        {
            DataTable dataTable = new DataTable();
            DataRow dataRow;

            using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter())
            {
                // Create the command and set its properties
                dataAdapter.SelectCommand = new OleDbCommand();
                dataAdapter.SelectCommand.Connection = new OleDbConnection(this.ConnectionString);
                dataAdapter.SelectCommand.CommandType = CommandType.Text;
                dataAdapter.SelectCommand.CommandText = Scripts.SqlGetUserDataById;

                // Add the parameter to the parameter collection
                dataAdapter.SelectCommand.Parameters.AddWithValue("@UserID", UserID);

                // Fill the datatable From adapter
                dataAdapter.Fill(dataTable);

                // Get the datarow from the table
                dataRow = dataTable.Rows.Count > 0 ? dataTable.Rows[0] : null;

                return dataRow;
            }
        }

        /// <summary>
        /// Method to search User Data by multiple parameters
        /// </summary>
        /// <param name="userRole">occupation value</param>
        /// <param name="maritalStatus">marital status</param>
        /// <param name="operand">AND OR operand</param>
        /// <returns>Data table</returns>
        public DataTable SearchUserData(object userRole, object maritalStatus, string operand)
        {
            DataTable dataTable = new DataTable();

            using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter())
            {
                // Create the command and set its properties
                oleDbDataAdapter.SelectCommand = new OleDbCommand();
                oleDbDataAdapter.SelectCommand.Connection = new OleDbConnection(this.ConnectionString);
                oleDbDataAdapter.SelectCommand.CommandType = CommandType.Text;

                // Assign the SQL to the command object
                oleDbDataAdapter.SelectCommand.CommandText = string.Format(Scripts.SqlSearchUserData, operand);

                // Add the input parameters to the parameter collection
                oleDbDataAdapter.SelectCommand.Parameters.AddWithValue("@UserRole", userRole == null ? DBNull.Value : userRole);
                oleDbDataAdapter.SelectCommand.Parameters.AddWithValue("@MaritalStatus", maritalStatus == null ? DBNull.Value : maritalStatus);

                // Fill the table from adapter
                oleDbDataAdapter.Fill(dataTable);
            }

            return dataTable;
        }

        /// <summary>
        /// Method to add new user
        /// </summary>
        /// <param name="userData">User Data model</param>
        /// <returns>true or false</returns>
        public bool AddUserData(UserDataModel userData)
        {
            using (OleDbCommand oleDbCommand = new OleDbCommand())
            {
                // Set the command object properties
                oleDbCommand.Connection = new OleDbConnection(this.ConnectionString);
                oleDbCommand.CommandType = CommandType.Text;
                oleDbCommand.CommandText = Scripts.SqlInsertUserData;

                // Add the input parameters to the parameter collection
                oleDbCommand.Parameters.AddWithValue("@FirstName", userData.FirstName);
                oleDbCommand.Parameters.AddWithValue("@LastName", userData.LastName);
                oleDbCommand.Parameters.AddWithValue("@UserName", userData.UserName);
                oleDbCommand.Parameters.AddWithValue("@UserEmail", userData.UserEmail);
                oleDbCommand.Parameters.AddWithValue("@UserDOB", userData.DateOfBirth.ToShortDateString());
                oleDbCommand.Parameters.AddWithValue("@Occupation", (int)userData.UserRole);
                oleDbCommand.Parameters.AddWithValue("@MaritalStatus", (int)userData.MaritalStatus);
                oleDbCommand.Parameters.AddWithValue("@HealthStatus", (int)userData.HealthStatus);
                oleDbCommand.Parameters.AddWithValue("@Password", userData.Password);
              

                // Open the connection, execute the query and close the connection
                oleDbCommand.Connection.Open();
                var rowsAffected = oleDbCommand.ExecuteNonQuery();
                oleDbCommand.Connection.Close();

                return rowsAffected > 0;
            }
        }

        /// <summary>
        /// Method to update User Data
        /// </summary>
        /// <param name="userData">User Data</param>
        /// <returns>true / false</returns>
        public bool UpdateUserData(UserDataModel userData)
        {
            using (OleDbCommand dbCommand = new OleDbCommand())
            {
                // Set the command object properties
                dbCommand.Connection = new OleDbConnection(this.ConnectionString);
                dbCommand.CommandType = CommandType.Text;
                dbCommand.CommandText = Scripts.SqlUpdateUserData;

                // Add the input parameters to the parameter collection
                dbCommand.Parameters.AddWithValue("@FirstName", userData.FirstName);
                dbCommand.Parameters.AddWithValue("@LastName", userData.LastName);
                dbCommand.Parameters.AddWithValue("@UserName", userData.UserName);
                dbCommand.Parameters.AddWithValue("@UserDOB", userData.DateOfBirth.ToShortDateString());
                dbCommand.Parameters.AddWithValue("@UserRole", (int)userData.UserRole);
                dbCommand.Parameters.AddWithValue("@MaritalStatus", (int)userData.MaritalStatus);
                dbCommand.Parameters.AddWithValue("@HealthStatus", (int)userData.HealthStatus);
                dbCommand.Parameters.AddWithValue("@Password", userData.Password);
                dbCommand.Parameters.AddWithValue("@UserEmail", userData.UserEmail);
                dbCommand.Parameters.AddWithValue("@UserID", userData.UserId);

                // Open the connection, execute the query and close the connection
                dbCommand.Connection.Open();
                var rowsAffected = dbCommand.ExecuteNonQuery();
                dbCommand.Connection.Close();

                return rowsAffected > 0;
            }
        }

        /// <summary>
        /// Method to delete a User Data
        /// </summary>
        /// <param name="UserID">user id</param>
        /// <returns>true / false</returns>
        public bool DeleteUserData(int UserID)
        {
            using (OleDbCommand dbCommand = new OleDbCommand())
            {
                // Set the command object properties
                dbCommand.Connection = new OleDbConnection(this.ConnectionString);
                dbCommand.CommandType = CommandType.Text;
                dbCommand.CommandText = Scripts.SqlDeleteUserData;

                // Add the input parameter to the parameter collection
                dbCommand.Parameters.AddWithValue("@UserID", UserID);

                // Open the connection, execute the query and close the connection
                dbCommand.Connection.Open();
                var rowsAffected = dbCommand.ExecuteNonQuery();
                dbCommand.Connection.Close();

                return rowsAffected > 0;
            }
        }
    }
}
