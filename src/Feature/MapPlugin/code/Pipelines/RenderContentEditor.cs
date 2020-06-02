using Liquid.Feature.MapPlugin.Repositories;
using Sitecore.Diagnostics;
using Sitecore.Pipelines;
using Sitecore.StringExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Liquid.Feature.MapPlugin.Pipelines
{
    public class RenderContentEditor
    {
        private const string JavascriptTag = "<script src=\"{0}\"></script>";
        private const string JavascriptAsyncTag = "<script src=\"{0}\" async defer></script>";
        private const string StylesheetLinkTag = "<link href=\"{0}\" rel=\"stylesheet\" />";

        public void Process(PipelineArgs args)
        {
            // Load resources from config file.
            AddControls(JavascriptTag, Resources.Resource.ConfigNodeScripts);
            AddControls(StylesheetLinkTag, Resources.Resource.ConfigNodeStyles);

            // Load bing maps dynamically.
            AddBingMapsControl();
        }

        private void AddControls(string resourceTag, string configKey)
        {
            Assert.IsNotNullOrEmpty(configKey, "Content Editor resource config key cannot be null");

            string resources = Sitecore.Configuration.Settings.GetSetting(configKey);

            if (String.IsNullOrEmpty(resources))
                return;

            foreach (var resource in resources.Split('|'))
            {
                Sitecore.Context.Page.Page.Header.Controls.Add((Control)new LiteralControl(resourceTag.FormatWith(resource)));
            }
        }

        private void AddBingMapsControl()
        {
            string apiKey = new MapPluginRepository().GetBingMapsApiKey();
            string baseUrl = "https://www.bing.com/api/maps/mapcontrol?key={0}";

            if (String.IsNullOrEmpty(apiKey))
                return;

            Sitecore.Context.Page.Page.Header.Controls.Add((Control)new LiteralControl(JavascriptAsyncTag.FormatWith(String.Format(baseUrl, apiKey))));
        }
    }
}