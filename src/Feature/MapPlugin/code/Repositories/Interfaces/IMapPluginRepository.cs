using Liquid.Feature.MapPlugin.Models;

namespace Liquid.Feature.MapPlugin.Repositories.Interfaces
{
    public interface IMapPluginRepository
    {
        MapPluginViewModel GetModel();

        string GetSerializedPushPins(string forItemId);

        PushPinModel GetNearestPushPin(Coordinate coordinate, string forItemId);

        string GetBingMapsApiKey();
    }
}