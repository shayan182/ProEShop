function addProductVariantToCart(message, data) {
    $('#cart-body').html(data.cartBody);

    $('#cart-body .persian-numbers').each(function () {
        var text = $(this).html();
        $(this).html(text.toPersinaDigit());
    });

    var allProductsCountInCart = $('#cart-page-title span').html();
   
    if (allProductsCountInCart) {
        $('#cart-count-text').html(allProductsCountInCart);
    } else {
        $('#cart-count-text').html('۰');
    }
}

$(function () {

    
    $(document).on('click', '.increaseProductVariantInCartButton, .decreaseProductVariantInCartButton, .empty-variants-in-cart',
        function () {
            if ($(this).parents('span').hasClass('text-custom-grey')) {
                return;
            }
            $(this).parent().submit();
        });
});