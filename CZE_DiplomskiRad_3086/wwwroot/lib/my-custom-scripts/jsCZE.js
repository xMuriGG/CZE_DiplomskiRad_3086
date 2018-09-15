function DodajAjaxEvente() {
  var progressBar = $(".progress-bar");

  //ajax eventi za form tagove
  var forms = $("form[ajax-call='da']");
  forms.each(function () {
    $.validator.unobtrusive.parse($(this));
  });

  forms.off("submit").submit(function (event) {
    $(this).attr("ajax-call", "dodan");
    event.preventDefault();
    if ($(this).valid()) {

      var urlZaPoziv1 = $(this).attr("ajax-url");
      var urlZaPoziv2 = $(this).attr("action");
      var divZaRezultat = $(this).attr("ajax-update");
      var onModelFail = $(this).attr("ajax-update-onModelFail");
      var modalId = $(this).attr("ajax-modal-id");
      //var alertText=$(this).attr("alert-text");
      var dataTableId = $(this).attr("datatable-id");
      var closeOnCreate = $(this).attr("close-on-create");

      if (typeof divZaRezultat != "undefined") {
        if (divZaRezultat === "parent") {
          divZaRezultat = $(this).parent();
        }
      }
      if (typeof onModelFail != "undefined" && onModelFail === "parent") {
        onModelFail = $(this).parent();
      }
      if (typeof divZaRezultat == "undefined") {
        if (modalId) {
          divZaRezultat = $(modalId).find(".modal-body");
        }
      }

      var urlZaPoziv;
      if (urlZaPoziv1 instanceof String)
        urlZaPoziv = urlZaPoziv1;
      else
        urlZaPoziv = urlZaPoziv2;

      var form = $(this);
      var formData = new FormData(form[0]);
      $.ajax({
        type: "POST",
        url: urlZaPoziv,
        cache: false,
        contentType: false,
        processData: false,
        data: formData,
        //data: form.serialize(),
        beforeSend: function (xhr) {
          progressBar.attr("style", "width:40%;");
          xhr.setRequestHeader("XSRF-TOKEN",
            $('input:hidden[name="__RequestVerificationToken"]').val());
        }
      })
        .done(function (data, status, xhr) {
          progressBar.attr("style", "width:70%;");
          if (xhr.status !== 201 && typeof onModelFail != "undefined") {
            divZaRezultat = onModelFail;
          }

          $(divZaRezultat).html(data);

          if (data.alert) {
            var a = data.alert;
            createAlert(a.title, a.summary, a.details, a.severity, a.appendToId, a.dismissible, a.autoDismiss);
          }
          if (dataTableId) {
            $(dataTableId).DataTable().ajax.reload(null, false);
          }

          if (xhr.status === 201 && modalId && closeOnCreate !== "no") {
            $(modalId).modal("hide");
          }
        })
        .fail(function (xhr, status, error) {
          if (typeof xhr.responseJSON !== 'undefined') {
            var a = xhr.responseJSON.alert;
            if (a) {
              createAlert(a.title, a.summary, a.details, a.severity, a.appendToId, a.dismissible, a.autoDismiss);
            }
          } else {
            createAlert('Opps!',
              'Greška prilikom komunikacije sa serverom.',
              'Server message: ' + xhr.responseText + '; status code: ' + xhr.status + '; error: ' + error,
              'danger',
              'pageMessages',
              true,
              false);
          }
        })
        .always(function () {
          progressBar.attr("style", "width:0%;");
        });
    }
  });

  if (typeof $().confirmation === "function") {

    //confirmation buttons
    $('[data-toggle=confirmation]').confirmation({
      rootSelector: '[data-toggle=confirmation]'
      // other options
    });
  }

  //ajax eventi za a tagove koji se nalaze u tabeli
  $("tbody").off("click").on("click", "tr a[ajax-call='da']", function (event) {

    event.preventDefault();
    var url = $(this).attr("href");
    var modalId = $(this).attr("ajax-modal-id");
    var dataTableId = $(this).attr("datatable-id");
    var divZaRezultat;

    var modal;
    if (modalId) {
      modal = $(modalId);
      divZaRezultat = modal.find(".modal-body");
    } else {
      divZaRezultat = $($(this).attr("ajax-update"));
    }

    $.ajax({
      type: "GET",
      url: url,
      beforeSend: function () {
        progressBar.attr("style", "width:40%;");
      }
    }).done(function (data, status, xhr) {
      progressBar.attr("style", "width:70%;");
      if (dataTableId) {
        $(dataTableId).DataTable().ajax.reload();
      }
      divZaRezultat.html(data);
      if (data.alert) {
        var a = data.alert;
        createAlert(a.title, a.summary, a.details, a.severity, a.appendToId, a.dismissible, a.autoDismiss);
      }
      if (modal) {
        modal.modal("show");
      }

    }).fail(function (xhr, status, error) {
      if (typeof xhr.responseJSON !== 'undefined') {
        var a = xhr.responseJSON.alert;
        if (a) {
          createAlert(a.title, a.summary, a.details, a.severity, a.appendToId, a.dismissible, a.autoDismiss);
        }
      } else {
        createAlert('Opps!',
          'Greška prilikom komunikacije sa serverom.',
          'Server message: ' + xhr.responseText + '; status code: ' + xhr.status + '; error: ' + error,
          'danger',
          'pageMessages',
          true,
          false);
      }
    }).always(function () {
      progressBar.attr("style", "width:0%;");
    });

  });

  //ajax eventi za a tag koji nije u tabeli
  var aTag = $("a[ajax-call='da']:not(tbody tr a[ajax-call='da'])");
  aTag.on("click", function (event) {
    event.preventDefault();

    var url = $(this).attr("href");

    var divZaRezultat;
    var modalId = $(this).attr("ajax-modal-id");
    var dataTableId = $(this).attr("datatable-id");
    var modal;
    if (modalId) {
      modal = $(modalId);
      divZaRezultat = modal.find(".modal-body");
    } else {
      divZaRezultat = $(this).attr("ajax-update");
    }

    $.ajax({
      type: "GET",
      url: url,
      beforeSend: function () {
        progressBar.attr("style", "width:40%;");
      }
    }).done(function (data, status, xhr) {
      progressBar.attr("style", "width:70%;");
      $(divZaRezultat).html(data);
      if (data.alert) {
        var a = data.alert;
        createAlert(a.title, a.summary, a.details, a.severity, a.appendToId, a.dismissible, a.autoDismiss);
      }
      if (modal) {
        modal.modal("show");
      }
      if (dataTableId) {
        $(dataTableId).DataTable().ajax.reload(null, false);
      }

    }).fail(function (xhr, status, error) {
      if (typeof xhr.responseJSON !== 'undefined') {
        var a = xhr.responseJSON.alert;
        if (a) {
          createAlert(a.title, a.summary, a.details, a.severity, a.appendToId, a.dismissible, a.autoDismiss);
        }
      } else {
        createAlert('Opps!',
          'Greška prilikom komunikacije sa serverom.',
          'Server message: ' + xhr.responseText + '; status code: ' + xhr.status + '; error: ' + error,
          'danger',
          'pageMessages',
          true,
          false);
      }
    }).always(function () {
      progressBar.attr("style", "width:0%;");
    });
  });
  aTag.each(function () {
    $(this).attr("ajax-call", "dodan");
  });

}

