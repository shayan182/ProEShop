function addProductVariantToCart(message, data) {
    var addProductVariantToCartEl = $('.add-product-variant-to-cart[variant-id="' + data.productVariantId + '"]');
    addProductVariantToCartEl.addClass('d-none');
    var cartSectionEl = $('.product-variant-in-cart-section[variant-id="' + data.productVariantId + '"]');
    cartSectionEl.removeClass('d-none');
    cartSectionEl.find('.product-variant-count-in-cart').html(data.count.toString().toPersinaDigit());

    if (data.count === 1) {
        cartSectionEl.find('.decreaseProductVariantInCartButton').parents('span').addClass('d-none');
        cartSectionEl.find('.empty-variants-in-cart').parents('span').removeClass('d-none');
    } else if (data.count === 0) {
        // اگر تعدا صفر بود بخش افزودن به سبد خرید رو دوباره نشون میدیم
        // و بخش سبد خرید رو مخفی میکنیم
        cartSectionEl.addClass('d-none');
        addProductVariantToCartEl.removeClass('d-none');
    } else {
        // اگر تعداد بیشتر از یک بود در اون صورت علامت آیکون
        // Trash
        // رو مخفی میکنیم و علامت منفی رو نشون میدیم
        cartSectionEl.find('.decreaseProductVariantInCartButton').parents('span').removeClass('d-none');
        cartSectionEl.find('.empty-variants-in-cart').parents('span').addClass('d-none');
    }
}

function copyProductLinkToClipboardFunction() {
    var copyButtonSelector = $('#copy-product-link-button');
    var copyButtonHtml = copyButtonSelector.html();
    copyButtonSelector.html('<i class="bi bi-clipboard-check rem20px"></i> کپی شد');
    setInterval(function () {
        copyButtonSelector.html(copyButtonHtml);
    }, 2000);
}

