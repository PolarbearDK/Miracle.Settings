using System.Xml;

namespace Miracle.Settings.Tests
{
    public abstract class LoadTestBase: LoadFailTestBase
    {
        protected readonly ISettingsLoader SettingsLoader;

		protected LoadTestBase(ISettingsLoader settingsLoader)
		{
			SettingsLoader = settingsLoader;
		}

		//protected LoadTestBase()
		//	: this(new SettingsLoader()
		//		.AddTypeConverter(s => XmlConvert.ToDateTime(s, XmlDateTimeSerializationMode.Local))
		//		.AddProvider(new EnvironmentValueProvider()))
  //      {
  //      }

		protected void AssertThrowsSettingsExceptionMessageTest<T>(string format, params object[] args)
		{
			AssertThrowsSettingsExceptionMessageTest(() => SettingsLoader.Create<T>(NotFoundPrefix), format, args);
		}
	}
}
