# UnityUtils 
Code and SO utils for unity. 
## Features 
  - [Extention Methods](#extention-methods)
  - [Attributes](#attributes)
  - [Enum Dictionary](#enum-dictionary)
  - [Singletons](#singletons)
  - [Consensus](#consensus)
  - [Serializers](#serializers)
  - [ScrubGlobalData](#scrubglobaldata)
  - [Runtime Set](#runtime-set)
  - [Service Locator](#service-locator)
  - [Observable](#observable)

## Extention Methods  
<details>
  
  ### Example
```csharp

```
</details>

## Attributes
<details>
  
  ### Example
```csharp

```
</details>

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

## Singletons
<details>
  
  ### Example
```csharp

```
</details>

## Consensus
<details>
  
  ### Example
```csharp

```
</details>

## Serializers
<details>
  
  ### Example
```csharp

```
</details>

## ScrubGlobalData
<details>
  
  ### Example
```csharp

```
</details>

## Runtime Set
<details>
  
  ### Example
```csharp

```
</details>

## Service Locator
<details>
  
  ### Example
```csharp

```
</details>

## Observable
<details>
  
  ### Example
```csharp

```
</details>
