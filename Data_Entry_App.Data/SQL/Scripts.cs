namespace Data_Entry_App.Data.SQL
{
    public static class Scripts
    {
        /// <summary>
        /// Sql to get a User Data details by Id
        /// </summary>
        public static readonly string SqlGetUserDataById = "Select" +
            " UserID, FirstName, LastName, UserName, Password, UserEmail, UserDOB, HealthStatus, MaritalStatus, UserRole" +
            " From UsersDataMember Where Id = @Id";

        /// <summary>
        /// Sql to get all Users Data
        /// </summary>
        public static readonly string SqlGetAllUserData = "Select" +
            " UserID, FirstName, LastName, UserName, Password, UserEmail, UserDOB, HealthStatus, MaritalStatus, UserRole" +
            " From UsersDataMember";

        /// <summary>
        /// sql to insert a User Data details
        /// </summary>
        public static readonly string SqlInsertUserData = "Insert Into" +
            " UsersDataMember(FirstName, LastName, UserName, Password, UserEmail, UserDOB, HealthStatus, MaritalStatus, UserRole)" +
            " Values(@FirstName, @LastName, @UserName, @Password, @UserEmail, @UserDOB, @HealthStatus, @MaritalStatus, @UserRole)";

        /// <summary>
        /// sql to search for User Data
        /// </summary>
        public static readonly string SqlSearchUserData = "Select " +
            " UserID, FirstName, LastName, UserName, Password, UserEmail, UserDOB, HealthStatus, MaritalStatus, UserRole" +
            " From UsersDataMember Where (@UserRole Is NULL OR @UserRole = UserRole) {0}" +
            " (@MaritalStatus Is NULL OR @MaritalStatus = MaritalStatus)";

        /// <summary>
        /// sql to update User Data details
        /// </summary>
        public static readonly string SqlUpdateUserData = "Update UsersDataMember " +
            " Set [FirstName] = @FirstName, [LastName] = @LastName, [UserName] = @UserName, [UserDOB] = @UserDOB, [UserRole] = @UserRole, [MaritalStatus] = @MaritalStatus, " +
            " [HealthStatus] = @HealthStatus, [UserEmail] = @UserEmail, [Password]=@Password Where ([UserID] = @UserID)";

        /// <summary>
        /// sql to delete a User Data record
        /// </summary>
        public static readonly string SqlDeleteUserData = "Delete From UsersDataMember Where (UserID = @UserID)";
    }
}