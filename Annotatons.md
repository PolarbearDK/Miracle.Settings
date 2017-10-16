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
Ignore specific values and treat them as "not present" by specifying an __IgnoreValue/IgnoreValues__ property (Miracle.Settings.SettingAttribute)
```CSharp
[Setting(IgnoreValue = "N/A")]
[Optional]
public IPAddress IPAddress { get; set; }
```

## Ignore properties
Ignore property by applying an __Ignore__ attribute (Miracle.Settings.IgnoreAttribute)
```CSharp
[Ignore]
public string MyIgnoredProperty { get; set; }
```
Note! that properties without settters are ignored by default.

## Optional properties
A simple property can be made optional with an __Optional__ attribute (Miracle.Settings.OptionalAttribute)
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
    <add key="My.Arr" value="Foo;Bar;Baz" />
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
Use __TypeConverter__ to specify the type converter for a single property.
```Csharp
[Setting(TypeConverter = typeof(MyFooTypeConverter))]
public MyFoo Foo { get; private set; }
```

## Map interface or abstract class to implementation class
Use __ConcreteType__ to specify the implementation class when the type of a property does not have a concrete implemetation.
```Csharp
[Setting(ConcreteType = typeof(MyConcreteImplementation))]
public IMyInterface Prop { get; private set; }
```

## Combine several values into one. 
Reference other values, and combine them into new values. The referenced value(s) are handed to the type converter first in the value array. The value of the setting value itself is the last element in the array.

__Note!__ Values (from "value providers") are referenced, not properties. 

```Csharp
[Setting(Reference="Bar")]
public Uri Foo { get; set; }
```
Result: the type converter is given an array of values containing the loaded value of "Bar" and "Foo". 
Note! The type converter must be able to handle multiple values.

```Csharp
[Setting(Name = "FileName", References = new[] { "Drive", "Folder" })]
public FileInfo File { get; set; }
```
Result: A FileInfo object is created by combining the loaded value of "Drive", "Folder" and "FileName" (in that order). 
