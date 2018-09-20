var card = $($.parseHTML(`
          <div class="card my-2 card-group card-shadow">
            <div class="thumbnail">
                <div class="date">
                  <div class="day">27</div>
                  <div class="month">Mar</div>
                </div>
                <a href="#">
                <img class="card-img-top" src="">
                </a>
            </div>
            <div class="card-body card-group-body">
              <div class="category"></div>
              <h4 class="card-title"></h4>
              <a href="#">
              <h5 class="card-subtitle"></h5>
              </a>
              <div class="card-text"></div>
              <div class="post-meta">
                <span class="users mr-2" title="prijavljenih osoba"><i class="fa fa-users"></i>&nbsp;</span>
                <span class="timestamp mr-2" title="časova"><i class="fa fa-clock-o">&nbsp;</i></span>
                <span class="price mr-2" title="cijena"><i class="fa fa-credit-card">&nbsp;</i></span>

              </div>
            </div>
          </div>
`));
var draw = 0;

$(document).ready(function (parameters) {

});

var $parallaxDiv = $("#parallax");
//NavBar transparency
var scrollPosition = 0;
$(window).scroll(function () {
  var scrollPositionNow = $(this).scrollTop();
  if (scrollPositionNow < 35 || scrollPositionNow < scrollPosition)  /*height in pixels when the navbar becomes non opaque*/ {
    $('.navbar-transparent').addClass('bg-dark');
  } else {
    $('.navbar-transparent').removeClass('bg-dark');
  }
  scrollPosition = scrollPositionNow;
  //paralax ypos
  var parallaxYposOffset = 300;
  $parallaxDiv.css("background-position", "center " + (-(scrollPositionNow - parallaxYposOffset) * 0.3) + "px");
});


function GroupsAjaxCall(grupeDivId, kategorijaId = null, mode = "append", status = "aktivna") {

  var grupeDiv = $(grupeDivId);
  const length = grupeDiv.attr("length");
  //if (!grupeDiv.attr("currentItemCount")) {
  //  grupeDiv.attr("currentItemCount", 0);
  //}
  //var currentItemCount = parseInt(grupeDiv.attr("currentItemCount"));
  var currentItemCount = 0;

  if (mode === "append") {
    currentItemCount = grupeDiv.children(".card").length;
  }

  $.ajax({
    url: "/Grupa/GetGrupeCardListItem",
    type: "GET",
    dataType: "json",
    data: { kategorijaId, draw: draw++, start: currentItemCount, length, status }
  })
    .done(function (data, status, xhr) {
      if (xhr.status === 200) {
        if (typeof data !== 'undefined' && data != null) {
          if (data.draw === draw - 1) {
            if (data.data != null && data.data.length > 0) {
              GenerateListGroupCard(data.data, grupeDiv, mode);
            } else if (data.data.length === 0 && mode === "replace") {
              grupeDiv.html("");
            }
          }
        }
      }

    });
}
function GenerateGroupCard(grupa) {
  var url = "/Grupa/Details/";

  var cloneCard = card.clone();
  var thumbnail = cloneCard.find(".thumbnail");
  //date
  var date = DateTransform(grupa.pocetak);
  thumbnail.find(".day").html(date.getDate());
  thumbnail.find(".month").html(date.toLocaleString("BA", { month: "short" }));
  //slika
  var img = thumbnail.find("img");
  if (grupa.slika) {
    img.attr("src", "data:image;base64," + grupa.slika);
  } else {
    img.attr("src", grupa.slikaUrl);
  }
  //url
  thumbnail.find("a").attr("href", url + grupa.grupaId);

  if (grupa.kraj) {
    grupa.pocetak += ` - ${grupa.kraj}`;
  }

  var cardBody = cloneCard.find(".card-body");
  cardBody.find(".category").html(grupa.centarLokacija + " - " + grupa.kursKategorijaNaziv);



  cardBody.find(".card-text").html($.parseHTML(`
      <dl class="row">
        <dt class="col-sm-4">Predavač:</dt>
        <dd class="col-sm-8">${grupa.zaposlenikNaziv}</dd>
        <dt class="col-sm-4">Početak:</dt>
        <dd class="col-sm-8">${grupa.pocetak}</dd>
        <dt class="col-sm-4">Ocjena:</dt>
        <dd class="col-sm-8">${GenerateStars(grupa.ocjena)}      
        </dd >
      </dl >
  `));
  cardBody.find(".users").append(grupa.kandidataPrijavljeno);
  cardBody.find(".timestamp").append(grupa.casova);
  cardBody.find(".price").append(grupa.cijena);
  cardBody.find(".card-title").html(grupa.kursTipNaziv + "-" + grupa.kursNaziv);
  cardBody.find(".card-subtitle").html(grupa.naziv);
  cardBody.find("a").attr("href", url + grupa.grupaId);

  return cloneCard;
}
function GenerateListGroupCard(grupeList, grupeDiv, mode = "append") {
  if ((typeof grupeList !== "undefined" && grupeList != null) && (typeof grupeDiv !== "undefined" && grupeDiv != null)) {
    var htmlResult = new Array();
    $.each(grupeList,
      function (index, value) {
        htmlResult.push(GenerateGroupCard(value));
      });
    //zamjeniti ili dodati rezultat na postojeci div
    if (mode === "replace") {
      grupeDiv.html(htmlResult);
    } else {
      grupeDiv.append(htmlResult);
    }
  } else {
    alert("fun GenerateListGroupCard() nema validne parametre!");
  }
}

function DateTransform(date) {
  //transformacija je potrebna jer js zamjeni dan sa mjesecom
  if (typeof date == "undefined" || date == null) {
    return "";
  }
  var dateSeparator = date.indexOf(".") !== -1 ? "." : "/";
  var dateParts = date.split(dateSeparator);
  if (dateParts.length === 0) {
    return "";
  } else {
    return new Date(dateParts[2].substring(0, 4), dateParts[1] - 1, dateParts[0]);
  }
}

function GenerateStars(ocjenaRaw) {
  if (typeof ocjenaRaw == "undefined" || ocjenaRaw == null) {
    ocjenaRaw = 0;
  } else {
    if (ocjenaRaw > 5) {
      ocjenaRaw = 5;
    }
    if (ocjenaRaw < 0) {
      ocjenaRaw = 0;
    }
  }

  var ocjena = parseInt(ocjenaRaw);

  var stars = "";
  var ostatak = ocjenaRaw % (ocjena < 1 ? 1 : ocjena) > 0;
  for (var i = ocjena; i > 0; i--) {
    stars += "<span class='fa fa-star checked'></span>\n";
  }
  if (ostatak) {
    ocjena++;
    stars += "<span class='fa fa-star-half-full checked'></span>\n";
  }
  for (var j = 5 - ocjena; j > 0; j--) {
    stars += "<span class='fa fa-star-o'></span>\n";

  }
  return stars;
}