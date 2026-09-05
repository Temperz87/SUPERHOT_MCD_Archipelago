using System;
using System.Collections.Generic;
using Assets.Scripts.Weapons;
using Assets.Scripts.Mods;
using SUPERHOT_MCD_Mod;

public enum ArchipelagoItem
{
    PISTOL_FIRING_CLEARANCE = 1,
    SHOTGUN_FIRING_CLEARANCE = 2,
    MACHINEGUN_FIRING_CLEARANCE = 3,
    SNIPERRIFLE_FIRING_CLEARANCE = 4,
    PRIVILEGE_ESCALATION_SHORT = 5,
    PRIVILEGE_ESCALATION_LONG = 6,
    PRIVILEGE_ESCALATION_CORE = 7,
    PRIVILEGE_ESCALATION_LOST = 8,
    CORRUPTED_MEMORY = 9,
    MOREcore = 10,
    HOTSWITCHcore = 11,
    RECALLcore = 12,
    CHARGEcore = 13,
    PUREcore = 14,
    HPhack4 = 15,
    HPhack5 = 16,
    rechargehack = 17,
    chainchrghack = 18,
    prfswitchhack = 19,
    ultraswitchhack = 20,
    piercinghack = 21,
    flwrecallhack = 22,
    HPhack3 = 23,
    grenadehack = 24,
    explodehack = 25,
    supthrowhack = 26,
    piercshothack = 27,
    wpnmstrhack = 28,
    defallhack = 29,
    ricochethack = 30,
    suppunchhack = 31,
    shotflowhack = 32,
    berserkhack = 33,
    killreloadhack = 34,
    dthstomphack = 35,
    lightreflxhack = 36,
    killhealhack = 37,
    healhack = 39,
}

public static class ArchipelagoDataManager
{
    public static string SaveFile = null;
    public static HashSet<MindID> UnlockedCharacters = new();
    public static HashSet<Modifier> UnlockedMods = new();
    public static HashSet<WeaponID> UnlockedGuns = new();
    public static int FloorPrivilege = 0;
    public static Dictionary<RunID, RunID> RemappedRuns = new();

    public static void UnlockItem(ArchipelagoItem item)
    {
        Plugin.Logger.LogDebug($"Unlocking item {item} of id {(int)item}");
        if (item == ArchipelagoItem.CORRUPTED_MEMORY)
            return;
        else if (item >= ArchipelagoItem.PISTOL_FIRING_CLEARANCE && item <= ArchipelagoItem.SNIPERRIFLE_FIRING_CLEARANCE)
        {
            UnlockedGuns.Add(ItemToWeaponID(item));
        }
        else if (item >= ArchipelagoItem.PRIVILEGE_ESCALATION_SHORT && item <= ArchipelagoItem.PRIVILEGE_ESCALATION_LOST)
        {
            FloorPrivilege = item - ArchipelagoItem.PRIVILEGE_ESCALATION_SHORT + 1;
        }
        else if (item >= ArchipelagoItem.MOREcore && item <= ArchipelagoItem.PUREcore) {
            MindID mind = (MindID)(item - ArchipelagoItem.MOREcore);
            UnlockedCharacters.Add(mind);
        } 
        else
        {
            // TODO: If the player is in the menu show the animation of the hack being unlocked
            // We unlocked a hack!
            // We don't need to inform superhot that the hack is unlocked
            // Because we have a patch setup for PlayerUpgrades that'll report all unlocked mods
            //  directly form the Archipelago
            Modifier mod = ItemToModifier(item);
            UnlockedMods.Add(mod);
        }
    }

    // Sadly the WeaponID's in MCD aren't contiguous
    // So we have to create a function for conversion
    private static WeaponID ItemToWeaponID(ArchipelagoItem item)
    {
        switch (item)
        {
            case ArchipelagoItem.PISTOL_FIRING_CLEARANCE:
                return WeaponID.Pistol;
            case ArchipelagoItem.SHOTGUN_FIRING_CLEARANCE:
                return WeaponID.Shotgun;
            case ArchipelagoItem.MACHINEGUN_FIRING_CLEARANCE:
                return WeaponID.MachineGun;
            case ArchipelagoItem.SNIPERRIFLE_FIRING_CLEARANCE:
                return WeaponID.SniperRifle;
            default:
                return WeaponID.None;
        }
    }

