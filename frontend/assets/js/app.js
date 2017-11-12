angular.module('app', ['ngMaterial', 'chart.js', 'ngStorage'])

.config(function($mdDateLocaleProvider) {
        $mdDateLocaleProvider.formatDate = function(date) {
            return moment(date).format("DD.MM.YYYY");
        }
    })

.controller('appCtrl', ['$scope', '$mdDialog', '$timeout', '$q', '$log', '$rootScope', '$localStorage', '$sessionStorage', '$window', '$http', function ($scope, $mdDialog, $timeout, $q, $log, $rootScope, $localStorage, $sessionStorage, $window, $http) {

    var getConfig = function () {
        $http.get('assets/config/config.json')
          .then(function (response) {
              $rootScope.config = response.data;
          });
    };
    getConfig();

    var webService = 'CheckIn.asmx';

    var checkUser = function () {
        if ($sessionStorage.userid == "" || $sessionStorage.userid == undefined) {
            $rootScope.currTpl = 'assets/partials/login.html';
            $rootScope.isLogin = false;
        } else {
            $rootScope.currTpl = 'assets/partials/dashboard.html';
            $scope.activeTab = 0;
            $rootScope.isLogin = true;
        }
    }
    checkUser();

    $scope.toggleTpl = function (x, y) {
        $rootScope.currTpl = x;
        $rootScope.activeTab = y;
    };

    $scope.newClient = function () {
        $http({
            url: $rootScope.config.backend + 'Clients.asmx/Init',
            method: "POST",
            data: ""
        })
       .then(function (response) {
           $rootScope.client = JSON.parse(response.data.d);
       },
       function (response) {
           alert(response.data.d)
       });

        $http({
            url: $rootScope.config.backend + 'ClientServices.asmx/Init',
            method: "POST",
            data: ""
        })
       .then(function (response) {
           $rootScope.clientService = JSON.parse(response.data.d);
       },
       function (response) {
           alert(response.data.d)
       });
    }

    $scope.today = new Date();

    $scope.getDateDiff = function (x) {
        var today = new Date();
        var date1 = today;
        var date2 = new Date(x);
        var diffDays = parseInt((date2 - date1) / (1000 * 60 * 60 * 24));
        return diffDays;
    }

    //CheckIn
    if ($localStorage.currentClients == undefined) {
        $scope.currentClients = [];
    } else {
        $scope.currentClients = $localStorage.currentClients
    }
  
    $rootScope.clientsFilter = function (clientType) {
        var obj = $rootScope.clients;
        return function (obj) {
            return obj.isActive === clientType;
        };
    };

    //$scope.currentNumberOfClients = function () {
    //    return $scope.currentClients.length;
    //}

    $scope.expirationMembership = function () {
        var count = 0;
        angular.forEach($rootScope.clients, function (value, key) {
            if ($scope.getDateDiff(value.dateTo) < 0) {
                count ++;
            }
        })
        return count;
    }

    $scope.NumberOfNewMembers = function () {
        var count = 0;
        angular.forEach($rootScope.clients, function (value, key) {
            if ($scope.getDateDiff(value.dateFrom) > -30) {
                console.log($scope.getDateDiff(value.dateFrom));
                count++;
            }
        })
        return count;
    }


    $scope.showSaveMessage = false;


    $scope.logout = function () {
        $sessionStorage.userid = "";
        $sessionStorage.username = "";
        $rootScope.isLogin = false;
        $rootScope.currTpl = 'assets/partials/login.html';
    }


}])

.controller('loginCtrl', ['$scope', '$http', '$sessionStorage', '$window', '$rootScope', function ($scope, $http, $sessionStorage, $window, $rootScope) {
    var webService = 'Users.asmx';
     $scope.login = function (u, p) {
        $http({
            url: $rootScope.config.backend + webService + '/Login',
            method: "POST",
            data: {
                userName: u,
                password: p
            }
        })
        .then(function (response) {
            if (JSON.parse(response.data.d).userName == u) {
                $rootScope.user = JSON.parse(response.data.d);
                $rootScope.loginUser = $rootScope.user;
                $sessionStorage.userid = $rootScope.user.userId;
                $sessionStorage.username = $rootScope.user.userName;
                $rootScope.isLogin = true;
                $rootScope.currTpl = 'assets/partials/dashboard.html';
            } else {
                $scope.errorLogin = true;
                $scope.errorMesage = 'Pogrešno korisničko ime ili lozinka.'
                //$rootScope.currTpl = 'assets/partials/singup.html';  //<< Only fo first registration
            }
        },
        function (response) {
            $scope.errorLogin = true;
            $scope.errorMesage = 'Korisnik nije pronađen.'
        });
    }
}])

