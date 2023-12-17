using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyAssets.Scripts.UI.GameScene
{
    public class TutorialUI: MonoBehaviour
    {
        [SerializeField] private TutorialData[] _tutorialData;
        [SerializeField] private GameObject _tutorialPanel;
        [SerializeField] private Button _nextButton;
        [SerializeField] private TextMeshProUGUI _guideText;
        [SerializeField] private Image _guideImage;

        private int _index = 0;
        
        private void Awake()
        {
            var firstTime = PlayerPrefs.GetInt("FirstTime", 0);
            if(firstTime == 0)
            {
                PlayerPrefs.SetInt("FirstTime", 1);
                _tutorialPanel.gameObject.SetActive(true);
                Set();
            }
            else
            {
                Destroy(_tutorialPanel);
            }
            _nextButton.onClick.AddListener(OnNextButtonClicked);
        }

        private void OnNextButtonClicked()
        {
            if(_index >= _tutorialData.Length - 1)
            {
                _tutorialPanel.gameObject.SetActive(false);
                PlayerPrefs.SetInt("FirstTime", 1);
                Destroy(_tutorialPanel);
            }
            else
                Set();
        }

        private void Set()
        {
            
            _guideText.text = _tutorialData[_index]._text;
            var image = _tutorialData[_index]._image;
            if(image == null)
                _guideImage.gameObject.SetActive(false);
            else
            {
                _guideImage.gameObject.SetActive(true);
                _guideImage.sprite = image;
            }
            _index++;
        }
    }
    
    [Serializable]
    public struct TutorialData
    {
        [SerializeField] public string _text;
        [SerializeField] public Sprite _image;

        public string Text => _text;
        public Sprite Image => _image;
    }
}