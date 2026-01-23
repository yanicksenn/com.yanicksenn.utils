# [com.yanicksenn.utils]
URL: https://github.com/yanicksenn/com.yanicksenn.utils.git

![Version](https://img.shields.io/badge/version-2.0.5-blue)
![Unity Version](https://img.shields.io/badge/unity-6000.0+-lightgrey)

**[com.yanicksenn.utils]** is a [Utility Library] for [Unity] that [provides a collection of quality-of-life tools, ScriptableObject architectures, and extension methods to streamline development].

## 📦 Installation

### via Git URL
Open the Unity Package Manager, click the "+" icon, select "Add package from git URL...", and paste:

```
https://github.com/yanicksenn/com.yanicksenn.utils.git#2.0.5
```

## ✨ Features

- **ScriptableObject Variables:** Share primitive data (Float, Int, Bool, etc.) across scenes using assets.
- **Smart References:** Inspector fields that can switch between constant values and ScriptableObject Variables (`Reference<T>`).
- **Global Event System:** Decoupled event architecture using ScriptableObjects with optional payloads and debugging.
- **PID Controller:** A `Vector3Pid` implementation for physics-based smoothing and control.
- **Class Type Serialization:** Select System.Type references in the Inspector with filtering support.
- **Optional & Observable:** Functional wrappers for handling nullable values and value changes (`Optional<T>`, `Observable<T>`).
- **Runtime Sets:** Manage collections of active GameObjects via ScriptableObjects.
- **Physics Snapshots:** Capture and restore the state of Colliders and Rigidbodies.
- **VContainer Utilities:** Helper classes like `ScriptableObjectInstaller` for dependency injection workflows.

## 🚀 Usage

### ⚙️ Variables & References

Use `Reference<T>` to allow flexibility in the Inspector. You can assign a constant value or link a `Variable<T>` asset (e.g., `FloatVariable`, `IntVariable`).

```csharp
using UnityEngine;
using YanickSenn.Utils.Variables;

public class HealthController : MonoBehaviour
{
    // In the Inspector, you can choose "Use Constant" or "Use Variable"
    [SerializeField] private FloatReference maxHealth;
    
    // Direct reference to a shared variable asset
    [SerializeField] private FloatVariable currentHealth;

    private void Start()
    {
        // Access value via .Value
        currentHealth.Value = maxHealth.Value;

        // Listen for changes
        currentHealth.OnValueChanged += (newValue, oldValue) => {
            Debug.Log($"Health changed: {oldValue} -> {newValue}");
        };
    }
}
```

### ⚙️ Global Events

Decouple your game logic by using `GlobalEvent` assets. Listeners can be attached via code or using the `GlobalEventListener` component.

```csharp
using UnityEngine;
using YanickSenn.Utils.Events;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GlobalEvent onPlayerSpawned;

    public void Spawn()
    {
        // Logic to spawn player...
        
        // Invoke the event with optional payload and sender details
        onPlayerSpawned.Invoke(
            payload: "Player1", 
            sender: GlobalEvent.Sender.Of(gameObject)
        );
    }
}
```

### ⚙️ Vector3 PID Controller

Use `Vector3Pid` to calculate forces or movements required to reach a target smoothly without overshooting.

```csharp
using UnityEngine;
using YanickSenn.Utils.Misc;

public class HoverPad : MonoBehaviour
{
    [SerializeField] private Transform targetHeight;
    [SerializeField] private Rigidbody rb;

    // Configure Proportional, Integral, Derivative gains and Clamping
    private Vector3Pid _pid = new Vector3Pid(
        kp: 10f, 
        ki: 0.5f, 
        kd: 2f, 
        clampStrategy: new Vector3Pid.Clamped(50f)
    );

    private void FixedUpdate()
    {
        if (targetHeight == null) return;

        // Calculate required output to reach target position
        _pid.Calculate(transform.position, targetHeight.position, Time.fixedDeltaTime);

        // Apply result to rigidbody
        rb.AddForce(_pid.Value);
    }
}
```

### ⚙️ Class Type Serialization

Serialize `System.Type` references in the Inspector with a dropdown, filtered by a base class.

```csharp
using UnityEngine;
using YanickSenn.Utils.Control;

public class EnemyFactory : MonoBehaviour
{
    // Filter dropdown to only show types inheriting from 'Enemy'
    [SerializeField, ClassTypeConstraint(typeof(Enemy), allowAbstract: false)]
    private ClassTypeReference enemyType;

    private void Start()
    {
        if (enemyType.Type != null)
        {
            Debug.Log($"Selected Enemy Type: {enemyType.Type.FullName}");
        }
    }

    public abstract class Enemy : MonoBehaviour { }
    public class Orc : Enemy { }
    public class Goblin : Enemy { }
}
```

## 🔧 Requirements

* **Unity Version:** 6000.0 or higher
* **Dependencies:**
* `jp.hadashikick.vcontainer`