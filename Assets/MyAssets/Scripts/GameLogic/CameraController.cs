using System;
using UnityEngine;

namespace MyAssets.Scripts.GameLogic
{
    public class CameraController: MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Player _player;
        [SerializeField] private float _thresholdRatio = 0.7f;

        private Transform PlayerTransform => _player.transform;
        
        private int _currentMeters = 0;
        
        public event Action OnPlayerOutOfCamera;
        public event Action OnMeterCrossed;

        private void LateUpdate()
        {
            if (PlayerTransform != null)
            {
                var cameraPosition = _camera.transform.position;
                var cameraHeight = _camera.orthographicSize * 2;
                var bottomCamera = cameraPosition.y - cameraHeight / 2;

                if (PlayerTransform.position.y > cameraPosition.y + _thresholdRatio * cameraHeight)
                {
                    cameraPosition.y = PlayerTransform.position.y - _thresholdRatio * cameraHeight;
                    if((int)cameraPosition.y > _currentMeters)
                    {
                        _currentMeters = (int)cameraPosition.y;
                        OnMeterCrossed?.Invoke();
                    }
                }
                if (PlayerTransform.position.y < bottomCamera)
                {
                    OnPlayerOutOfCamera?.Invoke();
                    return;
                }
                _camera.transform.position = cameraPosition;
            }
        }

        void OnDrawGizmos()
        {
            var cameraHeight = _camera.orthographicSize * 2;
            var thresholdHeight = _thresholdRatio * cameraHeight;
            Gizmos.color = Color.red;
            var lineY = transform.position.y + thresholdHeight;
            Gizmos.DrawLine(new Vector3(-1000, lineY, 0), new Vector3(1000, lineY, 0));
        }
    }
}