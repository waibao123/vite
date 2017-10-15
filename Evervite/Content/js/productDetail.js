$(function () {

    var snapList = $("#snapList");
    snapList.owlCarousel({
        items: 5, //10 items above 1000px browser width
        itemsDesktop: [840, 5], //5 items between 1000px and 901px
        itemsDesktopSmall: [740, 4], // betweem 900px and 601px
        itemsTablet: [600, 2], //2 items between 600 and 0
        itemsMobile: false,
        autoPlay: false,
        lazyLoad: true,
        pagination: false,
        navigation: true,
        navigationText: ["<", ">"]
    });

    $(".recommend-product").on("click", function () {
        $(".thumb-pic").attr("src", $(this).attr("src"));
    });

});