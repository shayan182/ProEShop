$('#legal-person-checkbox-create-seller').change(function () {
    var labelEl = $(this).parents('.form-switch').find('label');
    if (this.checked) {
        addRequiredRule('#CreateSeller_CompanyName');
        addRequiredRule('#CreateSeller_RegisterNumber');
        addRequiredRule('#CreateSeller_EconomicCode');
        addRequiredRule('#CreateSeller_SignatureOwners');
        addRequiredRule('#CreateSeller_NationalId');
        labelEl.html('شخص حقوقی');
    }
    else {
        removeRequiredRule('#CreateSeller_CompanyName');
        removeRequiredRule('#CreateSeller_RegisterNumber');
        removeRequiredRule('#CreateSeller_EconomicCode');
        removeRequiredRule('#CreateSeller_SignatureOwners');
        removeRequiredRule('#CreateSeller_NationalId');
        labelEl.html('شخص حقیقی');
    }
    $(this).parents('form').valid();
    $('#legal-person-box-create-seller').slideToggle();
});
$('#legal-person-box-create-seller').hide(0);

// required attribute do not work for legal property 
function removeRequiredRule(selector) {
    $(selector).rules('remove', 'required');
}

function addRequiredRule(selector) {
    var displayName = $(selector).parent().find('label').html().trim();
    $(selector).rules('add',
        {
            required: true,
            messages: {
                required: `لطفا ${displayName} را وارد نمایید`
            }
        });
}

// Next and previous Button in Page
var firstTab = $('#create-seller-container .nav-tabs button:first').attr('data-bs-target');
var lastTab = $('#create-seller-container .nav-tabs button:last').attr('data-bs-target');

$('#create-seller-container #previous-tab-create-seller').attr('disabled', 'disabled');
var currentTab = $('#create-seller-container .nav-tabs button').attr('data-bs-target');

$('#create-seller-container #next-tab-create-seller').click(function () {
    var nextTab = $(`#create-seller-container .nav-tabs button[data-bs-target="${currentTab}"]`).next();

    if (nextTab.attr('data-bs-target')) {
        currentTab = nextTab.attr('data-bs-target');
        nextTab.tab('show');
    }
});

$('#create-seller-container #previous-tab-create-seller').click(function () {
    var preTab = $(`#create-seller-container .nav-tabs button[data-bs-target="${currentTab}"]`).prev();

    if (preTab.attr('data-bs-target')) {
        currentTab = preTab.attr('data-bs-target');
        preTab.tab('show');
    }
});

$('#create-seller-container .nav-tabs button').on('show.bs.tab', function (e) {
    currentTab = $(e.target).attr('data-bs-target');
    if (currentTab == lastTab) {
        $('#create-seller-container #next-tab-create-seller').attr('disabled', 'disabled');
    } else {
        $('#create-seller-container #next-tab-create-seller').removeAttr('disabled');
    }
    if (currentTab == firstTab) {
        $('#create-seller-container #previous-tab-create-seller').attr('disabled', 'disabled');
    }
    else {
        $('#create-seller-container #previous-tab-create-seller').removeAttr('disabled');
    }
});

// End Next and previous Button in Page

$('#CreateSeller_ProvinceId').change(function () {
    var formData = {
        provinceId: $(this).val()
}
    getDataWithAjax("/Seller/CreateSeller/test?handler=GetCities", formData, 'putCitiesInTheSelectBox');

});

function putCitiesInTheSelectBox(data,message) {
    $('#CreateSeller_CityId option').remove();
    $('#CreateSeller_CityId').append('<option value="0">انتخاب کنید</optoin>');
    $.each(data, function (key, value) {
        $('#CreateSeller_CityId').append(`<option value="${key}">${value}</optoin>`);
    });
}

