﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameNormalLevelPanel : BasePanel {
    private string filePath;
    public int currentBigLevelID;
    public int currentLevelID;

    private string theSpritePath;
    private Transform levelContentTrans;
    private GameObject img_LockBtnGo;
    private Transform emp_TowerTrans;
    private Image img_BGLeft;
    private Image img_BGRight;
    private Image img_Carrot;
    private Image img_AllClear;
    private Text tex_TotalWaves;

    private PlayerManager playerManager;
    private SlideMoveDan slideMove;

    private List<GameObject> levelContentImageGos;
    private List<GameObject> towerContentImageGos;

    protected override void Awake()
    {
        base.Awake();
        filePath = "GameOption/Normal/Level/";
        playerManager = mUIFacade.mPlayManager;
        levelContentImageGos = new List<GameObject>();
        towerContentImageGos = new List<GameObject>();
        levelContentTrans = transform.Find("Scroll View").Find("Viewport").Find("Content");
        img_LockBtnGo = transform.Find("Img_LockBtn").gameObject;
        emp_TowerTrans = transform.Find("Emp_Tower");
        img_BGLeft = transform.Find("Img_BGLeft").GetComponent<Image>();
        img_BGRight = transform.Find("Img_BGRight").GetComponent<Image>();
        tex_TotalWaves = transform.Find("Img_TotalWaves").Find("Text").GetComponent<Text>();
        slideMove = transform.Find("Scroll View").GetComponent<SlideMoveDan>();
        currentBigLevelID = 1;
        currentLevelID = 1;
    }

    private void loadResouece()
    {
        mUIFacade.GetSprite(filePath + "AllClear");
        mUIFacade.GetSprite(filePath + "Carrot_1");
        mUIFacade.GetSprite(filePath + "Carrot_2");
        mUIFacade.GetSprite(filePath + "Carrot_3");
        for (int i = 0; i < 4; i++)
        {
            string spritePath = filePath + i.ToString() + "/";
            mUIFacade.GetSprite(spritePath + "BG_Left");
            mUIFacade.GetSprite(spritePath + "BG_Right");
            for (int j = 0; j < 6; j++)
            {
                mUIFacade.GetSprite(spritePath + "Level_" + j.ToString());
            }
            for (int j = 0; j < 13; j++)
            {
                mUIFacade.GetSprite(spritePath + "Tower/Tower_" + j.ToString());
            }
        }
    }

    public void UpdateMapUI(string spritePath)
    {
        img_BGLeft.sprite = mUIFacade.GetSprite(spritePath + "BG_Left");
        img_BGRight.sprite = mUIFacade.GetSprite(spritePath + "BG_Right");
        for (int i = 0; i < 5; i++)
        {
            levelContentImageGos.Add(CreateUIAndSet("Img_Level", levelContentTrans));
            //CreateUIAndSet("Img_Level", levelContentTrans); 
            levelContentImageGos[i].GetComponent<Image>().sprite = mUIFacade.GetSprite(spritePath + "Level_" + (i + 1).ToString());
            Stage stage = playerManager.unLockedNurmalModelLevelList[(currentBigLevelID - 1)*5 + i];
            levelContentImageGos[i].transform.Find("Img_Carrot").gameObject.SetActive(false);
            levelContentImageGos[i].transform.Find("Img_AllClear").gameObject.SetActive(false);

            if (stage.unLocked)
            {
                if (stage.mAllClear)
                {
                    levelContentImageGos[i].transform.Find("Img_AllClear").gameObject.SetActive(true);
                }
                if (stage.mCarrotState != 0)
                {
                    Image carrotImageGo = levelContentImageGos[i].transform.Find("Img_Carrrot").GetComponent<Image>();
                    carrotImageGo.gameObject.SetActive(true);
                    carrotImageGo.sprite = mUIFacade.GetSprite(filePath + "Carrot_" + stage.mCarrotState);
                }
                levelContentImageGos[i].transform.Find("Img_Lock").gameObject.SetActive(false);
                levelContentImageGos[i].transform.Find("Img_BG").gameObject.SetActive(false);
            }
            else
            {
                if (stage.mIsRewardLevel)
                {
                    levelContentImageGos[i].transform.Find("Img_Lock").gameObject.SetActive(false);
                    levelContentImageGos[i].transform.Find("Img_BG").gameObject.SetActive(false);
                    Image monsterPerImage = levelContentImageGos[i].transform.Find("Img_Monster").GetComponent<Image>();
                    monsterPerImage.sprite = mUIFacade.GetSprite("MonsterNest/Monster/Baby/" + currentBigLevelID.ToString());
                    monsterPerImage.SetNativeSize();
                    monsterPerImage.transform.localScale = new Vector3(2, 2, 1);
                }
                else
                {
                    levelContentImageGos[i].transform.Find("Img_Lock").gameObject.SetActive(true);
                    levelContentImageGos[i].transform.Find("Img_BG").gameObject.SetActive(false);
                }
            }
        }

        //设置Content的大小
        slideMove.SetContentLength(5);
    }

    private void DestoryMapUI()
    {
        if (levelContentImageGos.Count > 0)
        {
            for (int i = 0; i < 5; i++)
            {
                mUIFacade.PushGameObjectToFactor(FactoryType.UIFactory, "Img_Level",levelContentImageGos[i]);
            }
            slideMove.InitScrollLength();
            levelContentImageGos.Clear();
        }
    }
    //更新静态UI
    public void UpdateLevelUI(string SpritePath)
    {
        if (towerContentImageGos.Count!=0)
        {
            for (int i = 0; i < towerContentImageGos.Count; i++)
            {
                towerContentImageGos[i].GetComponent<Image>().sprite = null;
                mUIFacade.PushGameObjectToFactor(FactoryType.UIFactory, "Img_Tower", towerContentImageGos[i]);
            }
            towerContentImageGos.Clear();
        }

        Stage stage = playerManager.unLockedNurmalModelLevelList[(currentBigLevelID - 1) * 5 + currentLevelID - 1];
        if (stage.unLocked)
        {
            img_LockBtnGo.SetActive(false);
        }
        else
        {
            img_LockBtnGo.SetActive(true);
        }
        tex_TotalWaves.text = stage.mToTalRound.ToString();
        for (int i = 0; i < stage.mTowerIDListLength; i++)
        {
            towerContentImageGos.Add(CreateUIAndSet("Img_Tower", emp_TowerTrans));
            towerContentImageGos[i].GetComponent<Image>().sprite = mUIFacade.GetSprite(filePath + "Tower/Tower_" + stage.mTowerIDList[i].ToString());
        }
    }

    public void ToThisPanel(int currentBigLevel)
    {
        currentBigLevelID = currentBigLevel;
        currentLevelID = 1;
        EnterPanel();
    }

    public override void InitPanel()
    {
        base.InitPanel();
        gameObject.SetActive(false);
    }

    public override void EnterPanel()
    {
        base.EnterPanel();
        gameObject.SetActive(true);
        theSpritePath = filePath + currentBigLevelID.ToString() + "/";
        DestoryMapUI();
        UpdateMapUI(theSpritePath);
        UpdateLevelUI(theSpritePath);
        slideMove.Init();
    }
    public override void UpdatePanel()
    {
        base.UpdatePanel();
        string theSpritePath = filePath + currentBigLevelID.ToString() + "/";
        UpdateLevelUI(theSpritePath);

    }

    public override void ExitPanel()
    {
        base.ExitPanel();
        gameObject.SetActive(false);
    }

    public void ToGamePanel()
    {
        GameManager.Instance.CurrentStage = playerManager.unLockedNurmalModelLevelList[(currentBigLevelID - 1) * 5 + currentLevelID - 1];
        mUIFacade.currentScenePanelDict[StringManager.GameLoadPanel].EnterPanel();
        mUIFacade.ChangeSceneState(new NormalModelSceneState(mUIFacade));
    }

    public GameObject CreateUIAndSet(string uiName,Transform parentTrans)
    {
        GameObject itemGo = mUIFacade.GetGameObject(FactoryType.UIFactory, uiName);
        itemGo.transform.SetParent(parentTrans);
        itemGo.transform.localPosition = Vector3.zero;
        itemGo.transform.localScale = Vector3.one;
        return itemGo;
    }

    public void ToNextLevel()
    {
        currentLevelID++;
        UpdatePanel();
    }
    public void ToLastLevel()
    {
        currentLevelID--;
        UpdatePanel();
    }
}
