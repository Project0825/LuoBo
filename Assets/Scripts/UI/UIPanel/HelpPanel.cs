using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class HelpPanel : BasePanel {
    private GameObject helpPageGo;
    private GameObject monsterPageGo;
    private GameObject towerPageGo;
    private SlideMove slideMoveHelpPage;
    private SlideMove slideMoveTowerPage;


    private Tween helpPanelTween;

    protected override void Awake()
    {
        base.Awake();
        helpPageGo = transform.Find("HelpPage").gameObject;
        monsterPageGo = transform.Find("MonsterPage").gameObject;
        towerPageGo = transform.Find("TowerPage").gameObject;
        slideMoveHelpPage = transform.Find("HelpPage").Find("Scroll View").GetComponent<SlideMove>();
        slideMoveTowerPage = transform.Find("TowerPage").Find("Scroll View").GetComponent<SlideMove>();
        helpPanelTween = transform.DOLocalMoveX(0, 0.5f);
        helpPanelTween.SetAutoKill(false);
        helpPanelTween.Pause();
    }

    //显示页面的方法
    public void ShowHelpPage()
    {
        helpPageGo.SetActive(true);
        monsterPageGo.SetActive(false);
        towerPageGo.SetActive(false);
    }
    public void ShowMonsterPage()
    {
        helpPageGo.SetActive(false);
        monsterPageGo.SetActive(true);
        towerPageGo.SetActive(false);
    }
    public void ShowTowerPage()
    {
        helpPageGo.SetActive(false);
        monsterPageGo.SetActive(false);
        towerPageGo.SetActive(true);
    }

    public override void InitPanel()
    {
        base.InitPanel();
        transform.SetSiblingIndex(5);
        slideMoveTowerPage.Init();
        slideMoveHelpPage.Init();
        ShowHelpPage();
        if (transform.localPosition == Vector3.zero)
        {
            gameObject.SetActive(false);
            helpPanelTween.PlayBackwards();
        }
        transform.localPosition = new Vector3(1920, 0, 0);
    }
    public override void EnterPanel()
    {
        base.EnterPanel();
        gameObject.SetActive(true);
        slideMoveTowerPage.Init();
        slideMoveHelpPage.Init();
        MoveToCenter();
    }
    public override void ExitPanel()
    {
        base.ExitPanel();
        //在冒险模式 选择场景
        if (mUIFacade.currentSceneState.GetType()==typeof(NormalGameOptionSceneState))
        {
            mUIFacade.ChangeSceneState(new MainSceneState(mUIFacade));
        }
        else//如果是在主场景
        {
            helpPanelTween.PlayBackwards();
            mUIFacade.currentScenePanelDict[StringManager.MainPanel].EnterPanel();

        }
    }
    public void MoveToCenter()
    {
        helpPanelTween.PlayForward();
    }
}