.controller("dashboardCtrl", ['$scope', '$http', '$timeout', '$rootScope', function ($scope, $http, $timeout, $rootScope) {
    var webService = 'Analytics.asmx';
    $scope.displayType = 0;

    $scope.initServices = function () {
        $http({
            url: $rootScope.config.backend + webService + '/InitServices',
            method: "POST",
            data: ""
        })
        .then(function (response) {
            $scope.clientServicesCount = JSON.parse(response.data.d);
        },
        function (response) {
            alert(response.data.d)
        });
    }
    //$scope.initServices();

    $scope.initClients = function () {
        $http({
            url: $rootScope.config.backend + webService + '/InitClients',
            method: "POST",
            data: ""
        })
        .then(function (response) {
            $rootScope.clientsCount = JSON.parse(response.data.d);
        },
        function (response) {
            alert(response.data.d)
        });
    }
   // $scope.initClients();

    var getMessagesCount = function () {
        $http({
            url: $rootScope.config.backend + webService + '/GetMailMessagesCount',
            method: "POST",
            data: ""
        })
        .then(function (response) {
            $scope.messagesCount = JSON.parse(response.data.d);
        },
        function (response) {
            alert(response.data.d)
        });
    }
    getMessagesCount();

    var getClientServicesCount = function () {
        $http({
            url: $rootScope.config.backend + webService + '/GetClientServicesCount', // '../Clients.asmx/Load',
            method: "POST",
            data: ""
        })
      .then(function (response) {
          $scope.clientsSrvicesCount = JSON.parse(response.data.d);
        //  setServiceGraphData();
      },
      function (response) {
          alert(response.data.d)
      });
    };
    getClientServicesCount();

    var getClientsCount = function () {
        $http({
            url: $rootScope.config.backend + webService + '/GetClientsCount', // '../Clients.asmx/Load',
            method: "POST",
            data: ""
        })
      .then(function (response) {
          $rootScope.clientsCount = JSON.parse(response.data.d);
      },
      function (response) {
          alert(response.data.d)
      });
    };
    getClientsCount();

    //var setServiceGraphData = function () {
    //    $scope.series = ['Kupljene usluge'];
    //    $scope.labels = [];
    //    $scope.data = [];
    //    angular.forEach($scope.clientsSrvicesCount, function (x, key) {
    //        $scope.labels.push(x.service + ', ' + x.option);
    //        $scope.data.push($scope.displayType == 0 ? x.count : x.price);
    //    });

    //    $scope.options = {
    //        responsive: true,
    //        maintainAspectRatio: false,
    //        legend: {
    //            display: false
    //        },
    //        scales: {
    //            xAxes: [{
    //                display: true,
    //                scaleLabel: {
    //                    display: true,
    //                },
    //                ticks: {
    //                    beginAtZero: true,
    //                    stepSize: $scope.displayType == 0 ? 10 : 10000
    //                }
    //            }]
    //        }
    //    }
    //};

    //$scope.changeDisplayType = function () {
    //    setServiceGraphData();
    //}


}])

.controller("schedulerCtrl", ['$scope', '$localStorage', '$http', '$rootScope', '$timeout', function ($scope, $localStorage, $http, $rootScope, $timeout) {
    var webService = 'Scheduler.asmx';
    $scope.rooms = [
      {
          'value': '1',
          'text': 'Sala br.1'
      },
      {
          'value': '2',
          'text': 'Sala br.2'
      },
      {
          'value': '3',
          'text': 'Sala br.3'
      },
      {
          'value': '4',
          'text': 'Sala br.4'
      },
      {
            'value': '5',
            'text': 'Sala br.5'
      }
    ];
    $scope.room = '1';
    // $scope.testContent = $localStorage.testContent;
    
    //$scope.init = function () {
    //    //if ($localStorage.events == undefined) {
    //    //    $scope.events = [];
    //    //} else {
    //    //    $scope.events = $localStorage.events;
    //    //}
    //}
    //$scope.init();

   // $rootScope.events = [];
    //var  getJsonFromFile = function () {
    //    $http({
    //        method: 'GET',
    //        url: '../assets/json/scheduler.json'
    //    }).then(function successCallback(response) {
    //        $rootScope.events = response.data;
    //    }, function errorCallback(response) {
    //    });
    //}
   
    //var load = function () {
    //    $http({
    //        url: $rootScope.config.backend + webService + '/GetSchedulerByRoom', // '../Scheduler.asmx/Load',
    //        method: 'POST',
    //        data: { room: $scope.room }
    //    })
    //   .then(function (response) {
    //       $rootScope.events = JSON.parse(response.data.d);
    //       showScheduler();
    //   },
    //   function (response) {
    //       alert(response.data.d)
    //   });
    //}
    //load();

    $scope.getSchedulerByRoom = function () {
        $http({
            url: $rootScope.config.backend + webService + '/GetSchedulerByRoom', // '../Scheduler.asmx/Load',
            method: 'POST',
            data: {room: $scope.room}
        })
       .then(function (response) {
            $rootScope.events = JSON.parse(response.data.d);
            $timeout(function () {
               showScheduler();
           }, 50);
       },
       function (response) {
           alert(response.data.d)
       });
    }
    $scope.getSchedulerByRoom();

    var showScheduler = function () {
        YUI().use('aui-scheduler', function (Y) {
            var agendaView = new Y.SchedulerAgendaView();
            var dayView = new Y.SchedulerDayView();
            var weekView = new Y.SchedulerWeekView();
            var monthView = new Y.SchedulerMonthView();
            var eventRecorder = new Y.SchedulerEventRecorder({
                on: {
                    save: function (event) {
                        addEvent(this.getTemplateData(), event);
                      //  alert('Save Event:' + this.isNew() + ' --- ' + this.getContentNode().val());
                    },
                    edit: function (event) {
                        addEvent(this.getTemplateData(), event);
                       // editEvent(this.getTemplateData(), event);
                      //  alert('Edit Event:' + this.isNew() + ' --- ' + this.getContentNode().val() + ' --- ' + this.getTemplateData());
                    },
                    delete: function (event) {
                        removeEvent(this.getTemplateData(), event);
                       // alert('Delete Event:' + this.isNew() + ' --- ' + this.getContentNode().val());
                       //  Note: The cancel event seems to be buggy and occurs at the wrong times, so I commented it out.
                              },
                    //          cancel: function(event) {
                    //              alert('Cancel Event:' + this.isNew() + ' --- ' + this.getContentNode().val());
                    //}
                }
            });

            new Y.Scheduler({
                activeView: weekView,
                boundingBox: '#myScheduler',
                date: new Date(),
                eventRecorder: eventRecorder,
                items: $rootScope.events,
                render: true,
                views: [dayView, weekView, monthView, agendaView],
                strings: { agenda: 'Dnevni red', day: 'Dan', month: 'Mjesec', table: 'Tablica', today: 'Danas', week: 'Tjedan', year: 'Godina' },
            }
          );
        });
    }
    //showScheduler();

    var addEvent = function (x, event) {
        $rootScope.events.push({
            //'yuid': event.details[0].newSchedulerEvent._yuid,
            'content': event.details[0].newSchedulerEvent.changed.content,
            'endDate': x.endDate,
            'startDate': x.startDate,
            'room': $scope.room
        });
        var eventObj = {};
        eventObj.content = event.details[0].newSchedulerEvent.changed.content == null ? x.content : event.details[0].newSchedulerEvent.changed.content;
        eventObj.endDate = x.endDate;
        eventObj.startDate = x.startDate;
        eventObj.room = $scope.room;

        remove(eventObj);
        save(eventObj);
        $scope.getSchedulerByRoom();
       // load();
    }

    var save = function (x) {
        $http({
            url: $rootScope.config.backend + webService + '/Save', // '../Scheduler.asmx/Save',
            method: "POST",
            data: '{newEvent:' + JSON.stringify(x) + '}'
        })
        .then(function (response) {
        },
        function (response) {
            alert(response.data.d)
        });
    }

    var removeEvent = function (x, event) {
        var eventObj = {};
        eventObj.content = x.content;
        eventObj.endDate = x.endDate;
        eventObj.startDate = x.startDate;
        eventObj.room = $scope.room;
        remove(eventObj);
        $scope.getSchedulerByRoom();
      //  load();
    }

    var remove = function (x) {
        $http({
            url: $rootScope.config.backend + webService + '/Delete', // '../Scheduler.asmx/Delete',
            method: "POST",
            data: '{newEvent:' + JSON.stringify(x) + '}'
        })
        .then(function (response) {
        },
        function (response) {
            alert(response.data.d)
        });
    }

}])

