using System;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Newtonsoft.Json.Linq;

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

        string json = File.ReadAllText(Path.Combine(PluginFolder, "connection_info.json"));
        JObject data = JObject.Parse(json);
        string hostname = (string)data["hostname"];
        ushort port = (ushort)data["port"];
        string slot = (string)data["slot"];
        string password = (string)data["password"];
        bool connected = ArchipelagoManager.Connect(hostname, port, slot, password);
        if (!connected)
        {
            // TODO: Actual error handling            
            throw new Exception();
        }
    }
}   
