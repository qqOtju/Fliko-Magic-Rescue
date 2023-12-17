using System.Collections;
using MyAssets.Scripts.Data;
using MyAssets.Scripts.UI.GameScene;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MyAssets.Scripts.GameLogic
{
    public class GameController: MonoBehaviour
    {
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private ResultsUI _resultsUI;
        [SerializeField] private ChunkSpawner _chunkSpawner;
        [SerializeField] private GameObject _tapToStartPanel;
        [SerializeField] private Button _tapToStartButton;
        [SerializeField] private Player _player;

        private DataAbility _doubleScoreDataAbility;
        private DataOfGame _dataOfGame;
        private int _gainedGold = 1;
        
        
        [Inject]
        private void Construct(DataOfGame dataOfGame)
        {
            _dataOfGame = dataOfGame;
            foreach (var ability in _dataOfGame.Abilities)
            {
                ability.OnAbilityUse += OnAbilityUse;
                if(ability.Ability.ID == 0)
                    _doubleScoreDataAbility = ability;
            }
            _dataOfGame.DataPlayer.ResetHealth();
            _dataOfGame.DataScore.CurrentScore = 0;
            _dataOfGame.DataPlayer.OnPlayerDeath += OnPlayerDeath;
        }
        private void Awake()
        {
            _cameraController.OnPlayerOutOfCamera += OnPlayerOutOfCamera;
            _cameraController.OnMeterCrossed += OnMeterCrossed;
            _tapToStartButton.onClick.AddListener(StartGame);
        }

        private void Start()
        {
            _player.gameObject.SetActive(false);
            _resultsUI.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _cameraController.OnPlayerOutOfCamera -= OnPlayerOutOfCamera;
            _dataOfGame.DataPlayer.OnPlayerDeath -= OnPlayerDeath;
            foreach (var ability in _dataOfGame.Abilities)
                ability.OnAbilityUse -= OnAbilityUse;
        }

        private void StartGame()
        {
            _player.gameObject.SetActive(true);
            Time.timeScale = 1;
            _tapToStartPanel.gameObject.SetActive(false);
            _chunkSpawner.StartSpawn();
        }

        private void OnPlayerOutOfCamera()
        {
            Time.timeScale = 0;
            _dataOfGame.DataScore.UpdateHighScore(_dataOfGame.DataScore.CurrentScore);
            _resultsUI.gameObject.SetActive(true);
        }

        private void OnPlayerDeath()
        {
            Time.timeScale = 0;
            _dataOfGame.DataScore.UpdateHighScore(_dataOfGame.DataScore.CurrentScore);
            _resultsUI.gameObject.SetActive(true);
        }

        private void OnMeterCrossed()
        {
            if(_doubleScoreDataAbility.Activated)
                _dataOfGame.DataScore.CurrentScore++;
            _dataOfGame.DataScore.CurrentScore++;
            if (_dataOfGame.DataScore.CurrentScore % (10 * _gainedGold) <= 0) return;
            _gainedGold++;
            _dataOfGame.DataGold.GoldCount++;
        }

        private void OnAbilityUse(int id)
        {
            foreach (var ability in _dataOfGame.Abilities)
            {
                if (ability.Ability.ID != id) continue;
                StartCoroutine(AbilityDuration(ability));
                break;
            }
        }

        private IEnumerator AbilityDuration(DataAbility dataAbility)
        {
            yield return new WaitForSeconds(dataAbility.Ability.Duration);
            dataAbility.Activated = false;
        }
    }
}