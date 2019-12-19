using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager{
    public int AdventrueModelNum;//冒险模式解锁地图个数
    public int BurriedLevelNum;//隐藏关卡
    public int BossModelNum;
    public int Coin;
    public int KillMonsterNum;
    public int KillBossNum;
    public int ClearItemNum;
    public List<bool> unLockedNurmalModelBigLevelList;
    public List<Stage> unLockedNurmalModelLevelList;
    public List<int> unLockedNurmalModelLevelNum;

    //怪物窝
    public int Cookies;
    public int Milk;
    public int Nest;
    public int Diamands;


    public PlayerManager()
    {
        AdventrueModelNum = 100;
        BurriedLevelNum = 101;
        BossModelNum = 102;
        Coin = 999;
        KillBossNum = 123;
        KillMonsterNum = 234;
        ClearItemNum = 222;
        unLockedNurmalModelLevelNum = new List<int>()
        {
            2,2,2
        };
        unLockedNurmalModelBigLevelList = new List<bool>()
        {
            true,false,true
        };
        unLockedNurmalModelLevelList = new List<Stage>()
        {
            new Stage(10,2,new int[] { 1,2},false,0,1,1,true,false),
            new Stage(10,2,new int[] { 1,2},false,0,1,1,true,false),
            new Stage(10,2,new int[] { 1,2},false,0,1,1,true,false),
            new Stage(10,2,new int[] { 1,2},false,0,1,1,true,false),
            new Stage(10,2,new int[] { 1,2},false,0,1,1,true,false),
            new Stage(10,2,new int[] { 1,2},false,0,1,1,true,false),
            new Stage(10,2,new int[] { 1,2},false,0,1,1,true,false),

        };
    }
}
