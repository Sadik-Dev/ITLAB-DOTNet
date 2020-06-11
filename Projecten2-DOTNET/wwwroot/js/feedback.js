var ratingChosed = 0;

window.onload = function () {
var ratingExist = false;
//Rating hover

var stars = document.querySelectorAll(".rating img");
    for(let i = 0; i < stars.length; i++){
        stars[i].addEventListener('mouseover', function(){
                var starId = stars[i].getAttribute("data");
                for(let x = 0; x < starId; x++){
                   stars[x].src = "/images/starY.png";
                }
        });

        for(let i = 0; i < stars.length; i++){
            stars[i].addEventListener('mouseout', function(){

                if(ratingExist){
                    for(let x = 0; x < stars.length; x++){
                        stars[x].src = "/images/starB.png";
                     }

                    for(let x = 0; x < ratingChosed; x++){
                        stars[x].src = "/images/starY.png";
                     }
                }
                else{
                    for(let x = 0; x < stars.length; x++){
                        stars[x].src = "/images/starB.png";
                     }
                }
              
                });
            }

    }

//--

//Rating Clicked
for(let i = 0; i < stars.length; i++){
    stars[i].addEventListener('click', function(){
            var starId = stars[i].getAttribute("data");
            //Reset all stars
            for(let x = 0; x < stars.length; x++){
                stars[x].src = "/images/starB.png";
             }

            for(let x = 0; x < starId; x++){
               stars[x].src = "/images/starY.png";
            }
            ratingChosed = stars[i].getAttribute("data");
            ratingExist = true;

            console.log(ratingChosed);
    });


    }

    //Alerts na 5 seconden weg doen

    var alerts = document.getElementsByClassName("alert");
    for (let i = 0; i < alerts.length; i++) {
        setInterval(function () {
            alerts[i].style.display = "none";
        }, 5000);
    }
    //Submit new Feedback
    var button = document.getElementById("sumbitKnop");
    button.onclick = newFeedback;

    //Sessies tonen die geopend kunnen worden -- NavBar
    var navItem = document.getElementsByClassName("verSS")[0];
    navItem.onclick = openKalender;
    

}

function newFeedback() {
    var sessieId = document.getElementById("sumbitKnop").getAttribute("data-sessie");
    var inhoud = document.getElementById("textarea").value;
    score = ratingChosed;

    if (window.location.pathname === "/")
        window.open('/FeedBack/maakNieuweFeedbackEntry/?sessieId=' + sessieId + '&inhoud=' + inhoud + '&score=' + score, "_self");
    else window.open('/FeedBack/maakNieuweFeedbackEntry/?sessieId=' + sessieId + '&inhoud=' + inhoud + '&score=' + score, "_self");

}

function openKalender() {
    if (window.location.pathname === "/")
        window.open('/Overzicht/Index', "_self");
    else window.open('/Overzicht/Index', "_self");


}
