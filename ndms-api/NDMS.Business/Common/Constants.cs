namespace NDMS.Business.Common
{
    public static class Constants
    {
        public const string ScorecardExists = "Scorecard with the given name, division and facility already exists";
        public const string ScorecardNotExists = "Scorecard doesn't exist";
        public const string KPIOwnerMaxCountErrorMessage = "Maximum number of KPI Owners allowed is {0}";
        public const string KPIOwnersEmptyErrorMessage = "Please add KPI owners";
        public const string ScorecardTeamMaxCountErrorMessage = "Maximum number of teams allowed is {0}";
        public const string ScorecardTeamsEmptyErrorMessage = "Please add team";
        public const string KPIMaxCountErrorMessage = "Maximum number of KPI allowed is {0}";
        public const string ScorecardKPIOwnerAlreadyExists = "{0} is already KPI Owner of another scorecard";
        public const string ScorecardTeamAlreadyExists = "{0} is already a team member of another scorecard";
        public const string MetricNameEmpty = "Please enter the Metric name";
        public const string MetricExists = "{0} already exists";
        public const string EmptyOrganizationData = "Organization details cannot be empty";
        public const string EmptyGoalTypeorDataType = "Goal Type or Data Type cannot be empty";
        public const string MetricUpdateTargetSetErrorMessage = "Since targets are set for metric {0}, goal type/data type for this metric cannot be updated";
        public const string MetricNotFound = "Metric with name {0} doesn't exists";
        public const string MetricMappingNotFound = "Unable to update this mapping request as it doesn't exists";
        public const string MetricMappingExists = "Metric mapping with the selected combination already exists";

        public const string TargetStartDatePastErrorMessage = "Effective Start Date cannot be past month date";
        public const string TargetEndDatePastErrorMessage = "Effective End Date cannot be a past date";
        public const string TargetEndDateErrorMessage = "Effective End Date should be greater than Effective Start Date";
        public const string TargetEndDateCalendarYearErrorMessage = "Effective End Date should be within the Calendar Year";
        public const string TargetDeleteErrorMessage = "Since the metric has started capturing actuals, Metric cannot be deleted";
        public const string TargetNotFound = "Target not found to enter actual";
        public const string TargetCascadedDeleteErrorMessage = "Since the metric is cascaded to child,  Metric cannot be deleted";
        public const string TargetStartDateCalendarYearErrorMessage = "Effective Start Date should be within the Calendar Year";
        public const string DailyTargetErrorMessage = "Daily Target value totals must be less than or equal to the Monthly Target";
        public const string TargetStretchGoalEqualToErrorMessage = "Stretch Goal and Monthly Targets should be same for this Metric";
        public const string TargetStretchGoalGreaterThanErrorMessage = "Stretch Goal target should be greater than Monthly Targets for this Metric";
        public const string TargetStretchGoalLessThanErrorMessage = "Stretch Goal target should be less than Monthly Targets for this Metric";
        public const string DailyRateStretchGoalEqualToErrorMessage = "Stretch Goal and Daily Rate should be same for this Metric";
        public const string DailyRateStretchGoalGreaterThanErrorMessage = "Stretch Goal target should be greater than Daily Rate for this Metric";
        public const string DailyRatetStretchGoalLessThanErrorMessage = "Stretch Goal target should be less than Daily Rate for this Metric";
        public const string TargetCannotBeCascadedErrorMessage = "Since the Parent Scorecard does not have this metric set for the KPI, Target cannot be cascaded";
        public const string CascadedTargetEffectiveDatesErrorMessage = "For cascaded metrics, Effective dates should be within parent from the Parent Scorecard target's effective date range";
        public const string CascadedTargetGraphPlottingMethodErrorMessage = "For cascaded metrics, Graph Plotting Method should be derived from the Parent Scorecard";
        public const string CascadedTargetTrackingMethodErrorMessage = "For cascaded metrics, Tracking Method should be derived from the Parent Scorecard";
        public const string CascadedTargetMTDPerformanceTrackingErrorMessage = "For cascaded metrics, MTD Performance Tracking Method should be derived from the Parent Scorecard";
        public const string CascadedTargetStretchGoalErrorMessage = "For cascaded metrics, Stretch Goal option should be derived from the Parent Scorecard";
        public const string CascadedMonthlyTargetErrorMessage = "Monthly Goal values must be less than or equal to Maximum Allowed Goal.";
        public const string CascadedMonthlyTargetNotSetErrorMessage = "Monthly Goal values must be set for parent to allow cascading";
        public const string PrimaryMetricMaxCountErrorMessage = "Only {0} primary metric can be active within the effective date";
        public const string YearEndTargetTotalsErrorMessage = "Monthly Target [Goal Value] totals must add up to Full Year Target";
        public const string YearEndTargetErrorMessage = "Year End Target must match last month target";
        public const string MonthlyTargetInvalidMonthErrorMessage = "Monthly Target can be set only for months falling between the Effective Start Date and End Date";
        public const string MonthlyTargetEmptyErrorMessage = "Monthly Target should be set for all months falling between the Effective Start Date and End Date";
        public const string DailyRateEmptyErrorMessage = "Daily Rate should be set for all months falling between the Effective Start Date and End Date";
        public const string MonthlyTargetUpdatePastMonthErrorMessage = "Monthly Targets can be adjusted only upto previous month in the same year";
        public const string StretchGoalOptionRemovalErrorMessage = "Since the target has started capturing actuals, stretch goal option cannot be removed";
        public const string SecondaryMetricMaxCountErrorMessage = "Only {0} secondary metric can be active within the effective date";
        public const string MetricExistsErrorMessage = "This metric has already been added to this KPI within the effective date";
        public const string FullYearTargetEmpty = "Full Year Target cannot be empty";
        public const string YearEndTargetEmpty = "Year End Target cannot be empty";
        public const string TargetMetricChangeErrorMessage = "Since the target has started capturing actuals, metric cannot be changed";
        public const string AddTargetMetricErrorMessage = "Since a metric has ended within the month, new metric can only be started the following month or beyond that";
        public const string StretchGoalEmptyErrorMessage = "Please enter stretch goal for the current/future months in which goal is set";
        public const string MonthlyTargetNotSetErrorMessage = "Monthly Goal cannot be empty or blank";
        public const string EffectiveStartDateErrorMessage = "Since the target has started capturing actuals, effective start date cannot be changed";
        public const string CumulativeDataTypeErrorMessage = "Graph Plotting Method 'Cumulative' is not applicable for the current metric!";
        public const string SameAsChildRollupValidationErrorMessage = "Parent and child monthly goals should be same when Rollup method is set as Same As Child!";
        public const string SameAsChildRollupValidationErrorMessageForDailyRate = "Parent and child daily rates should be same when Rollup method is set as Same As Child!";
        public const string SameAsChildRollupTargetEntryMethodValidationErrorMessage = "Parent and child target entry method should be same when Rollup method is set as Same As Child!";
        public const string AverageOfChildrenRollupValidationErrorMessage = "Child monthly goal should be less than or equal to Parent monthly goal!";
        public const string DailyTargetStartDateMessage = "Daily Targets cannot be set before Effective Start Date";
        public const string DailyTargetEndDateMessage = "Daily Targets cannot be set after Effective End Date";
        public const string DuplicateDailyActualEntryErrorMessage = "Actual has already been added for the day";
        public const string DuplicateMonthlyActualEntryErrorMessage = "Actual has already been added for the month";
        public const string MetricExistsForKPIErrorMessage = "Metric already exists.";
        public const string MetricNotExistsForKPIErrorMessage = "No metrics found.";
        public const string LastRecordableDateErrorMessage = "Last Recordable Date can not be a future date";
        public const string TargettEntryMethodMethodValidationErrorMessage = "Since the target entry method is daily,actual entry method should also be daily!";
        public const string EditCascadingOptionValidationErrorMessage = "Sorry! This is not possible. Please end the current Target and start a new one";

        public const int GoalTypeEqualTo = 0;
        public const int GoalTypeGreaterThanOrEqualTo = 1;
        public const int GoalTypeLessThanOrEqualTo = 2;
        public const int DataTypeWholeNumber = 0;
        public const int DataTypeDecimalNumber = 1;
        public const int DataTypePercentage = 2;
        public const int DataTypeAmount = 3;
        //
        public const int TrackingMethodDaily = 0;
        public const int TrackingMethodMonthly = 1;
        public const int CounterMeasureStatusConfirmed = 4;
        // Support graph plotting method 
        public const int GraphPlottingMethodActual = 0;
        public const int GraphPlottingMethodCumulative = 1;
        // Supported Target entry method ids
        public const int TargetEntryMethodDaily = 0;
        public const int TargetEntryMethodMonthly = 1;
        public const int TargetEntryMethodWeekly = 2;
        public const int TargetEntryMethodShift = 3;
        // Supported roll up method Id's
        public const int RollupMethodSumOfChildren = 0;
        public const int RollupMethodAverageOfChildren = 1;
        public const int RollupMethodSameAsChild = 3;

        public const string CascadedMetricActualEntryErrorMessage = "Actuals cannot be captured for Cascaded Metric";
        public const string ActualEntryMonthErrorMessage = "Actuals can be entered only for current and previous month";
        public const string ActualEntryFutureDateErrorMessage = "Actuals cannot be entered for future dates";
        public const string ActualEntryOnHolidayErrorMessage = "Actuals cannot be entered on non-workdays";
        public const string ActualEntryTargetDatesErrorMessage = "Actuals can be entered only for dates within the target start date and end date";
        public const string ActualEntryTargetMonthsErrorMessage = "Actuals can be entered only for months within the target start date and end date";
        public const string MonthlyActualEntryFutureDateErrorMessage = "Actuals can only be entered effective from target start date";

        public const string HolidayEntryTargetDatesErrorMessage = "Workday/Non-Workday can be marked only for dates within the target start date and end date";
        public const string HolidayEntryMonthErrorMessage = "Workday/Non-Workday can be marked only for previous month, current month and future months";
        public const string HolidayEntryActualExistsErrorMessage = "Non-Workday can not be marked for a day having Actual entry";
        public const string HolidayEntryMetricActualExistsErrorMessage = "Since one of the Metrics has Actual entry for the day, Non-Workday can not be marked";
        public const string OutstandingCounterMeasureErrorMessage = "Already an outstanding counter measure exists for this metric";
        public const string ConfirmedCounterMeasureEditErrorMessage = "Confirmed counter measures cannot be edited";
        public const string CounterMeasureDueDateErrorMessage = "Counter Measure Due Date cannot be a past date";
        public const string HolidayPatternNameAlreadyExists = "Holiday Pattern with the same name already exists";

        public const string ScorecardWorkdayPatternNotExists = "Workday pattern is not set for this scorecard.";

        public const string OrganizationalDataNotFound = "Organizational data with name {0} doesn't exists";
        public const string OrganizationalDataExists = "{0} already exists";
        public const string OrganizationalDataMappingNotFound = "Unable to update this mapping request as it doesn't exists";
        public const string OrganizationalDataMappingExists = "Organizational data mapping with the selected combination already exists";

        public const string EmptyBusinessSegmentData = "Business Segment cannot be empty or blank.";
        public const string EmptyDivisionData = "Division cannot be empty or blank.";
        public const string EmptyFacilityData = "Facility cannot be empty or blank.";
        public const string EmptyProductLineData = "Product Line cannot be empty or blank.";
        public const string EmptyDepartmentData = "Department cannot be empty or blank.";
        public const string EmptyProcessData = "Process cannot be empty or blank.";
    }
}