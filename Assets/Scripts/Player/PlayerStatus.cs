using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPattern;
public class PlayerStatus : MonoBehaviour
{
    [field: SerializeField][field: Range(0, 10)]
    public float WalkSpeed { get; set; }
    
    [field: SerializeField][field: Range(0, 10)]
    public float RunSpeed { get; set; }
    
    [field: SerializeField][field: Range(0, 10)]
    public float RotateSpeed { get; set; }
    
    //플레이어가 어떤 상태인지를 observable property로 구현
    public ObservableProperty<bool> IsAming { get; private set; } = new(); //조준 상태인가
    public ObservableProperty<bool> IsMoving { get; private set; } = new(); //움직이는 상태인가
    public ObservableProperty<bool> IsAttacking { get; private set; } = new(); //공격 상태인가
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
