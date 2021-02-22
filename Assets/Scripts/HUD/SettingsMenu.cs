using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    //VARIABLEs
    public Toggle fullscreenToggle;
    public Toggle vSyncToggle;

    public ResolutionType[] resolutions;
    public int selectedRes;

    public TextMeshProUGUI resText;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.width & resolutions[i].height == Screen.height)
            {
                selectedRes = i;
                resText.text = resolutions[selectedRes].width + " x " + resolutions[selectedRes].height;
            }
        }

        fullscreenToggle.isOn = Screen.fullScreen;

        if (QualitySettings.vSyncCount == 0)
        {
            vSyncToggle.isOn = false;
        }
        else
        {
            vSyncToggle.isOn = true;
        }
    }

    public void ApplyFullScreen()
    {
        if (fullscreenToggle.isOn)
        {
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreen = false;
        }
    }

    public void ApplyVSync()
    {
        if (vSyncToggle.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
    }

    public void ResolutionLeft()
    {
        if (selectedRes > 0)
        {
            selectedRes--;
        }
        resText.text = resolutions[selectedRes].width + " x " + resolutions[selectedRes].height;

        //SetResolution();
    }

    public void ResolutionRight()
    {
        if (selectedRes < resolutions.Length - 1)
        {
            selectedRes++;
        }
        resText.text = resolutions[selectedRes].width + " x " + resolutions[selectedRes].height;

        //SetResolution();
    }

    public void SetResolution()
    {
        Screen.SetResolution(resolutions[selectedRes].width, resolutions[selectedRes].height, fullscreenToggle.isOn);
    }

    public void ApplyChanges()
    {
        SetResolution();
        ApplyFullScreen();
        ApplyVSync();
    }
}