using NDMS.Business.Common;
using NDMS.DataAccess.Interfaces;
using NDMS.DomainModel;
using NDMS.DomainModel.DTOs;
using NDMS.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace NDMS.Business.Validators
{
    public class TargetValidator
    {
        #region Field(s)
        /// <summary>
        /// Daily actual repository
        /// </summary>
        private IBaseRepository<DailyActual> dailyActualRepository;

        ///<summary>
        ///Montly actual repository
        ///</summary>
        private IBaseRepository<MonthlyActual> monthlyActualRepository;

        /// <summary>
        /// Year repository
        /// </summary>
        private IBaseRepository<Year> yearRepository;

        /// <summary>
        /// Target Repository
        /// </summary>
        private IBaseRepository<Target> targetRepository;

        /// <summary>
        /// Scorecard repository
        /// </summary>
        private IBaseRepository<Scorecard> scorecardRepository;

        /// <summary>
        /// Metric repository
        /// </summary>
        private IBaseRepository<Metric> metricRepository;

        /// <summary>
        /// Reference to target metric validator
        /// </summary>
        private TargetMetricValidator targetMetricValidator;

        #endregion

        #region Private Methods
        /// <summary>
        /// Method to validate effective start date and end date entered
        /// </summary>
        /// <param name="effectiveStartDate">effective start date</param>
        /// <param name="effectiveEndDate">effective end date</param>
        /// <param name="calendarYearId">selected calendar year</param>
        private void ValidateTargetDates(DateTime effectiveStartDate,
            DateTime effectiveEndDate, int calendarYearId)
        {
            var calendarYear = yearRepository.Get(calendarYearId);

            // check whether target's effective start date falls within the calendar year
            if (!(effectiveStartDate.Date >= calendarYear.StartDate.Date &&
                       effectiveStartDate.Date < calendarYear.EndDate.Date))
            {
                throw new NDMSBusinessException(Constants.TargetStartDateCalendarYearErrorMessage);
            }

            //check whether target's effective end date is greater than start date
            if (effectiveEndDate.Date <= effectiveStartDate.Date)
            {
                throw new NDMSBusinessException(Constants.TargetEndDateErrorMessage);
            }
            // check whether target's effective end date falls within the calendar year
            else if (!(effectiveEndDate.Date > calendarYear.StartDate.Date &&
                       effectiveEndDate.Date <= calendarYear.EndDate.Date))
            {
                throw new NDMSBusinessException(Constants.TargetEndDateCalendarYearErrorMessage);
            }
        }

        /// <summary>
        /// Method to validate whether monthly targets entered falls 
        /// between the selected start date and end date
        /// </summary>
        /// <param name="effectiveStartDate"></param>
        /// <param name="effectiveEndDate"></param>
        /// <param name="monthlyTargets"></param>
        private void ValidateTargetMonths(DateTime effectiveStartDate,
            DateTime effectiveEndDate, List<MonthlyTargetItem> monthlyTargets)
        {
            bool isMonthlyTargetPresent = monthlyTargets.Any(x => x.GoalValue.HasValue || x.DailyRateValue.HasValue);
            if (!isMonthlyTargetPresent)
            {
                throw new NDMSBusinessException(Constants.MonthlyTargetNotSetErrorMessage);
            }

            List<int> actualMonths = CalendarUtility.GetMonthsBetweenDates(effectiveStartDate,
                                       effectiveEndDate).Select(x => x.Id).ToList();
            List<int> goalAddedMonths = monthlyTargets.Where(x => x.GoalValue != null)
                                    .Select(x => x.Month.Id).ToList();
            // check if daily rate is entered if Target entry method is daily
            List<int> dailyRateAddedMonths = dailyRateAddedMonths = monthlyTargets.Where(x => x.DailyRateValue != null)
                                    .Select(x => x.Month.Id).ToList();
            
            if (goalAddedMonths.Except(actualMonths).Any() || dailyRateAddedMonths.Except(actualMonths).Any())
            {
                throw new NDMSBusinessException(Constants.MonthlyTargetInvalidMonthErrorMessage);
            }
            
        }

        /// <summary>
        /// method to validate the update of Monthly Targets for past months
        /// </summary>
        /// <param name="updatedMonthlyTargets">monthly targets that needs to be updated</param>
        /// <param name="existingMonthlyTargets">existing monthly targets</param>
        private void ValidateMonthlyTargetUpdate(List<MonthlyTargetItem> updatedMonthlyTargets,
            ICollection<MonthlyTarget> existingMonthlyTargets)
        {
            DateTime currentDate = TimeZoneUtility.GetCurrentTimestamp().Date;
            //check to restrict the update of Monthly Targets for past months
            foreach (var monthlyTarget in updatedMonthlyTargets)
            {
                //Condition to check whether update is for past months, If the years (current year 
                //and selected month's year) are the same, compare the months, if the years are 
                //not the same, selected month's year must be smaller than current year
                if ((monthlyTarget.Month.Year == currentDate.Year &&
                    monthlyTarget.Month.Id < currentDate.Month - 1) ||
                     monthlyTarget.Month.Year < currentDate.Year)
                {
                    var existingMonthlyTarget = existingMonthlyTargets
                            .Where(x => x.Id == monthlyTarget.Id).FirstOrDefault();
                    if (existingMonthlyTarget != null)
                    {
                        if ((existingMonthlyTarget.MaxGoalValue != monthlyTarget.GoalValue) ||
                            (existingMonthlyTarget.StretchGoalValue != monthlyTarget.StretchGoalValue))
                        {
                            throw new NDMSBusinessException(
                                            Constants.MonthlyTargetUpdatePastMonthErrorMessage);
                        }
                    }
                    else if (monthlyTarget.GoalValue.HasValue)
                    {
                        throw new NDMSBusinessException(
                                            Constants.MonthlyTargetUpdatePastMonthErrorMessage);
                    }
                }
            }
        }

        /// <summary>
        /// Method to validate daily targets entered
        /// </summary>
        /// <param name="monthlyTargets">list of monthly targets</param>
        /// <param name="targetStartDate">target start date</param>
        /// <param name="targetEndDate">target end date</param>
        private void ValidateDailyTargets(List<MonthlyTargetItem> monthlyTargets,
            DateTime targetStartDate, DateTime targetEndDate)
        {
            foreach (var monthlyTarget in monthlyTargets)
            {
                if (monthlyTarget.DailyTargets != null && monthlyTarget.DailyTargets.Count() > 0)
                {
                    //if the month is target start month check if target is entered for days before
                    //target start date
                    if (monthlyTarget.Month.Id == targetStartDate.Month)
                    {
                        if (monthlyTarget.DailyTargets.Where(x => x.GoalValue.HasValue && !x.IsHoliday && !x.IsOutofRange)
                            .Any(x => x.Day < targetStartDate.Day))
                        {
                            throw new NDMSBusinessException(Constants.DailyTargetStartDateMessage);
                        }
                    }

                    //if the month is target end month, check if target is entered for days after
                    //target end date
                    if (monthlyTarget.Month.Id == targetEndDate.Month)
                    {
                        if (monthlyTarget.DailyTargets.Where(x => x.GoalValue.HasValue && !x.IsHoliday && !x.IsOutofRange)
                            .Any(x => x.Day > targetEndDate.Day))
                        {
                            throw new NDMSBusinessException(Constants.DailyTargetEndDateMessage);
                        }
                    }

                    //check whether Daily Target value totals is greater than Monthly Target
                    var sumOfDailyTargets = monthlyTarget.DailyTargets.Sum(item => item.GoalValue);
                    if (sumOfDailyTargets > monthlyTarget.GoalValue)
                    {
                        throw new NDMSBusinessException(Constants.DailyTargetErrorMessage);
                    }
                }
            }
        }

        /// <summary>
        /// Method to validate whether Stretch Goal Targets must be on par or beat the  
        /// monthly target based on the Goal type of the Metric
        /// </summary>
        /// <param name="metricId">Identifier of Metric</param>
        /// <param name="monthlyTargets">list of monthly targets</param>
        private void ValidateStrechGoalTarget(int metricId, List<MonthlyTargetItem> monthlyTargets)
        {
            var metric = metricRepository.Get(metricId);
            DateTime currentDate = TimeZoneUtility.GetCurrentTimestamp().Date;
            //check whether stretch goal is not entered for any current or future months
            bool isStretchGoalNotEntered = monthlyTargets.Any(x => (x.GoalValue.HasValue || x.DailyRateValue.HasValue)
            && !x.StretchGoalValue.HasValue &&
            ((x.Month.Year == currentDate.Year && x.Month.Id >= currentDate.Month) ||
            x.Month.Year > currentDate.Year));

            if (isStretchGoalNotEntered)
            {
                throw new NDMSBusinessException(Constants.StretchGoalEmptyErrorMessage);
            }

            foreach (var monthlyTarget in monthlyTargets)
            {
                if (monthlyTarget.StretchGoalValue != null)
                {
                    if (metric.GoalTypeId == Constants.GoalTypeEqualTo)
                    {
                        if (monthlyTarget.GoalValue != null && monthlyTarget.StretchGoalValue != monthlyTarget.GoalValue)
                        {
                            throw new NDMSBusinessException(Constants.TargetStretchGoalEqualToErrorMessage);
                        }
                        if (monthlyTarget.DailyRateValue != null && monthlyTarget.StretchGoalValue != monthlyTarget.DailyRateValue)
                        {
                            throw new NDMSBusinessException(Constants.DailyRateStretchGoalEqualToErrorMessage);
                        }
                    }
                    else if (metric.GoalTypeId == Constants.GoalTypeGreaterThanOrEqualTo)
                    {
                        if (monthlyTarget.GoalValue != null && (monthlyTarget.StretchGoalValue < monthlyTarget.GoalValue))
                        {
                            throw new NDMSBusinessException(Constants.TargetStretchGoalGreaterThanErrorMessage);
                        }
                        if (monthlyTarget.DailyRateValue != null && (monthlyTarget.StretchGoalValue < monthlyTarget.DailyRateValue))
                        {
                            throw new NDMSBusinessException(Constants.DailyRateStretchGoalGreaterThanErrorMessage);
                        }
                    }
                    else if (metric.GoalTypeId == Constants.GoalTypeLessThanOrEqualTo)
                    {
                        if (monthlyTarget.GoalValue != null && (monthlyTarget.StretchGoalValue > monthlyTarget.GoalValue))
                        {
                            throw new NDMSBusinessException(Constants.TargetStretchGoalLessThanErrorMessage);
                        }
                        if (monthlyTarget.DailyRateValue != null && (monthlyTarget.StretchGoalValue > monthlyTarget.DailyRateValue))
                        {
                            throw new NDMSBusinessException(Constants.DailyRatetStretchGoalLessThanErrorMessage);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check if target has started capturing daily actuals and return the flag
        /// </summary>
        /// <param name="targetId">target Id</param>
        /// <returns>flag which says whether target has started entering actuals</returns>
        private bool CheckIfTargetHasStartedCapturingDailyActuals(int targetId)
        {
            return dailyActualRepository.GetAll().Any(x => x.TargetId == targetId);
        }

        /// <summary>
        /// Check if target has started capturing daily actuals and return the flag
        /// </summary>
        /// <param name="targetId">target Id</param>
        /// <returns>flag which says whether target has started entering actuals</returns>
        private bool CheckIfTargetHasStartedCapturingActuals(int targetId)
        {
            var hasDailyActualCaptured = dailyActualRepository.GetAll().Any(x => x.TargetId == targetId);
            var hasMonthlyActualCaptured = monthlyActualRepository.GetAll().Any(x => x.TargetId == targetId);
            return hasDailyActualCaptured || hasMonthlyActualCaptured;
        }

        #endregion

        #region Protected Method(s)
        /// <summary>
        /// Creates an instance of Target Metric Validator and returns
        /// </summary>
        /// <returns></returns>
        protected virtual TargetMetricValidator CreateTargetMetricValidator(
            IBaseRepository<Target> targetRepository,
            IBaseRepository<Scorecard> scorecardRepository,
            IBaseRepository<Metric> metricRepository)
        {
            if (targetMetricValidator == null)
            {
                targetMetricValidator = new TargetMetricValidator(targetRepository,
                        scorecardRepository, metricRepository);
            }
            return targetMetricValidator;
        }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="targetRepository">Target Repository</param>
        /// <param name="dailyActualRepository">Daily Actual Repository</param>
        /// <param name="yearRepository">Year Repository</param>
        /// <param name="scorecardRepository">Scorecard Repository</param>
        /// <param name="metricRepository">Metric Repository</param>
        public TargetValidator(IBaseRepository<Target> targetRepository,
            IBaseRepository<DailyActual> dailyActualRepository,
            IBaseRepository<MonthlyActual> monthlyActualRepository,
            IBaseRepository<Year> yearRepository,
            IBaseRepository<Scorecard> scorecardRepository,
            IBaseRepository<Metric> metricRepository)
        {
            if (targetRepository == null || dailyActualRepository == null ||
                monthlyActualRepository == null || yearRepository == null ||
                scorecardRepository == null || metricRepository == null)
            {
                throw new ArgumentNullException("Repository",
                    "The given parameter cannot be null.");
            }
            this.targetRepository = targetRepository;
            this.dailyActualRepository = dailyActualRepository;
            this.monthlyActualRepository = monthlyActualRepository;
            this.yearRepository = yearRepository;
            this.scorecardRepository = scorecardRepository;
            this.metricRepository = metricRepository;
            this.targetMetricValidator = CreateTargetMetricValidator(targetRepository,
                scorecardRepository, metricRepository);
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Method to validate whether the request to add a primary target is valid or not
        /// </summary>
        /// <param name="target">primary target add request</param>
        public virtual void ValidateMetricAddTargetRequest(TargetItem target)
        {

            bool isDailyTargetsApplicable = target.TargetEntryMethodId == Constants.TargetEntryMethodDaily;

            //check whether metric is already exists for the kpi and scorecard
            targetMetricValidator.ValidateMetricForScorecardKPI(target);

            if (target.MetricType == MetricType.Primary)
            {
                // method to validate whether a primary metric already exists in the given time period
                targetMetricValidator.ValidatePrimaryMetricCount(target);
            }
            else if (target.MetricType == MetricType.Secondary)
            {
                // Method to validate whether only maximum allowed number of secondary metrics exists 
                // in the given time period
                targetMetricValidator.ValidateSecondaryMetricCount(target);
            }
            //If the target entry method is daily,then the actual entry method should also be daily.
            if(target.TargetEntryMethodId == Constants.TargetEntryMethodDaily)
            {
                if (target.TrackingMethodId != Constants.TrackingMethodDaily)
                {
                    throw new NDMSBusinessException(Constants.TargettEntryMethodMethodValidationErrorMessage);
                }
            }

            if (target.CascadeFromParent)
            {
                targetMetricValidator.ValidateCascadedMetric(target);
            }
            else
            {
                var currentDate = TimeZoneUtility.GetCurrentTimestamp().Date;
                var currentMonthStartDate = new DateTime(currentDate.Year, currentDate.Month, 1);

                //check whether effective start date is not a past month date
                if (target.EffectiveStartDate < currentMonthStartDate)
                {
                    throw new NDMSBusinessException(Constants.TargetStartDatePastErrorMessage);
                }
                //check whether effective end date is not a future date
                if (target.EffectiveEndDate < currentDate)
                {
                    throw new NDMSBusinessException(Constants.TargetEndDatePastErrorMessage);
                }

                // validate effective start date and end date entered
                ValidateTargetDates(target.EffectiveStartDate,
                                    target.EffectiveEndDate,
                                    target.CalendarYearId.Value);

            }

            //Check whether monthly targets entered falls 
            //between the selected start date and end date
            ValidateTargetMonths(target.EffectiveStartDate,
                                 target.EffectiveEndDate,
                                 target.MonthlyTargets);


            //check whether Daily Targets entered are valid
            ValidateDailyTargets(target.MonthlyTargets, target.EffectiveStartDate,
                target.EffectiveEndDate);

            //check whether Stretch Goal Targets must be on par or beat the monthly target 
            //based on the Goal type of the Metric
            if (target.IsStretchGoalEnabled == true)
            {
                ValidateStrechGoalTarget(target.MetricId.Value, target.MonthlyTargets);
            }
            //set stretch goal value in monthlyTargets list to null if stretch goal is disabled
            else
            {
                target.MonthlyTargets.ForEach(x => x.StretchGoalValue = null);
            }
        }
        
        /// <summary>
        /// Performs all validations needed to update a primary metric
        /// </summary>
        /// <param name="target">Modified target</param>
        /// <param name="existingTarget">Existing target</param>
        public virtual void ValidateMetricTargetEditRequest(TargetItem target,
                                                              Target existingTarget)
        {
            var currentDate = TimeZoneUtility.GetCurrentTimestamp();
            DateTime currentMonthStartDate = new DateTime(currentDate.Year, currentDate.Month, 1);
            //If the target entry method is daily,then the actual entry method should also be daily.
            if (target.TargetEntryMethodId == Constants.TargetEntryMethodDaily)
            {
                if (target.TrackingMethodId != Constants.TrackingMethodDaily)
                {
                    throw new NDMSBusinessException(Constants.TargettEntryMethodMethodValidationErrorMessage);
                }
            }
            if (!existingTarget.IsActive)
            {
                throw new NDMSBusinessException(Constants.TargetNotFound);
            }
            //check if target has started capturing actuals
            bool actualExists = CheckIfTargetHasStartedCapturingDailyActuals(existingTarget.Id);
         
            bool isDailyTargetsApplicable = target.TrackingMethodId == Constants.TrackingMethodDaily;

            if ((target.MetricId != existingTarget.MetricId) && actualExists)
            {
                throw new NDMSBusinessException(Constants.TargetMetricChangeErrorMessage);
            }

            targetMetricValidator.ValidateMetricForScorecardKPI(target);

            if (target.MetricType == MetricType.Primary)
            {
                //method to validate whether a primary metric already exists in the given time period
                targetMetricValidator.ValidatePrimaryMetricCount(target);
            }
            else if (target.MetricType == MetricType.Secondary)
            {
                // Method to validate whether only maximum allowed number of secondary metrics exists 
                // in the given time period
                targetMetricValidator.ValidateSecondaryMetricCount(target);
            }

            //If the Scorecard has started capturing Actual, 
            //then the Stretch Goal cannot be removed
            if ((target.IsStretchGoalEnabled == false) &&
                            (existingTarget.IsStretchGoalEnabled != target.IsStretchGoalEnabled))
            {
                if (actualExists)
                {
                    throw new NDMSBusinessException(Constants.StretchGoalOptionRemovalErrorMessage);
                }
            }
            if(existingTarget.EffectiveStartDate < currentMonthStartDate)
            {
                if(existingTarget.CascadeFromParent != target.CascadeFromParent)
                {
                    throw new NDMSBusinessException(Constants.EditCascadingOptionValidationErrorMessage);
                }
            }

            if (target.CascadeFromParent)
            {
                targetMetricValidator.ValidateCascadedMetric(target);
            }
            else
            {
                if (target.EffectiveStartDate != existingTarget.EffectiveStartDate)
                {
                    if (actualExists)
                    {
                        throw new NDMSBusinessException(Constants.EffectiveStartDateErrorMessage);
                    }
                    //block when effective start date is a past month date
                    if (target.EffectiveStartDate < currentMonthStartDate)
                    {
                        throw new NDMSBusinessException(Constants.TargetStartDatePastErrorMessage);
                    }
                }

                if (target.EffectiveEndDate != existingTarget.EffectiveEndDate)
                {
                    //block effective end date is not a future date
                    if (target.EffectiveEndDate < currentDate)
                    {
                        throw new NDMSBusinessException(Constants.TargetEndDatePastErrorMessage);
                    }
                }

                ValidateTargetDates(target.EffectiveStartDate,
                                    target.EffectiveEndDate,
                                    target.CalendarYearId.Value);

            }

            var errors = new List<string>();
            var calendarYear = yearRepository.Get(target.CalendarYearId.Value);

            //Check whether monthly targets entered falls 
            //between the selected start date and end date
            ValidateTargetMonths(target.EffectiveStartDate,
                                 target.EffectiveEndDate,
                                 target.MonthlyTargets);

            //check to restrict the update of Monthly Targets for past months
            ValidateMonthlyTargetUpdate(target.MonthlyTargets, existingTarget.MonthlyTargets);

            //check whether Daily Targets entered are valid
            ValidateDailyTargets(target.MonthlyTargets, target.EffectiveStartDate,
                target.EffectiveEndDate);

            //check whether Stretch Goal Targets must be on par or beat the monthly target 
            //based on the Goal type of the Metric
            if (target.IsStretchGoalEnabled.Value)
            {
                ValidateStrechGoalTarget(target.MetricId.Value, target.MonthlyTargets);
            }
            //set stretch goal value in monthlyTargets list to null if stretch goal is disabled
            else
            {
                target.MonthlyTargets.ForEach(x => x.StretchGoalValue = null);
            }
        }

        /// <summary>
        /// Perform all validations before allowing to remove a metric 
        /// </summary>
        /// <param name="target"></param>
        public virtual void ValidateMetricTargetDeleteRequest(Target target)
        {
            if (target == null || !target.IsActive)
            {
                throw new NDMSBusinessException(Constants.TargetNotFound);
            }
            if (CheckIfTargetHasStartedCapturingActuals(target.Id))
            {
                throw new NDMSBusinessException(Constants.TargetDeleteErrorMessage);
            }
            if (target.IsCascaded)
            {
                throw new NDMSBusinessException(Constants.TargetCascadedDeleteErrorMessage);
            }
        }

        /// <summary>
        /// Checks if target can be deleted.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>true or false</returns>
        public virtual bool CheckIfTargetCanbeDeleted(TargetItem target)
        {
            if (CheckIfTargetHasStartedCapturingActuals(target.Id.Value))
                return false;
            else if (target.IsCascaded)
                return false;
            else
                return true;

        }

        /// <summary>
        /// Checks if target has changed the target entry methods and have both monthly and daily targets.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>true or false</returns>
        public virtual bool CheckIfMetricHasMonthlyAndDailyTargets(TargetItem target)
        {
            int monthlyGoalCount = target.MonthlyTargets?.Count(x => x.GoalValue != null) ?? 0;
            int dailyRateCount = target.MonthlyTargets?.Count(x => x.DailyRateValue != null) ?? 0;

            if(monthlyGoalCount > 0 && dailyRateCount > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if metric has monthly and daily target values.
        /// </summary>
        /// <param name="target">The target.</param>
        public virtual bool CheckIfMetricHasMonthlyAndDailyTargetValues(Target target)
        {
            int monthlyGoalCount = target.MonthlyTargets?.Count(x => x.MaxGoalValue != null) ?? 0;
            int dailyRateCount = target.MonthlyTargets?.Count(x => x.DailyRate != null) ?? 0;

            if (monthlyGoalCount > 0 && dailyRateCount > 0)
            {
                return true;
            }

            return false;
        }
        #endregion
    }
}
