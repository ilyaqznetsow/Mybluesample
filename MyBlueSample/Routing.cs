using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MyBlueSample
{
    public static class Routing
    {
        private static IDictionary<Type, Type> _dictionary = new Dictionary<Type, Type>();

        public static Page GetPage(Type vmType)
        {
            var pageType = _dictionary[vmType];
            var page = (Page)Activator.CreateInstance(pageType);
            page.BindingContext = Activator.CreateInstance(vmType);
            return page;
        }

        public static void RegisterRoute(Type vmType, Type pageType)
        {
            if (!_dictionary.ContainsKey(vmType))
                _dictionary.Add(vmType, pageType);
            else
                _dictionary[vmType] = pageType;
        }

        public static void UnRegisterRoutes() => _dictionary.Clear();
    }
}