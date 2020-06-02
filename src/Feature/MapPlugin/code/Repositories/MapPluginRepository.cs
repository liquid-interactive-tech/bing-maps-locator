using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Liquid.Feature.MapPlugin.Models;
using Liquid.Feature.MapPlugin.Repositories.Interfaces;
using Newtonsoft.Json;
using Sitecore.Data.Fields;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;

namespace Liquid.Feature.MapPlugin.Repositories
{
    public class MapPluginRepository : IMapPluginRepository
    {
        public MapPluginViewModel GetModel()
        {
            // Instantiate Map Model.
            MapPluginViewModel model = new MapPluginViewModel();
            
            // Current Rendering Item.
            var contentItem = RenderingContext.Current?.Rendering?.Item ?? null;
            model.Item = contentItem;
            
            if (contentItem == null)
                return model;

            // Get Bing Maps API Key.
            model.ApiKey = GetBingMapsApiKey();

            // Set Property Values.
            CheckboxField disableZoomField = contentItem.Fields[Resources.Resource.scFieldDisableZooming];
            model.DisableZooming = disableZoomField == null ? false : disableZoomField.Value == "1";

            CheckboxField disableSearchField = contentItem.Fields[Resources.Resource.scFieldDisableSearch];
            model.DisableSearch = disableSearchField == null ? false : disableSearchField.Value == "1";

            ReferenceField mapTypeField = contentItem.Fields[Resources.Resource.scFieldType];
            model.MapTypeId = (mapTypeField == null || String.IsNullOrEmpty(mapTypeField?.TargetItem?.Fields["Value"]?.Value)) ? "Microsoft.Maps.MapTypeId.road" : mapTypeField?.TargetItem?.Fields["Value"]?.Value;

            model.Guid = contentItem.ID.Guid.ToString();

            return model;
        }

        public string GetSerializedPushPins(string forItemId)
        {
            List<PushPinModel> pinItems = new List<PushPinModel>();

            try
            {
                // Get Map Plugin Item.
                var contentItem = Sitecore.Context.Database.GetItem(forItemId);

                pinItems = GetPushPins(contentItem);
            }
            catch (Exception)
            {
                Sitecore.Diagnostics.Log.Error("Error - Failed to get Map Plugin Pins", this);
            }

            return JsonConvert.SerializeObject(pinItems);
        }

        public PushPinModel GetNearestPushPin(Coordinate coordinate, string forItemId)
        {
            try
            {
                // Get Map Plugin Item.
                var contentItem = Sitecore.Context.Database.GetItem(forItemId);

                // Get Pins.
                List<PushPinModel> pins = GetPushPins(contentItem);

                pins.ForEach(x => x.Distance = GetDistance(coordinate, Convert.ToDouble(x.Latitude), Convert.ToDouble(x.Longitude)));
                return pins.OrderBy(x => x.Distance).FirstOrDefault();

            }
            catch (Exception)
            {
                Sitecore.Diagnostics.Log.Error("Error - Failed to get nearest location.", this);
            }
            return null;
        }

        public string GetBingMapsApiKey()
        {
            try
            {
                // Get Map Plugin Settings Item.
                Sitecore.Data.Database master = Sitecore.Configuration.Factory.GetDatabase("master");
                var contentItem = master.GetItem(Resources.Resource.scMapSettingsGuid);

                if (contentItem == null)
                    return null;

                // Get Bing Maps API Key.
                TextField apiKeyField = contentItem?.Fields[Resources.Resource.scFieldKey];
                return apiKeyField?.Value ?? string.Empty;
            } catch (Exception)
            {
                return null;
            }
        }

        private List<PushPinModel> GetPushPins(Item contentItem)
        {
            Regex coordinate = new Regex(@"([ns]?(?: ?[+-]?\d+(?:\.\d+)?[°´’'""d:]?){1,3} ?[ns]?) ?,? ?([ew]?(?: ?[+-]?\d+(?:\.\d+)?[°´’'""d:]?){1,3} ?[ew]?)");

            MultilistField pinsField = contentItem.Fields[Resources.Resource.scFieldPins];
            return pinsField.GetItems().Where(x => x.Fields[Resources.Resource.scFieldLatitude] != null && x.Fields[Resources.Resource.scFieldLongitude].Value != null)
                                .Where(x => coordinate.IsMatch(x.Fields[Resources.Resource.scFieldLatitude].Value) && coordinate.IsMatch(x.Fields[Resources.Resource.scFieldLongitude].Value))
                                .Select(x => new PushPinModel()
                                {
                                    Label = x.Fields[Resources.Resource.scFieldLabel].Value,
                                    Latitude = x.Fields[Resources.Resource.scFieldLatitude].Value,
                                    Longitude = x.Fields[Resources.Resource.scFieldLongitude].Value
                                }).ToList();
        }

        private double GetDistance(Coordinate origin, double latitude, double longitude)
        {
            if ((origin.Latitude == latitude) && (origin.Longitude == longitude))
            {
                return 0;
            }
            else
            {
                double LongitudeDifference = origin.Longitude - longitude;
                double dist = Math.Sin(GetRadius(origin.Latitude)) * Math.Sin(GetRadius(latitude)) + Math.Cos(GetRadius(origin.Latitude)) * Math.Cos(GetRadius(latitude)) * Math.Cos(GetRadius(LongitudeDifference));
                dist = Math.Acos(dist);
                dist = GetDegrees(dist);
                dist = dist * 60 * 1.1515;
                dist = dist * 1.609344;

                return (dist);
            }
        }

        private double GetRadius(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        private double GetDegrees(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
    }
}