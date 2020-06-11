window.onload = function () {

    var submitFilter = document.getElementById("submitFilter");

    submitFilter.onclick = filterPagina;


}

function filterPagina() {

    var url = "Overzicht/Test";
    window.location.href = url;

}