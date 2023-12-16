# HeroKit

The HeroKit library, providing utility operations, offers a comprehensive set of extension methods for controlling, converting, and manipulating various data types. Designed for projects developed in C#, this extensive library enables performing diverse operations on different data types, ranging from arrays to date-time values, objects, and strings.

## Installation & Usage

The HeroKit library contains extension methods to provide helpful functionalities in C# projects. Below, you can find examples of how to use this library:

1. Add the HeroKit library to your project:
    - Open NuGet Package Manager Console to add the library to your project via NuGet Package Manager.
    - Enter the following command to add the HeroKit library to your project:
      ```
      Install-Package HeroKit
      ```

2. Don't forget to include the relevant namespace statements before using extension methods:
    - For instance, if you're working with arrays, add the namespace of the relevant class at the beginning of your project (`using HeroKit.Arrays;`).

3. Perform operations using extension methods:
    - You can use classes like `ArrayControlExtensions`, `ArrayConversionExtensions`, `ArrayManipulationExtensions` for array-related operations.
    - For date and time formatting, you can utilize methods from the `DateTimeFormattingExtensions` class.
    - For object control and conversions, use `ObjectControlExtensions` and `ObjectConversionExtensions` classes.
    - String operations can be performed using `StringControlExtensions`, `StringConversionExtensions`, and `StringManipulationExtensions` classes.
    - For type control and conversions, you can use the `TypeControlExtensions` class.

## Contents

