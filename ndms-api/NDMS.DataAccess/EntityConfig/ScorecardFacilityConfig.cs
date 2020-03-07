using NDMS.DomainModel;

namespace NDMS.DataAccess.EntityConfig
{

    internal class ScorecardFacilityConfig : BaseEntityTypeConfiguration<ScorecardFacility>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ScorecardFacilityConfig() : base()
        {

            this.Property(e => e.ScorecardRefId).HasColumnName("ScorecardRefId").IsRequired();
            this.Property(e => e.FacilityRefId).HasColumnName("FacilityRefId").IsRequired();

            // Configure the foreign key with "Scorecards" table here
            this.HasRequired<Scorecard>(s => s.Scorecard).WithMany()
                .HasForeignKey(s => s.ScorecardRefId)
                .WillCascadeOnDelete(true);
            // Configure the foreign key with "Facilities" table here
            this.HasRequired<Facility>(s => s.Facility)
                .WithMany()
                .HasForeignKey(s => s.FacilityRefId)
                .WillCascadeOnDelete(true);

            //this.HasRequired<Scorecard>(e => e.Scorecard).WithMany(e => e.ScorecardFacilities).
            //HasForeignKey(e => e.ScorecardRefId).WillCascadeOnDelete(true);

        }
        #endregion
    }
}
