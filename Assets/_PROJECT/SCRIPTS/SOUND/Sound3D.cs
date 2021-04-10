using UnityEngine;

public class Sound3D : MonoBehaviour
{
    public string Name;

    public AudioClip Clip;

    [Range(0.0f, 1.0f)]
    public float Volume = 0.5f;
    [Range(0.3f, 3.0f)]
    public float Pitch = 1;
    [Range(0.0f, 1.0f)]
    public float SpatialBlend = 1;

    public bool Loop;

    private AudioSource _source;

    private void Awake()
    {
        SetupSource();
    }

    public void SetupSource()
    {
        _source = gameObject.AddComponent<AudioSource>();

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

}
