/// <binding BeforeBuild='cleanAndOptimize' AfterBuild='cleanAndOptimize' Clean='cleanMinfiles' />
/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
*/
var gulp = require('gulp');
var config = require('./gulp.config')();
var del = require('del');
var runSequence = require('run-sequence');
var $ = require('gulp-load-plugins')({ lazy: true });

// Orchestration task to combine all activities when we trigger a build
// in Visual Studio
gulp.task('cleanAndOptimize', function () {
    runSequence(
        'cleanMinfiles',
        'minifyJS',
        'minifyHtml',
        'optimizeCss'
    );
});

gulp.task('minifyHtml', function () {
    log('Copying and minifing html ' + config.allHtml);
    return gulp
 		.src(config.allHtml)
 		.pipe($.htmlmin({ collapseWhitespace: true }))
 		.pipe($.rename(function (path) { path.extname = '.min.html' }))
 		.pipe(gulp.dest(config.prod))
});

gulp.task('minifyJS', function () {
    log('Copying and minifing js files ' + config.allJs);
    return gulp
		.src(config.allJs)
		.pipe($.ngAnnotate())
		.pipe($.uglify().on('error', $.util.log))
		.pipe($.rename(function (path) { path.extname = '.min.js' }))
		.pipe(gulp.dest(config.prod));
});

gulp.task('optimizeCss', function () {
    log('css optimization ' + config.allCss);
    return gulp
 		.src(config.allCss)
 		.pipe($.csso())
 		.pipe($.rename(function (path) { path.extname = '.min.css' }))
 		.pipe(gulp.dest(config.prod))
});

//gulp.task('images', function(){
//	log('Copying and compressing images');
//	return gulp
//		.src(config.images)
//		.pipe($.imagemin({optimizationLevel:4}))
//		.pipe(gulp.dest(config.prod + 'Images'))
//});

gulp.task('cleanMinfiles', function (done) {
    log('Cleaning: ' + $.util.colors.blue(config.allMinfiles));
    return del(config.allMinfiles, done);
});

//gulp.task('cleanCache', function () {
//    $.cache.clearAll();
//});

function log(msg) {
    if (typeof (msg) === 'object') {
        for (var item in msg) {
            if (msg.hasOwnProperty(item)) {
                $.util.log($.util.colors.blue(msg[item]));
            }
        }
    } else {
        $.util.log($.util.colors.blue(msg));
    }
}
