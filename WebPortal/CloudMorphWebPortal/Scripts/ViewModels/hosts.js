/// <reference path="../knockout.js" />
/// <reference path="../jquery-1.7.1-vsdoc.js" />
/// <reference path="../backbone.js" />
/// <reference path="../underscore.js" />

$(function () {

    function formatHostUrl(value) {
        return "/hosts/" + value;
    }

    var viewModel = {

        hosts: ko.observableArray(),

        selectedHost: ko.observable(),

        selectHost: function (id) {
            var self = this;

            _.each(self.hosts(), function (host) {
                if (host.Guid === id)
                    self.selectedHost(host);
            });
        }

    };

    ko.applyBindings(viewModel);

    // Models
    var Job = Backbone.Model.extend({
        parse: function (response) {
            return {
                name: response.Name,
                guid: response.Guid,
                size: response.Size
            };
        }
    });

    var Jobs = Backbone.Collection.extend({
        model: Job,

        parse: function (response) {
            return response; //.results;
        }
    });

    var Host = Backbone.Model.extend({
        name: null,
        guid: null,
        ip: null
    });

    // Views

    var JobsView = Backbone.View.extend({
        //tagName: "table",

        initialize: function () {
            _.bindAll(this, 'render'); // remember: every function that uses 'this' as the current object should be in here
            this.model.bind("reset", this.render, this);
        },

        render: function () {
            _.each(this.model.models, function (job) {
                $(this.el).append(new JobListItemView({ model: job }).render().el);
            }, this);
            return this;
        }
    });

    var JobListItemView = Backbone.View.extend({
        tagName: "tr",

        template: _.template($('#tpl_job_item_row').html()),

        render: function (eventName) {
            //$(this.el).html(this.template(this.model.toJSON()));
            //$(this.el).append($("<td>").append(this.model.get('name')));
            $(this.el).html(this.template(this.model.toJSON()));
            //(this.el).html("This is me!");
            return this;
        }
    });

    var HostView = Backbone.View.extend({
        //tagName: 'table',
        //template: "#some-template",

        template: _.template($('#tpl_host_item').html()),

        //el: $("#singleHostPage"),

        initialize: function () {
            _.bindAll(this, 'render');
            //this.model.bind("reset", this.render, this);

            //this.render(); // not all views are self-rendering. This one is.
        },

        //render: function (eventName) {
        render: function () {
            //var html = $(this.template).tmpl(this.model);
            //$(this.el).html(this.model.get('name'));

            //_.each(this.model.models, function (wine) {
            //    $(this.el).append(new WineListItemView({model:wine}).render().el);
            //}, this);

            //(this.el).html("This is me!");
            //$(this.el).append("<ul> <li>hello world</li> </ul>");
            $(this.el).html(this.template(this.model.toJSON()));
            //$(this.el).html(this.template());

            return this;
        }
    });

    var AppRouter = Backbone.Router.extend({
        routes: {
            "hosts/:id": "getHost",
            'delete/:id': 'deleteMe',
            "*actions": "defaultRoute" // Backbone will try match the route above first
        },

        getHost: function (id) {
            viewModel.selectHost(id);

            $("#hostsPage").toggle('slide', { direction: 'left' }, 500, function () {
                $("#singleHostPage").toggle('slide', { direction: 'right' }, 500);
            });

            var host = viewModel.selectedHost();

            // Render host
            this.hostBB = new Host({ name: host.Name, guid: host.Guid, ip: host.IP });
            this.hostViewBB = new HostView({ model: this.hostBB });
            $('#singleHostPage').html(this.hostViewBB.render().el);

            if (!('getPackages' in host)) {
                host.getPackages = function (hostId) {

                    var url = "/svc/hosts/" + id + "/packages";
                    //Jobs.url = url;
                    //Jobs.fetch();
                    this.jobsList = new Jobs();
                    this.jobsList.url = url;
                    this.jobsView = new JobsView({ el: $("#runningJobs"), model: this.jobsList });
                    this.jobsList.fetch();

                    //$.get("/svc/hosts/" + id + "/packages", null, function (data) {

                    //    Jobs.url = 

                    //if (host != undefined) {
                    //host.packages = ko.observable(data);

                    //viewModel.selectedHost(host);

                    //window.SingleHostView = new HostView({ el: $("#singleHostPage") });

                    //}
                    //});
                };
            }
            host.getPackages(id);
        },

        deleteMe: function (id) {
            alert("Delete: " + id);
        },

        defaultRoute: function (actions) {
            //alert("Hmm: " + actions);
            $("#hostsPage").toggle('slide', { direction: 'left' }, 500);
        }
    });

    var app_router = new AppRouter;
    //Initiate a new history and controller class
    Backbone.emulateHTTP = true;
    Backbone.emulateJSON = true
    Backbone.history.start({
        root: "/"
    });

    $.get("/svc/hosts", "", function (data) {

        viewModel.hosts(data);

    });

});

// Setup the ajax indicator
$('#reloadPackages').append('<div id="ajaxBusy"><p><img src="/Content/images/ajax-loader.gif"></p></div>');

  $('#ajaxBusy').css({
    display:"none",
    margin:"0px",
    paddingLeft:"0px",
    paddingRight:"0px",
    paddingTop:"0px",
    paddingBottom:"0px",
    position:"absolute",
    right:"3px",
    top:"3px",
     width:"auto"
 });

 // Ajax activity indicator bound to ajax start/stop document events
 $(document).ajaxStart(function () {
     $('#ajaxBusy').show();
 }).ajaxStop(function () {
     $('#ajaxBusy').hide();
 });
