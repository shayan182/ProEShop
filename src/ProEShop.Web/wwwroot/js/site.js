//__RequestVerificationToken
var rvt = '__RequestVerificationToken';
var htmlModalPlace = `<div class="modal fade" id="html-modal-place" data-bs-backdrop="static">
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

function appendHtmlModalPlaceToBody() {
    if ($('#html-modal-place').length === 0) {
        $('body').append(htmlModalPlace);
    }
}

var formModalPlace = `<div class="modal fade" id="form-modal-place" data-bs-backdrop="static">
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

var loadingModalHtml = `<div class="modal" id="loading-modal" data-bs-backdrop="static">
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
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    //$('[data-toggle="tooltip"]').tooltip({
    //    trigger: 'hover'
    //});
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
}


function showErrorMessage(message) {
    showToastr('error', 'خطایی به وجود آمد، لطفا مجددا تلاش نمایید');
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
    var formData = new FormData();
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
        var textareaId = `#${$(this).attr('id')}`;
        tinymce.remove(textareaId);
        tinymce.init({
            selector: textareaId,
            setup: function (editor) {
                editor.on('blur', function (e) {
                    var elementId = $(e.target.targetElm).attr('id');
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
var imageInputsWithProblems = [];


function removeItemInArray(arr, item) {
    var found = arr.indexOf(item);

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

    var defaultRangeValidator = $.validator.methods.range;

    $.validator.methods.range = function (value, element, param) {
        if (element.type === 'checkbox') {
            return element.checked;
        } else {
            return defaultRangeValidator.call(this, value, element, param);
        }
    }

    // fileRequired
    jQuery.validator.addMethod("fileRequired", function (value, element, param) {
        if (element.files[0] != null)
            return element.files[0].size > 0;
        return false;
    });
    jQuery.validator.unobtrusive.adapters.addBool("fileRequired");

    // allowExtensions
    jQuery.validator.addMethod('allowExtensions', function (value, element, param) {
        if (element.files[0] != null) {
            var whiteListExtensions = $(element).data('val-whitelistextensions').split(',');
            return whiteListExtensions.includes(element.files[0].type);
        }
        return true;
    });
    jQuery.validator.unobtrusive.adapters.addBool('allowExtensions');

    // isImage
    jQuery.validator.addMethod('isImage', function (value, element, param) {
        var selectedFile = element.files[0];
        if (selectedFile === undefined) {
            return true;
        }
        var whiteListExtensions = $(element).data('val-whitelistextensions').split(',');
        if (!whiteListExtensions.includes(selectedFile.type)) {
            return false;
        }
        var currentElementId = $(element).attr('id');
        var currentForm = $(element).parents('form');

        if (imageInputsWithProblems.includes(currentElementId)) {
            removeItemInArray(imageInputsWithProblems, currentElementId);
            return false;
        }

        if ($('#image-preview-box-temp').length === 0) {
            $('body').append('<img class="d-none" id="image-preview-box-temp" />');
        }
        $('#image-preview-box-temp').attr('src', URL.createObjectURL(selectedFile));
        $('#image-preview-box-temp').off('error');
        $('#image-preview-box-temp').on('error',
            function () {
                imageInputsWithProblems.push(currentElementId);
                currentForm.validate().element(`#${currentElementId}`);
            });
        return true;
    });
    jQuery.validator.unobtrusive.adapters.addBool('isImage');

    // maxFileSize
    jQuery.validator.addMethod('maxFileSize', function (value, element, param) {
        if (element.files[0] != null) {
            var maxFileSize = $(element).data('val-maxsize');
            var selectedFileSize = element.files[0].size;
            return maxFileSize >= selectedFileSize;
        }
        return true;
    });
    jQuery.validator.unobtrusive.adapters.addBool('maxFileSize');

    // makeTinyMceRequired
    jQuery.validator.addMethod('makeTinyMceRequired', function (value, element, param) {
        var editorId = $(element).attr('id');
        var editorContent = tinyMCE.get(editorId).getContent();
        $('body').append(`<div id="test-makeTinyMceRequired">${editorContent}</div>`);
        var result = isNullOrWhitespace($('#test-makeTinyMceRequired').text());
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
        var currentForm = $(this).parent();
        var customMessage = $(this).attr('custom-message');
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
    if ($('.autocomplete').length > 0) {
        $('.autocomplete').autocomplete({
            source: `${location.pathname}?handler=AutocompleteSearch`,
            minLength: 2,
            delay: 500,
            select: function (event, ui) {
                window['onAutocompleteSelect'](event, ui);
            }
        });

    }
}
function activationModalForm() {
    $('.show-modal-form-button').click(function (e) {
        debugger
        e.preventDefault();
        var urlToLoadTheForm = $(this).attr('href');

        var customTitle = $(this).attr('custom-Title');
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
                if (typeof actionsAfterLoadModalForm === 'function') {
                    actionsAfterLoadModalForm();
                    //این یک فانکشن هست برای کد های اختصاصی و این فقط وقتی اجرا میشه که
                    //این رو خودتون بنویسید داخل فایل جاوااسکریپت اون صفحه
                }
                $.validator.unobtrusive.parse($('#form-modal-place form'));
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
    $('#main-pagianation button').click(function () {
        isMainPaginationClicked = true;
        var currentPageSelected = $(this).val();
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

    var currentForm = $('form.search-form-via-ajax');
    var formData = currentForm.serializeArray();

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
                activatingGetHtmlWithAjax();
            }

        }).fail(function () {
            showErrorMessage();
        }).always(function () {
            $('.search-form-submit-button').removeAttr('disabled');
            $('.data-table-loading').addClass('d-none');
        });
}
function activatingGetHtmlWithAjax() {
    $('.get-html-with-ajax').click(function () {
        var funcToCall = $(this).attr('functionNameToCallOnClick');
        window[funcToCall](this);
    });
}

$(document).on('submit', 'form.custom-ajax-form', function (e) {
    e.preventDefault();
    var currentForm = $(this);
    var formAction = currentForm.attr('action');
    var formData = new FormData(this);
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
            if (data.isSuccessful == false) {
                var finalData = data.data != null ? data.data : [data.message];
                fillValidationForm(finalData, currentForm);
                showToastr('warning', data.message);
            }
            else {
                fillDataTable();
                $('#form-modal-place').modal('hide');
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

$('form input').blur(function () {
    $(this).parents('form').valid();
});

$('form select').change(function () {
    $(this).parents('form').valid();
});
$('form input.custom-md-persian-datepicker').change(function () {
    $(this).parents('form').valid();
});
$('form input[type="file"]').change(function () {

    $(this).parents('form').valid();
});
$('form input[type="checkbox"] ').change(function () {

    $(this).parents('form').valid();
});


$(document).on('submit', 'form.public-ajax-form', function (e) {
    e.preventDefault();
    var currentForm = $(this);
    var formAction = currentForm.attr('action');
    var functionName = currentForm.attr('functionNameToCallInTheEnd');
    var formData = new FormData(this);
    $.ajax({
        url: formAction,
        data: formData,
        type: 'POST',
        enctype: 'multipart/form-data',
        dataType: 'json',
        processData: false,
        contentType: false,
        beforeSend: function () {
            showLoading();
        },
        success: function (data) {
            if (data.isSuccessful === false) {
                var finalData = data.data != null ? data.data : [data.message];
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


var isMainPaginationClicked = false;
var isGotoPageClicked = false;

function activatingPageCount() {
    $('#page-count-selectbox').change(function () {
        var pageCountValue = this.value;
        $('form.search-form-via-ajax input[name$="Pagination.PageCount"]').val(pageCountValue);
        $('form.search-form-via-ajax').submit();
    })
}

$(document).on('submit', 'form.search-form-via-ajax', function (e) {
    e.preventDefault();

    var currentForm = $(this);
    var pageNumberInput = $('#page-number-input').val();
    if (isGotoPageClicked || $('#page-number-input').is(':focus')) {
        currentForm.find('input[name$="Pagination.CurrentPage"').val(pageNumberInput);
    } else if (!isMainPaginationClicked) {
        currentForm.find('input[name$="Pagination.CurrentPage"').val(1);
    }
    const formData = currentForm.serializeArray();
    //show loading disabled button
    currentForm.find('.search-form-submit-button').attr('disabled', 'disabled')
    currentForm.find('.search-form-submit-button span').removeClass('d-none');

    $('.data-table-loading').removeClass('d-none');
    $('.data-table-body').html(''); // not working :(


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
            var finalData = data.data != null ? data.data : [data.message];
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
            activatingGetHtmlWithAjax();
        }
    });
});

function fillValidationForm(errors, currentForm) {

    var result = '<ul>';
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
            debugger
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
        error: function () {
            showErrorMessage();
        }
    });
}

function activatingInputAttributes() {
    $('input[data-val-ltrdirection="true"]').attr('dir', 'ltr');
    $('input[data-val-isimage]').attr('accept', 'image/*');
}

//show preview in under the input (Create Seller Page)
$('.image-preivew-input').change(function () {
    var selectedFile = this.files[0];
    var imgPreviewBox = $(this).attr('image-preview-box');
    if (selectedFile && selectedFile.size > 0) {
        $(`#${imgPreviewBox}`).removeClass('d-none');
        $(`#${imgPreviewBox} img`).attr('src', URL.createObjectURL(selectedFile));

    } else {
        $(`#${imgPreviewBox}`).addClass('d-none');
        $(`#${imgPreviewBox} img`).attr('src', '');
    }
});

$(function () {
    activatingInputAttributes();
    initializeSelect2WithoutModal();
    initializeTinyMCE();
    enablingTooltips();

    // Enable img for tinymce 
    $('textarea[add-image-plugin="true"]').each(function () {
        var elementId = $(this).attr('id');
        var currentTinyMce = tinymce.get(elementId);
        currentTinyMce.settings.plugins += ' image'; // enable image
        currentTinyMce.settings.toolbar[4].items.push('image'); // show in toolbar
        currentTinyMce.settings.image_title = true; // enable title input for picture
    });
})