using System;
using System.Diagnostics;
using System.Reflection;
using HarmonyLib;
using SUPERHOT_MCD_Mod;

// This patch is responsible for firing death link
[HarmonyPatch]
public static class Inject_Deathlink
{
    private static Random random = new Random();
    private static string[] bullet_sources = 
    [
        "SHOT TO DEATH",
        "SHOT BY A RED DUDE",
        "TOOK A BULLET TO THE HEAD"
    ];

    private static string[] katana_sources = 
    [
        "SLICED",
        "BECAME A VICTIM OF KENJUTSU",
        "LOST A SWORDFIGHT"
    ];

    // Out Of Bounds
    private static string[] oob_sources = 
    [
        "WENT OUT OF BOUNDS",
        "FELL TO DEATH",
        "TRIED TO ESCAPE FROM THE SYSTEM"
    ];

    private static string[] ninja_sources = 
    [
        "WAS DEFEATED BY NINDZA",
        "LOST TO RECALL.core",
        "GOT KILLED BY A FLYING KATANA"
    ];

    private static string[] dog_sources = 
    [
        "WAS DEFEATED BY THE DOG",
        "LOST TO CHARGE.core",
        "FOUND OUT CHARGE IS DEADLY"
    ];

    private static string[] melee_sources = 
    [
        "TOOK ONE TOO MANY PUNCHES",
        "PUNCHED BY A RED DUDE",
        "LOST A FIST FIGHT"
    ];

    private static string[] melee_gun_sources = 
    [
        "GOT BARREL STUFFED",
        "GOT PISTOL WHIPPED BY A RED DUE",
        "LOST A MELEE GUN FIGHT"
    ];

    private static string[] melee_weapon_sources = 
    [
        "GOT BONKED ON THE HEAD",
        "WAS HIT WITH A BLUNT OBJECT BY A RED DUE",
        "GOT TOO CLOSE WITH THEIR ENEMY"
    ];

    // Save state to see when the players goes from dying to dead
    // Have to do this because punches spam call kill for some reason
    // (like 100 times in a frame, and sending 100 deathlink messages would kill bandwidth)
    [HarmonyPatch(typeof(PlayerActions), nameof(PlayerActions.Kill))]
    [HarmonyPrefix]
    public static void Prefix_Kill(PlayerActions __instance, out PlayerState __state)
    {
        __state = __instance.state;
    }

    [HarmonyPatch(typeof(PlayerActions), nameof(PlayerActions.Kill))]
    [HarmonyPostfix]
    public static void Postfix_Kill(PlayerActions __instance, PlayerState __state, MethodBase __originalMethod)
    {
        if (!ArchipelagoManager.DeatLinkActive || !(__instance.state == PlayerState.Dying && __state == PlayerState.None))
            return;

        StackTrace stack = new StackTrace();
        string callerName = null;
        foreach (StackFrame frame in stack.GetFrames())
        {
            MethodBase method = frame.GetMethod();
            callerName = $"{method.DeclaringType.FullName}.{method.Name}";
            Plugin.Logger.LogDebug(callerName);
            if (method == null || method == __originalMethod 
                               || method == MethodBase.GetCurrentMethod())
                continue;


            // Skip any methods harmony tosses in 
            if (method.Name.StartsWith("DMD<") || method.DeclaringType == null || 
                    (method.DeclaringType.Namespace != null &&
                    method.DeclaringType.Namespace.StartsWith("HarmonyLib")))
                continue;

            break;
        }

        if (callerName == null)
        {
            Plugin.Logger.LogDebug("Null caller name in postfix kill");
            return;
        }

        Plugin.Logger.LogDebug($"Found caller {callerName}");
        string[] sources = ["UNKNOWN"];
        switch (callerName)
        {
            case "Bullet.HandleCollision":
                sources = bullet_sources;
                break;
            case "KatanaPickup.ApplyKillingEdge":
                sources = katana_sources;
                break;
            case "KillPlayer.OnTriggerEnter":
                sources = oob_sources;
                break;
            case "PejAiController.ApplyKillingEdge":
                sources = ninja_sources;
                break;
            case "PejAiController+DogChargeBehaviour.OnRun":
                sources = dog_sources;
                break;
            case "PejAiController+MeleeAttackBehaviour.OnRun":
                sources = melee_sources;
                break;
            case "PejAiController+MeleeGunAttackBehaviour.OnRun":
                sources = melee_gun_sources;
                break;
            case "PejAiController+MeleeWeaponAttackBehaviour.OnRun":
                sources = melee_weapon_sources;
                break;
            case "PlayerKiller.OnTriggerEnter":
                sources = oob_sources;
                break;
            case "ArchipelagoManager.HandleDeathLink":
                // This gets called by our mod
                // So we don't want a recursive death link situation
                // Hence we just return
                return;
            default:
                Plugin.Logger.LogDebug($"Unkown kill source: {callerName}");
                break;
        }

        int index = random.Next(sources.Length);
        ArchipelagoManager.SendDeathLink(sources[index]);
    }
}