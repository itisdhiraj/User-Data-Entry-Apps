using System;
using System.Data;
using System.Data.OleDb;
using Data_Entry_App.Data.DataAccess;
using Data_Entry_App.Data.DataModel;
using Data_Entry_App.Data.SQL;

namespace Data_Entry_App.Data.BusinessServices
{
    public class UserDataService : ConnectionConfiguration, IUserDataService
    {

        private IUserDataAccess userAccess;

        public UserDataService()
        {
            userAccess = new UserDataAccess();
        }

        /// <summary>
        /// Service method to get User Data by Id
        /// </summary>
        /// <param name="UserID">member id</param>
        /// <returns>Data row</returns>
        public DataRow GetUserDataById(int UserID)
        {
            return this.userAccess.GetUserDataById(UserID);
        }

        public DataTable GetAllUserData()
        {
            return this.userAccess.GetAllUserData();
        }

        /// <summary>
        /// Method to search User Data by multiple parameters
        /// </summary>
        /// <param name="userRole">occupation value</param>
        /// <param name="maritalStatus">marital status</param>
        /// <param name="operand">AND OR operand</param>
        /// <returns>Data table</returns>
        public DataTable SearchClubMembers(object userRole, object maritalStatus, string operand)
        {
            return this.userAccess.SearchUserData(userRole, maritalStatus, operand);
        }

        /// <summary>
        /// Method to add new user
        /// </summary>
        /// <param name="userData">User Data model</param>
        /// <returns>true or false</returns>
        public bool AddUserData(UserDataModel userData)
        {
            return this.userAccess.AddUserData(userData);
        }

        /// <summary>
        /// Method to update User Data
        /// </summary>
        /// <param name="userData">User Data</param>
        /// <returns>true / false</returns>
        public bool UpdateUserData(UserDataModel userData)
        {
            return this.userAccess.UpdateUserData(userData);
        }

        /// <summary>
        /// Method to delete a User Data
        /// </summary>
        /// <param name="UserID">user id</param>
        /// <returns>true / false</returns>
        public bool DeleteUserData(int UserID)
        {
            return this.userAccess.DeleteUserData(UserID);
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
            return this.SearchUserData(userRole, maritalStatus, operand);
        }
    }
}