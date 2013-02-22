/// <reference path="./js/jquery.d.ts" />
/// <reference path="./js/backbone.d.ts" />

/*
declare module "./data" { 
    function getData(): number;
}

import data = module("./data");

var myData = data.getData();
*/

module GuestBook {
    "use strict";

    export class App {

        constructor () {

            //var formData = new FormData($('form')[0]);
            $.ajax('api/files/uploadfile?folder=' + $('#ddlFolders').val() + '&newfileName=' + $('#txtNewFileName').val(),
                {
                    type: 'POST',
                    success: () => {
                        // $("#divResult").html(data);
                    },
                    //data: formData,
                    cache: false,
                    //contentType: false,
                    processData: false
                });

            $('#SignButton').click(() => {
                alert("hello");
            });
        }

        updateEntries(data: any) {
        }

    }
}

$(function () {
    var app = new GuestBook.App();

    window.setInterval(() => {
        $.get("/api/entries", (data: any) => {
            alert("Got some data");

            app.updateEntries(data);
        });
    }, 15000);
})