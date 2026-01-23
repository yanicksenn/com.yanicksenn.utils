# com.yanicksenn.utils
URL: https://github.com/yanicksenn/com.yanicksenn.utils.git

![Version](https://img.shields.io/badge/version-[VERSION]-blue)
![Unity Version](https://img.shields.io/badge/unity-[UNITY_VERSION]+-lightgrey)

**com.yanicksenn.utils** is a Utility library for Unity that provides ScriptableObject-based architecture tools, robust control structures, and essential extensions to streamline game development.

## 📦 Installation

### via Git URL
Open the Unity Package Manager, click the "+" icon, select "Add package from git URL...", and paste:

```
https://github.com/yanicksenn/com.yanicksenn.utils.git#[VERSION]
```

## ✨ Features

- **Variables & References:** ScriptableObject-based primitive variables (Float, Int, Bool, etc.) and Reference wrappers that allow toggling between constant values and shared assets in the Inspector.
- **Event System:** A decoupled event architecture using `GlobalEvent` (with payload/sender support) and generic `Event<T>` ScriptableObjects.
- **Control Structures:** Wrappers like `Optional<T>`, `Observable<T>`, `Prefab<T>`, and `Sequencer<T>` to handle data safety, reactivity, and instantiation logic.
- **PID Controller:** A `Vector3Pid` implementation for smooth physical movement and error correction.
- **Snapshots:** Utilities (`ColliderSnapshot`, `RigidbodySnapshot`) to save and restore component states.
- **Runtime Sets:** ScriptableObject-based Sets for managing collections of GameObjects at runtime.
- **VContainer Utilities:** Extensions for the VContainer dependency injection library, allowing ScriptableObject-based installers.
- **Extensions:** Helpful extension methods for `Vector3`, `GameObject`, `Component`, and more.

## 🚀 Usage

### ⚙️ Variables & References

Use `Reference<T>` types in your components to allow designers to switch between hardcoded constants and shared `Variable<T>` ScriptableObjects in the Inspector without changing code.

```csharp
using UnityEngine;
using YanickSenn.Utils.Variables;

public class PlayerHealth : MonoBehaviour
{
    // In the Inspector, this can be set to a Constant number or a FloatVariable asset
    [SerializeField] private FloatReference maxHealth;
    [SerializeField] private BoolVariable isInvincible; // Direct SO reference

    private void Start()
    {
        Debug.Log($"Max Health: {maxHealth.Value}");
        
        // React to variable changes
        isInvincible.OnValueChanged += (newValue, oldValue) => {
            Debug.Log($"Invincibility changed from {oldValue} to {newValue}");
        };
    }
}
```

### ⚙️ Event System

Decouple components using `GlobalEvent` or typed events like `IntEvent`. These exist as assets in the project.

```csharp
using UnityEngine;
using YanickSenn.Utils.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GlobalEvent onGameStart;
    [SerializeField] private IntEvent onScoreChanged;

    public void StartGame()
    {
        // Invoke with optional sender and payload
        onGameStart.Invoke(GlobalEvent.Sender.Of(gameObject));
        
        // Invoke generic event
        onScoreChanged.Invoke(100);
    }
}
```

You can also use the `GlobalEventListener` component to trigger UnityEvents in the Inspector, or subscribe via code:

```csharp
private void OnEnable()
{
    onScoreChanged.OnTrigger += HandleScore;
}

private void HandleScore(int newScore)
{
    Debug.Log("Score: " + newScore);
}
```

### ⚙️ Control Structures

The library provides structures like `Optional<T>` to handle null safety and `Observable<T>` for local reactive properties.

```csharp
using UnityEngine;
using YanickSenn.Utils.Control;

public class DataHandler : MonoBehaviour
{
    // Wrapper for values that might be absent
    private Optional<string> _playerName;
    
    // Wrapper for value change notification
    [SerializeField] private Observable<int> _coins;

    private void Start()
    {
        _playerName = new Optional<string>("Player1");

        // Functional approach to null checking
        _playerName.DoIfPresent(name => Debug.Log($"Hello {name}"));
        
        // Listen to local property changes
        _coins.OnValueChanged += (oldVal, newVal) => {
            Debug.Log($"Coins: {newVal}");
        };
        
        _coins.Value += 50;
    }
}
```

### ⚙️ Prefab Wrapper

`Prefab<T>` ensures that the assigned prefab has the specific component `T` at compile time.

```csharp
using UnityEngine;
using YanickSenn.Utils.Control;
using YanickSenn.Utils.Misc;

public class Spawner : MonoBehaviour
{
    // Inspector validates that the assigned prefab has a Follower component
    [SerializeField] private Prefab<Follower> followerPrefab;

    public void Spawn()
    {
        Follower instance = followerPrefab.Instantiate(transform.position, Quaternion.identity);
    }
}
```

### ⚙️ PID Controller

`Vector3Pid` allows for smooth logic-based movement towards a target using Proportional-Integral-Derivative logic.

```csharp
using UnityEngine;
using YanickSenn.Utils.Misc;

public class PidMover : MonoBehaviour
{
    [SerializeField] private Transform target;
    
    // Configure Kp, Ki, Kd and clamping strategy
    private Vector3Pid _pid = new Vector3Pid(5f, 0.1f, 0.5f, new Vector3Pid.Clamped(10f));

    private void Update()
    {
        if (target == null) return;

        // Calculate force/velocity required
        _pid.Calculate(transform.position, target.position, Time.deltaTime);
        
        // Apply result
        transform.position += _pid.Value * Time.deltaTime;
    }
}
```

### ⚙️ Runtime Sets

Use `RuntimeSet` assets to maintain a list of active objects (like enemies) without singletons.

1. Create a `RuntimeSet` asset via the context menu.
2. Add the `RuntimeSets` component to your prefabs.

```csharp
using UnityEngine;
using YanickSenn.Utils.RuntimeSets;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private RuntimeSet activeEnemies;

    private void Update()
    {
        // Iterate over all objects currently registered in the set
        foreach (var enemyGo in activeEnemies.Elements())
        {
            Debug.Log($"Enemy active: {enemyGo.name}");
        }
    }
}
```

### ⚙️ VContainer Integration

If using VContainer, use `ScriptableObjectInstaller` to define dependencies in assets, and `PlainLifetimeScope` to inject them.

```csharp
using VContainer;
using YanickSenn.Utils.VContainer;
using UnityEngine;

[CreateAssetMenu(menuName = "Installers/My Service Installer")]
public class MyServiceInstaller : ScriptableObjectInstaller
{
    public override void Install(IContainerBuilder builder)
    {
        // Register your services here
        builder.Register<MyService>(Lifetime.Singleton);
    }
}

// In your scene, use PlainLifetimeScope and add the scriptable object installer to the list.
```

### ⚙️ Snapshots

Capture and restore the state of physics components.

```csharp
using UnityEngine;
using YanickSenn.Utils.Snapshots;

public class TimeRewind : MonoBehaviour
{
    private RigidbodySnapshot _rbSnapshot;
    private ColliderSnapshot _colSnapshot;
    private Rigidbody _rb;
    private Collider _col;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
    }

    public void SaveState()
    {
        _rbSnapshot = RigidbodySnapshot.From(_rb);
        _colSnapshot = ColliderSnapshot.From(_col);
    }

    public void RestoreState()
    {
        _rbSnapshot.ApplyTo(_rb);
        _colSnapshot.ApplyTo(_col);
    }
}
```

## 🔧 Requirements

* **Unity Version:** [UNITY_VERSION] or higher
* **Dependencies:**
    * `VContainer` (referenced in `YanickSenn.Utils.VContainer`)
