
var Resource = {
    "en-us": {
        Scorecard: {
            RequiredScorecardName: "Scorecard name cannot be empty or blank.",
            RequiredHierarchyLevel: "Hierarchy level cannot be empty or blank.",
            RequiredKPIOwner: "KPI Owner cannot be empty or blank.",
            RequiredEmail: "Email is required",
            RequiredDrilldownLevel: "Drill-down level cannot be empty or blank.",
            InvalidDrilldownLevel: "Please enter a valid Drill-down level.",
            ValidEmail: "Please enter valid email address.",
            UpdatedSuccessfully: "Scorecard updated successfully.",
            AddedSuccessfully: "Scorecard added successfully.",
            MonthNavigationMaxError: "No Targets exist in Future Time Periods",
            MonthNavigationMinError: "No Targets exist in Past Time Periods",
            PatternNotSetError : "Scorecard Workday Pattern not set for {0} {1}"
        },

        Metrics: {
            RequiredMetricName: "Metric name cannot be empty or blank.",
            RequiredMetricDataType: "DataType cannot be empty or blank.",
            RequiredPMetricGoalType: "Goal Type cannot be empty or blank.",
            UpdatedSuccessfully: "Metric updated successfully.",
            AddedSuccessfully: "Metric added successfully."
        },

        OrganizationData:{
            UpdatedSuccessfully: "updated successfully.",
            AddedSuccessfully: "added successfully."
        },

        Target: {
            RequiredMetricType: "Metric type cannot be empty or blank.",
            RequiredMetricName: "Metric name cannot be empty or blank.",
            RequiredEffectiveStartDate: "Effective start date cannot be empty or blank.",
            RequiredEffectiveEndDate: "Effective end date cannot be empty or blank.",
            RequiredEffectiveStartDateAndEffectiveEndDate: "Effective start date and effective end date cannot be empty or blank.",
            InvalidEffectiveStartDate: "Effective start date should be start of the month.",
            PastEffectiveStartDate: "Effective start date cannot be past date.",
            SetSuccessfully: "set successfully.",
            UpdateSuccessfully: "updated successfully.",
            DeletedSuccessfully: " deleted successfully.",
            CopiedSuccessfully: "Metrics copied successfully.",
            PastEffectiveEndDate: "Effective end date cannot be past date.",
            SubmitConfirmationMsg: "Are you sure you want to set target?",
            DeleteConfirmationMsg: "Deletion of a metric is irreversible. Are you sure you want to delete the metric?",
            checkDailyTargetAgainstMonthlyTarget :"Entered targets do not match the monthly goal. Do you want to revise the monthly goal value to {0}?",
            assignMetrics:"Please assign metrics before setting targets!. There is no metrics assigned for this KPI",
            RequiredMonthlyTarget: "Monthly goal cannot be empty or blank.",
            RequiredDailyRate: "Daily rate cannot be empty or blank.",
            SetMonthlyTarget: "Please set Monthly Goal to use the Daily View",
            ChangeTabConfirmationMsg:"You have unsaved changes, Do you want to continue?",
            InvalidGoal: "Please enter a valid goal.",
            InvalidStretchGoal: "Please enter a valid stretch goal.",
            InvalidAnnualTarget: "Please enter a valid annual target.",
            InvalidSecondPrimaryMetric: "Cannot add another primary metric in this year, Please change the effective end date of existing primary metric or select next year.",
            CopyTargetConfirmationMsg: "Are you sure you want to copy target?",
            RequiredMetricSelection: "Please select at least one Metric.",
            InvalidDateRange: "Effective start date should be less than effective end date.",
            InvalidTargetEntry: "Since the tracking method is Monthly for previous months, target entry method can not be changed to Daily."
        },

        Actuals: {
            RequiredIssue: "Issue cannot be empty.",
            RequiredAction: "Action cannot be empty.",
            RequiredWho: "Who cannot be empty or invalid.",
            RequiredDueDate: "Due date cannot be empty.",
            RequiredComment: "Comment cannot be empty.",
            AddedSuccessfully: "Added successfully.",
            MarkedSuccessfully:"Marked successfully.",
            UpdatedSuccessfully:"Updated successfully.",
            RequiredActual: "Actual cannot be empty.",
            InvalidActual: "Actual is invalid, Please enter a valid number.",
            CannotEnterActualForcCscaded: "Actuals cannot be captured for cascaded metrics.",
            NoDailyorMonthlyGoal: "Please set target first to enter actuals.",
            NotAuthorizedToChange: "User not Authorized to make changes.",
            DailyRateMsg :"Target is set to ",
            DailyRateProceedMsg: ", do you want to proceed?.",
            MarkasWorkdayMsg: "Operation will reset all the Daily Targets for this month. Please confirm."
        },

        User: {
            RequiredFirstName: "FirstName cannot be empty or blank",
            RequiredLastName: "LastName cannot be empty or blank",
            RequiredPassword: "Password cannot be empty or blank",
            RequiredConfirmPassword: "Please enter confirm password",
            RequiredNewPassword: "Please enter new password.",
            PasswordLength: "Password length should be between 6 to 12 characters.",
            MatchPassword: "New password and confirm password does not match.",
            RequiredEmail: "Email is required",
            ValidEmail: "Please enter valid email address",
            UpdatedSuccessfully: "User updated successfully.",
            PasswordUpdated: "Password updated successfully.",
            DeletedSuccessfully: "User deleted successfully.",
            DeleteErrorMsg: "This user cannot be deleted now.",
            SelectRole: "Please Select at least one role",
            LogoutConfirmation: "Please confirm that you want to log out",
            UsernameRequired : "Username cannot be empty or blank",
            PasswordRequired : "Password cannot be empty or blank",
            sessionTimeout : "Session ended due to inactivity"
        },
        Holiday:{
            RequiredHolidayPatternName: "Holiday Schedule name cannot be empty or blank",
            UpdatedSuccessfully: "Holiday Schedule updated successfully",
            AddedSuccessfully: "Holiday Schedule added successfully",
            PastDateError: "Holidays can be marked only for future days",
            ListUpdatedSuccessfully: "Holidays Saved successfully",
            CopiedSuccessfully: "New copy of {0} created successfully",
            NavigateConfirmation: "You have unsaved changes, Do you want to continue?"
        }
    },
    "en-ca": {
    }
};

