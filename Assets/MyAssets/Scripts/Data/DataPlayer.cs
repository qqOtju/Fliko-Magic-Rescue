using System;
using MyAssets.Scripts.Data.SO;

namespace MyAssets.Scripts.Data
{
    public class DataPlayer
    {
        private readonly int _maxHealth;
        
        private int _currentHealth;

        public event Action<int> OnHealthUpdated;
        public event Action OnPlayerDeath;
        
        private int CurrentHealth
        {
            get => _currentHealth;
            set
            {
                if(value > _maxHealth)
                    value = _maxHealth;
                _currentHealth = value;
                OnHealthUpdated?.Invoke(value);
                if(value <= 0)
                    OnPlayerDeath?.Invoke();
            }
        }
        
        public PlayerSO PlayerSO { get; }

        public DataPlayer(PlayerSO stats)
        {
            PlayerSO = stats;  
            _maxHealth = PlayerSO.MaxHealth;
            CurrentHealth = PlayerSO.MaxHealth;
        }
        
        public void ResetHealth()
        {
            CurrentHealth = _maxHealth;
        }
        
        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
        }
    }
}