using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;

namespace Miracle.Settings
{
    public partial class SettingsLoader
    {
        /// <summary>
        /// List of value providers that are searched until a value is found.
        /// </summary>
        public List<Func<string,string>> PathProviders { get; }

	    private string GetFullPath(string path)
	    {
		    foreach (var pathProvider in PathProviders)
		    {
			    var result = pathProvider(path);
			    if (!string.IsNullOrEmpty(result))
			    {
				    return result;
			    }
		    }

		    return path;
	    }

	    private List<Func<string, string>> GetDefaultPathConverters()
	    {
		    return new List<Func<string, string>>()
		    {
#if NETFULL
				System.Web.Hosting.HostingEnvironment.MapPath,
#endif
				Path.GetFullPath,
		    };
	    }

		/// <summary>
		/// Add value provider to list of value providers
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		public SettingsLoader AddPathProvider(Func<string, string> provider)
        {
            PathProviders.Add(provider);
            return this;
        }
	}
}