module.exports = function () {
    var src = './';
    var config = {
        prod: src,
        allJs: [
            src + '**/*.js',
			'!' + src + '{Scripts,Scripts/**}',
            '!' + src + '{node_modules,node_modules/**}',
			'!' + src + '*.js',
			'!' + src + 'ngApp/app.config.js',
			'!' + src + '**/*.min.js'
        ],

        allHtml: [src + '**/*.html',
            '!' + src + '*.html',
            '!' + src + '**/*.min.html',
            '!' + src + '*.min.html',
            '!' + src + 'fonts/**/*.*'],

        allCss: [src + '**/*.css',
            '!' + src + '{node_modules,node_modules/**}',
            '!' + src + '{fonts,fonts/**}',
            '!' + src + '{Content,Content/**}',
            '!' + src + '{Scripts,Scripts/**}',
            '!' + src + '**/*.min.css'],

        allMinfiles: [
            src + '**/*.min.js',
			src + '**/*.min.html',
			src + '**/*.min.css',
			'!' + src + '{Scripts,Scripts/**}',
			'!' + src + '{Content,Content/**}',
			'!' + src + '{fonts,fonts/**}',
            '!' + src + 'Styles/kendo/**/*.min.css'
        ]
    }
    return config;
};
