using NDMS.DomainModel;

namespace NDMS.DataAccess.EntityConfig
{
   
    internal class ScorecardProcessConfig : BaseEntityTypeConfiguration<ScorecardProcess>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ScorecardProcessConfig() : base()
        {

            this.Property(e => e.ScorecardRefId).HasColumnName("ScorecardRefId").IsRequired();
            this.Property(e => e.ProcessRefId).HasColumnName("ProcessRefId").IsRequired();

            // Configure the foreign key with "Scorecards" table here
            this.HasRequired<Scorecard>(s => s.Scorecard).WithMany()
                .HasForeignKey(s => s.ScorecardRefId)
                .WillCascadeOnDelete(true);
            // Configure the foreign key with "Process" table here
            this.HasRequired<Process>(s => s.Process)
                .WithMany()
                .HasForeignKey(s => s.ProcessRefId)
                .WillCascadeOnDelete(true);

            //this.HasRequired<Scorecard>(e => e.Scorecard).WithMany(e => e.ScorecardProcesses).
            //HasForeignKey(e => e.ScorecardRefId).WillCascadeOnDelete(true);

        }
        #endregion
    }
}
