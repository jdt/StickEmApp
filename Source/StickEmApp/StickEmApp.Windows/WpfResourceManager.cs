using System.ComponentModel.Composition;
using System.Windows;
using StickEmApp.Service;

namespace StickEmApp.Windows
{
    [Export(typeof(IResourceManager))]
    public class WpfResourceManager : IResourceManager
    {
        public string GetString(string resourceName)
        {
            return Application.Current.Resources[resourceName].ToString();
        }
    }
}