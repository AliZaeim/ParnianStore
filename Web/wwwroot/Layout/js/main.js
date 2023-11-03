////function isValidEmail(email) {
////    return /^[a-z0-9]+([-._][a-z0-9]+)*@([a-z0-9]+(-[a-z0-9]+)*\.)+[a-z]{2,4}$/.test(email)
////        && /^(?=.{1,64}@.{4,64}$)(?=.{6,100}$).*/.test(email);

//const { data } = require("jquery");

////}
function isValidEmail(emailText) {
    var pattern = new RegExp(/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i);
    return pattern.test(emailText);
};
// Cookie functions from w3schools
function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) === ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) === 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}
(function ($) {
    "use strict";

    /*[ Load page ]
    ===========================================================*/
    $(".animsition").animsition({
        inClass: 'fade-in',
        outClass: 'fade-out',
        inDuration: 1500,
        outDuration: 800,
        linkElement: '.animsition-link',
        loading: true,
        loadingParentElement: 'html',
        loadingClass: 'animsition-loading-1',
        loadingInner: '<div data-loader="ball-scale"></div>',
        timeout: false,
        timeoutCountdown: 5000,
        onLoadEvent: true,
        browser: ['animation-duration', '-webkit-animation-duration'],
        overlay: false,
        overlayClass: 'animsition-overlay-slide',
        overlayParentElement: 'html',
        transition: function (url) { window.location.href = url; }
    });
    window.addEventListener("cookieAlertAccept", function () {
        alert("cookies accepted")

    });

    $(window).on("load", function () {
        $('.slider').fractionSlider({
            'fullWidth': true,
            'controls': true,
            'pager': true,
            'responsive': true,
            'dimensions': "1000,400",
            'increase': false,
            'pauseOnHover': true
        });
        $.ajax({
            url: "/Generic/GetNotifications",
            data: {},
            type: "GET",
            async: true,
            success: function (result) {
                if (result !== null) {
                    var srcc = '/images/notifications/' + result.image;
                    var linnk = result.link;
                    var pathname = window.location.pathname;

                    if (pathname === "/" || pathname === "/Index") {
                        Swal.fire({
                            title: result.title,
                            //toast: true,
                            text: result.text,
                            width: result.width,
                            padding: result.padding,
                            target: document.getElementById('fractionDiv'),
                            imageUrl: srcc,
                            imageWidth: result.imageWidth,
                            imageHeight: result.imageHeight,
                            imageAlt: result.title,
                            showCloseButton: result.showCloseButton,
                            showConfirmButton: result.showConfirmButton,
                            confirmButtonText: result.confirmButtonText,

                        })
                            .then((data) => {
                                if (data.isConfirmed) {
                                    window.location.href = linnk;
                                }
                            })
                    }


                }


            }
        })

    });


    //toastr.info('Are you the 6 fingered man?')
    /*[ Back to top ]
    ===========================================================*/
    var windowH = $(window).height() / 2;

    $(window).on('scroll', function () {
        if ($(this).scrollTop() > windowH) {
            $("#myBtn").css('display', 'flex');
        } else {
            $("#myBtn").css('display', 'none');
        }
    });

    $('#myBtn').on("click", function () {
        $('html, body').animate({ scrollTop: 0 }, 300);
    });


    /*[ Show header dropdown ]
    ===========================================================*/

    $('.js-show-header-dropdown').on('click', function () {
        $(this).parent().find('.header-dropdown');

    });

    var menu = $('.js-show-header-dropdown');
    var sub_menu_is_showed = -1;

    for (var i = 0; i < menu.length; i++) {
        $(menu[i]).on('click', function () {

            if (jQuery.inArray(this, menu) === sub_menu_is_showed) {
                $(this).parent().find('.header-dropdown').toggleClass('show-header-dropdown');
                sub_menu_is_showed = -1;
            }
            else {
                for (var i = 0; i < menu.length; i++) {
                    $(menu[i]).parent().find('.header-dropdown').removeClass("show-header-dropdown");
                }

                $(this).parent().find('.header-dropdown').toggleClass('show-header-dropdown');
                sub_menu_is_showed = jQuery.inArray(this, menu);
            }
        });
    }

    $(".js-show-header-dropdown").click(function (event) {
        event.stopPropagation();
    });


    $(window).on("click", function () {
        for (var i = 0; i < menu.length; i++) {
            $(menu[i]).parent().find('.header-dropdown').removeClass("show-header-dropdown");
        }
        sub_menu_is_showed = -1;

    });
    $(document).click(function (e) {

        if (e.target.id !== 'searchRes' && e.target.id !== "searchtxt") {
            $("#searchRes").hide();
        }

    });


    /*[ Fixed Header ]
    ===========================================================*/
    var posWrapHeader = $('.topbar').height();
    var header = $('.container-menu-header');

    $(window).on('scroll', function () {

        if ($(this).scrollTop() >= posWrapHeader) {
            $('.header1').addClass('fixed-header');
            $(header).css('top', -posWrapHeader);

        }
        else {
            var x = - $(this).scrollTop();
            $(header).css('top', x);
            $('.header1').removeClass('fixed-header');
        }

        if ($(this).scrollTop() >= 200 && $(window).width() > 992) {
            $('.fixed-header2').addClass('show-fixed-header2');
            $('.header2').css('visibility', 'hidden');
            $('.header2').find('.header-dropdown').removeClass("show-header-dropdown");

        }
        else {
            $('.fixed-header2').removeClass('show-fixed-header2');
            $('.header2').css('visibility', 'visible');
            $('.fixed-header2').find('.header-dropdown').removeClass("show-header-dropdown");
        }

    });

    /*[ Show menu mobile ]
    ===========================================================*/
    $('.btn-show-menu-mobile').on('click', function () {
        $(this).toggleClass('is-active');
        $('.wrap-side-menu').slideToggle();
    });

    var arrowMainMenu = $('.arrow-main-menu');

    for (var j = 0; j < arrowMainMenu.length; j++) {
        $(arrowMainMenu[j]).on('click', function () {
            $(this).parent().find('.sub-menu').slideToggle();
            $(this).toggleClass('turn-arrow');
        })
    }
    var DropDown = $('.dropdown');

    for (var j = 0; j < arrowMainMenu.length; j++) {
        $(DropDown[j]).on('click', function () {
            $(this).parent().find('.sub-menu').slideToggle();
            $(arrowMainMenu).toggleClass('turn-arrow');
        })
    }
    $(window).resize(function () {
        if ($(window).width() >= 992) {
            if ($('.wrap-side-menu').css('display') === 'block') {
                $('.wrap-side-menu').css('display', 'none');
                $('.btn-show-menu-mobile').toggleClass('is-active');
            }
            if ($('.sub-menu').css('display') === 'block') {
                $('.sub-menu').css('display', 'none');
                $('.arrow-main-menu').removeClass('turn-arrow');
            }
        }
    });


    /*[ remove top noti ]
    ===========================================================*/
    $('.btn-romove-top-noti').on('click', function () {
        $(this).parent().remove();
    })


    /*[ Block2 button wishlist ]
    ===========================================================*/
    $('.block2-btn-addwishlist').on('click', function (e) {
        e.preventDefault();
        $(this).addClass('block2-btn-towishlist');
        $(this).removeClass('block2-btn-addwishlist');
        $(this).off('click');
    });
    $(document).on('click', '.block2-btn-addwishlist', function () {
        var pid = $(this).attr("data-pid");
        var ki = $(this).attr("data-kind");
        var popid = "#" + $(this).attr("data-zaddId");
        $.ajax({
            url: "/Generic/AddPopularity",
            data: { prId: pid, kind:ki },
            type: "GET",
            success: function (result) {

                $(popid).text(result);
                Swal.fire({
                    title: "با تشکر",
                    text: "نظر شما ثبت شد",
                    icon: "success",
                    showConfirmButton: false,
                    timer: 2000
                });
            }
        });
        $(this).addClass('block2-btn-towishlist');
        $(this).removeClass('block2-btn-addwishlist');
        $(this).off('click');
    });
    $(document).on('click', '.zaddwish', function () {
        var pid = $(this).attr("data-pid");
        var ki = $(this).attr("data-kind");
        var popid = "#" + $(this).attr("data-zaddId");

        // alert(popid);
        $.ajax({
            url: "/Generic/AddPopularity",
            data: { prId: pid, kind: ki },
            type: "GET",
            success: function (result) {
                $(popid).text(result);
                Swal.fire({
                    title: "با تشکر",
                    text: "نظر شما ثبت شد",
                    icon: "success",
                    showConfirmButton: false,
                    timer: 3000
                });

            }
        });
        $(this).addClass('zaddwish');
        $(this).removeClass('zaddwish');
        $(this).off('click');
    });
    /*[ +/- num product ]
    ===========================================================*/
    $('.btn-num-product-down').on('click', function (e) {
        e.preventDefault();
        var numProduct = Number($(this).next().val());
        if (numProduct > 1) $(this).next().val(numProduct - 1);
        var price = parseInt($(this).attr("data-itemnetprice"));
        var dis = parseInt($(this).attr("data-itemdis"));

        var sumdis = dis * (numProduct - 1);
        if (numProduct > 1) {
            var sum = price * (numProduct - 1);
        }
        var totalcount = parseInt($("#itemscounttotal").text());
        totalcount = totalcount - 1;
        $("#itemscounttotal").text(totalcount);

        $(this).parent().parent().parent().find(".rowSum").attr("data-rowsum", sum);
        $(this).parent().parent().parent().find(".rowSum").text(sum.toLocaleString());
        $(this).parent().parent().parent().find(".rowSum").attr("data-rowdissum", sumdis);
        $(this).parent().parent().parent().find(".rowdisSum").text(sumdis.toLocaleString());

        var totalsum = 0;
        var totaldissum = 0;
        $(".rowSum").each(function () {
            totalsum = totalsum + parseInt($(this).attr("data-rowsum"));
            totaldissum = totaldissum + parseInt($(this).attr("data-rowdissum"));
        });

        $("#cart-sum").text(totalsum.toLocaleString());
        $("#itemstotal").text(totalsum.toLocaleString());
        $("#itemsdistotal").text(totaldissum.toLocaleString());
        //=================


        var $btnsave = $(this).closest("tr").find('.save-btn');
        var quantity = parseInt($btnsave.attr("data-qu"));
        //alert("q : " + quantity + " | " + "num : " + (numProduct+1));
        if (quantity !== (numProduct - 1)) {
            $btnsave.removeAttr("disabled").addClass("text-white");
            $btnsave.attr("data-qu", (numProduct));
            $btnsave.removeClass("cursor-not-allowed").addClass("cursor-pointer");
            if ($(this).parent().parent().parent().find($(".zalarm")).length == 0) {
                var aalert = "<div class='zalarm block w-100 bg-danger text-white fs-9 mt-n10 mb-1'>" + "ثبت تغییرات فراموش نشود !" + "</div>";
                $btnsave.parent().prepend(aalert);
            }

        }
        else {
            $btnsave.attr("disabled", "disabled").removeClass("text-white").addClass("text-black");
            $btnsave.addClass("cursor-not-allowed").removeClass("cursor-pointer");
            $(this).parent().parent().parent().find($(".zalarm")).remove();
        }
    });

    $('.btn-num-product-up').on('click', function (e) {
        e.preventDefault();

        var numProduct = Number($(this).prev().val());
        $(this).prev().val(numProduct + 1);

        var totalcount = parseInt($("#itemscounttotal").text());
        totalcount = totalcount + 1;
        $("#itemscounttotal").text(totalcount);

        var price = parseInt($(this).attr("data-itemnetprice"));
        var dis = parseInt($(this).attr("data-itemdis"));
        var sumdis = dis * (numProduct + 1);
        var sum = price * (numProduct + 1);


        $(this).parent().parent().parent().find(".rowSum").attr("data-rowsum", sum);
        $(this).parent().parent().parent().find(".rowSum").text(sum.toLocaleString());
        $(this).parent().parent().parent().find(".rowSum").attr("data-rowdissum", sumdis);
        $(this).parent().parent().parent().find(".rowdisSum").text(sumdis.toLocaleString());
        var totalsum = 0; var totaldissum = 0;
        $(".rowSum").each(function () {
            totalsum = totalsum + parseInt($(this).attr("data-rowsum"));
            totaldissum = totaldissum + parseInt($(this).attr("data-rowdissum"));
        });

        $("#cart-sum").text(totalsum.toLocaleString());
        $("#itemstotal").text(totalsum.toLocaleString());
        $("#itemsdistotal").text(totaldissum.toLocaleString());
        //=================
        var $btnsave = $(this).closest("tr").find('.save-btn');
        var quantity = parseInt($btnsave.attr("data-qu"));
        //alert("q : " + quantity + " | " + "num : " + (numProduct+1));
        if (quantity !== (numProduct + 1)) {
            $btnsave.removeAttr("disabled").addClass("text-white");
            $btnsave.attr("data-qu", (numProduct));
            $btnsave.removeClass("cursor-not-allowed").addClass("cursor-pointer");
            if ($(this).parent().parent().parent().find($(".zalarm")).length == 0) {
                var aalert = "<div class='zalarm block w-100 bg-danger text-white fs-9 mt-n10 mb-1'>" + "ثبت تغییرات فراموش نشود !" + "</div>";
                $btnsave.parent().prepend(aalert);
            }
        }
        else {
            $btnsave.attr("disabled", "disabled").removeClass("text-white").addClass("text-black");
            $btnsave.addClass("cursor-not-allowed").removeClass("cursor-pointer");
            $(this).parent().parent().parent().find($(".zalarm")).remove();
        }





    });
    $(".save-btn").on('click', function () {

        var pid = parseInt($(this).attr("data-pid"));
        var numProduct = Number($(this).closest('tr').find(".num-product").val());
        var typ = $(this).attr("data-kind");
        $.ajax({
            url: "/Cart/AddToCart",
            dataType: "json",
            data: { productId: pid, count: numProduct, op: "eq", kind: typ },
            type: "Get",
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (result.added === true) {
                    Swal.fire({
                        title: "<span class='text-success'>عالیان سبز</span>",
                        text: "سبد خرید اصلاح شد",
                        icon: "success",
                        showConfirmButton: false,
                        timer: 3000
                    });
                    window.location.href = "/Cart";
                }
                else {
                    Swal.fire({
                        title: "هشــدار",
                        text: result.message,
                        icon: "warning",
                        confirmButtonText: "متوجه شدم",

                    });
                }

            }
        });
    });
    $(".remove-btn").on('click', function () {
        Swal.fire({
            title: "حذف محصول از سبد کالا",
            text: "آیا مطمئن به حذف محصول از سبد کالا هستید؟",
            icon: "warning",
            showCancelButton: true,
            cancelButtonColor: '#365ddc',
            cancelButtonText: "خــیر",
            showConfirmButton: true,
            confirmButtonColor: '#fc0000',
            confirmButtonText: 'بله',
            dangerMode: true,
        })
            .then((result) => {
                if (result.isConfirmed) {
                    var itId = $(this).attr("data-itemId");
                    $.ajax({
                        url: "/Cart/RemoveCartItem",
                        dataType: "json",
                        data: { itemId: itId },
                        type: "Get",
                        contentType: 'application/json; charset=utf-8',
                        success: function (result) {
                            if (result.removed === true) {
                                window.location.href = "/Cart";

                            }
                            else {
                                Swal.fire({
                                    title: "ثبت تغییرات",
                                    text: "خطایی رخ داده است",
                                    icon: "warning",
                                    showConfirmButton: false,
                                    timer: 2000
                                });
                            }
                        }
                    });
                }
            });
    })
    $(".num-product").on("input", function () {
        var nVal = $(this).attr("data-num");
        var tVal = Number($(this).val());
        var price = parseInt($(this).attr("data-price"));
        var sum = price * tVal;
        var $btnsave = $(this).closest("tr").find('.save-btn');
        if (nVal !== tVal) {
            $(this).parent().parent().parent().find(".rowSum").attr("data-rowsum", sum);
            $(this).parent().parent().parent().find(".rowSum").text(sum.toLocaleString());
            var totalsum = 0;
            $(".rowSum").each(function () {
                totalsum = totalsum + parseInt($(this).attr("data-rowsum"));
            });

            $("#cart-sum").text(totalsum.toLocaleString());

            $btnsave.removeClass("disabled").addClass("text-success");
            $btnsave.attr("data-qu", tVal);
            $btnsave.removeClass("cursor-not-allowed").addClass("cursor-pointer");
        }
        else {
            $btnsave.addClass("disabled").removeClass("text-success");
            $btnsave.addClass("cursor-not-allowed").removeClass("cursor-pointer");
        }

    });

    /*[ Show content Product detail ]
    ===========================================================*/
    $('.active-dropdown-content .js-toggle-dropdown-content').toggleClass('show-dropdown-content');
    $('.active-dropdown-content .dropdown-content').slideToggle('fast');

    $('.js-toggle-dropdown-content').on('click', function () {
        $(this).toggleClass('show-dropdown-content');
        $(this).parent().find('.dropdown-content').slideToggle('fast');
    });


    /*[ Play video 01]
    ===========================================================*/
    var srcOld = $('.video-mo-01').children('iframe').attr('src');

    $('[data-target="#modal-video-01"]').on('click', function () {
        $('.video-mo-01').children('iframe')[0].src += "&autoplay=1";

        setTimeout(function () {
            $('.video-mo-01').css('opacity', '1');
        }, 300);
    });

    $('[data-dismiss="modal"]').on('click', function () {
        $('.video-mo-01').children('iframe')[0].src = srcOld;
        $('.video-mo-01').css('opacity', '0');
    });



    /* Cart*/
    //,.block-btn-bottom-addcart

    $(document).on("click", ".block2-btn-addcart,.block-btn-bottom-addcart", function () {

        var pid = $(this).attr("data-pid");
        var type = $(this).attr("data-kind");
        var zEnabled = navigator.cookieEnabled;

        if (!zEnabled) {

            Swal.fire({
                title: "<span class='text-success'>عالیان سبز</span>",
                text: "مجوز ثبت داده (کوکی) در مرورگر شما فعال نمی باشد، جهت ایجاد سبد خرید لطفا در سایت ثبت نام کنید!",
                icon: "warning",
                showCancelButton: true,
                showConfirmButton: true,
                cancelButtonText: "انصراف",
                confirmButtonText: "ثبت نام"
            }).then((data) => {

                if (data.isConfirmed) {
                    window.location.href = "/Register?ReturnUrl=" + $(location).attr('pathname');
                }
            });
            return;
        }
        $.ajax({
            url: "/Cart/AddToCart",
            data: { productId: pid, count: 1, op: "plus", kind: type },
            type: "GET",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (result.added === true) {
                    $.ajax({
                        url: "/Cart/ShowCart",
                        data: {},
                        type: "POST",
                        success: function (data) {
                            $("#cartmd").html(data);
                            $("#cartxd").html(data);
                        }
                    });

                    $(".header-icons-noti").html(result.count);

                    Swal.fire({
                        title: "<span class='text-success'>عالیان سبز</span>",
                        text: result.message,
                        icon: 'success',
                        showConfirmButton: false,
                        timer: 1500
                    })
                }
                else {

                    Swal.fire({
                        title: "<span class='text-success'>عالیان سبز</span>",
                        text: result.message,
                        icon: "warning",
                        showCancelButton: true,
                        showConfirmButton: false,
                        cancelButtonText: 'متوجه شدم'

                    });
                }


            },

            error: function () {

                swal({
                    title: "<span class='text-success'>عالیان سبز</span>",
                    text: "متاسفانه عملیات با خطا مواجه شده است، لطفا به مدیرسایت اطلاع دهید !",
                    icon: "warning",
                    showCancelButton: true,
                    showConfirmButton: false,
                    cancelButtonText: 'متوجه شدم'

                });
            }



        });

    });
    //$(".btn-addcart-product-detail").click(function () {
    //    var pid = $(this).attr("data-pid");
    //    var cnt = $(this).siblings(".num-product").val();
    //    alert(pid + " | " + cnt);
    //});

    $(document).on("click", ".btn-addcart-product-detail", function () {

        var pid = $(this).attr("data-pid");
        var curpcount = $(this).attr("data-curpcount");
        var cnt = $(this).parent().find(".num-product").val();
        var type = $(this).attr("data-kind");

        var p_op = $(this).attr("data-op");
        if (cnt === curpcount) {
            Swal.fire({
                title: "<span class='text-success'>عالیان سبز</span>",
                text: "اگر موجودی کالا در سبد تغییر پیدا کند، به سبد خرید اعمال خواهد شد !",
                icon: "warning",
                showConfirmButton: true,
                showCancelButton: false,
                confirmButtonText: "متوجه شدم"
            });
            return false;
        }
        var zEnabled = navigator.cookieEnabled;
        if (!zEnabled) {

            Swal.fire({
                title: "<span class='text-success'>عالیان سبز</span>",
                text: "مجوز ثبت داده (کوکی) در مرورگر شما فعال نمی باشد، لطفا در سایت ثبت نام کنید!",
                icon: "warning",
                showCancelButton: true,
                showConfirmButton: true,
                cancelButtonText: "انصراف",
                confirmButtonText: "ثبت نام"
            }).then((result) => {

                if (result.isConfirmed == true) {
                    window.location.href = "/Register";
                }
            });
            return;
        }
        $.ajax({
            url: "/Cart/AddToCart",
            dataType: "json",
            data: { productId: pid, count: cnt, op: p_op, kind: type },
            type: "Get",
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (result.added === true) {
                    $.ajax({
                        url: "/Cart/ShowCart",
                        data: {},
                        type: "GET",
                        success: function (result) {

                            $("#cartmd").html(result);
                            $("#cartxd").html(result);
                        }
                    });
                    //alert(result.count);
                    $(".header-icons-noti").text(result.count);
                    $(this).siblings(".num-product").val(result.count);
                    Swal.fire({
                        title: "<span class='text-success text-center col-12'>عالیان سبز</span>",
                        text: result.message,
                        icon: "success",
                        showConfirmButton: false,
                        timer: 2000
                    });
                    window.location.reload();
                }
                else {
                    Swal.fire({
                        title: "<span class='text-success'>عالیان سبز</span>",
                        text: result.message,
                        icon: "warning",
                        showConfirmButton: false,
                        showCancelButton: true,
                        cancelButtonText: "متوجه شدم"
                    });
                }
            },
            error: function () {
                Swal.fire({
                    title: "<span class='text-success'>عالیان سبز</span>",
                    text: "متاسفانه عملیات با خطا مواجه شده است، لطفا به مدیرسایت اطلاع دهید !",
                    icon: "warning",
                    showConfirmButton: false,
                    showCancelButton: true,
                    cancelButtonText: "متوجه شدم",

                });
            }
        });


    });

    $(".header-cart-item-remove").on("click", function () {

        var prname = $(this).attr("data-product-name");
        var delmes = "آیا مطمئن به حذف محصول " + prname + " از سبد کالا هستید؟";
        Swal.fire({
            title: "حذف محصول از سبد کالا",
            text: delmes,
            icon: "warning",
            showCancelButton: true,
            cancelButtonColor: '#365ddc',
            cancelButtonText: "خــیر",
            showConfirmButton: true,
            confirmButtonColor: '#fc0000',
            confirmButtonText: "بــله",

        })
            .then((result) => {
                if (result.isConfirmed) {
                    var itId = $(this).attr("data-cart-item-id");
                    $.ajax({
                        url: "/Cart/RemoveCartItem",
                        dataType: "json",
                        data: { itemId: itId },
                        type: "Get",
                        contentType: 'application/json; charset=utf-8',
                        success: function (result) {
                            if (result.removed === true) {
                                $.ajax({
                                    url: "/Cart/ShowCart",
                                    data: {},
                                    type: "GET",
                                    success: function (result) {
                                        $("#cartmd").html(result);
                                        $("#cartxd").html(result);
                                    }
                                });
                                $(".header-icons-noti").text(result.count);
                                if (result.count === 0) {
                                    $("#zCart").empty();
                                    var str = "<h1 class='text-xs-center alert alert-danger'>" + "سبد خرید خالی می باشد !" + "</h1>"
                                    $("#zCart").append(str);
                                }

                                Swal.fire({
                                    title: "<span class='text-success'>عالیان سبز</span>",
                                    text: "محصول از سبد خرید حذف شد !",
                                    icon: "success",
                                    showConfirmButton: false,
                                    timer: 3000
                                });

                            }
                            else {
                                Swal.fire({
                                    title: "<span class='text-success'>عالیان سبز</span>",
                                    text: "خطایی رخ داده است",
                                    icon: "warning",
                                    showConfirmButton: false,
                                    timer: 3000
                                });
                            }
                        }
                    });
                }
            });
    });
    $("#searchtxt").on("keyup", function () {
        var th = $(this).val();
        if (th !== '') {
            $.ajax({
                url: "/Generic/SearchResult",
                data: { text: th },
                type: "GET",
                contentType: 'application/json; charset=utf-8',
                async: true,
                cache: false,
            }).done(function (data) {
                $("#searchRes").empty();
                $.each(data, function (index, item) {
                    $("#searchRes").show();
                    $("#searchRes").append(item);

                });

            });
        }
        else {
            $("#searchRes").empty();
        }
    });
    $(document).on("click", "#searchRes .srchP", function () {
        var thval = $(this).text();
        $("#searchtxt").val(thval);
        $("#searchtxt").focus();
        $("#searchRes").empty();
    });
    $(document).find("#AddEmail").click(function () {
        var em = $("#inpEmail").val();
        if (em.length === 0) {
            Swal.fire({
                title: "<span class='text-success'><span class='text-success'>عالیان سبز</span></span>",
                text: "لطفا ایمیل خود را وارد کنید !",
                icon: "warning",
                showConfirmButton: false,
                showCancelButton: true,
                cancelButtonText: "متوجه شدم"
            });
            return false;
        }
        if (isValidEmail(em)) {
            $.ajax({
                url: "/Generic/AddEmail",
                dataType: "json",
                data: { email: em },
                type: "GET",
                success: function (result) {
                    var con = "success"
                    if (result.eresult == false) {
                        con = "warning";
                    }
                    Swal.fire({
                        text: result.message,
                        icon: con,
                        showConfirmButton: false,
                        timer: 3000
                    });
                },
                error: function (result) {
                    Swal.fire({
                        title: "<span class='text-success'>عالیان سبز</span>",
                        text: result,
                        icon: "warning",
                        showConfirmButton: false,
                        timer: 3000
                    });
                }
            });
        }
        else {
            Swal.fire({
                title: "<span class='text-success'>عالیان سبز</span>",
                text: "ایمیل وارد شده نامعتبر است !",
                icon: "warning",
                showConfirmButton: false,
                timer: 3000
            });
        }
    });

    $(document).on("click", ".sliderBtn", function () {
        var slid = $(this).attr("data-id");

        $.ajax({
            url: "/Generic/AddViews",
            data: { table: 'slider', id: slid },
            async: true,
            type: "GET",
            success: function (result) {

            }
        })
    });
    $(document).on("click", ".productBtn", function () {
        var slid = $(this).attr("data-id");
        $.ajax({
            url: "/Generic/AddViews",
            data: { table: 'product', id: slid },
            async: true,
            type: "GET",
            success: function (result) {

            }
        })
    });
    $(document).on("click", ".blogBtn", function () {
        var slid = $(this).attr("data-id");

        $.ajax({
            url: "/Generic/AddViews",
            data: { table: 'blog', id: slid },
            async: true,
            type: "GET",
            success: function (result) {

            }
        })
    });
    $(document).on("click", ".bannerBtn", function () {
        var slid = $(this).attr("data-id");
        $.ajax({
            url: "/Generic/AddViews",
            data: { table: 'banner', id: slid },
            async: true,
            type: "GET",
        })
    });
    $("[data-val-required]").each(function () {
        var label = $('label[for="' + $(this).attr("id") + '"]');
        var asterisksHtml = '<span style="color:red;font-size:18px;font-weight:700">&nbsp;*</span>';
        if (label.text().length > 0 && label.html().indexOf(asterisksHtml) === -1) {
            label.append(asterisksHtml);
        }
    });

    $("#btnApplyCoupen").click(function () {

        var Dcoupen = $("#couponcode").val();
        if (Dcoupen === "") {
            //toastr.warning("Warning");
            Swal.fire({
                title: "هشدار",
                text: "کوپن تخفیف وارد نشده است !",
                icon: "warning",
                showCancelButton: true,
                showConfirmButton: false,
                cancelButtonText: "متوجه شدم"
            });
        }
        $.ajax({
            url: "/Cart/ApplyCoupen",
            data: { coupen: Dcoupen },
            type: "Post",

        }).done(function (data) {
            if (data.success) {
                Swal.fire({
                    title: "<span class='text-success'>عالیان سبز</span>",
                    text: data.message,
                    icon: "success",
                    showConfirmButton: true,
                    confirmButtonText: "متوجه شدم",
                }).then((result) => {
                    if (result.isConfirmed == true) {
                        window.location.href = "/Cart";
                    }
                });
            }
            else {
                Swal.fire({
                    title: "<span class='text-success'>عالیان سبز</span>",
                    text: data.message,
                    icon: "warning",
                    showConfirmButton: true,
                    confirmButtonText: "متوجه شدم",
                });
            }
        });
    });
    //Forgot password
    var spp = $(document).find("#sendpass").val();
    var sp = $("#sendpass").val();
    if (spp !== "") {
        if (spp === "true") {

            Swal.fire({
                title: "<span class='text-success'>عالیان سبز</span>",
                text: "رمز عبور به تلفن همراه تان ارسال شد",
                icon: "success",
                showConfirmButton: false,
                showCancelButton: true,
                cancelButtonText: "متوجه شدم"
            });
        }
    }

    var maxchars = 500;
    $(document).on('keyup', "#Pdcomment", function () {
        var tlength = $(this).val().length;

        $(this).val($(this).val().substring(0, maxchars));
        var tlength = $(this).val().length;
        var remain = maxchars - parseInt(tlength);
        //alert(remain);
        $('#Pdchar-count').text(remain);
    });
    $(document).on('keyup', "#Bdcomment", function () {
        var tlength = $(this).val().length;

        $(this).val($(this).val().substring(0, maxchars));
        var tlength = $(this).val().length;
        var remain = maxchars - parseInt(tlength);

        $('#Bdchar-count').text(remain);
    });
    $(document).on('change', '#optionSort', function () {
        var soption = $(this).val();
        var ps = $("#priceSelect").val();
        var gid = $('option:selected', this).attr("data-groupId");
        var page_id = $('option:selected', this).attr("data-pageId");
        var recpp = $('option:selected', this).attr('data-recperpage');
        $.ajax({
            url: "/Generic/ProductSort",
            data: { gId: gid, page: page_id, sortOption: soption, priceSelect: ps, recperpage: recpp },
            type: "POST",
            async: true,
            cache: false,
            success: function (result) {
                $("#productListrow").html(result);
                var s = $(document).find("#sortCount");
                $(document).find("#resCount").text(s.val());
            },
            error: function () {
                alert("error");
            }
        });
    });

    $("#priceSelect").change(function () {
        var ps = $(this).val();
        var soption = $("#optionSort").val();
        var gid = $('option:selected', this).attr("data-groupId");
        var page_id = $('option:selected', this).attr("data-pageId");
        var recpp = $('option:selected', this).attr('data-recperpage');
        $.ajax({
            url: "/Generic/ProductSort",
            data: { gId: gid, page: page_id, priceSelect: ps, sortOption: soption, recperpage: recpp },
            type: "POST",
            async: true,
            cache: false,
            success: function (result) {
                $("#productListrow").html(result);
                var s = $(document).find("#sortCount");
                $(document).find("#resCount").text(s.val());
            },
            error: function () {
                alert("error");
            }
        });
    });

    $(document).on("keyup paste input", "#howtouse-myInput", function () {

        var value = $(this).val().toLowerCase();
        $("#tblProducts tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $("#FbtnBuyHelping , #HbtnBuyHelping").on("click", function () {

        $("#IntroModal").modal();
    });

})(jQuery);
(function ($) {

    /** ************************* **/
    /** Methods  **/
    /** ************************* **/
    var slider = null, methods = {
        init: function (option) {

            // defaults & options
            var options = $.extend({
                'slideTransition': 'none', // default slide transition
                'slideTransitionSpeed': 2000, // default slide transition
                'slideEndAnimation': true, // if set true, objects will transition out at slide end (before the slideTransition is called)
                'position': '0,0', // default position | should never be used
                'transitionIn': 'left', // default in - transition
                'transitionOut': 'left', // default out - transition
                'fullWidth': true, // transition over the full width of the window
                'delay': 0, // default delay for elements
                'timeout': 2000, // default timeout before switching slides
                'speedIn': 2500, // default in - transition speed
                'speedOut': 1000, // default out - transition speed
                'easeIn': 'easeOutExpo', // default easing in
                'easeOut': 'easeOutCubic', // default easing out

                'controls': false, // controls on/off
                'pager': false, // controls on/off
                'autoChange': true, // auto change slides
                'pauseOnHover': false, // Pauses slider on hover (current step will still be completed)

                'backgroundAnimation': false, // background animation
                'backgroundElement': null, // element to animate | default fractionSlider element
                'backgroundX': 500, // default x distance
                'backgroundY': 500, // default y distance
                'backgroundSpeed': 2500, // default background animation speed
                'backgroundEase': 'easeOutCubic', // default background animation easing

                'responsive': true, // activates the responsive slider
                'increase': false, // if set, slider is allowed to get bigger than basic dimensions
                'dimensions': '', // set basic dimension (width,height in px) for the responisve slider - the plugin with position elements with data-position relative to this dimensions (please see the documentation for more info),

                'startCallback': null,
                'startNextSlideCallback': null,
                'stopCallback': null,
                'pauseCallback': null,
                'resumeCallback': null,
                'nextSlideCallback': null,
                'prevSlideCallback': null,
                'pagerCallback': null
            }, option);

            return this.each(function () {
                // ready for take-off
                slider = new FractionSlider(this, options);
            });
        },
        pause: function () {
            slider.pause(true);
        },
        resume: function () {
            slider.resume();
        },
        stop: function () {
            slider.stop();
        },
        start: function () {
            slider.start();
        },
        startNextSlide: function () {
            slider.startNextSlide();
        }
    };

    /** ************************* **/
    /** SLDIER CLASS  **/
    /** ************************* **/

    // here happens all the fun
    var FractionSlider = function (element, options) {

        var vars = {
            init: true, // initialised the first time
            running: false, // currently running
            pause: false,
            stop: false,
            slideComplete: false, // current slide complete (needed for pause method)
            stepComplete: false,
            controlsActive: true, // controls pressed
            currentSlide: 0, // current slide number
            lastSlide: null, // last slide number (for anim out)
            maxSlide: 0, // max slide number
            currentStep: 0, // current step number
            maxStep: 0, // max step number
            currentObj: 0, // curent object number (in step)
            maxObjs: 0, // max object number (in step)
            finishedObjs: 0 // finsihed objects (in step)
        },

            // Here are Slide elements temporarily stored
            temp = {
                currentSlide: null, // current Slide
                lastSlide: null, // last Slide (for anim out)
                animationkey: 'none' // animation for slidechange
            }, timeouts = [], fractionObjs = null,
            // objs for current step
            dX = null, dY = null;

        $(element).wrapInner('<div class="fraction-slider" />');

        var slider = $(element).find('.fraction-slider'), pager = null;
        // the slider element

        vars.maxSlide = slider.children('.slide').length - 1;

        // some more needed vars
        var sliderWidth = slider.width(), bodyWidth = $('body').width(), offsetX = 0;

        if (options.fullWidth) {
            offsetX = (bodyWidth - sliderWidth) / 2;
            sliderWidth = bodyWidth;
        }

        var sliderHeight = slider.height();

        init();

        /** ************************* **/
        /** INITIALIZE **/
        /** ************************* **/

        function init() {

            // controls
            if (options.controls) {
                slider.append('<a href="#" class="prev"></a><a href="#" class="next" ></a>');

                slider.find('.next').bind('click', function () {
                    return nextBtnPressed();
                });

                slider.find('.prev').bind('click', function () {
                    return prevBtnPressed();
                });
            }

            if (options.pauseOnHover) {
                slider.bind({
                    mouseenter: function () { pause(false); },
                    mouseleave: function () { resume(); }
                });
            }

            // fullwidth setup
            if (options.fullWidth) {
                slider.css({
                    'overflow': 'visible'
                });
            } else {
                slider.css({
                    'overflow': 'hidden'
                });
            }

            if (options.pager) {
                var customPager = (typeof options.pager !== 'boolean');
                pager = (customPager) ? options.pager : $('<div class="fs-pager-wrapper"></div>');
                if (!customPager) {
                    slider.append(pager);
                } else {
                    pager.addClass('fs-custom-pager-wrapper');
                }
            }

            slider.children('.slide').each(function (index) {
                var slide = $(this);
                slide.children().attr('rel', index).addClass('fs_obj');
                slide.children('[data-fixed]').addClass('fs_fixed_obj');

                // pager again
                if (options.pager || customPager) {
                    var tempObj = $('<a rel="' + index + '" href="#"></a>').bind('click', function () {
                        return pagerPressed(this);
                    });
                    pager.append(tempObj);
                }
            });
            if (options.pager) {
                pager = $(pager).children('a');
            }

            // responisve
            if (options.responsive) {
                makeResponsive();
            }

            // remove spinner
            if (slider.find('.fs_loader').length > 0) {
                slider.find('.fs_loader').remove();
            }

            // all importent stuff is done, everybody has taken a shower, we can go
            // starts the slider and the slide rotation
            start();
        }

        /** ************************* **/
        /** METHODES **/
        /** ************************* **/

        function start() {
            vars.stop = false;
            vars.pause = false;
            vars.running = true;

            cycle('slide');

            methodeCallback(options.startCallback);
        }


        this.start = function () {// for method calls
            start();
        };

        function startNextSlide() {
            vars.stop = false;
            vars.pause = false;
            vars.running = true;

            nextSlide();

            methodeCallback(options.startNextSlideCallback);
        }


        this.startNextSlide = function () {// for method calls
            startNextSlide();
        };

        // use with start or startNextSlide
        function stop() {
            vars.stop = true;
            vars.running = false;

            slider.find('.slide').stop(true, true);
            slider.find('.fs_obj').stop(true, true).removeClass('fs-animation');
            stopTimeouts(timeouts);

            methodeCallback(options.stopCallback);
        }


        this.stop = function () {// for method calls
            stop();
        };

        // use with resume
        function pause(finish) {
            vars.pause = true;
            vars.running = false;

            if (finish) {
                slider.find('.fs-animation').finish(); // finish the current step
            }

            methodeCallback(options.pauseCallback);
        }


        this.pause = function () {// for method calls
            pause(false);
        };

        // use with pause
        function resume() {
            vars.stop = false;
            vars.pause = false;
            vars.running = true;

            if (vars.slideComplete) {
                cycle('slide');
            } else if (vars.stepComplete) {
                cycle('step');
            } else {
                if (vars.finishedObjs < vars.maxObjs) {
                    // do nothing - elements are still animating
                } else if (vars.currentStep < vars.maxStep) { // IS THIS STILL NEEDED?
                    cycle('step'); // IS THIS STILL NEEDED?
                } else {
                    cycle('slide'); // IS THIS STILL NEEDED?
                }
            }

            methodeCallback(options.resumeCallback);
        }


        this.resume = function () {// for method calls
            resume();
        };

        function nextSlide() {
            vars.lastSlide = vars.currentSlide;
            vars.currentSlide += 1;

            vars.stop = false;
            vars.pause = false;
            vars.running = true;

            slideChangeControler();

            methodeCallback(options.nextSlideCallback);
        }

        function prevSlide() {
            vars.lastSlide = vars.currentSlide;
            vars.currentSlide -= 1;

            vars.stop = false;
            vars.pause = false;
            vars.running = true;

            slideChangeControler();

            methodeCallback(options.prevSlideCallback);
        }

        function targetSlide(slide) {
            vars.lastSlide = vars.currentSlide;
            vars.currentSlide = slide;

            vars.stop = false;
            vars.pause = false;
            vars.running = true;

            slideChangeControler();

            methodeCallback(options.pagerCallback);
        }

        /** ************************* **/
        /** CALLBACKS **/
        /** ************************* **/

        function methodeCallback(callback) {
            $.isFunction(callback) && callback.call(this, slider, vars.currentSlide, vars.lastSlide, vars.currentStep);
        }

        /** ************************* **/
        /** PAGER & CONTROLS **/
        /** ************************* **/

        function pagerPressed(el) {
            var target = parseInt($(el).attr('rel'));

            if (target != vars.currentSlide) {
                stop();
                targetSlide(target);
            }
            return false;
        }

        function prevBtnPressed() {
            stop();
            prevSlide();
            return false;
        }

        function nextBtnPressed() {
            stop();
            nextSlide();
            return false;
        }

        /** ************************* **/
        /** CYCLE CONTROLLER **/
        /** ************************* **/

        function cycle(type) {
            if (!vars.pause && !vars.stop && vars.running) {
                switch (type) {
                    case "slide":
                        vars.slideComplete = false;
                        slideRotation();
                        break;
                    case "step":
                        vars.stepComplete = false;
                        iterateSteps();
                        break;
                    case "obj":
                        iterateObjs();
                        break;
                }
            }
        }

        /** ************************* **/
        /** SLIDES **/
        /** ************************* **/

        function slideRotation() {

            var timeout = options.timeout;
            // set timeout | first slide instant start
            if (vars.init) {
                vars.init = false;
                slideChangeControler(true);
            } else {
                // timeout after slide is complete
                timeouts.push(setTimeout(function () {
                    // stops the slider after first slide (only when slide count = 1)
                    if (vars.maxSlide == 0 && vars.running == true) {
                        // TODO: better solution!
                    } else {
                        vars.lastSlide = vars.currentSlide;
                        vars.currentSlide += 1;
                        slideChangeControler();
                    }
                }, timeout));
            }
        }

        function slideChangeControler(init) {
            slider.find('.active-slide').removeClass('active-slide');

            if (vars.currentSlide > vars.maxSlide) {
                vars.currentSlide = 0;
            }
            if (vars.currentSlide < 0) {
                vars.currentSlide = vars.maxSlide;
            }

            temp.currentSlide = slider.children('.slide:eq(' + vars.currentSlide + ')').addClass('active-slide');

            if (temp.currentSlide.length == 0) {
                vars.currentSlide = 0;
                temp.currentSlide = slider.children('.slide:eq(' + vars.currentSlide + ')');
            }

            if (vars.lastSlide != null) {
                if (vars.lastSlide < 0) {
                    vars.lastSlide = vars.maxSlide;
                }

                temp.lastSlide = slider.children('.slide:eq(' + vars.lastSlide + ')');
            }

            if (init) {
                temp.animation = 'none';
            } else {
                temp.animation = temp.currentSlide.attr("data-in");

                if (temp.animation == null) {
                    temp.animation = options.slideTransition;
                }
            }

            if (options.slideEndAnimation && vars.lastSlide != null) {
                objOut();
            } else {
                switch (temp.animation) {
                    case 'none':
                        startSlide();
                        endSlide();
                        break;
                    case 'scrollLeft':
                        startSlide();
                        endSlide();
                        break;
                    case 'scrollRight':
                        startSlide();
                        endSlide();
                        break;
                    case 'scrollTop':
                        startSlide();
                        endSlide();
                        break;
                    case 'scrollBottom':
                        startSlide();
                        endSlide();
                        break;
                    default:
                        startSlide();
                        break;
                }
            }
        }

        // starts a slide
        function startSlide() {
            if (options.backgroundAnimation) {
                backgroundAnimation();
            }

            if (options.pager) {
                pager.removeClass('active');
                pager.eq(vars.currentSlide).addClass('active');
            }

            getStepsForSlide();

            temp.currentSlide.children().hide();

            vars.currentStep = 0;
            vars.currentObj = 0;
            vars.maxObjs = 0;
            vars.finishedObjs = 0;

            temp.currentSlide.children("[data-fixed]").show();

            slideAnimationIn();
        }

        function startSlideComplete(slide) {
            if (temp.lastSlide != null) {
                temp.lastSlide.hide();
            }
            if (slide.hasClass('active-slide')) {
                // starts the cycle for the current slide
                cycle('step');
            }
        }

        // ends a slide
        function endSlide() {
            if (temp.lastSlide == null) {
                return;
            }
            if (temp.animation != 'none') {
                slideAnimationOut();
            }
        }

        function endSlideComplete() {
        }

        /** ************************* **/
        /** STEPS **/
        /** ************************* **/

        // gets the maximum step for the current slide
        function getStepsForSlide() {
            var objs = temp.currentSlide.children(), maximum = 0;

            objs.each(function () {
                var value = parseFloat($(this).attr('data-step'));
                maximum = (value > maximum) ? value : maximum;
            });

            vars.maxStep = maximum;
        }

        function iterateSteps() {
            var objs;

            if (vars.currentStep == 0) {
                objs = temp.currentSlide.children('*:not([data-step]):not([data-fixed]), *[data-step="' + vars.currentStep + '"]:not([data-fixed])');
            } else {
                objs = temp.currentSlide.children('*[data-step="' + vars.currentStep + '"]:not([data-fixed])');
            }

            vars.maxObjs = objs.length;

            fractionObjs = objs;

            if (vars.maxObjs > 0) {

                vars.currentObj = 0;
                vars.finishedObjs = 0;

                cycle('obj');
            } else {
                stepFinished();
            }
        }

        function stepFinished() {
            vars.stepComplete = true;
            vars.currentStep += 1;

            if (vars.currentStep > vars.maxStep) {
                if (options.autoChange) {
                    vars.currentStep = 0;
                    vars.slideComplete = true;
                    cycle('slide');
                }

                return;
            }
            cycle('step');
        }

        /** ************************* **/
        /** OBJECTS **/
        /** ************************* **/

        function iterateObjs() {
            var obj = $(fractionObjs[vars.currentObj]);

            obj.addClass('fs-animation');

            var position = obj.attr("data-position"), transition = obj.attr("data-in"), delay = obj.attr("data-delay"), time = obj.attr('data-time'), easing = obj.attr('data-ease-in'),
                // check for special options
                special = obj.attr("data-special");

            if (position == null) {
                position = options.position.split(',');
            } else {
                position = position.split(',');
            }
            if (transition == null) {
                transition = options.transitionIn;
            }
            if (delay == null) {
                delay = options.delay;
            }
            if (easing == null) {
                easing = options.easeIn;
            }
            objectAnimationIn(obj, position, transition, delay, time, easing, special);

            vars.currentObj += 1;

            if (vars.currentObj < vars.maxObjs) {
                cycle('obj');
            } else {
                vars.currentObj = 0;
            }
        }

        function objFinished(obj) {
            obj.removeClass('fs-animation');

            if (obj.attr('rel') == vars.currentSlide) {
                vars.finishedObjs += 1;
                if (vars.finishedObjs == vars.maxObjs) {
                    stepFinished();
                }
            }
        }

        function objOut() {
            var objs = temp.lastSlide.children(':not([data-fixed])');

            objs.each(function () {
                var obj = $(this), position = obj.position(), transition = obj.attr("data-out"), easing = obj.attr("data-ease-out");

                if (transition == null) {
                    transition = options.transitionOut;
                }

                if (easing == null) {
                    easing = options.easeOut;
                }

                objectAnimationOut(obj, position, transition, null, easing);
            }).promise().done(function () {
                endSlide();
                startSlide();
            });
        }

        /** ************************* **/
        /** TRANSITIONS **/
        /** ************************* **/

        /** TRANSITION SLIDES **/

        function slideAnimationIn() {
            var slide = temp.currentSlide, cssStart = {}, cssEnd = {}, speed = options.slideTransitionSpeed, animation = temp.animation;

            if (options.responsive) {
                unit = '%';
            } else {
                unit = 'px';
            }

            switch (animation) {
                case 'slideLeft':
                    cssStart.left = sliderWidth + unit;
                    cssStart.top = '0' + unit;
                    cssStart.display = 'block';
                    cssEnd.left = '0' + unit;
                    cssEnd.top = '0' + unit;
                    break;
                case 'slideTop':
                    cssStart.left = '0' + unit;
                    cssStart.top = -sliderHeight + unit;
                    cssStart.display = 'block';
                    cssEnd.left = '0' + unit;
                    cssEnd.top = '0' + unit;
                    break;
                case 'slideBottom':
                    cssStart.left = '0' + unit;
                    cssStart.top = sliderHeight + unit;
                    cssStart.display = 'block';
                    cssEnd.left = '0' + unit;
                    cssEnd.top = '0' + unit;
                    break;
                case 'slideRight':
                    cssStart.left = -sliderWidth + unit;
                    cssStart.top = '0' + unit;
                    cssStart.display = 'block';
                    cssEnd.left = '0' + unit;
                    cssEnd.top = '0' + unit;
                    break;
                case 'fade':
                    cssStart.left = '0' + unit;
                    cssStart.top = '0' + unit;
                    cssStart.display = 'block';
                    cssStart.opacity = 0;
                    cssEnd.opacity = 1;
                    break;
                case 'none':
                    cssStart.left = '0' + unit;
                    cssStart.top = '0' + unit;
                    cssStart.display = 'block';
                    speed = 0;
                    break;
                case 'scrollLeft':
                    cssStart.left = sliderWidth + unit;
                    cssStart.top = '0' + unit;
                    cssStart.display = 'block';
                    cssEnd.left = '0' + unit;
                    cssEnd.top = '0' + unit;
                    break;
                case 'scrollTop':
                    cssStart.left = '0' + unit;
                    cssStart.top = -sliderHeight + unit;
                    cssStart.display = 'block';
                    cssEnd.left = '0' + unit;
                    cssEnd.top = '0' + unit;
                    break;
                case 'scrollBottom':
                    cssStart.left = '0' + unit;
                    cssStart.top = sliderHeight + unit;
                    cssStart.display = 'block';
                    cssEnd.left = '0' + unit;
                    cssEnd.top = '0' + unit;
                    break;
                case 'scrollRight':
                    cssStart.left = -sliderWidth + unit;
                    cssStart.top = '0' + unit;
                    cssStart.display = 'block';
                    cssEnd.left = '0' + unit;
                    cssEnd.top = '0' + unit;
                    break;
            }

            slide.css(cssStart).animate(cssEnd, speed, 'linear', function () {
                startSlideComplete(slide);
            });
        }

        function slideAnimationOut() {
            var cssStart = {}, cssEnd = {}, speed = options.slideTransitionSpeed, unit = null, animation = temp.animation;

            if (options.responsive) {
                unit = '%';
            } else {
                unit = 'px';
            }

            switch (animation) {
                // case 'none':
                //  cssStart.display = 'none';
                //  speed = 0
                //  break;
                case 'scrollLeft':
                    cssEnd.left = -sliderWidth + unit;
                    cssEnd.top = '0' + unit;
                    break;
                case 'scrollTop':
                    cssEnd.left = '0' + unit;
                    cssEnd.top = sliderHeight + unit;
                    break;
                case 'scrollBottom':
                    cssEnd.left = '0' + unit;
                    cssEnd.top = -sliderHeight + unit;
                    break;
                case 'scrollRight':
                    cssEnd.left = sliderWidth + unit;
                    cssEnd.top = '0' + unit;
                    break;
                default:
                    speed = 0;
                    break;
            }

            temp.lastSlide.animate(cssEnd, speed, 'linear', function () {
                endSlideComplete();
            });
        }

        /** IN TRANSITION OBJECTS **/
        function objectAnimationIn(obj, position, transition, delay, time, easing, special) {
            var cssStart = {}, cssEnd = {}, speed = options.speedIn, unit = null;

            if (options.responsive) {
                unit = '%';
            } else {
                unit = 'px';
            }

            // #time
            if (time != null && time != "") {
                speed = time - delay;
            }

            cssStart.opacity = 1;

            // set start position
            switch (transition) {
                case 'left':
                    cssStart.top = position[0];
                    cssStart.left = sliderWidth;
                    break;
                case 'bottomLeft':
                    cssStart.top = sliderHeight;
                    cssStart.left = sliderWidth;
                    break;
                case 'topLeft':
                    cssStart.top = obj.outerHeight() * -1;
                    cssStart.left = sliderWidth;
                    break;
                case 'top':
                    cssStart.top = obj.outerHeight() * -1;
                    cssStart.left = position[1];
                    break;
                case 'bottom':
                    cssStart.top = sliderHeight;
                    cssStart.left = position[1];
                    break;
                case 'right':
                    cssStart.top = position[0];
                    cssStart.left = -offsetX - obj.outerWidth();
                    break;
                case 'bottomRight':
                    cssStart.top = sliderHeight;
                    cssStart.left = -offsetX - obj.outerWidth();
                    break;
                case 'topRight':
                    cssStart.top = obj.outerHeight() * -1;
                    cssStart.left = -offsetX - obj.outerWidth();
                    break;
                case 'fade':
                    cssStart.top = position[0];
                    cssStart.left = position[1];
                    cssStart.opacity = 0;
                    cssEnd.opacity = 1;
                    break;
                case 'none':
                    cssStart.top = position[0];
                    cssStart.left = position[1];
                    cssStart.display = 'none';
                    speed = 0;
                    break;
            }

            // set target position
            cssEnd.top = position[0];
            cssEnd.left = position[1];

            // sets the right unit
            cssEnd.left = cssEnd.left + unit;
            cssEnd.top = cssEnd.top + unit;
            cssStart.left = cssStart.left + unit;
            cssStart.top = cssStart.top + unit;

            // set the delay
            timeouts.push(setTimeout(function () {
                // for special=cylce
                if (special == 'cycle' && obj.attr('rel') == vars.currentSlide) {
                    var tmp = obj.prev();
                    if (tmp.length > 0) {
                        var tmpPosition = $(tmp).attr('data-position').split(',');
                        tmpPosition = {
                            'top': tmpPosition[0],
                            'left': tmpPosition[1]
                        };
                        var tmpTransition = $(tmp).attr('data-out');
                        if (tmpTransition == null) {
                            tmpTransition = options.transitionOut;
                        }
                        objectAnimationOut(tmp, tmpPosition, tmpTransition, speed);
                    }
                }

                // animate
                obj.css(cssStart).show().animate(cssEnd, speed, easing, function () {
                    objFinished(obj);
                }).addClass('fs_obj_active');
            }, delay));
        }

        /** OUT TRANSITION OBJECTS **/
        function objectAnimationOut(obj, position, transition, speed, easing) {
            var cssStart = {}, cssEnd = {}, unit = null;

            speed = options.speedOut;

            if (options.responsive) {
                unit = '%';
            } else {
                unit = 'px';
            }

            var w = obj.outerWidth(), h = obj.outerHeight();

            if (options.responsive) {
                w = pixelToPercent(w, dX);
                h = pixelToPercent(h, dY);
            }

            // set target position
            switch (transition) {
                case 'left':
                    cssEnd.left = -offsetX - 100 - w;
                    break;
                case 'bottomLeft':
                    cssEnd.top = sliderHeight;
                    cssEnd.left = -offsetX - 100 - w;
                    break;
                case 'topLeft':
                    cssEnd.top = -h;
                    cssEnd.left = -offsetX - 100 - w;
                    break;
                case 'top':
                    cssEnd.top = -h;
                    break;
                case 'bottom':
                    cssEnd.top = sliderHeight;
                    break;
                case 'right':
                    cssEnd.left = sliderWidth;
                    break;
                case 'bottomRight':
                    cssEnd.top = sliderHeight;
                    cssEnd.left = sliderWidth;
                    break;
                case 'topRight':
                    cssEnd.top = -h;
                    cssEnd.left = sliderWidth;
                    break;
                case 'fade':
                    cssStart.opacity = 1;
                    cssEnd.opacity = 0;
                    break;
                case 'hide':
                    cssEnd.display = 'none';
                    speed = 0;
                    break;
                default:
                    cssEnd.display = 'none';
                    speed = 0;
                    break;
            }

            if (typeof cssEnd.top != 'undefined') {
                // substracts the px
                if (cssEnd.top.toString().indexOf('px') > 0) {
                    cssEnd.top = cssEnd.top.substring(0, cssEnd.top.length - 2);
                    if (options.responsive) {
                        cssEnd.top = pixelToPercent(cssEnd.top, dY);
                    }
                }
            }

            if (typeof cssEnd.left != 'undefined') {
                if (cssEnd.left.toString().indexOf('px') > 0) {
                    cssEnd.left = cssEnd.left.substring(0, cssEnd.left.length - 2);
                    if (options.responsive) {
                        cssEnd.left = pixelToPercent(cssEnd.left, dX);
                    }
                }
            }

            // sets the right unit
            cssEnd.left = cssEnd.left + unit;
            cssEnd.top = cssEnd.top + unit;

            // animation
            obj.css(cssStart).animate(cssEnd, speed, easing, function () {
                obj.hide();
            }).removeClass('fs_obj_active');
        }

        function backgroundAnimation() {
            var el;
            if (options.backgroundElement == null || options.backgroundElement == "") {
                el = slider.parent();
            } else {
                el = $(options.backgroundElement);
            }

            var oldPos = el.css('background-position');
            oldPos = oldPos.split(' ');
            var moveX = options.backgroundX, moveY = options.backgroundY, x = Number(oldPos[0].replace(/[px,%]/g, '')) + Number(moveX), y = Number(oldPos[1].replace(/[px,%]/g, '')) + Number(moveY);

            el.animate({
                backgroundPositionX: x + 'px',
                backgroundPositionY: y + 'px'
            }, options.backgroundSpeed, options.backgroundEase);
        }

        /** ************************* **/
        /** RESPONSIVE **/
        /** ************************* **/

        function makeResponsive() {
            var d = options.dimensions.split(','),
                ie = msie();

            dX = d['0'];
            dY = d['1'];

            if (!options.increase) {
                $(element).css({ "maxWidth": dX + "px" });
            }

            var objs = slider.children('.slide').find('*');

            objs.each(function () {
                var obj = $(this), x = null, y = null, value = null;

                // calculate % position
                if (obj.attr("data-position") != null) {
                    var position = obj.attr("data-position").split(',');

                    x = pixelToPercent(position[1], dX);
                    y = pixelToPercent(position[0], dY);
                    obj.attr("data-position", y + ',' + x);
                }

                // calculate % width
                if (obj.attr("width") != null && obj.attr("width") != "") {
                    value = obj.attr("width");

                    x = pixelToPercent(value, dX);
                    obj.attr("width", x + "%");
                    obj.css("width", x + "%");
                } else if (obj.css('width') != '0px') {
                    value = obj.css("width");
                    if (value.indexOf('px') > 0) {
                        value = value.substring(0, value.length - 2);
                        x = pixelToPercent(value, dX);
                        obj.css("width", x + "%");
                    }
                } else if (obj.prop("tagName").toLowerCase() == 'img' && ie != -1) {
                    value = getWidth(obj);
                    x = pixelToPercent(value, dX);
                    obj.css("width", x + "%").attr('width', x + '%');
                }
                else if (obj.prop("tagName").toLowerCase() == 'img') {
                    value = obj.get(0).width;
                    x = pixelToPercent(value, dX);
                    obj.css("width", x + "%");
                }



                // calculate % height
                if (obj.attr("height") != null && obj.attr("height") != "") {
                    value = obj.attr("height");

                    y = pixelToPercent(value, dY);
                    obj.attr("height", y + "%");
                    obj.css("height", y + "%");
                } else if (obj.css('height') != '0px') {
                    value = obj.css("height");
                    if (value.indexOf('px') > 0) {
                        value = value.substring(0, value.length - 2);
                        y = pixelToPercent(value, dY);
                        obj.css("height", y + "%");
                    }
                } else if (obj.prop("tagName").toLowerCase() == 'img' && ie != -1) {
                    value = getHeight(obj);
                    y = pixelToPercent(value, dY);
                    obj.css("height", y + "%").attr('height', y + '%');
                } else if (obj.prop("tagName").toLowerCase() == 'img') {
                    value = obj.get(0).height;
                    y = pixelToPercent(value, dY);
                    obj.css("height", y + "%");
                }

                obj.attr('data-fontsize', obj.css('font-size'));

            });

            slider.css({
                'width': 'auto',
                'height': 'auto'
            }).append('<div class="fs-stretcher" style="width:' + dX + 'px; height:' + dY + 'px"></div>');

            resizeSlider();

            $(window).bind('resize', function () {
                resizeSlider();
            });

        }

        function getWidth(element) {
            var img = new Image();
            img.src = element.attr('src');
            return img.width;
        }
        function getHeight(element) {
            var img = new Image();
            img.src = element.attr('src');
            return img.height;
        }

        function resizeSlider() {
            var w = slider.innerWidth(), h = slider.innerHeight();
            if (w <= dX || options.increase) {
                var xy = dX / dY, nH = w / xy;
                slider.find('.fs-stretcher').css({
                    'width': w + 'px',
                    'height': nH + "px"
                });
            }

            // Update bodyWidth
            bodyWidth = $('body').width();

            // calculate the width/height/offsetX of the slider
            var sW = slider.width();
            offsetX = pixelToPercent(((bodyWidth - sW) / 2), dX);
            sliderWidth = 100;
            if (options.fullWidth) {
                sliderWidth = 100 + offsetX * 2;
            }
            sliderHeight = 100;

            if (vars.init == false || w < dX) {
                resizeFontSize();
            }
        }

        function resizeFontSize() {
            var value = null, n = null, objs = slider.children('.slide').find('*');

            objs.each(function () {
                obj = $(this);

                var value = obj.attr('data-fontsize');

                if (value.indexOf('px') > 0) {
                    value = value.substring(0, value.length - 2);
                    n = pixelToPercent(value, dY) * (slider.find('.fs-stretcher').height() / 100);
                    obj.css("fontSize", n + "px");
                    obj.css("lineHeight", '100%');
                }

            });
        }

        function pixelToPercent(value, d) {
            return value / (d / 100);
        }

        /** ************************* **/
        /** HELPER **/
        /** ************************* **/

        function stopTimeout(timeout) {
            clearTimeout(timeout);
        }

        function stopTimeouts(timeouts) {
            var length = timeouts.length;
            $.each(timeouts, function (index) {
                clearTimeout(this);
                if (index == length - 1) {
                    timeouts = [];
                }
            });
        }

        function msie()
        // Returns the version of Internet Explorer or a -1
        // (indicating the use of another browser).
        {
            var rv = -1; // Return value assumes failure.
            if (navigator.appName == 'Microsoft Internet Explorer') {
                var ua = navigator.userAgent;
                var re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
                if (re.exec(ua) != null)
                    rv = parseFloat(RegExp.$1);
            }
            return rv;
        }

    };

    /** ************************* **/
    /** PLUGIN  **/
    /** ************************* **/

    $.fn.fractionSlider = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method == 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.tooltip');
        }

    };

    /** ************************* **/
    /** EASING EXTEND  **/
    /** ************************* **/

    // based on jqueryui (http://jqueryui.com/)
    // based on easing equations from Robert Penner (http://www.robertpenner.com/easing)

    var baseEasings = {};

    $.each(["Quad", "Cubic", "Quart", "Quint", "Expo"], function (i, name) {
        baseEasings[name] = function (p) {
            return Math.pow(p, i + 2);
        };
    });

    $.extend(baseEasings, {
        Sine: function (p) {
            return 1 - Math.cos(p * Math.PI / 2);
        },
        Circ: function (p) {
            return 1 - Math.sqrt(1 - p * p);
        },
        Elastic: function (p) {
            return p == 0 || p == 1 ? p : -Math.pow(2, 8 * (p - 1)) * Math.sin(((p - 1) * 80 - 7.5) * Math.PI / 15);
        },
        Back: function (p) {
            return p * p * (3 * p - 2);
        },
        Bounce: function (p) {
            var pow2, bounce = 4;

            while (p < ((pow2 = Math.pow(2, --bounce)) - 1) / 11) {
            }
            return 1 / Math.pow(4, 3 - bounce) - 7.5625 * Math.pow((pow2 * 3 - 2) / 22 - p, 2);
        }
    });

    $.each(baseEasings, function (name, easeIn) {
        $.easing["easeIn" + name] = easeIn;
        $.easing["easeOut" + name] = function (p) {
            return 1 - easeIn(1 - p);
        };
        $.easing["easeInOut" + name] = function (p) {
            return p < 0.5 ? easeIn(p * 2) / 2 : 1 - easeIn(p * -2 + 2) / 2;
        };
    });
    // end easing
}(jQuery));

$(document).ready(function () {
    var em = $("#eml").val();
    if ($("#eml").length) {
        if (em !== " ") {
            Swal.fire({
                title: "با تشکر",
                text: em,
                icon: "success",
                showConfirmButton: false,
                timer: 2000
            });
        }
    }

});
