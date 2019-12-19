using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIFacade  {
    //上层的管理者
    private UIManager mUIManager;
    private GameManager mGameManager;
    private AudioManager mAudioManager;
    public PlayerManager mPlayManager;
    //下层UI面板
    public Dictionary<string, IBasePanel> currentScenePanelDict = new Dictionary<string, IBasePanel>();

    //其他成员变量
    private GameObject mask;
    private Image maskImage;
    public Transform canvasTransform;
    //场景状态
    public IBaseSceneState currentSceneState;
    public IBaseSceneState lastSceneState;


    public UIFacade(UIManager uiManager)
    {
        mGameManager = GameManager.Instance;
        mPlayManager = mGameManager.playerManager;
        mUIManager = uiManager;
        mAudioManager = mGameManager.audioManager;
        InitMask();
    }

    public void InitMask()
    {
        canvasTransform = GameObject.Find("Canvas").transform;
        //直接调用
        //mask = mGameManager.factoryManager.factoryDict[FactoryType.UIFactory].GetItem("Img_Mask");
        //调用GameManager的封装
        //mask = mGameManager.GetGameObject(FactoryType.UIFactory, "Img_Mask"); 
        //调用本身的封装
        //mask = GetGameObject(FactoryType.UIFactory, "Img_Mask");
        mask = CreateUI("Img_Mask");
        maskImage = mask.GetComponent<Image>();
    }

    //改变当前场景的状态
    public void ChangeSceneState(IBaseSceneState baseSceneState)
    {
        lastSceneState = currentSceneState;
        ShowMask();
        currentSceneState = baseSceneState;
    }

    //显示遮罩
    public void ShowMask()
    {
        //设置渲染层级
        mask.transform.SetSiblingIndex(10);
        Tween t = DOTween.To(() => maskImage.color, toColor => maskImage.color = toColor, new Color(0, 0, 0, 1), 1f);
        t.OnComplete(ExitSceneComplete);
    }
    //离开当前场景
    private void ExitSceneComplete()
    {
        lastSceneState.ExitScene();
        currentSceneState.EnterScene(); 
        HideMask();
    }
    //隐藏遮罩
    public void HideMask()
    {
        mask.transform.SetSiblingIndex(10);
        DOTween.To(() => maskImage.color, toColor => maskImage.color = toColor, new Color(0, 0, 0, 0), 1f);
    }

    //实例化当前场景所有的，并存入字典当中
    public void InitDict()
    {
        foreach (var item in mUIManager.currentScenePanelDict)
        {

            item.Value.transform.SetParent(canvasTransform);
            item.Value.transform.localPosition = Vector3.zero;
            item.Value.transform.localScale = Vector3.one;
            IBasePanel basePanel = item.Value.GetComponent<IBasePanel>();
            if(basePanel == null)
            {
                Debug.LogError(("获取IBasePanel脚本失败"));
            }
            basePanel.InitPanel();
            currentScenePanelDict.Add(item.Key, basePanel);
        }
    }
    //清空UIPanel字典
    public void ClearDict()
    {
        currentScenePanelDict.Clear();
        mUIManager.ClearDict();
    }


    //添加UIPanel到UIMaager的字典中
    public void AddPanelToDict(string uiPanelName)
    {
        mUIManager.currentScenePanelDict.Add(uiPanelName, GetGameObject(FactoryType.UIPanelFactory, uiPanelName));
    }

    /// <summary>
    /// 创建UI并对其位置和缩放进行初始化
    /// </summary>
    /// <param name="uiName"></param>
    /// <returns></returns>
    public GameObject CreateUI(string uiName)
    {
        GameObject itemGo=GetGameObject(FactoryType.UIFactory,uiName);
        itemGo.transform.SetParent(canvasTransform);
        itemGo.transform.localPosition = Vector3.zero;
        itemGo.transform.localScale = Vector3.one;
        return itemGo;
    }

    //基于GameManager 再次封装
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

    public GameObject GetGameObject(FactoryType factoryType, string resourcePath)
    {
        return mGameManager.GetGameObject(factoryType,resourcePath);
    }
    public void PushGameObjectToFactor(FactoryType factoryType, string resourcePath, GameObject ItemGo)
    {
        mGameManager.PushGameObjectToFactor(factoryType,resourcePath, ItemGo);
    }

    public void CloseOrOpenBGMusic()
    {
        mAudioManager.CloseOrOpenBGMusic();
    }
    public void CloseOrOpenEffectMusic()
    {
        mAudioManager.CloseOrOpenEffectMusic();
    }
}
