//__RequestVerificationToken
let rvt = '__RequestVerificationToken';

let htmlModalPlace = `<div class="modal fade" id="html-modal-place" data-bs-backdrop="static">
    <div class="modal-dialog modal-xl">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title"></h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
            </div>
            <div class="modal-body"></div>
            <div class="modal-footer d-flex justify-content-start">
                <button type="button" class="btn btn-danger" data-bs-dismiss="modal">بستن</button>
            </div>
        </div>
    </div>
</div>`;
var secondHtmlModalPlace = `<div class="modal fade" id="second-html-modal-place" data-bs-backdrop="static">
    <div class="modal-dialog modal-xl">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title"></h5>
                <button type="button" class="btn-close" data-bs-target="#html-modal-place" data-bs-toggle="modal" data-bs-dismiss="modal"></button>
            </div>
            <div class="modal-body"></div>
            <div class="modal-footer d-flex justify-content-start">
                <button type="button" class="btn btn-danger" data-bs-target="#html-modal-place" data-bs-toggle="modal" data-bs-dismiss="modal">بستن</button>
            </div>
        </div>
    </div>
</div>`;
function appendHtmlModalPlaceToBody() {
    if ($('#html-modal-place').length === 0) {
        $('body').append(htmlModalPlace);
    }
}

function appendSecondHtmlModalPlaceToBody() {
    if ($('#second-html-modal-place').length === 0) {
        $('body').append(secondHtmlModalPlace);
    }
}

let formModalPlace = `<div class="modal fade" id="form-modal-place" data-bs-backdrop="static">
    <div class="modal-dialog modal-xl">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title"></h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
            </div>
            <div class="modal-body"></div>
            <div class="modal-footer d-flex justify-content-start">
                <button type="button" class="btn btn-danger" data-bs-dismiss="modal">بستن</button>
            </div>
        </div>
    </div>
</div>`;

function appendFormModalPlaceToBody() {
    if ($('#form-modal-place').length === 0) {
        $('body').append(formModalPlace);
    }
}

let loadingModalHtml = `<div class="modal" id="loading-modal" data-bs-backdrop="static">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="exampleModalLabel">لطفا صبر کنید</h5>
            </div>
            <div class="modal-body text-center">
                <img src="/images/application/loading.gif" />
            </div>
        </div>
    </div>
</div>`;
function showLoading() {
    if ($('#loading-modal').length === 0) {
        $('body').append(loadingModalHtml);
    }
    $('#loading-modal').modal('show');
}
function hideLoading() {
    $('#loading-modal').modal('hide');
}
// Show sweet alert

function showSweetAlert2(text, functionToCallAfterConfirm, functionToCallAfterReject) {
    Swal.fire({
        title: 'اعلان',
        text: text,
        icon: 'warning',
        confirmButtonText: 'بله',
        showDenyButton: true,
        denyButtonText: 'خیر',
        confirmButtonColor: '#067719',
        allowOutsideClick: false
    }).then((result) => {
        if (result.isConfirmed) {
            window[functionToCallAfterConfirm]();
        } else {
            window[functionToCallAfterReject]();
        }
    });
}

// End show sweet alert

// Toastr

function showToastr(status, message) {
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-bottom-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
    toastr[status](message);
}

// End toastr

// Enable tooltips
function enablingTooltips() {
    let tooltipTriggerList = [].slice.call(document.querySelectorAll('.data-table-place[data-bs-toggle="tooltip"]'));
    //$('[data-toggle="tooltip"]').tooltip({
    //    trigger: 'hover'
    //});
    let tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl,
            {
                trigger:'hover'
                });
    });
}

function enablingNormalTooltips() {
    let tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    //$('[data-toggle="tooltip"]').tooltip({
    //    trigger: 'hover'
    //});
    let tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl,
            {
                trigger: 'hover'
            });
    });
}

function showErrorMessage(message) {
    showToastr('error', message != null ? message : 'خطایی به وجود آمد، لطفا مجددا تلاش نمایید');
}

//function initializeTinyMCE() {
//    tinymce.init({
//        selector: 'textarea.custom-tinymce',
//        height: 300,
//        max_height: 500,
//        language: 'fa_IR',
//        language_url: '/js/fa_IR.js',
//        content_style: 'body {font-family: Vazir}',
//        plugins: 'link table preview wordcount',
//        toolbar: 'link bold italic table preview'
//    });
//}

