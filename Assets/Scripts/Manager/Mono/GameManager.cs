using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏总管理
/// </summary>
public class GameManager : MonoBehaviour {
    [HideInInspector]
    public PlayerManager playerManager;
    public FactoryManager factoryManager;
    public AudioManager audioManager;

    [HideInInspector]
    public UIManager uiManager;

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public Stage CurrentStage;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _instance = this;
        playerManager = new PlayerManager();
        factoryManager = new FactoryManager();
        audioManager = new AudioManager();
        uiManager = new UIManager();
        uiManager.mUIFacade.currentSceneState.EnterScene();

    }
    public GameObject CreatItem(GameObject itemGo)
    {
        GameObject go = Instantiate(itemGo);
        return go;
    }
    //获取图片资源
    public Sprite GetSprite(string resourcePath)
    {
        return factoryManager.spriteFactory.GetSingleResources(resourcePath);
    }
    public AudioClip GetAudioClip(string resourcePath)
    {
        return factoryManager.audioClipFactory.GetSingleResources(resourcePath);
    }
    public RuntimeAnimatorController GetRuntimeAnimatorController(string resourcePath)
    {
        return factoryManager.runtimeAnimControllerFactory.GetSingleResources(resourcePath);
    }

    public GameObject GetGameObject(FactoryType factoryType, string resourcePath)
    {
        return factoryManager.factoryDict[factoryType].GetItem(resourcePath);
    }
    public void PushGameObjectToFactor(FactoryType factoryType, string resourcePath,GameObject ItemGo)
    {
        factoryManager.factoryDict[factoryType].PushItem(resourcePath, ItemGo);
    }
}
