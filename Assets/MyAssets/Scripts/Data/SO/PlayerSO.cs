using UnityEngine;

namespace MyAssets.Scripts.Data.SO
{
    [CreateAssetMenu(fileName = "PlayerSO", menuName = "MyAssets/PlayerSO")]
    public class PlayerSO: ScriptableObject
    {
        [SerializeField] private int _maxHealth;
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpForce;
        
        public int MaxHealth => _maxHealth;
        public float Speed => _speed;
        public float JumpForce => _jumpForce;
    }
}