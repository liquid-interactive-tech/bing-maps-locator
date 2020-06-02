using System;
using Liquid.Feature.MapPlugin.Helpers.Interfaces;
using Sitecore.Shell.Applications.ContentEditor;

namespace Liquid.Feature.MapPlugin.Helpers
{
    public class TextboxFieldHelper : Text, IFieldHelper
    {
        public TextboxFieldHelper()
            : base()
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            FieldHelper.SetControlProperties(this);
        }

        public string Source { get; set; }
    }
}