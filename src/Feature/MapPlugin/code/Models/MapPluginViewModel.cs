using Sitecore.Data.Items;

namespace Liquid.Feature.MapPlugin.Models
{
    public class MapPluginViewModel
    {
        public string ApiKey { get; set; }

        public bool DisableZooming { get; set; }

        public bool DisableSearch { get; set; }

        public string MapTypeId { get; set; }

        public string Guid { get; set; } 

        public Item Item { get; set; }
    }
}