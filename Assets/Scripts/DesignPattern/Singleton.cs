using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPattern
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    //싱글톤이 초기화된 상태에서 다른 객체가 참고하고자 한다면
                    _instance = FindObjectOfType<T>();
                    DontDestroyOnLoad(_instance);
                }
                return _instance;
            }
        }

        protected void SingletonInit()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this as T; //싱글톤을 Generic Type으로 전환하기
                //아래 코드를 사용해도 됨
                //_instance = GetComponent<T>();
            
                DontDestroyOnLoad(_instance);
            }
        }
    }
}
