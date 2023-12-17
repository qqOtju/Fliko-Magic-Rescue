using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyAssets.Scripts.Infrastructure.Startup.Operations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyAssets.Scripts.Infrastructure.Startup.Loading
{
    [RequireComponent(typeof(Canvas), 
        typeof(CanvasGroup))]
    public class LoadingScreen : MonoBehaviour, ILoadingScreen
    {
        [Header("Progress Fill")]
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _progressText;
        [Header("Values")] 
        [SerializeField] private float _barSpeed = 1f;
        
        private CanvasGroup _canvasGroup;
        private float _targetProgress;
        private bool _isProgress;
        private Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public async Task Load(Queue<ILoadingOperation> loadingOperations, bool withProgressBar = true)
        {
            _canvas.enabled = true;
            if(withProgressBar)
            {
                foreach (var operation in loadingOperations)
                    await LoadWithProgressBar(operation);
                await CloseAnimation();
            }
            else
                foreach (var operation in loadingOperations)
                    await LoadWithoutProgressBar(operation);
        }

        private async Task LoadWithProgressBar(ILoadingOperation operation)
        {
            ResetFill();
            var cor = StartCoroutine(UpdateProgressBar());
            await operation.Load(OnProgress);
            await WaitForBarFill();
            StopCoroutine(cor);
        }
        
        private async Task LoadWithoutProgressBar(ILoadingOperation operation)
        {
            ResetFill();
            await operation.Load(OnProgress);
        }
        
        private void ResetFill()
        {
            _slider.value = 0;
            _targetProgress = 0;
        }

        private void OnProgress(float progress) => 
            _targetProgress = progress;

        private async Task WaitForBarFill()
        {
            while (_slider.value < _targetProgress)
                await Task.Delay(1);
        }

        private IEnumerator UpdateProgressBar()
        {
            while (_canvas.enabled)
            {
                if(_slider.value < _targetProgress)
                {
                    _slider.value += Time.deltaTime * _barSpeed;
                    _progressText.text = $"{Mathf.RoundToInt(_slider.value * 100)}%";
                }
                yield return null;
            }
        }

        private async Task CloseAnimation()
        {
            var value = 0.5f;
            while (value >= 0f)
            {
                _canvasGroup.alpha = Mathf.Lerp(0, 1, value);
                value -= Time.deltaTime;
                await Task.Delay(1);
            }
            _canvasGroup.alpha = 0;
            _canvas.enabled = false;
        }
    }
}