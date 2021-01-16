
function FormAjax() {
    if ($("#email").val() == "" || $("#password").val() == "") {
        alert("You leave email or password empty. Please fill in the blank to login!");
        return;
    }
    var rolenum = getRole();
    if (rolenum == 0) {
        alert("Your role is not defined yet. Please choose your role!");
        return;
    }
    else if (rolenum == 3) {
        $.ajax({
            type: "POST",
            dataType: "text",
            url: "/Login/userLogin",
            data: { "userEmail": $("#email").val(), "userPassword": $("#password").val(), "userRole": rolenum },
            success: function (data) {
                var res = eval("(" + data + ")");
                if (res.result == "success") {
                    createCookie("userEmail", $("#email").val());
                    createCookie("userPassword", $("#password").val());
                    createCookie("userRole", 3);
                    createCookie("username", res.message);
                    window.location.href = "/Home/Doctor";
                }
                else {
                    alert(res.message);
                    window.location.href = "/Login/Login";
                }
            },
            error: function (xhr, ts, et) {
                alert("Error");
            }
        })
    }
    else if (rolenum == 2) {
        $.ajax({
            type: "POST",
            dataType: "text",
            url: "/Login/userLogin",
            data: { "userEmail": $("#email").val(), "userPassword": $("#password").val(), "userRole": rolenum },
            success: function (data) {
                var res = eval("(" + data + ")");
                if (res.result == "success") {
                    createCookie("userEmail", $("#email").val());
                    createCookie("userPassword", $("#password").val());
                    createCookie("userRole", 2);
                    createCookie("username", res.message);
                    window.location.href = "/Home/GuardList";
                }
                else {
                    alert(res.message);
                    window.location.href = "/Login/Login";
                }
            },
            error: function (xhr, ts, et) {
                alert("Error");
            }
        })
    }
    else if (rolenum == 1) {
        $.ajax({
            type: "POST",
            dataType: "text",
            url: "/Login/userLogin",
            data: { "userEmail": $("#email").val(), "userPassword": $("#password").val(), "userRole": rolenum },
            success: function (data) {
                var res = eval("(" + data + ")");
                if (res.result == "success") {
                    createCookie("userEmail", $("#email").val());
                    createCookie("userPassword", $("#password").val());
                    createCookie("userRole", 1);
                    createCookie("username", res.message);
                    window.location.href = "/Home/User";
                }
                else {
                    alert(res.message);
                    window.location.href = "/Login/Login";
                }
            },
            error: function (xhr, ts, et) {
                alert("Error");
            }
        })
    }
};

function getRole() {
    var rolenum = 0;
    if ($("#radio1").is(":checked")) {
        rolenum = 3;
    }
    else if ($("#radio2").is(":checked")) {
        rolenum = 2;
    }
    else if ($("#radio3").is(":checked")) {
        rolenum = 1;
    }
    else {
        return rolenum;
    }
    return rolenum;
}

function createCookie(name, value, days) {
    var expires;
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toGMTString();
    }
    else {
        expires = "";
    }
    document.cookie = name + "=" + value + expires + "; path=/";
}