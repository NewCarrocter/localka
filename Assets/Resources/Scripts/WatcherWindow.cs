using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WMIForUnity;

public class WatcherWindow : GetRadio
{
    // Start is called before the first frame update
    public GameObject TextWindow;
    public void OnClick()
    {
        int count = 0;
        foreach (Transform btn in box)
        {
            if (btn.GetComponent<Toggle>().isOn == true)
            {
                ListenerProgramm.ip = btn.GetComponentInChildren<Text>().text.Split(':')[0];
                break;
            }
            count++;
        }
        if (count == box.childCount)
            throw new Exception("Не выбраны клиенты!");
        else
        {
            ListenerProgramm.StartListenningPorcessChange();
            TextWindow.SetActive(true);
        }
    }
    private void OnApplicationQuit()
    {
        try
        {
            ListenerProgramm.StopListenningPorcessChange();
        }
        catch { }
    }
}