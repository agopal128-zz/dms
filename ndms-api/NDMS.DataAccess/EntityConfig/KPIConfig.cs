using NDMS.DomainModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace NDMS.DataAccess.EntityConfig
{
    /// <summary>
    /// Entity configuration class for KPI entity
    /// </summary>
    public class KPIConfig : BaseEntityTypeConfiguration<KPI>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public KPIConfig() : base()
        {
            this.ToTable("KPIs").HasKey<int>(e => e.Id); //table primary key
            this.Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(e => e.Id).HasColumnName("ID");
            this.Property(e => e.Name).HasColumnName("Name").HasMaxLength(50).IsRequired();
            this.Property(e => e.IsActive).HasColumnName("IsActive");
        }
        #endregion
    }
}