// Send `TinyMCE` images to server with specific url
function sendTinyMceImagesToServer(blobInfo, success, failure, progress, url) {
    let formData = new FormData();
    formData.append('file', blobInfo.blob(), blobInfo.filename());
    formData.append(rvt, $('textarea.custom-tinymce:first').parents('form').find('input[name="' + rvt + '"]').val());
    $.ajax({
        url: `${location.pathname}?handler=${url}`,
        data: formData,
        type: 'POST',
        enctype: 'multipart/form-data',
        dataType: 'json',
        processData: false,
        contentType: false,
        success: function (data) {
            if (data === false) {
                failure('خطایی به وجود آمد');
            } else {
                success(data.location);
            }
        },
        error: function () {
            failure('خطایی به وجود آمد');
        }
    });
};


function initializeTinyMCE() {
    $("textarea.custom-tinymce").each(function () {
        let textareaId = `#${$(this).attr('id')}`;
        tinymce.remove(textareaId);
        tinymce.init({
            selector: textareaId,
            setup: function (editor) {
                editor.on('blur', function (e) {
                    let elementId = $(e.target.targetElm).attr('id');
                    $(e.target.formElement).validate().element(`#${elementId}`);
                });
            },
            min_height: 300,
            max_height: 500,
            language: 'fa_IR',
            language_url: '/js/fa_IR.js',
            content_style: 'body {font-family: Vazir}',
            //images_upload_handler: uploadTinyMceImage, //Enable image with function to call handler
            image_title: true, // field alt for picture
            //plugins: ["link", "table", "preview", "wordcount", "media", "codesample", "emoticons", "insertdatetime", "a_tinymce_plugin", "advlist", "image", "textpattern", "template", "lists", "anchor", "print", "autolink", "noneditable", "pagebreak", "autosave", "code", "nonbreaking", "charmap"],
            plugins: ["image", "code", "table", "link", "media", "codesample", "autoresize"],
            //toolbar: 'link bold italic table preview ltr rtl a11ycheck addcommentContext showcommentContexts casechange  wordcount checklist  image export bullist formatpainter pagebreak charmap pageembed nonbreaking permanentpen table restoredraft numlist  table'
            toolbar: [
                {
                    name: 'history', items: ['undo', 'redo', 'preview']
                },
                {
                    name: 'styles', items: ['styleselect']
                },
                {
                    name: 'formatting', items: ['bold', 'italic', 'underline', 'link']
                },
                {
                    name: 'alignment', items: ['alignleft', 'aligncenter', 'alignright', 'alignjustify', 'forecolor', 'backcolor']
                },
                {
                    name: 'table', items: ['table', 'wordcount']
                },
                {
                    name: 'indentation', items: ['outdent', 'indent']
                }
            ],
            branding: false
        });
    });
    // for tool bar :  tabledelete | tableprops tablerowprops tablecellprops | tableinsertrowbefore tableinsertrowafter tabledeleterow | tableinsertcolbefore tableinsertcolafter tabledeletecol
}

//use this for solve type in search input  
document.addEventListener('focusin', function (e) {
    if (e.target.closest('.tox-tinymce-aux, .moxman-window, .tam-assetmanager-root') !== null) {
        e.stopImmediatePropagation();
    }
});

function initializeSelect2() {
    if ($('.modal .custom-select2').length > 0) {
        $('.modal .custom-select2').select2({
            theme: 'bootstrap-5',
            dropdownParent: $('#form-modal-place') // For Search  
        });
    }
}

function initializeSelect2WithoutModal() {
    if ($('.custom-select2').length > 0) {
        $('.custom-select2').select2({
            theme: 'bootstrap-5'
        });
    }
}

// Validation

// this is for IsImage
let imageInputsWithProblems = [];


function removeItemInArray(arr, item) {
    let found = arr.indexOf(item);

    while (found !== -1) {
        arr.splice(found, 1);
        found = arr.indexOf(item);
    }
}


