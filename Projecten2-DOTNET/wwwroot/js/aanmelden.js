window.onload = function () {
    //Open Sessie beeingdigen dialog
    var eindig = document.getElementsByClassName("stopSessie")[0];
    eindig.onclick = openDialog;
    //Sessie beeindigen dialog
    var neeButton = document.querySelectorAll('.botDialog p')[0];
    neeButton.onclick = closedialog;
    var jaButton = document.querySelectorAll('.botDialog p')[1];
    jaButton.onclick = closeSessie;

    //Alerts na 15 seconden weg doen

    var alerts = document.getElementsByClassName("alert");
    for (let i = 0; i < alerts.length; i++) {
        setInterval(function () {
            alerts[i].style.display = "none";
        }, 5000);
    }

    //Input Barcode call method at length 13

    var sessieId;
    var barcode;

    document.getElementById("barcode").addEventListener('input', function (evt) {
        // automatisch aanmelden
        if (this.value.length == 13) { // lengte van de barcode
            barcode = document.getElementById('barcode');
            sessieId = barcode.getAttribute("data-sessie");

            window.open('/beheersessie/meldaanvoorsessie' + '/?sessieid=' + sessieId + ' &barcode=' + barcode.value, "_self");
        }
    });

    document.getElementById("meldAanButton").addEventListener("click", function (evt) {
        barcode = document.getElementById('barcode');
        sessieId = barcode.getAttribute("data-sessie");
        window.open('/beheersessie/meldaanvoorsessie' + '/?sessieid=' + sessieId + ' &barcode=' + barcode.value, "_self");
    });
}

function closedialog() {
    document.getElementsByClassName("hidePage")[0].style.display = "none";
}

function openDialog() {
    document.getElementsByClassName("hidePage")[0].style.display = "flex";
}

function closeSessie() {

}

 