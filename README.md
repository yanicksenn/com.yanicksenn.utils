# com.yanicksenn.utils
URL: https://github.com/yanicksenn/com.yanicksenn.utils.git

![Version](https://img.shields.io/badge/version-[VERSION]-blue)
![Unity Version](https://img.shields.io/badge/unity-[UNITY_VERSION]+-lightgrey)

**com.yanicksenn.utils** is a Utility library for Unity that provides architectural primitives (ScriptableObject-based variables, events, sets) and robust control structures to facilitate decoupled and clean game development.

## 📦 Installation

### via Git URL
Open the Unity Package Manager, click the "+" icon, select "Add package from git URL...", and paste:

```
https://github.com/yanicksenn/com.yanicksenn.utils.git#[VERSION]
```

## ✨ Features

- **ScriptableObject Variables:** Type-safe, shareable data containers (Float, Int, Bool, etc.) that can be referenced by components.
- **Global Event System:** Decoupled event architecture using ScriptableObjects to manage game-wide messaging.
- **Runtime Sets:** Efficient management of groups of GameObjects using ScriptableObjects.
- **Control Structures:** Functional wrappers like `Optional<T>` and `Observable<T>` for safer code execution.
- **Extensions:** A suite of extension methods for `Vector3`, `GameObject`, and `Component` to simplify common operations.
- **VContainer Integration:** Utilities to integrate ScriptableObjects (like Installers) with the VContainer dependency injection framework.

## 🚀 Usage

### ⚙️ Variables and References

Use `Reference<T>` types in your components to allow assignment of either constant values or shared `Variable<T>` assets from the Inspector. This decouples logic from data storage.

```csharp
using UnityEngine;
using YanickSenn.Utils.Variables;

public class PlayerHealth : MonoBehaviour
{
    // In the Inspector, you can assign a constant float or a FloatVariable asset
    [SerializeField] private FloatReference maxHealth;
    [SerializeField] private FloatVariable currentHealth;

    private void Start()
    {
        // Accessing .Value transparently handles the reference type (Constant vs Variable)
        currentHealth.Value = maxHealth.Value;
        
        Debug.Log($"Health initialized to: {currentHealth.Value}");
    }
}
```

### ⚙️ Global Events

Create `GlobalEvent` assets to trigger events without direct dependencies between senders and receivers. Listeners can be attached via the `GlobalEventListener` component or code.

```csharp
using UnityEngine;
using YanickSenn.Utils.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GlobalEvent onGameStart;

    public void StartGame()
    {
        // Trigger the event
        onGameStart.Invoke();
        
        // You can also pass a sender or payload for context
        onGameStart.Invoke(GlobalEvent.Sender.Of(gameObject));
    }
}
```

### ⚙️ Optional Values

Use `Optional<T>` to handle values that might be missing, avoiding null checks and enabling functional-style operations.

```csharp
using UnityEngine;
using YanickSenn.Utils.Control;

public class TargetFinder : MonoBehaviour
{
    public void ProcessTarget(GameObject target)
    {
        var optionalTarget = new Optional<GameObject>(target);

        // Chain operations safely
        optionalTarget
            .Filter(t => t.activeInHierarchy)
            .DoIfPresent(t => Debug.Log($"Active target found: {t.name}"));
            
        optionalTarget
            .DoIfAbsent(() => Debug.Log("No valid target provided."));
    }
}
```

## 🔧 Requirements

* **Unity Version:** [UNITY_VERSION] or higher
* **Dependencies:**
* VContainer
* [List any dependencies found in package.json/manifest.json]