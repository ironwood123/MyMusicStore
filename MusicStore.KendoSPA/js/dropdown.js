var dropdowncol = function () {
    var genreds = null;
    var artistds = null;
    $.ajax({
        url: "https://localhost:44301/api/Genre",
        type: 'GET',
        dataType: 'json',
        async: false,
        success: function (data) {
            genreds = data;
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(errorThrown)
        },
        beforeSend: function (xhr) {
        }
    });

    $.ajax({
        url: "https://localhost:44301/api/Artist",
        type: 'GET',
        dataType: 'json',
        async: false,
        success: function (data) {
            artistds = data;
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(errorThrown)
        },
        beforeSend: function (xhr) {
        }
    });
    function GenreDropDownEditor(container, options) {
        statedrpdown = $('<input required  data-bind="value:GenreId"/>')
            .appendTo(container)
            .kendoDropDownList({
                autoBind: false,
                dataSource: {
                    transport: {
                        read: "https://localhost:44301/api/Genre",
                        dataType: "json"
                    }
                },
                optionLabel: "Select Genre",
                dataTextField: "Name",
                dataValueField: "GenreId",
            });
    }
    function ArtistDropDownEditor(container, options) {
        statedrpdown = $('<input required  data-bind="value:ArtistId"/>')
            .appendTo(container)
            .kendoDropDownList({
                autoBind: false,
                dataSource: {
                    transport: {
                        read: "https://localhost:44301/api/Artist",
                        dataType: "json"
                    }
                },
                optionLabel: "Select Artist",
                dataTextField: "Name",
                dataValueField: "ArtistId",
            });
    }
    function genreName(GenreId) {
        for (var i = 0, length = genreds.length; i < length; i++) {
            if (genreds[i].GenreId == GenreId) {
                return genreds[i].Name;
            }
        }
        return '';
    }

    function artistName(ArtistId) {
        for (var i = 0, length = artistds.length; i < length; i++) {
            if (artistds[i].ArtistId == ArtistId) {
                return artistds[i].Name;
            }
        }
        return '';
    }
    return {
        genreds: genreds,
        artistds: artistds,
        GenreDropDownEditor: GenreDropDownEditor,
        ArtistDropDownEditor: ArtistDropDownEditor,
        genreName: genreName,
        artistName: artistName
    };
}();