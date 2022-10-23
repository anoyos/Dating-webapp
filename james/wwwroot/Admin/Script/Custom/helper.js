function IsOverlappingMinMax(min,max , minNew,maxNew) {
    if ((min <= minNew && max >= minNew) ||
        (max>=minNew && max<=maxNew) ||
        (min<=minNew && max>=maxNew)||
        (maxNew>=min && maxNew<=max)
        )
    {
        return true;
    }
    return false;
}


function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
  if(charCode == 189)
    {
        return false;
    }
   else if (charCode == 189|| charCode == 46 || ( charCode > 31 && (charCode < 48 || charCode > 57)))
    {
        return false;
    }
   
    else if (charCode == 45 )
    {
        if (evt.currentTarget.id == "TxtTo")
        {
            $("#TxtTo").val(' '); return false;
        }
        else if (evt.currentTarget.id == "TxtFrom")
        {
            $("#TxtFrom").val(' '); return false;
        }
        else if (evt.currentTarget.id == "Txtquantitymin") {
            $("#Txtquantitymin").val(' '); return false;
        }
        else if (evt.currentTarget.id == "Txtquantitymax") {
            $("#Txtquantitymax").val(' '); return false;
        }
        else if (evt.currentTarget.id == "TxtFrom") {
            $("#TxtFrom").val(' '); return false;
        }
        else if (evt.currentTarget.id == "Txtmax") {
            $("#Txtmax").val(' '); return false;
        }
        else if (evt.currentTarget.id == "Txtmin") {
            $("#Txtmin").val(' '); return false;
        }
    }
    return true;
}


function isNumberKey2(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    var val = evt.target.value;
    var lastcode=$(evt.target).attr("data-last-code");
    $(evt.target).attr("data-last-code", charCode);
    var hasDecimal = (val * 1 % 1 != 0);
    if (lastcode == 46 || lastcode == 44) {
        hasDecimal = true;
    }

    if (charCode == 45) {
        return false;
    }
    if ((hasDecimal && (charCode == 46 || charCode == 44)) && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }   
    
    return true;
}

function NotAllowDecimalAndNegative(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode == 189) {
        return false;
    }
    else if (charCode == 189 || charCode == 46 || (charCode > 31 && (charCode < 48 || charCode > 57))) {
        return false;
    }

}


function AllowDecimalAndNegative(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    var val = evt.target.value;
    var lastcode = $(evt.target).attr("data-last-code");
    $(evt.target).attr("data-last-code", charCode);
    var hasDecimal = (val * 1 % 1 != 0);
    if (lastcode == 46 || lastcode == 44) {
        hasDecimal = true;
    }

 
    if ((hasDecimal && (charCode == 46 || charCode == 44)) && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }

    return true;
}

function discountrange(evt) {
    var value = evt.target.value * 1;
  //  var dis = $("#TxtDiscount").val();
    var dis = evt.target.value;
    if (value >=100 )
    {
        //   $("#TxtDiscount").val(dis.slice(0, 2));
       
        $("#"+evt.target.id).val(dis.slice(0, 2));
        return true;

    }
    else   if (  value <=-100)
    {
        $("#" + evt.target.id).val(dis.slice(0, 3));
        return true;


    }
        return false;
}
function uiBlock(msg, el, loaderOnTop) {
    lastBlockedUI = el;
    if (msg == '' || msg == null || msg == "") {
        msg = "LOADING";
    }
    if (el) {
        jQuery(el).block({
            message: '<div class="loading-message loading-message-boxed"><img src="Images/loading-spinner-grey.gif" align="absmiddle"><span>&nbsp;&nbsp;' + msg + '...</span></div>',
            baseZ: 2000,
            css: {
                border: 'none',
                padding: '2px',
                backgroundColor: 'none',
            },
            overlayCSS: {
                backgroundColor: '#000',
                opacity: 0.05,
                cursor: 'wait'
            }
        });
    } else {
        if (msg == '' || msg == null || msg == "") {
            msg = "LOADING";
        }
        $.blockUI({
            message: '<div class="loading-message loading-message-boxed"><img src="Images/loading-spinner-grey.gif" align="absmiddle"><span>&nbsp;&nbsp;' + msg + '...</span></div>',
            baseZ: 2000,
            css: {
                border: 'none',
                padding: '2px',
                backgroundColor: 'none'
            },
            overlayCSS: {
                backgroundColor: '#000',
                opacity: 0.05,
                cursor: 'wait'
            }
        });
    }
}

