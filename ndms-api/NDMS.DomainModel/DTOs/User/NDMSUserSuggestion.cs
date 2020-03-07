namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// Represents a suggestion for NDMS user
    /// </summary>
    public class NDMSUserSuggestion
    {
        #region Propertie(s)
        /// <summary>
        /// User ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Full name of the user
        /// </summary>
        public string FullName { get; set; }
        #endregion
    }
}
