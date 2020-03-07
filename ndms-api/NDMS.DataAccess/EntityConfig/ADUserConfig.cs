using NDMS.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.DataAccess.EntityConfig
{
    /// <summary>
    /// Entity configuration class for ADUser entity
    /// </summary>
    internal class ADUserConfig: EntityTypeConfiguration<ADUser>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ADUserConfig() : base()
        {
            this.ToTable("V_NDMS_AD").HasKey<string>(e => e.AccountName); // Mapping to View here
            this.Property(e => e.AccountName).HasColumnName("AccountName").IsRequired();
            this.Property(e => e.FirstName).HasColumnName("FirstName").HasMaxLength(200);
            this.Property(e => e.LastName).HasColumnName("LastName").HasMaxLength(200);
            this.Property(e => e.Email).HasColumnName("Email").HasMaxLength(200);
            this.Property(e => e.LastLocationID).HasColumnName("LastLocationID");
        }
        #endregion
    }
}
