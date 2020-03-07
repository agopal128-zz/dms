using NDMS.DomainModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace NDMS.DataAccess.EntityConfig
{
    /// <summary>
    /// Entity configuration class for DataType entity
    /// </summary>
    public class DataTypeConfig : BaseEntityTypeConfiguration<DataType>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public DataTypeConfig() : base()
        {
            this.ToTable("DataTypes").HasKey<int>(e => e.Id); //table primary key
            this.Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(e => e.Id).HasColumnName("ID");
            this.Property(e => e.Name).HasColumnName("Name").HasMaxLength(100).IsRequired();
            this.Property(e => e.IsActive).HasColumnName("IsActive");
        }
        #endregion
    }
}
