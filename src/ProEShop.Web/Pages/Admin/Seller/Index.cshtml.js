fillDataTable();
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