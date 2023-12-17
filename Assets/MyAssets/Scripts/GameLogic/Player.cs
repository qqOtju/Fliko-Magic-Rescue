using MyAssets.Scripts.Data;
using MyAssets.Scripts.Data.SO;
using MyAssets.Scripts.MyInput;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MyAssets.Scripts.GameLogic
{
    [SelectionBase]
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class Player: MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sr;
        [SerializeField] private Sprite _plusSprite;
        [SerializeField] private Sprite _minusSprite;
        [SerializeField] private Color _flyColor;
        [SerializeField] private Button _switchButton;
        [SerializeField] private ParticleSystem _plusParticles;
        [SerializeField] private ParticleSystem _minusParticles;
        
        private PlayerSO PlayerStats => _dataOfGame.DataPlayer.PlayerSO;
        
        private Color _baseColor;
        private StateType _stateType = StateType.Plus;
        private DataAbility _lowGravityDataAbility;
        private DataAbility _flyDataAbility;
        private InputController _inputController;
        private DataOfGame _dataOfGame;
        private Rigidbody2D _rb;

        [Inject]
        private void Construct(DataOfGame dataOfGame,InputController inputController)
        {
            _dataOfGame = dataOfGame;
            foreach (var ability in _dataOfGame.Abilities)
            {
                switch (ability.Ability.ID)
                {
                    case 1:
                        _lowGravityDataAbility = ability;
                        break;
                    case 2:
                        _flyDataAbility = ability;
                        break;
                }
            }
            _lowGravityDataAbility.OnAbilityUse += OnLowGravityDataAbilityUse;
            _lowGravityDataAbility.OnAbilityEnd += OnLowGravityDataAbilityUse;
            _flyDataAbility.OnAbilityUse += OnFlyDataAbilityUse;
            _flyDataAbility.OnAbilityEnd += OnFlyDataAbilityUse;
            _inputController = inputController;
            _inputController.OnInput += OnInput;
            _inputController.OnInputCanceled += OnInputCanceled;
        }

        private void Awake()
        {
            _switchButton.onClick.AddListener(Switch);
        }

        private void Start()
        {
            _baseColor = _sr.color;
            _sr.sprite = _plusSprite;
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Platform")) return;
            Debug.Log("Trigger");
            var platform = other.GetComponent<Platform>();
            if(platform.Type == _stateType)
                Jump();
        }

        private void OnDestroy()
        {
            _inputController.OnInput -= OnInput;
            _inputController.OnInputCanceled -= OnInputCanceled;
            _lowGravityDataAbility.OnAbilityUse -= OnLowGravityDataAbilityUse;
            _lowGravityDataAbility.OnAbilityEnd -= OnLowGravityDataAbilityUse;
            _flyDataAbility.OnAbilityUse -= OnFlyDataAbilityUse;
            _flyDataAbility.OnAbilityEnd -= OnFlyDataAbilityUse;
        }

        private void Switch()
        {
            if(_stateType == StateType.Plus)
            {
                _sr.sprite = _minusSprite;
                _stateType = StateType.Minus;
                _minusParticles.Play();
            }
            else
            {
                _sr.sprite = _plusSprite;
                _stateType = StateType.Plus;
                _plusParticles.Play();
            }
        }

        private void Jump()
        {
            _rb.velocity = new Vector2(_rb.velocity.x, 0);
            _rb.AddForce(Vector2.up * PlayerStats.JumpForce, ForceMode2D.Impulse);
        }

        private void OnInputCanceled()
        {
            _rb.velocity = new Vector2(0, _rb.velocity.y);
        }

        private void OnInput(float obj)
        {
            _rb.velocity = new Vector2(obj * PlayerStats.Speed, _rb.velocity.y);
        }

        private void OnLowGravityDataAbilityUse(int id)
        {
            _rb.gravityScale = _lowGravityDataAbility.Activated ? 0.5f : 1;
        }

        private void OnFlyDataAbilityUse(int obj)
        {
            _sr.color = _flyDataAbility.Activated ? _flyColor : _baseColor;
            if(_flyDataAbility.Activated)
            {
                _rb.gravityScale = 0;
                _rb.velocity = Vector2.zero;
                _rb.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
            }
            else
            {
                _rb.gravityScale = 1;
                _rb.velocity = Vector2.zero;
            }
        }
    }
}