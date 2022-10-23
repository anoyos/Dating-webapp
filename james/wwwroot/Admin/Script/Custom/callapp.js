
var activeCall = null;
var agoraAPPId = '8df2d5e17baf42a08a104d646603bcc3';
$(function () {

    $.extend({
        playSound: function () {
            return $(
                '<audio class="sound-player" loop="loop" autoplay="autoplay" style="display:none;">'
                + '<source src="' + arguments[0] + '" />'
                + '<embed src="' + arguments[0] + '" hidden="true" autostart="true" loop="false"/>'
                + '</audio>'
            ).appendTo('body');
        },
        stopSound: function () {
            $(".sound-player").remove();
        }
    });

    $(document).on('click', '.btdialcall', function () {
        if (AlreadyInCall()) {
            showNotification("Already In Call", "error");
            return;
        }
        var toUserId = $(this).attr('data-userid')
        var toUserName = $(this).attr('data-username');
        var photo = $(this).attr('data-photo');
        ResetCallApp();
        CallDial(toUserId, toUserName, photo, false);
    });
    $(document).on('click', '.btdialvideocall', function () {
        if (AlreadyInCall()) {
            showNotification("Already In Call", "error");
            return;
        }
        ResetCallApp();
        var toUserId = $(this).attr('data-userid')
        var toUserName = $(this).attr('data-username');
        var photo = $(this).attr('data-photo');

        CallDial(toUserId, toUserName, photo, true);
    });
    $(document).on('click', '.startcallbt', function () {
        stopplaying();
        CallAccepted();
    });
    $(document).on('click', '.endcallbt', function () {
        CallEnded();
    });
    $(document).on('click', '.rejectcallbt', function () {
        stopplaying();
        CallRejected();
    });

    $(document).on('click', '.mutebt', function () {

        if ($('.mutebt').hasClass('mute')) {
            $('.mutebt').addClass('unmute').removeClass('mute');
            $('.mutebt img').attr('src', '/Admin/icons/mic-btn.png');

            rtc.localAudioTrack.setMuted(false);
        }
        else {
            $('.mutebt').addClass('mute').removeClass('unmute');
            $('.mutebt img').attr('src', '/Admin/icons/mute-btn.png');
            rtc.localAudioTrack.setMuted(true)
        }
    });
});

function ResetCallApp() {
    $('.videoCall-to').show();
    //$('#videopanel').hide();
    $('#AudioCall,#videoCall').hide();
    $('.mutebt').addClass('unmute').removeClass('mute');
    $('.mutebt img').attr('src', '/Admin/icons/mic-btn.png');
    $('.endcallbt,.rejectcallbt,.startcallbt,.camerabt').hide();
    $('#videopanel').html('<div class="videoCall-to" ><img class="bor-10 videoCall-toimg" /></div><div class="videoCall-from"><img class="bor-10   videoCall-fromimg"  src="'+$('[name=userHiddenPhoto]').val()+'"  /></div>');
    try { rtc.localAudioTrack.setMuted(false); } catch (e) { }
}
function AlreadyInCall() {
    return activeCall ? true : false;
}
function CallDial(toUserId,toUsername,photo,isVideo) {
    $.ajax({
        url: "/api/default/CallDial?toUserId=" + toUserId + '&fromUserId=' + $('[name=userHiddenId]').val() + '&isVideo=' + isVideo,
        type: "post",
        async: true,
        cache: false
    }).done(function (data) {
        if (data.status == ResultStatus.Success) {
            debugger
            activeCall = { userId: toUserId, username: toUsername, photo: photo, token: data.data[0] };

            $('.calltoName').text(activeCall.username);
            $('.callphoto').attr('src', '/img/upload/' + (activeCall.photo ||'avatar.png'));
            $('.callstatus').text('Dialing');
            $('#callmodal').modal('show');
            if (isVideo) {
                $('#videoCall').show();
                $('.videoCall-toimg').attr('src', '/img/upload/' + (activeCall.photo || 'avatar.png'));
            }
            else {
                $('#AudioCall').show();
            }
            $('.mutebt,.camerabt,.startcallbt,.rejectcallbt').hide();
            $('.endcallbt').show();

        }
        else {
            showNotification(data.message, "error");
        }
    }).always(function () { });
}
function IamBusyForCall(payload) {
    $.ajax({
        url: "/api/default/CallIamBusy?callId=" + payload.agoraToken.callId + '&myId=' + $('[name=userHiddenId]').val(),
        type: "post",
        async: true,
        cache: false
    }).done(function (data) {
        if (data.status == ResultStatus.Success) {

        }
        else {
            showNotification(data.Message, "error");
        }
    }).always(function () { });
}