function uiUnBlock(msg, el) {
    if (msg == '' || msg == null || msg == "") {
        msg = "LOADING";
    }
    if (el) {
        jQuery(el).unblock({
            onUnblock: function () {
                jQuery(el).removeAttr("style");
            }
        });
    }
    else {
      //  $.unblockUI();
    }
}

function showConfirm(_text, _success, _cancel) {

    notyfy({
        layout: 'center',
        text: _text,
        modal: true,
        buttons: [
          {
              addClass: 'btn btn-primary', text: 'OK', onClick: function ($noty) {
                  _success();
                  $noty.close();
              }
          },
          {
              addClass: 'btn btn',
              text: 'Cancel',
              onClick: function ($noty) {
                  if (_cancel) {
                      _cancel();
                  }
                  $noty.close();
              }
          }
        ]
    });
    //notyfy(
    //{
    //    layout: 'top',
    //    text: text,
    //    type: 'confirm',
    //    dismissQueue: true,
    //    buttons: [{
    //        addClass: 'btn btn-success btn-small glyphicons btn-icon ok_2',
    //        text: '<i></i> Ok',
    //        onClick: function ($notyfy) {
    //            $notyfy.close();
    //            success();
    //        }
    //    }, {
    //        addClass: 'btn btn-danger btn-small glyphicons btn-icon remove_2',
    //        text: 'Cancel',
    //        onClick: function ($notyfy) {
    //            $notyfy.close();
    //        }
    //    }]
    //});
}

function showNotification(_message, _type) {

    switch (_type) {
        case 'success':
            toastr.success(_message);
            break;
        case 'error':
            toastr.error(_message)
            break;
        case 'warning':
            toastr.warning(_message)
            break;
    }
}


function XHRPOSTRequestForFormData(_url, _data, _onsuccess) {
    return $.ajax({
        type: "POST",
        url: _url,
        processData: false,
        contentType: false,
        data: _data,
        dataType: "json",
        beforeSend: function () {
            // uiBlock();
        },
        success: _onsuccess,
        error: function (error) {
            //uiUnBlock();
            if (error.status == 403) {
                RedirectLogin();
            }
        }
    });
}

function XHRGETRequest(_url, _data, _onsuccess) {
    $.ajax({
        type: "GET",
        url: _url,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: _data,
        beforeSend: function () {
            // uiBlock();
        },
        success: _onsuccess,
        error: function (error) { uiUnBlock(); }
    });
}

function XHRPOSTRequest(_url, _data, _onsuccess) {
    return $.ajax({
        type: "POST",
        url: _url,
        data: _data,
        dataType: "json",
        beforeSend: function () {
            // uiBlock();
        },
        success: _onsuccess,
        error: function (error) {
            //uiUnBlock();
            if (error.status == 403) {
                RedirectLogin();
            }
        }
    });
}

function XHRPOSTRequestAsync(_url, _data, _onsuccess) {
    $.ajax({
        type: "POST",
        url: _url,
        data: _data,
        dataType: "json",
        success: _onsuccess,
        error: function (error) {

            //  uiUnBlock();
            if (error.status == 403) {
                RedirectLogin();
               
            }

        }
    });
}

