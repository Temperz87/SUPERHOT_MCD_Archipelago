using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using SUPERHOT_MCD_Mod;

// Patch responsible for making sure the archipelago data gets loaded
// In place of a save file

[HarmonyPatch]
public static class Ensure_APModProgression
{
    [HarmonyPatch(typeof(PlayerUpgrades), "GetUnlockedMods")]
    [HarmonyPrefix]
    private static bool Prefix_GetUnlockedMods(ref List<Modifier> __result)
    {
        if (!ArchipelagoManager.Connected)
            return true;

        __result = ArchipelagoSaveManager.UnlockedMods.ToList();
        foreach (Modifier mod in __result) 
        {
            Plugin.Logger.LogDebug($"{mod.name} is unlocked");
        }
        return false;
    }
    
    [HarmonyPatch(typeof(PlayerUpgrades), nameof(PlayerUpgrades.UnlockMod))]
    [HarmonyPrefix]
    private static bool Prefix_UnlockMod(Modifier mod)
    {
        if (!ArchipelagoManager.Connected)
            return true;

        if (!ArchipelagoSaveManager.UnlockedMods.Contains(mod))
        {
            Plugin.Logger.LogWarning($"Not unlocking mod: {mod.name} due to it not being present in ArchipelagoSaveManager.UnlockedMods!");
            return false;
        }

        return true;
    }
}
