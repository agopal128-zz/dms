using NDMS.DomainModel;
using NDMS.DomainModel.DTOs;
using NDMS.Helpers;
using System.Linq;

namespace NDMS.Business.Converters
{
    internal static class CounterMeasureConverters
    {
        /// <summary>
        /// Method to convert Counter Measure Entity to Counter Measure Item DTO
        /// </summary>
        /// <param name="counterMeasure"></param>
        /// <returns></returns>
        public static CounterMeasureItem ConvertCounterMeasureToCounterMeasureItemDTO(
            CounterMeasure counterMeasure)
        {
            var counterMeasureItem = new CounterMeasureItem()
            {
                Id = counterMeasure.Id,
                MetricName = counterMeasure.Metric.Name,
                Issue = counterMeasure.Issue,
                Action = counterMeasure.Action,
                OpenedDate = counterMeasure.CreatedOn.Date,
                AssignedTo = counterMeasure.AssignedUser.AccountName,
                AssignedUserName = counterMeasure.AssignedUser.FirstName
                   + " " + counterMeasure.AssignedUser.LastName +
                   "(" + counterMeasure.AssignedUser.AccountName + ")",
                DueDate = counterMeasure.DueDate.Date,
                CounterMeasureStatusId = counterMeasure.CounterMeasureStatusId,
                CounterMeasureStatusName = counterMeasure.CounterMeasureStatus?.Status,
                CounterMeasurePriorityId = counterMeasure.CounterMeasurePriorityId,
                CounterMeasurePriorityName = counterMeasure.CounterMeasurePriority?.Name,
                CommentsCount = counterMeasure.CounterMeasureComments.Count,
                Comments = counterMeasure.CounterMeasureComments.Select(c => c.Comment)
            };

            return counterMeasureItem;
        }

        /// <summary>
        /// Method to convert Counter Measure Comment to Counter Measure Comment Entity
        /// </summary>
        /// <param name="comment">comment</param>
        /// <param name="loggedInUserId">Logged In UserId</param>
        /// <returns></returns>
        public static CounterMeasureComment ConvertCommentToCounterMeasureComment(string comment,
            int loggedInUserId)
        {
            var counterMeasureComment = new CounterMeasureComment()
            {
                Comment = comment,
                CreatedBy = loggedInUserId,
                LastModifiedBy = loggedInUserId,
                CreatedOn = TimeZoneUtility.GetCurrentTimestamp(),
                LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp()
            };

            return counterMeasureComment;
        }
    }
}
