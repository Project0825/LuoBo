using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MainPanel : BasePanel {
    private Animator carrotAnimator;
    private Transform monsterTrans;
    private Transform cloudTrans;

    private Tween[] mainPanelTween;//0 向右， 1 向左
    private Tween exitTween;

    protected override void Awake()
    {
        base.Awake();
        transform.SetSiblingIndex(8);
        carrotAnimator = transform.Find("Emp_Carrot").GetComponent<Animator>();
        carrotAnimator.Play("CarrotGrow");
        monsterTrans = transform.Find("Img_Monster");
        cloudTrans = transform.Find("Img_Cloud");

        mainPanelTween = new Tween[2];
        mainPanelTween[0] = transform.DOLocalMoveX(1920, 0.5f);
        mainPanelTween[0].SetAutoKill(false);
        mainPanelTween[0].Pause();
        mainPanelTween[1] = transform.DOLocalMoveX(-1920, 0.5f);
        mainPanelTween[1].SetAutoKill(false);
        mainPanelTween[1].Pause();
        playUITween();
    }

    public override void EnterPanel()
    {
        transform.SetSiblingIndex(8);
        carrotAnimator.Play("CarrotGrow");
        if (exitTween != null)
        {
            exitTween.PlayBackwards();
        }
        cloudTrans.gameObject.SetActive(true);
    }
    public override void ExitPanel()
    {
        exitTween.PlayForward();
        cloudTrans.gameObject.SetActive(false);

    }

    private void playUITween()
    {
        monsterTrans.DOLocalMoveY(550, 2f).SetLoops(-1, LoopType.Yoyo);
        cloudTrans.DOLocalMoveX(1300, 6f).SetLoops(-1, LoopType.Restart);
    }
    public void MoveToRight()
    {
        exitTween = mainPanelTween[0];
        mUIFacade.currentScenePanelDict[StringManager.SetPanel].EnterPanel();
        
    }
    public void MoveToLeft()
    {
        exitTween = mainPanelTween[1];
        mUIFacade.currentScenePanelDict[StringManager.HelpPanel].EnterPanel();
    }

    public void ToNormalModelScene()
    {
        mUIFacade.currentScenePanelDict[StringManager.GameLoadPanel].EnterPanel();
        mUIFacade.ChangeSceneState(new NormalGameOptionSceneState(mUIFacade));
    }
    public void ToBossModelScene()
    {
        mUIFacade.currentScenePanelDict[StringManager.GameLoadPanel].EnterPanel();
        mUIFacade.ChangeSceneState(new BossOptionSceneState(mUIFacade));
    }
    public void ToMonsterNest()
    {
        mUIFacade.currentScenePanelDict[StringManager.GameLoadPanel].EnterPanel();
        mUIFacade.ChangeSceneState(new MonsterNestSceneState(mUIFacade));
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
