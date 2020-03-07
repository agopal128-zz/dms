using NDMS.DomainModel;


namespace NDMS.DataAccess.EntityConfig
{
 
    internal class ScorecardDivisionConfig : BaseEntityTypeConfiguration<ScorecardDivision>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ScorecardDivisionConfig() : base()
        {

            this.Property(e => e.ScorecardRefId).HasColumnName("ScorecardRefId").IsRequired();
            this.Property(e => e.DivisionRefId).HasColumnName("DivisionRefId").IsRequired();

            // Configure the foreign key with "Scorecards" table here
            this.HasRequired<Scorecard>(s => s.Scorecard).WithMany()
                .HasForeignKey(s => s.ScorecardRefId)
                .WillCascadeOnDelete(true);
            // Configure the foreign key with "Divisions" table here
            this.HasRequired<Division>(s => s.Division)
                .WithMany()
                .HasForeignKey(s => s.DivisionRefId)
                .WillCascadeOnDelete(true);

            //this.HasRequired<Scorecard>(e => e.Scorecard).WithMany(e => e.ScorecardDivisions).
            //HasForeignKey(e => e.ScorecardRefId).WillCascadeOnDelete(true);

        }
        #endregion
    }
}
