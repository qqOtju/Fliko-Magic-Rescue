using UnityEngine;

namespace MyAssets.Scripts.Audio
{
    public class AudioManager: MonoBehaviour
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource _audioSourceMusic;
        [SerializeField] private AudioSource _audioSourceSound;
        [Header("Audio Clips")]
        [SerializeField] private AudioClip _buttonClickClip;
        [SerializeField] private AudioClip _musicClip;

        private void Awake() =>
            DontDestroyOnLoad(gameObject);

        private void Start()
        {
            _audioSourceMusic.clip = _musicClip;
            _audioSourceMusic.loop = true;
            _audioSourceMusic.Play();
        }

        public void SetMusicVolume(float value) =>
            _audioSourceMusic.volume = value;

        public void SetSoundVolume(float value) =>
            _audioSourceSound.volume = value;

        public void PlayButtonClick() =>
            _audioSourceSound.PlayOneShot(_buttonClickClip);

        public void PlaySound(AudioClip clip) =>
            _audioSourceSound.PlayOneShot(clip);
        
        public void PlayMusic(AudioClip music)
        { 
            _audioSourceMusic.Stop();
            _audioSourceMusic.clip = music;
            _audioSourceMusic.loop = true;
            _audioSourceMusic.Play();
        }
    }
}