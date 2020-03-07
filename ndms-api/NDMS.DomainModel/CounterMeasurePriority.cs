namespace NDMS.DomainModel
{
    public class CounterMeasurePriority : BaseEntity
    {
        #region Propertie(s)

        /// <summary>
        /// Priority Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Flag which says whether the priority is active or not
        /// </summary>
        public bool IsActive { get; set; }

        #endregion
    }
}
