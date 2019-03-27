# Model Validation

The model being loaded is validated and an exception is thrown on any validation error with explicit information about what property caused the error. This include full path to property in case of a nested error.

## Implicit validations

A value MUST be provided for each public property with a setter unless the property is optional.

A property is considered optional if:

- property is annotated with \[Optional\] (Miracle.Settings.OptionalAttribute)
- property type is a nullable value type (System.Nullable<T>)

## Explicit validations

Models can be annotated with validation attributes from System.ComponentModel.DataAnnotations namespace just like any MVC model.

Explicit validation example:

```csharp
public class SettingClass
{
    [StringLength(60, MinimumLength = 3)]
    public string FooString { get; set; }

    [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
    public string BarString { get; set; }

    [Range(1, 10)]
    public int RangedNumeric { get; set; }
}
```
