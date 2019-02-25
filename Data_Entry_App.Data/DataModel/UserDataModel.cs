using Data_Entry_App.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data_Entry_App.Data.DataModel
{
    public class UserDataModel
    {
        /// <summary>
        /// Gets or sets user id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets user First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets user Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets username
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets user password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets user email
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// Gets or sets user date of birth
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets user role
        /// </summary>
        public UserRole UserRole { get; set; }

        /// <summary>
        /// Gets or sets marital status
        /// </summary>
        public MaritalStatus MaritalStatus { get; set; }

        /// <summary>
        /// Gets or sets health status
        /// </summary>
        public HealthStatus HealthStatus { get; set; }

    }
}
