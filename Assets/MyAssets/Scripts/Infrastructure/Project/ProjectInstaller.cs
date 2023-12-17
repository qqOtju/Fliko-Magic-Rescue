using MyAssets.Scripts.Audio;
using MyAssets.Scripts.Data;
using MyAssets.Scripts.Data.SO;
using MyAssets.Scripts.MyInput;
using UnityEngine;
using Zenject;

namespace MyAssets.Scripts.Infrastructure.Project
{
    public class ProjectInstaller: MonoInstaller
    {
        [SerializeField] private GameDataSO _gameDataSO;
        [SerializeField] private AudioManager _audioManager;

        public override void InstallBindings()
        {
            BindDataOfGame();
            BindAudioManager();
            BindInputController();
        }

        private void BindDataOfGame()
        {
            var gameData = new DataOfGame(_gameDataSO.AbilitiesSO, _gameDataSO.PlayerSO);
            Container.Bind<DataOfGame>().FromInstance(gameData).AsSingle();
        }
        
        private void BindAudioManager() =>
            Container.Bind<AudioManager>().
                FromInstance(Instantiate(_audioManager)).AsSingle();
        
        private void BindInputController()
        {
            var handler = new GameObject("InputHandler").AddComponent<InputController>();
            Container.Bind<InputController>().FromInstance(handler).AsSingle();
        }
    }
}