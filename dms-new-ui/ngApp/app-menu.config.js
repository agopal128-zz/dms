var appMenu = function () {

    var menu = [
        {
            key: 'scorecard', value: 'Scorecard', url: 'Scorecard', roles: ['TeamMember', 'KPIOwner'],
            iconClass: 'fa fa-signal'
        },
        {
            key: 'hierarchy', value: 'Hierarchy', url: 'Hierarchy/0', roles: ['Admin', 'KPIOwner','TeamMember'],
            iconClass: 'fa fa-sitemap'
        },
        {
            key: 'admin', value: 'Admin', url: '#', roles: ['Admin'], 
            subitems: [{ key: 'addmetrics', url: 'Admin/AddMetrics', value: 'Add / View Metrics' },
                       { key: 'assignmetrics', url: 'Admin/AssignMetrics', value: 'Assign Metrics' },
                       { key: 'addhoidaypattern', url: 'Admin/HolidaySchedule', value: 'Holiday Schedule'},
                       { key: 'addrootscorecard', url: 'ScorecardAdmin/Add/0', value: 'Add Top Level Scorecard'},
                       { key: 'manageorganization', url: 'Admin/ListMaintenance', value: 'List Maintenance' }],
            iconClass: 'fa fa-th'
        }
    ];

    this.get = function (rolesCollection) {

        var usermenu = [];
        $.each(menu, function () {
            var menuRoles = this.roles,
                menuItem = this;
            if (menuRoles === null) {
                usermenu.push(menuItem);
            }
            else {
                $.each(rolesCollection, function (index) {
                    var r = rolesCollection[index];
                    if ($.inArray(r, menuRoles) !== -1) {
                        usermenu.push(menuItem);
                        return false;
                    }
                });
            }
        });

        return usermenu;
    };
};