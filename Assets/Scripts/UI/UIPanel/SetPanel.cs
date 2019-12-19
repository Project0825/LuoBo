using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SetPanel : BasePanel {

    private GameObject optionPageGo;
    private GameObject statisticePageGo;
    private GameObject producerPageGo;
    private GameObject panelResetGo;

    private Tween setPanelTween;
    private bool playBGMusic = true;
    private bool playEffectMusic = true;
    public Sprite[] btnSprites;//0.音效开，1，音效关，2，背景音乐开，3背景音乐关
    private Image img_Btn_EffectAudio;
    private Image img_Btn_BGAudio;
    public Text[] statisticeTexts;

    protected override void Awake()
    {
        base.Awake();
        setPanelTween = transform.DOLocalMoveX(0, 0.5f);
        setPanelTween.SetAutoKill(false);
        setPanelTween.Pause();
        optionPageGo = transform.Find("OptionPage").gameObject;
        statisticePageGo = transform.Find("StatisticsPage").gameObject;
        producerPageGo = transform.Find("ProducerPage").gameObject;
        img_Btn_EffectAudio = optionPageGo.transform.Find("Btn_EffectAudio").GetComponent<Image>();
        img_Btn_BGAudio = optionPageGo.transform.Find("Btn_BGAudio").GetComponent<Image>();
        panelResetGo = transform.Find("Panel_Reset").gameObject;
        //InitPanel();
    }

    public override void InitPanel()
    {
        transform.localPosition = new Vector3(-1920, 0, 0);
        transform.SetSiblingIndex(2);
    }
     
    public void ShowOptionPage()
    {
        optionPageGo.SetActive(true);
        statisticePageGo.SetActive(false);
        producerPageGo.SetActive(false);
    }
    public void ShowStatisticsPage()
    {
        optionPageGo.SetActive(false);
        statisticePageGo.SetActive(true);
        producerPageGo.SetActive(false);
        ShowStatistics();
    }
    public void ShowProducerPage()
    {
        optionPageGo.SetActive(false); 
        statisticePageGo.SetActive(false);
        producerPageGo.SetActive(true);
    }
    //场景进入和退出时的处理
    public override void EnterPanel()
    {
        ShowOptionPage();
        MoveToCenter();
    }
    public override void ExitPanel()
    {
        setPanelTween.PlayBackwards();
        mUIFacade.currentScenePanelDict[StringManager.MainPanel].EnterPanel();
        InitPanel();
    }
    public void MoveToCenter()
    {
        setPanelTween.PlayForward();
    }

    //音效按钮 的逻辑处理
    public void CloseOrOpenBGMusic()
    {
        playBGMusic = !playBGMusic;
        mUIFacade.CloseOrOpenBGMusic();
        if (playBGMusic)
        {
            img_Btn_BGAudio.sprite = btnSprites[2];
        }
        else
        {
            img_Btn_BGAudio.sprite = btnSprites[3];
        }
    }
    public void CloseOrOpenEffectMusic()
    {
        playEffectMusic = !playEffectMusic;
        mUIFacade.CloseOrOpenEffectMusic();
        if (playEffectMusic)
        {
            img_Btn_EffectAudio.sprite = btnSprites[0]; 
        }
        else
        {
            img_Btn_EffectAudio.sprite = btnSprites[1];
        }
    }

    //统计数据显示
    public  void ShowStatistics()
    {
        PlayerManager playerManager = mUIFacade.mPlayManager;
        statisticeTexts[0].text = playerManager.AdventrueModelNum.ToString();
        statisticeTexts[1].text = playerManager.BurriedLevelNum.ToString();
        statisticeTexts[2].text = playerManager.BossModelNum.ToString();
        statisticeTexts[3].text = playerManager.Coin.ToString();
        statisticeTexts[4].text = playerManager.KillMonsterNum.ToString();
        statisticeTexts[5].text = playerManager.KillBossNum.ToString();
        statisticeTexts[6].text = playerManager.ClearItemNum.ToString();

    }
    //重置游戏数据
    public void ResetGame()
    {

    }
    public void ShowResetPanel()
    {
        panelResetGo.SetActive(true);
    }
    public void CloseResetPanel()
    {
        panelResetGo.SetActive(false);
    }
}