function RedirectLogin() {
    swal({
        title: "Session Expire",
        text: "Need to Re-Login",
        type: "error",
        showCancelButton: false,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Ok",
        closeOnConfirm: false
    }, function () {

        window.location.href = "/Account/Index";
        // swal("Deleted!", "Your imaginary file has been deleted.", "success");
    });
}
var roles = {
    AdministrativeAssistants: "1",
    SalesPeople: "2",
    Customer: "3",
    PurchaseManager: "4",
    Scheduler: "5",
    Vendor: "6",
    SubContractor: "7",
    Account: "8",
    Manager: "9",
    Admin: "10"
}

var visitstatus = {
    Pending: "Pending",
    Incomplete: "Incomplete",
    Complete: "Complete"
}

var messages = {

    //Added
    SendEmail: "Email Send Successfully Successfully",
    OrderIncomplete: "Please fill items in Product List",
    OrderEmpty: "Please Add Items in Product List",
    OrderAdded: "",
    useradded: "adicional correctamente",
    indicatoradded: "Indicator Added Successfully",
    knowlegdeareaadded: "Knowlegde Area Added Successfully",
    visitadded: "Visit Added Successfully",
    visitfinalized: "Visit in now marked as Finalized",
    actionplanadedd: "Action Plan saved Successfully",
    passwordchange: "Password changed Successfully",
    trainingadded: "Training Added Successfully",

    //Update
    userupdate: "Actualizado correctamente",
    indicatorupdate: "Indicator Updated Successfully",
    knowlegdeareaupdate: "Knowlegde Area Updated Successfully",
    visitupdate: "Visit Updated Successfully",
    trainingupdate: "Training Updated Successfully",

    //Delete
    userdelete: "eliminado correctamente",
    indicatordelete: "Indicator deleted Successfully",
    knowlegdeareadelete: "Knowlegde Area deleted Successfully",
    visitadelete: "Visit deleted Successfully",
    trainingdelete: "Training deleted Successfully",


    //Confirm
    roledeleteconfirm: "Are you sure you want to delete this role?",
    productdeleteconfirm: "Are you sure you want to delete this product?",
    categorydeleteconfirm: "Are you sure you want to delete this category?",
    budgetdeleteconfirm: "Are you sure you want to delete this budget?",
    departmentdeleteconfirm: "Are you sure you want to delete this department?",
    userdeleteconfirm: "Are you sure you want to delete this user?",
    indicatordeleteconfirm: "Are you sure you want to delete this indicator?",
    knowlegdeareadeleteconfirm: "Are you sure you want to delete this knowlegde area?",
    visitdeleteconfirm: "Are you sure you want to delete this visit?",
    visitfinalizeconfirm: "Did you answer all the indicators for each knowlegde area & you want to finalize this scan?",
    trainingdeleteconfirm: "Are you sure you want to delete this Training ?",


    //Alert
    budgetalreadydefine: "Your already define the budget of department agains this category.",
    emailexist: "The emailAddress is already exists. Please enter another.",
    startscan: "Do you want to start the scan now.?",
    passwordnotmatch: "Password did not match with your current password.",
    selectall: "You cannot mark any indicator blank. Please mark highlighted Indicators.",
    //answerall: "Please mark all Indicators in order to complete this Knowlegde Area.",
    answerall: "Please finalized knowledged areas",
    indicatorsunanswerederror: "Scan cannot be finalized because there are some indicators are left unanswered.",
    indicatorssignatureerror: "Scan cannot be finalized because system doesn't have the signature against this visit.",
    leavescan: "You are leaving screen without saving. Do you want to save changes ?",
    leavescan2: "If you left any unsaved data we will save it for you !"
};

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}
Date.prototype.monthDays = function () {
    var d = new Date(this.getFullYear(), this.getMonth() + 1, 0);
    return d.getDate();
}
function getDateFrom_DDMMYYY(strdate) {
    strdate = strdate.replace('-', '/');
    var dt1 = strdate.substring(0, 2);
    var mon1 = strdate.substring(3, 5);
    var yr1 = strdate.substring(6, 10);
    temp1 = mon1 + "/" + dt1 + "/" + yr1;
    return new Date(temp1);
}
function getCurrentPageRole(actionname) {    
    var url = location.pathname.split('/');
    for (var i = 0; i < roleacess.length; i++) {
        var item = roleacess[i];
        if (url[1].toLowerCase()==item.controller.toLowerCase() && actionname.indexOf(item.action.toLowerCase()) > -1 && item.ischecked) {
            return true;
        }
    }
    return false;
}
$('[data-type=number]').keydown(function (e) {
        if (!$.isNumeric(String.fromCharCode(e.which)) && e.which != 8) {
            e.preventDefault();
        }
});

