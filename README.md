# UnityUtils 
Codeing utils for unity. 
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
  - [Visitor](#visitor)

## Extention Methods  
<details>
  <summary>General Extentions</summary>

```csharp
// Some examples of code usage

var componentReference = this.GetOrAdd<TComponent>();

var shuffledList = new List<int> { 1, 2, 3, 4 };
shuffledList.Shuffle();
var newList = shuffledList.Shuffled();

var randomFromList = newList.GetRandom();

var immediateChildren = transform.GetImmediateChildren();
var immediateChildrenTransforms = transform.GetImmediateChildrenTransforms();
```

</details>
<details>
  <summary>Vector Extentions</summary>
  
  ```csharp
// All of these work with Vector3 and Quaternion

var vector = new Vector2(1, 2);
vector.Set(x: 2);
var offsetVector = vector.WithOffset(x: 2);
var newVector = vector.With(y: 5);
```

</details>
<details>
  <summary>Reflection Extentions</summary>

  ```csharp
// Some examples of code usage

var allInts = this.GetFieldsOfType<int>();
var healthInt = this.GetFieldByNameAndType<int>("Health");
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

```csharp
// Create A Manager
// Singleton Inherits From Monobehaviour
// A PersistentSingleton Will Put The Object In DontDestroyOnLoad
// A LazyLoad Will Automatically Create Itself When Requested If No Instance Exists
public class GameManager : Singleton<GameManager> {
    public int score = 0;
}

public class Player : MonoBehaviour {
    private void Start() {
        // Call On The Static Instance Reference From Anywhere
        GameManager.Instance.score += 10;
    }
}
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

```csharp

```
</details>

## Service Locator
<details>

```csharp

```
</details>

## Observable
<details>

```csharp

```

</details>


## Visitor
<details>

```csharp
// Create An Interface For Objects That Are Interactable
public interface IInteractableObject : IVisitable { }
// Create An Interface For Objects That Can Interact With Them
public interface IInteractableObjectVisitor : IVisitor<IInteractableObject> { }

// Interactable Classes Need To Be Partial
// Boilerplate Code Is Made By Source Genereator
public partial class PostBox : MonoBehaviour, IInteractableObject {
    public bool isBroken;
}

public partial class Door : MonoBehaviour, IInteractableObject {
    public bool isOpen;
}

// Interacting Classes Need To Be Partial
// Boilerplate Code Is Made By Source Genereator
public partial class Screwdriver : MonoBehaviour, IInteractableObjectVisitor {
  // Create  Methods (Named "Visit") Where The Parameter Is the Interactable Object You Want To Handle With Custom Code
  public void Visit(PostBox postBox) {
      if (postBox.isBroken) {
          Debug.Log("I can fix this");
          postBox.isBroken = false;
      }
      else {
          Debug.Log("There's no need to fix this");
      }
  }

  // Add More Methods For Adding Specific Logic For Object Interacton For This Object Interactor
  public void Visit(Door door) {
      door.isOpen = !door.isOpen;
  }

  // If No Specific Logic For Interactable Object Exists
  public void DefaultVisit(IInteractableObject visitable) {
      Debug.Log("I don't need to screw this " + visitable);
  }
}

// Example User Of The Interacting Object
public class Player : MonoBehaviour {
    public IInteractableObjectVisitor equipedInteractor;
    public IInteractableObject selectedInteractableObject;

    private void Start() {
        equipedInteractor.Visit(selectedInteractableObject);
    }
}
```

</details>
