using NDMS.DomainModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;

namespace NDMS.DataAccess.EntityConfig
{
    public class DailyTargetConfig : BaseEntityTypeConfiguration<DailyTarget>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public DailyTargetConfig() : base()
        {
            this.ToTable("DailyTargets").HasKey<int>(e => e.Id); //table primary key
            this.Property(e => e.Id).HasColumnName("ID");
            this.Property(e => e.MonthlyTargetId).HasColumnName("MonthlyTargetID").IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                new IndexAnnotation(new IndexAttribute("IX_MonthlyTargetID_Day", 1) { IsUnique = true }));
            this.Property(e => e.Day).HasColumnName("Day").IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                new IndexAnnotation(new IndexAttribute("IX_MonthlyTargetID_Day", 2) { IsUnique = true }));            
            this.Property(e => e.MaxGoalValue).HasColumnName("MaxGoalValue");
            this.Property(e => e.RolledUpGoalValue).HasColumnName("RolledUpGoalValue");
            this.Property(e => e.IsManual).HasColumnName("IsManual");
            this.Property(e => e.CreatedOn).HasColumnName("CreatedOn");
            this.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
            this.Property(e => e.LastModifiedOn).HasColumnName("LastModifiedOn");
            this.Property(e => e.LastModifiedBy).HasColumnName("LastModifiedBy");
            // Configure the foreign key with "MontlyTargets" table here
            this.HasRequired<MonthlyTarget>(s => s.MonthlyTarget)
                .WithMany(s => s.DailyTargets)
                .HasForeignKey(s => s.MonthlyTargetId)
                .WillCascadeOnDelete(true);
        }
        #endregion
    }
}
