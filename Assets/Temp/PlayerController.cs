using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace LDH_Test
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerMovement _movement;
        public PlayerStatus _status;
        
        // Update is called once per frame
        void Update()
        {
            MoveTest();
            
            //IsAming 변경용 테스트 코드
            _status.IsAming.Value = Input.GetKey(KeyCode.Mouse1);
        }
        
        public void MoveTest()
        {
            //(회전 수행 후) 좌우 회전에 대한 벡터 반환
            Vector3 camRotateDir = _movement.SetAimRotation();

            float moveSpeed;
            if (_status.IsAming.Value) moveSpeed = _status.WalkSpeed;
            else moveSpeed = _status.RunSpeed;

            Vector3 moveDir = _movement.SetMove(moveSpeed);
            _status.IsMoving.Value = (moveDir != Vector3.zero);
            
            //몸체의 회전
            Vector3 avatarDir;
            if (_status.IsAming.Value) avatarDir = camRotateDir;
            else avatarDir = moveDir; //조준 상태아니면 움직이는 방향을 보도록
            
            _movement.SetAvatarRotation(avatarDir);

        }


    }

}
