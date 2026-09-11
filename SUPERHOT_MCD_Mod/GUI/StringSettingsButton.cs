using SUPERHOT_MCD_Mod;
using System;
using System.Collections;
using System.Text;
using UnityEngine;

public class StringSettingsButton : SHGUIcommanderbutton
{
    private readonly Action<string> onChange;
    private readonly bool isPassword;
    private readonly StringBuilder sb;
    private IEnumerator routine = null;

    public StringSettingsButton(string fieldName, ArchipelagoSettingsView parent, string initialText, Action<string> onChange, bool isPassword = false)
		: base(fieldName, 'w', null, null, true)
    {
        this.parent = parent;
        this.onChange = onChange;
        this.isPassword = isPassword;
        listLink = parent;
        sb = new(initialText);
        UpdateData();
    }

    public void StartRoutine()
    {
        UpdateData();
        if (routine == null)
        {
            routine = EditRoutine();
            Plugin.instance.StartCoroutine(routine);
        }
    }

    public override void Deactiavate()
    {
        StopRoutine();
    }

    public void StopRoutine()
    {
        if (routine != null)
        {
            Plugin.instance.StopCoroutine(routine);
            routine = null;
            onChange.Invoke(sb.ToString());
        }
    }

    private IEnumerator EditRoutine()
    {
        while (true)
        {
            if (Input.anyKeyDown)
            {
                // For some reason have to copy?
                string input = Input.inputString.ToString();
                foreach (char c in input)
                {
                    // Backspace
                    if (c == '\b')
                    {
                        // Remove last character
                        if (sb.Length > 0) 
                        {
                            sb.Length--;
                            UpdateData();
		                    SHGUI.current.PlaySound(SHGUIsound.tick);
                        }
                    }
                    else if (c >= 32 && c <= 126)
                    {
                        // ASCII is nice in that all human readable characters are continguos
                        // Hence between 32 and 126 is a typable char
                        sb.Append(c);
                        UpdateData();
		                SHGUI.current.PlaySound(SHGUIsound.tick);
                    }
                }
            }

            yield return null;
        }
    }

    private void UpdateData()
    {
        if (isPassword)
            data = new('*', sb.Length);
        else
            data = sb.ToString();
        RefreshRightPanel() ;
    }
}
