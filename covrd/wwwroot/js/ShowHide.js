function Collapse() {
    var value = $('#<%=ddlptype.ClientID%>').val();
    if (value === "General") {
        var t = document.getElementById("Organisation");
        $("#Organisation").fadeOut("slow");
    }
    else
        $("#Organisation").fadeIn("slow");
}

//$('.div .documentView').on('show.bs.collapse', function () {
//    $(this).closest("table")
//        .find(".collapse.in")
//        .not(this)
//        .collapse('toggle');
//})