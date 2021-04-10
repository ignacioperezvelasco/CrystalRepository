using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public enum MusicType { MENU, INGAME }
    public MusicType SelectedMusicType;
    private void Start()
    {
        if (SelectedMusicType == MusicType.MENU)
        {
            AudioManager.Instance.PlaySound("MainMenu");
            AudioManager.Instance.StopSound("Ambience");
        }
        else if (SelectedMusicType == MusicType.INGAME)
        {
            //AudioManager.Instance.StopSound("MainMenu");
            AudioManager.Instance.PlaySound("Ambience");
        }
    }
}
