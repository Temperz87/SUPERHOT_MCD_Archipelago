using HarmonyLib;

// Harmony patch to catch when someone starts the restore process
// When that process starts, the game has been won

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
