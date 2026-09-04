using System;
using HarmonyLib;

// Patches responsible for making sure the Archipelago data gets used
// Instead of the original save file

[HarmonyPatch]
public static class Ensure_ArchipelagoDataOnly
{
    [HarmonyPatch(typeof(SHRLSaveManager), nameof(SHRLSaveManager.SaveAsync))]
    public static bool Prefix()
    {
        return ArchipelagoManager.Connected;
    }
}
