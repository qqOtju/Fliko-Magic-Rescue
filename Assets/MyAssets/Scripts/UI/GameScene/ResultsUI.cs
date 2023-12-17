using MyAssets.Scripts.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace MyAssets.Scripts.UI.GameScene
{
    [RequireComponent(typeof(Canvas))]
    public class ResultsUI: MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _mainMenuButton;
        [Header("Scene Names")]
        [SerializeField] private string _gameSceneName;
        [SerializeField] private string _mainMenuSceneName;
        [Header("Score")]
        [SerializeField] private TextMeshProUGUI _scoreText;

        private DataOfGame _dataOfGame;
        
        [Inject]
        private void Construct(DataOfGame dataOfGame) =>
            _dataOfGame = dataOfGame;   
        
        private void Awake()
        {
            _restartButton.onClick.AddListener(OnRestartClicked);
            _mainMenuButton.onClick.AddListener(OnMainMenuClicked);
        }

        private void OnEnable()
        {
            _scoreText.text = _dataOfGame.DataScore.ToString();
        }

        private void OnMainMenuClicked() =>
            SceneManager.LoadScene(_mainMenuSceneName, LoadSceneMode.Single);

        private void OnRestartClicked() =>
            SceneManager.LoadScene(_gameSceneName, LoadSceneMode.Single);
    }
}