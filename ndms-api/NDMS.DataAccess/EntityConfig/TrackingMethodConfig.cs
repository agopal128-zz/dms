using NDMS.DomainModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.DataAccess.EntityConfig
{
    /// <summary>
    /// Entity configuration class for Tracking Method entity
    /// </summary>
    internal class TrackingMethodConfig : BaseEntityTypeConfiguration<TrackingMethod>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public TrackingMethodConfig() : base()
        {
            this.ToTable("TrackingMethods").HasKey<int>(e => e.Id); //table primary key
            this.Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(e => e.Id).HasColumnName("ID");
            this.Property(e => e.Name).HasColumnName("Name").HasMaxLength(100).IsRequired();
        }
        #endregion
    }
   
}
