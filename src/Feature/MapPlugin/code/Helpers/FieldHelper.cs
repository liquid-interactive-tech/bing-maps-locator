using Liquid.Feature.MapPlugin.Helpers.Interfaces;
using System;
using Sitecore;

namespace Liquid.Feature.MapPlugin.Helpers
{
    public static class FieldHelper
    {
        public static void SetControlProperties(IFieldHelper control)
        {
            if (String.IsNullOrEmpty(control.Source))
                return;

            if (!String.IsNullOrEmpty(control.Source.IsAddressField()))
            {
                control.Attributes.Add("class", Resources.Resource.scAddressFieldCssClass);
                control.Attributes[Resources.Resource.scAddressFieldDataAttribute] = control.Source.IsAddressField();
            }

            if (!String.IsNullOrEmpty(control.Source.IsLatitudeField()))
            {
                control.Attributes[Resources.Resource.scLatitudeFieldDataAttribute] = control.Source.IsAddressField();
            }

            if (!String.IsNullOrEmpty(control.Source.IsLongitudeField()))
            {
                control.Attributes[Resources.Resource.scLongitudeFieldDataAttribute] = control.Source.IsAddressField();
            }

            if (!String.IsNullOrEmpty(control.Source.IsLabelField()))
            {
                control.Attributes[Resources.Resource.scLabelFieldDataAttribute] = control.Source.IsLabelField();
            }
        }

        private static string IsAddressField(this string Source)
        {
            return StringUtil.ExtractParameter(Resources.Resource.scAddressSource, Source).Trim();
        }

        private static string IsLatitudeField(this string Source)
        {
            return StringUtil.ExtractParameter(Resources.Resource.scLatitudeSource, Source).Trim();
        }

        private static string IsLongitudeField(this string Source)
        {
            return StringUtil.ExtractParameter(Resources.Resource.scLongitudeSource, Source).Trim();
        }

        private static string IsLabelField(this string Source)
        {
            return StringUtil.ExtractParameter(Resources.Resource.scLabelSource, Source).Trim();
        }
    }
}