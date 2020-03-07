using NDMS.DomainModel;

namespace NDMS.DataAccess.EntityConfig
{
    public class OrganizationalDataConfig : BaseEntityTypeConfiguration<OrganizationalData>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public OrganizationalDataConfig() : base()
        {
            this.ToTable("OrganizationalData").HasKey<int>(e => e.Id); //table primary key
            this.Property(e => e.Id).HasColumnName("ID");
            this.Property(e => e.Name).HasColumnName("Name").HasMaxLength(100).IsRequired();
            this.Property(e => e.IsActive).HasColumnName("IsActive");            
        }
        #endregion
    }
}
