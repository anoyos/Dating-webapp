var notificationsHub;
var isEditorOpen = false;
$(function () {

    notificationsHub = new signalR.HubConnectionBuilder().withUrl("/chatr").build();
    registerClientMethods(notificationsHub);

    //   Start Hub
    console.log('signalr start');

    notificationsHub.start()
        .then(function () {
            registerEvents(notificationsHub)
        }).catch(function (err) {
            return console.error(err.toString());
        });
});



function registerEvents(notificationsHub) {
    //  if ($("#username").val()) {
    notificationsHub.invoke("connect", $("#username").val());
    notificationsHub.invoke("connectChat", $(".userHiddenId").val());
}
function registerClientMethods(notificationsHub) {

    // Calls when user successfully logged in


    notificationsHub.on("onConnected", function (id, userName, allUsers, messages) {
        
    });

    // On User Disconnected
    notificationsHub.on("onUserDisconnected", function (empId) {

        //$('.user-list-item').each(function () {
        //    if ($(this).attr('data-userid') == empId * 1) {
        //        $(this).find('.profile-status').removeClass('online');

        //    }
        //});
    });



    notificationsHub.on("onConnectedChat", function (id, userName, allUsers, messages) {

        //for (var i = 0; i < allUsers.length; i++) {
        //    $('.user-list-item').each(function () {

        //        if ($(this).attr('data-userid') == allUsers[i].EmpId * 1) {
        //            $(this).find('.profile-status').addClass('online');

        //        }
        //    });
        //}
    });
    notificationsHub.on("onNewUserConnected", function (id, name) {
  
    });

    notificationsHub.on("PrivateMessageSendByUser", function (windowId, fromUserId, message, MessageType, _temp, Photo) {
        _chatMessage();
        ChatThreads();
    });

    notificationsHub.on("ReceiveCall", function (payload) {
        payload = JSON.parse(payload);
        console.log('call', payload);
        if (AlreadyInCall()) {
            IamBusyForCall(payload);
        }
        else {
            startplaying('https://automaticlot.com/Content/RingTone/incoming_call.mp3');
            CallRinging(payload)
        }


        
    });

    notificationsHub.on("RingingCall", function (payload) {
        payload = JSON.parse(payload);
        console.log('call', payload);

        $('.callstatus').text('Ringing Call');
    });

    notificationsHub.on("UserBusy", function (payload) {
        payload = JSON.parse(payload);
        console.log('call', payload);
        showNotification("User Busy", "error");
        $('#callmodal').modal('hide');
       
    });

    notificationsHub.on("Accepted", function (payload) {
        payload = JSON.parse(payload);
        console.log('call', payload);
        AgoraStartCall();
        //start call

    });

    notificationsHub.on("Rejected", function (payload) {
        payload = JSON.parse(payload);
        console.log('call', payload);

      
        showNotification("Call Rejected", "error");
        $('#callmodal').modal('hide');

    });

    notificationsHub.on("CallEnded", function (payload) {
        payload = JSON.parse(payload);
        console.log('call', payload);

        AgoraEndCall();
        showNotification("Call Ended", "error");
        $('#callmodal').modal('hide');
    });

    notificationsHub.on("MatchFound", function (payload) {
        debugger
        payload = JSON.parse(payload);
        console.log('MatchFound', payload);

        $('#matchmodal').modal('show');
        $('.btmatchhello').attr('data-matchid', payload.referenceId);
        $('.match2img').attr('src', '/img/upload/' + (payload.senderPhoto || 'avatar.png'));
    });
    
}


$(document).on('click', '.btmatchhello', function () {
    window.location.href = "/messenger/index?id=" + $(this).attr('data-matchid');
    
});
