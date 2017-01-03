var musicstoreServices = angular.module('musicstoreServices', ['ngResource']);

musicstoreApp.factory('Album', ['$resource',
    function ($resource) {
        return $resource('https://localhost:44301/api/Album/:AlbumId');
    }
]);

musicstoreServices.factory('Genre', ['$resource',
    function ($resource) {
        return $resource('https://localhost:44301/api/Genre/:GenreId');
    }
]);

musicstoreServices.factory('Artist', ['$resource',
    function ($resource) {
        return $resource('https://localhost:44301/api/Artist/:ArtistId');
    }
]);