using NDMS.DataAccess.Common;
using NDMS.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.DataAccess.EntityConfig
{
   public class MonthlyActualHistoryConfig: BaseEntityTypeConfiguration<MonthlyActualHistory>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MonthlyActualHistoryConfig() : base()
        {
            this.ToTable("MonthlyActualsHistory",Constants.NDMSHistorySchemaName)
                .HasKey<int>(e => e.Id); //table primary key
            this.Property(e => e.Id).HasColumnName("ID");
            this.Property(e => e.MonthlyActualId).HasColumnName("MonthlyActualID");
            this.Property(e => e.TargetId).HasColumnName("TargetID").IsRequired();
            this.Property(e => e.Month).HasColumnName("Month").IsRequired();
            this.Property(e => e.ActualValue).HasColumnName("ActualValue");
            this.Property(e => e.Status).HasColumnName("Status").IsRequired();
            this.Property(e => e.CreatedOn).HasColumnName("CreatedOn").IsRequired();
            this.Property(e => e.CreatedBy).HasColumnName("CreatedBy").IsRequired();
            this.Property(e => e.LastModifiedOn).HasColumnName("LastModifiedOn").IsRequired();
            this.Property(e => e.LastModifiedBy).HasColumnName("LastModifiedBy").IsRequired();
            // Configure the foreign key with "Targets" table here
            this.HasRequired<Target>(s => s.Target)
                .WithMany()
                .HasForeignKey(s => s.TargetId)
                .WillCascadeOnDelete(true);
            // Configure the foreign key with "MonthlyActuals" table here
            this.HasRequired<MonthlyActual>(s => s.MonthlyActual)
                .WithMany(s=>s.MonthlyActualHistory)
                .HasForeignKey(s => s.MonthlyActualId)
                .WillCascadeOnDelete(true);

        }
        #endregion
    }
}
