using NDMS.DomainModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;

namespace NDMS.DataAccess.EntityConfig
{
    /// <summary>
    /// Entity configuration class for setting monthly target
    /// </summary>
    public class MonthlyTargetConfig : BaseEntityTypeConfiguration<MonthlyTarget>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MonthlyTargetConfig() : base()
        {
            this.ToTable("MonthlyTargets").HasKey<int>(e => e.Id); //table primary key
            this.Property(e => e.Id).HasColumnName("ID");
            this.Property(e => e.TargetId).HasColumnName("TargetID").IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                new IndexAnnotation(new IndexAttribute("IX_TargetID_Month", 1) { IsUnique = true }));
            this.Property(e => e.Month).HasColumnName("Month").IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                new IndexAnnotation(new IndexAttribute("IX_TargetID_Month", 2) { IsUnique = true }));
            this.Property(e => e.DailyRate).HasColumnName("DailyRate");
            this.Property(e => e.MaxGoalValue).HasColumnName("MaxGoalValue");
            this.Property(e => e.RolledUpGoalValue).HasColumnName("RolledUpGoalValue");
            this.Property(e => e.StretchGoalValue).HasColumnName("StretchGoalValue");
            this.Property(e => e.IsRolledUpGoal).HasColumnName("IsRolledUpGoal");
            this.Property(e => e.CreatedOn).HasColumnName("CreatedOn");
            this.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
            this.Property(e => e.LastModifiedOn).HasColumnName("LastModifiedOn");
            this.Property(e => e.LastModifiedBy).HasColumnName("LastModifiedBy");
            // Configure the foreign key with "Targets" table here
            this.HasRequired<Target>(s => s.Target)
                .WithMany(s => s.MonthlyTargets)
                .HasForeignKey(s => s.TargetId)
                .WillCascadeOnDelete(true);
        }
        #endregion
    }
}
