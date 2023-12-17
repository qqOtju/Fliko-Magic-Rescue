using System.Collections.Generic;
using MyAssets.Scripts.Data;
using MyAssets.Scripts.Data.SO;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MyAssets.Scripts.GameLogic
{
    public class ChunkSpawner: MonoBehaviour
    {  
        [SerializeField] private ChunksSO _chunksSO;
        [SerializeField] private Transform _container;
        [SerializeField] private Button _switchButton;

        private const int ChunkCount = 2;
        private const int StartChunkCount = 5;
        
        private readonly List<Chunk> _chunks = new ();
        private readonly List<Chunk> _passedChunks = new ();
        
        private StateType _currentStateType = StateType.Plus;
        private DataAbility _flyDataAbility;
        private float _currentYPos;
        private DataOfGame _dataOfGame;

        [Inject]
        private void Construct(DataOfGame dataOfGame)
        {
            _dataOfGame = dataOfGame;
            foreach (var ability in _dataOfGame.Abilities)
            {
                if (ability.Ability.ID != 2) continue;
                _flyDataAbility = ability;
                break;
            }
            _flyDataAbility.OnAbilityUse += OnFlyDataAbilityUse;
            _flyDataAbility.OnAbilityEnd += OnFlyDataAbilityUse;
        }

        private void Awake()
        {
            _switchButton.onClick.AddListener(Switch);
        }

        public void StartSpawn()
        {
            SpawnStartChunk();
            for (var i = 0; i < StartChunkCount; i++)
                SpawnChunk();
            Switch(_currentStateType);
        }

        private void OnDestroy()
        {
            _switchButton.onClick.RemoveListener(Switch);
            _flyDataAbility.OnAbilityUse -= OnFlyDataAbilityUse;
            _flyDataAbility.OnAbilityEnd -= OnFlyDataAbilityUse;
            foreach (var chunk in  _chunks)
                chunk.OnChunkPassed -= OnChunkPassed;                
            foreach (var chunk in  _passedChunks)
                chunk.OnChunkPassed -= OnChunkPassed;                
        }
        
        private void SpawnStartChunk()
        {
            var chunk = Instantiate(_chunksSO.StartChunk, _container);
            var chunkTransform = chunk.transform;
            chunkTransform.position = Vector3.zero;
            _currentYPos = chunkTransform.position.y;
            _chunks.Add(chunk);
            chunk.OnChunkPassed += OnChunkPassed;
            chunk.Init(_dataOfGame);
            chunk.Switch(_currentStateType);
        }

        private void OnChunkPassed(Chunk obj)
        {
            obj.OnChunkPassed -= OnChunkPassed;
            _passedChunks.Add(obj);
            _chunks.Remove(obj);
            if (_passedChunks.Count <= ChunkCount) return;
            Destroy(_passedChunks[0].gameObject);
            _passedChunks.RemoveAt(0);
            SpawnChunk();
        }

        private void SpawnChunk()
        {
            var chunk = Instantiate(_chunksSO.Chunks[Random.Range(0, _chunksSO.Chunks.Length)], _container);
            var chunkTransform = chunk.transform;
            chunkTransform.position = _currentYPos * Vector3.up + Vector3.up * chunk.Height;
            _currentYPos = chunkTransform.position.y;
            _chunks.Add(chunk);
            chunk.Switch(_currentStateType);
            chunk.OnChunkPassed += OnChunkPassed;
            chunk.Init(_dataOfGame);
        }

        private void Switch()
        {
            if(_currentStateType == StateType.Plus)
                _currentStateType = StateType.Minus;
            else
                _currentStateType = StateType.Plus;
            foreach (var chunk in _chunks)
                chunk.Switch(_currentStateType);
            foreach (var chunk in _passedChunks)
                chunk.Switch(_currentStateType);
        }
        
        private void Switch(StateType state)
        {
            _currentStateType = state;
            foreach (var chunk in _chunks)
                chunk.Switch(_currentStateType);
            foreach (var chunk in _passedChunks)
                chunk.Switch(_currentStateType);
        }

        private void OnFlyDataAbilityUse(int obj)
        {
            _switchButton.interactable = !_flyDataAbility.Activated;
            if (_flyDataAbility.Activated)
            {
                _currentStateType = StateType.Disabled;
                Switch(_currentStateType);
            }
            else
            {
                _currentStateType = StateType.Plus;
                Switch(_currentStateType);
            }
        }
    }
}