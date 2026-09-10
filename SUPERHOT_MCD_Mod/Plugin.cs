using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SUPERHOT_MCD_Mod;

[BepInPlugin("tempy.ap.SHMCD", MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin 
{
    // We'll be storing savefiles here!
    public static string PluginFolder {get; private set; }
    public static Plugin instance {get; private set;}
    internal static new ManualLogSource Logger;
    private readonly Harmony harmony = new Harmony("tempy.ap.SHMCD");
        
    private void Awake()
    {
        instance = this;
        PluginFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        Logger = base.Logger;
        Logger.LogInfo("Plugin \"tempy.ap.SHMCD\" is loading...");
        harmony.PatchAll();
        Logger.LogInfo("Patched!..");

        string connectionPath = Path.Combine(PluginFolder, "connection_info.json");
        if (!File.Exists(connectionPath))
            return;

        string json = File.ReadAllText(connectionPath);
        JObject data = JObject.Parse(json);
        ArchipelagoSettingsView.hostname = (string)data["hostname"];
        ArchipelagoSettingsView.port = (string)data["port"];
        ArchipelagoSettingsView.slot = (string)data["slot"];
        ArchipelagoSettingsView.password = (string)data["password"];
    }

    // Dump data back into connection info
    private void OnApplicationQuit()
    {
        Dictionary<string, string> data = new()
        {
            {"hostname", ArchipelagoSettingsView.hostname},
            {"port", ArchipelagoSettingsView.port},
            {"slot", ArchipelagoSettingsView.slot},
            {"password", ArchipelagoSettingsView.password}
        };
        
        JObject json = new()
        {
            new JProperty("hostname", ArchipelagoSettingsView.hostname),
            new JProperty("port", ArchipelagoSettingsView.port),
            new JProperty("slot", ArchipelagoSettingsView.slot),
            new JProperty("password", ArchipelagoSettingsView.password)
        };

        File.WriteAllText(Path.Combine(PluginFolder, "connection_info.json"), json.ToString(Formatting.Indented));
    }
}   
