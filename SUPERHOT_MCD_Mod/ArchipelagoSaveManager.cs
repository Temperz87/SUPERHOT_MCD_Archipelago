using System.Collections.Generic;
using Assets.Scripts.Weapons;

public static class ArchipelagoSaveManager
{
    public static HashSet<MindID> UnlockedCharacters = new();
    public static HashSet<Modifier> UnlockedMods = new();
    public static HashSet<WeaponID> UnlockedGuns = new();
}