function CallRinging(payload) {
    $.ajax({
        url: "/api/default/CallRinging?callId=" + payload.agoraToken.callId + '&myId=' + $('[name=userHiddenId]').val(),
        type: "post",
        async: true,
        cache: false
    }).done(function (data) {
        if (data.status == ResultStatus.Success) {
            ResetCallApp()
            activeCall = { userId: payload.referenceId, username: payload.senderName, photo: payload.senderPhoto, token: payload.agoraToken };


            $('.calltoName').text(activeCall.username);
            $('.callphoto').attr('src', '/img/upload/' + (activeCall.photo || 'avatar.png'));
           // $('.callphoto').text('/img/upload/' + (activeCall.photo || 'avatar.png'));
            $('.callstatus').text('Incoming Call');
            $('#callmodal').modal('show');
            $('.mutebt,.camerabt,.endcallbt').hide();
            $('.startcallbt,.rejectcallbt').show();
            if (activeCall.token.isVideo) {
                $('#videoCall').show();
                $('.videoCall-toimg').attr('src', '/img/upload/' + (activeCall.photo || 'avatar.png'));
            }
            else {
                $('#AudioCall').show();
            }
        }
        else {
            showNotification(data.Message, "error");
        }
    }).always(function () { });
}
function CallAccepted() {
    $.ajax({
        url: "/api/default/CallAccepted?callId=" + activeCall.token.callId + '&myId=' + $('[name=userHiddenId]').val(),
        type: "post",
        async: true,
        cache: false
    }).done(function (data) {
        if (data.status == ResultStatus.Success) {
         //add in zoom meeting 
            AgoraStartCall();
        }
        else {
            showNotification(data.message, "error");
        }
    }).always(function () { });
}
function CallEnded() {
    $.ajax({
        url: "/api/default/CallEnded?callId=" + activeCall.token.callId + '&myId=' + $('[name=userHiddenId]').val(),
        type: "post",
        async: true,
        cache: false
    }).done(function (data) {
        if (data.status == ResultStatus.Success) {
            //add in zoom meeting 
            AgoraEndCall();
            $('#callmodal').modal('hide');
        }
        else {
            showNotification(data.message, "error");
        }
    }).always(function () { });
}
function CallRejected() {
    $.ajax({
        url: "/api/default/CallRejected?callId=" + activeCall.token.callId + '&myId=' + $('[name=userHiddenId]').val(),
        type: "post",
        async: true,
        cache: false
    }).done(function (data) {
        activeCall = null;
        $('#callmodal').modal('hide');
    }).always(function () { });
}

//////////agora calling section
;
let rtc = {
    localAudioTrack: null,
    client: null
};

let options = {
    // Pass your App ID here.
    appId: agoraAPPId,
    // Set the channel name.
    channel: "test",
    // Pass your temp token here.
    token: "Your temp token",
    // Set the user ID.
    uid: 123456
};

startBasicCall()


