using MyAssets.Scripts.Audio;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MyAssets.Scripts.UI.MainMenu
{
    public class OptionsUI: MonoBehaviour
    {
        [Header("Sliders")]
        [SerializeField] private Slider _soundSlider;
        [SerializeField] private Slider _musicSlider;
        [Header("Other")]
        [SerializeField] private GameObject _optionsPanel;
        [SerializeField] private Button _backButton;
        
        private const string MusicVolumeKey = "MusicVolume";
        private const string SoundVolumeKey = "SoundVolume";
        
        private AudioManager _audioManager;
        
        [Inject]
        private void Construct(AudioManager audioManager)
        {
            Debug.Log("OptionsUI Construct");
            _audioManager = audioManager;
            var musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1);
            var effectsVolume = PlayerPrefs.GetFloat(SoundVolumeKey, 1);
            SetMusic(musicVolume);
            SetSound(effectsVolume);
        }
        
        private void Awake()
        {
            _backButton.onClick.AddListener(OnBackButtonClicked);
            _musicSlider.onValueChanged.AddListener(OnMusicVolumeChange);
            _soundSlider.onValueChanged.AddListener(OnSoundVolumeChange);
        }

        private void OnBackButtonClicked()
        {
            _audioManager.PlayButtonClick();
            _optionsPanel.SetActive(false);
        }
        
        private void OnMusicVolumeChange(float value) =>
            SetMusic(value);

        private void OnSoundVolumeChange(float value) =>
            SetSound(value);

        private void SetMusic(float value)
        {
            _audioManager.SetMusicVolume(value);
            _musicSlider.value = value;
            PlayerPrefs.SetFloat(MusicVolumeKey, value);
        }

        private void SetSound(float value)
        {
            _audioManager.SetSoundVolume(value);
            _soundSlider.value = value;
            PlayerPrefs.SetFloat(SoundVolumeKey, value);
        }
    }
}