using MyAssets.Scripts.Data;
using UnityEngine;

namespace MyAssets.Scripts.GameLogic.Obstacles
{
    [SelectionBase]
    [RequireComponent(typeof(Collider2D))]
    public class Rocket: MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _lifeTime;
        [SerializeField] private ParticleSystem _ps;

        private DataOfGame _dataOfGame;
        private float _timer;
        
        public void Init(DataOfGame dataOfGame)
        {
            _dataOfGame = dataOfGame;
        }
        
        private void FixedUpdate()
        {
            _timer += Time.fixedDeltaTime;
            if (_timer >= _lifeTime)
                Destroy(gameObject);
            transform.Translate(Vector2.right * (_speed * Time.fixedDeltaTime));
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            _dataOfGame.DataPlayer.TakeDamage(1);
            _ps.gameObject.transform.parent = gameObject.transform.parent;
            _ps.Play();
            Destroy(gameObject);
        }
    }
}