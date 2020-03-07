using System;
using System.Collections.Generic;

namespace NDMS.DomainModel.DTOs
{
    public class CounterMeasureItem
    {
        /// <summary>
        /// Id of Counter Measure
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the metric
        /// </summary>
        public string MetricName { get; set; }

        /// <summary>
        /// Issue
        /// </summary>
        public string Issue { get; set; }

        /// <summary>
        /// Action for the Counter Measure
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Due Date of Counter Measure
        /// </summary>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Id of Counter Measure Status(Foreign key attribute)
        /// </summary>
        public int CounterMeasureStatusId { get; set; }

        /// <summary>
        /// Name of Counter Measure Status(Foreign key attribute)
        /// </summary>
        public string CounterMeasureStatusName { get; set; }

        /// <summary>
        /// Id of Counter Measure Priority(Foreign key attribute)
        /// </summary>
        public int? CounterMeasurePriorityId { get; set; }

        /// <summary>
        /// Id of Counter Measure Priority Name
        /// </summary>
        public string CounterMeasurePriorityName { get; set; }

        /// <summary>
        /// Counter Measure Comment
        /// </summary>
        public IEnumerable<string> Comments { get; set; }

        /// <summary>
        /// Count of Counter Measure Comments
        /// </summary>
        public int CommentsCount { get; set; }

        /// <summary>
        /// Assigned User Account Name
        /// </summary>
        public string AssignedTo { get; set; }
        
        /// <summary>
        /// Assigned User Name
        /// </summary>
        public string AssignedUserName { get; set; }

        /// <summary>
        /// Created Date of the issue
        /// </summary>
        public DateTime OpenedDate { get; set; }

        /// <summary>
        /// Selected Date
        /// </summary>
        public DateTime Date { get; set; }

    }
}