.controller("servicesCtrl", ['$scope', '$localStorage', '$http', '$rootScope', '$mdDialog', function ($scope, $localStorage, $http, $rootScope, $mdDialog) {
    var webService = 'Files.asmx';
    $rootScope.services = [];
    $scope.units = ["dolazaka", "treninga", "trening", "sat", "min", "mj", "jelovnik", "jelovnika"];
    var getServices = function () {
            $http({
                url: $rootScope.config.backend + webService + '/GetFile', // '../Scheduler.asmx/Load',
                method: 'POST',
                data: { foldername: $rootScope.config.sitename, filename: 'services' }
            })
           .then(function (response) {
               $rootScope.services = JSON.parse(response.data.d);
           },
           function (response) {
               alert(response.data.d)
           });

    };
    getServices();

    //var load = function () {
    //    $http({
    //        url: $rootScope.config.backend + webService + '/Load', // '../Scheduler.asmx/Load',
    //        method: 'POST',
    //        data: ''
    //    })
    //   .then(function (response) {
    //       $rootScope.services = JSON.parse(response.data.d) == "" ? $rootScope.services = [] : JSON.parse(response.data.d);
    //   },
    //   function (response) {
    //       alert(response.data.d)
    //   });
    //}
    //load();


    $scope.addService = function () {
        var price = {
            "quantity": "",
            "unit": "",
            "price": "",
            "duration": 31
        };
        var option = {
            "option": "",
            "prices": [price]
        };
        var service = {
            "service": "",
            "options": [option]
        };
        $rootScope.services.push(service);
    };
    $scope.addService();

    $scope.addOption = function (x) {
        var price = {
            "quantity": "",
            "unit": "",
            "price": "",
            "duration": 31
        };
        var option = {
            "option": "",
            "prices": [price]
        };
        $rootScope.services[x].options.push(option);
    };

    $scope.addPrice = function (serviceIndex, optionIndex) {
        var price = {
            "quantity": "",
            "unit": "",
            "price": "",
            "duration": 31
        };
        $rootScope.services[serviceIndex].options[optionIndex].prices.push(price);
    }

    $scope.removeService = function (x, idx) {
        var confirm = $mdDialog.confirm()
             .title('Dali ste sigurni da želite izbrisati uslugu: "' + x.service + '" ?')
             .targetEvent(idx)
             .ok('DA!')
             .cancel('NE');

        $mdDialog.show(confirm).then(function () {
                $rootScope.services.splice(idx, 1);
            }, function () {
        });
    }

    $scope.removeOption = function (x, parentIdx, idx) {
        var confirm = $mdDialog.confirm()
            .title('Dali ste sigurni da želite izbrisati opciju: "' + x.option + '" ?')
            .targetEvent(idx)
            .ok('DA!')
            .cancel('NE');

        $mdDialog.show(confirm).then(function () {
                $rootScope.services[parentIdx].options.splice(idx, 1);
            }, function () {
        });
    }

    $scope.removePrice = function (x, serviceIndex, parentIdx, idx) {
        var confirm = $mdDialog.confirm()
           .title('Dali ste sigurni da želite izbrisati cijenu: "' + x.quantity + ' ' + x.unit + ', ' + x.price + ' kn, trajanje ' + x.duration + ' dana" ?')
           .targetEvent(idx)
           .ok('DA!')
           .cancel('NE');

        $mdDialog.show(confirm).then(function () {
            $rootScope.services[serviceIndex].options[parentIdx].prices.splice(idx, 1);
            }, function () {
        });
    }

    $scope.saveServices = function () {
       // $http({
       //     url: $rootScope.config.backend + webService + '/Save', // '../Scheduler.asmx/Save',
       //     method: "POST",
       //     data: {service: JSON.stringify($rootScope.services) }
       // })
       //.then(function (response) {
       //},
       //function (response) {
       //    alert(response.data.d)
       //});


        saveJsonToFile($rootScope.config.sitename, 'services', $rootScope.services);

    }

    var saveJsonToFile = function (foldername, filename, json) {
        $http({
            url: $rootScope.config.backend + webService + '/SaveJsonToFile', // '../Files.asmx/SaveJsonToFile',
            method: "POST",
            data: {foldername: foldername, filename: filename, json: JSON.stringify(json) }
        })
        .then(function (response) {
            $rootScope.services = JSON.parse(response.data.d);
            alert('Spremljeno.');
        },
        function (response) {
            alert(response.data.d)
        });
    }
}])

