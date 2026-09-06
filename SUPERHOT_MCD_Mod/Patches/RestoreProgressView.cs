using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using MCDView;

[HarmonyPatch]
public static class Inject_WinCondition
{
    [HarmonyPatch(typeof(RestoreProgressView), "StartRestoreProcess")]
    [HarmonyPostfix]
    public static void Postfix()
    {
        if (ArchipelagoManager.Connected)
            ArchipelagoManager.Win();
    }
}
