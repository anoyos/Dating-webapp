var selection = {
    companyId: null,
    locationId: null,
    registrationType: null,
    categoryId: null,
    serviceId: null,
    professionalId: null,
    from: null,
    to: null,
    time: null,
    roomId: null,
    clientId: null,
    date: null,
};
$(function () {
    selection.companyId = getParameterByName('id');
    $("#client").click(function () {
        selection.registrationType = "new";
        $(this).closest(".box").hide();

        $(".locationspanel").fadeIn("slow");
        $(".booking_btn").hide();
        backButtonVisible();
    });
    $("#Reclient").click(function () {
        selection.registrationType = "returing";
        $(this).closest(".box").hide();

        $('.returing_client').fadeIn("slow");        
        $(".booking_btn").hide();
        backButtonVisible()

        //$(".locationspanel").fadeIn("slow");
    });
    $(document).on('click', '.locationitem', function () {
        $(this).closest(".box").hide();
        selection.locationId = $(this).attr('data-id');
        GetRooms();
        backButtonVisible()
        
        
    });
    $(document).on('click', '.roomitem', function () {
        $(this).closest(".box").hide();
        selection.roomId = $(this).attr('data-id');
        $(".servicepanel").fadeIn("slow");
        backButtonVisible()
    });
    
    $(document).on('click', '.serviceitem', function () {
        $(this).closest(".box").hide();
        selection.serviceId = $(this).attr('data-id');
        $(".professionalpanel").fadeIn("slow");
        GetProfessional(selection.serviceId);
        backButtonVisible()
    });
    $(document).on('click', '.professionalitem', function () {
        $(this).closest(".box").hide();
        selection.professionalId = $(this).attr('data-id');
        $(".appointment-sec").fadeIn("slow");
        GetSchedule();
        backButtonVisible()
    });

    $('.apptbackbt').on('click', function () {
        back();
    });
    $('.btsubmitreview').on('click', function () {
        var parent = $('.review-form');
        var d = {
            name: $('[name=name]', parent).val(),
            email: $('[name=email]', parent).val(),
            text: $('[name=text]', parent).val(),
            rating: $('#backing5').val(),
            companyId: selection.companyId
        };
        if (!d.name || !d.email) {
            showNotification(Words.EnterNameEmail, "warning");
            return;
        }
        XHRPOSTRequest("/home/addReview", { data: d }, function (result) {
            showNotification(Words.ReviewAdded, "success");
            reviews();
            var parent = $('.review-form');
            ('[name=name]', parent).val('');
            $('[name=email]', parent).val('');
            $('[name=text]', parent).val('')
        });


    });
   
});
function reviews() {
    $('.reviewpanel').html('<div class="tabloader"><i class="fas fa-spinner fa-spin"></i> '+Words.Loading+'...</div>');
    $.ajax({
        url: "/home/reviews" + "?id=" + selection.companyId,
        type: "get",
        dataType: 'html',
        async: true,
        cache: false
    }).done(function (data) {
        $('.reviewpanel').html(data);
    }).always(function () {
    });
}
function GetRooms() {

    XHRPOSTRequest("/home/GetRooms", { locationId: selection.locationId }, function (result) {
        $('.roomitempanel').html('')
        if (result.length) {
            for (var i = 0; i < result.length; i++) {
                var item = result[i];
                var obj = $('<div class="callout callout-success roomitem" data-id="' + item.id + '"> <h5>' + item.name + '</h5><p></p> </div>');
                $('.roomitempanel').append(obj);
            }
            var obj = $('<div class="callout callout-success roomitem"> <h5>' + Words.AnyRoom+'</h5><p></p> </div>');
            $('.roomitempanel').append(obj);
            $(".roomspanel").fadeIn("slow");
        }
        else {
            $(".servicepanel").fadeIn("slow");
        }
    });
}
function GetProfessional(id) {
    $('.professionalpanel').html('<div class="tabloader"><i class="fas fa-spinner fa-spin"></i> '+Words.Loading+'...</div>');
    $.ajax({
        url: "/home/GetProfessional" + "?serviceId=" + id,
        type: "get",
        dataType: 'html',
        async: true,
        cache: false
    }).done(function (data) {
        $('.professionalpanel').html(data);
    }).always(function () {
    });
}
function PreWeekSched() {
    selection.from = moment(selection.from, "MM/DD/YYYY").add(-6, 'days').format("MM/DD/YYYY");
    selection.to = moment(selection.to, "MM/DD/YYYY").add(-6, 'days').format("MM/DD/YYYY");
    GetSchedule();
}
function NextWeekSched() {
    selection.from = moment(selection.from, "MM/DD/YYYY").add(6, 'days').format("MM/DD/YYYY");
    selection.to = moment(selection.to, "MM/DD/YYYY").add(6, 'days').format("MM/DD/YYYY");
    GetSchedule();
}
function GetSchedule() {
    $('.schedulepanel').html('<div class="tabloader"><i class="fas fa-spinner fa-spin"></i> '+Words.Loading+'...</div>');
    $.ajax({
        url: "/home/GetSchedule" + "?empId=" + selection.professionalId + "&locationId=" + selection.locationId + "&roomId=" + selection.roomId + "&serviceId=" + selection.serviceId + "&from=" + selection.from + "&to=" + selection.to,
        type: "get",
        dataType: 'html',
        async: true,
        cache: false
    }).done(function (data) {
        $('.schedulepanel').html(data);
        selection.from = $('#from').val();
        selection.to = $('#to').val();
    }).always(function () {
    });
}
function SelectTime(time,date) {
    selection.time = time;
    selection.date = date;
    $('.appointment-sec').hide();
    if (selection.registrationType == "new") {       
        $(".new_client").fadeIn("slow");
    }
    else {
        $(".summarypanel").fadeIn("slow");
        AppSelection();
        //$(".returing_client").fadeIn("slow");
    }
    backButtonVisible()
}
$('[name=new_type]').on('change', function () {
    var val = $(this).val();
    if (val =='dependent') {
        $('.new_client .dependentpanel').show();
    }
    else if (val == 'my') {
        $('.new_client .dependentpanel').hide();
    }
});

