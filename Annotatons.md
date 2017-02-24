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

## Change the name of the value loaded
Use Miracle.Setings.SettingAttribute to customize deserialization.
```CSharp
[Setting("Bar")]
public string Foo { get; set; }
```
Result: The value of the "Bar" value is loaded into "Foo" property.


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


## Per property type converter
Use a specific type converter for a single property.
```Csharp
[Setting(TypeConverter = typeof(MyFooTypeConverter))]
public MyFoo Foo { get; set; }
```

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

