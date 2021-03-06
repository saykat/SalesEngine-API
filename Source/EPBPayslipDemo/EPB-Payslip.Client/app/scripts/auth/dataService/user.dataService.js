﻿"use strict";

angular.module("psapp")
    .service("UserDataService", [
        "$q", "$http", "RoleDataService", "LocalStorageService",
        function ($q, $http, RoleDataService, LocalStorageService) {

            return {
                get: function() {
                    this.userList = [
                        { Id: 1, Name: "Payslip Admin View", Username: "admin", Password: "123", RoleId: 1 },
                        { Id: 2, Name: "Exporter View", Username: "exporter", Password: "123", RoleId: 2 },
                    ];

                    return this.userList;
                },

                isValidUser: function(user) {

                    if (user) {
                        var userList = this.get();

                        for (var u in userList) {
                            if (userList.hasOwnProperty(u)) {
                                if (userList[u].Username === user.Username && userList[u].Password === user.Password) {
                                    
                                    userList[u].IsLogin = true;
                                    userList[u].Token = "ABFH^456DFJUIFDKSKNAKJHEBHR";

                                    LocalStorageService.setUserInfo(userList[u]);

                                    return true;
                                }
                            }
                        }

                        return undefined;
                    }
                }
            }
        }
    ]);