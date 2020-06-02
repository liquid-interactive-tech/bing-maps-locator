/**
 * Content Editor.
 *
 * @link   https://www.liquidint.com
 * @file   Loads Bing Map functionality in Sitecore backoffice.
 * @author Interactive Liquid LLC.
 * @since  1.0.0
 */
var Liquid = Liquid || {};
Liquid.MapPlugin = function () {
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
        if (!$sc("input[data-location-label]").val()) {
            $sc("input[data-location-label]").val(result.formattedSuggestion);
        }
        $sc("input[data-location-latitude]").val(result.location.latitude);
        $sc("input[data-location-longitude]").val(result.location.longitude);
    }

    return {
        AutoSuggest: AutoSuggest
    };
}();
document.observe("sc:contenteditorupdated", function (event) {
    // Set Bing Maps container on the text box.
    $sc(".bing-input").parent().addClass("bing-container");

    // Disable overflow style for this field group; allows select list to display properly.
    $sc(".bing-input").parents("table.scEditorSectionPanel").css("overflow", "visible");

    // Init. AutoSuggest.
    Liquid.MapPlugin.AutoSuggest();
});