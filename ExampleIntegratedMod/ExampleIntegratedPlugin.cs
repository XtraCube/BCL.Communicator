using System;
using BCL.Communicator;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using UnityEngine;

namespace ExampleIntegratedMod;

[BepInAutoPlugin("com.example.integrated", "Example Integrated Plugin", "1.0.0")]
[BepInDependency(CommunicatorPlugin.Id)]
[BepInProcess("Among Us.exe")]
public partial class ExampleIntegratedPlugin : BasePlugin
{
    public Harmony Harmony { get; } = new(Id);

    public override void Load()
    {
        AddComponent<ExampleComponent>();
        Harmony.PatchAll();
    }

    public override bool Unload()
    {
        Harmony.UnpatchSelf();
        return base.Unload();
    }

    private sealed class ExampleComponent : MonoBehaviour
    {
        public ExampleComponent(IntPtr ptr) : base(ptr)
        {
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F2))
            {
                CommunicatorPlugin.Logger.LogInfo("Setting channels for local player.");
                PlayerControl.LocalPlayer.SetChannels(["example"], ["example"]);
            }
            if (Input.GetKeyDown(KeyCode.F3))
            {
                CommunicatorPlugin.Logger.LogInfo("Resetting channels for local player.");
                PlayerControl.LocalPlayer.ResetChannels();
            }
        }
    }
}
