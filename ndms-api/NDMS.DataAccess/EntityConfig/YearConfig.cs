using NDMS.DomainModel;

namespace NDMS.DataAccess.EntityConfig
{
    /// <summary>
    /// Entity configuration class for Scorecard Teams entity
    /// </summary>
    public class YearConfig : BaseEntityTypeConfiguration<Year>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public YearConfig() : base()
        {
            this.ToTable("Years").HasKey<int>(e => e.Id); //table primary key
            this.Property(e => e.Id).HasColumnName("ID");
            this.Property(e => e.Name).HasColumnName("Name").HasMaxLength(50);
            this.Property(e => e.StartDate).HasColumnName("StartDate");
            this.Property(e => e.EndDate).HasColumnName("EndDate");
        }
        #endregion
    }
}
