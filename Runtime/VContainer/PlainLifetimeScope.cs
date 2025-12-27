using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace YanickSenn.Utils.VContainer {
    public class PlainLifetimeScope : LifetimeScope {
        [SerializeField]
        private List<ScriptableObjectInstaller> scriptableObjectInstallers;

        protected override void Configure(IContainerBuilder builder) {
            InstallScriptableObjects(builder);
        }

        private void InstallScriptableObjects(IContainerBuilder builder) {
            if (scriptableObjectInstallers == null) {
                return;
            }

            foreach (var installer in scriptableObjectInstallers) {
                installer.Install(builder);
            }
        }
    }
}