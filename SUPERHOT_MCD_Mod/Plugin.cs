using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace SUPERHOT_MCD_Mod;

[BepInPlugin("tempy.ap.SHMCD", MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin 
{
    // We'll be storing savefiles here!
    public static string PluginFolder {get; private set; }
    internal static new ManualLogSource Logger;
    private readonly Harmony harmony = new Harmony("tempy.ap.SHMCD");
        
    private void Awake()
    {
        PluginFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        Logger = base.Logger;
        Logger.LogInfo("Plugin \"tempy.ap.SHMCD\" is loading...");
        harmony.PatchAll();

        Logger.LogInfo("Patched!..");
        bool connected = ArchipelagoManager.Connect("127.0.0.1", 38281, "Temperz87", null);
        if (!connected)
        {
            throw new Exception();
        }
    }
}   
