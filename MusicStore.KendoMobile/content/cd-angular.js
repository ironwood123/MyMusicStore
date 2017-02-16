angular.module('musicstoreMobileApp', [ 'kendo.directives' ])
    .run(['cdCart', function(cdCart){
        cdCart.init();
    }])

    .service('genreService', ['$rootScope', function ($rootScope) {
            this.currentItem = null;
            this.genreDataSource = new kendo.data.DataSource({
                transport: {
                    read: {
                        url: "https://localhost:44301/api/Genre",
                        dataType: "json"
                    }
                }
            });

            this.setCurrentItem = function (id) {
                this.currentItem = this.genreDataSource.get(GenreId);
            };
    }])

    .service('cdCart', ['$rootScope', function($rootScope) {
        
       this.init = function() {
            this.added = new kendo.data.ObservableArray([]);
       };

        this.addToCart = function(kendoEvent, dataItem) {
            var that = this, item, ordered;

            item = dataItem ? dataItem : $rootScope.cdDataSource;

            ordered = item.ordered ? item.ordered : 0;
            ordered += 1;
            item.ordered = ordered;

            if (item.ordered > 0 && item.visibleMessage == null) {
                    item.visibleMessage = true;
                    this.added.push(item);
            }
           this.added[this.added.length-1].ordered = ordered;
            kendoEvent.preventDefault();
        };

        this.removeItem = function(kendoEvent, dataItem) {
            var item = dataItem,
            index = this.added.indexOf(item),
            currentView = kendo.mobile.application.view();

            item.set("ordered", 0);
            item.set("visibleMessage", false);
            this.added.splice(index, 1);
            currentView.scroller.reset();
            kendoEvent.preventDefault();
        };

        this.checkout = function() {
            //var that = this,
            //dataSourceData = this.productsDataSource.data(),
            //length = dataSourceData.length;

            //setTimeout(function () {
            //    for (idx = 0; idx < length; idx++) {
            //        dataSourceData[idx].set("ordered", 0);
            //    }

            //    that.added = [];
            //}, 400);
            this.added = [];

        };

        this.showLabel = function() {
            return $rootScope.cdDataSource && $rootScope.cdDataSource.ordered > 0;
        };

        this.showTotal = function() {
            var cartItems = this.added,
            total = 0;
            for(var idx = 0; idx < cartItems.length; idx++) {
                total += cartItems[idx].ordered * cartItems[idx].Price;
            }
            return kendo.toString(total, "c");
        };

        this.GetCDbyGenre = function (id) {
            this.productsDataSource = new kendo.data.DataSource({
                transport: {
                    read: {
                        url: "https://localhost:44301/api/AlbumByGenre/" + id,
                        dataType: "json"
                    }
                }

            });
        };
        this.GetCDDetail = function (id) {
            $.ajax({
                url: "https://localhost:44301/api/Album/" + id,
                type: 'GET',
                dataType: 'json',
                async: false,
                success: function (data) {
                    $rootScope.cdDataSource = data;
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(errorThrown)
                },
                beforeSend: function (xhr) {
                }
            });
        };

    }])

    .factory('templates', function() {
        return {
            menuTemplate: $("#menuTemplate").html(),
            genreTemplate: $("#genreTemplate").html(),
            cartItemTemplate: $("#cartItemTemplate").html()
        };
    })

    .controller('genreController', ['$scope', 'genreService', 'templates', function ($scope, genreService, templates) {
        $scope.genreService = genreService;
        $scope.templates = templates;
    }])

    .controller('indexController', ['$scope', 'cdCart', 'templates', function($scope, cdCart, templates) {
        $scope.cdCart = cdCart;
        $scope.templates = templates;
        $scope.GetCDbyGenre = function (kendoEvent) {
            var id = parseInt(kendoEvent.view.params.GenreId);
            cdCart.GetCDbyGenre(id);
        }
    }])

    .controller('menuController', ['$scope', 'cdCart', 'templates', function($scope, cdCart, templates) {
        $scope.cdCart = cdCart;
        $scope.templates = templates;

        $scope.groupByCategory = function() {
            $scope.cdCart.productsDataSource.filter([]);
            $scope.cdCart.productsDataSource.group({ field: "category" });
        }
    }])

    .controller('cartController', ['$scope', 'cdCart', 'templates', function($scope, cdCart, templates){
        $scope.cdCart = cdCart;
        $scope.templates = templates;
    }])

    .controller('detailsController', ['$scope', 'cdCart', function($scope, cdCart){
        $scope.cdCart = cdCart;
        $scope.GetCDDetail = function (kendoEvent) {
            var id = parseInt(kendoEvent.view.params.id);
            cdCart.GetCDDetail(id);
        }
    }]);
