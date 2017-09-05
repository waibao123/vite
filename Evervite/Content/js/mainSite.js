﻿$(function () {

    var snapList = $("#snapList");
    snapList.owlCarousel({
        items: 4, //10 items above 1000px browser width
        itemsDesktop: [840, 4], //5 items between 1000px and 901px
        itemsDesktopSmall: [740, 3], // betweem 900px and 601px
        itemsTablet: [600, 2], //2 items between 600 and 0
        itemsMobile: false,
        autoPlay: false,
        lazyLoad: true,
        pagination: false,
        navigation: true,
        navigationText: ["<", ">"]
    });


    $('.carousel').carousel({
        interval: 5000
    });

    $('.shortcut-button-list .btn').on("click", function (e) {
        $('.shortcut-button-list .btn').removeClass("btn-primary").addClass("btn-default");
        $(e.currentTarget).removeClass("btn-default").addClass("btn-primary");
    });




});