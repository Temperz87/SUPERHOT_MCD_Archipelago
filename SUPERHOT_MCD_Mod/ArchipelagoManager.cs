using System;
using System.IO;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Models;
using Assets.Scripts.Weapons;
using SUPERHOT_MCD_Mod;


// TODO: The hack unlock view and the core unlock view almost never have the right contents 
public static class ArchipelagoManager
{
    public static bool Connected {get; private set;} = false;
    private static ArchipelagoSession session = null;

    public static bool Connect(string ip, ushort port, string slotName, string password)
    {
        Plugin.Logger.LogDebug($"Trying to connect to {ip}:{port} on slot {slotName}");
        session = ArchipelagoSessionFactory.CreateSession(ip, port);
        LoginResult result;
        try 
        {
            result = session.TryConnectAndLogin("SUPERHOT: MIND CONTROL DELETE", slotName, 
                                                Archipelago.MultiClient.Net.Enums.ItemsHandlingFlags.AllItems,
                                                password: password, requestSlotData: true);
            Connected = result.Successful;
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError($"An error has occured while connecting to {ip}:{port}");
            Plugin.Logger.LogError(e.GetBaseException().Message);
            throw e;
        }

        if (!Connected)
        {
            LoginFailure failure = (LoginFailure)result;
            Plugin.Logger.LogError($"An error has occured while connecting to {ip}:{port}");
            foreach (string error in failure.Errors)
                Plugin.Logger.LogError(error);
            return false;   
        }


        // TODO: Setup deathlink, setup randomized level order
        var slotdata = ((LoginSuccessful)result).SlotData;
        Plugin.Logger.LogDebug($"unlockPyramidLayers: {slotdata["unlockPyramidLayers"]}");
        if (slotdata["unlockPyramidLayers"].ToString() == "0")
            ArchipelagoDataManager.FloorPrivilege = 100;
        
        Plugin.Logger.LogDebug($"unlockWeaponFiring: {slotdata["unlockWeaponFiring"]}");
        if (slotdata["unlockWeaponFiring"].ToString() == "0")
        {
            ArchipelagoDataManager.UnlockedGuns.Add(WeaponID.Pistol);
            ArchipelagoDataManager.UnlockedGuns.Add(WeaponID.Shotgun);
            ArchipelagoDataManager.UnlockedGuns.Add(WeaponID.MachineGun);

            // The railguns weapon id is zero???
            ArchipelagoDataManager.UnlockedGuns.Add(0); 
        }

        Plugin.Logger.LogDebug($"randomize level: {slotdata["randomizeLevelOrder"].ToString()}");
        if (slotdata["randomizeLevelOrder"].ToString() == "1")
        {
            string randomstring = slotdata["order_string"].ToString();
            LevelRemapper.Remap(randomstring);
        }

        OnConnect();
        return true;
    }

    private static void OnConnect()
    {
        // Set seed for save file
        ArchipelagoDataManager.SaveFile = Path.Combine(Plugin.PluginFolder, "shmcd.save." + session.RoomState.Seed);
        SHRLSaveManager.Instance.LoadAsync((_) => { });

        // We're going to offload the entire save manager to persistent data
        // Hence start by clearing all data in it
        // (does NOT delete the save)

        if (SHRLSaveManager.Instance)
            SHRLSaveManager.Instance.ClearSaveManager();
        
        EventManager.Subscribe(SHRLManager.RunEvent.OnWin, RunWon);
        // TODO: Handle level randomization
        session.Items.ItemReceived += OnReceiveItem;
        session.Socket.SocketClosed += (reason) =>
        {
            // TODO: Graceful disconnect
            Connected = false;
            EventManager.Unsubscribe(SHRLManager.RunEvent.OnWin, RunWon);
            Plugin.Logger.LogWarning("Disconnected from Archipelago");
        };

        // Sync items on reconnect
        while (session.Items.Any())
            OnReceiveItem((Archipelago.MultiClient.Net.Helpers.ReceivedItemsHelper)session.Items);
    }

    private static void OnReceiveItem(Archipelago.MultiClient.Net.Helpers.ReceivedItemsHelper handler)
    {
        // TODO: When we have a gui for "I GOT AN ITEM!!!" invoke said GUI
        try 
        {
            ItemInfo item = session.Items.DequeueItem();
            ArchipelagoDataManager.UnlockItem((ArchipelagoItem)item.ItemId);   
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError($"Exception thrown while trying to unlock item");
            Plugin.Logger.LogError(e.Message);
        }
    }

    public static void CheckLocation(RunID location)
    {
        if (!Connected)
            return;

        Plugin.Logger.LogDebug($"Checking location {location} of id {(long)location}");
        long id = (long)location;
        if (session.Locations.AllLocationsChecked.Contains(id))
        {
            Plugin.Logger.LogDebug($"Not rechecking location {location}");
            return;
        }

        // If the socket closes then the complete function hangs
        // Hence first we check if the socket is open
        if (session.Socket.Connected)
            session.Locations.CompleteLocationChecks(id);
    }

    // Named function and not a lambda so we can unsubscribe when we disconnect
    private static void RunWon(object[] _) => CheckLocation(SHRLGame.Instance.PlayerStats.CurrentRun.RunID);
    
    public static void Win() 
    {
        Plugin.Logger.LogDebug("A winner is you!");
        session.SetGoalAchieved();
    }
}