.controller('userCtrl', ['$scope', '$http', '$sessionStorage', '$window', '$rootScope', function ($scope, $http, $sessionStorage, $window, $rootScope) {
    var webService = 'Users.asmx';
    //  $scope.newUser = [];

    $scope.adminTypes = [
       {
           'value': '0',
           'text': 'Supervizor'
       },
       {
           'value': '1',
           'text': 'Admin'
       }
    ];

    $scope.userTypes = [
      {
          'value': '0',
          'text': 'Tip korisnika 1'
      },
      {
          'value': '1',
          'text': 'Tip korisnika 2'
      }
    ];

    $scope.init = function () {
        $http({
            url: $rootScope.config.backend + webService + '/Init', // '../Users.asmx/Init',
            method: "POST",
            data: ""
        })
        .then(function (response) {
            $scope.newUser = JSON.parse(response.data.d);
        },
        function (response) {
            alert(response.data.d)
        });
    }
    $scope.init();

    var load = function () {
        $http({
            url: $rootScope.config.backend + webService + '/Load',
            method: 'POST',
            data: ''
        })
      .then(function (response) {
          $scope.users = JSON.parse(response.data.d);
      },
      function (response) {
          alert(response.data.d)
      });
    };
    load();

    $scope.adminType = function (x) {
        switch (x) {
            case 0:
                return 'Supervizor';
                break;
            case 1:
                return 'Admin';
                break;
            default:
                return '';
        }
    }

    $scope.singup = function () {
        if ($rootScope.user == undefined) { $rootScope.user = $scope.newUser; }
        $scope.newUser.companyName = $rootScope.user.companyName;
        $scope.newUser.address = $rootScope.user.address;
        $scope.newUser.postalCode = $rootScope.user.postalCode;
        $scope.newUser.city = $rootScope.user.city;
        $scope.newUser.country = $rootScope.user.country;
        $scope.newUser.pin = $rootScope.user.pin;
        $scope.newUser.phone = $rootScope.user.phone;
        $scope.newUser.userGroupId = $rootScope.user.userGroupId;
        $scope.newUser.activationDate = new Date();
        $scope.newUser.expirationDate = new Date();
        $scope.newUser.isActive = 1;
        $scope.newUser.ipAddress = "";

        if ($scope.newUser.password == "" || $scope.passwordConfirm == "") {
            alert("Upišite lozinku.");
            return false;
        }
        if ($scope.newUser.password != $scope.passwordConfirm) {
            alert("Lozinke nisu jednake.");
            return false;
        }

        $http({
            url: $rootScope.config.backend + webService + '/Singup',
            method: "POST",
            data: { user: $scope.newUser } // '{user:' + JSON.stringify($scope.newUser) + '}'  //{ user: $scope.newUser }
        })
        .then(function (response) {
            alert(response.data.d)
        },
        function (response) {
            alert(response.data.d)
        });
    }

    $scope.update = function () {
        $http({
            url: $rootScope.config.backend + webService + '/Update',
            method: 'POST',
            data: {user: $rootScope.user}
        }) .then(function (response) {
           alert(response.data.d);
       },
       function (response) {
           alert(response.data.d)
       });
    }

    $scope.updateUser = function (x) {
        $http({
            url: $rootScope.config.backend + webService + '/Update',
            method: 'POST',
            data: { user: x }
        }).then(function (response) {
            alert(response.data.d);
        },
       function (response) {
           alert(response.data.d)
       });
    }



    $scope.showUser = function (x) {
        $http({
            url: $rootScope.config.backend + webService + '/GetUser',
            method: 'POST',
            data: {userId: x}
        })
     .then(function (response) {
         $rootScope.user = JSON.parse(response.data.d);
         $rootScope.currTpl = 'assets/partials/user.html';
     },
     function (response) {
         alert(response.data.d)
     });
    };

}])