function RenderDDL(select, list, isAddDefault) {
    if (isAddDefault) {
        $(select).html('<option value="" disabled selected>--Select--</option>');
    }
    $.each(list, function (index, item) { $(select).append($("<option />").val(item.id).text(item.name).attr('data-detail', JSON.stringify(item))); });
    return $(select);
}


function Download(filename, actualname) {
    var myurl = "/Common/Download?";
    var mydata = new Object();
    mydata.filename = filename;
    mydata.actualname = actualname;
    DownloadFiles(myurl, mydata, null);
}

$(document).on('click', '[data-filename]:not(a)', function (e) {
    if (!$(e.target).hasClass('filetrashbt'))
        Download($(this).data('filename'), $(this).data('actualname'));
});
// Find-like method which masks any descendant
// branches matching the Mask argument.
$.fn.findExclude = function (Selector, Mask, result) {

    // Default result to an empty jQuery object if not provided
    var result = typeof result !== 'undefined' ?
        result :
        new jQuery();

    // Iterate through all children, except those match Mask
    this.children().each(function () {

        var thisObject = jQuery(this);
        if (thisObject.is(Selector))
            result.push(this);

        // Recursively seek children without Mask
        if (!thisObject.is(Mask))
            thisObject.findExclude(Selector, Mask, result);
    });

    return result;
}
function ObjToDomData(item, container) {
    for (var key in item) {
        var field = $('[name=' + key + ']', container).get(0);

        var selectobjfield = $('[data-selectobj=' + key + ']', container).get(0);

        var value = item[key];
        if ((field || selectobjfield) && value) {

            if (selectobjfield) {
                if ($(selectobjfield).prop("tagName").toLowerCase() == "select") {//multiple select
                    $(selectobjfield).val(value.map((d, i) => d.id));
                }
            }
            else if ($(field).prop("tagName").toLowerCase() == "img") {
                $(field).attr('src', path.upload + value);
            }
            else {
                $(field).val(value);
            }
        }
    }
    $('select[name]', container).trigger('chosen:updated');
}

function DomDataToObj(container, data) {
    if (!data) { data = {}; }
   // data.isactive = true;
   // data.isdeleted = false;
    $(container).findExclude('[name]', '[data-objname]').each(function () {
        var fieldname = $(this).attr('name');
        
        if ($(this).prop("tagName").toLowerCase() == "select" && $(this).attr('multiple') && $(this).val()) {
            data[$(this).attr('name')] = [];
            for (var i = 0; i < $(this).val().length; i++) {
                //var _obj = {};
                data[$(this).attr('name')].push( $(this).val()[i]);
                //data[$(this).attr('name')].push(_obj);
            }
        }
        else if ($(this).prop("tagName").toLowerCase() == "img") {
            data[fieldname] = $(this).attr('data-filename');
        }
        else if ($(this).attr('type') == "checkbox") {
            data[fieldname] = $(this).prop('checked');
        }
        else
            data[fieldname] = $(this).val();
    });

    $('[data-objname]:eq(0)', container).parent();

    var parents = $('[data-objname]', $('[data-objname]:eq(0)', container).parent());
    for (var i = 0; i < parents.length; i++) {
        var node = parents[i];
        var objname = $(node).data('objname');
        var _obj = {};
        if (!data[objname])
            data[objname] = [];
        data[objname].push(_obj);
        DomDataToObj(node, _obj);
    }
    return data;
}


