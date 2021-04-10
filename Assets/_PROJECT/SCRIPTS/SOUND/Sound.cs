using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{

    public string Name;

    public AudioClip Clip;

    [Range(0.0f, 1.0f)]
    public float Volume = 1;
    [Range(0.3f, 3.0f)]
    public float Pitch = 1;
    [Range(0.0f, 1.0f)]
    public float SpatialBlend = 0;

    public bool Loop = false;

    private AudioSource _source;


    public void SetupSource(GameObject go)
    {
        _source = go.AddComponent<AudioSource>();

        _source.clip = Clip;
        _source.volume = Volume;
        _source.pitch = Pitch;
        _source.loop = Loop;
        _source.spatialBlend = SpatialBlend;
    }

    public void Play()
    {
        _source.Play();
    }
    public void Stop()
    {
        _source.Stop();
    }
}
