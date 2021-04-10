using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public List<Sound> Sounds;


    private void Awake()
    {
        #region Singletone
        if (Instance != null)
        {
            Destroy(gameObject);

            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        #endregion

        foreach (Sound sound in Sounds)
        {
            sound.SetupSource(gameObject);
        }
    }

    public void PlaySound(string name)
    {
        Sound sound = Sounds.Find(s => s.Name == name);

        if (sound == null)
            throw new NullReferenceException("The sound you are trying to play does not exist. (Incorrect name?)");


        sound.Play();
    }


    public void StopSound(string name)
    {
        Sound sound = Sounds.Find(s => s.Name == name);

        if (sound == null)
            throw new NullReferenceException("The sound you are trying to play does not exist. (Incorrect name?)");


        sound.Stop();

    }

    public void PlayRandomThrow()
    {
        int random = UnityEngine.Random.Range(0, 3);
        if (random == 0)
            Instance.PlaySound("Throw1");
        else if(random == 1)
            Instance.PlaySound("Throw2");
        else if(random == 2)
            Instance.PlaySound("Throw3");
    }
}