function DomDataToObj2(container, data) {
    //$(container).findExclude('input', '[data-objname]');
    $(container).findExclude('[name]', '[data-objname]').each(function () {
        var fieldname = $(this).attr('name');
        if ($(this).prop("tagName").toLowerCase() == "select" && $(this).attr('multiple') && $(this).val()) {
            data[$(this).data('selectobj')] = [];
            for (var i = 0; i < $(this).val().length; i++) {
                var _obj = {};
                _obj[fieldname] = $(this).val()[i];
                data[$(this).data('selectobj')].push(_obj);
            }
        }
        else if ($(this).attr('type') == "checkbox") {
            data[fieldname] = $(this).prop('checked');
        }
        else if ($(this).attr('type') == "radio") {
            var form = $(this).closest('form')[0];
            if (form) {
                data[fieldname] = $('[name=' + fieldname + ']:checked', form).val();
            }
            else
                data[fieldname] = $(this).prop('checked');
        }
        else
            data[fieldname] = $(this).val();
    });
    var gparents = [];
    var objnames = [];
    $('[data-objname]', container).each(function () {
        var objname = $(this).data('objname');
        if (objnames.indexOf(objname) == -1) {
            objnames.push(objname);
            gparents.push(this);
        }
    });
    for (var j = 0; j < gparents.length; j++) {
        var parents = $('[data-objname]', $(gparents[j]).parent());
        for (var i = 0; i < parents.length; i++) {
            var node = parents[i];
            var objname = $(node).data('objname');
            var _obj = {};
            if (!data[objname])
                data[objname] = [];
            data[objname].push(_obj);
            DomDataToObj2(node, _obj);
        }
    }
    return data;
}

function SubmitData(parent, url, data, maincallback) {
    if (!isFillRequired($(parent))) { return false; }
    var callback = function () {
        DomDataToObj(parent, data);
        if (activeid) { data["id"] = activeid; }
        $('.submitbt', parent).buttonLoader('start');
        XHRPOSTRequest(url, { data: data }, function (result) {
            $('.submitbt', parent).buttonLoader('stop');
            showNotification(result.Message, getEnumName(ResultStatus, result.Status));
            activeuserid = null;
            maincallback();
        });
    }
    callback();
    return false;
}


function replaceAll(str, find, replace) {
    return str.replace(new RegExp(find, 'g'), replace);
}
String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.replace(new RegExp(search, 'g'), replacement);
};

Number.prototype.todigits = function () {
    var tem = '', z, d, s = this.toString(),
        x = s.match(/^(\d+)\.(\d+)[eE]([-+]?)(\d+)$/);
    if (x) {
        d = x[2];
        z = (x[3] == '-') ? x[4] - 1 : x[4] - d.length;
        while (z--) tem += '0';
        if (x[3] == '-') {
            return '0.' + tem + x[1] + d;
        }
        return x[1] + d + tem;
    }
    return s;
}

function UploadFile(file, callback, element) {

    var mydata = new FormData();
    mydata.append('file', file);
    $.ajax({
        type: "POST",
        url: "/Common/UploadFile",
        data: mydata,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (response) {
            callback(response, element);
        },
        error: function (error) {
            console.log(JSON.stringify(error));
        }
    });
}
function UploadMultiFile(files, callback, element) {
    debugger
    var mydata = new FormData();
    for (var i = 0; i < files.length; i++) {
        mydata.append('files', files[i]);
    }
    
    $.ajax({
        type: "POST",
        url: "/Common/UploadMultiFile",
        data: mydata,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (response) {
            callback(response, element);
        },
        error: function (error) {
            console.log(JSON.stringify(error));
        }
    });
}
function RemoveFile(filename, callback) {

    var mydata = new FormData();
    mydata.append('filename', filename);
    $.ajax({
        type: "POST",
        url: "/Common/DeleteFile",
        data: mydata,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (response) {
            callback(response);
        },
        error: function (error) {
            console.log(JSON.stringify(error));
        }
    });
}
function DisplayAmount(val) {
    val = (val || 0) * 1;
    if (val < 0) {
        return "(" + CurrencyFormat((val * -1), 2) + ")";
    }
    else {
        return CurrencyFormat(val, 2);
    }
}


