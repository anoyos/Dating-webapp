$(function () {
    $('.fc-datepicker').datepicker({
        showOtherMonths: true,
        selectOtherMonths: true
    });
    $('#calender').click(function () {
        jQuery('#diarydatepicker').datepicker("show");
        $("selectedDate").text($("#diarydatepicker").val());
    });

    $('#diarydatepicker').on('change', function () {
        var fdate = moment($('#diarydatepicker').val()).format('DD MMM, YYYY')
        $('#selectedDate').text(fdate);
    });

    $('.btattach').on('click', function () {
        $('#fup').click();
    });
    $('#fup').on('change', function () {
        for (var i = 0; i < $(this).get(0).files.length; i++) {
            var file = $(this).get(0).files[i];
            if (file) {

                UploadFile(file, function (result, element) {
                    $('.imgpanel').html('<img class="diaryimg" data-src="' + result + '" src="/img/upload/' + result + '" />');
                    $('.imgpanel').show();
                });
            }
        }
        $(this).val('');
    });

    $('.btcancel').on('click', function () {
        window.location.reload();
    });
    
 
});
function OnSubmitDiary() {
    var data = DomDataToObj('#addDiaryFrm', {});
    data.dateTime = $('#diarydatepicker').val();
    data.photo = $('.diaryimg').attr('data-src');
    if (!data.photo) {
        showNotification("Picture Required", "error");
        return false;
    }
    if (!data.dateTime) {
        showNotification("Date Required", "error");
        return false;
    }
    XHRPOSTRequest("/User/SaveDiary", { data: data }, function (result) {
        if (result.Status == ResultStatus.Success) {
            window.location.reload();
        }
        else {
            showNotification(result, "error")
        }
    });
    return false;
}