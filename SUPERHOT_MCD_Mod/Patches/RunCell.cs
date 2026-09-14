using HarmonyLib;
using MCDView;

// Don't show the unlock mind view ever if connected

[HarmonyPatch]
public static class Ensure_OnlyUnlockedMindsUnlock
{
    [HarmonyPatch(typeof(RunCell), "UnlockMind")]
    [HarmonyPrefix]
    public static bool Prefix()
    {
        return !ArchipelagoManager.Connected;
    }
}
