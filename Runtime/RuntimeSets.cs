using System;
using UnityEngine;
using YanickSenn.Utils;

[DisallowMultipleComponent]
public class RuntimeSets : MonoBehaviour {

    [SerializeField]
    private RuntimeSet[] runtimeSets;

    private void OnEnable() {
        foreach (var runtimeSet in runtimeSets) {
            runtimeSet.Add(gameObject);
        }
    }

    private void OnDisable() {
        foreach (var runtimeSet in runtimeSets) {
            runtimeSet.Remove(gameObject);
        }
    }
}