using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HpGuageUI : MonoBehaviour
    {
        //1. LookAt(메인카메라) : 카메라 위치에 따라서 hp 게이지가 기울어짐
        //2. 현재 카메라의 방향으로 회전 : 카메라의 방향 벡터를 적용 => 이 방법을 사용하면 된다.
        //3. 카메라의 반대 방향 벡터 적용 : ui가 뒤집히는 문제가 발생

        [SerializeField] private Image _image;
        private Transform _cameraTransform;
 

        private void Awake() => Init();

        private void LateUpdate() => SetUIForwardVector(_cameraTransform.forward);

        private void Init()
        {
            _cameraTransform = Camera.main.transform;
        }


        //value = 현재 수치 / 최대 수치
        public void SetImageFillAmount(float value)
        {
            _image.fillAmount = value;
        }
        
        private void SetUIForwardVector(Vector3 target)
        {
            
            //현재 카메라 방향으로 회전
            //둘 다 같은 기능을 하는 코드
            //transform.rotation = Camera.main.transform.rotation; 
            transform.forward = target;

        }

    }
}