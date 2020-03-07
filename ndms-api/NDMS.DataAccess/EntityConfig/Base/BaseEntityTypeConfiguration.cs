using NDMS.DomainModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace NDMS.DataAccess.EntityConfig
{
    /// <summary>
    /// Entity configuration class for BaseEntity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseEntityTypeConfiguration<T> : EntityTypeConfiguration<T> where T : BaseEntity
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public BaseEntityTypeConfiguration()
        {
            this.Property(b => b.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
        #endregion
    }
}
