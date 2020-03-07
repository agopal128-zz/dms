using NDMS.DataAccess.Common;
using NDMS.DomainModel;

namespace NDMS.DataAccess.EntityConfig
{
    public class MonthlyTargetHistoryConfig : BaseEntityTypeConfiguration<MonthlyTargetHistory>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MonthlyTargetHistoryConfig() : base()
        {
            this.ToTable("MonthlyTargetsHistory", Constants.NDMSHistorySchemaName)
                .HasKey<int>(e => e.Id); //table primary key
            this.Property(e => e.Id).HasColumnName("ID");
            this.Property(e => e.MonthlyTargetId).HasColumnName("MonthlyTargetID").IsRequired();
            this.Property(e => e.TargetId).HasColumnName("TargetID").IsRequired();
            this.Property(e => e.Month).HasColumnName("Month").IsRequired();
            this.Property(e => e.DailyRate).HasColumnName("DailyRate");
            this.Property(e => e.MaxGoalValue).HasColumnName("MaxGoalValue");
            this.Property(e => e.RolledUpGoalValue).HasColumnName("RolledUpGoalValue");
            this.Property(e => e.StretchGoalValue).HasColumnName("StretchGoalValue");
            this.Property(e => e.CreatedOn).HasColumnName("CreatedOn");
            this.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
            this.Property(e => e.LastModifiedOn).HasColumnName("LastModifiedOn");
            this.Property(e => e.LastModifiedBy).HasColumnName("LastModifiedBy");
            // Configure the foreign key with "Targets" table here
            this.HasRequired<Target>(s => s.Target)
                .WithMany()
                .HasForeignKey(s => s.TargetId)
                .WillCascadeOnDelete(true);
            // Configure the foreign key with "MonthlyTargets" table here
            this.HasRequired<MonthlyTarget>(s => s.MonthlyTarget)
                .WithMany(s => s.MonthlyTargetHistory)
                .HasForeignKey(s => s.MonthlyTargetId)
                .WillCascadeOnDelete(true);
        }
        #endregion
    }
}
