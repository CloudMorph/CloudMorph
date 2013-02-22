/* File Created: March 5, 2012 */
(function ($, undefined) {
    function initialize() {

        // Toggle main menu selection
        $("#mnuMain li.active").toggleClass("active");
        $("#mnuPackages").toggleClass("active");

        // Overall viewmodel for this screen, along with initial state
        function AppViewModel() {
            var self = this;

            // Non-editable catalog data - would come from the server
            self.packages = ko.observableArray([
                { id: "662FFB77-8BBF-46AF-A9A1-BB9F7DF8EB04", description: null, realm: "local", size: 25 },
                { id: "0837ACE5-08C2-42A7-8EA4-FC1EA4EE33BB", description: null, realm: "aws", size: 10 },
                { id: "2B795ECD-6AC9-4C2A-9B73-6DA08114A58E", description: null, realm: "aws", size: 35 }
            ]);

                self.totalSize = ko.computed(function () {

                    var total = 0;

                    for (var i = 0; i < self.packages().length; i++)
                        total += self.packages()[i].size;

                    return total;
                });
        }

        ko.applyBindings(new AppViewModel());
    }

    $(function () {
        initialize();
    });    
})(jQuery);