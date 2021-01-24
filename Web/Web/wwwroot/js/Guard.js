
window.onload = function () {
    var userem = getCookie("userEmail");
    var username = getCookie("username");
    $("#wel").html("Welcome the Guard User: " + userem);
    $("#thx").html("Dear " + username + " Thank you for your effort to keep everynody healthy!")
    getLocation();
    var device = getCookie("deviceid");

    setInterval(function () {
        $.ajax({
            type: "POST",
            dataType: "text",
            url: "/Home/visitorInfoCheck",
            data: { "deviceID": device },
            success: function (data) {
                var obj = JSON.parse(data);
                var usernm = obj.UserName;
                var updateTime = obj.LastUpdated;
                var temp = obj.VisitorTemperature;
                var sta = obj.UserStatus;
                if (updateTime > 30) {
                    $("#lastVisitor").html("Recently no visitor came in " + updateTime + " seconds!");
                }
                else if (temp == 0) {
                    var str1 = "Visitor temporaliy coming through this device is called " + userem;
                    var str2 = "<br> Visitor Temperature is testing!";
                    var str3 = "<br> Visitor time came through device in " + updateTime + " seconds";
                    $("#lastVisitor").html(str1 + str2 + str4 + str3);
                }

                else if (updateTime <= 30) {
                    var sta2 = sta == "0" ? "Healthy" : "Infected";
                    var str1 = "Last Visitor coming through this device is called " + userem;
                    var str2 = "<br> Visitor Temperature is " + temp;
                    var str4 = "<br> Visitor health Status is " + sta2;
                    var str3 = "<br> Visitor already go through the device in " + updateTime + " seconds";
                    $("#lastVisitor").html(str1 + str2 + str4 + str3);
                }
             

            },
            error: function (xhr, ts, et) {
                alert("Error");
            }
        })
    }, 3000)
}

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

function eraseCookie(name) {
    var date = new Date();
    date.setTime(date.getTime() - 10000);
    document.cookie = name + "=; expire=" + date.toGMTString() + "; path=/";
}

function signOut() {
    eraseCookie("userEmail");
    eraseCookie("userPassword");
    eraseCookie("username");
    window.location.href = '/Login/Login';
}

function getLocation() {
    var x = document.getElementById("demo");
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition);
    }
    else { x.innerHTML = "Geolocation is not supported by this browser."; }
}
function showPosition(position) {
    var x = document.getElementById("demo");
    x.innerHTML = "Latitude: " + position.coords.latitude +
        "<br />Longitude: " + position.coords.longitude;
}