using System.Xml;

namespace Miracle.Settings.Tests
{
    public abstract class LoadTestBase
    {
        protected readonly ISettingsLoader SettingsLoader;

        protected LoadTestBase()
        {
            SettingsLoader = new SettingsLoader()
                .AddTypeConverter(s => XmlConvert.ToDateTime(s, XmlDateTimeSerializationMode.Local))
                .AddProvider(new EnvironmentProvider());
        }
    }
}