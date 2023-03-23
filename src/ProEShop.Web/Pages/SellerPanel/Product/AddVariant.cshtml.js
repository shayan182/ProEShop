$(function () {
    var variantTitle = $('#variant-box label span').html();
    $('.variant-item-button').click(function () {
        var selectedVariantId = $(this).attr('variant-id');
        $('#Variant_VariantId').val(selectedVariantId);
        $('.public-ajax-form').validate().element('#Variant_VariantId');
        var selectedButtonText = $(this).html();
        $('#variant-box label span').html(variantTitle + selectedButtonText);
    });

    ////ajax in select2
    //$('.custom-select2').select2({
    //    theme: 'bootstrap-5',
    //    ajax: {
    //        url: location.pathname + '?handler=GetGuarantees',
    //        // After 250 seconds of stopping typing, it sends the request
    //        delay: 250,
    //        //we wanna change the name of the term (user typed data) to input
    //        //so you now can use input variable as the method argument
    //        data: function (params) {
    //            return {
    //                input: params.term
    //            };
    //        },
    //        cache: true
    //    },
    //    placeholder: 'انتخاب کنید',
    //    minimumInputLength: 2
    //});

    $('.custom-select2').select2({
        theme: 'bootstrap-5',
        ajax: {
            url: location.pathname + '?handler=GetGuarantees',
            delay: 250,
            data: function (params) {
                return {
                    input: params.term
                };
            },
            cache: true
        },
        placeholder: 'انتخاب کنید',
        minimumInputLength: 2
    });
});
function addProductVariantFunction(message, data) {
    showToastr('success', message);

    // data = Url to Redirect
    location.href = data;
}