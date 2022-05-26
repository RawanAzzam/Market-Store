(function ($) {
    "use strict";

    // Spinner
    var spinner = function () {
        setTimeout(function () {
            if ($('#spinner').length > 0) {
                $('#spinner').removeClass('show');
            }
        }, 1);
    };
    spinner();
    
    
    // Initiate the wowjs
    new WOW().init();


    // Fixed Navbar
    $(window).scroll(function () {
        if ($(window).width() < 992) {
            if ($(this).scrollTop() > 45) {
                $('.fixed-top').addClass('bg-white shadow');
            } else {
                $('.fixed-top').removeClass('bg-white shadow');
            }
        } else {
            if ($(this).scrollTop() > 45) {
                $('.fixed-top').addClass('bg-white shadow').css('top', -45);
            } else {
                $('.fixed-top').removeClass('bg-white shadow').css('top', 0);
            }
        }
    });
    
    
    // Back to top button
    $(window).scroll(function () {
        if ($(this).scrollTop() > 300) {
            $('.back-to-top').fadeIn('slow');
        } else {
            $('.back-to-top').fadeOut('slow');
        }
    });
    $('.back-to-top').click(function () {
        $('html, body').animate({scrollTop: 0}, 1500, 'easeInOutExpo');
        return false;
    });


    // Testimonials carousel
    $(".testimonial-carousel").owlCarousel({
        autoplay: true,
        smartSpeed: 1000,
        margin: 25,
        loop: true,
        center: true,
        dots: false,
        nav: true,
        navText : [
            '<i class="bi bi-chevron-left"></i>',
            '<i class="bi bi-chevron-right"></i>'
        ],
        responsive: {
            0:{
                items:1
            },
            768:{
                items:2
            },
            992:{
                items:3
            }
        }
    });

    
})(jQuery);


//////////////////// 1
function myFunction1Over() {
    var element = document.getElementById("1");
    element.classList.remove("fa-w");
}

function myFunction1Out() {
    var element = document.getElementById("1");
    element.classList.add("fa-w");
}

///////////////////////////// 2
function myFunction2Over() {
    var element = document.getElementById("2");
    element.classList.remove("fa-w");

    myFunction1Over();

}

function myFunction2Out() {
    var element = document.getElementById("2");
    element.classList.add("fa-w");
}

///////////////////// 3
function myFunction3Over() {
    var element = document.getElementById("3");
    element.classList.remove("fa-w");

    myFunction1Over();
    myFunction2Over();
}

function myFunction3Out() {
    var element = document.getElementById("3");
    element.classList.add("fa-w");
}

////////////////// 4
function myFunction4Over() {
    var element = document.getElementById("4");
    element.classList.remove("fa-w");

    myFunction1Over();
    myFunction2Over();
    myFunction3Over();
}

function myFunction4Out() {
    var element = document.getElementById("4");
    element.classList.add("fa-w");

    
}

//////////////// 5
function myFunction5Over() {
    var element = document.getElementById("5");
    element.classList.remove("fa-w");

    myFunction1Over();
    myFunction2Over();
    myFunction3Over();
    myFunction4Over();
}

function myFunction5Out() {
    var element = document.getElementById("5");
    element.classList.add("fa-w");
}

document.getElementById('1').addEventListener('mouseover', myFunction1Over);
document.getElementById('1').addEventListener('mouseout',myFunction1Out);

document.getElementById('2').addEventListener('mouseover', myFunction2Over);
document.getElementById('2').addEventListener('mouseout', myFunction2Out);

document.getElementById('3').addEventListener('mouseover', myFunction3Over);
document.getElementById('3').addEventListener('mouseout', myFunction3Out);


document.getElementById('4').addEventListener('mouseover', myFunction4Over);
document.getElementById('4').addEventListener('mouseout', myFunction4Out);


document.getElementById('5').addEventListener('mouseover', myFunction5Over);
document.getElementById('5').addEventListener('mouseout', myFunction5Out);



