using System.Collections.Generic;
using MyAssets.Scripts.Audio;
using MyAssets.Scripts.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MyAssets.Scripts.UI.MainMenu
{
    public class ShopUI: MonoBehaviour
    {
        [Header("Shop Items")]
        [SerializeField] private TextMeshProUGUI[] _abilitiesNames;
        [SerializeField] private Button[] _buyButtons;
        [SerializeField] private Image[] _abilityImages;
        [SerializeField] private TextMeshProUGUI[] _countTexts;
        [SerializeField] private TextMeshProUGUI[] _priceTexts;
        [Header("Panels")]
        [SerializeField] private GameObject _mainMenuPanel;
        [SerializeField] private GameObject _shopPanel;
        [Header("Other")]
        [SerializeField] private TextMeshProUGUI _goldText;
        [SerializeField] private Button _backButton;

        private readonly Dictionary<int, DataAbility> _abilitiesData = new();
        private AudioManager _audioManager;
        private DataOfGame _data;

        [Inject]
        private void Construct(DataOfGame data, AudioManager audioManager)
        {
            _data = data;
            _audioManager = audioManager;
            foreach (var abilityData in _data.Abilities)
            {
                var id = abilityData.Ability.ID;
                _abilitiesNames[id].text = abilityData.Ability.Name;
                _abilitiesData.Add(id, abilityData);
                _countTexts[id].text = abilityData.Count.ToString();
                _buyButtons[id].onClick.AddListener(() => OnAbilityButtonClicked(id));
                _abilityImages[id].sprite = abilityData.Ability.Icon;
                _priceTexts[id].text = abilityData.Ability.Price.ToString();
            }
            SetGoldCount();
        }
        
        private void Awake()
        {
            _backButton.onClick.AddListener(OnBackButtonClicked);
        }

        private void OnAbilityButtonClicked(int index)
        {
            _audioManager.PlayButtonClick();
            var ability = _abilitiesData[index];
            if(_data.DataGold.GoldCount < ability.Ability.Price) return;
            _data.DataGold.GoldCount -= ability.Ability.Price;
            ability.Count++;
            _countTexts[ability.Ability.ID].text = "x" + ability.Count;
            SetGoldCount();
        }

        private void OnBackButtonClicked()
        {
            _audioManager.PlayButtonClick();
            _mainMenuPanel.SetActive(true);
            _shopPanel.SetActive(false);
        }

        private void SetGoldCount() =>
            _goldText.text = _data.DataGold.GoldCount.ToString();
    }
}