using NDMS.DomainModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace NDMS.DataAccess.EntityConfig
{
    /// <summary>
    /// Entity configuration class for GoalType entity
    /// </summary>
    public class GoalTypeConfig : BaseEntityTypeConfiguration<GoalType>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public GoalTypeConfig() : base()
        {
            this.ToTable("GoalTypes").HasKey<int>(e => e.Id); //table primary key
            this.Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(e => e.Id).HasColumnName("ID");
            this.Property(e => e.Name).HasColumnName("Name").HasMaxLength(100).IsRequired();
            this.Property(e => e.IsActive).HasColumnName("IsActive");
        }
        #endregion
    }
}
