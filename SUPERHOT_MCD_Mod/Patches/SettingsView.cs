using HarmonyLib;
using MCDView;

// Patch responsible for getting the Archipelago settings
// Into the settings menu
[HarmonyPatch]
public static class Inject_ArchipelagoSettingsView
{
    [HarmonyPatch(typeof(SettingsView), "Setup")]
    [HarmonyPostfix]
    public static void Postfix_Setup(SettingsView __instance, SHGUIcommanderview ___view)
    {
        object[] args = {___view, typeof(ArchipelagoSettingsView).AssemblyQualifiedName, "Archipelago", "Connect to an Archipelago!"};
        Traverse.Create(__instance).Method("AddCommanderButton", args).GetValue();
    }
}
