using NDMS.DomainModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace NDMS.DataAccess.EntityConfig
{
    /// <summary>
    /// Entity configuration class for Facility entity
    /// </summary>
    public class FacilityConfig : BaseEntityTypeConfiguration<Facility>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public FacilityConfig() : base()
        {
            this.ToTable("Facilities").HasKey<int>(e => e.Id); //table primary key
            this.Property(e => e.Id).HasColumnName("ID");
            this.Property(e => e.Name).HasColumnName("Name").HasMaxLength(100).IsRequired();
            this.Property(e => e.CreatedOn).HasColumnName("CreatedOn").IsRequired();
            this.Property(e => e.CreatedBy).HasColumnName("CreatedBy").IsRequired();
            this.Property(e => e.LastModifiedOn).HasColumnName("LastModifiedOn").IsRequired();
            this.Property(e => e.LastModifiedBy).HasColumnName("LastModifiedBy").IsRequired();
            this.Property(e => e.IsActive).HasColumnName("IsActive");

            // Configure the foreign key with "Users" table here
            this.HasRequired<User>(s => s.CreatedByUser)
                .WithMany()
                .HasForeignKey(s => s.CreatedBy)
                .WillCascadeOnDelete(true);
            // Configure the foreign key with "Users" table here
            this.HasRequired<User>(s => s.LastModifiedByUser)
                .WithMany()
                .HasForeignKey(s => s.LastModifiedBy)
                .WillCascadeOnDelete(true);
        }
        #endregion
    }
}
