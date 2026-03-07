# [com.yanicksenn.utils]
URL: https://github.com/yanicksenn/com.yanicksenn.utils.git

![Version](https://img.shields.io/badge/version-2.0.5-blue)
![Unity Version](https://img.shields.io/badge/unity-6000.0+-lightgrey)

**[com.yanicksenn.utils]** is a Utility Library for Unity that provides a collection of quality-of-life tools, ScriptableObject architectures, and extension methods to streamline development.

## 📦 Installation

### via Git URL
Open the Unity Package Manager, click the "+" icon, select "Add package from git URL...", and paste:

```
https://github.com/yanicksenn/com.yanicksenn.utils.git#2.0.5
```

## ✨ Features

- **Variables & References:** Share primitive data (Float, Int, Bool, etc.) across scenes using `ScriptableObject` Variables. `Reference<T>` fields let the Inspector switch between constants and Variables.
- **Global Event System:** Decoupled event architecture using `GlobalEvent` assets and `GlobalEventListener` components, with optional payloads.
- **VContainer Utilities:** Attribute-based dependency injection registration using `ProviderLifetimeScope` and `[Provides]`.
- **Control Flow & Patterns:**
  - `Optional<T>`: Functional wrapper for nullable values.
  - `Observable<T>`: Wrapper for tracking and reacting to value changes.
  - `Cooldown`: Simple time-based cooldown tracker.
  - `StateMachine<T>`: Minimalistic generic state machine.
  - `Sequencer<T>`: Manager for stepping through sequential lists.
  - `Prefab<T>`: Typed prefab references in the Inspector.
- **Misc Utilities:**
  - `Vector3Pid`: A PID controller for physics-based smoothing.
  - `Scheduler`: MonoBehaviour-based delay and periodic task scheduler.
  - `Follower` & `Anchor`: Components for selective object following with offset and masking.
  - `FixedSizeQueue<T>`: Queue with fixed capacity and automatic eviction.
  - `Vector3Mask`: Inspector-friendly struct to mask specific axes.
- **Runtime Sets:** Manage collections of active GameObjects globally via ScriptableObjects.
- **Feature Flags:** Toggle features dynamically using `FeatureFlag` ScriptableObjects.
- **Physics Snapshots:** Capture and restore the state of `Collider` and `Rigidbody` (`ColliderSnapshot`, `RigidbodySnapshot`).
- **Class Type Serialization:** Select `System.Type` references in the Inspector using `ClassTypeReference` with optional base-class filtering.

## 🚀 Usage

### ⚙️ Variables & References (`YanickSenn.Utils.Variables`)

Use `Reference<T>` to allow flexibility in the Inspector. You can assign a constant value or link a `Variable<T>` asset (e.g., `FloatVariable`, `IntVariable`).

The following types are available:

| Type | Variable Asset | Reference Field |
| :--- | :--- | :--- |
| **AnimationCurve** | `AnimationCurveVariable` | `AnimationCurveReference` |
| **Bool** | `BoolVariable` | `BoolReference` |
| **Float** | `FloatVariable` | `FloatReference` |
| **GameObject** | `GameObjectVariable` | `GameObjectReference` |
| **Int** | `IntVariable` | `IntReference` |
| **LayerMask** | `LayerMaskVariable` | `LayerMaskReference` |
| **Material** | `MaterialVariable` | `MaterialReference` |
| **Schedule** | `ScheduleVariable` | `ScheduleReference` |

```csharp
using UnityEngine;
using YanickSenn.Utils.Variables;

public class HealthController : MonoBehaviour
{
    [SerializeField] private FloatReference maxHealth;
    [SerializeField] private FloatVariable currentHealth;

    private void Start()
    {
        currentHealth.Value = maxHealth.Value;
        currentHealth.OnValueChanged += (newValue, oldValue) => {
            Debug.Log($"Health changed: {oldValue} -> {newValue}");
        };
    }
}
```

### ⚙️ Global Events (`YanickSenn.Utils.Events`)

Decouple your game logic by using `GlobalEvent` assets. Listeners can be attached via code or using the `GlobalEventListener` component.

```csharp
using UnityEngine;
using YanickSenn.Utils.Events;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GlobalEvent onPlayerSpawned;

    public void Spawn()
    {
        onPlayerSpawned.Invoke("Player1", GlobalEvent.Sender.Of(gameObject));
    }
}
```

### ⚙️ VContainer Provider Lifetime Scope (`YanickSenn.Utils.VContainer`)

The `ProviderLifetimeScope` simplifies registrations by using `[Provides]` attribute-based methods.

```csharp
using VContainer;
using YanickSenn.Utils.VContainer;
using YanickSenn.Utils.VContainer.Attributes;
using YanickSenn.Utils.VContainer.Enums;

public class GameLifetimeScope : ProviderLifetimeScope
{
    [Provides]
    [RegisterLifetime(Lifetime.Singleton)]
    public IPlayerService ProvidePlayerService(IInputService input) => new PlayerService(input);

    [Provides(RegistrationType.Instance, "GameConfig")]
    public GameConfig ProvideConfig() => new GameConfig { Difficulty = "Hard" };
}
```

### ⚙️ Observable (`YanickSenn.Utils.Control`)

Wrapper for tracking and reacting to value changes directly from the Inspector.

```csharp
[SerializeField] private Observable<int> score = new(0);

private void Start()
{
    score.OnValueChanged += (newVal, oldVal) => Debug.Log($"Score: {newVal}");
}

public void AddPoint() => score.Value++;
```

### ⚙️ Optional (`YanickSenn.Utils.Control`)

Functional wrapper for handling nullable values.

```csharp
[SerializeField] private Optional<string> optionalName = new("Hero");

private void Start()
{
    optionalName.DoIfPresent(name => Debug.Log($"Hello, {name}"));
    string finalName = optionalName.OrElse("Default");
}
```

### ⚙️ Cooldown (`YanickSenn.Utils.Control`)

Simple time-based tracker to manage repeating actions.

```csharp
[SerializeField] private Cooldown dashCooldown = new(1.5f);

private void Update()
{
    if (Input.GetKeyDown(KeyCode.Space) && dashCooldown.Use())
    {
        // Dash logic...
    }
}
```

### ⚙️ Prefab (`YanickSenn.Utils.Control`)

Typed prefab references that ensure the selected object has the required component.

```csharp
[SerializeField] private Prefab<Rigidbody> projectilePrefab;

public void Fire()
{
    Rigidbody instance = projectilePrefab.Instantiate(transform.position, transform.rotation);
}
```

### ⚙️ State Machine (`YanickSenn.Utils.Control`)

A minimalistic generic state machine for object logic.

```csharp
public class MyState : IStateMachineState<MyState>
{
    public void EnterState(MyState previousState) { }
    public void ExitState(MyState newState) { }
}

public class StateManager : MonoBehaviour
{
    private StateMachine<MyState> _stateMachine;

    private void Start()
    {
        _stateMachine = new StateMachine<MyState>(new MyState());
        _stateMachine.SetCurrentState(new MyState());
    }
}
```

### ⚙️ Sequencer (`YanickSenn.Utils.Control`)

Manager for stepping through and reacting to changes in sequential lists.

```csharp
private Sequencer<string> _levels;

private void Start()
{
    _levels = new Sequencer<string>(new List<string> { "Level 1", "Level 2", "Level 3" });
    _levels.OnValueChanged += (newLevel, oldLevel) => Debug.Log($"Loading {newLevel}");
}

public void NextLevel() => _levels.Next();
```

### ⚙️ Scheduler (`YanickSenn.Utils.Misc`)

Schedule single or periodic tasks using the `Scheduler` MonoBehaviour.

```csharp
using UnityEngine;
using YanickSenn.Utils.Misc;

public class Spawner : MonoBehaviour, ISchedulerEventHandler
{
    [SerializeField] private Scheduler scheduler;

    private void Start()
    {
        scheduler.Schedule(new SchedulerOrder(this, "spawn_wave", new Schedule { Delay = 5f, IsPeriodic = true, Interval = 10f }));
    }

    public void OnScheduledEvent(string orderId)
    {
        if (orderId == "spawn_wave") { /* Spawn enemies */ }
    }
}
```

### ⚙️ Vector3 PID Controller (`YanickSenn.Utils.Misc`)

Calculate required forces to reach targets smoothly without overshooting.

```csharp
using UnityEngine;
using YanickSenn.Utils.Misc;

public class HoverPad : MonoBehaviour
{
    [SerializeField] private Transform targetHeight;
    [SerializeField] private Rigidbody rb;
    private Vector3Pid _pid = new Vector3Pid(10f, 0.5f, 2f, new Vector3Pid.Clamped(50f));

    private void FixedUpdate()
    {
        _pid.Calculate(transform.position, targetHeight.position, Time.fixedDeltaTime);
        rb.AddForce(_pid.Value);
    }
}
```

### ⚙️ Follower & Vector3Mask (`YanickSenn.Utils.Misc`)

Easily make objects follow an `Anchor` with specific axis restrictions using `Vector3Mask`.

```csharp
using UnityEngine;
using YanickSenn.Utils.Misc;

// Component logic is built-in: Add `Anchor` to target, `Follower` to following object, 
// and assign the anchor, offset, and mask in the Inspector.
```

### ⚙️ Runtime Sets (`YanickSenn.Utils.RuntimeSets`)

Use `RuntimeSet` ScriptableObjects to globally reference active GameObjects (e.g., active enemies).

```csharp
using UnityEngine;
using YanickSenn.Utils.RuntimeSets;

public class EnemyListTracker : MonoBehaviour
{
    [SerializeField] private RuntimeSet enemySet;

    private void OnEnable() => enemySet.Add(gameObject);
    private void OnDisable() => enemySet.Remove(gameObject);
}
```

### ⚙️ Feature Flags (`YanickSenn.Utils.Features`)

Toggle features easily through the Inspector.

```csharp
using UnityEngine;
using YanickSenn.Utils.Features;

public class FeatureToggle : MonoBehaviour
{
    [SerializeField] private FeatureFlag newUIFeature;

    private void Start()
    {
        if (newUIFeature.IsEnabled) { /* Enable New UI */ }
    }
}
```

### ⚙️ Physics Snapshots (`YanickSenn.Utils.Snapshots`)

Capture and restore `Rigidbody` and `Collider` properties easily.

```csharp
using UnityEngine;
using YanickSenn.Utils.Snapshots;

public class SnapshotExample : MonoBehaviour
{
    private RigidbodySnapshot _initialRbState;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _initialRbState = RigidbodySnapshot.From(_rb);
    }

    public void ResetPhysics()
    {
        _initialRbState.ApplyTo(_rb);
    }
}
```

### ⚙️ Class Type Serialization (`YanickSenn.Utils.Control`)

Serialize `System.Type` references in the Inspector with a dropdown, filtered by a base class.

```csharp
using UnityEngine;
using YanickSenn.Utils.Control;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField, ClassTypeConstraint(typeof(Enemy), allowAbstract: false)]
    private ClassTypeReference enemyType;

    public abstract class Enemy : MonoBehaviour { }
    public class Orc : Enemy { }
}
```

## 🔧 Requirements

* **Unity Version:** 6000.0 or higher
* **Dependencies:**
* `jp.hadashikick.vcontainer`
