using System.Collections.Generic;
using Assets.Scripts.Weapons;

public static class ArchipelagoDataManager
{
    public static HashSet<MindID> UnlockedCharacters = new();
    public static HashSet<Modifier> UnlockedMods = new();
    public static HashSet<WeaponID> UnlockedGuns = new();
    public static Dictionary<RunID, RunID> RemappedRuns = new();
}
