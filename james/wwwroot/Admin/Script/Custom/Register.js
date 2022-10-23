$(document).ready(function () {
    $(".divs .iit").each(function (e) {
        if (e != 0)
            $(this).hide();
    });

    $(".nextt").click(function () {

        var container = $(this).closest('.iit');
        if ($(container).attr('id') == "step1") {
            var data = {
                username: $('[name=username]').val(),
                name: $('[name=name]').val(),
                email: $('[name=email]').val(),
                password: $('[name=password]').val(),
            };
            if (!data.username || !data.name || !data.email || !data.password) {
                showNotification("Fill Up Required Fields", "error");
                return false;
            }
            XHRPOSTRequest("/home/CheckUserAvailability", data, function (result) {
                debugger;
                if (result.Status == ResultStatus.Success) {
                    moveTabToNext();
                }
                else {
                    showNotification(result, "error")
                }
            });
        }
        else {
            moveTabToNext();
        }
        return false;
    });

    $(".prevv").click(function () {
        if ($(".divs .iit:visible").prev().length != 0)
            $(".divs .iit:visible").prev().show().next().hide();
        else {
            $(".divs .iit:visible").hide();
            $(".divs .iit:last").show();
        }
        return false;
    });

    $(document).on('click', '.multibtn button', function () {
        $(this).toggleClass('active');
    });
    $(document).on('click', '.singlebtn button', function () {
        var container = $(this).closest('.singlebtn');
        $('button', container).removeClass('active');
        $(this).addClass('active');
    });


    $('.imgbt').on('click', function () {
        $('#imgfup').click();
    })
    $('#imgfup').on('change', function () {
        for (var i = 0; i < $(this).get(0).files.length; i++) {
            var file = $(this).get(0).files[i];
            if (file) {
             
                UploadFile(file, function (result, element) {
                    $('.imgbt').attr('src', '/img/upload/' + result).attr('data-filename', result);
                });
            }
        }
        $(this).val('');
    })

});
function moveTabToNext() {
    if ($(".divs .iit:visible").next().length != 0)
        $(".divs .iit:visible").next().show().prev().hide();
    else {
        $(".divs .iit:visible").hide();
        $(".divs .iit:first").show();
    }
}
$('.btsave').on('click', function () {
    var data = DomDataToObj('.datacontainer', {});
    data.photo = $('.imgbt').attr('data-filename');
    $('.multibtn').each(function () {
        var objName = $(this).attr('data-name');
        var fieldName = $(this).attr('data-field');
        data[objName] = [];
        $('button.active', this).each(function () {
            var obj = {};
            obj[fieldName] = $(this).attr('data-id');
            data[objName].push(obj);
        });
    });
    $('.singlebtn').each(function () {
        var objName = $(this).attr('data-name');
        data[objName] = $('button.active', this).attr('data-id');
    });
    console.log(data);
    XHRPOSTRequest("/home/SaveRegister", {data:data}, function (result) {
        if (result.Status == ResultStatus.Success) {
            window.location.replace("/user/index");
        }
        else {
            showNotification(result, "error")
        }
    });
    return false;
})