.controller('clientCtrl', ['$scope', '$http', '$sessionStorage', '$window', '$rootScope', '$mdDialog', function ($scope, $http, $sessionStorage, $window, $rootScope, $mdDialog) {
    var webService = 'Clients.asmx';
    $scope.showSaveMessage = false;
    $scope.today = new Date();

    $scope.inactiveServiceClass = function (x) {
        if (getDateDiff(x) > 0) {
            return "";
        } else {
            return "text-muted";
        }
        if(x.isFreezed==true){return "text-primary"}
    }

    $scope.activeService = function (x) {
        if (getDateDiff(x) > 0) {
            return true;
        } else {
            return false;
        }
    }

    var getDateDiff = function (x) {
        var today = new Date();
        var date2 = new Date(x);
        var diffDays = parseInt((date2 - today) / (1000 * 60 * 60 * 24));
        return diffDays;
    }

    $scope.newClient = function () {
        $scope.showSaveMessage = false;
        $http({
            url: $rootScope.config.backend + 'Clients.asmx/Init',
            method: "POST",
            data: ""
        })
       .then(function (response) {
           $rootScope.client = JSON.parse(response.data.d);
           $rootScope.client.activationDate = new Date($rootScope.client.activationDate);
       },
       function (response) {
           alert(response.data.d)
       });

        $http({
            url: $rootScope.config.backend + 'ClientServices.asmx/Init',
            method: "POST",
            data: ""
        })
       .then(function (response) {
           $rootScope.clientService = JSON.parse(response.data.d);
       },
       function (response) {
           alert(response.data.d)
       });
    }

    $scope.init = function () {
        $http({
            url: $rootScope.config.backend + 'ClientServices.asmx/Init',
            method: "POST",
            data: ""
        })
        .then(function (response) {
            $rootScope.clientService = JSON.parse(response.data.d);
            $rootScope.clientService.activationDate = new Date($rootScope.clientService.activationDate);
            $rootScope.clientService.expirationDate = new Date($rootScope.clientService.expirationDate);
        },
        function (response) {
            alert(response.data.d)
        });
    }
    $scope.init();

    var getServices = function () {
        $http({
            url: $rootScope.config.backend + 'Files.asmx/GetFile',
            method: 'POST',
            data: { foldername: $rootScope.config.sitename, filename: 'services' }
        })
       .then(function (response) {
           $rootScope.services = JSON.parse(response.data.d);
       },
       function (response) {
           alert(response.data.d)
       });

    };
    getServices();

    $scope.currentService = function (idx) {
        $scope.serviceIndex = idx;
       // $scope.quantityIndex = x;
    }

    $scope.currentOption = function (idx) {
        $scope.optionIndex = idx;
    }

    $scope.currentPrice = function (x, idx) {
        $scope.priceIndex = idx;
       // $rootScope.clientService = x;
        $rootScope.clientService.expirationDate = new Date(new Date($rootScope.clientService.activationDate).setDate(new Date($rootScope.clientService.activationDate).getDate() + x.duration));
        $rootScope.clientService.price = x.price;
        $rootScope.clientService.unit = x.unit;
        $scope.quantity = x.quantity;
        $scope.primaryPrice = $scope.price = x.price;
      //  $scope.price = x.price;
       
        $scope.discount = 0;
    }

    $scope.changePrice = function (qty, discount) {
        var k = $scope.primaryPrice / $scope.quantity;
        $scope.price = Math.round(k * qty);
        //  $scope.newprice = k * qty;
        //$rootScope.clientService.price = k * qty * discount/100;
        $rootScope.clientService.price = Math.round(k * qty - ( k * qty * discount / 100 ));
    }

    var getAllClients = function () {
        $http({
            url: $rootScope.config.backend + webService + '/Load', //'../Clients.asmx/Load',
            method: "POST",
            data: ""
        })
      .then(function (response) {
          $rootScope.clients = JSON.parse(response.data.d);
      },
      function (response) {
          alert(response.data.d)
      });
    };
    getAllClients();
    
    $scope.save = function () {
        if ($rootScope.client == undefined || $rootScope.client.clientId == undefined) {
            save();
        } else {
            update();
        }
    }

    var save = function () {
        $http({
            url: $rootScope.config.backend + webService + '/Save', // '../Clients.asmx/Save',
            method: "POST",
            data: '{client:' + JSON.stringify($rootScope.client) + '}'
        })
     .then(function (response) {
         getAllClients();
         toggleTpl('assets/partials/clients.html', 3);
        // $scope.showClient($rootScope.client);
         //$scope.message = response.data.d
         //$scope.showSaveMessage = true;
     },
     function (response) {
         $scope.message = response.data.d
         $scope.showSaveMessage = true;
     });
    }

    var update = function () {
        $http({
            url: $rootScope.config.backend + webService + '/Update', // '../Clients.asmx/Update',
            method: "POST",
            data: '{client:' + JSON.stringify($rootScope.client) + '}'
        })
       .then(function (response) {
           getAllClients();
           $scope.message = response.data.d
           $scope.showSaveMessage = true;
           //  $rootScope.user = JSON.parse(response.data.d);
           // alert(response.data.d);
       },
       function (response) {
           alert(response.data.d)
           $scope.message = response.data.d
           $scope.showSaveMessage = true;
       });
    }

    var remove = function (x) {
        $http({
            url: $rootScope.config.backend + webService + '/Delete',
            method: 'POST',
            data: { x: x }
        })
        .then(function (response) {
            $scope.newClient();
            getAllClients();
        },
        function (response) {
            alert(response.data.d)
        });
    }

    $scope.remove = function (x) {
        var confirm = $mdDialog.confirm()
              .title('Dali ste sigurni da želite izbrisati člana?')
              .textContent('Id:' + x.clientId + ' , ' + x.firstName + ' ' + x.lastName)
              .targetEvent(x)
              .ok('DA!')
              .cancel('NE');
        $mdDialog.show(confirm).then(function () {
            remove(x);
        }, function () {
        });
    };

    $scope.updateService = function (x) {
        $http({
            url: $rootScope.config.backend + 'ClientServices.asmx/Update',
            method: "POST",
            data: { clientService: x }
        })
     .then(function (response) {
         $scope.getClientServices(x.clientId);
     },
     function (response) {
         alert(response.data.d)
     });
    }

    var removeService = function (x) {
        $http({
            url: $rootScope.config.backend + 'ClientServices.asmx/Delete',
            method: 'POST',
            data: { id: x.id }
        })
        .then(function (response) {
            $scope.getClientServices(x.clientId);
        },
        function (response) {
            alert(response.data.d)
        });
    }

    $scope.removeService = function (x) {
        var confirm = $mdDialog.confirm()
              .title('Dali ste sigurni da želite izbrisati uslugu?')
              .textContent(x.service + ', ' + x.option)
              .targetEvent(x)
              .ok('DA!')
              .cancel('NE');

        $mdDialog.show(confirm).then(function () {
            removeService(x);
        }, function () {
        });
    };

    $scope.showClient = function (x) {
        $rootScope.client = x;
        $rootScope.client.activationDate = new Date($rootScope.client.activationDate);
        $scope.getClientServices(x.clientId);
        //toggleTpl('assets/partials/client.html', 2);
    };

    var toggleTpl = function (x, y) {
        $rootScope.currTpl = x;
        $rootScope.activeTab = y;
    };

    $scope.saveClientService = function (x) {
        x.clientId = $rootScope.client.clientId;
        x.activationDate = x.activationDate || new Date();
        x.expirationDate = x.expirationDate || new Date().setDate(new Date().getDate() + 31);
        x.price = $scope.services[$scope.serviceIndex].options[$scope.optionIndex].prices[$scope.priceIndex].price; // x.price || "";
        x.unit = $scope.services[$scope.serviceIndex].options[$scope.optionIndex].prices[$scope.priceIndex].unit;
        x.quantityLeft = x.quantity;
        $http({
            url: $rootScope.config.backend + 'ClientServices.asmx/Save', // '../ClientServices.asmx/Save',
            method: 'POST',
            data: { clientService: x }
        })
     .then(function (response) {
         $scope.getClientServices(x.clientId);
         $scope.init();
         //$rootScope.clientServices = JSON.parse(response.data.d);
         $scope.message = "Nova usluga je spremljena."
         $scope.showSaveMessage = true;
         //toggleTpl('assets/partials/client.html', 2);
     },
     function (response) {
         alert(response.data.d)
     });
    };

    $scope.editClientCervice = function (x) {
      
        $rootScope.clientService = x;
        $rootScope.clientService.activationDate = new Date(x.activationDate);
        $rootScope.clientService.expirationDate = new Date(x.expirationDate);

    }

    $scope.getClientServices = function (x) {
        $http({
            url: $rootScope.config.backend + 'ClientServices.asmx/GetClientServices',
            method: 'POST',
            data: { clientId: x }
        })
     .then(function (response) {
         $rootScope.clientServices = JSON.parse(response.data.d);
         $rootScope.servicesCount = $rootScope.clientServices.length.toString();
         toggleTpl('assets/partials/client.html', 2);
     },
     function (response) {
         alert(response.data.d)
     });
    };

    $scope.getTotal = function() {
        angular.forEach($rootScope.clientServices, function (value, key) {
            //if ($scope.getDateDiff(value.dateTo) < 0) {
            //    count ++;
            //}
        })
    }

    $scope.freezeService = function (x) {
        $mdDialog.show({
            controller: $scope.freezeServiceCtrl, // RealizationCtrl,
            templateUrl: 'assets/partials/popup/freezeservice.html',
            parent: angular.element(document.body),
            targetEvent: '',
            clickOutsideToClose: true,
            fullscreen: $scope.customFullscreen, // Only for -xs, -sm breakpoints.
            d: { service: x }
        })
       .then(function (result) {
           $scope.updateService(result);
       }, function () {
         
       });
    }

    $scope.freezeServiceCtrl = function ($scope, $mdDialog, d, $http) {
        $scope.d = angular.copy(d.service);
        $scope.d.isFreezed = true;
        $scope.isFreezed = d.service.isFreezed;
        $scope.d.activationDate = new Date($scope.d.activationDate);
        $scope.d.expirationDate = new Date($scope.d.expirationDate);
        $scope.freezeDays = 14;
        $scope.cancel = function () {
            $mdDialog.cancel();
        };

        $scope.setNewDate = function () {
            $scope.d.activationDate.setDate($scope.d.activationDate.getDate() + $scope.freezeDays);
            $scope.d.expirationDate.setDate($scope.d.expirationDate.getDate() + $scope.freezeDays);
        };

        $scope.save = function () {
            $scope.setNewDate();
            $mdDialog.hide($scope.d);
        }

        $scope.freezeOf = function () {
            $scope.d.isFreezed = false;
            $scope.freezeDays = 0;
        }


    };

    $scope.editService = function (x) {
        $mdDialog.show({
            controller: $scope.editServiceCtrl,
            templateUrl: 'assets/partials/popup/editservice.html',
            parent: angular.element(document.body),
            targetEvent: '',
            clickOutsideToClose: true,
            fullscreen: $scope.customFullscreen, // Only for -xs, -sm breakpoints.
            d: { service: x }
        })
       .then(function (result) {
           $scope.updateService(result);
       }, function () {

       });
    }

    $scope.editServiceCtrl = function ($scope, $mdDialog, d, $http) {
        $scope.d = angular.copy(d.service);
        $scope.d.activationDate = new Date(d.service.activationDate);
        $scope.d.expirationDate = new Date(d.service.expirationDate);
        $scope.cancel = function () {
            $mdDialog.cancel();
        };

        $scope.save = function () {
            $mdDialog.hide($scope.d);
        }
    };


}])