function initautoComplete(element, data) {
    $(element).autoComplete({
        minChars: 1,
        source: function (term, suggest) {
            term = term.toLowerCase();
            var choices = data;
            var suggestions = [];
            for (i = 0; i < choices.length; i++)
                if (~choices[i].toLowerCase().indexOf(term) && suggestions.length < 10) suggestions.push(choices[i]);
            suggest(suggestions);
        }
    });
}
function InitPopover() {
    $('[data-toggle]').popover({
        placement: 'bottom',
        html: 'true',
        title: '',
        container: 'body',
        content: function () {
            var content = $(this).attr("data-content");
            if (content) {
                return content.html();
            }
        },
    });


}
//$('body').on('click', function (e) {
//    //did not click a popover toggle or popover
//    if ($(e.target).data('toggle') !== 'popover'
//        && $(e.target).parents('.popover.in').length === 0) {
//        $('[data-toggle="popover"]').popover('hide');
//    }
//});

function GetDate(str, format) {
    if (!str) { return ""; }
    return moment(str.split('T')[0], 'YYYY-MM-DD').format(format);
}
function ShowButtonLoader(bt, text) {
    $(bt).attr('data-html', $(bt).html());
    $(bt).attr('disabled',true).html(`<i class="fa fa-spinner fa-spin"></i> <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>` + text);
}

function HideButtonLoader(bt) {
    $(bt).removeAttr('disabled').html($(bt).attr('data-html'));
}


