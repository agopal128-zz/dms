using System;

namespace NDMS.DomainModel.DTOs
{
    public class RecordableItem
    {
        /// <value>
        /// The recordable date.
        /// </value>
        public DateTime? RecordableDate { get; set; }

        public bool IsManual { get; set; }
    }
}
