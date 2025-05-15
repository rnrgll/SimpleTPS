using System;
using Cinemachine;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public bool IsControlActive { get; set; } = true; //제어

        private PlayerStatus _status;
        private PlayerMovement _movement;
        private Animator _animator;
        
        [SerializeField] private CinemachineVirtualCamera _aimCamera;
        [SerializeField] private KeyCode _aimKey;

        [SerializeField] private Gun _gun;
        [SerializeField] private KeyCode _shootKey;
        
        
        private void Awake() => Init();
        private void OnEnable() => SubscribeEvents();
        private void Update() => HandlePlayerControl();

        private void OnDisable() => UnsubscribeEvents();


        private void Init()
        {
            _status = GetComponent<PlayerStatus>();
            _movement = GetComponent<PlayerMovement>();
            //_mainCamera = Camera.main.gameObject;
            _animator = GetComponent<Animator>();
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

        private void SubscribeEvents()
        {
            _status.IsAiming.Subscribe(_aimCamera.gameObject.SetActive);
            _status.IsAiming.Subscribe(SetAimAnimation);
            _status.IsMoving.Subscribe(SetMoveAnimation);
            _status.IsAttacking.Subscribe(SetAttackAnimation);
        }

        private void UnsubscribeEvents()
        {
            _status.IsAiming.Unsubscribe(_aimCamera.gameObject.SetActive);
            _status.IsAiming.Unsubscribe(SetAimAnimation);
            _status.IsMoving.Unsubscribe(SetMoveAnimation);
            _status.IsAttacking.Unsubscribe(SetAttackAnimation);

        }

        private void SetAimAnimation(bool value) => _animator.SetBool("IsAim", value);
        private void SetMoveAnimation(bool value) => _animator.SetBool("IsMove", value);
        private void SetAttackAnimation(bool value) => _animator.SetBool("IsAttack", value);
    }
}