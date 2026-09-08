using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using MCDView;
using SUPERHOT_MCD_Mod;
using UnityEngine;

// This patch ensures that the hack screen only shows actually unlocked hacks

[HarmonyPatch]
public static class Ensure_OnlyViewUnlockedHacks
{
    // For some reason harmony complains if we try and use the two ref params anywhere else
    // So we toss it in here
    // And I could use a traverse, but that results in differnt behavior
    // So I'm just leaving this here, as I've spent too long debugging this
    [HarmonyPatch(typeof(HackUnlockView), "PrepareMods")]
    [HarmonyPrefix]
    public static bool Prefix_PrepareMods(ref List<Modifier> ___modsToUnlock, ref bool ___fakeUnlock)
    {
        if (!ArchipelagoManager.Connected)
            return true;

        ___fakeUnlock = true;
        ___modsToUnlock.Clear();
        return false;
    }

    [HarmonyPatch(typeof(HackUnlockView), "GetModsToShow")]
    [HarmonyPrefix]
    public static bool Prefix_GetModsToShow(ref List<Modifier> __result)
    {
        if (!ArchipelagoManager.Connected)
            return true;

        // The "shown" mods should be the already unlocked ones
        __result = ArchipelagoDataManager.UnlockedMods.ToList();
        foreach (Modifier mod in __result)
            Plugin.Logger.LogInfo($"HACKVIEW: Displaying unlocked    mod: {mod.Name}");
        return true;
    }

    [HarmonyPatch(typeof(HackUnlockView), "CreateModButton")]
    [HarmonyPrefix]
    public static void Prefix_CreateModButton(Modifier mod, ref bool unlocked)
    {
        if (!ArchipelagoManager.Connected)
            return;

        unlocked = !ArchipelagoDataManager.PendingViewedHacks.Remove(mod);
    }

    [HarmonyPatch(typeof(HackUnlockView), "GetHeight")]
    [HarmonyPrefix]
    public static bool Prefix_GetHeight(ref int __result)
    {
        if (!ArchipelagoManager.Connected)
            return true;

        __result = Mathf.Clamp(ArchipelagoDataManager.UnlockedMods.Count() + 2, 0, 20);
        return false;
    }

    [HarmonyPatch(typeof(HackUnlockView), "GetModBasedWidth")]
    [HarmonyPrefix]
    public static bool Prefix_GetModBasedWidth(ref int __result)
    {
        if (!ArchipelagoManager.Connected)
            return true;

        int longest = Traverse.Create<HackUnlockView>().Method("GetLengthOfLongestMod", [ArchipelagoDataManager.UnlockedMods.ToList()]).GetValue<int>();
        __result = longest + 11;
        return false;
    }
}
