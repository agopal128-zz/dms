using NDMS.DomainModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;

namespace NDMS.DataAccess.EntityConfig
{
    public class MonthlyActualConfig : BaseEntityTypeConfiguration<MonthlyActual>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MonthlyActualConfig() : base()
        {
            this.ToTable("MonthlyActuals").HasKey<int>(e => e.Id); //table primary key
            this.Property(e => e.Id).HasColumnName("ID");
            this.Property(e => e.TargetId).HasColumnName("TargetID").IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                new IndexAnnotation(new IndexAttribute("IX_TargetID_Month", 1) { IsUnique = true }));
            this.Property(e => e.Month).HasColumnName("Month").IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                new IndexAnnotation(new IndexAttribute("IX_TargetID_Month", 2) { IsUnique = true }));
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

        }
        #endregion
    }
}
