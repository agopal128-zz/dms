using NDMS.DomainModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;

namespace NDMS.DataAccess.EntityConfig
{
    /// <summary>
    /// Entity configuration class for Scorecard entity
    /// </summary>
    public class ScorecardConfig : BaseEntityTypeConfiguration<Scorecard>
    {
        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ScorecardConfig() : base()
        {
            this.ToTable("Scorecards").HasKey<int>(e => e.Id); //table primary key
            this.Property(e => e.Id).HasColumnName("ID");
            this.Property(e => e.Name).HasColumnName("Name").HasMaxLength(100).IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                new IndexAnnotation(new IndexAttribute("IX_Name_Division_Facility", 1) { IsUnique = true }));
            this.Property(e => e.ParentScorecardId).HasColumnName("ParentScorecardID");            
            this.Property(e => e.IsBowlingChartApplicable).HasColumnName("IsBowlingChartApplicable");
            this.Property(e => e.DrilldownLevel).HasColumnName("DrilldownLevel");
            this.Property(e => e.SortOrder).HasColumnName("SortOrder").IsRequired();
            this.Property(e => e.CreatedOn).HasColumnName("CreatedOn").IsRequired();
            this.Property(e => e.CreatedBy).HasColumnName("CreatedBy").IsRequired();
            this.Property(e => e.IsActive).HasColumnName("IsActive").IsRequired();
            this.Property(e => e.LastModifiedOn).HasColumnName("LastModifiedOn").IsRequired();
            this.Property(e => e.LastModifiedBy).HasColumnName("LastModifiedBy").IsRequired();            
            // Configuring the self referencing here
            this.HasOptional(e => e.ParentScorecard).WithMany(e => e.ChildScorecards).
                HasForeignKey(e => e.ParentScorecardId);
            // Configuring many to many relationship between score card and KPI
            this.HasMany<KPI>(s => s.KPIs)
                .WithMany(c => c.Scorecards)
                .Map(cs =>
                {
                    cs.MapLeftKey("ScorecardRefId");
                    cs.MapRightKey("KPIRefId");
                    cs.ToTable("ScorecardKPI");
                });
            this.HasMany<BusinessSegment>(s => s.BusinessSegments)
               .WithMany(c => c.Scorecards)
               .Map(cs =>
               {
                   cs.MapLeftKey("ScorecardRefId");
                   cs.MapRightKey("BussinessSegmentRefId");
                   cs.ToTable("ScorecardBusinessSegments");
               });
            this.HasMany<Division>(s => s.Divisions)
               .WithMany(c => c.Scorecards)
               .Map(cs =>
               {
                   cs.MapLeftKey("ScorecardRefId");
                   cs.MapRightKey("DivisionRefId");
                   cs.ToTable("ScorecardDivisions");
               });
            this.HasMany<Facility>(s => s.Facilities)
              .WithMany(c => c.Scorecards)
              .Map(cs =>
              {
                  cs.MapLeftKey("ScorecardRefId");
                  cs.MapRightKey("FacilityRefId");
                  cs.ToTable("ScorecardFacilities");
              });
            this.HasMany<ProductLine>(s => s.ProductLines)
             .WithMany(c => c.Scorecards)
             .Map(cs =>
             {
                 cs.MapLeftKey("ScorecardRefId");
                 cs.MapRightKey("ProductLineRefId");
                 cs.ToTable("ScorecardProductLines");
             }); 
            this.HasMany<Department>(s => s.Departments)
            .WithMany(c => c.Scorecards)
            .Map(cs =>
            {
                cs.MapLeftKey("ScorecardRefId");
                cs.MapRightKey("DepartmentRefId");
                cs.ToTable("ScorecardDepartments");
            });
            this.HasMany<Process>(s => s.Processes)
            .WithMany(c => c.Scorecards)
            .Map(cs =>
            {
                cs.MapLeftKey("ScorecardRefId");
                cs.MapRightKey("ProcessRefId");
                cs.ToTable("ScorecardProcesses");
            });

        }
        #endregion
    }
}
