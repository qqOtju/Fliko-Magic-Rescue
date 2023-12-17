using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyAssets.Scripts.MyInput
{
    public class InputController: MonoBehaviour
    {
        private const KeyCode ScreenshotKey = KeyCode.S;
        
        private Controls _controls;
        private int _count;

        public event Action<float> OnInput;
        public event Action OnInputCanceled; 

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _count = PlayerPrefs.GetInt("ScreenshotsCount");
            _controls = new Controls();
            _controls.Player.Input.started += InputOnStarted;
            _controls.Player.Input.canceled += InputOnCanceled;
        }

        private void InputOnCanceled(InputAction.CallbackContext obj) =>
            OnInputCanceled?.Invoke();

        private void InputOnStarted(InputAction.CallbackContext obj) =>
            OnInput?.Invoke(obj.ReadValue<float>());

        void Update()
        {
            if (!Input.GetKeyDown(ScreenshotKey)) return;
            _count++;
            ScreenCapture.CaptureScreenshot($"screenshot{_count}.png");
            PlayerPrefs.SetInt("ScreenshotsCount", _count);
            Debug.Log("A screenshot was taken!");
        }
        
        private void OnEnable() => _controls.Enable();
        
        private void OnDisable() => _controls.Disable();
    }
}