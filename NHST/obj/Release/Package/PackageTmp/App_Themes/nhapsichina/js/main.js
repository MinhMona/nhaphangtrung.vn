$(function() {
    // add class
    $(window).scroll(function() {
        var scroll = $(window).scrollTop();
        if (scroll >= 50) {
            $(".header").addClass("header-fixed");
        } else {
            $(".header").removeClass("header-fixed");
        }
    });

    $('.main-menu-btn').on('click', function() {
        $(this).addClass('active');
        $('.main-menu').addClass('active');
    });

    $('.main-menu-overlay').on('click', function() {
        $('.main-menu-btn').removeClass('active');
        $('.main-menu').removeClass('active');
    });

    $('.main-menu-btn').on('click', function() {
        $(this).addClass('active');
        $('.main-menu').addClass('active');
    });

    $('.main-menu-overlay').on('click', function() {
        $('.main-menu-btn').removeClass('active');
        $('.main-menu').removeClass('active');
    });

    $(".acc-info-btn").click(function() {
        $(".status-mobile").addClass("open");
        $(".overlay-status-mobile").show();
        return false;
    });

    $(".overlay-status-mobile").click(function() {
        $(".status-mobile").removeClass("open");
        $(this).hide();
    });
     $(document).ready(function() {
        $('.btn-dropdown').click(function() {
            $('.sub-menu').toggleClass('visible');
        });
    });

    /*if ($('.header').length > 0 && $('.main').length > 0) {
        header = $('.header');
        main = $('.main');
        main.css('margin-top', header.outerHeight());
        if ($(window).scrollTop() > 10) {
            header.addClass('fixed');
        } else {
            header.removeClass('fixed');
        }
        $(window).scroll(function() {
            if ($(this).scrollTop() > 10) {
                header.addClass('fixed');
            } else {
                header.removeClass('fixed');
            }
        })
    };*/

})


var toggleHeight = $(window).outerHeight();
$(window).scroll(function() {
    if ($(window).scrollTop() > toggleHeight) {
        //Adds active class to make button visible
        $(".m-backtotop").addClass("active");

        //Just some cool text changes
        $('h1 span').text('TA-DA! Now hover it and hit dat!')

    } else {
        //Removes active class to make button visible
        $(".m-backtotop").removeClass("active");

        //Just some cool text changes
        $('h1 span').text('(start scrolling)')
    }
});


//Scrolls the user to the top of the page again
$(".m-backtotop").click(function() {
    $("html, body").animate({ scrollTop: 0 }, "slow");
    return false;
});


$('.main-wrapper').slick({
    dots: true,
    arrows: false,
    infinite: true,
    speed: 700,
    autoplaySpeed: 5000,
    slidesToShow: 1,
    slidesToScroll: 1,
    customPaging: function(slider, i) {
        var thumb = $(slider.$slides[i]).data();
        return '<a class="dot"><span>0</span>' + i + '</a>';
    },
    responsive: [{
            breakpoint: 1024,
            settings: {
                slidesToShow: 1,
                slidesToScroll: 1,
                infinite: true,
                dots: true
            }
        },
        {
            breakpoint: 768,
            settings: {
                slidesToShow: 1,
                slidesToScroll: 1
            }
        },
        {
            breakpoint: 499,
            settings: {
                slidesToShow: 1,
                slidesToScroll: 1
            }
        }
    ]
});
$('.slide-consultation').slick({
    dots: false,
    arrows: true,
    infinite: true,
    speed: 700,
    autoplaySpeed: 5000,
    slidesToShow: 3,
    slidesToScroll: 1,
    responsive: [{
            breakpoint: 1024,
            settings: {
                slidesToShow: 1,
                slidesToScroll: 1,
                infinite: true,
                dots: true
            }
        },
        {
            breakpoint: 768,
            settings: {
                slidesToShow: 2,
                slidesToScroll: 1
            }
        },
        {
            breakpoint: 499,
            settings: {
                slidesToShow: 1,
                slidesToScroll: 1
            }
        }
    ]
});
$('#js-slide-works').slick({
    dots: false,
    arrows: true,
    infinite: true,
    speed: 700,
    autoplaySpeed: 5000,
    slidesToShow: 3,
    slidesToScroll: 1,
    responsive: [{
            breakpoint: 1024,
            settings: {
                slidesToShow: 1,
                slidesToScroll: 1,
                infinite: true,
                dots: true
            }
        },
        {
            breakpoint: 768,
            settings: {
                slidesToShow: 2,
                slidesToScroll: 1
            }
        },
        {
            breakpoint: 499,
            settings: {
                slidesToShow: 1,
                slidesToScroll: 1
            }
        }
    ]
});

$('.counting').each(function() {
  var $this = $(this),
      countTo = $this.attr('data-count');
  
  $({ countNum: $this.text()}).animate({
    countNum: countTo
  },

  {

    duration: 8000,
    easing:'linear',
    step: function() {
      $this.text(Math.floor(this.countNum));
    },
    complete: function() {
      $this.text(this.countNum);
      //alert('finished');
    }

  });  
  

});
/*$(function () {
    var fx = function fx() {
    $(".count").each(function (i, el) {
        var data = parseInt(this.dataset.n, 10);
        var props = {
            "from": {
                "count": 0
            },
                "to": { 
                "count": data
            }
        };
        $(props.from).animate(props.to, {
            duration: 3000 * 1,
            step: function (now, fx) {
                $(el).text(Math.ceil(now));
            },
            complete:function() {
                if (el.dataset.sym !== undefined) {
                  el.textContent = el.textContent.concat(el.dataset.sym)
                }
            }
        });
    });
    };
    
    var reset = function reset() {
        //console.log($(this).scrollTop())
        if ($(this).scrollTop() > 500) {
            $(this).off("scroll");
          fx()
        }
    };
    
    $(window).on("scroll", reset);
});*/
$('.count').each(function() {
    $(this).prop('Counter', 0).animate({
        Counter: $(this).text()
    }, {
        duration: 4000,
        easing: 'swing',
        step: function(now) {
            $(this).text(Math.ceil(now));
        }
    });
});
$(".tab-wrapper").each(function() {
    let tab = $(this);
    tab.find(".tab-link").first().addClass("current");
    let tabID = tab.find(".tab-link.current").attr("data-tab");
    tab.find(tabID).fadeIn().siblings().hide();
    $(tab).on("click", ".tab-link", function(e) {
        e.preventDefault();
        let tabID = $(this).attr("data-tab");
        $(this).addClass("current").siblings().removeClass("current");
        tab.find(tabID).fadeIn().siblings().hide();
    })
});
$(document).ready(function() {
    $('.c-tab__nav ul li').click(function() {
        var tab_id = $(this).attr('data-tab');

        $('.c-tab__nav ul li').removeClass('active');
        $('.c-tab__content').removeClass('active');

        $(this).addClass('active');
        $("." + tab_id).addClass('active');
    });
    $('.c-tab__nav-1 ul li').click(function() {
        var tab_id = $(this).attr('data-tab');

        $('.c-tab__nav-1 ul li').removeClass('active');
        $('.c-tab__content-1').removeClass('active');

        $(this).addClass('active');
        $("." + tab_id).addClass('active');
    });
});