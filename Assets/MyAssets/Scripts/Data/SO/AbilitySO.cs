using UnityEngine;

namespace MyAssets.Scripts.Data.SO
{
    [CreateAssetMenu(fileName = "AbilitySO", menuName = "MyAssets/AbilitySO")]
    public class AbilitySO: ScriptableObject
    {
        [Header("Info")]
        [SerializeField] private int _id;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _icon;
        [Header("Values")]
        [SerializeField] private float _cooldown;
        [SerializeField] private float _duration;
        [SerializeField] private int _price;
        
        public string Name => _name;
        public string Description => _description;
        public Sprite Icon => _icon;
        public int ID => _id;
        public float Cooldown => _cooldown;
        public float Duration => _duration;
        public int Price => _price;
    }
}