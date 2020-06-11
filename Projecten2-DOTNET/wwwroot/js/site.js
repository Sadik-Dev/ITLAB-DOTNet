window.onload = function () {

    var selectBox = document.getElementsByClassName("selectBox")[0];
    var filterBox = document.getElementsByClassName("selectBox")[1];

    var firstOption = document.querySelector(".sorteerSessies .selectBox a:nth-child(1)");
    var filterButton = document.getElementsByClassName("filterTittle")[0];

    selectBox.onclick = filter;


    //filter
    var submitFilter = document.getElementById("submitFilter");
    var resetFilter = document.getElementById("resetFilter");

    if (typeof filterButton !== 'undefined') {
        filterButton.onclick = showFilterBox;
        submitFilter.onclick = filterPagina;
        resetFilter.onclick = resetPagina;
        if (window.outerWidth < 1300) {
            var filterBoxItem = document.getElementsByClassName("filterContent")[0];
            filterBoxItem.style.height = "0px";
        }
        //Date Picker
        picker = new Lightpick({
            field: document.getElementById('datepicker'),
            singleDate: false,
            autoclose: false,
            onSelect: function (start, end) {
                var str = '';
                str += start ? start.format('Do MMMM YYYY') + ' to ' : '';
                str += end ? end.format('Do MMMM YYYY') : '...';
                if (picker.getEndDate() !== null) {
                    console.log(picker.getStartDate().format("YYYY-MM-DD") + " " + picker.getStartDate().format("YYYY-MM-DD"));
                    //  filterPagina(picker.getStartDate().format("YYYY-MM-DD").toString() + " 00:00 AM", picker.getEndDate().format("YYYY-MM-DD").toString() + " 00:00 AM");
                    vanDatum = picker.getStartDate().format("YYYY-MM-DD").toString() + " 00:00 AM";
                    totDatum = picker.getEndDate().format("YYYY-MM-DD").toString() + " 00:00 AM";

                }
            }
        });
    }
    //


  


    //Sessies OnClick
    var sessies =  document.getElementsByClassName("sessie");
    for (let i = 0; i < sessies.length; i++) {
        sessies[i].addEventListener('click', function () {

            if (sessies[i].getAttribute("data-clapped") === "false") {
                sessies[i].style.height = "70vh";
                sessies[i].setAttribute("data-clapped", "true");
                sessies[i].firstElementChild.style.height = "50%"

            }
            else {
                sessies[i].style.height = "25vh";
                sessies[i].setAttribute("data-clapped", "false");
                sessies[i].firstElementChild.style.height = "100%"

            }

        });
    }


    //Alerts na 3 seconden weg doen

    var alerts = document.getElementsByClassName("alert");
    for (let i = 0; i < alerts.length; i++) {
        setInterval(function () {
            alerts[i].style.display = "none";
        }, 5000);
    }

    //Sessies tonen die geopend kunnen worden -- NavBar
    var navItem = document.getElementsByClassName("verSS")[0];
    if (document.getElementsByClassName("verSS")[0].getAttribute("data-url") === "kalender")
        navItem.onclick = openBeheerSessie;
    else
        navItem.onclick = openKalender;
    

}
var picker;
var vanDatum;
var totDatum;

var filterBoxOpen = false;
var sorteerBoxOpen = false;


function openBeheerSessie() {
    if (window.location.pathname === "/")
        window.open('/BeheerSessie/', "_self");
    else window.open('/BeheerSessie/Index', "_self");


}

function openKalender() {
    if (window.location.pathname === "/")
        window.open('/Overzicht/Index', "_self");
    else window.open('/Overzicht/Index', "_self");


}


function filterPagina() {
    if (window.location.pathname === "/")
        window.open('/Overzicht/Filter/?vanS=' + vanDatum + '&totS=' + totDatum, "_self");
    else window.open('/Overzicht/Filter' + '/?vanS=' + vanDatum + ' &totS=' + totDatum, "_self");
   
    
   


}

function redirectToUrl(url) {
    window.open(url, "_blank");
}

function resetPagina() {
    if (window.location.pathname === "/")
        window.open('/Overzicht/Index', "_self");
    else window.open('/Overzicht/Index' , "_self");




}
function showFilterBox() {
    var filterIcon = document.querySelectorAll(".filterTittle img")[0];
    var title = document.querySelectorAll(".filterTittle")[0];
    if (!filterBoxOpen) {
        var filterBoxItem = document.getElementsByClassName("filterContent")[0];
        filterBoxItem.style.height = "150px";
        var filterButton = document.getElementsByClassName("filterTittle")[0];
        filterButton.style.height = "10px";
        filterIcon.style.display = "none";
        //     title.style.background = "black";

        if (window.outerWidth < 1300) {
            var filterBoxItem = document.getElementsByClassName("filterContent")[0];
            filterBoxItem.style.overflow = "visible";
        }
        filterBoxOpen = true;
    }
    else {
        var filterBoxItem = document.getElementsByClassName("filterContent")[0];
        filterBoxItem.style.height = "0px";
        var filterButton = document.getElementsByClassName("filterTittle")[0];
        filterButton.style.height = "38px";
        filterIcon.style.display = "block";
        if (window.outerWidth < 1300) {
            var filterBoxItem = document.getElementsByClassName("filterContent")[0];
            filterBoxItem.style.overflow = "hidden";
        }
        filterBoxOpen = false;

    }

}
function secondOptionClicked() {
    var par = secondOption.getAttribute("data");
    window.open("~/Overzicht/index/" + par);

}
function thirdOptionClicked() {
    var par = thirdOption.getAttribute("data");
    window.open("~/Overzicht/index/" + par);

}



function filter() {
    var selectBox = document.getElementsByClassName("selectBox")[0];

    if (!sorteerBoxOpen) {
        // selectBox.style.display = "none";
        //   selectBox.style.display = "flex";


        selectBox.style.transitionDuration = "0.5s";
        selectBox.style.height = "160px";



        sorteerBoxOpen = true;
    }
    else {
        selectBox.style.transitionDuration = "0.5s";

        selectBox.style.height = "46px";

        sorteerBoxOpen = false;

    }


}




