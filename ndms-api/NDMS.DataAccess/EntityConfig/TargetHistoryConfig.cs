using NDMS.DataAccess.Common;
using NDMS.DomainModel;

namespace NDMS.DataAccess.EntityConfig
{
    internal class TargetHistoryConfig : BaseEntityTypeConfiguration<TargetHistory>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public TargetHistoryConfig() : base()
        {
            this.ToTable("TargetsHistory", Constants.NDMSHistorySchemaName).HasKey<int>(e => e.Id); //table primary key
            this.Property(e => e.Id).HasColumnName("ID");
            this.Property(e => e.TargetId).HasColumnName("TargetID");
            this.Property(e => e.ScorecardId).HasColumnName("ScorecardID").IsRequired();
            this.Property(e => e.KPIId).HasColumnName("KPIID").IsRequired();
            this.Property(e => e.MetricId).HasColumnName("MetricID").IsRequired();
            this.Property(e => e.MetricType).HasColumnName("MetricType").IsRequired();
            this.Property(e => e.IsCascadeFromParent).HasColumnName("IsCascadeFromParent").IsRequired();
            this.Property(e => e.ParentTargetId).HasColumnName("ParentTargetID");
            this.Property(e => e.IsCascaded).HasColumnName("IsCascaded").IsRequired();
            this.Property(e => e.IsStretchGoalEnabled).HasColumnName("IsStretchGoalEnabled").IsRequired();
            this.Property(e => e.CalendarYearId).HasColumnName("CalendarYearID").IsRequired();
            this.Property(e => e.EffectiveStartDate).HasColumnName("EffectiveStartDate").IsRequired();
            this.Property(e => e.EffectiveEndDate).HasColumnName("EffectiveEndDate").IsRequired();
            this.Property(e => e.TargetEntryMethodId).HasColumnName("TargetEntryMethodID");
            this.Property(e => e.RollUpMethodId).HasColumnName("RollUpMethodID");
            this.Property(e => e.TrackingMethodId).HasColumnName("TrackingMethodID");
            this.Property(e => e.GraphPlottingMethodId).HasColumnName("GraphPlottingMethodID");
            this.Property(e => e.MTDPerformanceTrackingMethodId).HasColumnName("MTDPerformanceTrackingID");
            this.Property(e => e.CascadedMetricsTrackingMethodId).HasColumnName("CascadedMetricsTrackingID");
            this.Property(e => e.AnnualTarget).HasColumnName("AnnualTarget");
            this.Property(e => e.IsCopiedMetric).HasColumnName("IsCopiedMetric").IsRequired(); 
            this.Property(e => e.IsActive).HasColumnName("IsActive").IsRequired();
            this.Property(e => e.CreatedOn).HasColumnName("CreatedOn");
            this.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
            this.Property(e => e.LastModifiedOn).HasColumnName("LastModifiedOn");
            this.Property(e => e.LastModifiedBy).HasColumnName("LastModifiedBy");
            // Configure the foreign key with "AnnualTargets" table here
            this.HasRequired<Target>(s => s.Target)
                .WithMany(s => s.TargetHistory)
                .HasForeignKey(s => s.TargetId)
                .WillCascadeOnDelete(true);
            // Configure the foreign key with "Scorecards" table here
            this.HasRequired<Scorecard>(s => s.Scorecard).WithMany()
                .HasForeignKey(s => s.ScorecardId)
                .WillCascadeOnDelete(true);
            // Configure the foreign key with "KPIs" table here
            this.HasRequired<KPI>(s => s.KPI).WithMany()
                .HasForeignKey(s => s.KPIId)
                .WillCascadeOnDelete(true);
            // Configure the foreign key with "Metrics" table here
            this.HasRequired<Metric>(s => s.Metric).WithMany()
                .HasForeignKey(s => s.MetricId)
                .WillCascadeOnDelete(true);
            // Configure the foreign key with "RollupMethods" table here
            this.HasRequired<RollupMethod>(s => s.RollupMethod)
                .WithMany()
                .HasForeignKey(s => s.RollUpMethodId)
                .WillCascadeOnDelete(true);
            // Configuring the self referencing here
            this.HasOptional(e => e.ParentTarget).WithMany().
                HasForeignKey(e => e.ParentTargetId);
            // Configure the foreign key with "Years" table here
            this.HasRequired<Year>(s => s.CalendarYear)
                .WithMany().HasForeignKey(s => s.CalendarYearId)
                .WillCascadeOnDelete(true);
            // Configure the foreign key with "TrackingMethods" table here
            this.HasOptional<TrackingMethod>(s => s.TrackingMethod)
                .WithMany().HasForeignKey(s => s.TrackingMethodId)
                .WillCascadeOnDelete(true);
            // Configure the foreign key with "GraphPlottingMethods" table here
            this.HasOptional<GraphPlottingMethod>(s => s.GraphPlottingMethod)
                .WithMany().HasForeignKey(s => s.GraphPlottingMethodId)
                .WillCascadeOnDelete(true);
            // Configure the foreign key with "TargetEntryMethods" table here
            this.HasOptional<TargetEntryMethod>(s => s.TargetEntryMethod)
                .WithMany()
                .HasForeignKey(s => s.TargetEntryMethodId)
                .WillCascadeOnDelete(true);
        }
        #endregion
    }
}
