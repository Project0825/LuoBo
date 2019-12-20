using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour {
    //属性值
    public int MonsterID;
    public int Hp;
    public int CurrentHp;
    public float MoveSpeed;
    public float InitMoveSpeed;
    public int Prize;//怪物掉落金钱
    //引用
    private Animator animator;
    private Slider slider;
    public GameObject TshitGo;

    //用于计数的属性或开关
    private int roadPointIndex = 1;
    private bool reachCarrot;//到达终点
    private bool hasDecreasSpeed;//是否减速

    private float decreaseSpeedTimer;//减速计时器
    private float decreaseTime;//减速时间

    //资源
    public AudioClip dieAudioClip;
    public RuntimeAnimatorController animatorController;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void GetMonsterProperty()
    {
        animatorController = GameController.Instance.AnimControllers[MonsterID-1];
        animator.runtimeAnimatorController = animatorController;
    }
}
