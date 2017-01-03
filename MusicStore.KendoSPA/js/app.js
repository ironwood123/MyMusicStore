
var musicstoreApp = angular.module('musicstoreApp', [
    'ngRoute',
    'ngStorage',
    'musicstoreControllers',
    'kendo.directives',
    'musicstoreServices'
    //'musicstoreFilters',
    
]);

musicstoreApp.config(['$routeProvider',
   function ($routeProvider) {
       $routeProvider
         .when('/AlbumsByGenre/:GenreId', {
             templateUrl:  '/Store/AlbumsByGenre/',
             controller: 'AlbumCtrl'
         })       
         .when('/AlbumDetail/:AlbumId', {
             templateUrl: '/Store/AlbumDetail',
             controller: 'AlbumDetailCtrl'
         })
         .when('/CatalogAlbums',{
             templateUrl: '/Store/Catalog',
             controller: 'CatalogCtrl'
         })
         .when('/ShoppingCart', {
             templateUrl: '/Cart/ShoppingCart',
             controller: 'ShoppingCartCtrl'
         })
         .when('/AddtoCart/:AlbumId', {
             templateUrl: '/Cart/ShoppingCart',
             controller: 'AddCartItemCtrl'
         })
         .when('/Register', {
            templateUrl: '/Account/Register',
            controller: 'RegisterCtrl'
         })
         .when('/Login', {
             templateUrl: '/Account/Login',
             controller: 'LoginCtrl'
         })
         .when('/Checkout', {
             templateUrl: 'Cart/CheckOut',
             controller: 'CheckOutCtrl'
         })
         .when('/Complete/:OrderId', {
             templateUrl: 'Cart/Complete',
             controller: 'CompleteCtrl'
         })
        .otherwise('/', {
            redirecTo: '/Home/Index'
        });

   }]);