using System.ComponentModel;

namespace Data_Entry_App.Data.Enums
{
    public enum UserRole
    {
       /// <summary>
            /// Occupation - Doctor
            /// </summary>
            [Description("Admin")]
            Admin = 1,

            /// <summary>
            /// Occupation - Engineer
            /// </summary>
            [Description("Other")]
            Other = 2,

            /// <summary>
            /// Occupation - Professor
            /// </summary>
            [Description("Guest")]
            Guest=3,

    }
}