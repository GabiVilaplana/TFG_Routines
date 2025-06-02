using System;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Controls;


namespace Routines.Resources.Localization
{
    [ContentProperty(nameof(Text))]
    public class TranslateExtension : IMarkupExtension
    {
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";

            var ci = Thread.CurrentThread.CurrentUICulture;
            var translation = AppResources.ResourceManager.GetString(Text, ci);

            return translation ?? $"[{Text}]";
        }
    }
}