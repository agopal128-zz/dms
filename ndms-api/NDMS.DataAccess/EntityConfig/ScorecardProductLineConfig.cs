using NDMS.DomainModel;

namespace NDMS.DataAccess.EntityConfig
{
    internal class ScorecardProductLineConfig : BaseEntityTypeConfiguration<ScorecardProductLine>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ScorecardProductLineConfig() : base()
        {

            this.Property(e => e.ScorecardRefId).HasColumnName("ScorecardRefId").IsRequired();
            this.Property(e => e.ProductLineRefId).HasColumnName("ProductLineRefId").IsRequired();

            // Configure the foreign key with "Scorecards" table here
            this.HasRequired<Scorecard>(s => s.Scorecard).WithMany()
                .HasForeignKey(s => s.ScorecardRefId)
                .WillCascadeOnDelete(true);
            // Configure the foreign key with "ProductLine" table here
            this.HasRequired<ProductLine>(s => s.ProductLine)
                .WithMany()
                .HasForeignKey(s => s.ProductLineRefId)
                .WillCascadeOnDelete(true);

            //this.HasRequired<Scorecard>(e => e.Scorecard).WithMany(e => e.ScorecardProductLines).
            //HasForeignKey(e => e.ScorecardRefId).WillCascadeOnDelete(true);

        }
        #endregion
    }
}