async function startBasicCall() {
    // Create an AgoraRTCClient object.
    rtc.client = AgoraRTC.createClient({ mode: "rtc", codec: "vp8" });

    // Listen for the "user-published" event, from which you can get an AgoraRTCRemoteUser object.
    rtc.client.on("user-published", async (user, mediaType) => {
        // Subscribe to the remote user when the SDK triggers the "user-published" event
        await rtc.client.subscribe(user, mediaType);
        console.log("subscribe success");

        // If the remote user publishes a video track.
        if (mediaType === "video") {
            // Get the RemoteVideoTrack object in the AgoraRTCRemoteUser object.
            const remoteVideoTrack = user.videoTrack;
            // Dynamically create a container in the form of a DIV element for playing the remote video track.
            const remotePlayerContainer = document.createElement("div");
            // Specify the ID of the DIV container. You can use the uid of the remote user.
            remotePlayerContainer.id = user.uid.toString();
            //remotePlayerContainer.textContent = "Remote user " + user.uid.toString();
            remotePlayerContainer.style.width = "100%";
            remotePlayerContainer.style.height = "100vh";
            $(remotePlayerContainer).addClass('remotevideo bor-10 videoCall-toimg');
            $('.videoCall-to').html(remotePlayerContainer);
            
            // Play the remote video track.
            // Pass the DIV container and the SDK dynamically creates a player in the container for playing the remote video track.
            remoteVideoTrack.play(remotePlayerContainer);
           // $('.videoCall-to').hide();
          //  $('#videopanel').show();f
        }
        else {
           // $('.videoCall-to').show();
         //   $('#videopanel').hide();
        }
        // If the remote user publishes an audio track.
        if (mediaType === "audio") {
            // Get the RemoteAudioTrack object in the AgoraRTCRemoteUser object.
            const remoteAudioTrack = user.audioTrack;
            // Play the remote audio track. No need to pass any DOM element.
            remoteAudioTrack.play();
        }

        // Listen for the "user-unpublished" event
        rtc.client.on("user-unpublished", user => {
            // Get the dynamically created DIV container.
            const remotePlayerContainer = document.getElementById(user.uid);
            // Destroy the container.
            remotePlayerContainer.remove();
        });
    });
}
async function AgoraStartCall() {

    await rtc.client.join(agoraAPPId, activeCall.token.channel, activeCall.token.token, $('[name=userHiddenId]').val());
    // Create a local audio track from the audio sampled by a microphone.
    rtc.localAudioTrack = await AgoraRTC.createMicrophoneAudioTrack();
    if (activeCall.token.isVideo) {
        // Create a local video track from the video captured by a camera.
        rtc.localVideoTrack = await AgoraRTC.createCameraVideoTrack();
        await rtc.client.publish([rtc.localAudioTrack, rtc.localVideoTrack]);
    }
    else {
        rtc.localVideoTrack = await AgoraRTC.createCameraVideoTrack();
        await rtc.client.publish([rtc.localAudioTrack]);
    }
    // Publish the local audio and video tracks to the RTC channel.


    if (activeCall.token.isVideo) {

        // Dynamically create a container in the form of a DIV element for playing the local video track.
        const localPlayerContainer = document.createElement("div");
        // Specify the ID of the DIV container. You can use the uid of the local user.
        localPlayerContainer.id = options.uid;
       // localPlayerContainer.textContent = "Local user " + options.uid;
        localPlayerContainer.style.width = "315px";
        localPlayerContainer.style.height = "236px";
        $('.videoCall-from').html(localPlayerContainer);
        $(localPlayerContainer).addClass('localvideo bor-10  videoCall-fromimg');

        // Play the local video track.
        // Pass the DIV container and the SDK dynamically creates a player in the container for playing the local video track.
        rtc.localVideoTrack.play(localPlayerContainer);
        

    }
    console.log("publish success!");
        $('.camerabt,.startcallbt,.rejectcallbt').hide();
    $('.mutebt,.endcallbt').show();
    $('.callstatus').text('Call Started');

}
async function AgoraEndCall() {
    try {
        rtc.localAudioTrack.close();
        if (activeCall.token.isVideo)
            rtc.localVideoTrack.close();

        // Traverse all remote users.
        rtc.client.remoteUsers.forEach(user => {
            // Destroy the dynamically created DIV containers.
            const playerContainer = document.getElementById(user.uid);
            playerContainer && playerContainer.remove();
        });

        // Leave the channel.
        await rtc.client.leave();
    }
    catch (e) {}
       activeCall = null;
}
//async function startBasicCall() {
//    // Create an AgoraRTCClient object.
//    rtc.client = AgoraRTC.createClient({ mode: "rtc", codec: "vp8" });

//    // Listen for the "user-published" event, from which you can get an AgoraRTCRemoteUser object.
//    rtc.client.on("user-published", async (user, mediaType) => {
//        // Subscribe to the remote user when the SDK triggers the "user-published" event
//        await rtc.client.subscribe(user, mediaType);
//        console.log("subscribe success");

//        // If the remote user publishes an audio track.
//        if (mediaType === "audio") {
//            // Get the RemoteAudioTrack object in the AgoraRTCRemoteUser object.
//            const remoteAudioTrack = user.audioTrack;
//            // Play the remote audio track.
//            remoteAudioTrack.play();
//        }

//        // Listen for the "user-unpublished" event
//        rtc.client.on("user-unpublished", async (user) => {
//            // Unsubscribe from the tracks of the remote user.
//            await rtc.client.unsubscribe(user);
//        });

//    });
//}
//async function AgoraStartCall() {

//    // Join an RTC channel.
//    await rtc.client.join(agoraAPPId, activeCall.token.channel, activeCall.token.token, $('[name=userHiddenId]').val());
//    // Create a local audio track from the audio sampled by a microphone.
//    rtc.localAudioTrack = await AgoraRTC.createMicrophoneAudioTrack();
//    // Publish the local audio tracks to the RTC channel.
//    await rtc.client.publish([rtc.localAudioTrack]);

//    $('.camerabt,.startcallbt,.rejectcallbt').hide();
//    $('.mutebt,.endcallbt').show();
//    $('.callstatus').text('Call Started');
//}
//async function AgoraEndCall() {
//    activeCall = null;
//    rtc.localAudioTrack.close();

//    // Leave the channel.
//    await rtc.client.leave();
//}
/////////agora calling section end


function startplaying(file) {
    try {
        $.playSound(file);
    }
    catch (e) {

    }
}
function stopplaying() {
    try {
        $.stopSound()
    }
    catch (e) {

    }
    //  var tone = document.getElementById("callTone");
    //tone.pause();
}