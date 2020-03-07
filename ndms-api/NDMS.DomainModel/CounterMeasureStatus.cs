namespace NDMS.DomainModel
{
    /// <summary>
    /// Represents a counter measure status entity in the system
    /// </summary>
    public class CounterMeasureStatus : BaseEntity
    {
        #region Propertie(s)
        /// <summary>
        /// Status Name
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Flag which says whether the status is active or not
        /// </summary>
        public bool IsActive { get; set; }
        #endregion
    }
}
