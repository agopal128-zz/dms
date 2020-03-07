using NDMS.DomainModel;

namespace NDMS.DataAccess.EntityConfig
{
    /// <summary>
    /// Entity configuration class for Counter Measure entity
    /// </summary>
    internal class CounterMeasureConfig : BaseEntityTypeConfiguration<CounterMeasure>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public CounterMeasureConfig() : base()
        {
            this.ToTable("CounterMeasures").HasKey<int>(e => e.Id); //table primary key
            this.Property(e => e.Id).HasColumnName("ID");
            this.Property(e => e.ScorecardId).HasColumnName("ScorecardID").IsRequired();
            this.Property(e => e.KPIId).HasColumnName("KPIID").IsRequired();
            this.Property(e => e.MetricId).HasColumnName("MetricID").IsRequired();
            this.Property(e => e.Issue).HasColumnName("Issue").HasMaxLength(300).IsRequired();
            this.Property(e => e.Action).HasColumnName("Action").HasMaxLength(300).IsRequired();
            this.Property(e => e.DueDate).HasColumnName("DueDate").IsRequired();
            this.Property(e => e.AssignedTo).HasColumnName("AssignedTo").IsRequired();
            this.Property(e => e.CounterMeasureStatusId).HasColumnName("CounterMeasureStatusID")
                .IsRequired();
            this.Property(e => e.CounterMeasurePriorityId).HasColumnName("CounterMeasurePriorityId");
            this.Property(e => e.CreatedOn).HasColumnName("CreatedOn").IsRequired();
            this.Property(e => e.CreatedBy).HasColumnName("CreatedBy").IsRequired();
            this.Property(e => e.LastModifiedOn).HasColumnName("LastModifiedOn").IsRequired();
            this.Property(e => e.LastModifiedBy).HasColumnName("LastModifiedBy").IsRequired();
            // Configure the foreign key with "Scorecards" table here
            this.HasRequired<Scorecard>(s => s.Scorecard)
                .WithMany()
                .HasForeignKey(s => s.ScorecardId)
                .WillCascadeOnDelete(true);
            // Configure the foreign key with "KPIs" table here
            this.HasRequired<KPI>(s => s.KPI).WithMany()
                .HasForeignKey(s => s.KPIId)
                .WillCascadeOnDelete(true);
            // Configure the foreign key with "Metrics" table here
            this.HasRequired<Metric>(s => s.Metric)
                .WithMany()
                .HasForeignKey(s => s.MetricId)
                .WillCascadeOnDelete(true);
            // Configure the foreign key with "CounterMeasureStatus" table here
            this.HasRequired<CounterMeasureStatus>(s => s.CounterMeasureStatus)
                .WithMany()
                .HasForeignKey(s => s.CounterMeasureStatusId)
                .WillCascadeOnDelete(true);
            // Configure the foreign key with "CounterMeasurePriority" table here
            this.HasOptional<CounterMeasurePriority>(s => s.CounterMeasurePriority)
                .WithMany()
                .HasForeignKey(s => s.CounterMeasurePriorityId)
                .WillCascadeOnDelete(true);
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
            // Configure the foreign key with "Users" table here
            this.HasRequired<User>(s => s.AssignedUser)
                .WithMany()
                .HasForeignKey(s => s.AssignedTo)
                .WillCascadeOnDelete(true);

        }
        #endregion
    }
}