$('.btnreturningsubmit').on('click', function () {
    var parent = $('.returing_client');
    var data = {
        fname: $('[name=fname]', parent).val(),
        lname: $('[name=lname]', parent).val(),
        email: $('[name=email]', parent).val(),
        companyId: getParameterByName('id'),
    };
    if (!data.fname || !data.lname || !data.email) {
        return;
    }

    XHRPOSTRequest("/home/ClientLogin", data, function (result) {
        if (result.status == 'error') {
            showNotification(result.message, "error");
            return;
        }
        else {
            selection.clientId = result.data.id;
            showNotification(Words.Loginsuccessfully, "success");
            $('.returing_client').hide();
            $(".locationspanel").fadeIn("slow");
            backButtonVisible();
        }

    });
});

$('.btnnewsubmit').on('click', function () {
    var parent = $('.new_client');
    var data = {
        companyId: selection.companyId,
        FirstName: $('[name=FirstName]', parent).val(),
        LastName: $('[name=LastName]', parent).val(),
        email: $('[name=email]', parent).val(),
        mobileNumber: $('[name=mobileNumber]', parent).val(),
        homenumber: $('[name=homenumber]', parent).val(),
        isDependent: $('[name=new_type][value="dependent"]', parent).prop('checked'),
        D_FirstName: $('[name=D_FirstName]', parent).val(),
        D_LastName: $('[name=D_LastName]', parent).val(),
        D_dob: $('[name=D_dob]', parent).val(),
        D_gender: $('[name=D_gender]', parent).val(),
    };
    if (!data.FirstName || !data.LastName || !data.email) {
        return;
    }
    XHRPOSTRequest("/home/AddClientLogin", data, function (result) {
        if (result.status == 'error') {
            showNotification(result.message, "error");
            return;
        }
        else {
            selection.clientId = result.data.id;
            showNotification(Words.Registeredsuccessfully, "success");
            $('.new_client').hide();
            $(".summarypanel").fadeIn("slow");
            AppSelection();
            backButtonVisible()
        }

    });

    //ClientLogin
});

function AppSelection() {
    $('.summarypanel').html('<div class="tabloader"><i class="fas fa-spinner fa-spin"></i> '+Words.Loading+'...</div>');
    $.ajax({
        url: "/home/AppSelection",
        type: "get",
        dataType: 'html',
        data: selection,
        async: true,
        cache: false
    }).done(function (data) {
        $('.summarypanel').html(data);

    }).always(function () {
    });
}

function back() {
    //booking_btn
    if ($('.locationspanel:visible').length) {
        if (selection.registrationType == "new") {
            $('.locationspanel').hide();
            $(".booking_btn").fadeIn("slow");
        }
        else {
            $('.locationspanel').hide();
            $(".booking_btn").fadeIn("slow");
        }
    }
    else if ($('.roomspanel:visible').length) {
        $('.roomspanel').hide();
        $(".locationspanel").fadeIn("slow");
    }

    else if ($('.servicepanel:visible').length) {
        $('.servicepanel').hide();
        $(".roomspanel").fadeIn("slow");
    }

    else if ($('.professionalpanel:visible').length) {
        $('.professionalpanel').hide();
        $(".servicepanel").fadeIn("slow");
    }

  
    else if ($('.appointment-sec:visible').length) {
        $('.appointment-sec').hide();
        $(".professionalpanel").fadeIn("slow");
    }

    else if ($('.new_client:visible').length) {
        $('.new_client').hide();
        $(".appointment-sec").fadeIn("slow");
    }


    else if ($('.returing_client:visible').length) {
        $('.returing_client').hide();
        $(".booking_btn").fadeIn("slow");
    }
    backButtonVisible();
    
}
function backButtonVisible() {
    
    setTimeout(function () {
        if (!$('.booking_btn:visible').length) {
            $('.apptbackbt').show();
        }
        else {
            $('.apptbackbt').hide();
        }
    }, 500)
}
function SaveAppt() {
    XHRPOSTRequest("/home/SaveAppt", { data: selection }, function(result) {
        if (result.status) {
            $('.summarypanel').hide();
            $('.apptbackbt').hide();
            $(".messagepanel").fadeIn("slow");
        }
        else {
            showNotification(result.message, "error");
        }
    });
}
