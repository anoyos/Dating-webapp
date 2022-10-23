//Default State
var isSupported = false;

//Helper Methods
function getProtocol() {
    return urlval;// $('#protocol').val();
}

function getUrl() {
    return getProtocol();
}

function result() {
    if (isSupported) {        
    }
    else {
        //alert('Ask Admin For Player');
    }
    // alert(getProtocol() + " supported => " + isSupported);
}
var urlval;
//Handle Click on Launch button
$(document).on('click', '.launch',function () {
    urlval = $(this).attr('data-videoenckey');
    launchChrome();
    return;
    if ($.browser.mozilla) {
        launchMozilla();
    } else if ($.browser.chrome) {
        launchChrome();
    } else if ($.browser.msie) {
        launchIE();
    }
});

//Handle IE
function launchIE() {

    var url = getUrl(),
        aLink = $('#hiddenLink')[0];

    isSupported = false;
    aLink.href = url;

    //Case 1: protcolLong
    console.log("Case 1");
    if (navigator.appName == "Microsoft Internet Explorer"
        && aLink.protocolLong == "Unknown Protocol") {
        isSupported = false;
        result();
        return;
    }

    //Case2: Open New Window, set iframe src, and access the location.href
    console.log("Case 2");
    var myWindow = window.open('', '', 'width=0,height=0');
    myWindow.document.write("<iframe src='" + url + "></iframe>");
    setTimeout(function () {
        try {
            myWindow.location.href;
            isSupported = true;
        } catch (e) {
            //Handle Exception
        }

        if (isSupported) {
            myWindow.setTimeout('window.close()', 100);
        } else {
            myWindow.close();
        }
        result();
    }, 100)

};

//Handle Firefox
function launchMozilla() {

    var url = getUrl(),
        iFrame = $('#hiddenIframe')[0];

    isSupported = false;

    //Set iframe.src and handle exception
    try {
        iFrame.contentWindow.location.href = url;
        isSupported = true;
        result();
    } catch (e) {
        //FireFox
        if (e.name == "NS_ERROR_UNKNOWN_PROTOCOL") {
            isSupported = false;
            result();
        }
    }
}

//Handle Chrome
function launchChrome() {

    var url = getUrl(),
        protcolEl = $('#protocol')[0];

    isSupported = false;


    protcolEl.focus();
    protcolEl.onblur = function () {
        isSupported = true;
        console.log("Text Field onblur called");
    };

    //will trigger onblur
    location.href = url;

    setTimeout(function () {
        protcolEl.onblur = null;
        result()
    }, 300);

}