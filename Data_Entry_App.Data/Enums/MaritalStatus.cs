using System.ComponentModel;

namespace Data_Entry_App.Data.Enums
{
    public enum MaritalStatus
    {
        /// <summary>
        /// MaritalStatus - Married
        /// </summary>
        [Description("Married")]
        Married = 1,

        /// <summary>
        /// MaritalStatus - Single
        /// </summary>
        [Description("Single")]
        Single = 2,
    }
}