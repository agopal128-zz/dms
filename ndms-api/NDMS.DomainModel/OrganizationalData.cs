namespace NDMS.DomainModel
{
    public class OrganizationalData : BaseEntity
    {
        /// <summary>
        /// Name of the entity
        /// </summary>
        public string Name { get; set; }

        // <summary>
        /// Flag which says whether organizational data is active or not
        /// </summary>
        public bool IsActive { get; set; }
    }
}