if (jQuery.validator) {

    // Validate Hidden inputs in multiple pages (Create Seller) *************
    $.validator.setDefaults({
        ignore: []
    });

    let defaultRangeValidator = $.validator.methods.range;

    $.validator.methods.range = function (value, element, param) {
        if (element.type === 'checkbox') {
            return element.checked;
        } else {
            return defaultRangeValidator.call(this, value, element, param);
        }
    }

    // fileRequired
    jQuery.validator.addMethod("fileRequired", function (value, element, param) {
        let filesLength = element.files.length;
        if (filesLength > 0) {
            for (let i = 0; i < filesLength; i++) {
                if (element.files[0].size === 0) {
                    return false;
                }
            }
            return true;
        }
        return false;
    });
    jQuery.validator.unobtrusive.adapters.addBool("fileRequired");

    // allowExtensions
    jQuery.validator.addMethod('allowExtensions', function (value, element, param) {
        let selectedFiles = element.files;
        if (selectedFiles[0] === undefined) {
            return true;
        }
        let whiteListExtensions = $(element).data('val-whitelistextensions').split(',');
        for (let counter = 0; counter < selectedFiles.length; counter++) {
            let currentFile = selectedFiles[counter];
            if (currentFile != null) {
                if (!whiteListExtensions.includes(currentFile.type))
                    return false;
            }
        }
        return true;
    });
    jQuery.validator.unobtrusive.adapters.addBool('allowExtensions');

    // isImage
    jQuery.validator.addMethod('isImage', function (value, element, param) {
        let selectedFiles = element.files;
        if (selectedFiles[0] === undefined) {
            return true;
        }
        let whiteListExtensions = $(element).data('val-whitelistextensions').split(',');
        for (let counter = 0; counter < selectedFiles.length; counter++) {
            if (!whiteListExtensions.includes(selectedFiles[counter].type)) {
                return false;
            }
        }
        let currentElementId = $(element).attr('id');
        let currentForm = $(element).parents('form');

        if (imageInputsWithProblems.includes(currentElementId)) {
            removeItemInArray(imageInputsWithProblems, currentElementId);
            return false;
        }

        $('[id^="image-preview-box-temp"]').remove();
        for (let i = 0; i < selectedFiles.length; i++) {
            $('body').append(`<img class="d-none" id="image-preview-box-temp-${i}" />`);
        }

        for (let j = 0; j < selectedFiles.length; j++) {
            $(`#image-preview-box-temp-${j}`).attr('src', URL.createObjectURL(selectedFiles[j]));
            $(`#image-preview-box-temp-${j}`).off('error');
            $(`#image-preview-box-temp-${j}`).on('error',
                function () {
                    imageInputsWithProblems.push(currentElementId);
                    currentForm.validate().element(`#${currentElementId}`);
                });
        }
        return true;
    });
    jQuery.validator.unobtrusive.adapters.addBool('isImage');

    // maxFileSize
    jQuery.validator.addMethod('maxFileSize', function (value, element, param) {
        let selectedFiles = element.files;
        if (selectedFiles[0] === undefined) {
            return true;
        }
        let maxFileSize = $(element).data('val-maxsize');
        for (let counter = 0; counter < selectedFiles.length; counter++) {
            let currentFile = selectedFiles[counter];
            if (currentFile != null) {
                let currentFileSize = currentFile.size;
                if (currentFileSize > maxFileSize)
                    return false;
            }
        }

        return true;
    });
    jQuery.validator.unobtrusive.adapters.addBool('maxFileSize');

    // makeTinyMceRequired
    jQuery.validator.addMethod('makeTinyMceRequired', function (value, element, param) {
        let editorId = $(element).attr('id');
        let editorContent = tinyMCE.get(editorId).getContent();
        $('body').append(`<div id="test-makeTinyMceRequired">${editorContent}</div>`);
        let result = isNullOrWhitespace($('#test-makeTinyMceRequired').text());
        $('#test-makeTinyMceRequired').remove();
        return !result;
    });
    jQuery.validator.unobtrusive.adapters.addBool('makeTinyMceRequired');

}

// End validation

function isNullOrWhitespace(input) {

    if (typeof input === 'undefined' || input == null) return true;

    return input.replace(/\s/g, '').length < 1;
}

// Ajax (JQuery)                          *****************