    private static Modifier ItemToModifier(ArchipelagoItem item)
    {
        List<Modifier> allModifiers = GameData.Instance.GameplayModifiers.AllModifiers;
        Type t = null;
        switch (item)
        {
            case ArchipelagoItem.HPhack3:
                foreach (Modifier mod in allModifiers)
                {
                    // TODO: The way the translation system works breaks this
                    // In other languages "one" and "hp" can be translated differently
                    if (mod is OneHPMod && (mod.Name == "OneHP"))
                        return mod;
                }
                Plugin.Logger.LogError("Couldn't find 3HP.hack");
                throw new KeyNotFoundException("Couldn't find 3HP.hack");
            case ArchipelagoItem.HPhack4:
                foreach (Modifier mod in allModifiers)
                    if (mod is OneHPMod && (mod.Name == "TwoHP"))
                        return mod;
                Plugin.Logger.LogError("Couldn't find 4HP.hack");
                throw new KeyNotFoundException("Couldn't find 4HP.hack");
            case ArchipelagoItem.HPhack5:
                foreach (Modifier mod in allModifiers)
                    if (mod is OneHPMod && (mod.Name == "ThreeHP"))
                        return mod;
                Plugin.Logger.LogError("Couldn't find 5HP.hack");
                throw new KeyNotFoundException("Couldn't find 5HP.hack");
            case ArchipelagoItem.rechargehack:
                foreach (Modifier mod in allModifiers)
                    if (mod is ChargeMod && ((ChargeMod)mod).recharge)
                        return mod;
                Plugin.Logger.LogError("Couldn't find recharge.hack");
                throw new KeyNotFoundException("Couldn't find chainchrg.hack");
            case ArchipelagoItem.chainchrghack:
                foreach (Modifier mod in allModifiers)
                    if (mod is ChargeMod && ((ChargeMod)mod).chainCharge)
                        return mod;
                Plugin.Logger.LogError("Couldn't find chainchrg.hack");
                throw new KeyNotFoundException("Couldn't find chainchrg.hack");
            case ArchipelagoItem.prfswitchhack:
                foreach (Modifier mod in allModifiers)
                    if (mod is HotswitchModifier && ((HotswitchModifier)mod).keepWeapon)
                        return mod;
                Plugin.Logger.LogError("Couldn't find prfswitch.hack");
                throw new KeyNotFoundException("Couldn't find prfswitch.hack");
            case ArchipelagoItem.ultraswitchhack:
                foreach (Modifier mod in allModifiers)
                    if (mod is HotswitchModifier && ((HotswitchModifier)mod).chainHotswitch)
                        return mod;
                Plugin.Logger.LogError("Couldn't find ultraswitchk.hack");
                throw new KeyNotFoundException("Couldn't find ultraswitch.hack");
            case ArchipelagoItem.piercinghack:
                t = typeof(PiercingKatanaMod);
                break;
            case ArchipelagoItem.flwrecallhack:
                t = typeof(FlowRecallMod);
                break;
            case ArchipelagoItem.grenadehack:
                t = typeof(ExplodingThrowablesMod);
                break;
            case ArchipelagoItem.explodehack:
                t = typeof(ExplodeMod);
                break;
            case ArchipelagoItem.supthrowhack:
                t = typeof(SuperThrows);
                break;
            case ArchipelagoItem.piercshothack:
                t = typeof(PiercingShot);
                break;
            case ArchipelagoItem.wpnmstrhack:
                t = typeof(WeaponMasterMod);
                break;
            case ArchipelagoItem.defallhack:
                t = typeof(MassDeflectMod);
                break;
            case ArchipelagoItem.ricochethack:
                t = typeof(RicochetMod);
                break;
            case ArchipelagoItem.suppunchhack:
                t = typeof(SuperPunches);
                break;
            case ArchipelagoItem.shotflowhack:
                t = typeof(HeadshotFlowMod);
                break;
            case ArchipelagoItem.berserkhack:
                t = typeof(BerserkMod);
                break;
            case ArchipelagoItem.killreloadhack:
                t = typeof(KillReloadMod);
                break;
            case ArchipelagoItem.dthstomphack:
                t = typeof(GoombaStompMod);
                break;
            case ArchipelagoItem.lightreflxhack:
                t = typeof(LightningReflexesMod);
                break;
            case ArchipelagoItem.killhealhack:
                t = typeof(KillHealMod);
                break;
            case ArchipelagoItem.healhack:
                t = typeof(FullHealMod);
                break;
            default:
                t = null;
                break;
        }

        if (t == null)
        {
            // Either an ambiguous case above, or an item we don't know how to map.
            throw new KeyNotFoundException("Couldn't find modifier for item " + item.ToString());
        }

        return allModifiers.Find(m => m.GetType() == t);
    }
}
