using MyAssets.Scripts.Audio;
using MyAssets.Scripts.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MyAssets.Scripts.UI.MainMenu
{
    public class HistoryUI: MonoBehaviour
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private TextMeshProUGUI[] _scores;
        [SerializeField] private GameObject _historyPanel;
        
        private AudioManager _audioManager;
        private DataOfGame _dataOfGame;
        
        [Inject]
        private void Construct(AudioManager audioManager, DataOfGame dataOfGame)
        {
            _audioManager = audioManager;
            _dataOfGame = dataOfGame;
            for (int i = 0; i < _dataOfGame.DataScore.LastFiveScores.Length; i++)
            {
                var score = _dataOfGame.DataScore.LastFiveScores[i];
                _scores[i].text = score.ToString();
            }
        }
        
        private void Awake()
        {
            _backButton.onClick.AddListener(OnBackButtonClicked);
        }

        private void OnBackButtonClicked()
        {
            _audioManager.PlayButtonClick();
            _historyPanel.SetActive(false);
        }
    }
}