function IsValidEmail(email) {
    var re = /^\w+([-+.'][^\s]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;

    return re.test(email);// this return result in boolean type


}
function PasswordValidator(password) {
    if (password.length < 6) {
        return false;
    }
    return true;
}

function isFillRequired(parentTelemnt, notinculdes) {
    var isfill = true;
    $(parentTelemnt).find('[required]').each(function (i) {

        if (notinculdes) {
            for (var i = 0; i < notinculdes.length; i++) {
                if ($(notinculdes[i]).get(0) == $(this).get(0)) {
                    continue;
                }
            }
        }
        if (!$(this).attr('disabled') && !$(this).val()) {
            var field_error = $(this).closest('.fieldpanel').find('.field-error');
            $(field_error).show().text(messages.required_field);
            isfill = false;
            return;
        }
    });
    return isfill;
}

function ResetFields(model) {
    $('input[type=number],input[type=hidden],input[type=text],input[type=password],select,textarea', model).val('');
 //   $('img[name]', model).attr('src', path.upload + path.defaultimage).removeAttr('data-filename');
    $('select', model).trigger('chosen:updated');
    $('input[type=checkbox]', model).prop('checked', false);
    $('.field-error', model).hide();
}

function getStatusDropDown(is_active) {
    var status = is_active ? "Active" : "Deactive";

    var obj = $('<div>' +
        '<div class="btn-group" >' +
        '<button data-toggle="dropdown" class="statusbt btn btn-primary btn-xs dropdown-toggle">Action <span class="caret"></span></button>' +
        '<ul class="dropdown-menu">' +
        '<li data-status="Active"><a onclick="return ActivateDeactiveUser(this,true)">Active</a></li>' +
        '<li data-status="Deactive"><a onclick="return ActivateDeactiveUser(this,false)">Deactive</a></li>' +
        '</ul>' +
        '</div>' +
        '</div >');
    $('.statusbt', obj).html(status);
    $('[data-status=' + status + ']', obj).addClass('active');
    return $(obj).html();
}
window.addEventListener("pageshow", function (event) {
    var historyTraversal = event.persisted ||
        (typeof window.performance != "undefined" &&
            window.performance.navigation.type === 2);
    if (historyTraversal) {
        // Handle page restore.
        
        window.location.reload();
    }
});

function FieldValidations(container) {
    var isvalid = true;
    $('[name][required]', container).each(function () {
        if (!$(this).val()) {
            $(this).closest('.form-group').find('.field-error').show();
            isvalid = false;
        }
    });
    return isvalid;
}

function IsEmailAvaliable(emails,callback) {
    var myurl = "/Common/IsEmailAvaliable";
    mydata = data;
    ShowAjaxLoader();
    XHRPOSTRequest(myurl, {
        emails: emails
    }, function (result) {
            callback(result);
            HideAjaxLoader();
    });
}

function isImage(filename) {
    if (!filename) { return false; }
    var ext = getExtension(filename);
    switch (ext.toLowerCase()) {
        case 'jpg':
        case 'gif':
        case 'bmp':
        case 'png':
        case 'jpeg':
            //etc
            return true;
    }
    return false;
}

function isPdf(filename) {
    if (!filename) { return false; }
    var ext = getExtension(filename);
    switch (ext.toLowerCase()) {
        case 'pdf':   
            //etc
            return true;
    }
    return false;
}
function isWord(filename) {
    if (!filename) { return false; }
    var ext = getExtension(filename);
    switch (ext.toLowerCase()) {
        case 'doc':
        case 'docx':
        case 'rtf':
            //etc
            return true;
    }
    return false;
}
function isVideo(filename) {
    if (!filename) { return false; }
    var ext = getExtension(filename);
    switch (ext.toLowerCase()) {
        case 'm4v':
        case 'avi':
        case 'mpg':
        case 'mp4':
            // etc
            return true;
    }
    return false;
}
function getExtension(filename) {
    var parts = filename.split('.');
    return parts[parts.length - 1];
}
function getFileThumbnail(filename) {
    if (isImage(filename)) {
        return '<a><div class="img-size"><img style="    max-width:100% !important;" src="/img/upload/' + filename + '"/></div></a>';
    }
    else if (isVideo(filename)) {
        return '<a class="fa fa-file-video-o" style="font-size: 109px;" href="/img/upload/' + filename + '" target="_blank"></a>';
    }
    else if (isPdf(filename)) {
        return '<a class="fa fa-file-pdf-o" style="font-size: 109px;" href="/img/upload/' + filename + '" target="_blank"></a>';
    }
    else if (isWord(filename)) {
        return '<a class="fa fa-file-word-o" style="font-size: 109px;" href="/img/upload/' + filename + '" target="_blank"></a>';
    }
    else {
        return '<a class="fa fa-file"  href="/img/upload/' + filename + '" target="_blank" style="font-size: 109px;"></a>';
    }
    
}

function RequiredFieldDailog() {

    swal({
        title: Words.PleaseFillReqiredFields,

        showCancelButton: false,
        confirmButtonColor: '#DD6B55',
        confirmButtonText: Words.OK,
        closeOnConfirm: true,
        closeOnCancel: false
    });
}
function InfoDailog(message) {

    swal({
        title: message,

        showCancelButton: false,
        confirmButtonColor: '#DD6B55',
        confirmButtonText: Words.OK,
        closeOnConfirm: true,
        closeOnCancel: false
    });
}


function FormatDate(d) {
    if (d) {
        return moment(d, 'YYYY-MM-DD').format('MM-DD-YYYY');
    }
    return null;
}

function FormatDateDisp(d) {
    if (d) {
        return moment(d, 'MM-DD-YYYY').format('YYYY-MM-DD');
    }
    return null;
}