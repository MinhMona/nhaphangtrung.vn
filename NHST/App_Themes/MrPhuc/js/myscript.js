function sroll() {
    window.addEventListener('scroll', () => {
        const valueScroll = 100;

        const scroll = window.scrollY;

        if (scroll >= valueScroll) {
            $('.header').addClass('fixed');
        }
        if (scroll < valueScroll) {
            $('.header').removeClass('fixed');
        }
    });
}

/*function wow() {
    var wow = new WOW({
        boxClass: 'wow', // animated element css class (default is wow)
        animateClass: 'animated', // animation css class (default is animated)
        offset: 0, // distance to the element when triggering the animation (default is 0)
        mobile: true, // trigger animations on mobile devices (default is true)
        live: true, // act on asynchronously loaded content (default is true)
        scrollContainer: null, // optional scroll container selector, otherwise use window,
        resetAnimation: false, // reset animation on end (default is true)
    });
    wow.init();
}*/

function countNum() {
    document.querySelectorAll('.counter').forEach((el) => {
        number = el.getAttribute('data-counter');
        let numAnim = new countUp.CountUp(el, number);
        numAnim.start();
    });
}


$(function() {
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


        });
    });
});
$(document).ready(function(){
        $('.search-maintext h2').click(function(){
            var tab_id = $(this).attr('data-tab');

            $('.search-maintext h2').removeClass('active');
            $('.form-search').removeClass('active');

            $(this).addClass('active');
            $("."+tab_id).addClass('active');
        });
    });

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
  $(".acc-info-btn").click(function() {
        $(".status-mobile").addClass("open");
        $(".overlay-status-mobile").show();
        return false;
    });

    $(".overlay-status-mobile").click(function() {
        $(".status-mobile").removeClass("open");
        $(this).hide();
    });