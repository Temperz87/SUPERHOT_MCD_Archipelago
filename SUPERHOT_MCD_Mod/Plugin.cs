using System;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace SUPERHOT_MCD_Mod;

[BepInPlugin("tempy.ap.SHMCD", MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    private readonly Harmony harmony = new Harmony("tempy.ap.SHMCD");
        
    private void Awake()
    {
        Logger = base.Logger;
        harmony.PatchAll();
        Logger.LogInfo($"Plugin \"tempy.ap.SHMCD\" is loading...");
        
    }
}   
