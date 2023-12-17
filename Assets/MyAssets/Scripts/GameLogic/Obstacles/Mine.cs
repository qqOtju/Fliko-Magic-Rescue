using MyAssets.Scripts.Data;
using UnityEngine;

namespace MyAssets.Scripts.GameLogic.Obstacles
{
    [SelectionBase]
    [RequireComponent(typeof(Collider2D))]
    public class Mine: MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sr;
        [SerializeField] private ParticleSystem _ps;
        
        private DataOfGame _dataOfGame;
        
        public void Init(DataOfGame dataOfGame)
        {
            _dataOfGame = dataOfGame;
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