function Launcher_Launch() {
    $(".alert").hide();
    setInterval(
        function () {
            $.ajax({
                url: "/Launcher?Handler=LoginStatus",
                type: "GET",
                success: function (data) {
                    let pw = data["pgfc"] / data["ttfc"];
                    $("#prog-whole").css({ width: (pw * 100) + "%" }).attr("aria-valuenow", pw);
                    $("#prog-part").css({ width: data["prog"] + "%" }).attr("aria-valuenow", data["prog"]);
                    $("#prog-kind").text(data["kind"]);
                    $("#prog-name").text(data["name"]);
                    $("#prog-pgfc").text(data["pgfc"]);
                    $("#prog-ttfc").text(data["ttfc"]);
                    if (data["success"] == "true") {
                        location.reload();
                    }
                }
            });
        },
    1000); 
}

function Launcher_Download() {
    $(".alert").hide();
    setInterval(
        function () {
            $.ajax({
                url: "/Launcher?Handler=DownloadStatus",
                type: "GET",
                success: function (data) {
                    let pw = data["pgfc"] / data["ttfc"];
                    $("#prog-whole").css({ width: (pw * 100) + "%" }).attr("aria-valuenow", pw);
                    $("#prog-part").css({ width: data["prog"] + "%" }).attr("aria-valuenow", data["prog"]);
                    $("#prog-kind").text(data["kind"]);
                    $("#prog-name").text(data["name"]);
                    $("#prog-pgfc").text(data["pgfc"]);
                    $("#prog-ttfc").text(data["ttfc"]);
                    if (data["success"] == "true") {
                        location.reload();
                    }
                }
            });
        },
    1000);    
}
