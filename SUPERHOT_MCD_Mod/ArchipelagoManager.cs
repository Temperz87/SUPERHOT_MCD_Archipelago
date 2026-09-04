using System.Collections.Generic;
using Archipelago.MultiClient.Net;
using HarmonyLib;
using SUPERHOT_MCD_Mod;

public static class ArchipelagoManager
{
    // TODO: Conditionally set this!
    // Currently true for debugging purposes
    public static bool Connected {get; private set;} = true;
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
        catch (System.Exception e)
        {
            result = new LoginFailure(e.GetBaseException().Message);
        }

        if (!Connected)
            return false;   

        OnConnect();
        return true;
    }


    // TODO: Make this private
    // Currently public so our entry point can call it for debug purposes
    public static void OnConnect()
    {
        // TODO: Load data from archipelago website
        foreach (Modifier mod in GameData.Instance.GameplayModifiers.AllModifiers)
        {
            // TODO: check data to see if hack has already been unlocked
            if (mod is FullHealMod)
                ArchipelagoDataManager.UnlockedMods.Add(mod);
        }

        // TODO: Unlock minds based on archipelago data
        ArchipelagoDataManager.UnlockedCharacters.Add(MindID.TESTMIND);

        // TODO: Unlock guns based on archipelago data
    }
}