function activatingDeleteButtons(isModalMode) {
    $('.delete-row-button').click(function () {
        let currentForm = $(this).parent();
        let customMessage = $(this).attr('custom-message');
        const formData = currentForm.serializeArray();
        Swal.fire({
            title: 'اعلان',
            text: customMessage == undefined ? 'آیا مطمئن به حذف هستید ؟' : customMessage,
            icon: 'warning',
            confirmButtonText: 'بله',
            showDenyButton: true,
            denyButtonText: 'خیر',
            confirmButtonColor: '#067719',
            allowOutsideClick: false
        }).then((result) => {
            if (result.isConfirmed) {

                showLoading();
                $.post(currentForm.attr('action'), formData, function (data) {
                    if (data.isSuccessful == false) {
                        showToastr('warning', data.message);
                    }
                    else {
                        if (isModalMode) {
                            $('#html-modal-place').modal('hide');
                        }
                        showToastr('success', data.message);
                        fillDataTable();
                    }
                }).always(function () {
                    hideLoading();
                }).fail(function () {
                    showErrorMessage();
                });
            }
        });
    });
}

$('#modalOne').on('hide.bs.modal', function () {

    tinyMCE.editors = [];
});

function initializingAutocomplete() {
    $('.autocomplete').each(function () {
        let currentSearchUrl = $(this).attr('autocomplete-search-url');
        let currentId = $(this).attr('id');
        $(`#${currentId}`).autocomplete({
            source: currentSearchUrl == null ? `${location.pathname}?handler=AutocompleteSearch` : currentSearchUrl,
            minLength: 2,
            delay: 500,
            select: function (event, ui) {
                if (typeof window['onAutocompleteSelect'] === 'function') {
                    window['onAutocompleteSelect'](event, ui);
                }
            }
        });
    });
}
function activationModalForm() {
    $('.show-modal-form-button').click(function (e) {
        e.preventDefault();
        let urlToLoadTheForm = $(this).attr('href');

        let customTitle = $(this).attr('custom-Title');
        let functionNameToCallInTheEnd = $(this).attr('functionNameToCallInTheEnd');
        if (customTitle == undefined) {
            customTitle = $(this).text().trim();
        }
        appendFormModalPlaceToBody();
        $('#form-modal-place .modal-header h5').html(customTitle);
        showLoading();
        $.get(urlToLoadTheForm, function (data, status) {
            if (data.isSuccessful === false) {
                showToastr('warning', data.message);
            }
            else {
                $('#form-modal-place .modal-body').html(data);
                initializeTinyMCE();
                initializeSelect2();
                initializingAutocomplete();
                activatingInputAttributes();
                $.validator.unobtrusive.parse($('#form-modal-place form'));
                if (typeof window[functionNameToCallInTheEnd] === 'function') {
                    window[functionNameToCallInTheEnd](data);
                    //این یک فانکشن هست برای کد های اختصاصی و این فقط وقتی اجرا میشه که
                    //این رو خودتون بنویسید داخل فایل جاوااسکریپت اون صفحه
                }
                $('#form-modal-place').modal('show');
            }
        }).fail(function () {
            showErrorMessage();
        }).always(function () {
            hideLoading();
        });
    });
}



function activatingPagination() {
    $('#main-pagination button').not('.active').click(function () {
        isMainPaginationClicked = true;
        let currentPageSelected = $(this).val();
        $('.search-form-via-ajax input[name$="Pagination.CurrentPage"]').val(currentPageSelected);
        $('.search-form-via-ajax').submit();
    });
}
function activatingGotoPage() {
    $('#go-to-page-button').click(function () {

        isGotoPageClicked = true;
    });
}

function fillDataTable() {
    $('.data-table-place .data-table-body').remove();
    $('.search-form-submit-button').attr('disabled', 'disabled');
    $('.data-table-loading').removeClass('d-none');
    $('#RecordNotFound').remove();

    let currentForm = $('form.search-form-via-ajax');
    let formData = currentForm.serializeArray();

    $.get(`${location.pathname}?handler=GetDataTable`, formData,
        function (data) {

            if (data.isSuccessful == false) {
                fillValidationForm(data.data, currentForm);
                showToastr('warning', data.message);
            } else {
                $('.data-table-place').append(data);
                activatingPagination();
                activatingGotoPage();
                activationModalForm();
                activatingDeleteButtons();
                activatingPageCount();
                enablingTooltips();
            }

        }).fail(function () {
            showErrorMessage();
        }).always(function () {
            $('.search-form-submit-button').removeAttr('disabled');
            $('.data-table-loading').addClass('d-none');
        });
}

