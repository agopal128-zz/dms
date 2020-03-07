using NDMS.DomainModel;

namespace NDMS.DataAccess.EntityConfig
{
    internal class ScorecardDepartmentConfig : BaseEntityTypeConfiguration<ScorecardDepartment>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ScorecardDepartmentConfig() : base()
        {

            this.Property(e => e.ScorecardRefId).HasColumnName("ScorecardRefId").IsRequired();
            this.Property(e => e.DepartmentRefId).HasColumnName("DepartmentRefId").IsRequired();

            // Configure the foreign key with "Scorecards" table here
            this.HasRequired<Scorecard>(s => s.Scorecard).WithMany()
                .HasForeignKey(s => s.ScorecardRefId)
                .WillCascadeOnDelete(true);
            // Configure the foreign key with "Departments" table here
            this.HasRequired<Department>(s => s.Department)
                .WithMany()
                .HasForeignKey(s => s.DepartmentRefId)
                .WillCascadeOnDelete(true);

            //this.HasRequired<Scorecard>(e => e.Scorecard).WithMany(e => e.ScorecardDepartments).
            //HasForeignKey(e => e.ScorecardRefId).WillCascadeOnDelete(true);

        }
        #endregion
    }
}
