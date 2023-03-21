$(function () {

    if ($('.other-seller-table:first  tbody tr').length == 1) {
        $('#other-sellers-box, #other-sellers-count-box').addClass('d-none');

    }

    $('#show-all-product-features').click(function () {
        $(this).addClass("d-none");
        $('#features-next-to-product-box li').removeClass('d-none');
    });
    // Add border to the first variant
    $('#product-variants-box-in-show-product-info div:first').addClass('selected-variant-in-show-product-info');

    // Add check to the first variant
    $('#product-variants-box-in-show-product-info i:first').removeClass('d-none');

    $('#product-variants-box-in-show-product-info div').click(function () {
        $('#product-variants-box-in-show-product-info div').removeClass('selected-variant-in-show-product-info');
        $('#product-variants-box-in-show-product-info i').addClass('d-none');

        $(this).find('i').removeClass('d-none');
        $(this).addClass('selected-variant-in-show-product-info');

        var selectedVariantValue = $(this).attr('data-bs-original-title');

        $('.other-sellers-table').addClass('d-none');

        $('.other-sellers-table[variant-value="' + selectedVariantValue + '"]').removeClass('d-none');

     

        //change variant value
        $('#product-variant-value').html(selectedVariantValue);

        //change product info in left side box

        //change product name
        var selectedSeller = $('.other-sellers-table[variant-value="' + selectedVariantValue + '"] tbody tr:first');
        var selectedShopName = selectedSeller.find('td:first').text();
        $('#shop-details-in-single-page-of-product div').html(selectedShopName);

        // Change product logo
        var selectedShopLogo = selectedSeller.find('td:first i').length === 0 ? 'img' : 'i';
        if (selectedShopLogo === 'img') {
            selectedShopLogo = selectedSeller.find('td:first img').attr('src');
            $('#shop-details-in-single-page-of-product i').addClass('d-none');
            $('#shop-details-in-single-page-of-product img').removeClass('d-none');
            $('#shop-details-in-single-page-of-product img').attr('src', selectedShopLogo);
        }
        else {
            $('#shop-details-in-single-page-of-product i').removeClass('d-none');
            $('#shop-details-in-single-page-of-product img').addClass('d-none');
        }

        // Change product guarantee
        var selectedGuarantee = selectedSeller.find('td:nth-child(2)').text();
        $('#product-guarantee-in-single-page-of-product').html(selectedGuarantee);

        // Change product price
        var selectedprice = selectedSeller.find('td:nth-child(3)').text();
        $('#product-price-in-single-page-of-product').html(selectedprice);

        // Change product price
        var selectedscore = selectedSeller.find('td:nth-child(4) span').text();
        $('#product-score-in-single-page-of-product span').html(selectedscore);
       
        // Hide other sellers box if it has just one item
        if ($('.other-sellers-table[variant-value="' + selectedVariantValue + '"] tbody tr').length === 1) {
            $('#other-sellers-box, #other-sellers-count-box').addClass('d-none');
        } else {
            $('#other-sellers-box, #other-sellers-count-box').removeClass('d-none');
        }

        //show or hide free delivery box
        if (selectedSeller.attr('free-delivery') === 'true') {
            $('#free-delivery-box').removeClass('d-none');
        } else {
            $('#free-delivery-box').addClass('d-none');
        }
    });
});