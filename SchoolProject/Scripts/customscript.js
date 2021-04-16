window.onload = hideAllFields;

function hideAllFields() {
    if (document.getElementById("salary_div")) { document.getElementById("salary_div").style.display = "none"; }
    if (document.getElementById("hiring_div")) { document.getElementById("hiring_div").style.display = "none"; }
    if (document.getElementById("employeeno_div")) { document.getElementById("employeeno_div").style.display = "none"; }
    if (document.getElementById("enrolling_div")) { document.getElementById("enrolling_div").style.display = "none"; }
    if (document.getElementById("studentno_div")) { document.getElementById("studentno_div").style.display = "none"; }
    if (document.getElementById("duration_div")) { document.getElementById("duration_div").style.display = "none"; }
    if (document.getElementById("classcode_div")) { document.getElementById("classcode_div").style.display = "none"; }
    if (document.getElementById("employeeName_div")) { document.getElementById("employeeName_div").style.display = "none"; }
}


function toggle_fields(id) {
    var ele = document.getElementById(id);
    if (ele.style.display === "none") {
        ele.style.display = "block";
    } else {
        ele.style.display = "none";
    }
}

