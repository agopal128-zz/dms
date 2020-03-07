namespace NDMS.DataAccess.Migrations
{
    using DomainModel;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<NDMS.DataAccess.NDMSDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(NDMS.DataAccess.NDMSDataContext context)
        {
            IList<KPI> kpis = new List<KPI>();
            IList<BusinessSegment> businessSegments = new List<BusinessSegment>();
            IList<ProductLine> productLines = new List<ProductLine>();
            IList<Process> processes = new List<Process>();
            IList<Department> departments = new List<Department>();
            IList<Division> divisions = new List<Division>();
            IList<Facility> allFacilities = new List<Facility>();
            IList<Facility> westernRegionFacilities = new List<Facility>();
            IList<Facility> easternRegionFacilities = new List<Facility>();

            kpis.Add(new KPI() { Id = 1, Name = "Safety", IsActive = true });
            kpis.Add(new KPI() { Id = 2, Name = "Quality", IsActive = true });
            kpis.Add(new KPI() { Id = 3, Name = "Delivery", IsActive = true });
            kpis.Add(new KPI() { Id = 4, Name = "Innovation", IsActive = true });
            kpis.Add(new KPI() { Id = 5, Name = "Cost", IsActive = true });
            kpis.Add(new KPI() { Id = 6, Name = "People [Culture]", IsActive = true });

            allFacilities.Add(new Facility() { Id = 1, Name = "All" });
            //westernRegionFacilities.Add(new Facility() { Id = 2, Name = "All" });

            westernRegionFacilities.Add(new Facility() { Id = 2, Name = "Abu Dhabi" });
            westernRegionFacilities.Add(new Facility() { Id = 3, Name = "Belgium" });
            westernRegionFacilities.Add(new Facility() { Id = 4, Name = "Dubai" });
            westernRegionFacilities.Add(new Facility() { Id = 5, Name = "Russia" });
            westernRegionFacilities.Add(new Facility() { Id = 6, Name = "Saudi Arabia" });
            westernRegionFacilities.Add(new Facility() { Id = 7, Name = "Singapore" });
            westernRegionFacilities.Add(new Facility() { Id = 8, Name = "Stonehouse" });

            easternRegionFacilities.Add(new Facility() { Id = 9, Name = "Conroe" });
            easternRegionFacilities.Add(new Facility() { Id = 10, Name = "Breen" });
            easternRegionFacilities.Add(new Facility() { Id = 11, Name = "Canada" });
            easternRegionFacilities.Add(new Facility() { Id = 12, Name = "Brazil" });

            productLines.Add(new ProductLine() { Id = 1, Name = "All", IsActive = true });
            productLines.Add(new ProductLine() { Id = 2, Name = "Drilling Tools", IsActive = true });
            productLines.Add(new ProductLine() { Id = 3, Name = "Motors", IsActive = true });
            productLines.Add(new ProductLine() { Id = 4, Name = "Fishing Tools", IsActive = true });
            productLines.Add(new ProductLine() { Id = 5, Name = "Intervention and Coiled Tubing", IsActive = true });
            productLines.Add(new ProductLine() { Id = 6, Name = "Fixed Cutter Bits", IsActive = true });
            productLines.Add(new ProductLine() { Id = 7, Name = "Coring Systems", IsActive = true });
            productLines.Add(new ProductLine() { Id = 8, Name = "E Tools", IsActive = true });
            productLines.Add(new ProductLine() { Id = 9, Name = "Service Equipment", IsActive = true });

            divisions.Add(new Division()
            {
                Id = 1,
                Name = "Drilling and Intervention"
            });
            // divisions.Add(new Division() { Name = "Dynamic Drill Solutions", IsActive = true, Regions = regions });

            businessSegments.Add(new BusinessSegment() { Id = 1, Name = "Wellbore Technologies"});
            businessSegments.Add(new BusinessSegment() { Id = 2, Name = "Rig Systems" });
            businessSegments.Add(new BusinessSegment() { Id = 3, Name = "Completions & Productions" });

            processes.Add(new Process() { Id = 1, Name = "All", IsActive = true });
            processes.Add(new Process() { Id = 2, Name = "Machining", IsActive = true });

            departments.Add(new Department() { Id = 1, Name = "All", IsActive = true });
            departments.Add(new Department() { Id = 2, Name = "Production", IsActive = true });
            departments.Add(new Department() { Id = 3, Name = "Planning", IsActive = true });
            departments.Add(new Department() { Id = 4, Name = "Programming", IsActive = true });
            departments.Add(new Department() { Id = 5, Name = "Purchasing", IsActive = true });
            departments.Add(new Department() { Id = 6, Name = "Quality", IsActive = true });
            departments.Add(new Department() { Id = 7, Name = "Shipping", IsActive = true });
            departments.Add(new Department() { Id = 8, Name = "Manufacturing", IsActive = true });
            departments.Add(new Department() { Id = 9, Name = "HSE", IsActive = true });

            //divisions.Add(new Division() {DivisionName="", BusinessSegment=  });

            foreach (KPI kpi in kpis)
            {
                context.KPIs.AddOrUpdate(k => k.Name, kpi);
            }

            foreach (BusinessSegment businessSegment in businessSegments)
            {
                context.BusinessSegments.AddOrUpdate(bs => bs.Name, businessSegment);
            }

            foreach (Process process in processes)
            {
                context.Processes.AddOrUpdate(p => p.Name, process);
            }

            foreach (Department department in departments)
            {
                context.Departments.AddOrUpdate(d => d.Name, department);
            }

        }
    }
}
