using System.IO;
using NUnit.Framework;

namespace Miracle.Settings.Tests
{
#if NETCORE

	using Microsoft.Extensions.Configuration;

	static class CoreWrapper
    {
	    public static IConfigurationRoot JsonConfig()
	    {
		    var builder = new ConfigurationBuilder()
			    .SetBasePath(TestContext.CurrentContext.TestDirectory)
			    .AddJsonFile("appsettings.json");

		    return builder.Build();
		}
	    public static IConfigurationRoot XmlConfig()
	    {
		    var builder = new ConfigurationBuilder()
			    .SetBasePath(TestContext.CurrentContext.TestDirectory)
			    .AddXmlFile("app.config");

		    return builder.Build();
	    }
    }
#endif
}
