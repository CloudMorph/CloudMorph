/// <reference path="../backbone.js" />
/// <reference path="../jquery-1.7.1-vsdoc.js" />

(function ($) {

    Backbone.sync = function (method, model) {
        alert(method + ": " + model.url);
    };

    var Job = Backbone.Model.extend({
        name: null
    });

    var JobView = Backbone.View.extend({
        tagName: 'li',

        initialize: function () {
            _.bindAll(this, 'render');
        },

        render: function () {
            $(this.el).html(this.model.get('name'));
            return this;
        }
    });

    var Jobs = Backbone.Collection.extend({
        model: Job,
        url: "/svc/Jobs"
    });

    var HostsView = Backbone.View.extend({

        el: $("#hosts"),

        initialize: function () {
            _.bindAll(this, 'render');


            /*
            var job1 = new Job({ name: "job1" });
            var job2 = new Job({ name: "job1" });
            var job3 = new Job({ name: "job1" });

            this.jobCollection = new Jobs([job1, job2, job3]);
            //this.collection.bind
            */
            this.jobCollection = new Jobs();
            this.jobCollection.fetch();

            this.render();
        },

        render: function () {
            //$(this.el).append("<strong>Hello</strong>");
            //$(this.el).append("<ul></ul>");
            var $list = $('<ul></ul>');
            _(this.jobCollection.models).each(function (item) {
                //$('ul', this.el).append("<li>" + item.get('name') + "</li>");
                //$list.append("<li>" + item.get('name') + "</li>");
                var itemView = new JobView({
                    model: item
                });
                $list.append(itemView.render().el);
            });
            $(this.el).append($list);
        }
    });

    var listView = new HostsView();

    /*
    // Models
    window.Hosts = Backbone.Model.extend();

    window.HostsCollection = Backbone.Collection.extend({
    model: Hosts,
    url: "/svc/Hosts"
    });

    // Views
    window.HostListView = Backbone.View.extend({



    });
    */
    // Router
    var AppRouter = Backbone.Router.extend({

        routes: {
            "do": "list"
        },

        list: function () {
            this.hostList = new HostsCollection();
            this.hostListView = new HostListView({ model: this.hostList });
            this.hostList.fetch();
            $('#body').html(this.hostListView.render().el);
        }

    });

    var app = new AppRouter();

    Backbone.history.start();

})(jQuery);