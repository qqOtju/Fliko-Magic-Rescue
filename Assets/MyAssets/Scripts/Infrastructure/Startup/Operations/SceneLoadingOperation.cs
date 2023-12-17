using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace MyAssets.Scripts.Infrastructure.Startup.Operations
{
    public class SceneLoadingOperation : ILoadingOperation
    {
        private readonly string _sceneToLoad;
        private readonly LoadSceneMode _mode;

        public event Action<string> OnDescriptionChange;
        public string Description => "Loading scene";

        public SceneLoadingOperation(string sceneName, LoadSceneMode mode)
        { 
            _sceneToLoad = sceneName;
            _mode = mode;
        }
        
        public async Task Load(Action<float> onProgress)
        {
            var op = SceneManager.LoadSceneAsync(_sceneToLoad, _mode);
            op.allowSceneActivation = false;
            var completionSource = new TaskCompletionSource<bool>();
            async Task LoadScene()
            {
                while (!op.isDone)
                {
                    onProgress?.Invoke(op.progress);
                    if (op.progress >= 0.9f)
                        op.allowSceneActivation = true;
                    await Task.Yield();
                }
                completionSource.SetResult(true);
            }
            _ = LoadScene();
            await completionSource.Task;
            onProgress?.Invoke(1f);
        }
    }
}