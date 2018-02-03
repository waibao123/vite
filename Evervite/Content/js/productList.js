﻿$(function () {

    var productListCarousel = $("#product_recommend_list_carousel");
    productListCarousel.owlCarousel({
        items: 1, //10 items above 1000px browser width
        itemsDesktop: [840, 1], //5 items between 1000px and 901px
        itemsDesktopSmall: [740, 1], // betweem 900px and 601px
        itemsTablet: [600, 1], //2 items between 600 and 0
        itemsMobile: false,
        autoPlay: true,
        lazyLoad: true,
        pagination: true,
        navigation: false,
        navigationText: ["<", ">"]
    });

});