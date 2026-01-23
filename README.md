# com.yanicksenn.utils
URL: https://github.com/yanicksenn/com.yanicksenn.utils.git

![Version](https://img.shields.io/badge/version-[VERSION]-blue)
![Unity Version](https://img.shields.io/badge/unity-[UNITY_VERSION]+-lightgrey)

**com.yanicksenn.utils** is a Utility library for Unity that provides architectural patterns (Variables, Events), control structures, extensions, and math tools to simplify component development and state management.

## 📦 Installation

### via Git URL
Open the Unity Package Manager, click the "+" icon, select "Add package from git URL...", and paste:

```
https://github.com/yanicksenn/com.yanicksenn.utils.git#[VERSION]
```

## ✨ Features

- **Scriptable Object Variables:** Primitives wrapped in ScriptableObjects (`FloatVariable`, `BoolVariable`) allowing shared state across scenes and prefabs.
- **Reference System:** `Reference<T>` types allow inspector fields to toggle between constant values and Scriptable Object Variables transparently.
- **Global Event System:** A decoupled, ScriptableObject-based event architecture supporting payloads and debug logging.
- **Control Structures:** Wrappers like `Optional<T>`, `Observable<T>`, and `Prefab<T>` to enhance code safety and readability.
- **PID Controller:** A `Vector3Pid` implementation for calculating smooth physics-based movement towards a target.
- **Snapshot System:** Utilities (`RigidbodySnapshot`, `ColliderSnapshot`) to capture and restore the state of Unity components.
- **Class Type Reference:** Serializable inspector dropdown for selecting C# types with constraints.
- **VContainer Utilities:** Helpers for using VContainer with ScriptableObjects via `PlainLifetimeScope` and `ScriptableObjectInstaller`.

## 🚀 Usage

### ⚙️ Scriptable Object Variables & References

Use `Reference<T>` types in your MonoBehaviours to allow flexibility between hardcoded constants and shared ScriptableObject variables.

```csharp
using UnityEngine;
using YanickSenn.Utils.Variables;

public class PlayerHealth : MonoBehaviour
{
    // In the Inspector, you can choose "Use Constant" or "Use Variable"
    // If "Use Variable" is selected, you can assign a FloatVariable asset.
    [SerializeField] private FloatReference maxHealth;
    
    // Variables act as shared state
    [SerializeField] private FloatVariable currentHealth;

    private void Start()
    {
        // Access .Value to get the underlying float
        currentHealth.Value = maxHealth.Value;
        
        // Listen for changes
        currentHealth.OnValueChanged += (newValue, oldValue) => 
        {
            Debug.Log($"Health changed from {oldValue} to {newValue}");
        };
    }
}
```

### ⚙️ Vector3 PID Controller

Use the `Vector3Pid` class to calculate smooth forces or velocities to move an object towards a target position without overshooting.

```csharp
using UnityEngine;
using YanickSenn.Utils.Misc;

public class SmoothFollower : MonoBehaviour
{
    [SerializeField] private Transform target;
    
    // Configure PID coefficients (Proportional, Integral, Derivative)
    // and a clamping strategy to limit maximum output.
    private Vector3Pid _pid = new Vector3Pid(
        kp: 5.0f, 
        ki: 0.5f, 
        kd: 0.2f, 
        clampStrategy: new Vector3Pid.Clamped(10.0f)
    );

    private void FixedUpdate()
    {
        if (target == null) return;

        // Calculate the PID output based on current position, target, and delta time
        _pid.Calculate(transform.position, target.position, Time.fixedDeltaTime);
        
        // Use the result (e.g., move the object)
        transform.position += _pid.Value * Time.fixedDeltaTime;
    }
    
    public void ResetPID()
    {
        // Clear integral accumulation and previous error history
        _pid.Reset();
    }
}
```

## 🔧 Requirements

* **Unity Version:** [UNITY_VERSION] or higher
* **Dependencies:**
* VContainer