using System.Collections.Generic;
using System.Threading.Tasks;
using MyAssets.Scripts.Infrastructure.Startup.Operations;

namespace MyAssets.Scripts.Infrastructure.Startup.Loading
{
    public interface ILoadingScreen
    {
        public Task Load(Queue<ILoadingOperation> loadingOperations, bool withProgressBar = true);
    }
}