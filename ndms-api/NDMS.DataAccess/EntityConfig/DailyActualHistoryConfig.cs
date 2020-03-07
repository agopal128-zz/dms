using NDMS.DataAccess.Common;
using NDMS.DomainModel;

namespace NDMS.DataAccess.EntityConfig
{
   public class DailyActualHistoryConfig : BaseEntityTypeConfiguration<DailyActualHistory>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public DailyActualHistoryConfig() : base()
        {
            this.ToTable("DailyActualsHistory",Constants.NDMSHistorySchemaName)
                .HasKey<int>(e => e.Id); //table primary key
            this.Property(e => e.Id).HasColumnName("ID");
            this.Property(e => e.DailyActualId).HasColumnName("DailyActualID").IsRequired();
            this.Property(e => e.TargetId).HasColumnName("TargetID").IsRequired();
            this.Property(e => e.Date).HasColumnName("Date").IsRequired();
            this.Property(e => e.ActualValue).HasColumnName("ActualValue");
            this.Property(e => e.GoalValue).HasColumnName("GoalValue");
            this.Property(e => e.Status).HasColumnName("Status").IsRequired();
            this.Property(e => e.CreatedOn).HasColumnName("CreatedOn").IsRequired();
            this.Property(e => e.CreatedBy).HasColumnName("CreatedBy").IsRequired();
            this.Property(e => e.LastModifiedOn).HasColumnName("LastModifiedOn").IsRequired();
            this.Property(e => e.LastModifiedBy).HasColumnName("LastModifiedBy").IsRequired();
            // Configure the foreign key with "Targets" table here
            this.HasRequired<Target>(s => s.Target)
                .WithMany()
                .HasForeignKey(s => s.TargetId)
                .WillCascadeOnDelete(true);
            // Configure the foreign key with "Daily Actuals" table here
            this.HasRequired<DailyActual>(s => s.DailyActual)
                .WithMany(x=>x.DailyActualHistory)
                .HasForeignKey(s => s.DailyActualId)
                .WillCascadeOnDelete(true);

        }
        #endregion
    }
}
