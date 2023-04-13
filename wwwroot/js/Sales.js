$(document).ready(function () {
    $("#cart-badge").removeClass("cart-badge");
    getSongs();
    getInvoiceSongs();
    getUserCard();

    $("#loadMoreButton").on("click", function () {
        displaySongs();
    });

    $("#SongName").keyup(function () {
        updateSongs();
    });

    $("#AlbumeName").keyup(function () {
        updateSongs();
    });

    $("#AuthorName").keyup(function () {
        updateSongs();
    });

    $("#GenderDescription").keyup(function () {
        updateSongs();
    });

});

//Variables
var currentPage = 0;
var songsPerPage = 13;
var songs = [];
var totalPages;
//+++++++++

/*CONFIGURACION GLOBAL DEL alertify*/
alertify.set('notifier', 'position', 'top-right');
/*+++++++++++++++++++++++++++++++++*/

function updateSongs() {
    $.ajax({
        url: "/Sales/GetSongs",
        type: "POST",
        data: { SongName: $("#SongName").val(), AlbumeName: $("#AlbumeName").val(), AuthorName: $("#AuthorName").val(), GenderDescription: $("#GenderDescription").val() },
        success: function (result) {
            songs = result;
            totalPages = Math.ceil(songs.length / songsPerPage);
            createPaginationButtons(totalPages);
            currentPage = 0;
            displaySongs();
        },
        error: function () {
            alert("Error retrieving songs.");
        }
    });
}

function displaySongs() {
    var start = currentPage * songsPerPage;
    var end = start + songsPerPage - 1;
    var displayedSongs = songs.slice(start, end);

    $("#songRow").empty();

    $.each(displayedSongs, function (i, song) {
        var songCard = '<div class="col-lg-2 shadow-left lift">' +
            '<div class="card mb-4">' +
            '<div class="card-body text-center">' +
            '<img class="img-fluid mb-5" src="~/../../assets/img/cd_music.png" style="max-width: 50%">' +
            '<h5 id="songName" class="titulo"> ' + song.songName + '</h5>' +
            '<p id="songDesc" class="mb-4 descripSong"> ' + song.description + '</p>' +
            '<button id="addSong" class="btn btn-primary" onclick="addSong(' + song.songId + ')">Añadir <i class="fas fa-plus-square fa-fw"></i></button>' +
            '</div>' +
            '</div>' +
            '</div>';
        $("#songRow").append(songCard);
    });
    $("#cantidad").empty();
    $("#cantidad").append("Mostrando " + displayedSongs.length + " de " + songs.length);
    currentPage++;

    if (currentPage >= totalPages) {
        $("#loadMoreButton").hide();
    }
}

function createPaginationButtons(totalPages) {
    $("#paginationButtons").empty();

    for (var i = 0; i < totalPages; i++) {
        var button = '<button class="btn paginationButton datatable-pagination-list-item" data-page="' + i + '">' + (i + 1) + '</button>';
        $("#paginationButtons").append(button);
    }

    $(".paginationButton").on("click", function () {
        currentPage = $(this).data("page");
        displaySongs();
    });
}

function getSongs() {
    $.ajax({
        url: "/Sales/GetSongs",
        type: "POST",
        data: { SongName: $("#SongName").val(), AlbumeName: $("#AlbumeName").val(), AuthorName: $("#AuthorName").val(), GenderDescription: $("#GenderDescription").val() },
        success: function (result) {
            songs = result;
            totalPages = Math.ceil(songs.length / songsPerPage);
            createPaginationButtons(totalPages);
            currentPage = 0;
            displaySongs();
        },
        error: function () {
            alert("Error retrieving songs.");
        }
    });
}

//Funciones añadir al carrito
function addSong(songId) {

    // Call controller action with songId
    $.ajax({
        type: "POST",
        url: "/Sales/AddSong",
        data: { songId: songId },
        success: function (result) {
            alertify.notify('<strong>Procesado exitosamente</strong> ¡Revisa tu carrito!', 'success', 15);
            $("#cart-badge").addClass("cart-badge");
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status);
            console.log(thrownError);
        }
    });
}

