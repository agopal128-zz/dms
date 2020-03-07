using NDMS.DomainModel;
namespace NDMS.DataAccess.EntityConfig
{
  
    internal class ScorecardBusinessSegmentConfig : BaseEntityTypeConfiguration<ScorecardBusinessSegment>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ScorecardBusinessSegmentConfig() : base()
        {
          
            this.Property(e => e.ScorecardRefId).HasColumnName("ScorecardRefId").IsRequired();
            this.Property(e => e.BussinessSegmentRefId).HasColumnName("BussinessSegmentRefId").IsRequired();

            // Configure the foreign key with "Scorecards" table here
            this.HasRequired<Scorecard>(s => s.Scorecard).WithMany()
                .HasForeignKey(s => s.ScorecardRefId)
                .WillCascadeOnDelete(true);
            // Configure the foreign key with "BusinessSegments" table here
            this.HasRequired<BusinessSegment>(s => s.BusinessSegment)
                .WithMany()
                .HasForeignKey(s => s.BussinessSegmentRefId)
                .WillCascadeOnDelete(true);

            //this.HasRequired<Scorecard>(e => e.Scorecard).WithMany(e => e.ScorecardBusinessSegments).
            //HasForeignKey(e => e.ScorecardRefId).WillCascadeOnDelete(true);

        }
        #endregion
    }
}
