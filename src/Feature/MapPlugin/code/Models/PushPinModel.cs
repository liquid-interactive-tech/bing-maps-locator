using Newtonsoft.Json;

namespace Liquid.Feature.MapPlugin.Models
{
    public class PushPinModel
    {
        [JsonProperty(PropertyName = "title")]
        public string Label { get; set; }

        [JsonProperty(PropertyName = "lat")]
        public string Latitude { get; set; }

        [JsonProperty(PropertyName = "lng")]
        public string Longitude { get; set; }

        public double Distance { get; set; }
    }
}