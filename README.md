# com.yanicksenn.utils
URL: https://github.com/yanicksenn/com.yanicksenn.utils.git

![Version](https://img.shields.io/badge/version-[VERSION]-blue)
![Unity Version](https://img.shields.io/badge/unity-[UNITY_VERSION]+-lightgrey)

**com.yanicksenn.utils** is a Utility/Tool for Unity that provides essential architectural components, functional control structures, extensions, and editor enhancements to streamline development.

## ✨ Features

- **ScriptableObject Variables:** Share data (Bool, Int, Float, GameObject, Material) across scenes and prefabs using ScriptableObjects.
- **Reference System:** `Reference<T>` allows inspectors to toggle between constant values and shared ScriptableObject Variables.
- **Global Events:** Decoupled event system (`GlobalEvent`, `BoolEvent`, etc.) with support for payloads, senders, and debug logging.
- **Control Structures:** `Optional<T>` for null-safe functional programming and `Observable<T>` for reactive value changes.
- **Extensions:** Comprehensive extension methods for `Vector3` (masking, PID), `GameObject`, and `Component`.
- **Runtime Sets:** Manage lists of GameObjects at runtime via ScriptableObjects (`RuntimeSet`).
- **Snapshots:** Utilities to capture and restore `Rigidbody` and `Collider` states (`RigidbodySnapshot`, `ColliderSnapshot`).
- **Editor Enhancements:** Custom property drawers for types (`ClassTypeReference`), references, and vector masks.
- **VContainer Integration:** Helper classes like `PlainLifetimeScope` and `ScriptableObjectInstaller` to facilitate dependency injection.

## 📦 Installation

### via Git URL
Open the Unity Package Manager, click the "+" icon, select "Add package from git URL...", and paste:

```
https://github.com/yanicksenn/com.yanicksenn.utils.git#[VERSION]
```

## 🚀 Usage

```csharp
using UnityEngine;
using YanickSenn.Utils.Control;
using YanickSenn.Utils.Variables;

public class PlayerController : MonoBehaviour
{
    // 1. Use Reference<T> to allow Inspector choice between a constant value or a ScriptableObject Variable
    [SerializeField] private FloatReference movementSpeed;

    // 2. Use Optional<T> for safer null handling
    private Optional<Transform> _target;

    private void Start()
    {
        // Access the value transparently
        Debug.Log($"Current Speed: {movementSpeed.Value}");

        _target = new Optional<Transform>(transform);

        // Functional chain: Filter -> Action
        _target
            .Filter(t => t.position.y > 0)
            .DoIfPresent(t => Debug.Log($"{t.name} is above ground."));
    }
}
```

## 🔧 Requirements

* **Unity Version:** [UNITY_VERSION] or higher
* **Dependencies:**
*   VContainer