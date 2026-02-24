# UnityUtils 
Code and SO utils for unity. 
## Features 
  - [Extention Methods](##)
  - [Attributes](##)
  - [Enum Dictionary](##EnumDictionary)
  - [Singletons](##Singletons)
  - [Consensus](##)
  - [Serializers](##Serializers)
  - [ScrubGlobalData](##)
  - [Runtime Set](##)
  - [Service Locator](##)
  - [Observable](##)

## Enum Dictionary 
<details> 
  A Unity package that provides a strongly-typed dictionary for enums where all the enum corresponding values must be specified on creation.
  
  - The dictionary is **readonly** and cannot be changed added to or removed from after creation.
  
  - A Roslyn analyzer will give a compile-time error if not all enum entries are assigned a value.
    
  - Only index-assignment syntax '[Enum.Member] = value' is supported

  ### Example
```csharp
public enum MyEnum { item1, item2 }

public int item1Value = 5;
public int item2Value = 10;

var myEnumDictionary = new EnumDictionary<MyEnum, int>{
    [MyEnum.item1] = item1Value, 
    [MyEnum.item2] = item2Value
};
```
</details>
