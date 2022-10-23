var activeReportUserId;
$(function () {
    $(document).on('click', '.multibtn button', function () {
        $(this).toggleClass('active');
        var container = $(this).closest('.multibtn');
        $('select', container).val('');

        var ids = [];
        $('button.active').each(function () {
            ids.push($(this).attr('data-id'));
        });
        $('select', container).val(ids);
    });
    $('.btlike').on('click', function () {
        var container = $(this).closest('.story-card');
        var id = $(container).attr('data-id');
        XHRPOSTRequest("/user/UpdateLike", { id: id }, function (result) {
            $(container).fadeOut();
            showNotification("Liked!", "success");
            //if (result.Status == ResultStatus.Success) {
            
            //}
            //else {
            //    showNotification(result, "error")
            //}
        });
    })
    $('.btdislike').on('click', function () {
        var container = $(this).closest('.story-card');
        var id = $(container).attr('data-id');
        XHRPOSTRequest("/user/UpdateDisLike", { id: id }, function (result) {
            $(container).fadeOut();
            showNotification("Disliked!", "success");
            //if (result.Status == ResultStatus.Success) {
            //    $(container).fadeOut();
            //    showNotification("Disliked!", "success");
            //}
            //else {
            //    showNotification(result, "error")
            //}
        });
    });
    $('.btsuperlike').on('click', function () {
        var container = $(this).closest('.story-card');
        var id = $(container).attr('data-id');
        XHRPOSTRequest("/user/UpdateSuperLike", { id: id }, function (result) {
            $(container).fadeOut();
            showNotification("SuperLiked!", "success");
            //if (result.Status == ResultStatus.Success) {
             
            //}
            //else {
            //    showNotification(result, "error")
            //}
        });
    });
    
    $('.btview').on('click', function () {
        var container = $(this).closest('.story-card');
        var id = $(container).attr('data-id');
        window.location.href = "/user/profile?id=" + id;
    });
    $(document).on('click', '.btreport', function () {
        var container = $(this).closest('.story-card');
        var id = $(container).attr('data-id');
        activeReportUserId = id;
        $('#reportusermodal').modal('show')
    })


});
function ReportUser(reason) {

    XHRPOSTRequest("/user/AddReportUser", { toUserId: activeReportUserId, reason: reason }, function (result) {

        $('#reportusermodal').modal('hide');
        $('.story-card[data-id="' + activeReportUserId + '"]').remove();
        showNotification("Reported User!", "success");
    });
}