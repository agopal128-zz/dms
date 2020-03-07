// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace NDMS.DependencyRegistry
{
    using Business.Interfaces;
    using Business.Managers;
    using DataAccess;
    using DataAccess.Interfaces;
    using DataAccess.Repositories;
    using DomainModel;
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;
    using StructureMap.Web.Pipeline;
    public class NDMSRegistry : Registry
    {
        #region Constructors and Destructors
        public NDMSRegistry()
        {
            Scan(
                scan =>
                {
                    // Include all business interfaces and repositories by convention. 
                    // Need to make sure that implementation classes inside 
                    // NDMS.Business.Managers, NDMS.DataAccess are named according to
                    // the convention
                    scan.AssemblyContainingType<IScorecardManager>();
                    scan.AssemblyContainingType<ScorecardManager>();
                    scan.AssemblyContainingType<IScorecardRepository>();
                    scan.WithDefaultConventions();

                    // Register all generic repository types here
                    For<IBaseRepository<BusinessSegment>>().Use<BaseRepository<BusinessSegment>>();
                    For<IBaseRepository<CounterMeasure>>().Use<BaseRepository<CounterMeasure>>();
                    For<IBaseRepository<CounterMeasureComment>>().
                        Use<BaseRepository<CounterMeasureComment>>();
                    For<IBaseRepository<CounterMeasureStatus>>().
                        Use<BaseRepository<CounterMeasureStatus>>();
                    For<IBaseRepository<DailyActual>>().Use<BaseRepository<DailyActual>>();
                    For<IBaseRepository<DailyActualHistory>>().
                        Use<BaseRepository<DailyActualHistory>>();
                    For<IBaseRepository<DailyTarget>>().Use<BaseRepository<DailyTarget>>();
                    For<IBaseRepository<DailyTargetHistory>>().
                        Use<BaseRepository<DailyTargetHistory>>();
                    For<IBaseRepository<DataType>>().Use<BaseRepository<DataType>>();
                    For<IBaseRepository<Department>>().Use<BaseRepository<Department>>();
                    For<IBaseRepository<Division>>().Use<BaseRepository<Division>>();
                    For<IBaseRepository<Facility>>().Use<BaseRepository<Facility>>();
                    For<IBaseRepository<GoalType>>().Use<BaseRepository<GoalType>>();
                    For<IBaseRepository<GraphPlottingMethod>>().
                        Use<BaseRepository<GraphPlottingMethod>>();
                    For<IBaseRepository<KPI>>().Use<BaseRepository<KPI>>();
                    For<IBaseRepository<Metric>>().Use<BaseRepository<Metric>>();
                    For<IBaseRepository<MetricMapping>>().
                      Use<BaseRepository<MetricMapping>>();
                    For<IBaseRepository<MonthlyActual>>().Use<BaseRepository<MonthlyActual>>();
                    For<IBaseRepository<MonthlyActualHistory>>().
                        Use<BaseRepository<MonthlyActualHistory>>();
                    For<IBaseRepository<MonthlyTarget>>().Use<BaseRepository<MonthlyTarget>>();
                    For<IBaseRepository<MonthlyTargetHistory>>().
                        Use<BaseRepository<MonthlyTargetHistory>>();
                    For<IBaseRepository<Process>>().Use<BaseRepository<Process>>();
                    For<IBaseRepository<ProductLine>>().Use<BaseRepository<ProductLine>>();
                    For<IBaseRepository<RollupMethod>>().Use<BaseRepository<RollupMethod>>();
                    For<IBaseRepository<Scorecard>>().Use<BaseRepository<Scorecard>>();
                    For<IBaseRepository<ScorecardKPIOwner>>().
                        Use<BaseRepository<ScorecardKPIOwner>>();
                    For<IBaseRepository<ScorecardTeam>>().Use<BaseRepository<ScorecardTeam>>();
                    For<IBaseRepository<Target>>().Use<BaseRepository<Target>>();
                    For<IBaseRepository<TargetHistory>>().Use<BaseRepository<TargetHistory>>();
                    For<IBaseRepository<TrackingMethod>>().Use<BaseRepository<TrackingMethod>>();
                    For<IBaseRepository<User>>().Use<BaseRepository<User>>();
                    For<IBaseRepository<Year>>().Use<BaseRepository<Year>>();
                    For<IBaseRepository<TargetEntryMethod>>().Use<BaseRepository<TargetEntryMethod>>();
                    For<IBaseRepository<CounterMeasurePriority>>().Use<BaseRepository<CounterMeasurePriority>>();
                    For<IBaseRepository<Recordable>>().Use<BaseRepository<Recordable>>(); 
                    For<IBaseRepository<HolidayPattern>>().Use<BaseRepository<HolidayPattern>>(); 
                    For<IBaseRepository<HolidayPatternInfo>>().Use<BaseRepository<HolidayPatternInfo>>(); 
                    For<IBaseRepository<ScorecardWorkdayTracker>>().Use<BaseRepository<ScorecardWorkdayTracker>>(); 
                    For<IBaseRepository<ScorecardWorkdayPattern>>().Use<BaseRepository<ScorecardWorkdayPattern>>();
                    For<IBaseRepository<ScorecardHolidayPattern>>().Use<BaseRepository<ScorecardHolidayPattern>>();
                    For<IBaseRepository<OrganizationalData>>().Use<BaseRepository<OrganizationalData>>();

                    // Need to explicitly specify the Data context with transient life cycle
                    For<NDMSDataContext>().Use(x => new NDMSDataContext()).Transient();

                });

        }
        #endregion
    }
}