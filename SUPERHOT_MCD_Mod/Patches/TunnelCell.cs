using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using MCDView;
using SUPERHOT_MCD_Mod;
using Utilities.CameraEffects;

// This patch is responsible for the popup that occurs
// When you access a tunnel for the first time
// Or don't have the neccesary unlock to use it
[HarmonyPatch]
public static class Inject_TunnelCellConfirmation
{
    private static string[] layers = {
        "[@---]",
        "[-@--]",
        "[--@-]",
        "[---@]",
        "[▀▄▒▪]"
    };

    [HarmonyPatch(typeof(TunnelCell), nameof(TunnelCell.ActivateUnlocked))]
    [HarmonyPrefix]
    public static bool Prefix_ActivateUnlocked(PyramidView ___pyramidView, PyramidDataContainer ___pyramid)
    {
        if (!ArchipelagoManager.Connected)
            return true;


        PyramidDataContainer pyramid = Traverse.Create(___pyramidView).Field("pyramid").GetValue<PyramidDataContainer>();
        int currentLayer = Array.IndexOf(layers, pyramid.Description);
        int nextLayer = Array.IndexOf(layers, ___pyramid.Description);
        if (currentLayer > nextLayer)
            return true;

        if (SHRLSaveManager.Instance.GetValueAs($"TUNNEL_USABLE{currentLayer}", false))
            return true; 

        ConfirmPopup confirmPopup = new ConfirmPopup(new Action(() => PopUpAction(nextLayer)), "DO YOU HAVE THE NECESSARY PRIVILEGE TO\nUNDERTAKE THIS ACTION?", 
                                                                "YES", "NO", 3, 2, null);
        
        SHGUI.current.AddViewOnTop(confirmPopup);
        // ___pyramidView.AddSubView(confirmPopup);
        return false;
    }

    private static void PopUpAction(int layer)
    {
        if (ArchipelagoDataManager.FloorPrivilege >= layer)
        {   
            // Set tunnel as being usable
            SHRLSaveManager.Instance.SetValue($"TUNNEL_USABLE{layer - 1}", true);

            // Create info popup
            string confirm = "AUTHENTICATED, YOU MAY PROCEED";
            int x = SHGUI.current.resolutionX / 2 - confirm.Length / 2 - 1;;
            int y =  SHGUI.current.resolutionY / 2 - 1;
            Plugin.Logger.LogInfo("here"); 
            Type internalType = AccessTools.TypeByName("MCDView.InfoFadePopup");
            Plugin.Logger.LogInfo("here2"); 
            object[] parameters = [confirm, x, y, 'w', 'w', 1.5f, false];
            Plugin.Logger.LogInfo("here3"); 
            object infoFadePopup = Activator.CreateInstance(internalType, parameters);
            Plugin.Logger.LogInfo("here4"); 
            SHGUI.current.AddViewOnTop((SHGUIview)infoFadePopup);
            Plugin.Logger.LogInfo("here5"); 
        }   
        else
        {
            // All of this traverse stuff is just to get a camera effect,,,           
            Type cameraEffectsManager = AccessTools.TypeByName("Utilities.CameraEffects.CameraEffectsManager");
            object instance = Traverse.Create(cameraEffectsManager).Property("Instance").GetValue();
            Dictionary<string, CameraEffectsInterpolator> dict = 
                Traverse.Create(instance).Field("interpolators").GetValue<Dictionary<string, CameraEffectsInterpolator>>();
            SHGUI.current.PlaySound(SHGUIsound.wrong);
            dict["ErrorPunch"].Play();

            // Create info popup
            string denyMessage = "YOUR PRIVILEGE IS NOT HIGH ENOUGH";
            int x = SHGUI.current.resolutionX / 2 - 28 / 2 - 1;;
            int y = SHGUI.current.resolutionY / 2 - 1;
            Type internalType = AccessTools.TypeByName("MCDView.InfoFadePopup");
            object[] parameters = [denyMessage, x, y, 'r', 'r', 1.5f, false];
            object infoFadePopup = Activator.CreateInstance(internalType, parameters);

            SHGUI.current.AddViewOnTop((SHGUIview)infoFadePopup);
        }     
    }
}
