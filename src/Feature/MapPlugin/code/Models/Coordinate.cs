using Newtonsoft.Json;

namespace Liquid.Feature.MapPlugin.Models
{
    public class Coordinate
    {
        [JsonProperty(PropertyName = "latitude")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public double Longitude { get; set; }
    }
}