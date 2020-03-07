using NDMS.DataAccess.Interfaces;
using NDMS.DomainModel;
using NDMS.DomainModel.DTOs;
using NDMS.Helpers;
using System;
using System.Configuration;
using System.Linq;

namespace NDMS.Business.Common
{
    public class ScorecardRecordablesCalculator
    {
        #region Field(s)

        /// <summary>
        /// Target Repository
        /// </summary>
        private IBaseRepository<Target> targetRepository;

        /// <summary>
        /// Recordable repository
        /// </summary>
        private IBaseRepository<Recordable> recordableRepository;

        /// <summary>
        /// User repository
        /// </summary>
        private IBaseRepository<User> userRepository;

        #endregion

        #region CONSTRUCTOR

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="dailyActualRepository">Target Repository</param>
        public ScorecardRecordablesCalculator(IBaseRepository<Target> targetRepository,
            IBaseRepository<Recordable> recordableRepository, IBaseRepository<User> userRepository)
        {
            if (targetRepository == null || recordableRepository == null || userRepository == null)
            {
                throw new ArgumentNullException("Repository",
                    "The given parameter cannot be null.");
            }
            this.targetRepository = targetRepository;
            this.recordableRepository = recordableRepository;
            this.userRepository = userRepository;
        }

        #endregion

        #region PRIVATE METHODS

        /// <summary>
        /// Deletes the recordable details for the given date.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="recordableDate">The recordable date.</param>
        /// <param name="loggedInUserId">The logged in user identifier.</param>
        private void DeleteRecordablesOfDay(Target target, DateTime recordableDate, int loggedInUserId)
        {
            var scorecardRecordable = recordableRepository.GetAll()
                                                 .Where(x => x.ScorecardId == target.ScorecardId &&
                                                 x.RecordableDate == recordableDate && !x.IsManual && x.IsActive).FirstOrDefault();

            if (scorecardRecordable != null)
            {
                scorecardRecordable.IsActive = false;
                scorecardRecordable.LastModifiedBy = loggedInUserId;
                scorecardRecordable.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
                recordableRepository.AddOrUpdate(scorecardRecordable);
            }
        }

        /// <summary>
        /// Deletes the recordable details for the given month.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="recordableDate">The recordable date.</param>
        /// <param name="loggedInUserId">The logged in user identifier.</param>
        private void DeleteRecordablesOfMonth(Target target, DateTime recordableDate, int loggedInUserId)
        {
            var scorecardRecordable = recordableRepository.GetAll()
                                                 .Where(x => x.ScorecardId == target.ScorecardId &&
                                                 x.RecordableDate.Month == recordableDate.Month &&
                                                 x.RecordableDate.Year == recordableDate.Year &&
                                                 !x.IsManual && x.IsActive);

            if (scorecardRecordable != null)
            {
                foreach (var recordable in scorecardRecordable)
                {
                    recordable.IsActive = false;
                    recordable.LastModifiedBy = loggedInUserId;
                    recordable.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
                    recordableRepository.AddOrUpdate(recordable);
                }
            }
        }

        /// <summary>
        /// Adds the or update recordable date.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="recordableDate">The recordable date.</param>
        /// <param name="loggedInUserId">The logged in user identifier.</param>
        private void AddOrUpdateRecordable(Target target, DateTime recordableDate, int loggedInUserId)
        {
            var scorecardRecordable = recordableRepository.GetAll()
                                                 .Where(x => x.ScorecardId == target.ScorecardId &&
                                                 x.RecordableDate == recordableDate && !x.IsManual).FirstOrDefault();

            if (scorecardRecordable == null)
            {
                scorecardRecordable = new Recordable
                {
                    ScorecardId = target.ScorecardId,
                    RecordableDate = recordableDate,
                    IsManual = false,
                    IsActive = true,
                    CreatedBy = loggedInUserId,
                    LastModifiedBy = loggedInUserId,
                    CreatedOn = TimeZoneUtility.GetCurrentTimestamp(),
                    LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp()
                };
            }
            else
            {
                // update recordable date
                scorecardRecordable.IsManual = false;
                scorecardRecordable.IsActive = true;
                scorecardRecordable.RecordableDate = recordableDate;
                scorecardRecordable.LastModifiedBy = loggedInUserId;
                scorecardRecordable.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
            }

            recordableRepository.AddOrUpdate(scorecardRecordable);
        }

        #endregion

        #region PUBLIC METHODS

        public double? GetNumberOfDaysWithOutRecordables(Scorecard scorecard)
        {
            bool isNumberOfDaysWithOutRecordablesEnabled = Convert.ToBoolean(
                ConfigurationManager.AppSettings[AppSettingsKeys.
                        EnableNumberOfDaysWithoutRecordables]);

            if (isNumberOfDaysWithOutRecordablesEnabled && scorecard.Recordables != null && scorecard.Recordables.Any(x => x.IsActive))
            {
                Recordable recordable = scorecard.Recordables.Where(x => x.IsActive)
                                            .OrderByDescending(x => x.RecordableDate).First();

                DateTime currentDate = TimeZoneUtility.GetCurrentTimestamp().Date;
                double numberofDaysWithoutRecordables = (currentDate - recordable.RecordableDate).TotalDays;
                return numberofDaysWithoutRecordables;
            }

            return null;
        }

        /// <summary>
        /// Tracks the recordable date.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="actualRequestDate">The actual request date.</param>
        public void TrackRecordables(int targetId, ActualItem actualRequest, string userName)
        {
            Target target = targetRepository.Get(targetId);

            DateTime recordableDate = actualRequest.Date;
            DateTime currentDate = TimeZoneUtility.GetCurrentTimestamp().Date;
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(
               x => x.AccountName == userName).Id;

            // if tracking method is monthly and actual entry month and current month is same
            // set recordable date as current date
            if (target.TrackingMethodId == Constants.TrackingMethodMonthly &&
                actualRequest.Date.Month == currentDate.Month)
            {
                recordableDate = currentDate;
            }

            if (actualRequest.ActualValue > 0)
            {
                AddOrUpdateRecordable(target, recordableDate, loggedInUserId);
            }
            else
            {
                if (target.TrackingMethodId == Constants.TrackingMethodMonthly)
                {
                    DeleteRecordablesOfMonth(target, recordableDate, loggedInUserId);
                }
                else
                {
                    DeleteRecordablesOfDay(target, recordableDate, loggedInUserId);
                }
            }

            recordableRepository.Save();

        }

        #endregion
    }
}