$(document).on('click', '.get-html-with-ajax', function () {
    var funcToCall = $(this).attr('functionNameToCallOnClick');
    window[funcToCall](this);
});

$(document).on('submit', 'form.custom-ajax-form', function (e) {
    debugger 
    e.preventDefault();
    let currentForm = $(this);
    let formAction = currentForm.attr('action');
    let closeWhenDone = $(this).attr('close-when-done');
    let callFunctionInTheEnd = $(this).attr('call-function-in-the-end');
    let formData = new FormData(this);
    $.ajax({
        url: formAction,
        data: formData,
        type: 'POST',
        enctype: 'multipart/form-data',
        dataType: 'json',
        processData: false,
        contentType: false,
        beforeSend: function () {
            currentForm.find('.submit-custom-ajax-button span').removeClass('d-none');
            currentForm.find('.submit-custom-ajax-button').attr('disabled', 'disabled');
        },
        success: function (data) {
            if (data.isSuccessful === false) {
                let finalData = data.data != null ? data.data : [data.message];
                fillValidationForm(finalData, currentForm);
                showToastr('warning', data.message);
            }
            else {
                fillDataTable();
                if (callFunctionInTheEnd) {
                    customAjaxFormFunction(data);
                }
                if (closeWhenDone !== 'false') {
                    $('#form-modal-place').modal('hide');
                }
                showToastr('success', data.message);
            }
        },
        complete: function () {
            currentForm.find('.submit-custom-ajax-button span').addClass('d-none');
            currentForm.find('.submit-custom-ajax-button').removeAttr('disabled');
        },
        error: function () {
            showErrorMessage();
        }
    });
});

//$('form input').blur(function () {
//    $(this).parents('form').valid();
//});

//$('form select').change(function () {
//    $(this).parents('form').valid();
//});
//$('form input.custom-md-persian-datepicker').change(function () {
//    $(this).parents('form').valid();
//});
//$('form input[type="file"]').change(function () {

//    $(this).parents('form').valid();
//});
//$('form input[type="checkbox"] ').change(function () {

//    $(this).parents('form').valid();
//});

// کد های پایین را جایگزین با بالایی ها کردیم
$(document).on('blur', 'form input', function () {
    var currentForm = $(this).parents('form');
    currentForm.valid();
    if (currentForm.valid()) {
        currentForm.find('div[class*="validation-summary"] ul').html('');
    }

});

$(document).on('change', 'form input.custom-md-persian-datepicker, form select, form input[type="checkbox"], form input[type="file"]', function () {
    $(this).parents('form').valid();
});

$(document).on('submit', 'form.public-ajax-form', function (e) {
    e.preventDefault();
    let currentForm = $(this);
    let formAction = currentForm.attr('action');
    let functionName = currentForm.attr('functionNameToCallInTheEnd');
    let formData = new FormData(this);
    $.ajax({
        url: formAction,
        data: formData,
        type: 'POST',
        enctype: 'multipart/form-data',
        dataType: 'json',
        processData: false,
        contentType: false,
        beforeSend: function () {
            $('#html-modal-place').modal('hide')
            $('#second-html-modal-place').modal('hide')
            showLoading();
        },
        success: function (data) {
            if (data.isSuccessful === false) {
                let finalData = data.data != null ? data.data : [data.message];
                fillValidationForm(finalData, currentForm);
                showToastr('warning', data.message);
            }
            else {
                window[functionName](data.message, data.data);
            }
        },
        complete: function () {
            hideLoading();
        },
        error: function () {
            showErrorMessage();
        }
    });
});

$(document).on('submit', '.get-html-by-sending-form', function (e) {
    e.preventDefault();
    let currentForm = $(this);
    let formAction = currentForm.attr('action');
    let functionName = currentForm.attr('functionNameToCallInTheEnd');
    let formData = new FormData(this);
    $.ajax({
        url: formAction,
        data: formData,
        type: 'POST',
        enctype: 'multipart/form-data',
        dataType: 'json',
        processData: false,
        contentType: false,
        beforeSend: function () {
            $('#html-modal-place').modal('hide');
            showLoading();
        },
        success: function (data) {
            if (data.isSuccessful === false) {
                let finalData = data.data != null ? data.data : [data.message];
                fillValidationForm(finalData, currentForm);
                showToastr('warning', data.message);
            }
            else {
                window[functionName](data.data);
            }
        },
        complete: function () {
            hideLoading();
        },
        error: function () {
            showErrorMessage();
        }
    });
});

