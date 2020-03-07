using NDMS.DomainModel;

namespace NDMS.DataAccess.EntityConfig
{
    /// <summary>
    /// Entity configuration class for Metric mapping entity
    /// </summary>
    internal class MetricMappingConfig : BaseEntityTypeConfiguration<MetricMapping>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MetricMappingConfig() : base()
        {
            this.ToTable("MetricMappings").HasKey<int>(e => e.Id); //table primary key 
            this.Property(e => e.Id).HasColumnName("ID");
            this.Property(e => e.BusinessSegmentId).HasColumnName("BusinessSegmentID").IsRequired();
            this.Property(e => e.DivisionId).HasColumnName("DivisionID").IsRequired();
            this.Property(e => e.ProductLineId).HasColumnName("ProductLineID").IsRequired();
            this.Property(e => e.FacilityId).HasColumnName("FacilityID").IsRequired();
            this.Property(e => e.DepartmentId).HasColumnName("DepartmentID").IsRequired();
            this.Property(e => e.ProcessId).HasColumnName("ProcessID").IsRequired();
            this.Property(e => e.KPIId).HasColumnName("KPIID").IsRequired();
            this.Property(e => e.MetricId).HasColumnName("MetricID").IsRequired();
            this.Property(e => e.CreatedOn).HasColumnName("CreatedOn");
            this.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
            this.Property(e => e.LastModifiedOn).HasColumnName("LastModifiedOn");
            this.Property(e => e.LastModifiedBy).HasColumnName("LastModifiedBy");
            this.Property(e => e.IsActive).HasColumnName("IsActive");

            // Establish all foreign key attributes here
            this.HasRequired(e => e.KPI).WithMany().
                HasForeignKey(e => e.KPIId).WillCascadeOnDelete(false);
            this.HasRequired(e => e.Metric).WithMany().
                HasForeignKey(e => e.MetricId).WillCascadeOnDelete(false);
            this.HasRequired(e => e.BusinessSegment).WithMany().
                HasForeignKey(e => e.BusinessSegmentId).WillCascadeOnDelete(false);
            this.HasRequired(e => e.Division).WithMany().
                HasForeignKey(e => e.DivisionId).WillCascadeOnDelete(false);
            this.HasRequired(e => e.Facility).WithMany().
                HasForeignKey(e => e.FacilityId).WillCascadeOnDelete(false);
            this.HasRequired(e => e.ProductLine).WithMany().
                HasForeignKey(e => e.ProductLineId).WillCascadeOnDelete(false);
            this.HasRequired(e => e.Department).WithMany().
                HasForeignKey(e => e.DepartmentId).WillCascadeOnDelete(false);
            this.HasRequired(e => e.Process).WithMany().
                HasForeignKey(e => e.ProcessId).WillCascadeOnDelete(false);
        }
        #endregion
    }
}