$(function () {
    $('.increaseProductVariantInCartButton, .decreaseProductVariantInCartButton, .empty-variants-in-cart').click(function () {
        $(this).parent().submit();
    });

    $('.count-down-timer-in-other-variants').each(function () {
        var currentEl = $(this);
        var selectorToShow = currentEl.parents('td').find('div:first');
        var selectorToHide = currentEl.parents('td').find('div:eq(1)');
        countDownTimer(currentEl, selectorToShow, selectorToHide);
    });
    $('.count-down-timer').each(function () {
        var currentEl = $(this);
        var variantValue = currentEl.parent().attr('variant-value');
        var selectorToShow = $('.product-price-in-single-page-of-product[variant-value="' + variantValue + '"]');
        var selectorToHide = currentEl.parent();
        var selectorToHide2 = $('.product-final-price-in-single-page-of-product[variant-value="' + variantValue + '"]');
        countDownTimer(currentEl, selectorToShow, selectorToHide, selectorToHide2); 
    });
    function countDownTimerFunction(selector, selectorToShow, selectorToHide, selectorToHide2, countDownDate) {
        // Get today's date and time
        var now = new Date().getTime();

        // Find the distance between now and the count down date
        var distance = countDownDate - now;

        // Time calculations for days, hours, minutes and seconds
        var days = Math.floor(distance / (1000 * 60 * 60 * 24));
        var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        var seconds = Math.floor((distance % (1000 * 60)) / 1000);

        var daysText = `${days} روز و<br />`;
        if (days === 0) {
            daysText = '';
        }

        var result =
            `${daysText}${seconds < 10 ? '0' + seconds : seconds} : ${minutes < 10 ? '0' + minutes : minutes} : ${hours < 10 ? '0' + hours : hours}`;

        selector.html(result.toPersinaDigit());

        // If the count down is finished, write some text
        if (distance < 0) {
            selector.parents('tr').attr('is-discount-active', 'false');
            selectorToShow.removeClass('d-none');
            selectorToHide.addClass('d-none');
            if (selectorToHide2) {
                selectorToHide2.addClass('d-none');
            }
        }
        return distance;
    }

    function countDownTimer(selector, selectorToShow, selectorToHide, selectorToHide2) {

        var endDateTime = selector.html().trim();

        // Set the date we're counting down to
        var countDownDate = new Date(endDateTime).getTime();

        countDownTimerFunction(selector, selectorToShow, selectorToHide, selectorToHide2, countDownDate);

        // Update the count down every 1 second
        var x = setInterval(function () {
            var result = countDownTimerFunction(selector, selectorToShow, selectorToHide, selectorToHide2, countDownDate);
            if (result < 0) {
                clearInterval(x);
            }
        }, 1000);
   
    }
    
    //CountDownEnd


    $('#other-sellers-count-box').click(function () {
        $('html, body').animate({
            scrollTop: $('#other-sellers-box').offset().top - 20
        }, 1);
    });

    $('#copy-product-link-button').click(function () {
        var productLink = $(this).attr('product-link');
        copyTextToClipboard(productLink, 'copyProductLinkToClipboardFunction');
    });

    var zoomPluginOptions = {
        fillContainer: true,
        zoomPosition: 'original'
    };
    new ImageZoom(document.getElementById('zoom-image-place'), zoomPluginOptions);

    $('#add-product-to-favorite-form').submit(function () {
        if (!isAuthenticated) {
            showFirstLoginModal();
            return false;
        }
    });

    //start custom code (this code is in the main-script.js file but this file can not find these codes)
    const firstLoginModalBody = `<div class="modal" id="first-login-modal">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-body text-center">
                <h5>
                    لطفا ابتدا وارد وبسایت شوید
                </h5>
                <a class="btn btn-secondary">ورود</a>
                <button type="button" class="btn btn-danger" data-bs-dismiss="modal">بستن</button>
            </div>
        </div>
    </div>
    </div>`;
    function showFirstLoginModal() {
        if ($('#first-login-modal').length === 0) {
            $('body').append(firstLoginModalBody);
            $('#first-login-modal a').attr('href', loginPageUrl);
        }
        $('#first-login-modal').modal('show');
    }

    //end custom code
    $('#share-product-button').click(function () {
        $('#share-product-modal').modal('show');
    });

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

    //change variants (color)
    $('#product-variants-box-in-show-product-info div').click(function () {
        if ($(this).find('i').hasClass('d-none') === false) {
            return;
        }

        $('#product-variants-box-in-show-product-info div').removeClass('selected-variant-in-show-product-info');
        $('#product-variants-box-in-show-product-info i').addClass('d-none');

        $(this).find('i').removeClass('d-none');
        $(this).addClass('selected-variant-in-show-product-info');

        var selectedVariantValue = $(this).attr('data-bs-original-title'); //aria-label
        var selectedProductVariantId = $(this).attr('product-variant-id');
        changeVariant(selectedVariantValue, selectedProductVariantId);
    });

    //change variants (size)
    $('#product-variants-box-in-show-product-info select').change(function () {

        var selectedVariantValue = this.value;
        var selectedProductVariantId = $(this).find(':selected').attr('product-variant-id');

        changeVariant(selectedVariantValue, selectedProductVariantId);
    });
    function changeVariant(selectedVariantValue, selectedProductVariantId) {

        $('.other-sellers-table').addClass('d-none');
        $('.product-final-price-in-single-page-of-product').addClass('d-none');
        $('.product-price-in-single-page-of-product').addClass('d-none');
        $('.discount-count-down-box').addClass('d-none');

        $('.other-sellers-table[variant-value="' + selectedVariantValue + '"]').removeClass('d-none');



        //change variant value
        $('#product-variant-value').html(selectedVariantValue);

        //change product info in left side box

        //change product name
        var selectedSeller = $('.other-sellers-table[variant-value="' + selectedVariantValue + '"] tbody tr:first');

        if (selectedSeller.attr('is-discount-active') === 'true') {
            $('.product-final-price-in-single-page-of-product[variant-value="' + selectedVariantValue + '"]').removeClass('d-none');
            $('.discount-count-down-box[variant-value="' + selectedVariantValue + '"]').removeClass('d-none');
        } else {
            $('.product-price-in-single-page-of-product[variant-value="' + selectedVariantValue + '"]').removeClass('d-none');

        }
        $('.latest-product-stock-in-inventory').addClass('d-none');
        $('.latest-product-stock-in-inventory[variant-value=' + selectedVariantValue + ']').removeClass('d-none');


        var selectedShopName = selectedSeller.find('td:first').text();
        $('#shop-details-in-single-page-of-product div').html(selectedShopName);

        // Change tooltip value
        // Shop name tooltip
        var tooltip = bootstrap.Tooltip.getInstance('#product-shop-name-tooltip');
        //tooltip.setContent({ '.tooltip-inner': `این کالا توسط فروشنده آن، ${selectedShopName.trim()}، قیمت گذاری شده است.` }); // main code
        document.getElementById("product-shop-name-tooltip").setAttribute("data-bs-original-title", `این کالا توسط فروشنده آن، ${selectedShopName.trim()}، قیمت گذاری شده است.`); // custom code


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

        //// we dont need this code
        //// Change product price
        //var selectedprice = selectedSeller.find('td:nth-child(3)').text();
        //$('#product-price-in-single-page-of-product').html(selectedprice);

        // Change product price
        var selectedscore = selectedSeller.find('td:nth-child(4) span').text();
        $('#product-score-in-single-page-of-product span').html(selectedscore);

        // Hide other sellers box if it has just one item
        var otherSellersCount = $('.other-sellers-table[variant-value="' + selectedVariantValue + '"] tbody tr').length;
        if (otherSellersCount === 1) {
            $('#other-sellers-box, #other-sellers-count-box').addClass('d-none');
        } else {
            $('#other-sellers-box, #other-sellers-count-box').removeClass('d-none');
        }

        //change other sellers count
        $('#other-sellers-count-box span').html((otherSellersCount - 1).toString().toPersinaDigit());
        //show or hide free delivery box
        if (selectedSeller.attr('free-delivery') === 'true') {
            $('#free-delivery-box').removeClass('d-none');
        } else {
            $('#free-delivery-box').addClass('d-none');
        }
        debugger
        $('.product-variant-in-cart-section').addClass('d-none');
        var cartSectionEl = $('.product-variant-in-cart-section[variant-id="' + selectedProductVariantId + '"]');
        if (cartSectionEl.find('.product-variant-count-in-cart').text() !== '۰') {
            cartSectionEl.removeClass('d-none');
        }

        // Change add product to cart button
        $('.add-product-variant-to-cart').addClass('d-none');
        if (cartSectionEl.find('.product-variant-count-in-cart').text() === '۰') {
            $('.add-product-variant-to-cart[variant-id="' + selectedProductVariantId + '"]').removeClass('d-none');
        }
    }
});



function addFavoriteFunction() {
    var addFavoriteButton = $('#addFavoriteButton').parent().find('input[name="addFavorite"]');
    if (addFavoriteButton.val() === 'true') {
        addFavoriteButton.val('false');
        $('#addFavoriteButton i:first').addClass('d-none');
        $('#addFavoriteButton i:last').removeClass('d-none');
    }
    else {
        addFavoriteButton.val('true');
        $('#addFavoriteButton i:first').removeClass('d-none');
        $('#addFavoriteButton i:last').addClass('d-none');
    }
}