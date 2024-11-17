using UnityEngine;


    [RequireComponent(typeof(AudioSource))]
    public class GuardSounds : MonoBehaviour
    {
        [SerializeField] private AudioSource _AudioSource;
        [SerializeField] private AudioClip _AudioClip;
        public void PlayStep()
        {
            _AudioSource.PlayOneShot(_AudioClip);
        }
    }