using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace ExampleIntegratedMod;

[BepInAutoPlugin("com.example.integrated", "Example Integrated Plugin", "1.0.0")]
[BepInProcess("Among Us.exe")]
public partial class ExampleIntegratedPlugin : BasePlugin
{
    public Harmony Harmony { get; } = new(Id);

    public override void Load()
    {
        Harmony.PatchAll();
    }

    public override bool Unload()
    {
        Harmony.UnpatchSelf();
        return base.Unload();
    }
}
