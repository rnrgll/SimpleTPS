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
        [SerializeField] private CinemachineVirtualCamera _aimCamera;
        
        [SerializeField] private KeyCode _aimKey;
        
        
        private void Awake() => Init();
        private void OnEnable() => SubscribeEvents();
        private void Update() => HandlePlayerControl();

        private void OnDisable() => UnsubscribeEvents();


        private void Init()
        {
            _status = GetComponent<PlayerStatus>();
            _movement = GetComponent<PlayerMovement>();
            //_mainCamera = Camera.main.gameObject;
        }

        private void HandlePlayerControl()
        {
            if (!IsControlActive) return;
            HandleMovement();
            HandleAiming();
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
        }

        private void HandleAiming()
        {
            _status.IsAiming.Value = Input.GetKey(_aimKey);
        }

        private void SubscribeEvents()
        {
            _status.IsAiming.Subscribe(_aimCamera.gameObject.SetActive);
        }

        private void UnsubscribeEvents()
        {
            _status.IsAiming.Unsubscribe(_aimCamera.gameObject.SetActive);

        }
        
    }
}