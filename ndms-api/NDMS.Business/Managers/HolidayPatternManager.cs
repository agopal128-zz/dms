using System;
using System.Linq;
using System.Collections.Generic;
using NDMS.Business.Interfaces;
using NDMS.DomainModel.DTOs;
using NDMS.Helpers;
using NDMS.DomainModel;
using NDMS.DataAccess.Interfaces;
using NDMS.DomainModel.Common;
using NDMS.Business.Common;

namespace NDMS.Business.Managers
{
    /// <summary>
    /// Implements Holiday Pattern Manager
    /// </summary>
    public class HolidayPatternManager : IHolidayPatternManager
    {
        #region Field(s)
        /// <summary>
        /// Holiday Pattern Repository
        /// </summary>
        private IBaseRepository<HolidayPattern> holidayPatternRepository;

        ///<summary>
        ///User Repository
        /// </summary>
        private IBaseRepository<User> userRepository;
        #endregion

        #region Private Method(s)
        /// <summary>
        /// Converts Holiday pattern entity to holiday pattern DTO
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        private HolidayPatternItem ConvertHolidayPatternToHolidayPatternItemDTO(HolidayPattern pattern)
        {
            var holidayPatternItem = new HolidayPatternItem
            {
                Id = pattern.Id,
                Name = pattern.Name,
                IsActive = pattern.IsActive
            };
            return holidayPatternItem;
        }

        /// <summary>
        /// Validates the list of updated holidays
        /// </summary>
        /// <param name="holidays"></param>
        private void ValidateHolidaysListUpdateRequest(HolidayPatternInfoRequest request)
        {
            var existingHolidays = holidayPatternRepository.Get(request.HolidayPatternId)
                                                    .Holidays.Where(x => x.IsActive).Select(y => y.Date).ToList();

            if (request.Holidays.Where(x =>
                        x.Date <= TimeZoneUtility.GetCurrentTimestamp().Date).Any(y =>
                        !existingHolidays.Contains(y)))
            {
                throw new NDMSBusinessException(ValidationMessages.PastDateError);
            }
        }
        /// <summary>
        /// Validate Holiday Pattern create request
        /// </summary>
        private void ValidateHolidayPatternAddOrUpdateRequest(HolidayPatternItem holidayPatternRequest)
        {
            if (holidayPatternRequest.Id.HasValue)
            {
                // Update validation
                var existingPattern = holidayPatternRepository.Get(holidayPatternRequest.Id.Value);
                if (holidayPatternRequest.Name != existingPattern.Name)
                {
                    if (holidayPatternRepository.GetAll().Any(x => x.Name == holidayPatternRequest.Name))
                    {
                        throw new NDMSBusinessException(Constants.HolidayPatternNameAlreadyExists);
                    };
                }
            }
            else
            {
                if (holidayPatternRepository.GetAll().Any(x => x.Name == holidayPatternRequest.Name))
                {
                    throw new NDMSBusinessException(Constants.HolidayPatternNameAlreadyExists);
                };
            }
            
        }
        /// <summary>
        /// Gets all Holiday patterns available in the system.
        /// </summary>
        /// <returns>HolidayPatternItem List</returns>
        private IEnumerable<HolidayPatternItem> GetAllHolidayPatterns()
        {
            var holidayPatterns = holidayPatternRepository.GetAll().ToList()
                                                .Select(m => ConvertHolidayPatternToHolidayPatternItemDTO(m)).ToList();

            return holidayPatterns;
        }

        /// <summary>
        /// Gets the required Holiday pattern and holidays associated
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private HolidayPatternItem GetHolidayPatternHolidays(int id)
        {
            var holidayPatternItem = holidayPatternRepository.GetAll().Where(x =>
                                        x.Id == id).Select(m => new HolidayPatternItem
                                        {
                                            Id = m.Id,
                                            Name = m.Name,
                                            IsActive = m.IsActive,
                                            Holidays = m.Holidays.Where(n => n.IsActive).Select(y => y.Date).ToList()
                                        }).FirstOrDefault();
            holidayPatternItem.CurrentDate = TimeZoneUtility.GetCurrentTimestamp();
            return holidayPatternItem;
        }

        /// <summary>
        /// Updates existing Holiday Pattern
        /// </summary>
        /// <param name="requestItem"></param>
        /// <param name="userName"></param>
        private void UpdateHolidayPattern(HolidayPatternItem requestItem, string userName)
        {
            var loggedInUserId = userRepository.GetAll().FirstOrDefault(x =>
                                        x.AccountName == userName).Id;
            var existingPattern = holidayPatternRepository.Get(requestItem.Id.Value);
            existingPattern.Name = requestItem.Name;
            existingPattern.IsActive = requestItem.IsActive;
            existingPattern.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
            existingPattern.LastModifiedBy = loggedInUserId;
            holidayPatternRepository.Save();
        }

        /// <summary>
        /// Update Holiday Mappings
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userName"></param>
        private void UpdateHolidayMapping(HolidayPatternInfoRequest request, string userName)
        {
            var loggedInUserId = userRepository.GetAll().FirstOrDefault(x =>
                                        x.AccountName == userName).Id;
            var holidayPattern = holidayPatternRepository.Get(request.HolidayPatternId);

            var removedHolidays = holidayPattern.Holidays.Where(m =>
                                    m.IsActive &&
                                    !request.Holidays.Contains(m.Date));
            foreach (var holiday in removedHolidays)
            {
                holiday.IsActive = false;
                holiday.LastModifiedBy = loggedInUserId;
                holiday.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();

            }
            var reactivatedHolidays = holidayPattern.Holidays.Where(m =>
                                      !m.IsActive &&
                                      request.Holidays.Contains(m.Date));
            foreach (var holiday in reactivatedHolidays)
            {
                holiday.IsActive = true;
                holiday.LastModifiedBy = loggedInUserId;
                holiday.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
            }

            var addedHolidays = request.Holidays.Where(x =>
                                    !holidayPattern.Holidays.Select(m => m.Date).Contains(x));
            foreach (var holiday in addedHolidays)
            {
                holidayPattern.Holidays.Add(new HolidayPatternInfo
                {
                    HolidayPatternId = request.HolidayPatternId,
                    Date = holiday.Date,
                    IsActive = true,
                    CreatedBy = loggedInUserId,
                    CreatedOn = TimeZoneUtility.GetCurrentTimestamp(),
                    LastModifiedBy = loggedInUserId,
                    LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp()
                });
            }

            holidayPatternRepository.AddOrUpdate(holidayPattern);
            holidayPatternRepository.Save();
        }

