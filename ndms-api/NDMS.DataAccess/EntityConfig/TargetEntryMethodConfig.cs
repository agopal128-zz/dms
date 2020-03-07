using NDMS.DomainModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace NDMS.DataAccess.EntityConfig
{
    public class TargetEntryMethodConfig : BaseEntityTypeConfiguration<TargetEntryMethod>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public TargetEntryMethodConfig() : base()
        {
            this.ToTable("TargetEntryMethods").HasKey<int>(e => e.Id); //table primary key
            this.Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(e => e.Id).HasColumnName("ID");
            this.Property(e => e.Name).HasColumnName("Name").HasMaxLength(100).IsRequired();
        }
        #endregion
    }
}
