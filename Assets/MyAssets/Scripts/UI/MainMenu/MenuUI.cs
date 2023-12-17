using MyAssets.Scripts.Audio;
using MyAssets.Scripts.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace MyAssets.Scripts.UI.MainMenu
{
    public class MenuUI: MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _optionsButton;
        [SerializeField] private Button _historyButton;
        [Header("Panels")]
        [SerializeField] private GameObject _menuPanel;
        [SerializeField] private GameObject _shopPanel;
        [SerializeField] private GameObject _optionsPanel;
        [SerializeField] private GameObject _historyPanel;
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _recordText;
        [SerializeField] private TextMeshProUGUI _goldCountText;
        [Header("Scene Name")]
        [SerializeField] private string _gameSceneName;

        private AudioManager _audioManager;
        private DataOfGame _data;
        
        [Inject]
        private void Construct(DataOfGame data, AudioManager audioManager)
        { 
            Debug.Log("MenuUI Construct");
            _audioManager = audioManager;
            _data = data;
            SetBestScore();
            SetGoldCount();
            _data.DataGold.OnGoldCountChanged += SetGoldCount;
        }
        
        private void Awake()
        {
            _playButton.onClick.AddListener(OnStartButtonClicked);
            _shopButton.onClick.AddListener(OnShopButtonClicked);
            _optionsButton.onClick.AddListener(OnOptionsButtonClicked);
            _historyButton.onClick.AddListener(OnHistoryButtonClicked);
        }
        
        private void OnDestroy()
        {
            _playButton.onClick.RemoveListener(OnStartButtonClicked);
            _shopButton.onClick.RemoveListener(OnShopButtonClicked);
            _optionsButton.onClick.RemoveListener(OnOptionsButtonClicked);
            _historyButton.onClick.RemoveListener(OnHistoryButtonClicked);
            _data.DataGold.OnGoldCountChanged -= SetGoldCount;
        }

        private void OnStartButtonClicked()
        { 
            _audioManager.PlayButtonClick();
            SceneManager.LoadScene(_gameSceneName, LoadSceneMode.Single);
        }

        private void OnOptionsButtonClicked()
        {
            _audioManager.PlayButtonClick();
            _optionsPanel.SetActive(true);
        }

        private void OnShopButtonClicked()
        {
            _audioManager.PlayButtonClick();
            _menuPanel.SetActive(false);
            _shopPanel.SetActive(true);
        }

        private void OnHistoryButtonClicked()
        {
            _audioManager.PlayButtonClick();
            _historyPanel.SetActive(true);
        }
        
        private void SetGoldCount() =>
            _goldCountText.text = _data.DataGold.GoldCount.ToString();
        
        private void SetBestScore() =>
            _recordText.text = _data.DataScore.LastFiveScores[^1].ToString();
    }
}