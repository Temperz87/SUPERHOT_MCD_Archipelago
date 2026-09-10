using SUPERHOT_MCD_Mod;
using System;
using UnityEngine.SceneManagement;

public class ConnectButton : SHGUIcommanderbutton
{
    public ConnectButton(ArchipelagoSettingsView parent)
		: base("connect     │>------<", 'w', null, null, true)
    {
        this.parent = parent;
        listLink = parent;
        data = "Connect to:" +
                $"\n\tHost:{ArchipelagoSettingsView.hostname}" + 
                $"\n\tPort:{ArchipelagoSettingsView.port}" +
                $"\n\tSlot:{ArchipelagoSettingsView.slot}";
        RefreshRightPanel();
    }

    public override void Activate()
    {
        data = "Connect to:" +
                $"\n\tHost:{ArchipelagoSettingsView.hostname}" + 
                $"\n\tPort:{ArchipelagoSettingsView.port}" +
                $"\n\tSlot:{ArchipelagoSettingsView.slot}";
        RefreshRightPanel();
    }

    public void Connect(string hostname, string port_text, string slotname, string password)
    {
        Plugin.Logger.LogDebug("Disabling saves");
        ArchipelagoSettingsView.DoNotSave = true;
        Plugin.Logger.LogDebug("Trying to connect...");
        data = "Connecting...";
        RefreshRightPanel();
        if (!ushort.TryParse(port_text, out ushort port))
        {
            data = "Connection failed";
            RefreshRightPanel();
            Plugin.Logger.LogDebug("Enabling saves");
            ArchipelagoSettingsView.DoNotSave = false;
            return;
        }

        bool connected = false;
        string failReason = "";
        try {
            connected = ArchipelagoManager.Connect(hostname, port, slotname, password);
            if (connected)
            {
                data = "Connected, have fun!";
                SHRLSaveManager.Instance.LoadAsync(delegate { });
                RefreshRightPanel();
                Scene currentScene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(currentScene.buildIndex);      
            }
            else
            {
                failReason = "Timed out";
            }
        }
        catch (Exception e)
        {
            failReason = e.Message;
        }
        
        if (!connected)
        {
            data = $"Connection failed!\n{failReason}";
            RefreshRightPanel();
        }

        Plugin.Logger.LogDebug("Enabling saves");
        ArchipelagoSettingsView.DoNotSave = false;
    }

    public override void Deactiavate()
    {
		SHGUI.current.PlaySound(SHGUIsound.tick);
    }
}
