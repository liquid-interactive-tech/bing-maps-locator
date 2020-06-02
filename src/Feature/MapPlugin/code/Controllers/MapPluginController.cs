using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using Liquid.Feature.MapPlugin.Models;
using Liquid.Feature.MapPlugin.Repositories.Interfaces;

namespace Liquid.Feature.MapPlugin.Controllers
{
    public class MapPluginController : Controller
    {
        protected readonly IMapPluginRepository _repository;

        public MapPluginController(IMapPluginRepository repository)
        {
            this._repository = repository;
        }
    
        public ActionResult Index()
        {
            return View(GetModel());
        }

        [HttpPost]
        public ContentResult GetPushPins(string itemId)
        {
            if (String.IsNullOrEmpty(itemId))
                return Content("Missing item id.");

            Response.ContentType = "application/json";

            return Content(_repository.GetSerializedPushPins(itemId));
        }

        [HttpPost]
        public ContentResult GetNearestPin(Coordinate coordinate, string itemId)
        {
            if (String.IsNullOrEmpty(itemId))
                return Content("Missing item id.");

            Response.ContentType = "application/json";

            var response = _repository.GetNearestPushPin(coordinate, itemId);

            return Content(JsonConvert.SerializeObject(response));

        }

        protected object GetModel()
        {
            return _repository.GetModel();
        }
    }
}