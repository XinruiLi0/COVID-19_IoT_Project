
window.onload = function () {
    var userem = getCookie("userEmail");
    var username = getCookie("username");
    var userpass = getCookie("userPassword");
    var userrol = getCookie("userRole");
    $("#wel").html("Welcome the Guard User: " + userem);
    $("#thx").html("Dear " + username + " Thank you for your effort to keep everynody healthy!");
    $.ajax({
        type: "POST",
        dataType: "text",
        url: "/Home/getGuardDevices",
        data: { "userEmail": userem, "userPassword": userpass },
        success: function (data) {
            showList(data);
        },
        error: function (xhr, ts, et) {
            alert("Error");
        }
    });
}

function updateList() {
    var userem = getCookie("userEmail");
    var userpass = getCookie("userPassword");
    $.ajax({
        type: "POST",
        dataType: "text",
        url: "/Home/getGuardDevices",
        data: { "userEmail": userem, "userPassword": userpass },
        success: function (data) {
            showList(data);
        },
        error: function (xhr, ts, et) {
            alert("Error");
        }
    });
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

function jump(td) {
    var parentTR = td.parentNode.parentNode; // 获取当前 A 标签所在的TR
    var tem = $(parentTR).children('td').eq(1).text();
    createCookie("deviceid", tem);
    window.location.href = "/Home/Guard";
}

function showList(data) {
    var table = document.getElementById("list");
    var obj = JSON.parse(data);
    var butlink = '<input class="btn btn-primary"onClick="jump(this)" type="button" value="Device Info">'
    //alert(obj[0]); json数字开头
    var str = "";//把数据组装起来
    for (var i = 0; i < Object.keys(obj).length; i++) {
        var id = obj[i].DeviceID;
        var des = obj[i].Description;
        str += "<tr><td>" + (i + 1) +
            "</td><td>" + id +
            "</td><td>" + des +
            "</td><td>" + butlink + "</td></tr>";
    }
    var str2 = "<tr><th>No of Device</th><th>Device ID</th><th>Description</th><th>Device Link</th></tr>";
    var str3 = str2.concat(str);
    $("#list").html(str3);
}



function delAjax() {
    var userem = getCookie("userEmail");
    var userpass = getCookie("userPassword");
    var vem = $("#delid").val();
    if ($("#delid").val() == "") {
        alert("You leave Device Id empty. Please fill in the blank to delete device!");
        return;
    }
    $.ajax({
        type: "POST",
        dataType: "text",
        url: "/Home/deleteGuardDevice",
        data: { "userEmail": userem, "userPassword": userpass, "deviceID": vem },
        success: function (data) {
            alert(data);
            updateList();
        },
        error: function (xhr, ts, et) {
            alert("Error");
        }
    })
}

function RegAjax() {
    var userem = getCookie("userEmail");
    var userpass = getCookie("userPassword");
    var dev = $("#devid").val();
    var des = $("#descr").val();

    if (dev == "" || des == "") {
        alert("You leave desciption or device ID empty. Please fill in the blank to Register!");
        return;
    }
    $.ajax({
        type: "POST",
        dataType: "text",
        url: "/Home/registerGuardDevice",
        data: { "userEmail": userem, "userPassword": userpass, "deviceID": dev, "deviceDescription": des },
        success: function (data) {
            alert(data);
            updateList();
        },
        error: function (xhr, ts, et) {
            alert("Error");
        }
    })
}