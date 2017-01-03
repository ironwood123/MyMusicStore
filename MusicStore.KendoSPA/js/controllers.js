var musicstoreControllers = angular.module('musicstoreControllers', []);

musicstoreControllers.controller('GenreCtrl', ['$scope', 
    function ($scope) {
        $.ajax({
            url: "https://localhost:44301/api/Genre",
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (data) {
                $scope.genres = data;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(errorThrown)
            },
            beforeSend: function (xhr) {
            }
        });

    }]);

musicstoreControllers.controller('AlbumCtrl', ['$scope', '$routeParams',
    function ($scope, $routeParams) {
        $scope.GenreId = $routeParams.GenreId;
    }
]);

musicstoreControllers.controller('AlbumDetailCtrl', ['$scope', '$routeParams','Album','Genre','Artist',
    function ($scope, $routeParams, Album, Genre, Artist) {
        $scope.Album = Album.get({AlbumId: $routeParams.AlbumId}, function (album) {
            $scope.Genre = Genre.get({ GenreId: album.GenreId });
            $scope.Artist = Artist.get({ ArtistId: album.ArtistId });
        });
    }
]);

musicstoreControllers.controller('CatalogCtrl', ['$scope', function () {

}]);

musicstoreControllers.controller('ShoppingCartCtrl', ['$scope', '$sessionStorage', '$http', function ($scope, $sessionStorage, $http) {
    $scope.sessionid = $sessionStorage.SessionId;
    $scope.mainGridOptions = {
        columns: [
                  { field: "Album.Title", title: "CD Name", width: "200px", aggregates: ["count"]},
                  { field: "Album.Price", title: "Price", width: "100px"},
                  { field: "Count", title: "Quantity", width: "100px" },
                  { field: "ProductTotal()", title: "Product Total", aggregates: ["sum"], footerTemplate: "Total Price: #=  kendo.toString(sum, 'C') #" },
                  { command: [{ name: "destroy", width: "90px",text:"Remove from Cart"  }] }
        ],
        editable: "inline",
        selectable: true,
        scrollable: true,
        sortable: {
            mode: "multiple",
            allowUnsort: true
        },
        groupable: true,
        pageable: {
            input: true,
            numeric: true
        },
        dataSource: {
            transport: {
                read: {
                    url: "https://localhost:44301/api/Cart/" + $scope.sessionid,
                    dataType: "json",
                    type: "GET",
                    contentType: "application/json; charset=utf-8"
                },
                destroy: {
                    url: function (e) {
                        return "https://localhost:44301/api/Cart/" + e.RecordId + "/" + $scope.sessionid
                    },
                    type: "DELETE",
                    dataType: "json",                              
                    complete: function (options) {
                        $scope.mainGrid.dataSource.read();
                    }
                },
                parameterMap: function (options, operation) {
                    if (options.models) {
                        console.log(options);
                        return { models: kendo.stringify(options.models) };
                    } else if (operation == "read") {
                        return kendo.stringify(options);
                    } else {
                        return options;
                    }

                }

            },
            aggregate: [{ field: "ProductTotal()", aggregate: "sum" }, { field: "Album.Title", aggregate: "count" }],
            schema: {
                    data: "CartItems",
                    total: "CartTotal",
                    model: {
                            id: "RecordId",
                            fields: {
                                CartId: { type: "string", editable: true, nullable: false },
                                RecordId: { type: "number" },
                                AlbumId: { type: "number" },
                                Count: { type: "number" }
                            },
                            ProductTotal:function(){
                                return this.Album.Price * this.Count;
                            }

                    }
            },
            pageSize: 10,
            serverPaging: true,
        },
    };

}]);
musicstoreControllers.controller('CompleteCtrl', ['$scope', '$routeParams', function ($scope,$routeParams) {
    $scope.orderid = $routeParams.OrderId;
}]);
musicstoreControllers.controller('CheckOutCtrl', ['$scope', '$location', '$sessionStorage', function ($scope, $location, $sessionStorage) {
    var token = $sessionStorage.tokenKey;
    var headers = {};
    if (token) {
        headers.Authorization = 'Bearer ' + token;
    }
    else
    {
        $location.path("/Login");
    }
    $scope.checkout = function () {
        var data = {
            'FirstName': $scope.firstname, 'LastName': $scope.lastname, 'Address': $scope.address, 'City': $scope.city, 'State': $scope.state,
            'PostalCode': $scope.postalcode, 'Country': $scope.country, 'Phone': $scope.phone, 'Email': $scope.email, 'PromoCode': $scope.promocode,
            'SessionId':$sessionStorage.SessionId
        };

        $.ajax({
            url: "https://localhost:44301/api/CheckOut",
            type: 'POST',
            dataType: 'json',
            data: data,
            headers: headers,
            async: false,
            success: function (data) {
                console.log(data);
                $location.path("/Complete/" + data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(errorThrown)
            },
            beforeSend: function (xhr) {
            }
        });
    }
}]);
musicstoreControllers.controller('AddCartItemCtrl', ['$scope', '$routeParams', '$location', '$sessionStorage',function ($scope, $routeParams, $location, $sessionStorage) {
    var json = { 'AlbumId': $routeParams.AlbumId, 'SessionId': $sessionStorage.SessionId }
    console.log(json.SessionId);
    $.ajax({
        url: "https://localhost:44301/api/Cart/AddtoCart",
        type: 'POST',
        dataType: 'json',
        data: json,
        async: false,
        success: function (data) {
            $sessionStorage.SessionId = data;
            $location.path("/ShoppingCart");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(errorThrown)
        },
        beforeSend: function (xhr) {
        }
    });
}]);
musicstoreControllers.controller('RegisterCtrl', ['$scope', function ($scope) {

    $scope.registeruser = function ()
    {
        var login = { 'Email': $scope.email, 'Password': $scope.password, 'ConfirmPassword': $scope.confirmedpassword };
        $.ajax({
            url: "https://localhost:44301/api/Account/Register",
            type: 'POST',
            dataType: 'json',
            data: login,
            async: false,
            success: function (data) {
                $location.path("/ShoppingCart");
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(errorThrown)
            },
            beforeSend: function (xhr) {
            }
        });
    }
}]);

musicstoreControllers.controller('LoginCtrl', ['$scope', '$sessionStorage', '$location', function ($scope, $sessionStorage,$location) {

    $scope.login = function () {
        var loginData = {
            grant_type: 'password',
            username: $scope.username,
            password: $scope.password
        };
        $.ajax({
            url: "https://localhost:44301/Token",
            type: 'POST',
            dataType: 'json',
            data: loginData,
            async: false,
            success: function (data) {
                $sessionStorage.tokenKey = data.access_token;
                console.log($sessionStorage.tokenKey);
                $location.path("/ShoppingCart");
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(errorThrown)
            },
            beforeSend: function (xhr) {
            }
        });
    }
}]);

musicstoreControllers.controller("AppController", function ($scope, $http) {
    $scope.mainGridOptions = {
        columns: [
            { field: "ProductID", title: "ID" },
            { field: "ProductName", title: "Product Name" },
            { command: [{ template: "<button class='k-button' ng-click='showDetails(dataItem)'>Show details</button" }] },
        ],
        pageable: true,            
        dataSource: {                  
            pageSize: 5,
            transport: {
                read: function (e) {
                    $http.jsonp('http://demos.telerik.com/kendo-ui/service/Products?callback=JSON_CALLBACK').
                    success(function (data, status, headers, config) {
                        e.success(data)
                    }).
                    error(function (data, status, headers, config) {
                        alert('something went wrong')
                        console.log(status);
                    });
                }
            }
        },              
    };
    $scope.showDetails = function (dataItem) {
        $scope.details = true;
        $scope.listView.dataSource.data([dataItem]);
    }
    $scope.hideDetails = function () {
        $scope.details = false;
    }
})
