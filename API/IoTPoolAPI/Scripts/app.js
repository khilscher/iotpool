var ViewModel = function () {
    var self = this;
    self.pool = ko.observableArray();
    self.error = ko.observable();

    var poolUri = '/api/pool/';

    function ajaxHelper(uri, method, data) {
        self.error(''); // Clear error message
        return $.ajax({
            type: method,
            url: uri,
            dataType: 'json',
            contentType: 'application/json',
            data: data ? JSON.stringify(data) : null
        }).fail(function (jqXHR, textStatus, errorThrown) {
            self.error(errorThrown);
        });
    }

    function getAllTelemetry() {
        ajaxHelper(poolUri, 'GET').done(function (data) {
            self.pool(data);
        });
    }

    // Fetch the initial data.
    getAllTelemetry();
};

ko.applyBindings(new ViewModel());