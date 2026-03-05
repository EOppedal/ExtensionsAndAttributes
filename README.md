# UnityUtils 
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
// Only Some Examples

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
// All Of These Work With Vector3 And Quaternion

var vector = new Vector2(1, 2);
vector.Set(x: 2);
var offsetVector = vector.WithOffset(x: 2);
var newVector = vector.With(y: 5);
```

</details>
<details>
  <summary>Reflection Extentions</summary>

  ```csharp
// Only Some Examples

var allInts = this.GetFieldsOfType<int>();
var healthInt = this.GetFieldByNameAndType<int>("Health");
```
</details>

## Attributes
<details>
  
  ### Example

```csharp
// Adds The Asset Preview Of A Reference To The Inspector
[AssetPreviewIcon] public Sprite sprite;

// Gets Or Adds A Component On The Object On Validation And Saves It In The Serialization Of Tho Object
// NB! Must Be Serialized, But Can Be Hidden In The Inspector
[GetOrAddComponent, HideInInspector] public Script1 script1;
[GetOrAddComponent, SerializeField, HideInInspector] private Script2 script2;

// Must Be Not Null Or Default Value
[Required] public GameObject prefab;

[CreateAssetMenu(fileName = nameof(Test1), menuName = "Scriptable Objects/Create " + nameof(Test1))]
public class Test1 : ScriptableObject {
    // Resets Changes Done To Field Of Scriptable Object In Play Mode
    [ResetFieldOnExitPlayMode] public float health;
}

// Resets Changes Done To Entire Scriptable Object In Play Mode
[CreateAssetMenu(fileName = nameof(Test2), menuName = "Scriptable Objects/Create " + nameof(Test2))]
[ResetFieldsOnExitPlayMode]
public class Test2 : ScriptableObject {
    public float health;
}
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

```csharp
// JSON And XML Serializers Available

var serializer = new JSONSerializer();
var serialized = serializer.Serialize(this); // String To Be Saved To A File
var deSerialized = serializer.Deserialize<TestMonobehaviour>(serialized);
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
// Generic Observable With OnValueChanged Event
var score =  new Observable<int>(2);
score.OnValueChanged += x => Debug.Log(x);

// Float Observable Clamped Between Values With Extra Events And Operators 
var health = new ClampedFloat(0, 5, 5);
health.OnDecreaseValue += f => Debug.Log(f + " damage taken");
health.OnIncreaseValue += f => Debug.Log(f + " health points healed");
health -= 1;
health += 1;
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
