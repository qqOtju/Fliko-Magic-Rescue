using System.Collections;
using UnityEngine;

namespace MyAssets.Scripts.UI.GameScene
{
    public class TapToStart: MonoBehaviour
    {
        [SerializeField] private AnimationCurve _animationCurve;
        [SerializeField] private GameObject _textField;
        
        private const float AnimTime = 0.7f;
        
        private Vector2 _anchorMax, _anchorMin, _startMax, _startMin;
        private RectTransform _rectTransform;

        private void Start()
        {
            const float dif = 0.06f;
            _rectTransform = _textField.gameObject.GetComponent<RectTransform>();
            _anchorMax = _rectTransform.anchorMax;
            _anchorMin = _rectTransform.anchorMin;
            _startMax = new Vector2(_anchorMax.x - dif, _anchorMax.y - dif);
            _startMin = new Vector2(_anchorMin.x + dif, _anchorMin.y + dif);
            _rectTransform.anchorMax = _startMax;
            _rectTransform.anchorMin = _startMin;
            StartCoroutine(StartAnimation());
        }

        private IEnumerator StartAnimation()
        {
            _rectTransform.anchorMax = _startMax;
            _rectTransform.anchorMin = _startMin;
            var time = AnimTime;
            while (time > 0f)
            {
                time -= Time.deltaTime;
                var value = 1f - time / AnimTime;
                Debug.Log($"value: {value}| time: {time}");
                _rectTransform.anchorMax = new Vector2(
                    Mathf.Lerp(_startMax.x, _anchorMax.x, _animationCurve.Evaluate(value)),
                    Mathf.Lerp(_startMax.y, _anchorMax.y, _animationCurve.Evaluate(value)));
                _rectTransform.anchorMin = new Vector2(
                    Mathf.Lerp(_startMin.x, _anchorMin.x, _animationCurve.Evaluate(value)),
                    Mathf.Lerp(_startMin.y, _anchorMin.y, _animationCurve.Evaluate(value)));
                yield return null;
            }

            _rectTransform.anchorMax = _anchorMax;
            _rectTransform.anchorMin = _anchorMin;
            StartCoroutine(EndAnimation());
        }

        private IEnumerator EndAnimation()
        {
            _rectTransform.anchorMax = _anchorMax;
            _rectTransform.anchorMin = _anchorMin;
            var value = 0f;
            while (value < 1f)
            {
                value += Time.deltaTime / AnimTime;
                _rectTransform.anchorMax = new Vector2(
                    Mathf.Lerp(_anchorMax.x, _startMax.x, _animationCurve.Evaluate(value)),
                    Mathf.Lerp(_anchorMax.y, _startMax.y, _animationCurve.Evaluate(value)));
                _rectTransform.anchorMin = new Vector2(
                    Mathf.Lerp(_anchorMin.x, _startMin.x, _animationCurve.Evaluate(value)),
                    Mathf.Lerp(_anchorMin.y, _startMin.y, _animationCurve.Evaluate(value)));
                yield return null;
            }
            
            _rectTransform.anchorMax = _startMax;
            _rectTransform.anchorMin = _startMin;
            StartCoroutine(StartAnimation());
        }

        private void OnDestroy() =>
            StopAllCoroutines();
    }
    
}