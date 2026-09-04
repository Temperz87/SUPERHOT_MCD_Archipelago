using System;
using HarmonyLib;

// Patches responsible for making sure the Archipelago data gets used
// Instead of the original save file

// TODO: This does not work!
// Need another way of ensuring the save file doesn't get written to

// [HarmonyPatch]
// public static class Ensure_ArchipelagoDataOnly
// {
//     [HarmonyPatch(typeof(SHRLSaveManager), nameof(SHRLSaveManager.SaveAsync))]
//     public static bool Prefix()
//     {
//         return ArchipelagoManager.Connected;
//     }
// }
