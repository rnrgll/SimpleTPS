using System;
using Cinemachine;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public bool IsControlActive { get; set; } = true; //제어

        private PlayerStatus _status;
        private PlayerMovement _movement;
        private Animator _animator;
        private Image _aimImg;
        
        [SerializeField] private CinemachineVirtualCamera _aimCamera;
        [SerializeField] private Gun _gun;
        [SerializeField] private Animator _aimAnimator;
        [SerializeField] private HpGuageUI _hpUI;
        
        
        [SerializeField] private KeyCode _aimKey;
        [SerializeField] private KeyCode _shootKey;
        
        
        private void Awake() => Init();
        private void OnEnable() => SubscribeEvents();
        private void Update() => HandlePlayerControl();

        private void OnDisable() => UnsubscribeEvents();


        private void Init()
        {
            _status = GetComponent<PlayerStatus>();
            _movement = GetComponent<PlayerMovement>();
            _animator = GetComponent<Animator>();
            _aimImg = _aimAnimator.GetComponent<Image>();
        }

        private void HandlePlayerControl()
        {
            if (!IsControlActive) return;
            HandleMovement();
            HandleAiming();
            HandleShooting();
        }

        private void HandleShooting()
        {
            //aim 상태일 때 + shoot 키 누르면 -> 공격 수행
            if (_status.IsAiming.Value && Input.GetKey(_shootKey))
            {
                _status.IsAttacking.Value = _gun.Shoot();
            }
            else
            {
                _status.IsAttacking.Value = false; 
            }
        }

        private void HandleMovement()
        {
            Vector3 camRotateDir = _movement.SetAimRotation();

            float moveSpeed;
            if (_status.IsAiming.Value) moveSpeed = _status.WalkSpeed;
            else moveSpeed = _status.RunSpeed;

            Vector3 moveDir = _movement.SetMove(moveSpeed);
            _status.IsMoving.Value = (moveDir != Vector3.zero);

            Vector3 avatarDir;
            if (_status.IsAiming.Value) avatarDir = camRotateDir;
            else avatarDir = moveDir;

            _movement.SetAvatarRotation(avatarDir);
            
            //입력값에 대한 벡터를 가져온다. 이동방향이 아니라!!
            if (_status.IsAiming.Value)
            {
                Vector3 input = _movement.GetInputDirection();
                _animator.SetFloat("X", input.x);
                _animator.SetFloat("Z", input.z);
                
            }
        }

        private void HandleAiming()
        {
            _status.IsAiming.Value = Input.GetKey(_aimKey);
        }


        public void TakeDamage(int value)
        {
            //체력을 떨어뜨리되, 체력이 0 이하기 되면 플레이어가 죽도로 처리
            _status.CurrentHp.Value -= value;
            if (_status.CurrentHp.Value <= 0) Dead();
        }

        public void RecoveryHp(int value)
        {
            //체력을 회복시키되, MaxHp보다 초과가 되는 것을 막아야 함
            int hp = _status.CurrentHp.Value + value;

            _status.CurrentHp.Value = Mathf.Clamp(hp, 0, _status.MaxHp);
        }

        public void Dead()
        {
            //직접 구현해보기
            Debug.Log("플레이어 사망 처리");
        }
        

        private void SubscribeEvents()
        {
            
            _status.CurrentHp.Subscribe(SetHpUIGuage);
            
            _status.IsAiming.Subscribe(_aimCamera.gameObject.SetActive);
            _status.IsAiming.Subscribe(SetAimAnimation);
            _status.IsMoving.Subscribe(SetMoveAnimation);
            _status.IsAttacking.Subscribe(SetAttackAnimation);
        }

        private void UnsubscribeEvents()
        {
            _status.CurrentHp.Unsubscribe(SetHpUIGuage);
            
            _status.IsAiming.Unsubscribe(_aimCamera.gameObject.SetActive);
            _status.IsAiming.Unsubscribe(SetAimAnimation);
            _status.IsMoving.Unsubscribe(SetMoveAnimation);
            _status.IsAttacking.Unsubscribe(SetAttackAnimation);

        }

        private void SetAimAnimation(bool value)
        {
            if (!_aimImg.enabled) _aimImg.enabled = true;
            _animator.SetBool("IsAim", value);
            _aimAnimator.SetBool("IsAim", value);
        } 
        private void SetMoveAnimation(bool value) => _animator.SetBool("IsMove", value);
        private void SetAttackAnimation(bool value) => _animator.SetBool("IsAttack", value);


        private void SetHpUIGuage(int currentHp)
        {
            // 현재 수치 / 최대 수치 적용
            float hp = currentHp / (float) _status.MaxHp;
            _hpUI.SetImageFillAmount(hp);
        }
        
    }
}