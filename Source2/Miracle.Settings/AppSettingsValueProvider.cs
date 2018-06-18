using System.Configuration;

namespace Miracle.Settings
{
#if NETFULL
	/// <summary>
	/// Value provider using ConfigurationManager.AppSettings as values
	/// </summary>
	public class AppSettingsValueProvider : NameValueValueProvider
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public AppSettingsValueProvider()
			: base(ConfigurationManager.AppSettings)
		{
		}
	}
#endif

}
