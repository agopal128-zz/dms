using NDMS.DomainModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace NDMS.DataAccess.EntityConfig
{
    /// <summary>
    /// Entity configuration class for rollup method entity
    /// </summary>
    public class RollupMethodConfig : BaseEntityTypeConfiguration<RollupMethod>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public RollupMethodConfig() : base()
        {
            this.ToTable("RollupMethods").HasKey<int>(e => e.Id); //table primary key
            this.Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(e => e.Id).HasColumnName("ID");
            this.Property(e => e.Name).HasColumnName("Name").HasMaxLength(100).IsRequired();
            this.Property(e => e.IsActive).HasColumnName("IsActive");
        }
        #endregion
    }
}
