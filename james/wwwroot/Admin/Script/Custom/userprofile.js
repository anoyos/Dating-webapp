

$("[name=Password]").keyup(function () {


    CheckPasswordStrength($(this).val());

})

$("[name=Password]").change(function () {


    let oldPassword = $("[name=oldPassword]").val();
    let newPassword = $(this).val();

    PasswordCheck(oldPassword, newPassword);

    //CheckPasswordStrength($(this).val());

})

function PasswordCheck(oldPassword, newPassword) {
    if (oldPassword && newPassword && oldPassword == newPassword) {
        showNotification("Old password and new password can't be same", "error");
        return false;
    }
    else {
        return true;
    }
}

$(".changePasswordbtn").click(function () {


    if (!FieldValidations($(".passwordPanel"))) { return; }

    data = {
        oldPassword: $('[name="oldPassword"]').val(),
        Password: $('[name="Password"]').val(),
        cPassword: $('[name="cPassword"]').val(),

    }
    if (!data.Password || !data.cPassword) {
        showNotification("Please fill password and re-password", "error");
        return false;
    }
    if (!$("#oldPasswordCheck").val()) {
        showNotification("Incorrect Old Password", "error");
        return;
    }

    if (!PasswordCheck(data.oldPassword, data.Password)) return;

    debugger
    //let password_strength = $("#password_strength").html();


    //if ((password_strength == "Weak" || password_strength == "Good")) {
    //    showNotification("Please enter strong password!", "error");
    //    return

    //}

    if (data.Password.length < 6) {
        showNotification("Password length must be 6 or greater than 6.!", "error");
        return
    }


    if (data.Password != data.cPassword) {
        showNotification("Password do not match", "error");
        return false;
    }

    XHRPOSTRequest("/User/updatePassword", data, function (result) {
        if (result == "Success") {
            showNotification("Password Changed", "success")
            window.location.reload();
        }
        else {
            showNotification(result, "error")
        }
    })
})
function CheckPasswordStrength(password) {
    var password_strength = document.getElementById("password_strength");

    //TextBox left blank.
    if (password.length == 0) {
        password_strength.innerHTML = "";
        return;
    }

    //Regular Expressions.
    var regex = new Array();
    regex.push("[A-Z]"); //Uppercase Alphabet.
    regex.push("[a-z]"); //Lowercase Alphabet.
    regex.push("[0-9]"); //Digit.
    regex.push("[$@@$!%*#?&]"); //Special Character.

    var passed = 0;

    //Validate for each Regular Expression.
    for (var i = 0; i < regex.length; i++) {
        if (new RegExp(regex[i]).test(password)) {
            passed++;
        }
    }

    //Validate for length of Password.
    if (password.length > 8) {
        passed++;
    }

    //Display status.
    var color = "";
    var strength = "";
    switch (passed) {
        case 0:
        case 1:
            strength = "Weak";
            color = "red";
            break;
        case 2:
            strength = "Good";
            color = "darkorange";
            break;
        case 3:
        case 4:
            strength = "Strong";
            color = "green";
            break;
        case 5:
            strength = "Very Strong";
            color = "darkgreen";
            break;
    }
    password_strength.innerHTML = strength;
    password_strength.style.color = color;
}


$("[name=oldPassword]").change(function () {


    let password = $(this).val();

    if (!password) return false;

    XHRGETRequest("/User/checkPassword", { oldPassword: password }, function (result) {
        debugger
        if (result == "Error") {
            $(".oldPasswordLabel").show()
            showNotification("Incorrect Old Password ", "error");
            $("#oldPasswordCheck").val('');

        }
        else if (result == "Success") {
            //$("#oldPasswordCheck").val('true');

            $(".oldPasswordLabel").hide()
            let oldPassword = $("[name=oldPassword]").val();
            let newPassword = $("[name=Password]").val();

            if (oldPassword && newPassword && oldPassword == newPassword) {
                showNotification("Old password and new password can't be same", "error")
            }
        }
        else {
            showNotification("Invalid User", "error")
        }
    })

})
