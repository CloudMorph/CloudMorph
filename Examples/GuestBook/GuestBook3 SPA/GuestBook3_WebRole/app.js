var GuestBook;
(function (GuestBook) {
    "use strict";
    var App = (function () {
        function App() {
            $.ajax('api/files/uploadfile?folder=' + $('#ddlFolders').val() + '&newfileName=' + $('#txtNewFileName').val(), {
                type: 'POST',
                success: function () {
                },
                cache: false,
                processData: false
            });
            $('#SignButton').click(function () {
                alert("hello");
            });
        }
        App.prototype.updateEntries = function (data) {
        };
        return App;
    })();
    GuestBook.App = App;    
})(GuestBook || (GuestBook = {}));

$(function () {
    var app = new GuestBook.App();
    window.setInterval(function () {
        $.get("/api/entries", function (data) {
            alert("Got some data");
            app.updateEntries(data);
        });
    }, 15000);
});
