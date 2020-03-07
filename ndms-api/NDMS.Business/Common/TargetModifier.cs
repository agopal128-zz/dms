using NDMS.Business.Converters;
using NDMS.DataAccess.Interfaces;
using NDMS.DomainModel;
using NDMS.DomainModel.DTOs;
using NDMS.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NDMS.Business.Common
{
    public class TargetModifier
    {
        #region Field(s)

        /// <summary>
        /// Daily target repository
        /// </summary>
        private IBaseRepository<DailyTarget> dailyTargetRepository;

        /// <summary>
        /// Monthly target repository
        /// </summary>
        private IBaseRepository<MonthlyTarget> monthlyTargetRepository;

        /// <summary>
        /// User repository
        /// </summary>
        private IBaseRepository<User> userRepository;

        /// <summary>
        /// Metric repository
        /// </summary>
        private IBaseRepository<Metric> metricRepository;

        /// <summary>
        /// Scorecard goal calculator
        /// </summary>
        private ScorecardGoalCalculator goalCalculator;

        /// <summary>
        /// Holiday calculator
        /// </summary>
        private HolidayCalculator holidayCalculator;

        #endregion

        #region Private Method(s)

        /// <summary>
        /// add or update the monthly target.
        /// </summary>
        /// <param name="targetId">The target identifier.</param>
        /// <param name="targetValue">The target value.</param>
        /// <param name="targetEntryDate">The target entry date.</param>
        /// <param name="loggedInUserId">The logged in user identifier.</param>
        /// <returns></returns>
        private MonthlyTarget AdjustMonthlyTarget(int targetId, int month, int loggedInUserId)
        {
            var existingMonthlyTarget = monthlyTargetRepository.GetAll()
                                        .FirstOrDefault(x => x.TargetId == targetId &&
                                        x.Month == month);

            if (existingMonthlyTarget != null)
            {
                return UpdateExistingMonthlyRollupTarget(existingMonthlyTarget, loggedInUserId);
            }
            else
            {
                return CreateMonthlyRollupTarget(targetId, month, loggedInUserId);
            }
        }

        /// <summary>
        /// Creates the monthly target.
        /// </summary>
        /// <param name="targetId">The target identifier.</param>
        /// <param name="targetValue">The target value.</param>
        /// <param name="targetEntryDate">The target entry date.</param>
        /// <param name="loggedInUserId">The logged in user identifier.</param>
        /// <returns></returns>
        private MonthlyTarget CreateMonthlyRollupTarget(int targetId, int month, int loggedInUserId)
        {
            DateTime curTimestamp = TimeZoneUtility.GetCurrentTimestamp();
            var monthlyTarget = new MonthlyTarget
            {
                TargetId = targetId,
                Month = month,
                CreatedBy = loggedInUserId,
                CreatedOn = curTimestamp,
                LastModifiedBy = loggedInUserId,
                LastModifiedOn = curTimestamp
            };

            var monthlyTargetHistory = TargetConverters.ConvertMonthlyTargetToMonthlyTargetHistory(monthlyTarget);
            monthlyTargetHistory.TargetId = targetId;
            monthlyTarget.MonthlyTargetHistory = new List<MonthlyTargetHistory>
            {
                monthlyTargetHistory
            };

            return monthlyTarget;
        }

        /// <summary>
        /// Updates the existing monthly target.
        /// </summary>
        /// <param name="existingMonthlyTarget">The existing monthly target.</param>
        /// <param name="targetValue">The target value.</param>
        /// <param name="targetEntryDate">The target entry date.</param>
        /// <param name="loggedInUserId">The logged in user identifier.</param>
        /// <returns></returns>
        private MonthlyTarget UpdateExistingMonthlyRollupTarget(MonthlyTarget existingMonthlyTarget, int loggedInUserId)
        {
            DateTime curTimestamp = TimeZoneUtility.GetCurrentTimestamp();
            existingMonthlyTarget.LastModifiedBy = loggedInUserId;
            existingMonthlyTarget.LastModifiedOn = curTimestamp;
            var monthlyTargetHistory = TargetConverters.ConvertMonthlyTargetToMonthlyTargetHistory(existingMonthlyTarget);
            monthlyTargetHistory.TargetId = existingMonthlyTarget.TargetId;
            existingMonthlyTarget.MonthlyTargetHistory.Add(monthlyTargetHistory);
            return existingMonthlyTarget;
        }

        /// <summary>
        /// Finds the sum of rolled up target excluding current entry date.
        /// </summary>
        /// <param name="monthlyTargetId">The monthly target identifier.</param>
        /// <param name="targetEntryDate">The target entry date.</param>
        /// <returns></returns>
        private decimal FindSumOfRolledUpTargetExcludingCurrentEntryDate(int monthlyTargetId, DateTime targetEntryDate)
        {
            var dailyTargets = dailyTargetRepository.GetAll().Where(x => x.MonthlyTargetId == monthlyTargetId
                                    && x.Day != targetEntryDate.Day && x.RolledUpGoalValue.HasValue);
            return dailyTargets?.Any() ?? false ? dailyTargets.Sum(x => x.RolledUpGoalValue.Value) : 0;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="dailyTargetRepository">The daily target repository.</param>
        /// <param name="monthlyTargetRepository">The monthly target repository.</param>
        /// <exception cref="System.ArgumentNullException">Repository - The given parameter cannot be null.</exception>
        public TargetModifier(IBaseRepository<DailyTarget> dailyTargetRepository,
                              IBaseRepository<MonthlyTarget> monthlyTargetRepository,
                              IBaseRepository<User> userRepository,
                              IBaseRepository<Metric> metricRepository,
                              ScorecardGoalCalculator goalCalculator,
                              HolidayCalculator holidayCalculator)
        {
            if (dailyTargetRepository == null || monthlyTargetRepository == null || userRepository == null || metricRepository == null)
            {
                throw new ArgumentNullException("Repository",
                    "The given parameter cannot be null.");
            }

            this.dailyTargetRepository = dailyTargetRepository;
            this.monthlyTargetRepository = monthlyTargetRepository;
            this.userRepository = userRepository;
            this.metricRepository = metricRepository;
            this.goalCalculator = goalCalculator;
            this.holidayCalculator = holidayCalculator;
        }

        #endregion

        #region Public Method(s)        

        /// <summary>
        /// Adds the or update daily targets.
        /// </summary>
        /// <param name="targetId">The target identifier.</param>
        /// <param name="dailyTargets">The daily targets.</param>
        /// <param name="userName">Name of the user.</param>
        public void AddOrUpdateDailyTargets(int targetId, IList<RollupTargetItem> dailyTargets, string userName)
        {
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(
               x => x.AccountName == userName)?.Id ?? 0;
            DateTime curTimestamp = TimeZoneUtility.GetCurrentTimestamp();

            // exclude months before previous month from roll up
            var targetMonths = dailyTargets?.Where(x => x.TargetEntryDate.Month >= (curTimestamp.Month - 1))?
                                     .Select(x => x.TargetEntryDate.Month).Distinct();

            if (targetMonths == null || !targetMonths.Any())
            {
                return;
            }


            foreach (int month in targetMonths)
            {
                decimal? monthlyRollupValue = null;
                var monthlyTarget = AdjustMonthlyTarget(targetId, month, loggedInUserId);

                var dailyTargetsInMonth = dailyTargets.Where(x => x.TargetEntryDate.Month == month);

                foreach (RollupTargetItem target in dailyTargetsInMonth)
                {
                    DailyTarget existingDailyTarget = null;

                    if (target.TargetId.HasValue)
                    {
                        existingDailyTarget = dailyTargetRepository.Get(target.TargetId.Value);
                    }
                    else
                    {
                        //extra check for duplicate entry
                        existingDailyTarget = dailyTargetRepository.GetAll().FirstOrDefault(x => x.MonthlyTargetId == monthlyTarget.Id &&
                                              x.Day == target.TargetEntryDate.Day);
                    }

                    if (existingDailyTarget != null)
                    {
                        if ((target.IsHoliday && existingDailyTarget.MaxGoalValue.HasValue) || target.GoalValue.HasValue)
                        {
                            existingDailyTarget.MaxGoalValue = target.GoalValue;
                        }
                        existingDailyTarget.RolledUpGoalValue = target.RollUpValue;
                        existingDailyTarget.LastModifiedBy = loggedInUserId;
                        existingDailyTarget.LastModifiedOn = curTimestamp;
                        existingDailyTarget.DailyTargetHistory.Add(
                                TargetConverters.ConvertDailyTargetToDailyTargetHistory(existingDailyTarget)
                            );
                    }
                    else if (target.RollUpValue.HasValue || target.GoalValue.HasValue)
                    {
                        var dailyTarget = new DailyTarget
                        {
                            MonthlyTarget = monthlyTarget,
                            MonthlyTargetId = monthlyTarget.Id,
                            Day = target.TargetEntryDate.Day,
                            RolledUpGoalValue = target.RollUpValue,
                            MaxGoalValue = target.GoalValue,
                            IsManual = false,
                            CreatedBy = loggedInUserId,
                            CreatedOn = curTimestamp,
                            LastModifiedBy = loggedInUserId,
                            LastModifiedOn = curTimestamp
                        };

                        dailyTarget.DailyTargetHistory = new List<DailyTargetHistory> {
                                                TargetConverters.ConvertDailyTargetToDailyTargetHistory(dailyTarget)};

                        dailyTargetRepository.AddOrUpdate(dailyTarget);
                    }

                    if (monthlyRollupValue.HasValue)
                    {
                        if (target.RollUpValue.HasValue)
                        {
                            monthlyRollupValue += target.RollUpValue.Value;
                        }
                    }
                    else
                    {
                        monthlyRollupValue = target.RollUpValue;
                    }

                }

                monthlyTarget.RolledUpGoalValue = monthlyRollupValue;
            }
            dailyTargetRepository.Save();
        }

        /// <summary>
        /// Adds the or update monthly targets.
        /// </summary>
        /// <param name="targetId">The target identifier.</param>
        /// <param name="monthlyTargets">The monthly targets.</param>
        /// <param name="userName">Name of the user.</param>
        public void AddOrUpdateMonthlyTargets(int targetId, IList<RollupTargetItem> monthlyTargets, string userName)
        {
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(
               x => x.AccountName == userName)?.Id ?? 0;
            DateTime curTimestamp = TimeZoneUtility.GetCurrentTimestamp();

            foreach (RollupTargetItem target in monthlyTargets)
            {
                if (target.TargetId.HasValue)
                {
                    var existingMonthlyTarget = monthlyTargetRepository.Get(target.TargetId.Value);
                    existingMonthlyTarget.RolledUpGoalValue = target.RollUpValue;
                    existingMonthlyTarget.LastModifiedBy = loggedInUserId;
                    existingMonthlyTarget.LastModifiedOn = curTimestamp;
                    var monthlyTargetHistory = TargetConverters.ConvertMonthlyTargetToMonthlyTargetHistory(existingMonthlyTarget);
                    monthlyTargetHistory.TargetId = existingMonthlyTarget.TargetId;
                    existingMonthlyTarget.MonthlyTargetHistory.Add(monthlyTargetHistory);
                }
                else
                {
                    if (target.RollUpValue.HasValue)
                    {
                        var monthlyTarget = CreateMonthlyRollupTarget(targetId, target.TargetEntryDate.Month, loggedInUserId);
                        monthlyTarget.RolledUpGoalValue = target.RollUpValue;
                        monthlyTargetRepository.AddOrUpdate(monthlyTarget);
                    }
                }
            }

            monthlyTargetRepository.Save();
        }

        /// <summary>
        /// Distribute the existing targets.
        /// </summary>
        /// <param name="request">The request to generate daily targets.</param>
        public IEnumerable<DailyTargetItem> DistributeExistingDailyTarget(GenerateDailyTargetsRequest request, int targetYear)
        {
            //gets the day list of the required month
            var dayList = CalendarUtility.GetAllDaysInMonth(targetYear, request.MonthId).ToList();

            var dailyTargetList = new List<DailyTargetItem>();

            dayList.ForEach(day =>
            {
                dailyTargetList.Add(new DailyTargetItem
                {
                    Day = day,
                    IsHoliday = holidayCalculator.CheckIfDateIsaHoliday(request.ScorecardId, day, request.MonthId, targetYear),
                    IsOutofRange = goalCalculator.CheckIfSelectedDateIsOutOfDateRange(request.EffectiveStartDate, request.EffectiveEndDate, new DateTime(targetYear, request.MonthId, day))
                });
            });

            var metric = metricRepository.Get(request.MetricId); // distribute monthly goal evenly

            if (request.MonthlyGoalValue.HasValue)
            {
                dailyTargetList = goalCalculator.DistributeMonthlyGoalValueAmongWorkdaysEvenly(dailyTargetList, metric.DataTypeId, request.MonthlyGoalValue.Value);
            }
            else if (request.DailyRateValue.HasValue)
            {
                dailyTargetList = goalCalculator.DistributeDailyRateAmongWorkDays(dailyTargetList, metric.DataTypeId, request.DailyRateValue.Value);
            }

            return dailyTargetList;
        }

        /// <summary>
        /// Checks if target exists for holidays.
        /// </summary>
        /// <param name="monthlyTarget">The monthly target.</param>
        /// <param name="scorecardId">The scorecard identifier.</param>
        /// <param name="targetYear">The target year.</param>
        /// <returns>true if target exists, otherwise false</returns>
        public bool CheckIfTargetExistsForHolidays(MonthlyTarget monthlyTarget, int scorecardId, int targetYear)
        {
            bool isHolidayTargetExists = false;

            if (monthlyTarget.DailyTargets.Any(x => x.MaxGoalValue.HasValue && holidayCalculator.CheckIfDateIsaHoliday(scorecardId, new DateTime(targetYear, monthlyTarget.Month, x.Day))))
            {
                isHolidayTargetExists = true;
            }

            return isHolidayTargetExists;
        }

        /// <summary>
        /// Function to check if currently updating month required system regenerated dialyTargets
        /// </summary>
        /// <param name="targetItem"></param>
        /// <param name="existingTarget"></param>
        /// <param name="updatingMonth"></param>
        /// <returns></returns>
        public bool CheckIfSelectedMonthNeedsUpdation(TargetItem targetItem, Target existingTarget, MonthlyTargetItem updatingMonth)
        {
            bool updateMonthlyTarget = false;
            var existingMonth = existingTarget.MonthlyTargets?.FirstOrDefault(x => x.Month == updatingMonth.Month.Id);
            if (existingMonth != null)
            {
                updateMonthlyTarget = CheckIfTargetExistsForHolidays(existingMonth, targetItem.ScorecardId, targetItem.EffectiveEndDate.Year);
            }
            updateMonthlyTarget = updateMonthlyTarget || CheckIfEffectiveDateChangeInvolvesSelectedMonthlyTarget(targetItem, existingTarget, updatingMonth);
            return updateMonthlyTarget;
        }

        /// <summary>
        /// Function to check if currently updating monthly target item need to be regenerated due to a change in effective start dat in that month
        /// </summary>
        /// <param name="targetItem"></param>
        /// <param name="existingTarget"></param>
        /// <param name="updatingMonth"></param>
        /// <returns></returns>
        public bool CheckIfEffectiveDateChangeInvolvesSelectedMonthlyTarget(TargetItem targetItem, Target existingTarget, MonthlyTargetItem updatingMonth)
        {

            if (targetItem.EffectiveStartDate == existingTarget.EffectiveStartDate && targetItem.EffectiveEndDate == existingTarget.EffectiveEndDate)
            {
                return false;
            }
            else
            {
                if (targetItem.EffectiveStartDate != existingTarget.EffectiveStartDate)
                {
                    return updatingMonth.Month.Id == targetItem.EffectiveStartDate.Month || updatingMonth.Month.Id == existingTarget.EffectiveStartDate.Month;
                }
                else
                {
                    return updatingMonth.Month.Id == targetItem.EffectiveEndDate.Month || updatingMonth.Month.Id == existingTarget.EffectiveEndDate.Month;
                }

            }
        }

        #endregion

    }
}
