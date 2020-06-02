using System.Web.UI;

namespace Liquid.Feature.MapPlugin.Helpers.Interfaces
{
    public interface IFieldHelper
    {
        string Source { get; set; }
        string Class { get; set; }
        AttributeCollection Attributes { get; }
    }
}