        /// <summary>
        /// Copies holidays to a new Holiday Pattern
        /// </summary>
        /// <param name="holidayPatternId"></param>
        /// <param name="userName"></param>
        private void CopyHolidayPatternInformation(int holidayPatternId, string userName)
        {
            var loggedInUserId = userRepository.GetAll().FirstOrDefault(x =>
                                        x.AccountName == userName).Id;
            var existingPattern = holidayPatternRepository.Get(holidayPatternId);
            var newPattern = new HolidayPattern
            {
                Name = "Copy Of " + existingPattern.Name,
                IsActive = existingPattern.IsActive,
                CreatedBy = loggedInUserId,
                CreatedOn = TimeZoneUtility.GetCurrentTimestamp(),
                LastModifiedBy = loggedInUserId,
                LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp(),
                Holidays = existingPattern.Holidays.Where(x =>
                                                    x.IsActive).Select(y =>
                                                    new HolidayPatternInfo
                                                    {
                                                        Date = y.Date,
                                                        IsActive = true,
                                                        CreatedBy = loggedInUserId,
                                                        CreatedOn = TimeZoneUtility.GetCurrentTimestamp(),
                                                        LastModifiedBy = loggedInUserId,
                                                        LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp(),
                                                    }).ToList()
            };
            ValidateHolidayPatternCopyRequest(newPattern);
            holidayPatternRepository.AddOrUpdate(newPattern);
            holidayPatternRepository.Save();

        }
        /// <summary>
        /// Validate copy request of patterns 
        /// </summary>
        /// <param name="newPattern"></param>
        private void ValidateHolidayPatternCopyRequest(HolidayPattern newPattern)
        {
            if(holidayPatternRepository.GetAll().Any(x => x.Name == newPattern.Name))
            {
                throw new NDMSBusinessException(Constants.HolidayPatternNameAlreadyExists);
            }
        }

        /// <summary>
        /// Add news Holiday Pattern
        /// </summary>
        /// <param name="requestItem"></param>
        /// <param name="userName"></param>
        private void AddHolidayPattern(HolidayPatternItem requestItem, string userName)
        {
            var loggedInUserId = userRepository.GetAll().FirstOrDefault(x =>
                                        x.AccountName == userName).Id;
            var newPattern = new HolidayPattern
            {
                Name = requestItem.Name,
                IsActive = requestItem.IsActive,
                CreatedBy = loggedInUserId,
                CreatedOn = TimeZoneUtility.GetCurrentTimestamp(),
                LastModifiedBy = loggedInUserId,
                LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp()
            };
            holidayPatternRepository.AddOrUpdate(newPattern);
            holidayPatternRepository.Save();
        }


        #endregion

        #region Constructor(s)
        public HolidayPatternManager(IBaseRepository<HolidayPattern> holidayPatternRepository,
                    IBaseRepository<User> userRepository)
        {
            if (holidayPatternRepository == null || userRepository == null)
            {
                throw new ArgumentNullException("Repository", "The given parameter cannot be null.");
            }
            this.holidayPatternRepository = holidayPatternRepository;
            this.userRepository = userRepository;
        }
        #endregion

        #region Public Method(s)

        /// <summary>
        /// Gets all HolidayPatterns and returns to the action method.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HolidayPatternItem> GetAll()
        {
            return GetAllHolidayPatterns();
        }
        /// <summary>
        /// Gets the selected holiday Pattern the holidays associated with it
        /// </summary>
        /// <param name="id"></param>
        /// <returns>HolidayPatternItem</returns>
        public HolidayPatternItem GetHolidayPattern(int id)
        {
            return GetHolidayPatternHolidays(id);
        }

        /// <summary>
        /// Adds or Updates Holiday Pattern
        /// </summary>
        /// <param name="holidayPatternRequest"></param>
        /// <param name="userName"></param>
        public void AddOrUpdateHolidayPattern(HolidayPatternItem holidayPatternRequest, string userName)
        {
            ValidateHolidayPatternAddOrUpdateRequest(holidayPatternRequest);
            if (holidayPatternRequest.Id.HasValue)
            {
                UpdateHolidayPattern(holidayPatternRequest, userName);
            }
            else
            {
                AddHolidayPattern(holidayPatternRequest, userName);
            }
        }

        /// <summary>
        /// Update Holidays Mapping
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userName"></param>
        public void UpdateHolidayPatternMapping(HolidayPatternInfoRequest request, string userName)
        {
            ValidateHolidaysListUpdateRequest(request);
            UpdateHolidayMapping(request, userName);
        }



        /// <summary>
        /// Copies holidays to a new Holiday Pattern
        /// </summary>
        /// <param name="holidayPatternId"></param>
        /// <param name="userName"></param>
        public void CopyHolidayPatternInfo(int holidayPatternId, string userName)
        {
            CopyHolidayPatternInformation(holidayPatternId, userName);
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

                }
                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
            }
            disposedValue = true;
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
