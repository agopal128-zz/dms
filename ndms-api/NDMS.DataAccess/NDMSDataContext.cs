using NDMS.DataAccess.EntityConfig;
using NDMS.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace NDMS.DataAccess
{
    public class NDMSDataContext : DbContext
    {
        #region Protected Method(s)
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Find all Generic types in the current assembly that inherit from EntityTypeConfiguration
            // and add them to the model-builder configurations
            IEnumerable<Type> types = Assembly.GetExecutingAssembly().GetTypes().Where(
                t => t.BaseType != null && t.BaseType.IsGenericType &&
                    t.BaseType.GetGenericTypeDefinition() == typeof(BaseEntityTypeConfiguration<>));

            foreach (Type t in types)
            {
                dynamic instance = Activator.CreateInstance(t);
                modelBuilder.Configurations.Add(instance);
            };

            // Add ADUser entity config explictly here since it does not have 
            // any base entity type configuration
            dynamic adUserCfg = Activator.CreateInstance(typeof(ADUserConfig));
            modelBuilder.Configurations.Add(adUserCfg);
            modelBuilder.HasDefaultSchema("ndms");
        }
        #endregion

        #region Constructor(s)
        public NDMSDataContext() : base("name=NDMS")
        {
            this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.ProxyCreationEnabled = true;
            // CEW - This is a hack to ensure that Entity Framework SQL Provider is copied across 
            // to the output folder. As it is installed in the GAC, Copy Local does not work. 
            // It is required for probing. Fixed "Provider not loaded" error
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;

            // Set the initializer to null to disable code first migrations
            Database.SetInitializer<NDMSDataContext>(null);
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="disableAutoDetectChanges">True to disable automatic detect changes</param>
        public NDMSDataContext(bool disableAutoDetectChanges) : this()
        {
            // Disable change tracking, since we don't use same context again for update
            this.Configuration.AutoDetectChangesEnabled = disableAutoDetectChanges;
        }
        #endregion

        #region DBSets
        public virtual DbSet<BusinessSegment> BusinessSegments { get; set; }

        public virtual DbSet<Division> Divisions { get; set; }

        public virtual DbSet<KPI> KPIs { get; set; }

        public virtual DbSet<ScorecardKPIOwner> ScorecardKPIOwners { get; set; }

        public virtual DbSet<Facility> Facilities { get; set; }

        public virtual DbSet<Metric> Metrics { get; set; }

        public virtual DbSet<Process> Processes { get; set; }

        public virtual DbSet<ProductLine> ProductLines { get; set; }

        public virtual DbSet<Scorecard> Scorecards { get; set; }

        public virtual DbSet<ScorecardTeam> ScorecardTeams { get; set; }

        public virtual DbSet<Department> Departments { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<GoalType> GoalTypes { get; set; }

        public virtual DbSet<DataType> DataTypes { get; set; }

        public virtual DbSet<DailyActual> DailyActuals { get; set; }

        public virtual DbSet<MonthlyActual> MonthlyActuals { get; set; }

        public virtual DbSet<Target> Targets { get; set; }

        public virtual DbSet<ADUser> ADUsers { get; set; }

        public virtual DbSet<TargetEntryMethod> TargetEntryMethods { get; set; }
        #endregion
    }
}
