using System;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _avatar; //좌우 회전 및 이동
        [SerializeField] private Transform _aim; //aim 위아래 회전

        private Rigidbody _rigidbody;
        private PlayerStatus _playerStatus;
        
        [Header("Mouse Config")] 
        [SerializeField][Range(-90, 0)] private float _minPitch; //최소 각도
        [SerializeField][Range(0, 90)] private float _maxPitch; //최대 각도
        [SerializeField][Range(0, 5)] private float _mouseSensitivity = 1;
        //센서 값 : 마우스 움직였을 때 얼마나 빨리 회전시킬 것인가 

        private Vector2 _currentRotation;
        private void Awake()
        {
            Init();
        }

        //초기화 함수
        private void Init()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _playerStatus = GetComponent<PlayerStatus>();
        }
        
        
        //실제 이동
        //controller에서 제어하기 위해서 return을 해준다
        public Vector3 SetMove(float moveSpeed)
        {
            Vector3 moveDirection = GetMoveDirection();
            Vector3 velocity = _rigidbody.velocity; //현재 rigidbody의 velocity를 받아와서
            
            //moveDirection 값에 따라 velocity 값을 조정해준다.
            //플레이어 상태에 따라 이동 속도가 달라지기 때문에 매개변수로 받아오는 걸로 한다.
            velocity.x = moveDirection.x * moveSpeed;
            velocity.z = moveDirection.z * moveSpeed;

            //조정한 속도로 rigidbody 속도를 설정해준다.
            _rigidbody.velocity = velocity;

            return moveDirection;
        }
        
        //aim 회전
        //어떤 방향으로 회전하고 있는지를 마찬가지로 반환해준다.
        public Vector3 SetAimRotation()
        {
            Vector2 mouseDir = GetMouseDirection();
         
            
            //현재 회전 방향 체크 -> 가로는 회전 제한이 없지만, 세로는 min,max pitch 내로 제한되어야 하기 때문에 clamping이 필요함

            //x축의 경우 제한이 없음
            _currentRotation.x += mouseDir.x;
            //y축의 경우 각도 제한을 걸어야 함
            _currentRotation.y = Mathf.Clamp(
                _currentRotation.y + mouseDir.y, 
                _minPitch, 
                _maxPitch
                );

            //캐릭터 오브젝트의 경우에는 y축 회전만 반영되어야 한다 => aim 만 회전히야지 캐릭터 자체가 회전하면 안됨. 캐릭터는 y축 회전만 적용되어야 한다.
            transform.rotation = Quaternion.Euler(0, _currentRotation.x, 0);
            
            //에임의 경우 상하 회전을 반영한다.
            Vector3 currentEuler = _aim.localEulerAngles; //에임의 현재 각도를 받아온다.
            _aim.localEulerAngles = new Vector3(_currentRotation.y, currentEuler.y, currentEuler.z);
            
            //회전 방향 벡터 반환
            Vector3 rotateDirVector = transform.forward;
            rotateDirVector.y = 0; // 이 벡터의 Y축 높이(상하 이동) 값을 제거하여, 지면을 기준으로 한 평면 방향만 남기 (축 회전이랑 헷갈리지 말자)
            return rotateDirVector.normalized;
        }

        //body 회전
        public void SetAvatarRotation(Vector3 direction)
        {
            if (direction == Vector3.zero) return;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            _avatar.rotation = Quaternion.Lerp(
                _avatar.rotation,
                targetRotation,
                _playerStatus.RotateSpeed * Time.deltaTime
            );
            
        }


        #region Move
        
        //실제 이동해야하는 방향을 반환 (입력 값에 대해 가공 처리)
        public Vector3 GetMoveDirection()
        {
            Vector3 input = GetInputDirection();
            Vector3 direction =
                (transform.right * input.x)
                + (transform.forward * input.z); //플레이어의 오른쪽 방향, 앞 방향 벡터를 이용해서 실제 이동해야하는 방향을 계산
            return direction.normalized; //단위벡터로 변환하여 반환
        }

        //입력 값에 대한 방향 벡터 반환
        public Vector3 GetInputDirection()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            
            return new Vector3(h, 0, v);
        }
        #endregion

        #region Rotation

        private Vector2 GetMouseDirection()
        {
            float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity; //가로축으로 얼만큼 이동했는지를 반환받을 수 있음 / 마우스 감도를 곱해서 속도까지 지정
            float mouseY = -Input.GetAxis("Mouse Y") * _mouseSensitivity; //y축은 반대로 되어있어서 -로 뒤집어줘야함

            return new Vector2(mouseX, mouseY);
        }
        

        #endregion
    }
}