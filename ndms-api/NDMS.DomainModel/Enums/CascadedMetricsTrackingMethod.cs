using System.ComponentModel;

namespace NDMS.DomainModel.Enums
{
    public enum CascadedMetricsTrackingMethod
    {
        /// <summary>
        /// The rolled up target
        /// </summary>
        [Description("Rolled up Targets")]
        RolledUpTargets,

        /// <summary>
        /// The entered target
        /// </summary>
        [Description("Entered Targets")]
        EnteredTargets
    }
}
