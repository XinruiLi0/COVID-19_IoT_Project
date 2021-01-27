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
        url: "/Login/doctorRegister",
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

function UserAjax() {
    if ($("#exampleInputEmail3").val() == "" || $("#password3").val() == "" || $("#uname").val() == "" || $("#age").val() == "") {
        alert("You leave email or password or username or age empty. Please fill in the blank to Register!");
        return;
    }
    if (($("#conpassword3").val()) != ($("#password3").val())) {
        alert("Your two password mismatch! Please do again!");
        return;
    }

    var inf;
    if ($("#radio1").prop("checked", true)) {
        inf = 0;
    }
    if ($("#radio2").prop("checked", true)) {
        inf = 1;
    }
    $.ajax({
        type: "POST",
        dataType: "text",
        url: "/Login/userRegister",
        data: { "userName": $("#uname").val(), "userEmail": $("#exampleInputEmail3").val(), "userPassword": $("#password3").val(), "age": $("#age").val(), "hasInfectedBefore": inf },
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
    var geocoder = new google.maps.Geocoder();
    var address = document.getElementById("Address").value;
    if ($("#exampleInputEmail2").val() == "" || $("#password2").val() == "" || $("#gname").val() == "") {
        alert("You leave email or password or username empty. Please fill in the blank to Register!");
        return;
    }

    if (($("#conpassword2").val()) != ($("#password2").val())) {
        alert("Your two password mismatch! Please do again!");
        return;
    }
    geocoder.geocode({ 'address': address }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            var loca = [];
            loca[0] = results[0].geometry.location.lat();
            loca[1] = results[0].geometry.location.lng();          
            $.ajax({
                type: "POST",
                dataType: "text",
                url: "/Login/guardRegister",
                data: { "guardName": $("#gname").val(), "guardEmail": $("#exampleInputEmail2").val(), "guardPassword": $("#password2").val(), "address": address, "latitude": loca[0], "longitude": loca[1] },
                success: function (data) {
                    alert(data);
                    window.location.href = "/Login/Signup";
                },
                error: function (xhr, ts, et) {
                    alert("Error");
                }
            })
        } else {
            alert("Request failed. Please input valid address!!!!!!!!!")
            return;
        }
    });
};
