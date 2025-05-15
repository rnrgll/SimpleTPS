using System;
using DesignPattern;
using UnityEngine;


public class AudioManager : Singleton<AudioManager>
{
    private AudioSource _bgmSource;

    //오브젝트 풀을 가지도록 하여 효과음 재생 처리?

    private void Awake() => Init();

    private void Init()
    {
        _bgmSource = GetComponent<AudioSource>();
    }

    public void BgmPlay()
    {
        _bgmSource.Play();
    }
}