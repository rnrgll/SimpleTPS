using UnityEngine;
using UnityEngine.Events;

namespace DesignPattern
{
    public class ObservableProperty<T>
    {
        [SerializeField] private T _value;

        public T Value
        {
            get => _value;
            set
            {
                if (_value.Equals(value)) return; //기존의 _value와 새로 들어온 값(value)이 같다면 굳이 변경할 필요가 없음
                _value = value;
               Notify(); //값이 변경될 때 알림 전송
            }
        }
        
        
        //_value가 변경됐을 때 실행시킬 유니티 이벤트
        private UnityEvent<T> _onValueChanged = new();
        
        //생성자 : value로 아무것도 안들어 온다면 default, 들어오면 그 값으로 셋팅
        public ObservableProperty(T value = default)
        {
            _value = value;
        }

        //구독 : unity action을 받아서 유니티 이벤트에 구독한다
        public void Subscribe(UnityAction<T> action)
        {
            _onValueChanged.AddListener(action);   
        }

        //구독 해제
        public void Unsubscribe(UnityAction<T> action)
        {
            _onValueChanged.RemoveListener(action);
        }
        
        //모든 구독 해제
        public void UnsubscribeAll()
        {
            _onValueChanged.RemoveAllListeners();
        }
        
        //모든 구독자들에게 알림을 전송하는 메소드
        private void Notify()
        {
            _onValueChanged?.Invoke(Value);
        }
        
    }
}