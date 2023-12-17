using System.Collections.Generic;
using System.Threading.Tasks;
using MyAssets.Scripts.Infrastructure.Startup.Loading;
using MyAssets.Scripts.Infrastructure.Startup.Operations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyAssets.Scripts.Infrastructure.Startup
{
    public class AppStartup : MonoBehaviour
    {
        [Header("Menu Scene")] 
        [SerializeField] private string _menuScene;
        [Header("Loading Screen")]
        [SerializeField] private LoadingScreen _loadingScreen;
        
        private async void Awake()
        {
            var loadingOperations = new Queue<ILoadingOperation>();
            loadingOperations.Enqueue(new SceneLoadingOperation(_menuScene, LoadSceneMode.Additive));
            await Load(loadingOperations);
            Destroy(gameObject);
        }

        private async Task Load(Queue<ILoadingOperation> loadingOperations)
        {
            var loadingScreen = Instantiate(_loadingScreen);
            await loadingScreen.Load(loadingOperations);
            Destroy(loadingScreen.gameObject);
        }
    }
}