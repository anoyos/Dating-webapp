/// <reference path="Constants.js" />
//var setting={ismultiple:true,title:'Upload Attachments',AllowMultipleElement:[]}
$(function () {
    $(document).on('click', '.btnuploadcasefiles', function () {
        var parent = $(this).closest('.fileuploadpanel');
        $('.uploadcasefiles', parent).click();
    });
    $(document).on('change', '.uploadcasefiles', function () {
        
        var parent = $(this).closest('.fileuploadpanel');
        var setting =$('.btnuploadcasefiles', parent).data('uploadsetting');
        
        for (var i = 0; i < $(this).get(0).files.length; i++) {
            var file = $(this).get(0).files[i];
            if (file) {
                
                var element = CreateFileButton();
                

                if (IsMultipleFileAllowed(setting))//AllowMultipleElement
                    $(element).append('&nbsp;&nbsp;<i class="fa fa-trash filetrashbt"></i>');
                else {
                    $('.uploadcasefilespanel',parent).html('');
                }

                $('.uploadcasefilespanel', parent).append(element);                
                $(element).find('.filename').text(file.name)
                ShowAjaxLoader();
                $(element).attr('data-actualname', file.name);
                UploadFile(file, function (result, element) {
                    HideAjaxLoader();
                    if (!IsImage(result)) {
                        $(element).attr('data-filename', result);
                     //  $(element).button('reset');
                    }
                    else {
                        CreateImageOfAttachment(element, result);
                    }
                }, element);
            }
        }
    });
    $(document).on('click', '.filetrashbt', function (e) {
        e.preventDefault();
        $(this).closest('button').remove();

    });
});
function IsMultipleFileAllowed(setting){
    return setting.ismultiple || (setting.AllowMultipleElement && setting.AllowMultipleElement.indexOf($('#AttachModal').attr('data-sourcebt')) > -1)?true:false;
}
function CreateImageOfAttachment(element, filename) {
    var _element = $('<a><img class="galpop-single" src="/public/img/upload/' + filename + '" style="width: 76px;cursor: pointer;"/></a>');
    $(_element).attr('data-actualname', $(element).attr('data-actualname'));
    $(_element).attr('data-filename', filename);


     $(element).replaceWith(_element);
    $(_element).attr('href', '/public/img/upload/' + filename);
    try {
        $(_element).galpop();
    }
    catch{}
    return _element;
}
function IsImage(filename) {
    filename = filename.toLowerCase();
    var imgexts = ['.jpg','jpeg','.gif','.png'];
    for (var i = 0; i < imgexts.length; i++) {
        if (filename.indexOf(imgexts[i])>-1) {
            return true;
        }
    }
    return false;
}

function InitFileUploader(panel,setting) {
    var obj = CreateFileUploader();
    $('.btnuploadcasefiles', obj).attr('data-uploadsetting', JSON.stringify(setting));
    $('.uploadbttitle', obj).text(setting.title);
    if (IsMultipleFileAllowed(setting)) {
        $('.uploadcasefiles', obj).attr('multiple', true);
    }
    $(panel).append(obj);
}
function ResetFileUploader(parent) {
    $('.uploadcasefilespanel', parent).html('');
}
function getFilesObjectArray(parent) {
    var list = [];
    $('.uploadcasefilespanel button,.uploadcasefilespanel a', parent).each(function () {
        if ($(this).attr('data-filename')) {
            list.push({ Name: $(this).data('filename'), ActualName: $(this).data('actualname') });
        }
    });
    return list;
}
function RenderExistingFiles(list, parent) {
    for (var i = 0; i < list.length; i++) {
        var file = list[i];
        var element = $('<button class="btn btn-danger m-r-5" style="margin: 5px;" type="button"><i class="fa fa-download"></i>&nbsp;&nbsp;<span class="bold">' + file.ActualName + '</span></button>');
        $(element).attr('data-actualname', file.ActualName);
        $('.uploadcasefilespanel', parent).append(element);
        $(element).attr('data-filename', file.Name);
        if (IsImage(file.Name)) {
            CreateImageOfAttachment(element, file.Name);
        }
    }
}
function CreateFileUploader() {
    return $('<div class="row fileuploadpanel">' +
        '<div class="col-lg-3">' +
          '<br>' +
          '<button class="btn btn-success btnuploadcasefiles" type="button"><i class="fa fa-upload"></i>&nbsp;&nbsp;<span class="bold uploadbttitle"></span></button>' +
       '</div>' +
       '<div class="col-lg-9 "><br><p class="uploadcasefilespanel"></p></div>' +
       '<input type="file"  class="uploadcasefiles" style="display:none" />' +
     '</div>');
}
function CreateFileButton() {
    return $('<button class="btn btn-danger m-r-5" data-loading-text="<i class=\'fa fa-circle-o-notch fa-spin\'></i> Uploading..." style="margin: 5px;" type="button"><i class="fa fa-download"></i>&nbsp;&nbsp;<span class="bold"><span class="filename"></span></span></button>');
}