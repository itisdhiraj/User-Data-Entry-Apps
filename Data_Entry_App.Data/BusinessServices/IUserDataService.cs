using Data_Entry_App.Data.DataModel;
using System.Data;

namespace Data_Entry_App.Data.BusinessServices
{
    public interface IUserDataService
    {

        DataRow GetUserDataById(int UserID);


        /// <summary>
        /// Method to get all Users Data
        /// </summary>
        /// <returns>Data table</returns>
        DataTable GetAllUserData();

        /// <summary>
        /// Method to search User Data by multiple parameters
        /// </summary>
        /// <param name="userRole">User role value</param>
        /// <param name="maritalStatus">marital status</param>
        /// <param name="operand">AND OR operand</param>
        /// <returns>Data table</returns>
        DataTable SearchUserData(object userRole, object maritalStatus, string operand);

        /// <summary>
        /// Method to create new user
        /// </summary>
        /// <param name="userData">User Data model</param>
        /// <returns>true or false</returns>
        bool AddUserData(UserDataModel userData);

        /// <summary>
        /// Method to update User Data details
        /// </summary>
        /// <param name="userData">User Data</param>
        /// <returns></returns>
        bool UpdateUserData(UserDataModel userData);

        /// <summary>
        /// Method to delete a User
        /// </summary>
        /// <param name="id">member id</param>
        /// <returns>true / false</returns>
        bool DeleteUserData(int id);
    }
}