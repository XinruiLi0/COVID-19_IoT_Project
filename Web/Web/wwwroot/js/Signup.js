$(document).ready(function () {
    $(".btn-success").on("click", function () {
        $(".first").text("Hello Doctor, Please fill in the form below to start register.");
        $(".doctor").attr("style", "display:block;");
        $(".guard").attr("style", "display:none;");
        $(".user").attr("style", "display:none;");
    });
    $(".btn-primary").on("click", function () {
        $(".first").text("Hello User, Please fill in the form below to start register.");
        $(".doctor").attr("style", "display:none;");
        $(".guard").attr("style", "display:none;");
        $(".user").attr("style", "display:block;");
    });
    $(".btn-warning").on("click", function () {
        $(".first").text("Hello Guard, Please fill in the form below to start register.");
        $(".doctor").attr("style", "display:none;");
        $(".guard").attr("style", "display:block;");
        $(".user").attr("style", "display:none;");
    });
});

function DoctorAjax() {
    if ($("#exampleInputEmail1").val() == "" || $("#password1").val() == "" || $("#dname").val() == "") {
        alert("You leave email or password or username empty. Please fill in the blank to Register!");
        return;
    }
    if (($("#conpassword1").val()) != ($("#password1").val())) {
        alert("Your two password mismatch! Please do again!");
        return;
    }

    $.ajax({
        type: "POST",
        dataType: "text",
        url: "/Login/userRegister",
        data: { "userName": $("#dname").val(), "userEmail": $("#exampleInputEmail1").val(), "userPassword": $("#password1").val(), "userRole": 3 },
        success: function (data) {
            alert(data);
            window.location.href = "/Login/Signup";
        },
        error: function (xhr, ts, et) {
            alert("Error");
        }
    })
};

function GuardAjax() {
    if ($("#exampleInputEmail2").val() == "" || $("#password2").val() == "" || $("#gname").val() == "") {
        alert("You leave email or password or username empty. Please fill in the blank to Register!");
        return;
    }

    if (($("#conpassword2").val()) != ($("#password2").val())) {
        alert("Your two password mismatch! Please do again!");
        return;
    }

    $.ajax({
        type: "POST",
        dataType: "text",
        url: "/Login/userRegister",
        data: { "userName": $("#gname").val(), "userEmail": $("#exampleInputEmail2").val(), "userPassword": $("#password2").val(), "userRole": 2 },
        success: function (data) {
            alert(data);
            window.location.href = "/Login/Signup";
        },
        error: function (xhr, ts, et) {
            alert("Error");
        }
    })
};

function UserAjax() {
    if ($("#exampleInputEmail3").val() == "" || $("#password3").val() == "" || $("#uname").val() == "") {
        alert("You leave email or password or username empty. Please fill in the blank to Register!");
        return;
    }
    if (($("#conpassword3").val()) != ($("#password3").val())) {
        alert("Your two password mismatch! Please do again!");
        return;
    }

    $.ajax({
        type: "POST",
        dataType: "text",
        url: "/Login/userRegister",
        data: { "userName": $("#uname").val(), "userEmail": $("#exampleInputEmail3").val(), "userPassword": $("#password3").val(), "userRole": 1 },
        success: function (data) {
            alert(data);
            window.location.href = "/Login/Signup";
        },
        error: function (xhr, ts, et) {
            alert("Error");
        }
    })
};

function getLocation() {
    var x;
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition(showPosition);
    }
    else { x = "Geolocation is not supported by this browser."; }

    return x;
}

function showPosition(position) {
    var pos = new Array(position.coords.latitude, position.coords.longitude);
    alert(pos);
    GuardAjax();
}