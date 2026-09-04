using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;

[HarmonyPatch]
public static class Inject_APGunProgression
{
    private static Random random = new Random();
    private static string[][] lockedWeaponMessages =
    [
        ["NOT", "YET"],
        ["FIND", "THE", "WEAPONS", "CLEARANCE"],
        ["CAN'T", "FIRE", "THIS", "YET"],
        ["FIRING", "NOT", "CLEARED"],
        ["SAFETY", "IS", "ON"],
        ["LOCKED", "BEHIND", "PROGRESSION"]
    ];

    [HarmonyTargetMethods]
    private static IEnumerable<MethodBase> TargetMethods()
    {
        yield return AccessTools.Method(typeof(Gun), nameof(Gun.Attack));
        yield return AccessTools.Method(typeof(MachineGun), nameof(MachineGun.Attack));
    }
    
    public static bool Prefix(Gun __instance)
    {
        if (!ArchipelagoManager.Connected)
            return true;
        
        if (__instance.ammoCount >= 0 && !ArchipelagoDataManager.UnlockedGuns.Contains(__instance.WeaponId))
        {
            __instance.Invoke("LaunchNoAmmoAnimation", 0);
            if (__instance.weapon_SubType != Weapon_SubType.Shotgun && 
                __instance.weapon_SubType != Weapon_SubType.Pistol && 
                __instance.weapon_SubType != Weapon_SubType.Machinegun)
			{
				__instance.SwitchWeaponPositionToThrowing();
			}


            int index = random.Next(lockedWeaponMessages.Length); 
            string[] message = lockedWeaponMessages[index];
            TextManager.DisplayQuick(message);
            __instance.ammoCount = -1;
            return false;
        }

        return true;
    }
}

[HarmonyPatch]
public static class Inject_ShotgunMeleeWhenNotUnlocked
{
    [HarmonyPatch(typeof(Gun), nameof(Gun.Pickup))]
    public static void Postfix(Gun __instance)
    {
        if (__instance.weapon_SubType == Weapon_SubType.Shotgun && !ArchipelagoDataManager.UnlockedGuns.Contains(Assets.Scripts.Weapons.WeaponID.Shotgun))
        {
            __instance.Invoke("LaunchNoAmmoAnimation", 0);
        }
    }
}