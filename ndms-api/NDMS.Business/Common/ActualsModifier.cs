using NDMS.Business.Converters;
using NDMS.DataAccess.Interfaces;
using NDMS.DomainModel;
using NDMS.DomainModel.DTOs;
using NDMS.DomainModel.Enums;
using NDMS.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NDMS.Business.Common
{
    /// <summary>
    /// Implements methods which helps to Add/Update Score card actuals
    /// </summary>
    public class ActualsModifier
    {
        #region Field(s)
        /// <summary>
        /// Daily actual repository
        /// </summary>
        private IBaseRepository<DailyActual> dailyActualRepository;

        /// <summary>
        /// Monthly actual repository
        /// </summary>
        private IBaseRepository<MonthlyActual> monthlyActualRepository;

        /// <summary>
        /// Target Repository
        /// </summary>
        private IBaseRepository<Target> targetRepository;

        /// <summary>
        /// User repository
        /// </summary>
        private IBaseRepository<User> userRepository;

        /// <summary>
        /// Reference to ScorecardActualCalculator
        /// </summary>
        private ScorecardActualCalculator actualCalculator;

        /// <summary>
        /// Reference to ScorecardGoalCalculator
        /// </summary>
        private ScorecardGoalCalculator goalCalculator;

        /// <summary>
        /// Monthly Target repository
        /// </summary>
        private IBaseRepository<MonthlyTarget> monthlyTargetRepository;

        /// <summary>
        /// Daily target repository
        /// </summary>
        private IBaseRepository<DailyTarget> dailyTargetRepository;      

        /// <summary>
        /// Daily target history repository
        /// </summary>
        private IBaseRepository<DailyTargetHistory> dailyTargetHistoryRepository;
        #endregion

        #region Private Method(s)
        /// <summary>
        /// Adjusts(add or update)monthly actual when adding a new daily actual
        /// </summary>
        /// <param name="actualRequest">actual request</param>
        /// <param name="goalTypeId">goal type id</param>
        /// <param name="dataTypeId">data type id</param>
        /// <param name="loggedInUserId">logged in user id</param>
        /// <returns>Adjusted monthly Actual Entity after including daily target</returns>
        private MonthlyActual AdjustMonthlyActual(ActualItem actualRequest, int goalTypeId,
            int dataTypeId, int loggedInUserId)
        {
            // Get existing monthly actual entry if exists
            var existingMonthlyActual = monthlyActualRepository.GetAll().FirstOrDefault(x =>
                x.TargetId == actualRequest.TargetId &&
                x.Month == actualRequest.Date.Month);

            if (existingMonthlyActual != null)
            {
                // updating existing monthly actual
                return UpdateExistingMonthlyActualOfDailyActual(existingMonthlyActual,
                     actualRequest, goalTypeId, dataTypeId, loggedInUserId);
            }
            else
            {
                DateTime curTimestamp = TimeZoneUtility.GetCurrentTimestamp();
                // get monthly target
                decimal? monthlyTarget = goalCalculator.GetMonthlyGoal(actualRequest.TargetId,
                    actualRequest.Date.Month);
                // Assign the monthly target instead of daily target, because we
                // need to create monthly actual here
                actualRequest.GoalValue = monthlyTarget;
                return CreateMonthlyActual(actualRequest, goalTypeId, loggedInUserId);
            }
        }

        /// <summary>
        /// Method to get sum of daily actuals excluding current date
        /// </summary>
        /// <param name="targetId">target id</param>
        /// <param name="date">Date which needs to be excluded</param>
        /// <returns>Sum of daily actual</returns>
        private decimal FindSumOfDailyActualsExcludingCurrentEntryDate(int targetId, DateTime date)
        {
            var dailyActuals = dailyActualRepository.GetAll().Where(x => x.TargetId == targetId &&
                x.Date.Month == date.Month && x.Date != date && x.ActualValue.HasValue);

            return (dailyActuals.Count() > 0) ? dailyActuals.Sum(x => x.ActualValue.Value) : 0;
        }

        /// <summary>
        /// Create MonthlyActual entity from ActualEntry request
        /// </summary>
        /// <param name="actualRequest">Actual entry</param>
        /// <param name="goalTypeId">Goal type id</param>
        /// <param name="loggedInUserId">Logged in user id</param>
        /// <returns></returns>
        private MonthlyActual CreateMonthlyActual(ActualItem actualRequest, int goalTypeId,
            int loggedInUserId)
        {
            DateTime curTimestamp = TimeZoneUtility.GetCurrentTimestamp();
            var monthlyActual = new MonthlyActual()
            {
                Month = actualRequest.Date.Month,
                ActualValue = actualRequest.ActualValue,
                Status = TargetActualComparer.GetActualStatus(actualRequest.GoalValue,
                    actualRequest.ActualValue, goalTypeId),
                TargetId = actualRequest.TargetId,
                CreatedOn = curTimestamp,
                LastModifiedOn = curTimestamp,
                CreatedBy = loggedInUserId,
                LastModifiedBy = loggedInUserId
            };
            monthlyActual.MonthlyActualHistory = new List<MonthlyActualHistory>() {
                        ActualConverters.ConvertMonthlyActualToMonthlyActualHistory(monthlyActual)
            };
            return monthlyActual;
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
            IBaseRepository<ScorecardWorkdayPattern> scorecardWorkdayPatternRepository,
            IBaseRepository<ScorecardHolidayPattern> scorecardHolidayPatternRepository,
            IBaseRepository<ScorecardWorkdayTracker> scorecardWorkdayTrackerRepository
            )
        {
            if (goalCalculator == null)
            {
                goalCalculator = new ScorecardGoalCalculator(targetRepository, dailyActualRepository, scorecardWorkdayPatternRepository,
                    scorecardHolidayPatternRepository, scorecardWorkdayTrackerRepository);
            }
            return goalCalculator;
        }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="targetRepository">Target Repository</param>
        /// <param name="dailyActualRepository">Daily Actual Repository</param>
        /// <param name="monthlyActualRepository">Monthly Actual Repository</param>
        /// <param name="userRepository">User Repository</param>
        /// <param name="scorecardWorkdayPatternRepository">The scorecard workday pattern repository.</param>
        /// <param name="scorecardHolidayPatternRepository">The scorecard holiday pattern repository.</param>
        /// <param name="scorecardWorkdayTrackerRepository">The scorecard workday tracker repository.</param>
        /// <exception cref="System.ArgumentNullException">Repository - The given parameter cannot be null.</exception>
        public ActualsModifier(IBaseRepository<Target> targetRepository,
            IBaseRepository<DailyActual> dailyActualRepository,
            IBaseRepository<MonthlyActual> monthlyActualRepository,
            IBaseRepository<User> userRepository,
            IBaseRepository<ScorecardWorkdayPattern> scorecardWorkdayPatternRepository,
            IBaseRepository<ScorecardHolidayPattern> scorecardHolidayPatternRepository,
            IBaseRepository<ScorecardWorkdayTracker> scorecardWorkdayTrackerRepository,
            IBaseRepository<MonthlyTarget> monthlyTargetRepository,
            IBaseRepository<DailyTarget> dailyTargetRepository,
           IBaseRepository<DailyTargetHistory> dailyTargetHistoryRepository)
         {
            if (targetRepository == null || dailyActualRepository == null ||
                monthlyActualRepository == null || userRepository == null || monthlyTargetRepository == null 
                || dailyTargetRepository == null || dailyTargetHistoryRepository == null)
            {
                throw new ArgumentNullException("Repository",
                    "The given parameter cannot be null.");
            }
            this.targetRepository = targetRepository;
            this.dailyActualRepository = dailyActualRepository;
            this.monthlyActualRepository = monthlyActualRepository;
            this.userRepository = userRepository;
            this.monthlyTargetRepository = monthlyTargetRepository;
            this.dailyTargetRepository = dailyTargetRepository;
            this.dailyTargetHistoryRepository = dailyTargetHistoryRepository;
            this.actualCalculator = CreateScorecardActualCalculator(dailyActualRepository);
            this.goalCalculator = CreateScorecardGoalCalculator(targetRepository, dailyActualRepository, scorecardWorkdayPatternRepository, 
                scorecardHolidayPatternRepository, scorecardWorkdayTrackerRepository);
        }
        #endregion

        #region Public Method(s)
        /// <summary>
        /// Method to add new actual entry
        /// </summary>
        /// <param name="actualRequest">Daily actual request</param>
        /// <param name="goalTypeId">Metric goal type id</param>
        /// <param name="dataTypeId">Data type id</param>
        /// <param name="userName">Logged in user name</param>
        /// <returns>Newly created Entity Id</returns>
        public virtual int AddDailyActual(ActualItem actualRequest, int goalTypeId,
            int dataTypeId, string userName)
        {
            // Get logged in user id
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(
               x => x.AccountName == userName).Id;

            var target = targetRepository.Get(actualRequest.TargetId);
            decimal? actualValueToBeCompared = actualRequest.ActualValue;
            decimal? goalValueToBeCompared = actualRequest.GoalValue;
            decimal? goalValueToBeSaved = actualRequest.GoalValue;

            //get cumulative actual & goal value that needs to be compared in case of cumulative plotting
            if (target.GraphPlottingMethodId == Constants.GraphPlottingMethodCumulative)
            {
                actualValueToBeCompared = actualCalculator.CalculateCumulativeActual(
                    actualRequest.TargetId, actualRequest.Date, actualRequest.ActualValue);
                goalValueToBeCompared = goalCalculator.CalculateCumulativeGoal(
                    target, actualRequest.Date);
                goalValueToBeSaved = goalCalculator.GetDailyGoal(actualRequest.TargetId,
                    actualRequest.Date.Month, actualRequest.Date.Day);
            }

            DateTime curTimestamp = TimeZoneUtility.GetCurrentTimestamp();
            // Creating daily actual entity
            var dailyActual = new DailyActual()
            {
                Date = actualRequest.Date,
                GoalValue = goalValueToBeSaved,
                ActualValue = actualRequest.ActualValue,
                Status = (actualValueToBeCompared.HasValue) ? TargetActualComparer.GetActualStatus(
                    goalValueToBeCompared, actualValueToBeCompared, goalTypeId)
                    : ActualStatus.NotEntered,
                TargetId = actualRequest.TargetId,
                CreatedOn = curTimestamp,
                LastModifiedOn = curTimestamp,
                CreatedBy = loggedInUserId,
                LastModifiedBy = loggedInUserId
            };
            dailyActual.DailyActualHistory = new List<DailyActualHistory>() {
                ActualConverters.ConvertDailyActualToDailyActualHistory(dailyActual)
            };

            // Add/Update corresponding monthly actual
            var monthlyActual = AdjustMonthlyActual(actualRequest, goalTypeId,
                dataTypeId, loggedInUserId);

            // If it is a new monthly actual entry, add explicitly
            if (monthlyActual.Id == 0)
            {
                monthlyActualRepository.AddOrUpdate(monthlyActual);
            }

            dailyActualRepository.AddOrUpdate(dailyActual);
            dailyActualRepository.Save();
            return dailyActual.Id;
        }

        /// <summary>
        /// Method to update daily actual and monthly entry
        /// </summary>
        /// <param name="actualRequest">daily actual request</param>
        /// <param name="goalTypeId">metric goal type id</param>
        /// <param name="dataTypeId">metric data type id</param>
        /// <param name="userName">logged in user name</param>
        public virtual void UpdateDailyActual(ActualItem actualRequest, int goalTypeId,
            int dataTypeId, string userName)
        {
            //get logged in user id
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(
               x => x.AccountName == userName).Id;
            //get existing daily actual
            var existingDailyActual = dailyActualRepository.Get(actualRequest.Id.Value);

            var target = targetRepository.Get(actualRequest.TargetId);
            decimal? actualValueToBeCompared = actualRequest.ActualValue;
            decimal? goalValueToBeCompared = actualRequest.GoalValue;
            decimal? goalValueToBeSaved = actualRequest.GoalValue;
         
            //get cumulative actual & goal value that needs to be compared in case of cumulative plotting
            if (target.GraphPlottingMethodId == Constants.GraphPlottingMethodCumulative)
            {
                actualValueToBeCompared = actualCalculator.CalculateCumulativeActual(
                    actualRequest.TargetId, actualRequest.Date, actualRequest.ActualValue);
                goalValueToBeCompared = goalCalculator.CalculateCumulativeGoal(
                    target, actualRequest.Date);
                goalValueToBeSaved = goalCalculator.GetDailyGoal(actualRequest.TargetId,
                    actualRequest.Date.Month, actualRequest.Date.Day);
            }

            //update daily actual with new changes
            existingDailyActual.ActualValue = actualRequest.ActualValue;
            existingDailyActual.GoalValue = goalValueToBeSaved;
            existingDailyActual.Status = (actualValueToBeCompared.HasValue) ?
                TargetActualComparer.GetActualStatus(goalValueToBeCompared,
                actualValueToBeCompared, goalTypeId) : ActualStatus.NotEntered;
            existingDailyActual.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
            existingDailyActual.LastModifiedBy = loggedInUserId;
            //add history of daily actual
            existingDailyActual.DailyActualHistory.Add(
                ActualConverters.ConvertDailyActualToDailyActualHistory(existingDailyActual));

            // Add/Update corresponding monthly actual
            var monthlyActual = AdjustMonthlyActual(actualRequest, goalTypeId,
                dataTypeId, loggedInUserId);

            // If it is a new monthly actual entry, add explicitly
            if (monthlyActual.Id == 0)
            {
                monthlyActualRepository.AddOrUpdate(monthlyActual);
            }

            dailyActualRepository.Save();
        }

        /// <summary>
        /// Method to update status and goal for daily actual and monthly entry
        /// </summary>
        /// <param name="targetId">The target identifier.</param>
        /// <param name="selectedDate">The selected date.</param>
        /// <param name="userName">logged in user name</param>
        public virtual void UpdateDailyActualStatusAndGoalForMonth(int targetId, DateTime selectedDate, string userName)
        {
            //get logged in user id. Default NdmsAdmin
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(
               x => x.AccountName == userName)?.Id ?? 0;
            
            //get existing daily actual
            var existingDailyActuals = dailyActualRepository.GetAll().Where(x=>x.TargetId == targetId 
                                       && x.Date.Month == selectedDate.Month && x.Date.Year == selectedDate.Year)?.ToList();

            if(existingDailyActuals == null || !existingDailyActuals.Any())
            {
                return;
            }

            var target = targetRepository.Get(targetId);

            foreach (var dailyActual in existingDailyActuals)
            {
                decimal? actualValueToBeCompared = dailyActual.ActualValue;
                decimal? goalValueToBeCompared;
                decimal? goalValueToBeSaved;
                dailyActual.GoalValue = goalCalculator.GetDailyGoal(dailyActual.TargetId,
                    dailyActual.Date.Month, dailyActual.Date.Day);
                goalValueToBeCompared = goalValueToBeSaved = dailyActual.GoalValue;
                //get cumulative actual & goal value that needs to be compared in case of cumulative plotting
                if (target.GraphPlottingMethodId == Constants.GraphPlottingMethodCumulative)
                {
                    actualValueToBeCompared = actualCalculator.CalculateCumulativeActual(
                        dailyActual.TargetId, dailyActual.Date, dailyActual.ActualValue);
                    goalValueToBeCompared = goalCalculator.CalculateCumulativeGoal(
                        target, dailyActual.Date);
                }

                //update daily actual with new changes
                dailyActual.GoalValue = goalValueToBeSaved;
                dailyActual.Status = (actualValueToBeCompared.HasValue) ?
                    TargetActualComparer.GetActualStatus(goalValueToBeCompared,
                    actualValueToBeCompared, target.Metric.GoalTypeId) : ActualStatus.NotEntered;
                dailyActual.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
                dailyActual.LastModifiedBy = loggedInUserId;
                //add history of daily actual
                dailyActual.DailyActualHistory.Add(
                    ActualConverters.ConvertDailyActualToDailyActualHistory(dailyActual));
            }


            //Update corresponding monthly actual
            var existingMonthlyActual = monthlyActualRepository.GetAll().FirstOrDefault(x => x.TargetId == targetId && x.Month == selectedDate.Month);

            if (existingMonthlyActual != null)
            {
                existingMonthlyActual.LastModifiedBy = loggedInUserId;
                existingMonthlyActual.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
                decimal? monthlyTarget = goalCalculator.GetMonthlyGoal(targetId,
                   selectedDate.Month);
                existingMonthlyActual.Status = TargetActualComparer.GetActualStatus(monthlyTarget,
                    existingMonthlyActual.ActualValue, target.Metric.GoalTypeId);

                // Add the history as well
                existingMonthlyActual.MonthlyActualHistory.Add(
                    ActualConverters.ConvertMonthlyActualToMonthlyActualHistory(existingMonthlyActual));
            }

            dailyActualRepository.Save();
        }

        /// <summary>
        /// Add a new monthly actual entry
        /// </summary>
        /// <param name="actualRequest">Monthly actual request</param>
        /// <param name="goalTypeId">Metric goal type id</param>
        /// <param name="username">Logged in user name</param>
        /// <returns>Newly created Entity Id</returns>
        public virtual int AddMonthlyActual(ActualItem actualRequest, int goalTypeId,
            string username)
        {
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(
             x => x.AccountName == username)?.Id ?? 0;
            var monthlyActual = CreateMonthlyActual(actualRequest, goalTypeId, loggedInUserId);
            monthlyActualRepository.AddOrUpdate(monthlyActual);
            monthlyActualRepository.Save();
            return monthlyActual.Id;
        }

        /// <summary>
        /// Method to update monthly actual entry
        /// </summary>
        /// <param name="actualRequest">daily actual request</param>
        /// <param name="goalTypeId">metric goal type id</param>
        /// <param name="userName">logged in user name</param>
        public virtual void UpdateMonthlyActual(ActualItem actualRequest, int goalTypeId, string userName)
        {
            //get logged in user id
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(
               x => x.AccountName == userName)?.Id ?? 0;
            var monthlyActual = monthlyActualRepository.Get(actualRequest.Id.Value);

            monthlyActual.ActualValue = actualRequest.ActualValue;
            monthlyActual.Status = TargetActualComparer.GetActualStatus(actualRequest.GoalValue,
                    actualRequest.ActualValue, goalTypeId);
            monthlyActual.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
            monthlyActual.LastModifiedBy = loggedInUserId;
            monthlyActual.MonthlyActualHistory.Add(
                   ActualConverters.ConvertMonthlyActualToMonthlyActualHistory(monthlyActual));
            monthlyActualRepository.Save();
        }

        /// <summary>
        /// Update existing monthly actual while adding or updating daily actuals
        /// </summary>
        /// <param name="existingActual">existing monthly actual</param>
        /// <param name="actualUpdRequest">Actual update request</param>
        /// <param name="goalTypeId">goal type id</param>
        /// <param name="dataTypeId">data type id</param>
        /// <param name="loggedInUserId">logged in user id</param>
        /// <returns>Updated monthly actual</returns>
        public virtual MonthlyActual UpdateExistingMonthlyActualOfDailyActual(
            MonthlyActual existingActual, ActualItem actualUpdRequest,
            int goalTypeId, int dataTypeId, int loggedInUserId)
        {
            //set monthly actual value as sum of daily actuals in case 
            //metric data type is amount/whole number
            if (dataTypeId == Constants.DataTypeAmount ||
                dataTypeId == Constants.DataTypeWholeNumber||
                dataTypeId == Constants.DataTypeDecimalNumber)
            {
                if (actualUpdRequest.ActualValue != null)
                {
                    existingActual.ActualValue = FindSumOfDailyActualsExcludingCurrentEntryDate
                        (actualUpdRequest.TargetId, actualUpdRequest.Date) +
                        actualUpdRequest.ActualValue.Value;
                }
            }
            else
            {
                existingActual.ActualValue = actualUpdRequest.ActualValue;
            }

            existingActual.LastModifiedBy = loggedInUserId;
            existingActual.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
            decimal? monthlyTarget = goalCalculator.GetMonthlyGoal(actualUpdRequest.TargetId,
               actualUpdRequest.Date.Month);
            existingActual.Status = TargetActualComparer.GetActualStatus(monthlyTarget,
                existingActual.ActualValue, goalTypeId);

            // Add the history as well
            existingActual.MonthlyActualHistory.Add(
                ActualConverters.ConvertMonthlyActualToMonthlyActualHistory(existingActual));

            return existingActual;
        }

        /// <summary>
        /// Method to update status and goal for monthly entry
        /// </summary>
        /// <param name="targetId">The target identifier.</param>
        /// <param name="selectedDate">The selected date.</param>
        /// <param name="userName">logged in user name</param>
        public virtual void UpdateMonthlyActualStatusAndGoalForMonth(int targetId, DateTime selectedDate, string userName)
        {
            //get logged in user id
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(
               x => x.AccountName == userName)?.Id ?? 0;

            var target = targetRepository.Get(targetId);

            //Update corresponding monthly actual
            var existingMonthlyActual = monthlyActualRepository.GetAll().FirstOrDefault(x => x.TargetId == targetId && x.Month == selectedDate.Month);

            if (existingMonthlyActual != null)
            {
                existingMonthlyActual.LastModifiedBy = loggedInUserId;
                existingMonthlyActual.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
                decimal? monthlyTarget = goalCalculator.GetMonthlyGoal(targetId,
                   selectedDate.Month);
                existingMonthlyActual.Status = TargetActualComparer.GetActualStatus(monthlyTarget,
                    existingMonthlyActual.ActualValue, target.Metric.GoalTypeId);

                // Add the history as well
                existingMonthlyActual.MonthlyActualHistory.Add(
                    ActualConverters.ConvertMonthlyActualToMonthlyActualHistory(existingMonthlyActual));
            }

            monthlyActualRepository.Save();
        }

        /// <summary>
        /// Method to update daily rate
        /// </summary>
        /// <param name="targetId">The target identifier.</param>
        /// <param name="IsMarkWorkday">if set to <c>true</c> [is mark workday].</param>
        /// <param name="date">The date.</param>
        /// <param name="loggedInUserId">The logged in user identifier.</param>
        public virtual void UpdateDailyRateValue(int targetId, bool IsMarkWorkday, DateTime date, int loggedInUserId)
        {
            var currentDate = TimeZoneUtility.GetCurrentTimestamp();
            var monthlyTarget = monthlyTargetRepository.GetAll().FirstOrDefault(x => x.TargetId == targetId && x.Month == date.Month);
            if (monthlyTarget != null)
            {
                if (IsMarkWorkday)
                { //Mark a workday
                  //Add or update dailyTarget with Daily Rate
                    AddOrUpdateDailyTarget(monthlyTarget, date, loggedInUserId);
                }
                else
                {//Mark a holiday
                    var dailyTarget = dailyTargetRepository.GetAll().FirstOrDefault((x => x.MonthlyTargetId == monthlyTarget.Id && x.Day == date.Day));
                    if (dailyTarget != null)
                    {
                        dailyTarget.MaxGoalValue = null;
                        dailyTarget.LastModifiedBy = loggedInUserId;
                        dailyTarget.LastModifiedOn = currentDate;
                        dailyTargetRepository.AddOrUpdate(dailyTarget);
                    }

                }
                dailyTargetRepository.Save();
            }
        }

        /// <summary>
        /// Method to Add or Update Daily Target
        /// </summary>
        /// <param name="monthlyTarget">The monthly target.</param>
        /// <param name="date">The date.</param>
        /// <param name="loggedInUserId">The logged in user identifier.</param>
        public virtual void AddOrUpdateDailyTarget(MonthlyTarget monthlyTarget, DateTime date, int loggedInUserId)
        {

            var currentDate = TimeZoneUtility.GetCurrentTimestamp();
            var dailyTarget = dailyTargetRepository.GetAll().FirstOrDefault(x => x.MonthlyTargetId == monthlyTarget.Id && x.Day == date.Day);
          //  add update delete

            if (dailyTarget == null)
            {
                var dailyTargetDate = new DailyTarget()
                {
                    MonthlyTargetId = monthlyTarget.Id,
                    Day = date.Day,
                    MaxGoalValue = monthlyTarget.DailyRate,
                    CreatedOn = currentDate,
                    CreatedBy = loggedInUserId,
                    LastModifiedOn = currentDate,
                    LastModifiedBy = loggedInUserId
                };
                dailyTargetRepository.AddOrUpdate(dailyTargetDate);
            }
            else
            {
                dailyTarget.MaxGoalValue = monthlyTarget.DailyRate;
                dailyTarget.LastModifiedBy = loggedInUserId;
                dailyTarget.LastModifiedOn = currentDate;
                dailyTargetRepository.AddOrUpdate(dailyTarget);
            }
           
            dailyTargetRepository.Save();
        }


        #endregion
    }
}
