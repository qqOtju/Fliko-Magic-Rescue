using MyAssets.Scripts.GameLogic;
using MyAssets.Scripts.MyInput;
using UnityEngine;

namespace MyAssets.Scripts.Data.SO
{
    [CreateAssetMenu(fileName = "Chunks", menuName = "MyAssets/Chunks")]
    public class ChunksSO: ScriptableObject
    {
        [SerializeField] private Chunk[] _chunks;
        [SerializeField] private Chunk _startChunk;
        
        public Chunk[] Chunks => _chunks;
        public Chunk StartChunk => _startChunk;
    }
}