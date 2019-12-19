using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager {

    public UIFacade mUIFacade;
    public Dictionary<string, GameObject> currentScenePanelDict;
    private GameManager mGameManager;
    public UIManager()
    {
        mGameManager = GameManager.Instance;
        currentScenePanelDict = new Dictionary<string, GameObject>();
        mUIFacade = new UIFacade(this);
        mUIFacade.currentSceneState = new StartLoadScene(mUIFacade);
    }
    public void PushUIPanel(string uiPanelName,GameObject uiPanelGo)
    {
        mGameManager.PushGameObjectToFactor(FactoryType.UIPanelFactory, uiPanelName, uiPanelGo);
    }

    public void ClearDict()
    {
        foreach (var item in currentScenePanelDict)
        {
            //减去(clone) 字段
            PushUIPanel(item.Value.name.Substring(0,item.Value.name.Length-7), item.Value);
        }
        currentScenePanelDict.Clear();
    } 
}
