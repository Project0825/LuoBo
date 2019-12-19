using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameNormalBigLevelPanel : BasePanel {
    public Transform bigLevelContentTrans;
    public int BigLevelPageCount;
    private SlideMoveDan slideMove;
    private PlayerManager playerManager;
    private Transform[] bigLevelPage;

    private bool hasRigisterEvent;

    protected override void Awake()
    {
        base.Awake();
        playerManager = mUIFacade.mPlayManager;
        bigLevelPage = new Transform[BigLevelPageCount];
        slideMove = transform.Find("Scroll View").GetComponent<SlideMoveDan>();
        for (int i = 0; i < BigLevelPageCount; i++)
        {
            bigLevelPage[i] = bigLevelContentTrans.GetChild(i);
            ShowBigLevelState(playerManager.unLockedNurmalModelBigLevelList[i], playerManager.unLockedNurmalModelLevelNum[i],
                5, bigLevelPage[i], i + 1);
        }
        hasRigisterEvent = true;
    }
    private void OnEnable()
    {
        for (int i = 0; i < BigLevelPageCount; i++)
        {
            bigLevelPage[i] = bigLevelContentTrans.GetChild(i);
            ShowBigLevelState(playerManager.unLockedNurmalModelBigLevelList[i], playerManager.unLockedNurmalModelLevelNum[i],
                5, bigLevelPage[i], i + 1);
        }

    }
    public override void EnterPanel()
    {
        base.EnterPanel();
        slideMove.Init();
        gameObject.SetActive(true);
    }
    public override void ExitPanel()
    {
        base.ExitPanel();
        gameObject.SetActive(false);
    }
    //显示大关卡的信息
    public void ShowBigLevelState(bool unLocked,int unLockedLevelNum,int totalNum,Transform theBigLevelButtonTrans,int bigLevelID)
    {
        if (unLocked)//解锁
        {
            theBigLevelButtonTrans.Find("Img_Lock").gameObject.SetActive(false);
            theBigLevelButtonTrans.Find("Img_Page").gameObject.SetActive(true);
            theBigLevelButtonTrans.Find("Img_Page").Find("Tex_Page").GetComponent<Text>().text = unLockedLevelNum.ToString() + "/" + totalNum.ToString();
            Button theBigLevelButtonCom = theBigLevelButtonTrans.GetComponent<Button>();
            theBigLevelButtonCom.interactable = true;
            if (!hasRigisterEvent)//防止事件重复注册
            {
                theBigLevelButtonCom.onClick.AddListener(() =>
                {
                    mUIFacade.currentScenePanelDict[StringManager.GameNormalBigLevelPanel].ExitPanel();
                    //mUIFacade.currentScenePanelDict[StringManager.GameNormalLevelPanel].EnterPanel();
                    GameNormalLevelPanel gameNormalLevelPanel = mUIFacade.currentScenePanelDict[StringManager.GameNormalLevelPanel] as GameNormalLevelPanel;
                    gameNormalLevelPanel.ToThisPanel(bigLevelID);
                    GameNormalOptionPanel gameNormalOptionPanel = mUIFacade.currentScenePanelDict[StringManager.GameNormalOptionPanel] as GameNormalOptionPanel;
                    gameNormalOptionPanel.isInBigLevelPanel = false;
                });
            }
        }
        else
        {
            theBigLevelButtonTrans.Find("Img_Lock").gameObject.SetActive(true);
            theBigLevelButtonTrans.Find("Img_Page").gameObject.SetActive(false);
            theBigLevelButtonTrans.GetComponent<Button>().interactable = false;
        }
    }



    public void ToNextPage()
    {
        slideMove.ToNextPage();
    }
    public void ToLastPage()
    {
        slideMove.ToLastPage();
    }
}
