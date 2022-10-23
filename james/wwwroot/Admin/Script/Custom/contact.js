var activeId;
$(function () {
    // $('.ddldepartments').chosen();
    $('.addbt').on('click', function () {
        ResetFields('#AddModal');
        activeId = 0;
        $('#AddModal').modal('show');
    });
    $(document).on('click', '.editbt', function () {
        var container = $('#AddModal');
        ResetFields('#AddModal');
        var id = $(this).closest('tr').attr('data-id') * 1;
        activeId = id;
        var data = _.filter(Model.contacts, function (o) { return o.id == id })[0];
        ObjToDomData(data, container);
        $('#AddModal').modal('show');
      
    });

    $(document).on('click', '.deletebt', function () {

        var id = $(this).closest('tr').attr('data-id');
        activeId = id;
        swal({
            title: Words.AreYouSure,
            text: Words.DeleteMessage,
            confirmButtonText: Words.DeleteConfirmation,
            cancelButtonText: Words.Cancel,
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            closeOnConfirm: false
        }, function (isConfirm) {

            if (isConfirm) {
                var myurl = "/Home/Delete";
                var mydata = new Object();
                mydata.id = activeId;
                XHRPOSTRequest(myurl, mydata, function (result) {

                    swal({
                        title: Words.Deleted,
                        text: Words.DeleteSuccess,
                        type: "success",
                        showCancelButton: false,
                        confirmButtonColor: '#DD6B55',
                        confirmButtonText: Words.OK,
                        closeOnConfirm: true,
                        closeOnCancel: false
                    });
                    setTimeout(function () { window.location.reload() }, 1000);
                });
            }
            $(".sweet-alert .sa-button-container .sa-custom-check").remove();
        });
    });



    $('.savebt').on('click', function () {
        var container = $('#AddModal');

        if (!isFillRequired(container)) {
            showNotification(Words.PleaseFillRequiredFields, "error");
            return;
        }
      
        ShowAjaxLoader();
        var data = DomDataToObj(container, { id: activeId || 0, departments: [] });
       
        XHRPOSTRequest("/Home/AddUpdate", { data: data }, function (result) {
            if (result == 'success') {
                showNotification("Saved", "success");
                setTimeout(function () { window.location.reload() }, 1000);
            }
            else {
                showNotification(result, "error");
                HideAjaxLoader();
            }
        });
    });
})
