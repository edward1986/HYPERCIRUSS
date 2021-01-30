function filterOffice() {

    var campus = document.getElementById("ddlCampus");

    var office = document.getElementById("ddlOffice");

    var selectedCampus = campus.options[campus.selectedIndex].value;

    office.value = -1;

    for (i = 0; i < office.length; i++) {

        if (office.options[i].value.split(",")[0] != selectedCampus) {

            if (office.options[i].value.split(",")[0] != -1) {
                office[i].style.display = "none";
            }

        }
        else {
            office[i].style.display = "block";
        }
    }
}

function filterDepartment() {

    var office = document.getElementById("ddlOffice");

    var department = document.getElementById("ddlDepartment");

    var selectedOffice = office.options[office.selectedIndex].value.split(",")[1];

    department.value = -1;
    for (i = 0; i < department.length; i++) {

        if (department.options[i].value.split(",")[0] != selectedOffice) {

            if (department.options[i].value.split(",")[0] != -1) {
                department[i].style.display = "none";

            }
        }
        else {
            department[i].style.display = "block";
        }
    }
}
