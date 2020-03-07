using NDMS.Business.Common;
using NDMS.Business.Converters;
using NDMS.Business.Interfaces;
using NDMS.DataAccess.Interfaces;
using NDMS.DomainModel;
using NDMS.DomainModel.DTOs;
using NDMS.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NDMS.Business.Managers
{
    public class CounterMeasureManager : ICounterMeasureManager
    {
        #region Field(s)
        /// <summary>
        /// Counter Measure repository
        /// </summary>
        private IBaseRepository<CounterMeasure> counterMeasureRepository;

        /// <summary>
        /// Counter Measure Status repository
        /// </summary>
        private IBaseRepository<CounterMeasureStatus> counterMeasureStatusRepository;

        /// <summary>
        /// The counter measure priority repository
        /// </summary>
        private IBaseRepository<CounterMeasurePriority> counterMeasurePriorityRepository;

        /// <summary>
        /// The scorecard holiday pattern repository
        /// </summary>
        private IBaseRepository<ScorecardHolidayPattern> scorecardHolidayPatternRepository;

        /// <summary>
        /// The scorecard workday pattern repository
        /// </summary>
        private IBaseRepository<ScorecardWorkdayPattern> scorecardWorkdayPatternRepository;

        /// <summary>
        /// The scorecard workday tracker repository
        /// </summary>
        private IBaseRepository<ScorecardWorkdayTracker> scorecardWorkdayTrackerRepository;

        /// <summary>
        /// User repository
        /// </summary>
        private INDMSUserRepository userRepository;

        /// <summary>
        /// Target repository
        /// </summary>
        private IBaseRepository<Target> targetRepository;

        /// <summary>
        /// Reference to ScorecardGoalCalculator
        /// </summary>
        private ScorecardActualCalculator actualCalculator;

        /// <summary>
        /// Reference to ScorecardGoalCalculator
        /// </summary>
        private ScorecardGoalCalculator goalCalculator;

        /// <summary>
        /// Year Repository
        /// </summary>
        private IBaseRepository<Year> yearRepository;
        #endregion

        #region Private Method(s)
        /// <summary>
        /// Method to validate due date of counter measure
        /// </summary>
        /// <param name="date">counter measure due date</param>
        private void ValidateCounterMeasureDueDate(DateTime date)
        {
            //check whether the due date is a past date
            if (date < TimeZoneUtility.GetCurrentTimestamp().Date)
            {
                throw new NDMSBusinessException(Constants.CounterMeasureDueDateErrorMessage);
            }
        }

        /// <summary>
        /// Method to check whether any outstanding counter measure exists for a metric 
        /// which belongs to a score card and KPI
        /// </summary>
        /// <param name="target">target item</param>
        /// <returns>flag which says whether counter measure already exists or not</returns>
        private bool CheckIfAnyOutstandingCounterMeasureExists(Target target)
        {
            bool outstandingCounterMeasureExists = counterMeasureRepository.GetAll().Any(x =>
                    x.ScorecardId == target.ScorecardId &&
                    x.KPIId == target.KPIId && x.MetricId == target.MetricId &&
                    x.CounterMeasureStatusId != Constants.CounterMeasureStatusConfirmed);
            return outstandingCounterMeasureExists;
        }
        #endregion

        #region Protected Method(s)
        /// <summary>
        /// Creates an instance of ScorecardActualCalculator and returns
        /// </summary>
        /// <returns></returns>
        protected virtual ScorecardActualCalculator CreateScorecardActualCalculator(
            IBaseRepository<DailyActual> dailyActualRepository)
        {
            if (actualCalculator == null)
            {
                actualCalculator = new ScorecardActualCalculator(dailyActualRepository);
            }
            return actualCalculator;
        }

        /// <summary>
        /// Creates an instance of ScorecardGoalCalculator and returns
        /// </summary>
        /// <returns></returns>
        protected virtual ScorecardGoalCalculator CreateScorecardGoalCalculator(
            IBaseRepository<Target> targetRepository,
            IBaseRepository<DailyActual> dailyActualRepository,
            IBaseRepository<ScorecardWorkdayPattern> ScorecardWorkdayPatternRepository,
            IBaseRepository<ScorecardHolidayPattern> scorecardHolidayPatternRepository,
            IBaseRepository<ScorecardWorkdayTracker> scorecardWorkdayTrackerRepository)
        {
            if (goalCalculator == null)
            {
                goalCalculator = new ScorecardGoalCalculator(targetRepository, dailyActualRepository,
                    ScorecardWorkdayPatternRepository, scorecardHolidayPatternRepository, scorecardWorkdayTrackerRepository);
            }
            return goalCalculator;
        }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="counterMeasureRepository">Counter measure repository</param>
        /// <param name="counterMeasureStatusRepository">Counter measure status repository</param>
        /// <param name="userRepository">User repository</param>
        /// <param name="targetRepository">Target repository</param>
        /// <param name="dailyActualRepository">The daily actual repository.</param>
        /// <param name="holidayRepository">The holiday repository.</param>
        /// <param name="yearRepository">The year repository.</param>
        /// <param name="counterMeasurePriorityRepository">The counter measure priority repository.</param>
        /// <exception cref="System.ArgumentNullException">Repository - The given parameter cannot be null.</exception>
        public CounterMeasureManager(IBaseRepository<CounterMeasure> counterMeasureRepository,
            IBaseRepository<CounterMeasureStatus> counterMeasureStatusRepository,
            INDMSUserRepository userRepository, IBaseRepository<Target> targetRepository,
            IBaseRepository<DailyActual> dailyActualRepository,
            IBaseRepository<ScorecardWorkdayPattern> scorecardWorkdayPatternRepository,
            IBaseRepository<ScorecardHolidayPattern> scorecardHolidayPatternRepository,
            IBaseRepository<ScorecardWorkdayTracker> scorecardWorkdayTrackerRepository,
            IBaseRepository<Year> yearRepository,
            IBaseRepository<CounterMeasurePriority> counterMeasurePriorityRepository)
        {
            if (counterMeasureRepository == null || counterMeasureStatusRepository == null ||
                userRepository == null || targetRepository == null ||
                dailyActualRepository == null || scorecardWorkdayPatternRepository == null || yearRepository == null ||
                counterMeasurePriorityRepository == null || scorecardHolidayPatternRepository == null ||
                scorecardWorkdayTrackerRepository == null)
            {
                throw new ArgumentNullException("Repository",
                    "The given parameter cannot be null.");
            }
            this.counterMeasureRepository = counterMeasureRepository;
            this.counterMeasureStatusRepository = counterMeasureStatusRepository;
            this.userRepository = userRepository;
            this.targetRepository = targetRepository;
            this.yearRepository = yearRepository;
            this.scorecardHolidayPatternRepository = scorecardHolidayPatternRepository;
            this.counterMeasurePriorityRepository = counterMeasurePriorityRepository;
            this.scorecardWorkdayPatternRepository = scorecardWorkdayPatternRepository;
            this.scorecardWorkdayTrackerRepository = scorecardWorkdayTrackerRepository;
            this.actualCalculator = CreateScorecardActualCalculator(dailyActualRepository);
            this.goalCalculator = CreateScorecardGoalCalculator(targetRepository,
                dailyActualRepository, scorecardWorkdayPatternRepository, 
                scorecardHolidayPatternRepository, scorecardWorkdayTrackerRepository);
        }
        #endregion

        #region Public Method(s)
        /// <summary>
        /// Retrieves all counter measures related to a KPI of a particular scorecard for 
        /// an year
        /// </summary>
        /// <param name="scorecardId">Scorecard Identifier</param>
        /// <param name="kpiId">KPI Identifier</param>
        /// <returns>All Counter Measures related to the KPI</returns>
        public IEnumerable<CounterMeasureItem> GetCounterMeasures(int scorecardId, int kpiId,
            bool isShowClosed)
        {
            DateTime currentDate = TimeZoneUtility.GetCurrentTimestamp().Date;

            // get all counter measures except the confirmed counter measure if isShowClosed is false
            // otherwise get all counter measures 
            var counterMeasures = counterMeasureRepository.GetAll()
                .Where(x => x.ScorecardId == scorecardId &&
                x.KPIId == kpiId && 
                x.CounterMeasureStatusId != (isShowClosed ? 
                -1 : Constants.CounterMeasureStatusConfirmed))
                .OrderByDescending(x => x.CreatedOn)
                .ToList().Select(x => CounterMeasureConverters.
                                    ConvertCounterMeasureToCounterMeasureItemDTO(x));
            return counterMeasures.ToList();
        }

        /// <summary>
        /// Retrieves  counter measure details of selected counter measure
        /// </summary>
        /// <param name="counterMeasureId">Counter Measure Id</param>
        /// <returns>Counter Measure DTO</returns>
        public CounterMeasureItem GetCounterMeasure(int counterMeasureId)
        {
            var counterMeasure = counterMeasureRepository.Get(counterMeasureId);

            if (counterMeasure != null)
            {
                return CounterMeasureConverters.
                                    ConvertCounterMeasureToCounterMeasureItemDTO(counterMeasure);
            }

            return null;
        }

        /// <summary>
        /// Retrieves all counter measure status
        /// </summary>
        /// <returns>counter measure status list</returns>
        public IEnumerable<CounterMeasureStatusItem> GetAllCounterMeasureStatus()
        {
            return counterMeasureStatusRepository.GetAll().Where(x => x.IsActive)
                  .Select(x => new CounterMeasureStatusItem()
                  {
                      Id = x.Id,
                      Status = x.Status
                  }).ToList();
        }

        /// <summary>
        /// Add a new counter measure for a metric which belongs to a score card and KPI
        /// </summary>
        /// <param name="counterMeasureRequest">new counter measure entry</param>
        /// <param name="userName">Logged in user name</param>
        public void AddCounterMeasure(CounterMeasureAddRequest counterMeasureRequest,
            string userName)
        {
            ValidateCounterMeasureDueDate(counterMeasureRequest.DueDate);
            // Get logged in user id
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(
               x => x.AccountName == userName).Id;

            // If the assigned user is not added to NDMS users table, add it here
            var assignedUserIds = userRepository.AddADUsersToNDMS(new List<string>() {
                    counterMeasureRequest.AssignedTo
                });

            var counterMeasure = new CounterMeasure()
            {
                ScorecardId = counterMeasureRequest.ScorecardId.Value,
                KPIId = counterMeasureRequest.KPIId.Value,
                MetricId = counterMeasureRequest.MetricId.Value,
                Issue = counterMeasureRequest.Issue,
                Action = counterMeasureRequest.Action,
                AssignedTo = assignedUserIds.First(),
                DueDate = counterMeasureRequest.DueDate,
                CounterMeasureStatusId = counterMeasureRequest.CounterMeasureStatusId.Value,
                CounterMeasurePriorityId = counterMeasureRequest.CounterMeasurePriorityId != null ? 
                                                 counterMeasureRequest.CounterMeasurePriorityId.Value:
                                                 (int?) null,
                CreatedBy = loggedInUserId,
                LastModifiedBy = loggedInUserId,
                CreatedOn = TimeZoneUtility.GetCurrentTimestamp(),
                LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp(),
                CounterMeasureComments = !string.IsNullOrEmpty(counterMeasureRequest.Comment) ?
                new List<CounterMeasureComment>(){ CounterMeasureConverters.
                        ConvertCommentToCounterMeasureComment(counterMeasureRequest.Comment,
                        loggedInUserId)
                } : null
            };
            counterMeasureRepository.AddOrUpdate(counterMeasure);
            counterMeasureRepository.Save();
        }

        /// <summary>
        /// Method to update existing counter measure
        /// </summary>
        /// <param name="counterMeasureRequest">counter measure to be updated</param>
        /// <param name="userName">logged in user name</param>
        public void EditCounterMeasure(CounterMeasureEditRequest counterMeasureRequest,
            string userName)
        {
            //get counter measure entity based on counter measure id
            var existingCounterMeasure = counterMeasureRepository.Get
                (counterMeasureRequest.CounterMeasureId.Value);

            if (existingCounterMeasure != null)
            {
                if (existingCounterMeasure.CounterMeasureStatusId ==
                    Constants.CounterMeasureStatusConfirmed)
                {
                    throw new NDMSBusinessException
                        (Constants.ConfirmedCounterMeasureEditErrorMessage);
                }

                if (existingCounterMeasure.DueDate != counterMeasureRequest.DueDate)
                {
                    ValidateCounterMeasureDueDate(counterMeasureRequest.DueDate);
                }

                // Get logged in user id
                int loggedInUserId = userRepository.GetAll().FirstOrDefault(
                    x => x.AccountName == userName).Id;

                // If the assigned user is not added to NDMS users table, add it here
                var assignedUserIds = userRepository.AddADUsersToNDMS(new List<string>() {
                    counterMeasureRequest.AssignedTo
                });

                //update changes to existing counter measure entity
                existingCounterMeasure.CounterMeasurePriorityId = counterMeasureRequest.CounterMeasurePriorityId != null ?
                                                                        counterMeasureRequest.CounterMeasurePriorityId.Value :
                                                                        (int?) null;
                existingCounterMeasure.Action = counterMeasureRequest.Action;
                existingCounterMeasure.AssignedTo = assignedUserIds.First();
                existingCounterMeasure.DueDate = counterMeasureRequest.DueDate;
                existingCounterMeasure.LastModifiedBy = loggedInUserId;
                existingCounterMeasure.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
                existingCounterMeasure.CounterMeasureStatusId = counterMeasureRequest
                    .CounterMeasureStatusId.Value;
                if (!string.IsNullOrEmpty(counterMeasureRequest.Comment))
                {
                    // Add new comment if there are no comments
                    if (existingCounterMeasure.CounterMeasureComments == null)
                    {
                        existingCounterMeasure.CounterMeasureComments =
                            new List<CounterMeasureComment>()
                                {
                                    CounterMeasureConverters.ConvertCommentToCounterMeasureComment
                                        (counterMeasureRequest.Comment,loggedInUserId)
                                };
                    }
                    else
                    {
                        //else add new comment.
                        existingCounterMeasure.CounterMeasureComments.Add(
                            CounterMeasureConverters.ConvertCommentToCounterMeasureComment
                                    (counterMeasureRequest.Comment, loggedInUserId)
                            );
                    }
                }
                counterMeasureRepository.Save();
            }
        }

        /// <summary>
        /// Method to get active counter measure count for a KPI which belongs to scorecard
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="kpiId">KPI Id</param>
        /// <returns>counter measure count</returns>
        public int GetCounterMeasureCount(int scorecardId, int kpiId)
        {
            DateTime currentDate = TimeZoneUtility.GetCurrentTimestamp().Date;

            //get count of all counter measure which are not confirmed
            int counterMeasureCount = counterMeasureRepository.GetAll()
                .Count(x => x.ScorecardId == scorecardId &&
                x.KPIId == kpiId &&
                x.CounterMeasureStatusId != Constants.CounterMeasureStatusConfirmed);

            return counterMeasureCount;
        }

        /// <summary>
        /// Method to check whether target is achieved or not by comparing goal and actuals based on 
        /// goal type and return status and outstanding counter measure flag if any 
        /// </summary>
        /// <param name="actualRequest">Actual Value</param>
        /// <returns>Actual and counter measure status</returns>
        public ActualandCounterMeasureStatus GetActualandCounterMeasureStatus(
            ActualStatusRequest actualRequest)
        {
            // Round the actual and goal value to two decimal places
            ActualUtility.RoundActualStatusRequest(actualRequest);

            // Find target with the given id
            var target = targetRepository.Get(actualRequest.TargetId);
            if (target != null && target.IsActive)
            {
                if (target.GraphPlottingMethodId.HasValue &&
                target.GraphPlottingMethodId == Constants.GraphPlottingMethodCumulative &&
                target.TrackingMethodId == Constants.TrackingMethodDaily)
                {
                    actualRequest.ActualValue = actualCalculator.CalculateCumulativeActual(
                        actualRequest.TargetId, actualRequest.SelectedDate, actualRequest.ActualValue) ?? 0;
                    actualRequest.GoalValue = goalCalculator.CalculateCumulativeGoal(
                        target, actualRequest.SelectedDate);
                }


                var actualandCounterMeasureStatus = new ActualandCounterMeasureStatus();

                actualandCounterMeasureStatus.ActualStatus = TargetActualComparer.GetActualStatus(
                    actualRequest.GoalValue, actualRequest.ActualValue, target.Metric.GoalTypeId);

                //if status is not met check for outstanding counter measures
                if (actualandCounterMeasureStatus.ActualStatus == ActualStatus.NotAchieved)
                {
                    actualandCounterMeasureStatus.OutstandingCounterMeasureExists =
                        CheckIfAnyOutstandingCounterMeasureExists(target);
                }
                return actualandCounterMeasureStatus;
            }

            else {
                throw new NDMSBusinessException(Constants.TargetNotFound);
            }
        }

        /// <summary>
        /// Method to retrieve metrics belonging to a kpi in a scorecard
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="kpiId">KPI Id</param>
        /// <param name="yearId">Year Id</param>
        /// <param name="month">Month</param>
        /// <returns>metric list</returns>
        public List<MetricItem> GetScorecardKPIMetrics(int scorecardId, int kpiId, int month,
            int yearId)
        {
            var calendarYear = yearRepository.Get(yearId);
            //form a date using the given month and the corresponding year
            int year = CalendarUtility.GetYearOfTheMonth(calendarYear, month);
            var requestedDate = new DateTime(year, month, 1);

            //get all metrics associated with the scorecard kpi
            var scorecardKPIMetricList = targetRepository.GetAll().Where(x =>
            x.ScorecardId == scorecardId && x.KPIId == kpiId && x.CalendarYearId == yearId && x.IsActive);
            //get the metrics which are active in the selected month by comparing target 
            //start date (set day as 1) and target end date (set day as last day of the month)
            var scorecardKPIMetrics = scorecardKPIMetricList.ToList().Where(x =>
                        requestedDate >= new DateTime
                        (x.EffectiveStartDate.Year, x.EffectiveStartDate.Month, 1)
                        && requestedDate <= new DateTime
                        (x.EffectiveEndDate.Year, x.EffectiveEndDate.Month,
                        DateTime.DaysInMonth(x.EffectiveEndDate.Year, x.EffectiveEndDate.Month)));

            return scorecardKPIMetrics.Select(x => new MetricItem()
            {
                Id = x.MetricId,
                Name = x.Metric.Name
            }).ToList();

        }

        /// <summary>
        /// Gets all counter measure priority.
        /// </summary>
        /// <returns>The counter measure priority list</returns>
        public List<CounterMeasurePriorityItem> GetAllCounterMeasurePriority()
        {
            return counterMeasurePriorityRepository.GetAll().Where(x => x.IsActive)
                  .Select(x => new CounterMeasurePriorityItem()
                  {
                      Id = x.Id,
                      Name = x.Name
                  }).OrderBy(y => y.Name).ToList();
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (counterMeasureRepository != null)
                    {
                        counterMeasureRepository.Dispose();
                    }
                    if (counterMeasureStatusRepository != null)
                    {
                        counterMeasureStatusRepository.Dispose();
                    }
                    if (userRepository != null)
                    {
                        userRepository.Dispose();
                    }
                    if (targetRepository != null)
                    {
                        targetRepository.Dispose();
                    }
                    if (yearRepository != null)
                    {
                        yearRepository.Dispose();
                    }
                    if(scorecardWorkdayPatternRepository != null)
                    {
                        scorecardWorkdayPatternRepository.Dispose();
                    }
                    if (scorecardWorkdayTrackerRepository != null)
                    {
                        scorecardWorkdayTrackerRepository.Dispose();
                    }
                    if (scorecardHolidayPatternRepository != null)
                    {
                        scorecardHolidayPatternRepository.Dispose();
                    }
                    counterMeasureRepository = null;
                    counterMeasureStatusRepository = null;
                    userRepository = null;
                    targetRepository = null;
                    yearRepository = null;
                    scorecardWorkdayPatternRepository = null;
                    scorecardWorkdayTrackerRepository = null;
                    scorecardHolidayPatternRepository = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
