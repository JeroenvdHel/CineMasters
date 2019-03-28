"use strict";

// Load plugins
const autoprefixer = require("autoprefixer");
const browsersync = require("browser-sync").create();
const cssnano = require("cssnano");
const del = require("del");
const gulp = require("gulp");
const imagemin = require("gulp-imagemin");
const newer = require("gulp-newer");
const plumber = require("gulp-plumber");
const postcss = require("gulp-postcss");
const rename = require("gulp-rename");
const sass = require("gulp-sass");
const sourcemaps = require('gulp-sourcemaps');
const babel = require('gulp-babel');
const concat = require('gulp-concat');

// BrowserSync
function browserSync(done) {
  browsersync.init({
    proxy: {
      target: 'http://localhost:5000',
    }
  });
  done();
}

// BrowserSync Reload
function browserSyncReload(done) {
  browsersync.reload();
  done();
}

// Clean assets
function clean() {
  return del([
      "./wwwroot/css/",
      "./wwwroot/js/",
      "./wwwroot/img/"
]);
}

// Optimize Images
function images() {
  return gulp
    .src("./dev/img/**/*")
    .pipe(newer("./wwwroot/img/"))
    .pipe(
      imagemin([
        imagemin.gifsicle({ interlaced: true }),
        imagemin.jpegtran({ progressive: true }),
        imagemin.optipng({ optimizationLevel: 5 }),
        imagemin.svgo({
          plugins: [
            {
              removeViewBox: false,
              collapseGroups: true
            }
          ]
        })
      ])
    )
    .pipe(gulp.dest("./wwwroot/img/"));
}

// CSS task
function css() {
  return gulp
    .src("./dev/scss/**/*.scss")
    .pipe(plumber())
    .pipe(sass({ outputStyle: "expanded" }))
    .pipe(gulp.dest("./wwwroot/css/"))
    .pipe(rename({ suffix: ".min" }))
    .pipe(postcss([autoprefixer(), cssnano()]))
    .pipe(sourcemaps.write('.'))
    .pipe(gulp.dest("./wwwroot/css/"))
    .pipe(browsersync.stream());
}

// Transpile, concatenate and minify scripts
function scripts() {
  return (
    gulp
      .src(["./dev/js/**/*"])
      .pipe(plumber())
      .pipe(sourcemaps.init())
      .pipe(babel({
          presets: ['@babel/preset-env']
      }))
      .pipe(concat('all.js'))
      .pipe(sourcemaps.write('.'))
      .pipe(gulp.dest("./wwwroot/js/"))
      .pipe(browsersync.stream())
  );
}



// Watch files
function watchFiles() {
  gulp.watch("./dev/scss/**/*", css);
  gulp.watch("./dev/js/**/*", gulp.series(scripts));
  gulp.watch("./dev/img/**/*", images);
}

// define complex tasks
const js = gulp.series(scripts);
const build = gulp.series(clean, gulp.parallel(css, images, js));
const watch = gulp.parallel(watchFiles, browserSync);

// export tasks
exports.images = images;
exports.css = css;
exports.js = js;
exports.clean = clean;
exports.build = build;
exports.watch = watch;
exports.default = build;