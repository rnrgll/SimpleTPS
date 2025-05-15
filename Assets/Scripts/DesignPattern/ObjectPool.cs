using System.Collections.Generic;
using UnityEngine;

namespace DesignPattern
{
    public class ObjectPool
    {
        private Stack<PooledObject> _stack;
        private PooledObject _targetPrefab;
        private GameObject _poolObject;

        //오브젝트 풀
        //1 . 오브젝트 release
        //2. 오브젝트 반환
        //3. 오브젝트 풀 생성

        public ObjectPool(PooledObject targetPrefab, int initSize = 5) => Init(targetPrefab, initSize);

        private void Init(PooledObject targetPrefab, int initSize)
        {
            _stack = new Stack<PooledObject>(initSize);
            _targetPrefab = targetPrefab;
            _poolObject = new GameObject($"{targetPrefab.name} Pool"); //오브젝트 풀 이름을 pooledObject될 오브젝트의 이름으로 생성해버리기

            for (int i = 0; i < initSize; i++)
            {
                //초기화 사이즈만큼 프리팹을 생성해준다. (Instantiate)
                CreatePoolObject();
            }
        }

        public PooledObject Get()
        {
            //효과음의 경우 어느정도 오브젝트를 생성해야할지 잘 모르기 때문에 감이 안오기 때문에 일단 용량 제한 걸지 않고 오브젝트 풀 정의
            
            //풀에 아무것도 없는 경우 새로 생성해주기
            if (_stack.Count==0) CreatePoolObject();
            
            PooledObject pooledObject = _stack.Pop();
            pooledObject.gameObject.SetActive(true);

            return pooledObject;

        }

        public void PushPool(PooledObject target)
        {
            target.transform.SetParent(_poolObject.transform);
            target.gameObject.SetActive(false);
            _stack.Push(target);
        }

        private void CreatePoolObject()
        {
            PooledObject obj = MonoBehaviour.Instantiate(_targetPrefab);
            obj.PooledInit(this);
            PushPool(obj); //생성한 오브젝트를 풀에 넣어주기
        }
    }
}