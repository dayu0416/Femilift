require.config({
  // 避免緩存 (上線要刪掉)
  'urlArgs': 'bust=' + (new Date()).getTime(),
  "baseUrl": "/assets/js/vendor",
  // 路徑或別名
  "paths": {

    "jquery": "jquery.min",
    "bootstrap": "bootstrap.bundle.min",
    "common": "../common",

    "easing": "jQuery/jquery.easing.min",
    "mousewheel": "jQuery/jquery.mousewheel.min",

    "TweenLite": "GSAP/TweenLite.min",
    "TweenMax": "GSAP/TweenMax.min",
    "TimelineLite": "GSAP/TimelineLite.min",
    "TimelineMax": "GSAP/TimelineMax.min",

    "ScrollMagic": "ScrollMagic.min",
    "ScrollMagic.debug": "ScrollMagic/debug.addIndicators.min",
    "ScrollMagic.gsap": "ScrollMagic/animation.gsap.min",

    "aos": "aos.min",
    "swiper": "swiper.min",

    "magnific-popup": "jQuery/jquery.magnific-popup.min",
    "parallax": "jQuery/jquery.parallax.min"
  },
  // 初始化模組
  "map" : {
      "*": {
        "css": "RequireJS/css.min"
      }
  },
  // 依賴
  "shim" : {
    "bootstrap": {
      "deps": [
        "jquery"
      ]
    },
    "common": {
      "deps": [
        "jquery",
        "bootstrap"
      ]
    },
    "ScrollMagic.debug": {
      "deps": [
        "ScrollMagic",
      ]
    },
    "ScrollMagic.gsap": {
      "deps": [
        "ScrollMagic",
        "TweenMax",
        "TimelineMax"
      ]
    },
    "aos": {
      "deps": [
        "css!../../css/aos"
      ]
    },
    "swiper": {
      "deps": [
        "jquery",
      ]
    },
    "slick": {
      "deps": [
        "jquery",
      ]
    },
    "parallax": {
      "deps": [
        "jquery"
      ]
    }
  },
  "deps": [
    "common"
  ]
});
