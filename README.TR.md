# HeroKit

Yardımcı işlemler sağlayan HeroKit kütüphanesi, farklı veri türlerini kontrol etmek, dönüştürmek ve manipüle etmek için kullanılan geniş bir dizi uzantı metodu sunmaktadır. Bu kapsamlı kütüphane, C# dilinde geliştirilen projelerde, dizilerden tarih-saat değerlerine, nesnelerden metinlere kadar birçok farklı veri tipi üzerinde çeşitli işlemler yapmak için tasarlanmıştır.

## Kurulum & Kullanım

Aşağıda, bu kütüphanenin nasıl kullanılacağına dair örnekler bulabilirsiniz:

1. Projeye HeroKit kütüphanesini ekleyin:
    - Projeye NuGet Paket Yöneticisi üzerinden eklemek için NuGet Package Manager Console'ı açın.
    - Aşağıdaki komutu girerek HeroKit kütüphanesini projenize ekleyin:
      ```
      Install-Package HeroKit
      ```

2. Projede ilgili namespace ifadelerini eklemeyi unutmayın:
    - Örneğin, dizilerle ilgili işlemler yapacaksanız, ilgili sınıfın namespace ifadesini projenizin başına ekleyin (`using HeroKit.Arrays;`).

3. Uzantı metotlarını kullanarak işlemleri gerçekleştirin:
    - Dizilerle ilgili işlemler için `ArrayControlExtensions`, `ArrayConversionExtensions`, `ArrayManipulationExtensions` gibi sınıflar ve bu sınıflardaki metotlar kullanılabilir.
    - Tarih ve saat biçimlemesi için `DateTimeFormattingExtensions` sınıfındaki metotları kullanabilirsiniz.
    - Nesne kontrolü ve dönüşümleri için `ObjectControlExtensions` ve `ObjectConversionExtensions` sınıflarını kullanabilirsiniz.
    - String işlemleri için `StringControlExtensions`, `StringConversionExtensions`, ve `StringManipulationExtensions` sınıfları kullanılabilir.
    - Tür kontrolü ve dönüşümleri için `TypeControlExtensions` sınıfını kullanabilirsiniz.


## İçerik

