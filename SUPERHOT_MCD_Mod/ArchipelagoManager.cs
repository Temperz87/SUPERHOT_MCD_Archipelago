using System;
using System.IO;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Models;
using BepInEx.Logging;
using SUPERHOT_MCD_Mod;

public static class ArchipelagoManager
{
    // TODO: Conditionally set this!
    // Currently true for debugging purposes
    public static bool Connected {get; private set;} = false;
    private static ArchipelagoSession session = null;

    public static bool Connect(string ip, ushort port, string slotName, string password)
    {
        session = ArchipelagoSessionFactory.CreateSession(ip, port);

        // TODO: proper flags
        LoginResult result;
        try 
        {
            result = session.TryConnectAndLogin("SUPERHOT: MIND CONTROL DELETE", slotName, 
                                                Archipelago.MultiClient.Net.Enums.ItemsHandlingFlags.AllItems,
                                                password: password);
            Connected = result.Successful;
        }
        catch (Exception e)
        {
            result = new LoginFailure(e.GetBaseException().Message);
            Plugin.Logger.LogError(result);
            throw e;
        }

        if (!Connected)
            return false;   

        OnConnect();
        return true;
    }

    private static void OnConnect()
    {
        // Set seed for save file
        ArchipelagoDataManager.SaveFile = Path.Combine(Plugin.PluginFolder, "shmcd.save." + session.RoomState.Seed);
        Plugin.Logger.LogDebug("Here!");

        // We're going to offload the entire save manager to persistent data
        // Hence start by clearing all data in it
        // (does NOT delete the save)

        if (SHRLSaveManager.Instance)
            SHRLSaveManager.Instance.ClearSaveManager();
        
        Plugin.Logger.LogDebug("Here2!");
        EventManager.Subscribe(SHRLManager.RunEvent.OnWin, RunWon);
        Plugin.Logger.LogDebug("Here3!");
        // TODO: Handle level randomization
        session.Items.ItemReceived += OnReceiveItem;
        Plugin.Logger.LogDebug("Here4!");
        session.Socket.SocketClosed += (reason) =>
        {
            // TODO: Graceful disconnect
            Connected = false;
            EventManager.Unsubscribe(SHRLManager.RunEvent.OnWin, RunWon);
            Plugin.Logger.LogWarning("Disconnected from Archipelago");
        };

        Plugin.Logger.LogDebug("Here5!");
        // Sync items on reconnect
        while (session.Items.Any())
            OnReceiveItem((Archipelago.MultiClient.Net.Helpers.ReceivedItemsHelper)session.Items);
        Plugin.Logger.LogDebug("Here6!");
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
            throw e;
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
}
