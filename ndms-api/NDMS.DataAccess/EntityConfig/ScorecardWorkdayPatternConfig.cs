using NDMS.DomainModel;

namespace NDMS.DataAccess.EntityConfig
{
    /// <summary>
    /// Entity configuration class for Holiday Pattern entity
    /// </summary>

    internal class ScorecardWorkdayPatternConfig : BaseEntityTypeConfiguration<ScorecardWorkdayPattern>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ScorecardWorkdayPatternConfig() : base()
        {
            this.ToTable("ScorecardWorkdayPatterns").HasKey<int>(e => e.Id); //table primary key
            this.Property(e => e.Id).HasColumnName("ID");
            this.Property(e => e.ScorecardId).HasColumnName("ScorecardID").IsRequired();
            this.Property(e => e.IsSunday).HasColumnName("IsSunday").IsRequired();
            this.Property(e => e.IsMonday).HasColumnName("IsMonday").IsRequired();
            this.Property(e => e.IsTuesday).HasColumnName("IsTuesday").IsRequired();
            this.Property(e => e.IsWednesday).HasColumnName("IsWednesday").IsRequired();
            this.Property(e => e.IsThursday).HasColumnName("IsThursday").IsRequired();
            this.Property(e => e.IsFriday).HasColumnName("IsFriday").IsRequired();
            this.Property(e => e.IsSaturday).HasColumnName("IsSaturday").IsRequired();
            this.Property(e => e.CreatedOn).HasColumnName("EffectiveStartDate").IsRequired();
            this.Property(e => e.CreatedOn).HasColumnName("EffectiveEndDate");
            this.Property(e => e.CreatedOn).HasColumnName("CreatedOn").IsRequired();
            this.Property(e => e.CreatedBy).HasColumnName("CreatedBy").IsRequired();
            this.Property(e => e.LastModifiedOn).HasColumnName("LastModifiedOn").IsRequired();
            this.Property(e => e.LastModifiedBy).HasColumnName("LastModifiedBy").IsRequired();

            // Configure the foreign key with "Scorecards" table here
            this.HasRequired<Scorecard>(s => s.Scorecard).WithMany()
                .HasForeignKey(s => s.ScorecardId)
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
            this.HasRequired<Scorecard>(e => e.Scorecard).WithMany(e => e.ScorecardWorkdayPatterns).
               HasForeignKey(e => e.ScorecardId).WillCascadeOnDelete(true);

        }
        #endregion
    }
}
