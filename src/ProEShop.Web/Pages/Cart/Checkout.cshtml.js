$(function () {
   
    $('#shipping-dropdown').hover(
        function () {
            $('.dropdown-menu').addClass('show');
        },
        function () {
            $('.dropdown-menu').removeClass('show');
        }
    );

});