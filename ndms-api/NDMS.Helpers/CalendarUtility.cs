using NDMS.DomainModel;
using NDMS.DomainModel.DTOs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NDMS.Helpers
{
    /// <summary>
    /// Utility class to deal with calendar related functionalities
    /// </summary>
    public static class CalendarUtility
    {
        /// <summary>
        /// Returns all the days in a month
        /// </summary>
        /// <param name="year">Year</param>
        /// <param name="month">Month</param>
        /// <returns>Days list an enumerable</returns>
        public static IEnumerable<int> GetAllDaysInMonth(int year, int month)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month));
        }

        /// <summary>
        /// Returns the months between two dates
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>Return the list of months between two dates</returns>
        public static IEnumerable<MonthItem> GetMonthsBetweenDates(DateTime startDate,
            DateTime endDate)
        {
            DateTime iterator;
            DateTime limit;

            if (endDate > startDate)
            {
                iterator = new DateTime(startDate.Year, startDate.Month, 1);
                limit = endDate;
            }
            else
            {
                throw new ArgumentException("The end date should be greater than start date");
            }

            var dateTimeFormat = CultureInfo.InvariantCulture.DateTimeFormat;
            while (iterator <= limit)
            {
                yield return new MonthItem
                {
                    Id = iterator.Month,
                    Name = dateTimeFormat.GetMonthName(iterator.Month),
                    Year = iterator.Year
                };
                iterator = iterator.AddMonths(1);
            }
        }

        /// <summary>
        /// Gets the year of the month in case NDMS year's are overlapping across calendar year
        /// </summary>
        /// <param name="year">Year Entity</param>
        /// <param name="month">Month</param>
        /// <returns>Year corresponding to the month</returns>
        public static int GetYearOfTheMonth(Year year, int month)
        {
            int requestedYear;
            if (year.StartDate.Month <= month)
            {
                requestedYear = year.StartDate.Year;
            }
            else
            {
                requestedYear = year.EndDate.Year;
            }
            return requestedYear;
        }

        /// <summary>
        /// Gets the name of the month.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <returns>Month Name</returns>
        public static string GetMonthName(int year, int month)
        {
            DateTime date = new DateTime(year, month, 1);
            return date.ToString("MMMM");
        }

    }
}
