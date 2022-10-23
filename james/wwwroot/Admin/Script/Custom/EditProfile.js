var activeAlbumId;
$(function () {
    GetAlbums();
    $(document).on('click', '.multibtn button', function () {
        $(this).toggleClass('active');

    
    });
    $(document).on('click', '.singlebtn button', function () {
        var container = $(this).closest('.singlebtn');
        $('button', container).removeClass('active');
        $(this).addClass('active');
    });

    $('.btprofileimg').on('click', function () {
        $('#profileimgfup').click();
    })
    $('#profileimgfup').on('change', function () {
        for (var i = 0; i < $(this).get(0).files.length; i++) {
            var file = $(this).get(0).files[i];
            if (file) {

                UploadFile(file, function (result, element) {
                    $('.profileimg').attr('src', '/img/upload/' + result).attr('data-src', result);
                });
            }
        }
        $(this).val('');
    })

    $('.btsave').on('click', function () {
        var data = DomDataToObj('.datacontainer', {});
        data.photo = $('.profileimg').attr('data-src');
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

        console.log(data);
        XHRPOSTRequest("/user/updateprofile", { data: data }, function (result) {
            if (result.Status == ResultStatus.Success) {
                window.location.replace("/user/index");
            }
            else {
                showNotification(result, "error")
            }
        });
        return false;
    });


    $('.btaddalbum').on('click', function () {
        $('#albumModal').modal('show');
        $('[name=albumName]').val('')
    })
    $(document).on('click', '.btaddalbumimg', function () {
        $('#albumimgfup').click();
        activeAlbumId = $(this).closest('[data-albumid]').attr('data-albumid');
    })
    $('#albumimgfup').on('change', function () {
        var files = [];
        for (var i = 0; i < $(this).get(0).files.length; i++) {
            var file = $(this).get(0).files[i];
            if (file) {
                files.push(file);
            }
        }
        UploadMultiFile(files, function (result, element) {
            SaveAlbumPhoto(result);
        });
        
        $(this).val('');
    });
    $(document).on('change', '[name=isPrivate]', function () {
      
        activeAlbumId = $(this).closest('[data-albumid]').attr('data-albumid');        
        XHRPOSTRequest("/user/UpdateAlbumIsPrivate", { id: activeAlbumId,isPrivate:$(this).prop('checked') }, function (result) {
            if (result.Status == ResultStatus.Success) {
                GetAlbums();
            }
            else {
                showNotification(result, "error")
            }
        });
    })
    $(document).on('click', '.btprivatealbummodal', function () {

        activeAlbumId = $(this).closest('[data-albumid]').attr('data-albumid');
        XHRPOSTRequest("/user/GenerateAlbumCode", { id: activeAlbumId }, function (result) {
            if (result.Status == ResultStatus.Success) {
                $('[name=hiddenalbumcode]').val(result.Data);
                $('#privatealbummodal').modal('show');
            }
            else {
                showNotification(result, "error")
            }
        });
    });
    $(document).on('click', '.btcopycode', function () {
        var code = $('[name=hiddenalbumcode]').val();
        navigator.clipboard.writeText(code);
        $('#privatealbummodal').modal('hide');
    });
    

    
    
});
function SaveAlbumPhoto(photos) {
    var list = [];
    for (var i = 0; i < photos.length; i++) {
        var photo = photos[i];
        list.push({ albumId: activeAlbumId, photo: photo });
    }
    XHRPOSTRequest("/user/SaveAlbumPhoto", { list: list }, function (result) {
        if (result.Status == ResultStatus.Success) {
            GetAlbums();
        }
        else {
            showNotification(result, "error")
        }
    });
    return false;
}
function OnSubmitAlbum() {
    XHRPOSTRequest("/user/AddAlbum", { name: $('[name=albumName]').val() }, function (result) {
        if (result.Status == ResultStatus.Success) {
            $('#albumModal').modal('hide');
            GetAlbums();
        }
        else {
            showNotification(result, "error")
        }
    });
    return false;
}
function GetAlbums() {
    $.ajax({
        url: "/user/GetAlbums",
        type: "get",
        dataType: 'html',
        async: true,
        cache: false
    }).done(function (data) {
        $('.albumpanel').html(data);
    }).always(function () { });
}




