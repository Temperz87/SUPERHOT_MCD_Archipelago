
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using MCDView;

[HarmonyPatch]
public static class Ensure_OnlyViewUnlockedHacks
{
    [HarmonyPatch(typeof(HackUnlockView), "PrepareMods")]
    [HarmonyPrefix]
    public static void Prefix(ref ModTerminalInfo ___modTerminalInfo)
    {
        ___modTerminalInfo.Unlocks = ___modTerminalInfo.Unlocks
            .Where(ArchipelagoDataManager.UnlockedMods.Contains).ToList();
    }
}
