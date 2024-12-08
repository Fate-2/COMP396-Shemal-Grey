using UnityEngine;


    [RequireComponent(typeof(AudioSource))]
    public class GuardSounds : MonoBehaviour
    {
        [SerializeField] private AudioSource _AudioSource;
        [SerializeField] private AudioClip _AudioClip;
        [SerializeField] private AudioSourceSettings _AudioSettings;

    public void PlayStep()
        {
            _AudioSource.PlayOneShot(_AudioClip);
        }
    public void GetAudioSourceSettings(AudioSourceSettings audioSettings)
    {
        _AudioSettings = audioSettings;
    }
    private void Awake()
    {
        _AudioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        _AudioSource.playOnAwake = _AudioSettings.AudioSourcePlayOnAwake;
        _AudioSource.spatialBlend = _AudioSettings.AudioSourceSpatialBlend;
        _AudioSource.minDistance = _AudioSettings.AudioSourceMinDistance;
        _AudioSource.maxDistance = _AudioSettings.AudioSourceMaxDistance;
        _AudioSource.rolloffMode = _AudioSettings.AudioRolloffMode;
        _AudioClip = _AudioSettings.audioClip;
    }
}