- [ArrayControlExtensions](#arraycontrolextensions)
- [ArrayConversionExtensions](#arrayconversionextensions)
- [ArrayManipulationExtensions](#arraymanipulationextensions)
- [AssemblyExtensions](#assemblyextensions)
- [BooleanControlExtensions](#booleancontrolextensions)
- [BooleanConversionExtensions](#booleanconversionextensions)
- [DateTimeFormattingExtensions](#datetimeformattingextensions)
- [ObjectControlExtensions](#objectcontrolextensions)
- [ObjectConversionExtensions](#objectconversionextensions)
- [StringControlExtensions](#stringcontrolextensions)
- [StringConversionExtensions](#stringconversionextensions)
- [StringManipulationExtensions](#stringmanipulationextensions)
- [TypeControlExtensions](#typecontrolextensions)
- [TypesExtensions](#typesextensions)

## `ArrayControlExtensions`

This class contains extension methods for performing value checks on array types.

### `IsNullOrEmpty<T>(this T[] array)`

Checks if a given array is null or empty.

- `T`: Type of elements in the array.
- `array`: The array to be checked.

Returns: True if the array is null or empty, otherwise false.

### `IsNotNullOrEmpty<T>(this T[] array)`

Checks if a given array is not null or empty.

- `T`: Type of elements in the array.
- `array`: The array to be checked.

Returns: True if the array is not null or empty, otherwise false.

## Usage

Using the extension methods in this library, you can check whether arrays are empty or filled. Below are usage examples:

```csharp
using HeroKit.Arrays;

class Program
{
    static void Main()
    {
        int[] emptyArray = new int[0];
        int[] filledArray = new int[] { 1, 2, 3 };

        bool isEmpty = emptyArray.IsNullOrEmpty(); // true
        bool isNotEmpty = filledArray.IsNotNullOrEmpty(); // true
    }
}
```

## `ArrayConversionExtensions`

This class contains extension methods for converting data in array types to various formats.

### `ToList<T>(this T[] items, Func<object, T> mapFunction)`

Used to convert a given array into a list. A mapping function is applied to each element.

- `T`: Type of elements in both the array and the list.
- `items`: The array to be converted.
- `mapFunction`: The mapping function to be applied to each element.

Returns: A list composed of the transformed elements.

### `UrlTokenEncode(this byte[] array)`

Encodes a byte array into a URL-friendly string.

- `array`: The byte array to be encoded.

Returns: The URL-friendly encoded string.

## Usage

Using the extension methods in this library, you can convert arrays to different data types and encode byte arrays into URL-friendly format. Below are usage examples:

```csharp
using HeroKit.Arrays;

class Program
{
    static void Main()
    {
        int[] numbers = new int[] { 1, 2, 3, 4, 5 };
        
        List<string> stringNumbers = numbers.ToList(num => num.ToString()); // ["1", "2", "3", "4", "5"]
        
        byte[] byteArray = new byte[] { 65, 66, 67 };
        string encoded = byteArray.UrlTokenEncode(); // "QkM="
    }
}
```
## `ArrayManipulationExtensions`

This class contains extension methods for performing manipulation operations on data in array types.

### `InsertItems<T>(this T[] array, IEnumerable<T> items)`

Inserts items from a given enumerable collection into the array starting from a specified index.

- `T`: Type of elements in the array.
- `array`: The array to which the items will be added.
- `items`: The enumerable collection containing the items to be added.

No return value.

## Usage

Using the extension methods in this library, you can perform item insertion operations on arrays. Below are usage examples:

```csharp
using HeroKit.Arrays;

class Program
{
    static void Main()
    {
        int[] array = new int[5];
        List<int> items = new List<int> { 1, 2, 3, 4, 5 };

        array.InsertItems(items);
        // The "array" content now is: [1, 2, 3, 4, 5]
    }
}
```

## `AssemblyExtensions`

This class contains extension methods for values of the Assembly type.

### `GetAttribute<T>(this Assembly callingAssembly)`

Used to retrieve a specified attribute from the calling Assembly.

- `T`: Type of the attribute to be retrieved.
- `callingAssembly`: The Assembly from which the attribute will be retrieved.

Returns: An element of the specified attribute type, or null if not found.

## Usage

Using the extension methods in this library, you can perform operations on values of the Assembly type. Below are usage examples:

```csharp
using System;
using HeroKit.Assemblies;

class Program
{
    static void Main()
    {
        var assembly = typeof(Program).Assembly;
        
        AssemblyTitleAttribute titleAttribute = assembly.GetAttribute<AssemblyTitleAttribute>();
        if (titleAttribute != null)
        {
            string assemblyTitle = titleAttribute.Title; // The assembly title
        }
    }
}
```

## `BooleanControlExtensions`

This class contains extension methods for values of the Boolean (bool) type.

### `IsTrue(this bool? value)`

This extension method checks whether a nullable boolean value is true. If the value is not null and true, it returns true; otherwise, it returns false.

### `IsFalse(this bool? value)`

This extension method checks whether a nullable boolean value is false. It uses the IsTrue method to determine if the value is not null and true. If the value is not null and false, it returns true; otherwise, it returns false.

### `AllTrue(this IEnumerable<bool> values)`
This extension method checks if all boolean values in a collection are true. It uses the LINQ All method to determine if all values in the collection are true. If they are, it returns true; otherwise, it returns false.

### `AnyTrue(this IEnumerable<bool> values)`
This extension method checks if any boolean value in a collection is true. It uses the LINQ Any method to determine if at least one value in the collection is true. If there is such a value, it returns true; otherwise, it returns false.

### `AllFalse(this IEnumerable<bool> values)`
This extension method checks if all boolean values in a collection are false. It uses the LINQ All method along with the negation (!) operator to determine if all values in the collection are false. If they are, it returns true; otherwise, it returns false.

### `AnyFalse(this IEnumerable<bool> values)`

This extension method checks if any boolean value in a collection is false. It uses the LINQ Any method along with the negation (!) operator to determine if at least one value in the collection is false. If there is such a value, it returns true; otherwise, it returns false.

## Usage

Using the extension methods in this library, you can perform operations on values of the `Boolean` (`bool`) type. Below are usage examples:

```csharp
using HeroKit.Booleans;

class Program
{
    static void Main(string[] args)
    {
        bool? nullableValue = true;
        bool isNullableValueTrue = nullableValue.IsTrue(); // Returns true

        List<bool> boolList = new List<bool> { true, false, true };
        bool allValuesTrue = boolList.AllTrue(); // Returns false
        bool anyValueTrue = boolList.AnyTrue(); // Returns true

        List<bool> anotherBoolList = new List<bool> { false, false, false };
        bool allValuesFalse = anotherBoolList.AllFalse(); // Returns true
        bool anyValueFalse = anotherBoolList.AnyFalse(); // Returns true
    }
}

```

## `BooleanConversionExtensions`

## ToYesNo(this bool value)

This extension method converts a boolean value to a string representation of "Yes" or "No". If the value is true, it returns "Yes"; otherwise, it returns "No".

## ToYesNo(this bool? value)

This extension method converts a nullable boolean value to a string representation of "Yes" or "No". If the value is not null and true, it returns "Yes"; otherwise, it returns "No".

## ToInt(this bool value)

This extension method converts a boolean value to an integer representation. It returns 1 if the value is true, and 0 if the value is false.

## ToString(this bool value, string trueText, string falseText)

This extension method converts a boolean value to given string equivalents. If the value is `true`, it returns `trueText`, otherwise, it returns `falseText`.

## ToString(this bool? value, string trueText, string falseText, string nullText = "-")

This extension method converts a boolean value to given string equivalents. If the value is `null`, it returns `nullText`, if it's `true`, it returns `trueText`, otherwise, it returns `falseText`.

## Usage

Using the extension methods provided by this class, you can easily perform conversions on boolean values. Here are some examples:

```csharp
using HeroKit.BooleanConversions;

class Program
{
    static void Main(string[] args)
    {
        bool value1 = true;
        string value1AsYesNo = value1.ToYesNo(); // Returns "Yes"
        int value1AsInt = value1.ToInt(); // Returns 1
        
        bool? value2 = false;
        string value2AsYesNo = value2.ToYesNo(); // Returns "No"
        
        // Nullable boolean conversion
        bool? value3 = null;
        string value3AsYesNo = value3.ToYesNo(); // Returns "No"

        bool? nullableValue = true;
        string result = nullableValue.ToString("True", "False", "Null"); // "True"

        bool someValue = true;
        string result = someValue.ToString("True", "False"); // "True"
    }
}
```


## `DateTimeFormattingExtensions`

This class contains extension methods for values of the DateTime type, allowing formatting and conversion to various string representations.

### `ToTrString(this DateTime datetime, string format = "dd MMM yyyy, dddd")`

Returns a string representation of the given DateTime formatted according to Turkish culture.

- `datetime`: The DateTime value to be formatted.
- `format`: Formatting pattern. Default value: "dd MMM yyyy, dddd".

Returns: A string formatted according to Turkish culture.

### `ToCultureString(this DateTime datetime, CultureInfo formatProvider, string format = "dd MMM yyyy, dddd")`

Returns a string representation of the given DateTime formatted according to the specified culture and formatting pattern.

- `datetime`: The DateTime value to be formatted.
- `formatProvider`: The culture format provider.
- `format`: Formatting pattern. Default value: "dd MMM yyyy, dddd".

Returns: A formatted string.

### `AsDirectoryName(this DateTime datetime, string format = "yyyy-MM-dd")`

Returns a DateTime formatted as a directory name.

- `datetime`: The DateTime value to be formatted.
- `format`: Formatting pattern. Default value: "yyyy-MM-dd".

Returns: A formatted string.

### `AsFileName(this DateTime datetime, string baseFileName = "")`

Returns a DateTime formatted as a file name.

- `datetime`: The DateTime value to be formatted.
- `baseFileName`: The base file name to be used.

Returns: A formatted file name string.

## Usage

Using the extension methods in this library, you can format DateTime values into different patterns and cultures. Below are usage examples:

```csharp
using System;
using HeroKit.DateTimes;

class Program
{
    static void Main()
    {
        DateTime now = DateTime.Now;
        
        string trFormatted = now.ToTrString(); // "15 Ağu 2023, Pazartesi"
        
        string customFormatted = now.ToCultureString(new CultureInfo("en-US"), "MMMM dd, yyyy"); // "August 15, 2023"
        
        string directoryName = now.AsDirectoryName(); // "2023-08-15"
        
        string fileName = now.AsFileName("example"); // "example-2023-08-15-10-30-45"
    }
}
```

## `ObjectControlExtensions`

This class contains extension methods for performing control operations on values of the Object type.

### `IsNull(this object @object)`

Checks if a given object is null.

- `object`: The object to be checked.

Returns: True if the object is null, otherwise false.

### `IsNotNull(this object @object)`

Checks if a given object is not null.

- `object`: The object to be checked.

Returns: True if the object is not null, otherwise false.

### `IsNumeric(this object value)`

Checks if a given object is numeric.

- `value`: The object to be checked.

Returns: True if the object is numeric, otherwise false.

## Usage

Using the extension methods in this library, you can perform control operations on values of the Object type. Below are usage examples:

```csharp
using HeroKit.Objects;

class Program
{
    static void Main()
    {
        object obj = null;
        bool isNull = obj.IsNull(); // true
        
        object notNullObj = new object();
        bool isNotNull = notNullObj.IsNotNull(); // true
        
        object numericValue = 123;
        bool isNumeric = numericValue.IsNumeric(); // true
        
        object nonNumericValue = "Hello";
        bool isNonNumeric = nonNumericValue.IsNumeric(); // false
    }
}
```

## `ObjectConversionExtensions`

This class contains extension methods for converting values of the Object type to specified types.

### `ConvertTo<T>(this object source, T defaultValue = default)`

Converts a given object to the specified type, using an optional default value if conversion fails.

- `T`: The target type of the conversion.
- `source`: The object to be converted.
- `defaultValue`: The default value to be used if conversion fails. Default value: `default` (default value of the target type).

Returns: The object converted to the specified type or the default value if conversion fails.

## Usage

Using the extension methods in this library, you can convert values of the Object type to specified types. Below are usage examples:

```csharp
using HeroKit.Objects;

class Program
{
    static void Main()
    {
        object intValue = 123;
        int convertedInt = intValue.ConvertTo<int>(); // 123
        
        object stringValue = "456";
        int convertedStringInt = stringValue.ConvertTo<int>(); // 456
        
        object nonConvertibleValue = "Hello";
        int defaultValue = 0;
        int nonConvertibleInt = nonConvertibleValue.ConvertTo(defaultValue); // 0
    }
}
```

## `StringControlExtensions`

This class contains extension methods for performing control and manipulation operations on values of the String type.

### `IsNullOrEmpty(this string value)`

Checks if a given string is empty or null.

- `value`: The string to be checked.

Returns: True if the string is empty or null, otherwise false.

### `IsNotNullOrEmpty(this string value)`

Checks if a given string is not empty and not null.

- `value`: The string to be checked.

Returns: True if the string is not empty and not null, otherwise false.

### `IsNullOrEmpty(this string source, char splitValue)`

Checks if a given string consists solely of a specific split value character.

- `source`: The string to be checked.
- `splitValue`: The split value character.

Returns: True if the string is empty, null, or consists only of the split value character, otherwise false.

### `IsNotNullOrEmpty(this string source, char splitValue)`

Checks if a given string does not consist solely of a specific split value character.

- `source`: The string to be checked.
- `splitValue`: The split value character.

Returns: True if the string is not empty, not null, and does not consist only of the split value character, otherwise false.

## Usage

Using the extension methods in this library, you can control and manipulate values of the String type. Below are usage examples:

```csharp
using HeroKit.Strings;

class Program
{
    static void Main()
    {
        string emptyString = "";
        bool isEmpty = emptyString.IsNullOrEmpty(); // true
        
        string nonEmptyString = "Hello";
        bool isNotEmpty = nonEmptyString.IsNotNullOrEmpty(); // true
        
        string splitString = ",";
        bool isSplitEmpty = splitString.IsNullOrEmpty(','); // true
        
        string nonSplitString = "Hello,World";
        bool isNonSplitEmpty = nonSplitString.IsNotNullOrEmpty(','); // false
    }
}
```

## `StringConversionExtensions`

This class contains extension methods for converting values of the String type.

### `ToDecimal(this string @this, string numberDecimalSeperator = ",", bool autoReplaceDotToSeperator = true)`

Converts a given string to a decimal number.

- `@this`: The string to be converted to a decimal number.
- `numberDecimalSeperator`: The decimal separator character. Default value: `,`.
- `autoReplaceDotToSeperator`: Whether to automatically replace dots with the specified separator.
- Returns: The converted decimal number.

### `ToEnum<T>(this string value, T defaultValue = default)`

Converts a given string to the specified enum type.

- `T`: The enum type.
- `value`: The string to be converted to an enum.
- `defaultValue`: The default value to be used if conversion fails. Default value: `default` (default value of the enum type).

Returns: The converted enum value or the default value if conversion fails.

### `ToByteArray(string text)`

   This method converts a base64 encoded string (variable: `text`) to a byte array. This method returns a byte array.

### `ToNullable<T>(string source)`

   This function converts a string (variable: `source`) to a nullable value of the specified type `T`. It returns a tuple containing the conversion status (boolean) and the nullable result value (of type T).

### `ToBoolean(string value, string trueCondition, bool defaultvalue)`

   This method attempts to convert the `value` (a string representation of a logical value) to its `System.Boolean` equivalent. It takes a string `trueCondition` and a boolean `defaultvalue`. It returns a boolean value.

### `ToShort(string value)`

   This method converts the `value` (a string representation of a number) to its equivalent short integer. It returns a short integer.

### `ToInt(string value)`

   This method converts a string (variable: `value`) to an integer. This method returns an integer.

### `ToLong(string value)`

   Converts the `value` (a string representation of a number) to its equivalent `System.Int64` value. It returns a long integer.

### `ToDateTime(string value, CultureInfo cultureInfo, DateTime defaultValue)`

   This method converts a string representation of a date and time (`value`) to its `System.DateTime` equivalent by using the optional `cultureInfo` and `defaultValue`. It returns a DateTime object.

## Usage

Using the extension methods in this library, you can convert and manipulate values of the String type. Below are usage examples:

```csharp
using HeroKit.Strings;

class Program
{
    static void Main()
    {
        string decimalString = "123,45";
        decimal convertedDecimal = decimalString.ToDecimal(); // 123.45
        
        string enumString = "2";
        MyEnum enumValue = enumString.ToEnum(MyEnum.Default); // MyEnum.Value2
    }
}
```

## `StringManipulationExtensions`

This class contains extension methods for manipulating and formatting values of the String type.

### `ToSlug(this string value)`

Converts a given string to a URL-friendly "slug" format.

- `value`: The string to be converted.

Returns: The URL-friendly "slug" string.

### `RemoveAccent(this string value)`

Removes accents from a given string.

- `value`: The string from which accents will be removed.

Returns: The string with removed accents.

### `ClearTurkishChars(this string value)`

Replaces Turkish characters in a given string with their non-accented counterparts.

- `value`: The string in which Turkish characters will be replaced.

Returns: The string with replaced Turkish characters.

### `Format(this string value, params object[] args)`:

This static method extends the string class by replacing the format items in a specified string with the string representation of specified objects.

Returns: Return Value: A copy of value in which the format items have been replaced by the string representations of the corresponding objects in args.

### `ToDashCase(this string input)`:

This static method extends the string class by converting the input string to dash case.

Returns: The string converted to dash case.


## Usage

Using the extension methods in this library, you can manipulate, format, and transform values of the String type. Below are usage examples:

```csharp
using HeroKit.Strings;

class Program
{
    static void Main()
    {
        string originalText = "Merhaba Dünya!";
        string slug = originalText.ToSlug(); // merhaba-dunya
    }
}
```
## `TypeControlExtensions`

This class contains extension methods for performing various control operations on type values.

### `IsNullable(this Type type)`

Checks if a given type is a Nullable type (e.g., int?).

- `type`: The type to be checked.

Returns: True if the type is a Nullable type, otherwise false.

### `IsGenericAssignableFrom(this Type toType, Type fromType, out Type[] genericArguments)`

Checks if a given generic type can be assigned from another type and provides generic arguments if necessary.

- `toType`: The target generic type.
- `fromType`: The source type.
- `genericArguments`: The generic arguments if assignment is possible, otherwise null.
- Returns: True if the assignment is possible, otherwise false.

### `GetUnderlyingType(this Type type)`

Gets the underlying non-nullable type of a given type if it's a nullable type.

- `type`: The type to examine.

Returns: The underlying non-nullable type if the type is nullable, otherwise the same type.

### `IsDefaultValue<T>(this T value)`

Checks if a given value is the default value of its type.

- `value`: The value to check for default value.

Returns: True if the value is the default value of its type, otherwise false.

## Usage

Using the extension methods in this library, you can perform various control operations on type values. Below are usage examples:

```csharp
using HeroKit.Types;

class Program
{
    static void Main()
    {
        Type nullableIntType = typeof(int?);
        bool isNullable = nullableIntType.IsNullable(); // true
        
        Type genericListType = typeof(List<>);
        Type genericArgument;
        bool isGenericAssignable = genericListType.IsGenericAssignableFrom(typeof(List<string>), out genericArgument); // true, genericArgument: string
        
        int value = 0;
        bool isDefault = value.IsDefaultValue(); // true
    }
}
```

## `TypesExtensions`

This class contains extension methods for performing various operations on type values.

### `GetDefault(this Type type)`

Gets the default value of a given type.

- `type`: The type for which to get the default value.
- Returns: The default value of the type or null if not applicable.

### `GetDefault<T>()`

Gets the default value of a given type.

- Returns: The default value.

### `GetRealType(this Type type)`

Gets the concrete implementation type of a given interface or abstract type.

- `type`: The interface or abstract type for which to get the concrete implementation type.
- Returns: The concrete implementation type or null if not found.

### `GetExtensionMethods(this Type type, Assembly extensionsAssembly, BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)`

Gets the extension methods of a given type from the specified assembly.

- `type`: The type for which to get extension methods.
- `extensionsAssembly`: The assembly where the extension methods are defined.
- `bindingFlags`: The binding flags used to determine method visibility.
- Returns: A collection of extension methods for the specified type.

### `GetExtensionMethod(this Type type, Assembly extensionsAssembly, string name)`

Gets the specified named extension method of a given type from the specified assembly.

- `type`: The type for which to get the extension method.
- `extensionsAssembly`: The assembly where the extension method is defined.
- `name`: The name of the extension method.
- Returns: MethodInfo for the specified named extension method or null if not found.

### `GetExtensionMethod(this Type type, IEnumerable<Assembly> assemblies, string name)`

Gets the specified named extension method of a given type from the specified assembly collection.

- `type`: The type for which to get the extension method.
- `assemblies`: The collection of assemblies where the extension method is defined.
- `name`: The name of the extension method.
- Returns: MethodInfo for the specified named extension method or null if not found.

### `GetExtensionMethod(this Type type, Assembly extensionsAssembly, string name, Type[] types)`

Gets the specified named extension method with specified parameter types of a given type from the specified assembly.

- `type`: The type for which to get the extension method.
- `extensionsAssembly`: The assembly where the extension method is defined.
- `name`: The name of the extension method.
- `types`: An array of parameter types for the extension method.
- Returns: MethodInfo for the specified named extension method with specified parameter types or null if not found.

## Usage

By using the extension methods in this library, you can perform various operations on type values. Below are examples of how to use them:

```csharp
using HeroKit.Types;

class Program
{
    static void Main()
    {
        Type intType = typeof(int);
        object defaultInt = intType.GetDefault(); // 0

        int defaultValue = TypesExtensions.GetDefault<int>(); // 0

        Type interfaceType = typeof(IDisposable);
        Type implementationType = interfaceType.GetRealType(); // System.IO.StreamReader

        Assembly extensionsAssembly = Assembly.GetExecutingAssembly(); // Using the default assembly
        MethodInfo extensionMethod = typeof(int).GetExtensionMethod(extensionsAssembly, "IsDefaultValue"); // MethodInfo

    }
}
```