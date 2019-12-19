using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    public int MonsterID;
    public int Hp;
    public int CurrentHp;
    public float MoveSpeed;
    public float InitMoveSpeed;
    public int Prize;//怪物掉落金钱

    private Animator animator;
    public RuntimeAnimatorController animatorController;

    public void GetMonsterProperty()
    {
        animatorController = GameController.Instance.AnimControllers[MonsterID-1];
        animator.runtimeAnimatorController = animatorController;
    }
}
