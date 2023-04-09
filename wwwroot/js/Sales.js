$(document).ready(function () {
    var currentPage = 0;
    var songsPerPage = 13;
    var songs = [];
    var totalPages;

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

    $.ajax({
        url: "/Sales/GetSongs",
        type: "GET",
        success: function (result) {
            songs = result;
            totalPages = Math.ceil(songs.length / songsPerPage);
            createPaginationButtons(totalPages);
            displaySongs();
        },
        error: function () {
            alert("Error retrieving songs.");
        }
    });

    $("#loadMoreButton").on("click", function () {
        displaySongs();
    });
});

function addSong(songId) {
    // Do something with the song ID
    console.log("Song ID: " + songId);
}