using NDMS.DomainModel;

namespace NDMS.DataAccess.EntityConfig
{
    /// <summary>
    /// Entity configuration class for Holiday Pattern entity
    /// </summary>

    internal class ScorecardHolidayPatternConfig : BaseEntityTypeConfiguration<ScorecardHolidayPattern>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ScorecardHolidayPatternConfig() : base()
        {
            this.ToTable("ScorecardHolidayPatterns").HasKey<int>(e => e.Id); //table primary key
            this.Property(e => e.Id).HasColumnName("ID");
            this.Property(e => e.ScorecardId).HasColumnName("ScorecardID").IsRequired();
            this.Property(e => e.HolidayPatternId).HasColumnName("HolidayPatternID").IsRequired();
           // this.Property(e => e.Name).HasColumnName("Name").IsRequired();
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
            // Configure the foreign key with "HolidayPatterns" table here
            this.HasRequired<HolidayPattern>(s => s.HolidayPattern)
                .WithMany()
                .HasForeignKey(s => s.HolidayPatternId)
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

            this.HasRequired<Scorecard>(e => e.Scorecard).WithMany(e => e.ScorecardHolidayPatterns).
            HasForeignKey(e => e.ScorecardId).WillCascadeOnDelete(true);

        }
        #endregion
    }
}
