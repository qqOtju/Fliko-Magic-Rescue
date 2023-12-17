using System.Collections;
using System.Collections.Generic;
using LeanTween.Framework;
using MyAssets.Scripts.Data;
using TMPro;
using Tools.MyGridLayout;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace MyAssets.Scripts.UI.GameScene
{
    public class GameUI: MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private HealthLayout _horizontalLayout;
        [Header("Health Settings")]
        [SerializeField] private Image _healthSprite;
        [SerializeField] private Color _healthColor;
        [SerializeField] private Color _healthLostColor;
        [Header("Buttons")]
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _pauseButton;
        [Header("Abilities")] 
        [SerializeField] private TextMeshProUGUI[] _abilitiesNames;
        [SerializeField] private TextMeshProUGUI[] _abilitiesCountText;
        [SerializeField] private Button[] _abilitiesButtons;
        [SerializeField] private Image[] _abilitiesImages;
        [SerializeField] private Image[] _abilitiesFillImages;
        [SerializeField] private Image[] _abilitiesIcons;
        [Header("Other")]
        [SerializeField] private string _mainMenuSceneName;
        [SerializeField] private TextMeshProUGUI _counterText;

        private readonly List<Image> _healthImages = new();
        
        private DataOfGame _dataOfGame;
        private bool _paused;
        
        private int MaxHealth => _dataOfGame.DataPlayer.PlayerSO.MaxHealth;

        [Inject]
        private void Construct(DataOfGame dataOfGame)
        {
            _dataOfGame = dataOfGame;
            _dataOfGame.DataScore.OnScoreUpdated += OnScoreChanged;
            _dataOfGame.DataPlayer.OnHealthUpdated += OnPlayerHit;
            if(_abilitiesButtons.Length != _dataOfGame.Abilities.Length)
                throw new System.Exception("Abilities buttons count must be equal to abilities count");
            foreach (var ability in _dataOfGame.Abilities)
            {
                var id = ability.Ability.ID;
                _abilitiesNames[id].text = ability.Ability.Name;
                _abilitiesCountText[id].text = "x" + ability.Count;
                _abilitiesButtons[id].interactable = ability.Count > 0;
                _abilitiesImages[id].sprite = ability.Ability.Icon;
                _abilitiesFillImages[id].sprite = ability.Ability.Icon;
                _abilitiesIcons[id].sprite = ability.Ability.Icon;
                _abilitiesButtons[id].onClick.AddListener(() => OnAbilityUse(ability));
            }
            for (var i = 0; i < MaxHealth; i++)
            {
                var image = Instantiate(_healthSprite, _horizontalLayout.transform);
                image.color = _healthColor;
                _healthImages.Add(image);
            }
            _horizontalLayout.Align(MaxHealth);
        }
        

        private void Awake()
        {
            _homeButton.onClick.AddListener(OnHomeButtonClicked);
            _pauseButton.onClick.AddListener(OnPauseButtonClicked);
            _counterText.text = "0";
        }

        private void OnDestroy()
        {
            _dataOfGame.DataScore.OnScoreUpdated -= OnScoreChanged;
            _dataOfGame.DataPlayer.OnHealthUpdated -= OnPlayerHit;
            _homeButton.onClick.RemoveListener(OnHomeButtonClicked);
            _pauseButton.onClick.RemoveListener(OnPauseButtonClicked);
        }
        
        private void OnScoreChanged(int score)
        {
            _counterText.text = score.ToString();
            LeanTween.Framework.LeanTween.scale(_counterText.gameObject, Vector3.one * 1.2f, 0.1f)
                .setEase(LeanTweenType.easeOutBack)
                .setOnComplete(() =>
                {
                    LeanTween.Framework.LeanTween.scale(_counterText.gameObject, Vector3.one, 0.1f)
                        .setEase(LeanTweenType.easeInBack);
                });
        }

        private void OnPauseButtonClicked()
        {
            _paused = !_paused;
            Time.timeScale = _paused ? 0 : 1;
        }

        private void OnHomeButtonClicked() =>
            SceneManager.LoadScene(_mainMenuSceneName, LoadSceneMode.Single);
        
        private void OnPlayerHit(int value)
        {
            if(value < 0) return;
            for (int i = 0; i < value; i++)
            {
                _healthImages[i].color = _healthColor;
            }
            for (int i = value; i < MaxHealth; i++)
            {
                _healthImages[i].color = _healthLostColor;
            }
        }

        private void OnAbilityUse(DataAbility dataAbility)
        {
            if(dataAbility.Count <= 0) return;
            var id = dataAbility.Ability.ID;
            StartCoroutine(AbilityCooldown(_abilitiesFillImages[id], 
                _abilitiesButtons[id], dataAbility.Ability.Cooldown, dataAbility));
            _abilitiesCountText[id].text = "x" + --dataAbility.Count;
            dataAbility.Activated = true;
        }

        private IEnumerator AbilityCooldown(Image fill,Button button, float duration, DataAbility dataAbility)
        {
            var time = 0f;
            button.interactable = false;
            while (time < duration)
            {
                fill.fillAmount = time / duration;
                time += Time.deltaTime;
                yield return null;
            }
            button.interactable = dataAbility.Count > 0;
        }
    }
}