//custom filtriranje data tabele na enter
$(document).on("preInit.dt",
  function () {
    var $dbFilter = $(".dataTables_filter ");

    $dbFilter.each(function (parameters) {
      var $sb = $(this).find("input[type='search']");
      $sb.attr("placeholder", "Search");
      //label fix
      var label = $(this).find("label");
      label.html("");
      label.append($sb);
      $sb.wrap("<div class='input-group'></div>");

      var tableWrapper = $sb.closest(".dataTables_wrapper");
      var table = tableWrapper.find("table").first();
      // Add key hander
      $sb.off().on("keypress",
        function (evtObj) {
          if (evtObj.keyCode === 13) {
            //$('#osobaDataTable').DataTable().search($sb.val()).draw();           
            table.DataTable().search($sb.val()).draw();
          }
        });

      // add button and button handler
      var $btn = $("<button class='btn btn-outline-secondary' type='button'>Go</button>");
      $sb.after($btn);
      $btn.wrap("<div class='input-group-append'></div>");
      $btn.on("click",
        function (evtObj) {
          table.DataTable().search($sb.val()).draw();
        });
    });

  });

//Dasboard Chart functions->
function addChartData(chart, labels, data) {
  chart.data.labels = labels;
  chart.data.datasets[0].data = data;
  chart.update();
}
function removeChartData(chart) {
  chart.data.labels = [];
  chart.data.datasets[0].data = [];

  chart.update();
}
function ajaxChartCall(chart, godinaDDL, url, progressBar) {

  $.ajax({
    type: "GET",
    url: url + "?godinaDDL=" + godinaDDL,
    beforeSend: function (xhr) {
      progressBar.attr("style", "width:40%;");
    }

  }).done(function (data, status, xhr) {
    addChartData(chart, data.data.x, data.data.y);
    progressBar.attr("style", "width:0%;");
  }).fail(function (xhr, status, error) {
    createAlert('Opps!',
      'Greška prilikom komunikacije sa serverom.',
      'Server message: ' + xhr.responseText + '; status code: ' + xhr.status + '; error: ' + error,
      'danger',
      'pageMessages',
      true,
      false);
    progressBar.attr("style", "width:0%;");
  });
}
//<-Dasboard Chart functions

$(document).ready(function () {
  // izvršava nakon što glavni html dokument bude generisan
  DodajAjaxEvente();
});
$(document).ajaxComplete(function () {
  // izvršava nakon bilo kojeg ajax poziva
  DodajAjaxEvente();

});