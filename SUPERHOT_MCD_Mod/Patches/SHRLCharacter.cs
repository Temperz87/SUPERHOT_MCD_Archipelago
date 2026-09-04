using HarmonyLib;

// We need this patch because of how SUPERHOT MCD detects unlocked minds
// We want mind unlocks to be separate of their hacks unlocks
// However, when a mind gets unlocked their corresponding hacks also do
// Hence the game can check if any hack on the mind is unlocked to see if it's available
// But we want scenarios where no abilities are unlocked for a mind but the mind is available!
// Hence, we make two prefixes 
[HarmonyPatch]
public static class Ensure_APMindProgression
{

    // This delegates "mind unlocks" to ArchipelagoSaveManager
    [HarmonyPatch(typeof(SHRLCharacter), nameof(SHRLCharacter.IsAvailable))]
    [HarmonyPrefix]
    public static bool Prefix_IsAvailable(SHRLCharacter __instance, ref bool __result)
    {
        if (!ArchipelagoManager.Connected)
            return true;
            
        __result = ArchipelagoSaveManager.UnlockedCharacters.Contains(__instance.ID);
        return false;
    }

    // This stops unlocking all hacks for a given mind
    // This patch is redundant as another patch stops unlocking hacks,
    //  however I think this leads to a marginal performance incrase so we'll keep it
    [HarmonyPatch(typeof(SHRLCharacter), nameof(SHRLCharacter.UnlockAbilities))]
    [HarmonyPrefix]
    public static bool Prefix_UnlockAbilities()
    {
        return !ArchipelagoManager.Connected;
    }
}
