/**
 * Map.
 *
 * @link   https://www.liquidint.com
 * @file   Loads presentation layer functionality for Bing Maps.
 * @author Interactive Liquid LLC.
 * @since  1.0.0
 */
var Liquid = Liquid || {};
Liquid.MapPlugin = function () {

    var map;
    var guid;

    GetMap = function (id, options) {
        if (!id)
            return;

        guid = `{${id.replace("map-", "")}}`;

        map = new Microsoft.Maps.Map(`#${id}`, options);

        GetPushPins();
    },

    DisplayInfobox = function (e) {
        pinInfobox.setOptions({ title: e.target.Title, description: e.target.Description, visible: e.target.Title ? true : false, offset: new Microsoft.Maps.Point(0, 25) });
        pinInfobox.setLocation(e.target.getLocation());
    },

    GetPushPins = function () {
        var xhr = new XMLHttpRequest();
        xhr.open("POST", "/api/sitecore/MapPlugin/GetPushPins", true);
        xhr.setRequestHeader("Content-Type", "application/json");
        xhr.send(JSON.stringify({
            itemId: guid
        }));
        xhr.onload = function () {
            var data = JSON.parse(this.responseText);

            if (data) {
                SetPushPins(data);
            }
        };
    },

    SetPushPins = function (data) {

        var infoboxLayer = new Microsoft.Maps.EntityCollection();
        var pinLayer = new Microsoft.Maps.EntityCollection();

        pinInfobox = new Microsoft.Maps.Infobox(new Microsoft.Maps.Location(0, 0), { visible: false });
        infoboxLayer.push(pinInfobox);

        var locs = [];
        for (var i = 0; i < data.length; i++) {
            locs[i] = new Microsoft.Maps.Location(data[i].lat, data[i].lng);
            var pin = new Microsoft.Maps.Pushpin(locs[i]);
            pin.Title = data[i].title;
            pin.Description = data[i].description;
            pinLayer.push(pin);
            Microsoft.Maps.Events.addHandler(pin, "click", DisplayInfobox);
        }

        map.entities.push(pinLayer);
        map.entities.push(infoboxLayer);

        var bounds = Microsoft.Maps.LocationRect.fromLocations(locs);
        map.setView({ bounds: bounds, zoom: 10 });
    },

    AutoSuggest = function () {
        Microsoft.Maps.loadModule("Microsoft.Maps.AutoSuggest", {
            callback: function () {
                var manager = new Microsoft.Maps.AutosuggestManager({
                    placeSuggestions: false
                });
                manager.attachAutosuggest(".bing-input", ".bing-container", AutoFillSuggestion);
            },
            errorCallback: function (msg) {
                console.log("Map Plugin API Error.", msg);
            }
        });
    },

    AutoFillSuggestion = function (result) {
        // extend autofill functionality here.
        return LocateNearestPushPin(result);
    },

    LocateNearestPushPin = function (result) {
        var xhr = new XMLHttpRequest();
        xhr.open("POST", "/api/sitecore/MapPlugin/GetNearestPin", true);
        xhr.setRequestHeader("Content-Type", "application/json");
        xhr.send(JSON.stringify({
            coordinate: {
                latitude: result.location.latitude,
                longitude: result.location.longitude
            },
            itemId: guid
        }));
        xhr.onload = function () {
            var data = JSON.parse(this.responseText);

            if (data) {
                var location = new Microsoft.Maps.Location(data.lat, data.lng);
                map.setView({ center: location, zoom: 17 });

            }
        };
    }

    return {
        GetMap: GetMap,
        AutoSuggest: AutoSuggest
    };

}();