using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public enum MusicType { MENU, COMBAT }
    public MusicType SelectedMusicType;
    private void Start()
    {
        if(SelectedMusicType == MusicType.MENU)
        {
            AudioManager.Instance.PlaySound("Menu Music");
            AudioManager.Instance.StopSound("Combat Music");
        }
        else if(SelectedMusicType == MusicType.COMBAT)
        {
            AudioManager.Instance.StopSound("Menu Music");
            AudioManager.Instance.PlaySound("Combat Music");
        }
    }
}
