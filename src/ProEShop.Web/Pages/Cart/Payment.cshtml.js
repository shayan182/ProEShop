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
function createOrderAndPayFunction(message, data) {
    console.log(message);
    console.log(data);
}