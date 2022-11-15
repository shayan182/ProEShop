$(function () {
    fillDataTable();
    initializingAutocomplete();
});
function editBrandFunction() {
    var isIranianBrandChecked = $('#IsColor').is(':checked');
    $('#IsColor').parents('.form-switch').find('label').html(isIranianBrandChecked ? 'رنگ' : 'سایز');
    $('#IsColor').change(function () {
        var textToReplace = this.checked ? 'رنگ' : 'سایز';
        $(this).parents('.form-switch').find('label').html(textToReplace);
        if (this.checked) {
            $('#ColorCode').removeAttr('disabled');
        } else {
            $('#ColorCode').attr("disabled", "disabled");
        }
    });
}

