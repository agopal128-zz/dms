require.config({
    waitSeconds: 0,
    urlArgs: 'bust=v1',
    paths: {
        bootstrap: '../Scripts/bootstrap.min',
        jquery: '../Scripts/jquery-1.10.2.min',
        angular: '../Scripts/angular.min',
        angularRoute: '../Scripts/angular-route.min',
        angularAMD: '../Scripts/angularAMD.min',
        uiBootstrap: '../Scripts/angular-ui/ui-bootstrap.min',
        uiBootstrapTpl: '../Scripts/angular-ui/ui-bootstrap-tpls.min',
        loginApp: 'loginApp.min',
        appConfig: '../ngApp/app.config',

        /*register Services - Start*/
        constants: '../Common/constants.min',
        configService: '../ngServices/Common/configService.min',
        notificationService: '../ngServices/Common/notificationService.min',
        validationService: '../ngServices/Common/validationService.min',
        sessionStorageService: '../ngServices/Common/sessionStorageService.min',
        authService: '../ngServices/Common/authService.min',
        scorecardService: '../ngServices/scorecard-service.min',
        serviceInterceptor: '../ngServices/Common/ndmsServiceInterceptor.min',
        filterUrlService: '../ngServices/Common/filterUrlService.min',
        notificationModalService: '../ngServices/Common/notificationModalService.min',
        /*register Services - End*/
    },

    // specifying library dependencies
    shim: {
        'bootstrap': { deps: ['jquery'] },
        'angular': { deps: ['jquery'], exports: 'angular' },
        'angularRoute': { deps: ['angular'] },
        'angularAMD': { deps: ['angular'] },
        'uiBootstrap': { deps: ['angular'] },
        'uiBootstrapTpl': { deps: ['angular'] }
    },

    // define application bootstrap
    deps: ['loginApp']
});