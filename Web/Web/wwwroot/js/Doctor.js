
var userEmail;
var userName;
var userStatus;

function getCookie(c_name) {
    if (document.cookie.length > 0) {
        c_start = document.cookie.indexOf(c_name + "=");
        if (c_start != -1) {
            c_start = c_start + c_name.length + 1;
            c_end = document.cookie.indexOf(";", c_start);
            if (c_end == -1) {
                c_end = document.cookie.length;
            }
            return unescape(document.cookie.substring(c_start, c_end));
        }
    }
    return "";
}

window.onload = function () {
    var getUserName = getCookie("username");
    $("#getUserName").html(getUserName);

    $('#searchBtn').click(function () {
        var getUserEmail = getCookie("userEmail");
        var getUserPassword = getCookie("userPassword");

        if ($('#searchBox').val() != '') {
            $.ajax({
                type: "POST",
                dataType: "text",
                url: "/Home/checkPatientStatus",
                data: { "userEmail": getUserEmail, "userPassword": getUserPassword, "visitorEmail": $('#searchBox').val() },
                success: function (data) {
                    userName = JSON.parse(data).UserName;
                    userEmail = JSON.parse(data).UserEmail;
                    userStatus = JSON.parse(data).UserStatus;
                    $('#vistorName').html(userName);
                    $('#vistorEmail').html(userEmail);
                    if (userStatus == "0") {
                        $('#vistorHealthStatus').html("Healthy");
                    } else if (userStatus == "1") {
                        $('#vistorHealthStatus').html("Infected");
                    }

                }
            });

        } else {
            $('#vistorName').html("");
            $('#vistorEmail').html("");
            $('#vistorHealthStatus').html("");
        }
    });
}

function update() {
    var getUserEmail = getCookie("userEmail");
    var getUserPassword = getCookie("userPassword");

    if ($('#searchBox').val() != '') {
        $.ajax({
            type: "POST",
            dataType: "text",
            url: "/Home/checkPatientStatus",
            data: { "userEmail": getUserEmail, "userPassword": getUserPassword, "visitorEmail": $('#searchBox').val() },
            success: function (data) {
                userName = JSON.parse(data).UserName;
                userEmail = JSON.parse(data).UserEmail;
                userStatus = JSON.parse(data).UserStatus;
                $('#vistorName').html(userName);
                $('#vistorEmail').html(userEmail);
                if (userStatus == "0") {
                    $('#vistorHealthStatus').html("Healthy");
                } else if (userStatus == "1") {
                    $('#vistorHealthStatus').html("Infected");
                }
            }
        });

    } else {
        $('#vistorName').html("");
        $('#vistorEmail').html("");
        $('#vistorHealthStatus').html("");
    }
}

function healthy() {
    var getUserEmail = getCookie("userEmail");
    var getUserPassword = getCookie("userPassword");
    if (userName != "" && userEmail != "") {
        $.ajax({
            type: "POST",
            dataType: "text",
            url: "/Home/updatePatientStatus",
            data: { "userEmail": getUserEmail, "userPassword": getUserPassword, "visitorEmail": userEmail, "status": 0 },
            success: function (data) {
                alert(data);
                update();
            }
        });
    }
}

function infected() {
    var getUserEmail = getCookie("userEmail");
    var getUserPassword = getCookie("userPassword");
    if (userName != "" && userEmail != "") {
        $.ajax({
            type: "POST",
            dataType: "text",
            url: "/Home/updatePatientStatus",
            data: { "userEmail": getUserEmail, "userPassword": getUserPassword, "visitorEmail": userEmail, "status": 1 },
            success: function (data) {
                alert(data);
                update();
            }
        });
    }
}

function eraseCookie(name) {
    var date = new Date();
    date.setTime(date.getTime() - 10000);
    document.cookie = name + "=v; expire=" + date.toGMTString() + "; path=/";
}

function signOut() {
    eraseCookie("userEmail");
    eraseCookie("userPassword");
    eraseCookie("username");
    window.location.href = "/Login/Login";
}
