using System.ComponentModel;

namespace Data_Entry_App.Data.Enums
{
    public enum HealthStatus
    {
        /// <summary>
        /// HealthStatus - Excellent
        /// </summary>
        [Description("Excellent")]
        Excellent = 1,

        /// <summary>
        /// HealthStatus - Good
        /// </summary>
        [Description("Good")]
        Good,

        /// <summary>
        /// HealthStatus - Average
        /// </summary>
        [Description("Average")]
        Average,

        /// <summary>
        /// HealthStatus - Poor
        /// </summary>
        [Description("Poor")]
        Poor
    }
}
