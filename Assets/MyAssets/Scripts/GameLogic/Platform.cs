using UnityEngine;

namespace MyAssets.Scripts.GameLogic
{
    [SelectionBase]
    [RequireComponent(typeof(Collider2D))]
    public class Platform: MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sr;
        [SerializeField] private Color _disabledColor;
        [SerializeField] private Collider2D _triggerCollider;
        [SerializeField] private Collider2D _platformCollider;
        [SerializeField] private StateType _state;
        
        private Color _defaultColor;

        public StateType Type => _state;

        private void Awake()
        {
            _defaultColor = _sr.color;
        }
        
        public void Switch(StateType state)
        {
            if (state == _state)
            {
                SwitchCollider(true);
                _sr.color = _defaultColor;
            }
            else
            {
                SwitchCollider(false);
                _sr.color = _disabledColor;
            }         
        }
        
        private void SwitchCollider(bool status)
        {
            _triggerCollider.enabled = status;
            _platformCollider.enabled = status;    
        }
    }
}