using NDMS.DomainModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace NDMS.DataAccess.EntityConfig
{
    /// <summary>
    /// Entity configuration class for Counter Measure Status entity
    /// </summary>
    internal class CounterMeasureStatusConfig : BaseEntityTypeConfiguration<CounterMeasureStatus>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public CounterMeasureStatusConfig() : base()
        {
            this.ToTable("CounterMeasureStatus").HasKey<int>(e => e.Id); //table primary key
            this.Property(e => e.Id).HasColumnName("ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(e => e.Status).HasColumnName("Status").HasMaxLength(100).IsRequired();
            this.Property(e => e.IsActive).HasColumnName("IsActive").IsRequired();
        }
        #endregion
    }
}