.controller('checkinCtrl', ['$scope', '$http', '$sessionStorage', '$window', '$rootScope', '$mdDialog', function ($scope, $http, $sessionStorage, $window, $rootScope, $mdDialog) {
    var webService = 'CheckIn.asmx';
    $scope.date = new Date(new Date().setHours(0, 0, 0, 0));

    $scope.init = function () {
        $http({
            url: $rootScope.config.backend + webService + '/Init',
            method: "POST",
            data: ""
        })
        .then(function (response) {
            $scope.checkIn = JSON.parse(response.data.d);
        },
        function (response) {
            alert(response.data.d)
        });
    }
    $scope.init();

    var getAllClients = function () {
        $http({
            url: $rootScope.config.backend + 'Clients.asmx/Load', // '../Clients.asmx/Load',
            method: "POST",
            data: ""
        })
      .then(function (response) {
          $rootScope.clients = JSON.parse(response.data.d);
      },
      function (response) {
          alert(response.data.d)
      });
    };
    getAllClients();

    $scope.getCheckInsByDate = function () {
        $http({
            url: $rootScope.config.backend + webService + '/GetCheckInsByDate',
            method: 'POST',
            data: { date: $scope.date }
        })
      .then(function (response) {
          $scope.checkIns = JSON.parse(response.data.d);
      },
      function (response) {
          alert(response.data.d)
      });
    };
    $scope.getCheckInsByDate();

    $scope.save = function () {
        $scope.checkIn.checkInTime = $scope.checkIn.checkInTime.toLocaleString();
        $scope.checkIn.userId = $sessionStorage.userid;
        var activeService = JSON.parse($scope.activeService);
        var quantity = activeService.quantity > 10000 ? 'neograničeno' : activeService.quantity;
        $scope.checkIn.service = activeService.service + ' (' + activeService.option + ') ' + quantity + ' ' + activeService.unit
        $http({
            url: $rootScope.config.backend + webService + '/Save',
            method: 'POST',
            data: '{checkIn:' + JSON.stringify($scope.checkIn) + '}'
        })
         .then(function (response) {
            // $scope.checkIns = JSON.parse(response.data.d);
             updateClientServices();
             $scope.getCheckInsByDate();
             $scope.checkIn.clientId = null;
         },
         function (response) {
             alert(response.data.d)
         });
    }

    var updateClientServices = function () {
        var activeService = JSON.parse($scope.activeService);
        activeService.quantityLeft = angular.isNumber(parseInt(activeService.quantityLeft)) ? activeService.quantityLeft - 1 : activeService.quantityLeft;
        $http({
            url: $rootScope.config.backend + 'ClientServices.asmx/Update',
            method: 'POST',
            data: { clientService: activeService }
        })
        .then(function (response) {
        },
        function (response) {
            alert(response.data.d)
        });
    }

    var getCheckInCount = function () {
        $http({
            url: $rootScope.config.backend + webService + '/GetCheckInCount',
            method: 'POST',
            data: { clientId: $scope.checkIn.clientId, service: checkIn.service }
        })
      .then(function (response) {
          $scope.checkInCount = JSON.parse(response.data.d);
      },
      function (response) {
          alert(response.data.d)
      });
    };
    // getCheckInCount();

    $scope.checkOut = function (x) {
        x.checkOutTime = x.checkOutTime.toLocaleString();
        $http({
            url: $rootScope.config.backend + webService + '/CheckOut',
            method: 'POST',
            data: { x: x }
        })
        .then(function (response) {
            // $scope.checkIns = JSON.parse(response.data.d);
            $scope.getCheckInsByDate();
        },
        function (response) {
            alert(response.data.d)
        });
    }

    $scope.checkOutClass = function (x) {
        if (x.isCheckOut == 1) {
            return 'text-muted';
        } else {
            return 'bg-success';
        }
    }

    var remove = function (x) {
        $http({
            url: $rootScope.config.backend + webService + '/Delete',
            method: 'POST',
            data: { id: x.id }
        })
        .then(function (response) {
           // $scope.checkIns = JSON.parse(response.data.d);
            $scope.getCheckInsByDate();
        },
        function (response) {
            alert(response.data.d)
        });
    }

    $scope.remove = function (x) {
        var confirm = $mdDialog.confirm()
              .title('Dali ste sigurni da želite izbrisati unos?')
              .textContent('Br. ' + x.id + ', ' + x.firstName + ' ' + x.lastName)
              .targetEvent(x)
              .ok('DA!')
              .cancel('NE');

        $mdDialog.show(confirm).then(function () {
            remove(x);
        }, function () {
        });
    };

    $scope.GetActiveClientServices = function (x) {
        $http({
            url: $rootScope.config.backend + 'ClientServices.asmx/GetActiveClientServices',
            method: 'POST',
            data: { clientId: x }
        })
     .then(function (response) {
         $scope.activeServices = JSON.parse(response.data.d);
     },
     function (response) {
         alert(response.data.d)
     });
    };

    $scope.showClient = function (x) {
        $http({
            url: $rootScope.config.backend + 'Clients.asmx/GetClient',
            method: 'POST',
            data: { clientId: x.clientId }
        })
     .then(function (response) {
         $rootScope.client = JSON.parse(response.data.d);
         $rootScope.client.activationDate = new Date($rootScope.client.activationDate);
         toggleTpl('assets/partials/client.html', 2);
         getClientServices(x.clientId);
     },
     function (response) {
         alert(response.data.d)
     });
    };

    var getClientServices = function (x) {
        $http({
            url: $rootScope.config.backend + 'ClientServices.asmx/GetClientServices',
            method: 'POST',
            data: { clientId: x }
        })
     .then(function (response) {
         $rootScope.clientServices = JSON.parse(response.data.d);
         $rootScope.servicesCount = $rootScope.clientServices.length.toString();
     },
     function (response) {
         alert(response.data.d)
     });
    };

    var toggleTpl = function (x, y) {
        $rootScope.currTpl = x;
        $rootScope.activeTab = y;
    };

}])

