using System;
using DesignPattern;
using UnityEngine;

namespace Util
{
    public class SFXController : PooledObject
    {
        private AudioSource _audioSource;
        private float _currentCount;
        
        private void Awake() => Init();

        private void Init()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            _currentCount -= Time.deltaTime;
            
            if(_currentCount<=0)
            {
                _audioSource.Stop();
                _audioSource.clip = null;
                ReturnPool(); //audio clip's length = current count가 0이 되면 스스로를 풀에 반납
            }
        }

        //클립 길이를 계산해서 길이만큼 재생하고 끝나면 종료 시켜줘야함
        public void Play(AudioClip clip, bool loop = false, bool onAwake = false)
        {
            // //효과음 재생 옵션 설정
            // _audioSource.loop = loop;
            // _audioSource.playOnAwake = onAwake;
            
            
            _audioSource.Stop();
            _audioSource.clip = clip;
            _audioSource.Play();
            
            //clip.length -> clip이 몇 초인지 반환
            _currentCount = clip.length;
        }
    }
}