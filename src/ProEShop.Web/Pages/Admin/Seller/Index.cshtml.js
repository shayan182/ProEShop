fillDataTable();
appendHtmlModalPlaceToBody();

$('#Sellers_SearchSellers_ProvinceId').change(function () {
    var formData = {
        provinceId: $(this).val()
    }
    getDataWithAjax("/Admin/Seller?handler=GetCities", formData, 'putCitiesInTheSelectBox');

});

function putCitiesInTheSelectBox(data, message) {
    $('#Sellers_SearchSellers_CityId option').remove();
    $('#Sellers_SearchSellers_CityId').append('<option value="0">انتخاب کنید</optoin>');
    $.each(data, function (key, value) {
        $('#Sellers_SearchSellers_CityId').append(`<option value="${key}">${value}</optoin>`);
    });
}


function getSellerDetails(e) {
    var sellerId = $(e).attr('seller-id');
    getHtmlWithAJAX('?handler=GetSellerDetails', { sellerId: sellerId }, 'showSellerDetailsInModal', e);
}

function showSellerDetailsInModal(result, clickedButton) {
    var currnetModal = $('#html-modal-place');
    currnetModal.find('.modal-body').html(result);
    currnetModal.modal('show');
    $('#html-modal-place .modal-header h5').html($(clickedButton).text().trim());
    initializeTinyMCE();
} 

function rejectReasonInManagingSellers(message) {
    showToastr('success', message);
    $('#html-modal-place').modal('hide');
    fillDataTable();
}