- [ArrayControlExtensions](#arraycontrolextensions)
- [ArrayConversionExtensions](#arrayconversionextensions)
- [ArrayManipulationExtensions](#arraymanipulationextensions)
- [AssemblyExtensions](#assemblyextensions)
- [DateTimeFormattingExtensions](#datetimeformattingextensions)
- [ObjectControlExtensions](#objectcontrolextensions)
- [ObjectConversionExtensions](#objectconversionextensions)
- [StringControlExtensions](#stringcontrolextensions)
- [StringConversionExtensions](#stringconversionextensions)
- [StringManipulationExtensions](#stringmanipulationextensions)
- [TypeControlExtensions](#typecontrolextensions)
- [TypesExtensions](#typesextensions)


## `ArrayControlExtensions`

Bu sınıf, dizi (array) türü üzerinde değer kontrolü yapmak için uzantı metotları içerir.

### `IsNullOrEmpty<T>(this T[] array)`

Verilen bir dizinin (array) null veya boş olup olmadığını kontrol eder.

- `T`: Dizideki öğelerin türü.
- `array`: Kontrol edilecek dizi.

Dönen değer: Dizi null veya boş ise true, aksi halde false.

### `IsNotNullOrEmpty<T>(this T[] array)`

Verilen bir dizinin (array) null veya boş olmadığını kontrol eder.

- `T`: Dizideki öğelerin türü.
- `array`: Kontrol edilecek dizi.

Dönen değer: Dizi null veya boş değilse true, aksi halde false.

## Kullanım

Bu kütüphanedeki uzantı metotlarını kullanarak, dizilerin boş veya dolu olup olmadığını kontrol edebilirsiniz. Aşağıda kullanım örnekleri bulunmaktadır:

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

Bu sınıf, dizi (array) türündeki verileri çeşitli formatlara dönüştürmek için uzantı metotları içerir.

### `ToList<T>(this T[] items, Func<object, T> mapFunction)`

Verilen bir diziyi (array) bir liste olarak dönüştürmek için kullanılır. Her bir öğe üzerine bir eşleme fonksiyonu uygulanır.

- `T`: Dizideki öğelerin ve listenin türü.
- `items`: Dönüştürülecek dizi.
- `mapFunction`: Her bir öğe üzerine uygulanacak eşleme fonksiyonu.

Dönen değer: Dönüştürülmüş öğelerden oluşan liste.

### `UrlTokenEncode(this byte[] array)`

Bir byte dizisini URL dostu bir stringe kodlar.

- `array`: Kodlanacak byte dizisi.

Dönen değer: URL dostu kodlanmış string.

## Kullanım

Bu kütüphanedeki uzantı metotlarını kullanarak, dizileri farklı veri tiplerine dönüştürebilir ve byte dizilerini URL dostu şekilde kodlayabilirsiniz. Aşağıda kullanım örnekleri bulunmaktadır:

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

Bu sınıf, dizi (array) türündeki verilere manipülasyon işlemleri yapmak için uzantı metotları içerir.

### `InsertItems<T>(this T[] array, IEnumerable<T> items)`

Bir numaralı indisten itibaren verilen bir enumerable koleksiyonu, diziye (array) öğe ekler.

- `T`: Dizideki öğelerin türü.
- `array`: Öğelerin ekleneceği dizi.
- `items`: Eklenen öğelerin bulunduğu enumerable koleksiyonu.

Dönüş değeri yok.

## Kullanım

Bu kütüphanedeki uzantı metotlarını kullanarak, dizilerde öğe ekleme işlemleri yapabilirsiniz. Aşağıda kullanım örnekleri bulunmaktadır:

```csharp
using HeroKit.Arrays;

class Program
{
    static void Main()
    {
        int[] array = new int[5];
        List<int> items = new List<int> { 1, 2, 3, 4, 5 };

        array.InsertItems(items);
        // Şimdi "array" içeriği: [1, 2, 3, 4, 5]
    }
}
```

## `AssemblyExtensions`

Bu sınıf, Assembly (derleme) türündeki değerlere yönelik uzantı metotları içerir.

### `GetAttribute<T>(this Assembly callingAssembly)`

Çağrılan Assembly'den (derleme) belirtilen niteliği (attribute) almak için kullanılır.

- `T`: Alınacak nitelik (attribute) türü.
- `callingAssembly`: Niteliğin alınacağı Assembly.

Dönen değer: Belirtilen nitelik (attribute) tipindeki öğe, bulunamazsa null.

## Kullanım

Bu kütüphanedeki uzantı metotlarını kullanarak, Assembly türündeki değerlere yönelik işlemler yapabilirsiniz. Aşağıda kullanım örnekleri bulunmaktadır:

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
            string assemblyTitle = titleAttribute.Title; // Assembly başlığı
        }
    }
}
```

## `DateTimeFormattingExtensions`

Bu sınıf, DateTime (tarih ve saat) türündeki değerlere yönelik uzantı metotları içerir.

### `ToTrString(this DateTime datetime, string format = "dd MMM yyyy, dddd")`

Verilen bir DateTime'i Türk kültürüne göre biçimlendirilmiş bir string olarak döndürür.

- `datetime`: Biçimlendirilecek DateTime değeri.
- `format`: Biçimlendirme şablonu. Varsayılan değer: "dd MMM yyyy, dddd".

Dönen değer: Türk kültürüne göre biçimlendirilmiş string.

### `ToCultureString(this DateTime datetime, CultureInfo formatProvider, string format = "dd MMM yyyy, dddd")`

Verilen bir DateTime'i belirtilen kültüre ve biçimlendirme şablonuna göre döndürür.

- `datetime`: Biçimlendirilecek DateTime değeri.
- `formatProvider`: Kültürü belirten format sağlayıcı.
- `format`: Biçimlendirme şablonu. Varsayılan değer: "dd MMM yyyy, dddd".

Dönen değer: Biçimlendirilmiş string.

### `AsDirectoryName(this DateTime datetime, string format = "yyyy-MM-dd")`

Verilen bir DateTime'i dizin adı için uygun bir biçimde döndürür.

- `datetime`: Biçimlendirilecek DateTime değeri.
- `format`: Biçimlendirme şablonu. Varsayılan değer: "yyyy-MM-dd".

Dönen değer: Biçimlendirilmiş string.

### `AsFileName(this DateTime datetime, string baseFileName = "")`

Verilen bir DateTime'i dosya adı için uygun bir biçimde döndürür.

- `datetime`: Biçimlendirilecek DateTime değeri.
- `baseFileName`: Kullanılacak temel dosya adı.

Dönen değer: Biçimlendirilmiş dosya adı string'i.

## Kullanım

Bu kütüphanedeki uzantı metotlarını kullanarak, DateTime türündeki değerleri farklı formatlara ve kültürlere göre biçimlendirebilirsiniz. Aşağıda kullanım örnekleri bulunmaktadır:

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

Bu sınıf, Object (nesne) türündeki değerlere yönelik kontrol işlemleri yapmak için uzantı metotları içerir.

### `IsNull(this object @object)`

Verilen bir nesnenin (object) null olup olmadığını kontrol eder.

- `object`: Kontrol edilecek nesne.

Dönen değer: Nesne null ise true, aksi halde false.

### `IsNotNull(this object @object)`

Verilen bir nesnenin (object) null olmadığını kontrol eder.

- `object`: Kontrol edilecek nesne.

Dönen değer: Nesne null değilse true, aksi halde false.

### `IsNumeric(this object value)`

Verilen bir nesnenin (object) sayısal (numeric) olup olmadığını kontrol eder.

- `value`: Kontrol edilecek nesne.

Dönen değer: Nesne sayısal ise true, aksi halde false.

## Kullanım

Bu kütüphanedeki uzantı metotlarını kullanarak, Object türündeki değerleri kontrol edebilirsiniz. Aşağıda kullanım örnekleri bulunmaktadır:

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

Bu sınıf, Object (nesne) türündeki değerleri belirtilen türe çevirmek için uzantı metotları içerir.

### `ConvertTo<T>(this object source, T defaultValue = default)`

Verilen bir nesneyi (object) belirtilen türe çevirir, isteğe bağlı olarak varsayılan değeri kullanır.

- `T`: Dönüşümün hedef türü.
- `source`: Çevrilecek nesne.
- `defaultValue`: Dönüşüm başarısız olursa kullanılacak varsayılan değer. Varsayılan değer: `default` (varsayılan tür değeri).

Dönen değer: Nesnenin belirtilen türe çevrilmiş hali veya dönüşüm başarısız olursa varsayılan değer.

## Kullanım

Bu kütüphanedeki uzantı metotlarını kullanarak, Object türündeki değerleri belirtilen türlere çevirebilirsiniz. Aşağıda kullanım örnekleri bulunmaktadır:

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

Bu sınıf, string (dize) türündeki değerlere yönelik kontrol ve manipülasyon işlemleri yapmak için uzantı metotları içerir.

### `IsNullOrEmpty(this string value)`

Verilen bir dizinin (string) boş veya null olup olmadığını kontrol eder.

- `value`: Kontrol edilecek dize.

Dönen değer: Dize boş veya null ise true, aksi halde false.

### `IsNotNullOrEmpty(this string value)`

Verilen bir dizinin (string) boş veya null olmadığını kontrol eder.

- `value`: Kontrol edilecek dize.

Dönen değer: Dize boş veya null değilse true, aksi halde false.

### `IsNullOrEmpty(this string source, char splitValue)`

Verilen bir dizinin (string) belirli bir ayırma değerinden oluşup oluşmadığını kontrol eder.

- `source`: Kontrol edilecek dize.
- `splitValue`: Ayırma değeri karakteri.

Dönen değer: Dize boş, null veya sadece ayırma değerinden oluşuyorsa true, aksi halde false.

### `IsNotNullOrEmpty(this string source, char splitValue)`

Verilen bir dizinin (string) belirli bir ayırma değerinden oluşmadığını kontrol eder.

- `source`: Kontrol edilecek dize.
- `splitValue`: Ayırma değeri karakteri.

Dönen değer: Dize boş, null veya sadece ayırma değerinden oluşmuyorsa true, aksi halde false.

## Kullanım

Bu kütüphanedeki uzantı metotlarını kullanarak, string türündeki değerleri kontrol edebilir ve manipüle edebilirsiniz. Aşağıda kullanım örnekleri bulunmaktadır:

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

Bu sınıf, string (dize) türündeki değerleri dönüştürmek için uzantı metotları içerir.

### `ToDecimal(this string @this, string numberDecimalSeperator = ",", bool autoReplaceDotToSeperator = true)`

Verilen bir dizeyi (string) ondalık sayıya dönüştürür.

- `@this`: Ondalık sayıya dönüştürülecek dize.
- `numberDecimalSeperator`: Ondalık ayırıcı karakteri. Varsayılan değer: `,`.
- `autoReplaceDotToSeperator`: Otomatik olarak noktayı belirtilen ayıracıyla değiştirip değiştirmeyeceği.
- Dönen değer: Dönüştürülen ondalık sayı.

### `ToEnum<T>(this string value, T defaultValue = default)`

Verilen bir diziyi (string) belirtilen enum türüne dönüştürür.

- `T`: Enum türü.
- `value`: Enuma dönüştürülecek dize.
- `defaultValue`: Dönüşüm başarısız olursa kullanılacak varsayılan değer. Varsayılan değer: `default` (varsayılan tür değeri).

Dönen değer: Dönüştürülen enum değeri veya dönüşüm başarısız olursa varsayılan değer.

## Kullanım

Bu kütüphanedeki uzantı metotlarını kullanarak, string türündeki değerleri dönüştürebilir ve manipüle edebilirsiniz. Aşağıda kullanım örnekleri bulunmaktadır:

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

Bu sınıf, string (dize) türündeki değerleri manipüle etmek ve biçimlendirmek için uzantı metotları içerir.

### `ToSlug(this string value)`

Verilen bir diziyi (string) URL dostu bir "slug" haline dönüştürür.

- `value`: Dönüştürülecek dize.

Dönen değer: URL dostu "slug" dizesi.

### `RemoveAccent(this string value)`

Verilen bir diziden (string) aksanları (accent) kaldırır.

- `value`: Aksanları kaldırılacak dize.

Dönen değer: Aksanları kaldırılmış dize.

### `ClearTurkishChars(this string value)`

Verilen bir dizide (string) Türkçe karakterleri aksanlı (accented) halleriyle değiştirir.

- `value`: Türkçe karakterlerin değiştirileceği dize.

Dönen değer: Türkçe karakterleri değiştirilmiş dize.

## Kullanım

Bu kütüphanedeki uzantı metotlarını kullanarak, string türündeki değerleri manipüle edebilir, biçimlendirebilir ve dönüştürebilirsiniz. Aşağıda kullanım örnekleri bulunmaktadır:

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

Bu sınıf, tür (type) değerleri üzerinde çeşitli kontrol işlemleri yapmak için uzantı metotları içerir.

### `IsNullable(this Type type)`

Verilen bir türün (type) Nullable tür (örneğin, int?) olup olmadığını kontrol eder.

- `type`: Kontrol edilecek tür.

Dönen değer: Tür Nullable tür ise true, aksi halde false.

### `IsGenericAssignableFrom(this Type toType, Type fromType, out Type[] genericArguments)`

Verilen bir generic türün (type) başka bir türden atanabilir olup olmadığını ve gerekirse generic argümanları sağlar.

- `toType`: Hedef generic tür.
- `fromType`: Kaynak tür.
- `genericArguments`: Atama mümkün ise generic argümanlar, aksi halde null.
- Dönen değer: Atama mümkün ise true, aksi halde false.

### `GetUnderlyingType(this Type type)`

Verilen bir türün (type) nullable tür ise altında yatan non-nullable türünü alır.

- `type`: İncelemek için tür.

Dönen değer: Eğer tür nullable tür ise altında yatan non-nullable türü, değilse aynı tür.

### `IsDefaultValue<T>(this T value)`

Verilen bir değerin, türünün varsayılan değeri olup olmadığını kontrol eder.

- `value`: Varsayılan değeri kontrol edilecek değer.

Dönen değer: Değer türünün varsayılan değeri ise true, aksi halde false.

## Kullanım

Bu kütüphanedeki uzantı metotlarını kullanarak, tür (type) değerleri üzerinde çeşitli kontrol işlemleri yapabilirsiniz. Aşağıda kullanım örnekleri bulunmaktadır:

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

Bu sınıf, tür (type) değerleri üzerinde çeşitli işlemler yapmak için uzantı metotları içerir.

### `GetDefault(this Type type)`

Verilen bir türün (type) varsayılan değerini alır.

- `type`: Varsayılan değeri alınacak tür.
- Dönen değer: Varsayılan değeri veya uygulanamıyorsa null.

### `GetDefault<T>()`

Verilen bir türün (type) varsayılan değerini alır.

- Dönen değer: Varsayılan değeri.

### `GetRealType(this Type type)`

Verilen bir arayüzün veya soyut türün somut uygulama türünü alır.

- `type`: İşlem yapılacak arayüz veya soyut tür.
- Dönen değer: Somut uygulama türü veya bulunamazsa null.

### `GetExtensionMethods(this Type type, Assembly extensionsAssembly, BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)`

Verilen bir türün (type) uzantı metotlarını belirtilen derlemeden alır.

- `type`: Uzantı metotları alınacak tür.
- `extensionsAssembly`: Uzantı metotlarının bulunduğu derleme.
- `bindingFlags`: Metot görünürlüğünü kontrol etmek için kullanılacak BindingFlag'ler.
- Dönen değer: Belirtilen tür için uzantı metotlarının koleksiyonu.

### `GetExtensionMethod(this Type type, Assembly extensionsAssembly, string name)`

Verilen bir türün (type) belirtilen adlı uzantı metotunu belirtilen derlemeden alır.

- `type`: Uzantı metodu alınacak tür.
- `extensionsAssembly`: Uzantı metodu bulunan derleme.
- `name`: Uzantı metodu adı.
- Dönen değer: Belirtilen adlı uzantı metodu için MethodInfo veya bulunamazsa null.

### `GetExtensionMethod(this Type type, IEnumerable<Assembly> assemblies, string name)`

Verilen bir türün (type) belirtilen adlı uzantı metotunu belirtilen derleme koleksiyonundan alır.

- `type`: Uzantı metodu alınacak tür.
- `assemblies`: Uzantı metodu bulunan derleme koleksiyonu.
- `name`: Uzantı metodu adı.
- Dönen değer: Belirtilen adlı uzantı metodu için MethodInfo veya bulunamazsa null.

### `GetExtensionMethod(this Type type, Assembly extensionsAssembly, string name, Type[] types)`

Verilen bir türün (type) belirtilen ad ve parametre türlerine sahip uzantı metotunu belirtilen derlemeden alır.

- `type`: Uzantı metodu alınacak tür.
- `extensionsAssembly`: Uzantı metodu bulunan derleme.
- `name`: Uzantı metodu adı.
- `types`: Uzantı metodu parametre türleri dizisi.
- Dönen değer: Belirtilen ad ve parametre türlerine sahip uzantı metodu için MethodInfo veya bulunamazsa null.

## Kullanım

Bu kütüphanedeki uzantı metotlarını kullanarak, tür (type) değerleri üzerinde çeşitli işlemler yapabilirsiniz. Aşağıda kullanım örnekleri bulunmaktadır:

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

        Assembly extensionsAssembly = Assembly.GetExecutingAssembly(); // Varsayılan derlemeyi kullanarak
        MethodInfo extensionMethod = typeof(int).GetExtensionMethod(extensionsAssembly, "IsDefaultValue"); // MethodInfo

    }
}
```



