using NDMS.DataAccess.Common;
using NDMS.DomainModel;

namespace NDMS.DataAccess.EntityConfig
{
    internal class DailyTargetHistoryConfig : BaseEntityTypeConfiguration<DailyTargetHistory>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public DailyTargetHistoryConfig() : base()
        {
            this.ToTable("DailyTargetsHistory", Constants.NDMSHistorySchemaName)
                .HasKey<int>(e => e.Id); //table primary key
            this.Property(e => e.Id).HasColumnName("ID");
            this.Property(e => e.DailyTargetId).HasColumnName("DailyTargetID").IsRequired();
            this.Property(e => e.MonthlyTargetId).HasColumnName("MonthlyTargetID").IsRequired();
            this.Property(e => e.Day).HasColumnName("Day").IsRequired();
            this.Property(e => e.MaxGoalValue).HasColumnName("MaxGoalValue");
            this.Property(e => e.RolledUpGoalValue).HasColumnName("RolledUpGoalValue");
            this.Property(e => e.IsManual).HasColumnName("IsManual");
            this.Property(e => e.CreatedOn).HasColumnName("CreatedOn");
            this.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
            this.Property(e => e.LastModifiedOn).HasColumnName("LastModifiedOn");
            this.Property(e => e.LastModifiedBy).HasColumnName("LastModifiedBy");
            // Configure the foreign key with "MontlyTargets" table here
            this.HasRequired<MonthlyTarget>(s => s.MonthlyTarget)
                .WithMany()
                .HasForeignKey(s => s.MonthlyTargetId)
                .WillCascadeOnDelete(true);
            // Configure the foreign key with "DailyTargets" table here
            this.HasRequired<DailyTarget>(s => s.DailyTarget)
                .WithMany(s => s.DailyTargetHistory)
                .HasForeignKey(s => s.DailyTargetId)
                .WillCascadeOnDelete(true);
        }
        #endregion
    }
}
