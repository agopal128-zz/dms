using NDMS.DomainModel;
using System.Data.Entity.ModelConfiguration;

namespace NDMS.DataAccess.EntityConfig
{
    public class RecordableConfig: EntityTypeConfiguration<Recordable>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public RecordableConfig() : base()
        {
            this.ToTable("Recordables").HasKey<int>(e => e.Id);
            this.Property(e => e.Id).HasColumnName("ID");
            this.Property(e => e.ScorecardId).HasColumnName("ScorecardID");
            this.Property(e => e.RecordableDate).HasColumnName("RecordableDate");
            this.Property(e => e.IsManual).HasColumnName("IsManual");
            this.Property(e => e.CreatedOn).HasColumnName("CreatedOn");
            this.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
            this.Property(e => e.LastModifiedOn).HasColumnName("LastModifiedOn");
            this.Property(e => e.LastModifiedBy).HasColumnName("LastModifiedBy");
            this.Property(e => e.IsActive).HasColumnName("IsActive");

            this.HasRequired<Scorecard>(e => e.Scorecard).WithMany(e => e.Recordables).
                HasForeignKey(e => e.ScorecardId).WillCascadeOnDelete(true);
        }
        #endregion
    }
}
