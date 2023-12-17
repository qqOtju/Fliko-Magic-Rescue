using System;
using System.Collections;
using MyAssets.Scripts.Data;
using UnityEngine;

namespace MyAssets.Scripts.GameLogic.Obstacles
{
    [SelectionBase]
    [RequireComponent(typeof(Collider2D))]
    public class RocketLaser: MonoBehaviour
    {
        [SerializeField] private Rocket _rocketPrefab;
        [SerializeField] private float _spawnDelay;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Transform _view;

        private DataOfGame _dataOfGame;
        
        public void Init(DataOfGame dataOfGame)
        {
            _dataOfGame = dataOfGame;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.CompareTag("Player"))
                StartCoroutine(LaunchRocket());
        }
        
        private IEnumerator LaunchRocket()
        {
            _view.gameObject.SetActive(false);
            yield return new WaitForSeconds(_spawnDelay);
            SpawnRocket();
            Destroy(gameObject);
        }

        private void SpawnRocket()
        {
            var rocket = Instantiate(_rocketPrefab);
            rocket.Init(_dataOfGame);
            rocket.transform.position = _spawnPoint.position;
        }
    }
}