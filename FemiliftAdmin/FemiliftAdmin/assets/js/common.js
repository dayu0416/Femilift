$(document).ready(function(e) {

  var $win = $(window), $body = $('body');

  $body
    .on('click', 'a[href="#"]', function(e) {
      e.preventDefault();
    });

  var $navbarSwitch = $('#global-header-wrapper .navbar-switch');
  $navbarSwitch.on('click', function(e) {
    e.preventDefault();
    if ($body.hasClass('navbar-open') == false) {
      $body.addClass('navbar-open');
    } else {
      $body.removeClass('navbar-open');
    }
  });

  var $allSubmenu = $('#global-header-wrapper .nav-item > .submenu');
  $('#global-header-wrapper .nav-item > a.nav-link').on('click', function(e) {
    var $domEle = $(this).closest('.nav-item');
    var $subEle = $domEle.find('.submenu');
    if ($navbarSwitch.is(':visible') == true && $subEle.length > 0) {
      e.preventDefault();
      if ($subEle.is(':visible') == false) {
        $allSubmenu.hide();;
        $subEle.stop().fadeIn(200);
      } else {
        $subEle.stop().fadeOut(200);
      }
    }
  });

  var $navbarSearch = $('#global-header-wrapper .navbar-search');
  $navbarSearch.on('click', function(e) {
    e.preventDefault();
    if ($body.hasClass('search-open') == false) {
      $body.addClass('search-open');
    } else {
      $body.removeClass('search-open');
    }
  });

  $('#btn-multi-layer').on('click', function(e) {
    e.preventDefault();
    $('.navbar-large-frame').toggleClass('open');
  });

  require(['easing'], function() {

    $('.go-to-page').on('click', function(e) {
      e.preventDefault();
      $('html, body').stop().animate({
        scrollTop: 0
      }, 1000, 'easeInOutExpo', function() {

      });
    });

  });

  require(['aos', 'parallax'], function(AOS) {

    AOS.init({
      duration: 800,
      offset: 50
    });

    setTimeout(function() {
      AOS.refresh();
    }, 1000);

  });

  if ($('[data-js-code="main-home"]').length) {

    require(['swiper'], function(Swiper) {

      $('[data-bg-src]').each(function() {
        var $cEle = $(this);
        $cEle.css('background-image', 'url(' + $cEle.data('bg-src') + ')');
      });

      new Swiper('#global-carousel-banner .swiper-container', {
        pagination: {
          el: '.swiper-pagination',
          clickable: true
        },
        loop: true,
        autoplay: {
          delay: 6000,
          stopOnLastSlide: false,
          disableOnInteraction: true
        }
      });

      var $sIconFrame = new Swiper('.suitable-wrapper .swiper-container.icon-frame', {
        autoHeight: true,
        on: {
          slideChangeTransitionEnd: function() {
            $sTextFrame.slideTo(this.activeIndex, 1000, false); // alert(this.activeIndex);//切换结束时，告诉我现在是第几个slide
          }
        },
      });

      var $sTextFrame = new Swiper('.suitable-wrapper .swiper-container.text-frame', {
        on: {
          slideChangeTransitionEnd: function() {
            $sIconFrame.slideTo(this.activeIndex, 1000, false); // alert(this.activeIndex);//切换结束时，告诉我现在是第几个slide
          }
        },
        navigation: {
          nextEl: '.suitable-wrapper .swiper-button-next',
          prevEl: '.suitable-wrapper .swiper-button-prev'
        }
      });

      new Swiper('.tool-grip-wrapper .swiper-container', {
        navigation: {
          nextEl: '.tool-grip-wrapper .swiper-button-next',
          prevEl: '.tool-grip-wrapper .swiper-button-prev'
        },
        slidesPerView: 1,
        breakpoints: {
          448: {
            slidesPerView: 2,
            spaceBetween: 15
          },
          576: {
            slidesPerView: 3,
            spaceBetween: 20
          },
          768: {
            slidesPerView: 4,
            spaceBetween: 25
          }
        }
      });

      new Swiper('.recommend-wrapper .swiper-container', {
        autoHeight: true,
        navigation: {
          nextEl: '.recommend-wrapper .swiper-button-next',
          prevEl: '.recommend-wrapper .swiper-button-prev'
        },
        slidesPerView: 1,
        breakpoints: {
          768: {
            slidesPerView: 2,
            spaceBetween: 90
          }
        }
      });

      new Swiper('.certification-wrapper .swiper-container', {
        watchSlidesProgress: true,
        slidesPerView: 'auto',
        centeredSlides: true,
        loop: true,
        loopedSlides: 5,
        autoplay: true,
        on: {
            progress: function(progress) {
            for (i = 0; i < this.slides.length; i++) {
              var slide = this.slides.eq(i);
              var slideProgress = this.slides[i].progress;
              modify = 1;
              if (Math.abs(slideProgress) > 1) {
                modify = (Math.abs(slideProgress) - 1) * 0.3 + 1;
              }
              translate = slideProgress * modify * 260 + 'px';
              scale = 1 - Math.abs(slideProgress) / 5;
              zIndex = 999 - Math.abs(Math.round(10 * slideProgress));
              slide.transform('translateX(' + translate + ') scale(' + scale + ')');
              slide.css('zIndex', zIndex);
              slide.css('opacity', 1);
              if (Math.abs(slideProgress) > 3) {
                slide.css('opacity', 0);
              }
            }
          },
          setTransition: function(transition) {
            for (var i = 0; i < this.slides.length; i++) {
              var slide = this.slides.eq(i)
              slide.transition(transition);
            }
          }
        }
      });

    });

    require(['magnific-popup'], function() {

      $('.popup-modal').magnificPopup({
        type: 'inline',

        fixedContentPos: true,
        fixedBgPos: false,

        closeBtnInside: true,
        preloader: false,

        midClick: true,
        removalDelay: 300,
        mainClass: 'my-mfp-zoom-in',

        modal: true
      });

      $(document).on('click', '.popup-modal-dismiss', function (e) {
        e.preventDefault();
        $.magnificPopup.close();
      });

    });

    $('.effect-wrapper .small-picture').on('click', function(e) {
      e.preventDefault();
      var data = $(this).data('view-json');

      $('.effect-wrapper')
        .find('.picture-frame')
        .removeClass('embed-responsive embed-responsive-16by9')
        .html('<img src="' + data.picture + '">');

      $('.effect-wrapper')
        .find('.title')
        .html(data.title || '');

      $('.effect-wrapper')
        .find('.brief')
        .html(data.brief || '');

      $(this).addClass('current').siblings().removeClass('current');
    });
  }

  if ($('.highlights-wrapper').length) {

    require(['photoswipe', 'photoswipe-ui-default'], function(PhotoSwipe, PhotoSwipeUI_Default) {

      var $pswp = $('.pswp');

      var $listEle = $('.highlights-wrapper .item-frame a[data-items]');
      $listEle.on('click', function(e) {
        e.preventDefault();

        var options = {};
        options.index = $listEle.index(this);
        options.history = false;
        options.mainClass = 'pswp--minimal--dark';
        options.barsSize = {top:0,bottom:0};
        options.captionEl = false;
        options.shareEl = false;
        options.bgOpacity = 0.85;
        options.tapToClose = true;
        options.closeOnScroll = false;
        options.tapToToggleControls = false;

        var gallery = new PhotoSwipe($pswp[0], PhotoSwipeUI_Default, $(this).data('items'), options);
        gallery.init();
      });
    });
  }

  require([
    'ScrollMagic',
    'ScrollMagic.debug',
    'ScrollMagic.gsap'
  ],
  function(ScrollMagic) {

    var tl = {};
    var controller = new ScrollMagic.Controller({});

    tl = new TimelineMax({paused: true});
    // tl.staggerFrom('.equipment-wrapper .picture-large-frame .equipment-img', 1, {opacity: 0}, .5, 'labelMove-01');
    tl.staggerFrom('.equipment-wrapper .picture-large-frame .block-01', 1, {opacity: 0, right: '40%'}, .5, 'labelMove-02');
    tl.staggerFrom('.equipment-wrapper .picture-large-frame .block-02', 1, {opacity: 0, left: '40%'}, .5, 'labelMove-02');
    tl.staggerFrom('.equipment-wrapper .picture-large-frame .block-03', 1, {opacity: 0, right: '20%'}, .5, 'labelMove-03');
    tl.staggerFrom('.equipment-wrapper .picture-large-frame .block-04', 1, {opacity: 0, left: '20%'}, .5, 'labelMove-03');

    new ScrollMagic
      .Scene({
        triggerHook: 'onLeave',
        duration: '100%'
      })
      .on('end', function(event) {
        $('#global-go-to-page').toggleClass('view', event.scrollDirection == 'FORWARD');
      })
      // .addIndicators()
      .addTo(controller);

    if ($('[data-js-code="main-home"]').length) {

      new ScrollMagic
          .Scene({
            triggerElement: '.page-body-wrapper',
            triggerHook: 'onLeave',
            offset: -180
          })
          .on('start', function(event) {
            $('#global-header-wrapper').toggleClass('scroll-view', event.scrollDirection == 'FORWARD');
          })
          // .addIndicators()
          .addTo(controller);

      new ScrollMagic
          .Scene({
            triggerElement: '.equipment-wrapper .picture-large-frame',
            triggerHook: 'onEnter',
            offset: -100
          })
          .on('enter', function(event) {
            tl.restart();
          })
          // .addIndicators()
          .addTo(controller);

    }

    controller.scrollTo(function(newScrollPos) {
      $('html, body').animate({
        scrollTop: newScrollPos
      });
    });

    $('#global-go-to-page').on('click', function(e) {
      e.preventDefault();
      controller.scrollTo(0);
    });

  });

});