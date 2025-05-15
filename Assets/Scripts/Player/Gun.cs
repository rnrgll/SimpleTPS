using System;
using Cinemachine;
using UnityEngine;
using Util;

namespace Player
{
    //확장성을 고려한다면 Weapon 추상 클래스를 만들고 이를 상속받아서 Gun을 구현해도 됨
    public class Gun : MonoBehaviour
    {
        //총 필드 : 공격 거리(사거리), 공격력, 연사 시 딜레이, 격발음
        [SerializeField] private LayerMask _targetLayer; //충돌할 대상 레이어
        [SerializeField][Range(0, 100)] private float _attackRange;
        [SerializeField] private int _shootDamage;
        [SerializeField] private float _shootDelay;
        [SerializeField] private AudioClip _shootSFX; //격발음

        private CinemachineImpulseSource _impluseSrc;
        private Camera _camera;
        private bool _canShoot => _currentCount <= 0; //current count가 0이되면 발사 가능
        private float _currentCount; //발사 딜레이 계산을 위한 변수

        private void Awake() => Init();

        private void Update() => HandleCanShoot();
        
        private void Init()
        {
            _impluseSrc = GetComponent<CinemachineImpulseSource>();
            _camera = Camera.main;
        }
        
        //총 기능 : 발사(효과음, 그래픽 등)
        
        //아래의 발사 관련 메서드들을 관리할 핸들러
        public bool Shoot()
        {
            //누르고 있을 때 일정하게 발사되도록 해야함
            if (!_canShoot) return false;
            
            PlayShootSound();
            PlayCameraEffect();
            PlayShootEffect();

            _currentCount = _shootDelay;
            
            //todo: Ray 발사 -> 반환받은 대상에게 데미지 부여
            GameObject target = RayShoot(); //공격 대상 받아오기
            if (target == null) return true; //총을 쏘았으나 아무것도 공격하지 못한 상황. 총을 쏘았으니 true 반환
            
            Debug.Log($"총에 맞음 : {target.name}");
            //---
            
            return true;
        }

        private void HandleCanShoot()
        {
            if (_canShoot) return;
            _currentCount -= Time.deltaTime;
        }
        
        
        //레이캐스트로 피격 처리?
        
        //레이캐스트 발사 함수
        private GameObject RayShoot()
        {
            Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, _attackRange, _targetLayer))
            {
                return hit.transform.gameObject;
                //todo: 몬스터를 어떻게 구현하는가에 따라 다름.
                //공격 판정은 몬스터 구현하면서 같이 구현할 예정
            }

            return null;
        }

        private void PlayShootSound()
        {
            SFXController sfx = GameManager.Instance.Audio.GetSFX();
            sfx.Play(_shootSFX);
        }

        private void PlayCameraEffect()
        {
            _impluseSrc.GenerateImpulse(); //세부 impulse 효과는 inspector 창에서
        }

        private void PlayShootEffect()
        {
            //todo : 총구 화염 효과 -> 파티클로 구현
        }


    }
}