# com.yanicksenn.utils
URL: https://github.com/yanicksenn/com.yanicksenn.utils.git

![Version](https://img.shields.io/badge/version-[VERSION]-blue)
![Unity Version](https://img.shields.io/badge/unity-[UNITY_VERSION]+-lightgrey)

**com.yanicksenn.utils** is a Utility Library for Unity that provides ScriptableObject-based architecture primitives, robust control structures, and essential extensions to decouple logic and simplify component development.

## 📦 Installation

### via Git URL
Open the Unity Package Manager, click the "+" icon, select "Add package from git URL...", and paste:

```
https://github.com/yanicksenn/com.yanicksenn.utils.git#[VERSION]
```

## ✨ Features

- **Variables & References:** Architecture primitives (`FloatVariable`, `BoolVariable`, etc.) allowing values to be shared across scenes or toggled between constants and ScriptableObjects in the Inspector.
- **Global Events:** A decoupled event system (`GlobalEvent`, `IntEvent`, etc.) based on ScriptableObjects, supporting payloads, senders, and debug tracing.
- **Control Structures:** Functional wrappers like `Optional<T>`, `Observable<T>`, and `Sequencer<T>` to handle data presence and state changes safely.
- **PID Controller:** A generic `Vector3Pid` implementation for smooth, physics-based target following with configurable clamping strategies.
- **Snapshot System:** Utilities to save and restore state (`RigidbodySnapshot`, `ColliderSnapshot`) for specific components.
- **Extensions:** A suite of extensions for `Vector3`, `GameObject`, and `Component` to reduce boilerplate (e.g., masking vectors, safe destruction).

## 🚀 Usage

### ⚙️ Variables & References

Use `Reference<T>` types (like `FloatReference`) in your MonoBehaviours. This allows you to assign either a hardcoded value or a shared `FloatVariable` asset via the Inspector without changing code.

```csharp
using UnityEngine;
using YanickSenn.Utils.Variables;

public class HealthComponent : MonoBehaviour
{
    // In the Inspector, you can choose "Use Constant" or "Use Variable"
    [SerializeField] private FloatReference maxHealth = new FloatReference(100f);
    
    // Shared variable to expose current health to UI or other systems
    [SerializeField] private FloatVariable currentHealth;

    private void Start()
    {
        // Initialize using the reference value
        currentHealth.Value = maxHealth.Value;

        // Listen for changes on the shared variable
        currentHealth.OnValueChanged += OnHealthChanged;
    }

    private void OnHealthChanged(float newValue, float oldValue)
    {
        Debug.Log($"Health changed from {oldValue} to {newValue}");
    }

    private void OnDestroy()
    {
        currentHealth.OnValueChanged -= OnHealthChanged;
    }
}
```

### ⚙️ Global Events

Decouple systems using `GlobalEvent`. Create a `GlobalEvent` asset in the project, reference it in a sender, and listen to it elsewhere using `GlobalEventListener` or code.

```csharp
using UnityEngine;
using YanickSenn.Utils.Events;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] private GlobalEvent onPlayerEnterArea;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Invoke the event, optionally passing the sender or a payload
            onPlayerEnterArea.Invoke(GlobalEvent.Sender.Of(gameObject));
        }
    }
}
```

### ⚙️ Vector3 PID Controller

Use `Vector3Pid` to smoothly drive a position or velocity towards a target using Proportional-Integral-Derivative logic.

```csharp
using UnityEngine;
using YanickSenn.Utils.Misc;

public class PIDFollower : MonoBehaviour
{
    [SerializeField] private Transform target;
    
    // Configure PID constants: Kp, Ki, Kd
    private Vector3Pid _pidController;

    private void Awake()
    {
        // Initialize with PID values and a clamping strategy
        _pidController = new Vector3Pid(5.0f, 0.1f, 0.5f, new Vector3Pid.Unclamped());
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        // Calculate the required output based on current position and delta time
        _pidController.Calculate(transform.position, target.position, Time.fixedDeltaTime);
        
        // Apply result (e.g., as velocity to a Rigidbody)
        GetComponent<Rigidbody>().linearVelocity = _pidController.Value;
    }
}
```

## 🔧 Requirements

* **Unity Version:** [UNITY_VERSION] or higher
* **Dependencies:**
    * `VContainer` (Detected in source usage)