let isMainPaginationClicked = false;
let isGotoPageClicked = false;

function activatingPageCount() {
    $('#page-count-selectbox').change(function () {
        let pageCountValue = this.value;
        $('form.search-form-via-ajax input[name$="Pagination.PageCount"]').val(pageCountValue);
        $('form.search-form-via-ajax').submit();
    })
}

$(document).on('submit', 'form.search-form-via-ajax', function (e) {
    e.preventDefault();

    let currentForm = $(this);
    let pageNumberInput = $('#page-number-input').val();
    if (isGotoPageClicked || $('#page-number-input').is(':focus')) {
        currentForm.find('input[name$="Pagination.CurrentPage"').val(pageNumberInput);
    } else if (!isMainPaginationClicked) {
        currentForm.find('input[name$="Pagination.CurrentPage"').val(1);
    }
    const formData = currentForm.serializeArray();
    //show loading disabled button
    currentForm.find('.search-form-submit-button').attr('disabled', 'disabled');
    currentForm.find('.search-form-submit-button span').removeClass('d-none');

    $('.data-table-loading').removeClass('d-none');
    $('.data-table-body').html('');
    $('#RecordNotFound').html(''); // my code


    $('[data-bs-toggle="tooltip"], .tooltip').tooltip("hide");
    $('#record-not-found-box').remove();

    $.get(`${location.pathname}?handler=GetDataTable`, formData, function (data) {
        isMainPaginationClicked = false;
        isGotoPageClicked = false;
        //Hide loading and activation button
        currentForm.find('.search-form-submit-button').removeAttr('disabled')
        currentForm.find('.search-form-submit-button span').addClass('d-none');
        $('.data-table-loading').addClass('d-none');

        if (data.isSuccessful == false) {
            let finalData = data.data != null ? data.data : [data.message];
            fillValidationForm(finalData, currentForm);
            showToastr('warning', data.message);
        }
        else {
            $('.data-table-place ').html('');
            $('.data-table-place ').append(data);
            currentForm.find('div[class*="validation-summary"]').html('');
            activatingPagination();
            activatingGotoPage();
            activationModalForm();
            activatingDeleteButtons();
            activatingPageCount();
            enablingTooltips();
        }
    }).fail(function () {
        showErrorMessage();
    }).always(function () {
        hideLoading();
    });;
});

function fillValidationForm(errors, currentForm) {

    let result = '<ul>';
    errors.forEach(function (e) {
        result += `<li>${e}</li>`;
    });
    result += '</ul>';
    currentForm.find('div[class*="validation-summary"]').html(result);

}
// End Ajax

function getDataWithAjax(url, formData, functionNameToCallInTheEnd) {
    $.ajax({
        url: url,
        data: formData,
        type: 'GET',
        dataType: 'json',
        processData: true,
        contentType: false,
        beforeSend: function () {
            showLoading();
        },
        success: function (data) {
            if (data.isSuccessful == false) {
                showToastr('warning', data.message);
            }
            else {
                window[functionNameToCallInTheEnd](data.data, data.message);
            }
        },
        complete: function () {
            hideLoading();
        },
        error: function () {
            showErrorMessage();
        }
    });
}
// خواندن صفحات
// html
// از سمت سرور
function getHtmlWithAJAX(url, formData, functionNameToCallInTheEnd, clickedButton) {
    $.ajax({
        url: url,
        data: formData,
        type: 'GET',
        dataType: 'html',
        traditional: true,
        beforeSend: function () {
            showLoading();
        },
        success: function (data) {

            if (data.isSuccessful === false) {
                showToastr('warning', data.message);
            } else {
                window[functionNameToCallInTheEnd](data, clickedButton);
            }
        },
        complete: function () {
            hideLoading();
        },
        error: function (e) {
            console.log(e)
            showErrorMessage();
        }
    });
}

function activatingInputAttributes() {
    $('input[data-val-ltrdirection="true"]').attr('dir', 'ltr');
    $('input[data-val-isimage]').attr('accept', 'image/*');
}

