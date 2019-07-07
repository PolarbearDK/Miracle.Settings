# Annotations

Setting loading can be customized by applying annotations to properties.

## Default value

Specify default values using System.ComponentModel.DefaultValueAttribute

```Csharp
[DefaultValue(4200)]
public int Port { get; set; }
[DefaultValue(typeof(System.Data.SqlClient.SqlConnection)]
public Type ConnectionType { get; set; }
```

## Ignore values

Ignore specific values and treat them as "not present" by specifying an **IgnoreValue/IgnoreValues** property (Miracle.Settings.SettingAttribute)

```CSharp
[Setting(IgnoreValue = "N/A")]
[Optional]
public IPAddress IPAddress { get; set; }
```

## Ignore properties

Ignore property by applying an **Ignore** attribute (Miracle.Settings.IgnoreAttribute)

```CSharp
[Ignore]
public string MyIgnoredProperty { get; set; }
```

Note! that properties without settters are ignored by default.

## Optional properties

All nullable value types are by convension optional.

```CSharp
public TimeSpan? TTL { get; set; }
public Nullable<int> MyOptionalInt { get; set; }
```

Reference type properties can be made optional with an **Optional** attribute (Miracle.Settings.OptionalAttribute)

```CSharp
[Optional]
public string MyOptionalValue { get; set; }
```

## Change the name of the property key loaded

Use Miracle.Settings.SettingAttribute to customize deserialization.

```CSharp
[Setting("Bar")]
public string Foo { get; set; }
```

Result: The value of the "Bar" value is loaded into "Foo" property.

## Split string value into array/list

Simple arrays and lists can be loaded from a single string value containing separated values.

```XML
<configuration>
  <appSettings>
    <add key="My:Arr" value="Foo;Bar;Baz" />
  </appSettings>
</configuration>
```

Must specify Separator or Separators.

```CSharp
public class FooSetting
{
  [Setting(Separator=';')]
  public string[] Arr { get; private set; }
}

// Load the 3 values Foo, Bar and Baz into Arr property
var settings = settingsLoader.Create<FooSetting>("My");
```

## Per property type converter

Use **TypeConverter** argument to specify the type converter for a single property.

```Csharp
[Setting(TypeConverter = typeof(MyFooTypeConverter))]
public MyFoo Foo { get; private set; }
```

## Inline classes
Use **Inline** argument to specify that the properties of a nested class should be loaded from the same setting level as the parent class. In other words, the name of the nested property is ignored.

Example:
```CSharp
public class ParentSetting
{
  [Setting(Inline = true)]
  public ChildSetting Child { get; private set; }

  public string Foo { get; private set; }
}

public class ChildSetting
{
  public string Bar { get; private set; }
}

...
var settings = settingsLoader.Create<ParentSetting>("My");
```

In above example "Foo" property is loaded from "My:Foo" and "Bar" property is loaded from "My:Bar".
Without **Inline** the "Bar" parameter would be loaded from "My:Child:Bar"

**Hint!** Use **Inline** to group properties that must be all specified or all left out, by making nested class optional, and leave all properties of nested class required.

## Map interface or abstract class to implementation class

Use **ConcreteType** argument to specify the implementation class when the type of a property does not have a concrete implemetation.

```Csharp
[Setting(ConcreteType = typeof(MyConcreteImplementation))]
public IMyInterface Prop { get; private set; }
```

## Combine values.

The **Reference** and **References** attributes are used to combine several values from the value provider together at load time.

When using the Referece(s) keyword the loader will

- Load value of all **referenced keys** from value provider.
- Load value of the **setting itself** from value provider.
- Combine all loaded values into a array.
- Convert values in array to final type using type converter.
- The type converter matching the property type must be able to handle the number of values in the array.

Consider:

```XML
<configuration>
  <appSettings>
    <add key="Root" value="https://github.com/PolarbearDK" />
    <add key="Partial" value="Miracle.Settings" />
  </appSettings>
</configuration>
```

```Csharp
[Setting("Partial", Reference="Root")]
public Uri CombinedUrl { get; set; }
```

In this example, SettingsLoader will:

- load value of all referenced keys ("Root")
- load value of the setting itself ("Partial").
- The value provider gets the following array:

```Csharp
["https://github.com/PolarbearDK","Miracle.Settings"]
```

- The Url value provider combines the values resulting in a setting containing: "https://github.com/PolarbearDK/Miracle.Settings".

**Note!** that in above example, "Root" and "Partial" refers to values (from "value providers"), and that there are no class properties called "Root" or "Partial".

**Hint!** The FileInfo and DirectoryInfo type converters can combine several elements together.

```Csharp
[Setting(Name = "FileName", References = new[] { "Drive", "Folder" })]
public FileInfo File { get; set; }
```

Result: A FileInfo object is created by combining the loaded value of "Drive", "Folder" and "FileName" (in that order).
