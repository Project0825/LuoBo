using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 逻辑管理
/// </summary>
public class GameController : MonoBehaviour {
    private static GameController _instance;
    public static GameController Instance
    {
        get
        {
            return _instance;
        }
    }

    public Level mLevel;
    private GameManager mGameManager;
    public int[] mMonsterIDList;
    public int mMonsterIDIndex;
    public Stage mCurrentStage;
    public MapMaker mapMaker;
    public RuntimeAnimatorController[] AnimControllers;

    public NormalModelPanel normalModelPanel;

    public int KillMonsterNum;//当前波次杀怪数
    public int ClearItemNum;
    public int KillMonsterTotalNum;

    public int CarrotHp = 10;
    public int Coin;
    public int GameSpeed;
    public bool isPause;
    public Transform TargetTrans;//集火目标
    public GameObject TargetSignal;//集火标记
    public GridPoint SelectGrid;
    public bool creatingMonster;//是否继续产怪
    public bool gameOver;


    //建造者
    public MonsterBuilder monsterBuilder;

    public Dictionary<int, int> TowerPriceDict;
    //建塔种类列表
    public GameObject towerListGo;
    public GameObject handleTowerCanvasGo;//处理塔的画布

    private void Awake()
    {
#if Game
        _instance = this;
        mGameManager = GameManager.Instance;
        mCurrentStage = mGameManager.CurrentStage;
        //normalModelPanel = mGameManager.uiManager.mUIFacade.currentScenePanelDict[StringManager.NormalModelPanel] as NormalModelPanel;
        mapMaker = GetComponent<MapMaker>();
        mapMaker.InitMapMaker();
        //mapMaker.LoadMap(mCurrentStage.mBigLevelID, mCurrentStage.mLevelID);
        mapMaker.LoadMap(1, 3);

        GameSpeed = 1;
        monsterBuilder = new MonsterBuilder();
        mLevel = new Level(mapMaker.roundInfoList.Count, mapMaker.roundInfoList);
        mLevel.HandleRound();


        AnimControllers = new RuntimeAnimatorController[12];
        for (int i = 0; i < AnimControllers.Length; i++)
        {
            AnimControllers[i] = GetRuntimeAnimatorController("Monster/" + mapMaker.bigLevelID.ToString() + "/" + (i + 1).ToString());
        }

#endif
    }

    private void Update()
    {
#if Game
        if (!isPause)
        {
            if (KillMonsterTotalNum>=mMonsterIDList.Length)
            {
                AddRoundNum();
            }
            else
            {
                if (!creatingMonster)
                {
                    CreateMonster();
                }
            }
        }
        else
        {
            //暂停
            StopCreateMonster();
            creatingMonster = false;
        }
#endif
    }

    public void CreateMonster()
    {
        creatingMonster = true;
        InvokeRepeating("instantiateMonster",(float)1/GameSpeed, (float)1 / GameSpeed);
    }
    /// <summary>
    /// 产生怪物
    /// </summary>
    private void instantiateMonster()
    {
        GameObject effectGo = GetGameObjectResources("DoorEffect");
        effectGo.transform.SetParent(transform);
        effectGo.transform.position = mapMaker.monsterPathPos[0];
        //产生怪物
        if (mMonsterIDIndex<mMonsterIDList.Length)
        {
            monsterBuilder.m_monsterID = mLevel.RoundsList[mLevel.CurrentRound].roundInfo.mMonsterIDList[mMonsterIDIndex];
        }

        GameObject monsterGo = monsterBuilder.GetProduct();
        monsterGo.transform.SetParent(transform);
        monsterGo.transform.position = mapMaker.monsterPathPos[0];
        mMonsterIDIndex++;
        if (mMonsterIDIndex>=mMonsterIDList.Length)
        {
            StopCreateMonster();
        }
    }
    public void StopCreateMonster()
    {
        CancelInvoke();
    }
    //增加当前回合数，并且交给下一个关卡来处理产怪(怪物击杀完成)
    public void AddRoundNum()
    {
        mMonsterIDIndex = 0;
        KillMonsterNum = 0;
        mLevel.AddRoundNum();
        mLevel.HandleRound();
        //更新回合产怪UI

    }

    public Sprite GetSprite(string resourcePath)
    {
        return mGameManager.GetSprite(resourcePath);
    }
    public AudioClip GetAudioClip(string resourcePath)
    {
        return mGameManager.GetAudioClip(resourcePath);
    }
    public RuntimeAnimatorController GetRuntimeAnimatorController(string resourcePath)
    {
        return mGameManager.GetRuntimeAnimatorController(resourcePath);
    }
    public GameObject GetGameObjectResources(string resourcePath)
    {
        return mGameManager.GetGameObject(FactoryType.GameFactory,resourcePath);
    }
    public void PushGameObjectToFactory(string resourcePath,GameObject itemGo)
    {
        mGameManager.PushGameObjectToFactor(FactoryType.GameFactory, resourcePath,itemGo);
    }
}
