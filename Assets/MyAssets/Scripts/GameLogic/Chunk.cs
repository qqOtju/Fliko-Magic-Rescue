using System;
using MyAssets.Scripts.Data;
using MyAssets.Scripts.GameLogic.Obstacles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MyAssets.Scripts.GameLogic
{
    [RequireComponent(typeof(Collider2D))]
    public class Chunk: MonoBehaviour
    {
        [SerializeField] private Transform _plusPlatformsParent;
        [SerializeField] private Transform _minusPlatformsParent;
        [SerializeField] private Transform _minesParent;
        [SerializeField] private Transform  _rocketLaserParent;
        
        private Platform[] _plusPlatforms;
        private Platform[] _minusPlatforms;
        private Mine[] _mines;
        private RocketLaser[] _rocketLasers;

        public float Height { get; private set; }
        
        public event Action<Chunk> OnChunkPassed;

        public void Init(DataOfGame dataOfGame)
        {
            foreach (var mine in _mines)
                mine.Init(dataOfGame);
            foreach (var laser in _rocketLasers)
                laser.Init(dataOfGame);
        }

        private void Awake()
        {
            _plusPlatforms = _plusPlatformsParent.GetComponentsInChildren<Platform>();
            _minusPlatforms = _minusPlatformsParent.GetComponentsInChildren<Platform>();
            _mines = _minesParent.GetComponentsInChildren<Mine>();
            _rocketLasers = _rocketLaserParent.GetComponentsInChildren<RocketLaser>();
            var coll = GetComponent<Collider2D>();
            coll.isTrigger = true;
            var bounds = coll.bounds;
            Height = bounds.size.y;
        }

        private void Start()
        {
            int minesCount;
            if (_mines.Length > 1)
                minesCount = Random.Range(1, _mines.Length - 1);
            else
                minesCount = _mines.Length;
            foreach (var mine in _mines)
                mine.gameObject.SetActive(false);
            for (var i = 0; i < minesCount; i++)
            {
                var mine = _mines[Random.Range(0, _mines.Length)];
                while (mine.gameObject.activeSelf)
                    mine = _mines[Random.Range(0, _mines.Length)];
                mine.gameObject.SetActive(true);
            }

            int rocketLasersCount;
            if (_rocketLasers.Length > 1)
                rocketLasersCount = Random.Range(1, _rocketLasers.Length - 1);
            else
            {
                var random = Random.Range(0, 100);
                if (random < 50)
                    rocketLasersCount = 0;
                else
                    rocketLasersCount = _rocketLasers.Length;
            }

            foreach (var laser in _rocketLasers)
                laser.gameObject.SetActive(false);
            for (var i = 0; i < rocketLasersCount; i++)
            {
                var laser = _rocketLasers[Random.Range(0, _rocketLasers.Length)];
                while (laser.gameObject.activeSelf)
                    laser = _rocketLasers[Random.Range(0, _rocketLasers.Length)];
                laser.gameObject.SetActive(true);
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            OnChunkPassed?.Invoke(this);
        }
        
        public void Switch(StateType state)
        {
            if(_plusPlatforms == null || _minusPlatforms == null) return;
            foreach (var platform in _plusPlatforms)
                platform.Switch(state);
            foreach (var platform in _minusPlatforms)
                platform.Switch(state);
        }
    }
}