//show preview in under the input (Create Seller Page)
$('.image-preview-input').change(function () {
    let selectedFile = this.files[0];
    let imgPreviewBox = $(this).attr('image-preview-box');
    if (selectedFile && selectedFile.size > 0) {
        $(`#${imgPreviewBox}`).removeClass('d-none');
        $(`#${imgPreviewBox} img`).attr('src', URL.createObjectURL(selectedFile));

    } else {
        $(`#${imgPreviewBox}`).addClass('d-none');
        $(`#${imgPreviewBox} img`).attr('src', '');
    }
});

// نمایش پیش نمایش عکس برای حالت چند عکسی
$('.multiple-images-preview-input').change(function () {
    let selectedFiles = this.files;
    let imagesPreviewBox = $(this).attr('images-preview-box');
    if (selectedFiles && selectedFiles.length > 0) {
        $(`#${imagesPreviewBox}`).html('');
        $(`#${imagesPreviewBox}`).removeClass('d-none');
        for (let i = 0; i < selectedFiles.length; i++) {
            $(`#${imagesPreviewBox}`).append('<div class="my-2"><img width="100" src="" /></div>');
            $(`#${imagesPreviewBox} img:last`).attr('src', URL.createObjectURL(selectedFiles[i]));
        }
    } else {
        $(`#${imagesPreviewBox}`).addClass('d-none');
    }
});

// Convert English numbers to Persian numbers
// https://seifzadeh.blog.ir/post/convert-number-javascript
String.prototype.toPersinaDigit = function () {
    var id = ['۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹'];
    return this.replace(/[0-9]/g, function (w) {
        return id[+w];
    });
}

// copy Text
function fallbackCopyTextToClipboard(text, functionNameToCallInTheEnd) {
    var textArea = document.createElement('textarea');
    textArea.value = text;

    // Avoid scrolling to bottom
    textArea.style.top = '0';
    textArea.style.left = '0';
    textArea.style.position = 'fixed';

    document.body.appendChild(textArea);
    textArea.focus();
    textArea.select();

    try {
        var successful = document.execCommand('copy');
        if (!successful)
            showErrorMessage('مرورگر شما قابلیت کپی کردن متن را ندارد');
        else {
            if (typeof window[functionNameToCallInTheEnd] === 'function') {
                window[functionNameToCallInTheEnd]();
            }
        }
    } catch (err) {
        showErrorMessage('مرورگر شما قابلیت کپی کردن متن را ندارد');
    }

    document.body.removeChild(textArea);
}
function copyTextToClipboard(text, functionNameToCallInTheEnd) {
    if (!navigator.clipboard) {
        fallbackCopyTextToClipboard(text, functionNameToCallInTheEnd);
        return;
    }
    navigator.clipboard.writeText(text).then(function () {
        if (typeof window[functionNameToCallInTheEnd] === 'function') {
            window[functionNameToCallInTheEnd]();
        }
    }, function (err) {
        showErrorMessage('مرورگر شما قابلیت کپی کردن متن را ندارد');
    });
}
function convertEnglishNumbersToPersianNumber() {
    $('.persian-numbers').each(function () {
        var result = $(this).html().toPersinaDigit();
        $(this).html(result);
    });
}
// Add comma after 3 digits
String.prototype.addCommaToDigits = function () {
    return this.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
}
$(function () {
    activatingInputAttributes();
    initializeSelect2WithoutModal();
    initializeTinyMCE();
    enablingNormalTooltips();
    convertEnglishNumbersToPersianNumber();
  
    // Enable img for tinymce 
    $('textarea[add-image-plugin="true"]').each(function () {
        let elementId = $(this).attr('id');
        let currentTinyMce = tinymce.get(elementId);
        currentTinyMce.settings.plugins += ' image'; // enable image
        currentTinyMce.settings.toolbar[4].items.push('image'); // show in toolbar
        currentTinyMce.settings.image_title = true; // enable title input for picture
    });

    // Initialize TinyMCE upload image plugin
    $('textarea.custom-tinymce').each(function () {
        let elementId = $(this).attr('id');
        let uploadImageUrl = $(this).attr('upload-image-url');
        let tinyMceInstance = tinymce.get(elementId);
        tinyMceInstance.settings.images_upload_handler = function (blobInfo, success, failure, progress) {
            sendTinyMceImagesToServer(blobInfo, success, failure, progress, uploadImageUrl);
        };
    });
})