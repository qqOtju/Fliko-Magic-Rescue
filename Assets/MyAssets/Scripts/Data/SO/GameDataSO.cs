using UnityEngine;

namespace MyAssets.Scripts.Data.SO
{
    [CreateAssetMenu(fileName = "GameDataSO", menuName = "MyAssets/GameDataSO")]
    public class GameDataSO: ScriptableObject
    {
        [SerializeField] private PlayerSO _playerSO;
        [SerializeField] private AbilitySO[] _abilitiesSO;
        
        public PlayerSO PlayerSO => _playerSO;
        public AbilitySO[] AbilitiesSO => _abilitiesSO;
    }
}