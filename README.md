# com.yanicksenn.utils
URL: https://github.com/yanicksenn/com.yanicksenn.utils.git

![Version](https://img.shields.io/badge/version-2.0.5-blue)
![Unity Version](https://img.shields.io/badge/unity-6000.0+-lightgrey)

**com.yanicksenn.utils** is a Utility for Unity that provides architectural patterns like ScriptableObject-based variables and events, control structures, and quality of life extensions to improve development workflow.

## 📦 Installation

### via Git URL
Open the Unity Package Manager, click the "+" icon, select "Add package from git URL...", and paste:

```
https://github.com/yanicksenn/com.yanicksenn.utils.git#2.0.5
```

## ✨ Features

- **Variables & References:** Define primitive values (Float, Int, Bool, etc.) as ScriptableObjects to share state across scenes, with `Reference<T>` wrappers to transparently switch between constant values and shared variables.
- **Events Architecture:** A ScriptableObject-based event system (`GlobalEvent`, `BoolEvent`, etc.) allowing decoupled communication between prefabs and scenes, including debug logging and disable toggles.
- **Control Structures:** Monadic `Optional<T>`, reactive `Observable<T>`, `Sequencer<T>` for cycling lists, and `Prefab<T>` wrappers to enforce safe instantiation.
- **Runtime Sets:** Manage collections of active GameObjects (like enemies or spawn points) at runtime using ScriptableObjects.
- **Snapshots:** Capture and restore the state of `Rigidbody` and `Collider` components.
- **Vector3 Utilities:** Includes a `Vector3Pid` controller implementation and `Vector3Mask` for axis-specific operations.
- **Class Type Reference:** Serialize `System.Type` in the Inspector with a filtered dropdown using the `[ClassTypeConstraint]` attribute.
- **VContainer Integration:** Utilities for VContainer dependency injection, including `ScriptableObjectInstaller` and `PlainLifetimeScope`.
- **Extensions:** A collection of extensions for `Vector3`, `GameObject`, `Component`, and `Optional` to reduce boilerplate.

## 🚀 Usage

### ⚙️ Variables & References

Use `Variable<T>` assets to store shared data and `Reference<T>` in your components to allow assigning either a static value or a variable asset.

```csharp
using UnityEngine;
using YanickSenn.Utils.Variables;

public class PlayerHealth : MonoBehaviour
{
    // In the Inspector, you can assign a constant float OR a FloatVariable asset
    [SerializeField] private FloatReference maxHealth;
    [SerializeField] private FloatVariable currentHealthVar;

    private void Start()
    {
        // Access the value directly
        float hp = maxHealth.Value; 
        
        // Listen for changes on the variable
        currentHealthVar.OnValueChanged += OnHealthChanged;
        currentHealthVar.Value = hp;
    }

    private void OnHealthChanged(float newValue, float oldValue)
    {
        Debug.Log($"Health changed: {oldValue} -> {newValue}");
    }
}
```

### ⚙️ Global Events

Decouple logic using ScriptableObject events. Supports payloads and sender tracking.

```csharp
using UnityEngine;
using YanickSenn.Utils.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GlobalEvent onGameStart;
    [SerializeField] private BoolEvent onPause;

    private void OnEnable()
    {
        // Subscribe to events
        onGameStart.OnTrigger += HandleGameStart;
        onPause.OnTrigger += HandlePause;
    }

    private void OnDisable()
    {
        onGameStart.OnTrigger -= HandleGameStart;
        onPause.OnTrigger -= HandlePause;
    }

    private void HandleGameStart(GlobalEvent.Metadata metadata)
    {
        Debug.Log($"Game Started at {metadata.Timestamp}. Payload present: {metadata.Payload.IsPresent}");
    }

    private void HandlePause(bool isPaused)
    {
        Time.timeScale = isPaused ? 0 : 1;
    }

    public void TriggerEvents()
    {
        // Invoke events from code
        onGameStart.Invoke(GlobalEvent.Sender.Of(gameObject));
        onPause.Invoke(true);
    }
}
```

### ⚙️ Control Primitives (Optional & Observable)

Use `Optional<T>` to handle nullability safely and `Observable<T>` to react to value changes locally.

```csharp
using UnityEngine;
using YanickSenn.Utils.Control;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] private Observable<int> score = new Observable<int>(0);

    private void Start()
    {
        // React to local value changes
        score.OnValueChanged += (oldVal, newVal) => Debug.Log($"Score: {newVal}");
        score.Value += 10;

        // Optional usage
        Optional<string> playerName = new Optional<string>("Yanick");
        
        playerName
            .Filter(n => n.Length > 0)
            .Map(n => n.ToUpper())
            .DoIfPresent(n => Debug.Log($"Player: {n}"));
            
        string finalName = playerName.OrElse("Unknown");
    }
}
```

### ⚙️ Runtime Sets

Track objects automatically by creating a `RuntimeSet` asset and using the `RuntimeSets` component.

```csharp
using UnityEngine;
using YanickSenn.Utils.RuntimeSets;

public class EnemyTracker : MonoBehaviour
{
    [SerializeField] private RuntimeSet activeEnemies;

    private void Update()
    {
        foreach (GameObject enemy in activeEnemies.Elements())
        {
            Debug.Log($"Tracking: {enemy.name}");
        }
    }
}

// Attach the 'RuntimeSets' component to your Enemy prefab and assign the RuntimeSet asset 
// to automatically register/unregister it on Enable/Disable.
```

### ⚙️ Snapshots

Save and restore component states efficiently.

```csharp
using UnityEngine;
using YanickSenn.Utils.Snapshots;

public class StateManager : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    private RigidbodySnapshot _rbSnapshot;

    public void Save()
    {
        _rbSnapshot = RigidbodySnapshot.From(rb);
    }

    public void Load()
    {
        _rbSnapshot.ApplyTo(rb);
    }
}
```

### ⚙️ Vector3 PID Controller

Smoothly drive a Vector3 value towards a target using a PID controller.

```csharp
using UnityEngine;
using YanickSenn.Utils.Misc;

public class HomingMissile : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Vector3Pid _pid;

    private void Awake()
    {
        // Kp, Ki, Kd, Clamp Strategy
        _pid = new Vector3Pid(5f, 0.1f, 0.5f, new Vector3Pid.Clamped(20f));
    }

    private void FixedUpdate()
    {
        _pid.Calculate(transform.position, target.position, Time.fixedDeltaTime);
        Vector3 force = _pid.Value;
        GetComponent<Rigidbody>().AddForce(force);
    }
}
```

### ⚙️ Class Type Reference

Select class types in the inspector with filtering support.

```csharp
using UnityEngine;
using YanickSenn.Utils.Control;

public class SystemLoader : MonoBehaviour
{
    // Restricts the dropdown to types deriving from 'BaseSystem'
    [SerializeField, ClassTypeConstraint(typeof(MonoBehaviour), allowAbstract: false)]
    private ClassTypeReference systemType;

    private void Start()
    {
        if (systemType.Type != null)
        {
            gameObject.AddComponent(systemType.Type);
        }
    }
}
```

### ⚙️ VContainer Extensions

Easily inject ScriptableObject settings using `ScriptableObjectInstaller` and `PlainLifetimeScope`.

```csharp
using VContainer;
using YanickSenn.Utils.VContainer;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/Game Settings")]
public class GameSettingsInstaller : ScriptableObjectInstaller
{
    public float globalSpeed = 1f;

    public override void Install(IContainerBuilder builder)
    {
        builder.RegisterInstance(globalSpeed);
    }
}

// 1. Create a GameObject with 'PlainLifetimeScope'.
// 2. Create the 'GameSettingsInstaller' asset.
// 3. Add the asset to the 'Scriptable Object Installers' list on the scope.
```

## 🔧 Requirements

* **Unity Version:** 6000.0 or higher
* **Dependencies:**
* `jp.hadashikick.vcontainer` (via `package.json`)