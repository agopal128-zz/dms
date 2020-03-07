using NDMS.DomainModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace NDMS.DataAccess.EntityConfig
{
    /// <summary>
    /// Entity configuration class for Metric entity
    /// </summary>
    public class MetricConfig : BaseEntityTypeConfiguration<Metric>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MetricConfig() : base()
        {
            this.ToTable("Metrics").HasKey<int>(e => e.Id); //table primary key
            this.Property(e => e.Id).HasColumnName("ID");
            this.Property(e => e.Name).HasColumnName("Name").HasMaxLength(100).IsRequired();
            this.Property(e => e.GoalTypeId).HasColumnName("GoalTypeID");
            this.Property(e => e.DataTypeId).HasColumnName("DataTypeID");
            this.Property(e => e.IsActive).HasColumnName("IsActive").IsRequired();
            this.Property(e => e.CreatedOn).HasColumnName("CreatedOn");
            this.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
            this.Property(e => e.LastModifiedOn).HasColumnName("LastModifiedOn");
            this.Property(e => e.LastModifiedBy).HasColumnName("LastModifiedBy");
            this.HasRequired(e => e.GoalType).WithMany(e => e.Metrics).
               HasForeignKey(e => e.GoalTypeId).WillCascadeOnDelete(false);
            this.HasRequired(e => e.DataType).WithMany(e => e.Metrics).
               HasForeignKey(e => e.DataTypeId).WillCascadeOnDelete(false);
        }
        #endregion
    }
}
