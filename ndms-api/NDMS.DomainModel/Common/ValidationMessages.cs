namespace NDMS.DomainModel.Common
{
    /// <summary>
    /// Defines various validation messages for DTO's
    /// </summary>
    public static class ValidationMessages
    {
        public const string ScorecardNameEmpty = "Please enter the Scorecard name";
        public const string ScorecardNameMaxLength = "Maximum character entered for Scorecard name exceeds 100";
        public const string MetricNameEmpty = "Please enter the Metric name";
        public const string MetricNameMaxLength = "Maximum character entered for Metric name exceeds 85";
        public const string MetricGoalTypeEmpty = "Metric can not be added/updated without a goal type";
        public const string MetricDataTypeEmpty = "Metric can not be added/updated without a data type";

        public const string MetricMappingBusinessSegmentEmpty = "Metric assignment cannot be done without a Business Segment";
        public const string MetricMappingDivisionEmpty = "Metric assignment cannot be done without a Division";
        public const string MetricMappingFacilityEmpty = "Metric assignment cannot be done without a Facility";
        public const string MetricMappingProductLineEmpty = "Metric assignment cannot be done without a ProductLine";
        public const string MetricMappingDepartmentEmpty = "Metric assignment cannot be done without a Department";
        public const string MetricMappingProcessEmpty = "Metric assignment cannot be done without a Process";
        public const string MetricMappingKPIEmpty = "Metric assignment cannot be done without a KPI";
        public const string MetricMappingMetricEmpty = "Metric assignment cannot be done without a Metric";

        public const string TargetScorecardEmpty = "Target cannot be set/managed without a Scorecard";
        public const string TargetKPIEmpty = "Target cannot be set/managed without a KPI";
        public const string TargetMetricEmpty = "Target cannot be set/managed without a Metric";
        public const string TargetStartDateEmpty = "Target cannot be set/managed without Effective Start Date";
        public const string TargetEndDateEmpty = "Target cannot be set/managed without Effective End Date";
        public const string TargetMetriTypeEmpty = "Target cannot be set/managed without a Metric Type";
        public const string TargetYearEmpty = "Target cannot be set/managed without a Year";
        public const string MonthlyTargetEmpty = "Monthly Target cannot be empty";
        public const string TargetGraphPlottingMethodEmpty = "Target cannot be set/managed without a Graph Plotting Method";
        public const string AnnualTargetInvalid = "Please enter a valid annual target";
        public const string MTDPerformanceTrackingMethodEmpty = "Target cannot be set/managed without MTD Performance tracking method";
        public const string TargetEntryMethodEmpty = "Target Entry Method cannot be empty";
        public const string ActualTrackingMethodEmpty = "Tracking Method cannot be empty";


        public const string CounterMeasureScorecardEmpty = "Counter Measure cannot be added without a Scorecard";
        public const string CounterMeasureKPIEmpty = "Counter Measure cannot be added without a KPI";
        public const string CounterMeasureMetricEmpty = "Counter Measure cannot be added without a Metric";
        public const string CounterMeasureIssueEmpty = "Please enter Issue";
        public const string CounterMeasureStatusEmpty = "Counter Measure cannot be added/updated without a Status";
        public const string CounterMeasurePriorityEmpty = "Counter Measure cannot be added/updated without a Priority";
        public const string CounterMeasureActionEmpty = "Please enter Action";
        public const string CounterMeasureDueDateEmpty = "Please select a due date for Counter Measure";
        public const string CounterMeasureAssigneeEmpty = "Please select an Assignee for Counter Measure";
        public const string CounterMeasureIdEmpty = "Please provide the Id of Counter Measure that needs to be updated";
        public const string CounterMeasureIssueMaxLength = "Maximum character entered for issue exceeds 300";
        public const string CounterMeasureActionMaxLength = "Maximum character entered for action exceeds 300";
        public const string CounterMeasureCommentMaxLength = "Maximum character entered for comment exceeds 4000";

        public const string ActualTargetEmpty = "Actual cannot be entered without target";
        public const string ActualValueEmpty = "Please enter Actual Value";
        public const string ActualScorecardEmpty = "Actual cannot be entered without a Scorecard";

        public const string HolidayPatternNameEmpty = "Holiday Pattern name cannot be blank";
        public const string HolidayPatternNameMaxLength = "Character entered for Holiday Pattern name exceeds 85";
        public const string PastDateError = "Holidays can be marked only for future days";

        public const string BusinessSegmentNameEmpty = "Please enter the business segment name";
        public const string FacilityNameEmpty = "Please enter the facility name";
        public const string DivisionNameEmpty = "Please enter the division name";
        public const string ProcessNameEmpty = "Please enter the process name";
        public const string ProductLineNameEmpty = "Please enter the product line name";
        public const string DepartmentNameEmpty = "Please enter the department name";
        public const string BusinessSegmentNameMaxLength = "Maximum character entered for business segment name exceeds 100";
        public const string FacilityMaxLength = "Maximum character entered for facility name exceeds 100";
        public const string DivisionNameMaxLength = "Maximum character entered for division name exceeds 100";
        public const string ProcessNameMaxLength = "Maximum character entered for process name exceeds 100";
        public const string ProductLineNameMaxLength = "Maximum character entered for product line name exceeds 100";
        public const string DepartmentNameMaxLength = "Maximum character entered for department name exceeds 100";
    }
}