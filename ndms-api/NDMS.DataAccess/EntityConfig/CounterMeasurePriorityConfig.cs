using NDMS.DomainModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace NDMS.DataAccess.EntityConfig
{
    /// <summary>
    /// Entity configuration class for Counter Measure Priority entity
    /// </summary>
    internal class CounterMeasurePriorityConfig : BaseEntityTypeConfiguration<CounterMeasurePriority>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public CounterMeasurePriorityConfig() : base()
        {
            this.ToTable("CounterMeasurePriority").HasKey<int>(e => e.Id); //table primary key
            this.Property(e => e.Id).HasColumnName("ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(e => e.Name).HasColumnName("Name").HasMaxLength(100).IsRequired();
            this.Property(e => e.IsActive).HasColumnName("IsActive").IsRequired();
        }
        #endregion
    }
}
