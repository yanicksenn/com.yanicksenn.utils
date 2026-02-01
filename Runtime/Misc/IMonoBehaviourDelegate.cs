using UnityEngine;

public interface IMonoBehaviourDelegate {
    void Awake() { }
    void OnEnable() { }
    void Start() { }
    void Update() { }
    void FixedUpdate() { }
    void OnDisable() { }
    void OnDestroy() { }

    void OnDrawGizmos() { }
    void OnDrawGizmosSelected() { }

    void OnCollisionEnter(Collision other) { }
    void OnCollisionStay(Collision other) { }
    void OnCollisionExit(Collision other) { }
}