function getInvoiceSongs() {
    $.ajax({
        url: "/Sales/getInvoiceSongs",
        type: "GET",
        success: function (result) {
            // Recorre cada canción devuelta y agrega una fila a la tabla HTML
            $.each(result, function (index, song) {
                var row = $("<tr>").addClass("border-bottom");
                var name = $("<div>").attr("id", "SongName").addClass("fw-bold").text(song.songName);
                var description = $("<div>").attr("id", "SongDescript").addClass("small text-muted d-none d-md-block").text(song.description);
                var td1 = $("<td>").append(name, description);
                var td2 = $("<td>").attr("id", "SongPrice").addClass("text-end fw-bold").text("₡" + song.songPrice.toFixed(2));
                var td3 = $("<td>").attr("id", "SongIVA").addClass("text-end fw-bold").text("₡" + song.iva.toFixed(2));
                var td4 = $("<td>").addClass("text-end");
                var deleteButton = $("<button>").addClass("btn btn-sm btn-danger").html('<i class="fas fa-trash" aria-hidden="true"></i>').click(function () {
                    CleanItem(song.invoiceDetailId);
                });
                td4.append(deleteButton);
                row.append(td1, td2, td3, td4);
                $("#invoice tbody").append(row);
            });

            // Calcula el subtotal, el IVA y el total
            var subtotal = 0;
            var totalIVA = 0;
            $.each(result, function (index, song) {
                subtotal += song.songPrice;
                totalIVA += song.iva;
            });
            var totalFinal = subtotal + totalIVA;

            // Agrega las filas para el subtotal, el IVA y el total
            var subtotalRow = $("<tr>");
            subtotalRow.append($("<td>").attr("colspan", "3").addClass("text-end pb-0").append($("<div>").addClass("text-uppercase small fw-700 text-muted").text("Subtotal:")));
            subtotalRow.append($("<td>").addClass("text-end pb-0").append($("<div>").attr("id", "Subtotal").addClass("h5 mb-0 fw-700").text("₡" + subtotal.toFixed(2))));
            $("#invoice tbody").append(subtotalRow);

            var ivaRow = $("<tr>");
            ivaRow.append($("<td>").attr("colspan", "3").addClass("text-end pb-0").append($("<div>").addClass("text-uppercase small fw-700 text-muted").text("IVA (13%):")));
            ivaRow.append($("<td>").addClass("text-end pb-0").append($("<div>").attr("id", "TotalIVA").addClass("h5 mb-0 fw-700").text("₡" + totalIVA.toFixed(2))));
            $("#invoice tbody").append(ivaRow);

            var totalRow = $("<tr>");
            totalRow.append($("<td>").attr("colspan", "3").addClass("text-end pb-0").append($("<div>").addClass("text-uppercase small fw-700 text-muted").text("Total:")));
            totalRow.append($("<td>").addClass("text-end pb-0").append($("<div>").attr("id", "TotalFinal").addClass("h5 mb-0 fw-700 text-green").text("₡" + totalFinal.toFixed(2))));
            $("#invoice tbody").append(totalRow);
        },
        error: function () {
            alert("Error retrieving songs.");
        }
    });
}

function getUserCard() {
    $.ajax({
        url: "/Sales/getUserCard",
        type: "GET",
        success: function (result) {
            if (result.cardNumber != null) {
                $("#Titular").text(result.userName);
                $("#Email").text(result.userEmail);
                $("#CardNumber").text(result.cardNumber);
                $("#CardExp").text(result.cardExpiration);
                $("#CardType").text(result.cardName);
            } else {
                alertify.warning("Usuario no tiene una tarjeta de pago registrada");
                $("#payInvoice").prop("disabled", true);
                $("#addCard").prop("hidden", false);
            }
        },
        error: function () {
            alert("Error retrieving songs.");
        }
    });
}

//Funcion quitar del carrito
function CleanItem(invoiceDetailId) {
    $.ajax({
        url: "/Sales/CleanItem",
        type: "POST",
        data: { invoiceDetailId: invoiceDetailId },
        success: function () {
            alertify.success("Se quitó la canción exitosamente");
                $("#invoice tbody").empty();
                getInvoiceSongs();
        },
        error: function () {
            alertify.error("Error al quitar la canción");
        }
    });
}

//Funcion pagar
function PayInvoice() {
    if ($("#TotalFinal").text() !== "₡0.00") {
        $.ajax({
            url: "/Sales/PayInvoice",
            type: "GET",
            success: function () {
                alertify.alert("PAGO ÉXITOSO", "Factura pagada exitosamente, hemos enviado las canciones en formato .mp3 a tu correo", function () {
                    window.location.href = "/Sales/Index";
                });
                $("#invoice tbody").empty();
                getInvoiceSongs();
            },
            error: function () {
                alertify.error("Error al realizar el pago");
            }
        });
    } else {
        alertify.warning("El carrito de compras se encuentra vacío");
    }
}