.controller('analyticsCtrl', ['$scope', '$http', '$sessionStorage', '$window', '$rootScope', function ($scope, $http, $sessionStorage, $window, $rootScope) {
    var webService = 'Analytics.asmx';
    $scope.displayType = 0;

    $scope.toggleTpl = function (tpl) {
        $scope.currTpl = tpl;
    };
    $scope.toggleTpl('servicesTpl');

    var getNewDay = function (dateDiff) {
        var date = new Date(new Date().setHours(0, 0, 0, 0));
        var today = date;
        var newDate = date;
        newDate.setDate(today.getDate() + dateDiff);
        return newDate;
    }

    $scope.from = getNewDay(-7);
    $scope.to = new Date(new Date().setHours(23, 59, 59, 59));
    $scope.isPaid = 1;

    $scope.login = function (u, p) {
        $http({
            url: $rootScope.config.backend + 'Users.asmx/Login',
            method: "POST",
            data: { userName: u, password: p }
        })
        .then(function (response) {
            if (JSON.parse(response.data.d).userName == u) {
                $scope.adminUser = JSON.parse(response.data.d);

                if ($scope.adminUser.adminType != 0) {
                    $scope.errorLogin = true;
                    $scope.errorMesage = 'Nemate administracijska prava za pristup ovoj stranici.'
                } else {
                    $scope.getData();
                }
            } else {
                $scope.errorLogin = true;
                $scope.errorMesage = 'Pogrešno korisničko ime ili lozinka.'
            }
        },
        function (response) {
            $scope.errorLogin = true;
            $scope.errorMesage = 'Korisnik nije pronađen.'
        });
    }

    $scope.getData = function () {
       // $scope.getActiveClientServicesCount();
        $scope.getPayedClientServicesCount();
        $scope.getCheckInsCount();
    }

    $scope.getActiveClientServicesCount = function () {
        $http({
            url: $rootScope.config.backend + webService + '/GetActiveClientServicesCountByDate',
            method: "POST",
            data: { from: $scope.from, to: $scope.to, isPaid: $scope.isPaid }
        })
      .then(function (response) {
          $scope.clientsSrvicesCount = JSON.parse(response.data.d);
          setServiceGraphData();
      },
      function (response) {
          alert(response.data.d)
      });
    };

    $scope.getPayedClientServicesCount = function () {
        $http({
            url: $rootScope.config.backend + webService + '/GetPayedClientServicesCountByDate',
            method: "POST",
            data: { from: $scope.from, to: $scope.to, isPaid: $scope.isPaid }
        })
      .then(function (response) {
          $scope.clientsSrvicesCount = JSON.parse(response.data.d);
          setServiceGraphData();
      },
      function (response) {
          alert(response.data.d)
      });
    };

    var setServiceGraphData = function () {
        $scope.series = ['Kupljene usluge'];
        $scope.labels = [];
        $scope.data = [];
        $scope.total = {count:0, price:0};
        angular.forEach($scope.clientsSrvicesCount, function (x, key) {
            $scope.labels.push(x.service + ', ' + x.option);
            $scope.data.push($scope.displayType == 0 ? x.count : x.price);
            $scope.total.count += x.count;
            $scope.total.price += x.price;
        });

        $scope.options = {
            responsive: true,
            maintainAspectRatio: false,
            legend: {
                display: false
            },
            scales: {
                xAxes: [{
                    display: true,
                    scaleLabel: {
                        display: true,
                    },
                    ticks: {
                        beginAtZero: true,
                        stepSize: $scope.displayType == 0 ? 10 : 10000
                    }
                }]
            }
        }
    };

    $scope.changeDisplayType = function (x) {
        $scope.displayType = x;
        setServiceGraphData();
    }

    $scope.getCheckInsCount = function () {
        $http({
            url: $rootScope.config.backend + webService + '/GetCheckInCountByDate',
            method: "POST",
            data: { from: $scope.from, to: $scope.to }
        })
      .then(function (response) {
          $scope.checkInsCount = JSON.parse(response.data.d);
          setCheckInsCountGraphData();
      },
      function (response) {
          alert(response.data.d)
      });
    };

    var setCheckInsCountGraphData = function () {
        $scope.seriesCheckIns = ['Broj dolazaka'];
        $scope.labelsCheckIns = [];
        $scope.dataCheckIns = [];
        angular.forEach($scope.checkInsCount, function (x, key) {
            $scope.labelsCheckIns.push(x.date);
            $scope.dataCheckIns.push(x.count);
        });

        $scope.optionsCheckIns = {
            responsive: true,
            maintainAspectRatio: false,
            legend: {
                display: false
            },
            scales: {
                yAxes: [{
                    display: true,
                    scaleLabel: {
                        display: true,
                    },
                    ticks: {
                        beginAtZero: true,
                        stepSize: 10
                    }
                }]
            }
        }
    };


}])

