using System;
using System.Collections.Generic;
using DesignPattern;
using UnityEngine;
using Util;


public class AudioManager : Singleton<AudioManager>
{
    private AudioSource _bgmSource;
    
    [SerializeField] private List<AudioClip> _bgmList = new(); //bgm 리스트
    [SerializeField] private SFXController _sfxPrefab;
    
    private ObjectPool _sfxPool; //효과음 재생을 위한 효과음 오브젝트 풀
    
    private void Awake() => Init();

    private void Init()
    {
        _bgmSource = GetComponent<AudioSource>();

        _sfxPool = new ObjectPool(transform, _sfxPrefab, 10);

    }

    public void BgmPlay(int index)
    {
        if (0 <= index && index <= _bgmList.Count)
        {
            _bgmSource.Stop(); //기존 bgm 플레이 중지 
            _bgmSource.clip = _bgmList[index]; //bgm 변경
            _bgmSource.Play(); //새로운 bgm 플레이
        }
    }
    
    //효과음 재생을 위한 메서드
    public SFXController GetSFX()
    {
        //풀에서 꺼내서 반환
        return _sfxPool.PopPool() as SFXController;
    }
    
}