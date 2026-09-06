using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using MCDView;

// This patch ensures that the hack screen only shows actually unlocked hacks

[HarmonyPatch]
public static class Ensure_OnlyViewUnlockedHacks
{
    [HarmonyPatch(typeof(HackUnlockView), "PrepareMods")]
    [HarmonyPrefix]
    public static bool Prefix(ref List<Modifier> ___modsToUnlock)
    {
        if (!ArchipelagoManager.Connected)
            return true;

        ___modsToUnlock = ArchipelagoDataManager.PendingViewedHacks.ToList();
        ArchipelagoDataManager.PendingViewedHacks.Clear();
        return true;
    }
}
