using MCDView;
using System;
using Assets.Scripts.Utilities;
using SystemStorage;
using SUPERHOT_MCD_Mod;

public class ArchipelagoSettingsView : APPSettings
{
    private bool inButton;

    // User inputs
    public static string hostname = "";

    // Using a string to store user input
    // Will attempt to convert to an ushort later
    public static string port = "";
    public static string slot = "";
    public static string password = "";
    public static bool DoNotSave = false;

    public ArchipelagoSettingsView() : base("ARCHIPELAGO SETTINGS", "ARCHIPELAGO")
    {
        StringSettingsButton hostname_button = new("host name   │>------<", this, hostname, (str) => hostname = str);
        StringSettingsButton port_button = new("port        │>------<", this, port, (str) => port = str);
        StringSettingsButton slot_button = new("slot        │>------<", this, slot, (str) => slot = str);
        StringSettingsButton password_button = new("password    │>------<", this, password, (str) => password = str, true);
        ConnectButton connect_button = new(this);
        AddButtonView(hostname_button);
        AddButtonView(port_button);
        AddButtonView(slot_button);
        AddButtonView(password_button);
        AddButtonView(connect_button);
    }

    public override void ReactToInputKeyboard(SHGUIinput key)
    {
        if (key == SHGUIinput.esc && inButton)
        {
            inButton = false;
            ((StringSettingsButton)buttons[currentButton]).StopRoutine();
        }
        else if (key == SHGUIinput.enter || key == SHGUIinput.space)
        {
            if (inButton)
            {
                inButton = false;
                ((StringSettingsButton)buttons[currentButton]).StopRoutine();
            }
            else if (currentButton >= 1 && currentButton <= 4)
            {
                inButton = true;
                ((StringSettingsButton)buttons[currentButton]).StartRoutine();
                return;
            }
            else if (currentButton == 5)
            {
                ((ConnectButton)buttons[currentButton]).Connect(hostname, port, slot, password);
                return;
            }
        }

        if (!inButton)
            base.ReactToInputKeyboard(key);
    }
}