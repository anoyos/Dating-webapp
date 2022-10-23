var MessengerThreadId, MessengerUserId;
$(function () {
    //getMessagesList();

    $(document).on('keypress', ".messagetxt", function (event) {
        if (event.which == 13) {
            $('.btsendmessage').click();
        }
    });

    $(document).on('click', '.btsendmessage', function () {
        var message = $('.messagetxt').val();
        if (message) {
            debugger
            notificationsHub.invoke("sendPrivateMessage", $(".userHiddenId").val(), MessengerUserId, message, MessengerThreadId, 1);

            setTimeout(function () {
                $('.messagetxt').val('');
                _chatMessage();
                ChatThreads();
            }, 100);
        }
    });

    $(document).on('click', '.threadblock', function () {
        MessengerThreadId = $(this).attr('data-threadid');
        MessengerUserId = $(this).attr('data-userid');
        _chatMessage();
        $('.fullchat-chatpanel').removeClass('hide-mb');
        $('.fullchat-chatlistpanel').addClass('hide-mb');
    });


    $(document).on('click', '.btbackchat', function () {
        $('.fullchat-chatpanel').addClass('hide-mb');
        $('.fullchat-chatlistpanel').removeClass('hide-mb');
    })
    $(document).on('click', '.btmsgsearch', function () {
        ChatThreads();
    })
});
function _chatMessage() {
    $('.threadblock[data-userid="' + MessengerUserId + '"][data-threadid="' + MessengerThreadId + '"] .main-img-user span').text('0');
    $.ajax({
        url: "/messenger/_chatMessage?EmpId=" + MessengerUserId + "&threadId=" + MessengerThreadId,
        type: "get",
        dataType: 'html',
        async: true,
        cache: false
    }).done(function (data) {
        $('.ms-body').html(data);
        $('#ChatList .threadblock').removeClass('active');
        $('#ChatList [data-threadid=' + MessengerThreadId + '][data-userid=' + MessengerUserId + ']').addClass('active');
        $('[data-toggle="tooltip"]').tooltip();
        $('#ChatBody').scrollTop($('#ChatBody')[0].scrollHeight);
    }).always(function () {
    });
}
function ChatThreads() {
    $.ajax({
        url: "/messenger/ChatThreads?q=",
        type: "get",
        dataType: 'html',
        async: true,
        cache: false
    }).done(function (data) {
        $('#ChatList').html(data);
        $('#ChatList [data-threadid=' + MessengerThreadId + '][data-userid=' + MessengerUserId + ']').addClass('active');
    }).always(function () {
    });
}




$(document).on('click', '.chatfileuploadbtn', function () {
    $('.chatfileuploadfull').click();
});
$(document).on('change', '.chatfileuploadfull', function () {

    var parent = $(this).closest('.chatmessagebox');
    var file = $(this).get(0).files[0];
    if (file) {

        UploadFile(file, function (result, element) {

            notificationsHub.invoke("sendPrivateMessage", $(".userHiddenId").val(), MessengerUserId, result, MessengerThreadId, 2);

            setTimeout(function () {
                $('.messagetxt').val('');
                _chatMessage();
                ChatThreads();
            }, 100);
        }, null);
    }
});

function sendAudioMessage(audio) {
    XHRPOSTRequest("/user/sendMessage", { receipentRole: receipentRole, receiptId: receiptId, message: audio, messagetype: MessageType.VoiceMessage, additionalInfo: $('.rec-timer').text() }, function (result) {
        _chatMessage();
        ChatThreads();
    });
}