.controller("messagesCtrl", ['$scope', '$http', '$timeout', '$rootScope', function ($scope, $http, $timeout, $rootScope) {
    var webService = 'MailMessages.asmx';

    var init = function () {
        $http({
            url: $rootScope.config.backend + webService + '/Init',
            method: "POST",
            data: ""
        })
        .then(function (response) {
            $scope.mail = JSON.parse(response.data.d);
        },
        function (response) {
            alert(response.data.d)
        });
    }
    init();

    var load = function () {
        $http({
            url: $rootScope.config.backend + webService + '/Load',
            method: "POST",
            data: ""
        })
        .then(function (response) {
            $scope.messages = JSON.parse(response.data.d);
        },
        function (response) {
            alert(response.data.d)
        });
    }
    load();

    $scope.getGroups = function () {
        $http({
            url: $rootScope.config.backend + webService + '/GetGroups',
            method: "POST",
            data: ""
        })
        .then(function (response) {
            $scope.groups = JSON.parse(response.data.d);
        },
        function (response) {
            alert(response.data.d)
        });
    }
    $scope.getGroups();

    $scope.toogleAll = function () {
            angular.forEach($scope.groups, function (value, key) {
                $scope.groups[key].isSelected = $scope.selectAll;
            })
    }

    $scope.send = function () {
        $http({
            url: $rootScope.config.backend + webService + '/SendNewMail',
            method: 'POST',
            data: { mail: $scope.mail, groups: $scope.groups, sitename: $rootScope.config.sitename }
        })
     .then(function (response) {
         load();
         $scope.message = response.data.d;
         $scope.showSaveMessage = true;
     },
     function (response) {
         alert(response.data.d)
     });
    }

    $scope.show = function (x) {
        $http({
            url: $rootScope.config.backend + webService + '/GetMessage',
            method: "POST",
            data: { mail : x}
        })
        .then(function (response) {
            $scope.mail = JSON.parse(response.data.d);
        },
        function (response) {
            alert(response.data.d)
        });
    }

    var getMailSettings = function () {
        $http({
            url: $rootScope.config.backend + 'Files.asmx/GetFile',
            method: 'POST',
            data: { foldername: $rootScope.config.sitename, filename: 'mailsettings' }
        })
       .then(function (response) {
           $scope.mailSettings = JSON.parse(response.data.d);
       },
       function (response) {
           alert(response.data.d)
       });
    };
    getMailSettings();

    $scope.saveMailSettings = function () {
        saveJsonToFile($rootScope.config.sitename, 'mailsettings', $scope.mailSettings);
    }

    var saveJsonToFile = function (foldername, filename, json) {
        $http({
            url: $rootScope.config.backend + 'Files.asmx/SaveJsonToFile', // '../Files.asmx/SaveJsonToFile',
            method: "POST",
            data: { foldername: foldername, filename: filename, json: JSON.stringify(json) }
        })
        .then(function (response) {
            $scope.mailSettings = JSON.parse(response.data.d);
            alert('Spremljeno.');
        },
        function (response) {
            alert(response.data.d)
        });
    }




}])

;