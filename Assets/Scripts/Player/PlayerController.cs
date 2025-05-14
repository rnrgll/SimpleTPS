using System;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public bool IsControlActive { get; set; } = true; //제어

        private PlayerStatus _status;
        private PlayerMovement _movement;
        [SerializeField] private GameObject _aimCamera;
        private GameObject _mainCamera;
        
        [SerializeField] private KeyCode _aimKey;
        
        
        private void Awake()
        {
            Init();
        }

        private void OnEnable() => SubscribeEvents();
        private void Update() => HandlePlayerControl();

        private void OnDisable() => UnsubscribeEvents();


        private void Init()
        {
            _status = GetComponent<PlayerStatus>();
            _movement = GetComponent<PlayerMovement>();
            _mainCamera = Camera.main.gameObject;
        }

        private void HandlePlayerControl()
        {
            if (!IsControlActive) return;
            HandleMovement();
            HandleAiming();
        }
        
        private void HandleMovement(){}

        private void HandleAiming()
        {
            _status.IsAming.Value = Input.GetKey(_aimKey);
        }

        private void SubscribeEvents()
        {
            _status.IsAming.Subscribe(value => SetActiveAimCamera(value));
            //_status.IsAming.Subscribe(SetActiveAimCamera); //람다식 아닌 버전
        }

        private void UnsubscribeEvents()
        {
            _status.IsAming.Unsubscribe(value => SetActiveAimCamera(value));
        }

        public void SetActiveAimCamera(bool value)
        {
            _aimCamera.SetActive(value);
            _mainCamera.SetActive(!value);
        }
    }
}