$(function () {
    fillDataTable();
    $('#FeatureConstantValues_SearchFeatureConstantValues_CategoryId').change(function () {
        var selectedCategoryId = $(this).val();
        getDataWithAjax(`${location.pathname}?handler=GetCategoryFeatures`, { categoryId: selectedCategoryId }, 'fillFeaturesInSelectbox');
    });
    $(document).on('change', '#CategoryId', function () {
        var selectedCategoryId = $(this).val();
        getDataWithAjax(`${location.pathname}?handler=GetCategoryFeatures`, { categoryId: selectedCategoryId }, 'fillFeaturesInModalSelectbox');
    });
});
function fillFeaturesInSelectbox(data, message) {
    $('#FeatureConstantValues_SearchFeatureConstantValues_FeatureId').html('<option value="0">انتخاب کنید</option>');
    data.forEach(function (item) {
        var optionToAdd = `<option value="${item.featureId}">${item.featureTitle}</option>`;
        $('#FeatureConstantValues_SearchFeatureConstantValues_FeatureId').append(optionToAdd);
    });
}
function fillFeaturesInModalSelectbox(data, message) {
    $('#FeatureId').html('<option value="0">انتخاب کنید</option>');
    data.forEach(function (item) {
        var optionToAdd = `<option value="${item.featureId}">${item.featureTitle}</option>`;
        $('#FeatureId').append(optionToAdd);
    });
}
