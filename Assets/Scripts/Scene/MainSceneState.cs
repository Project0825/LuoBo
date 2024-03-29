﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;using UnityEngine.SceneManagement;

public class MainSceneState : BaseSceneState {
    public MainSceneState(UIFacade uiFacade) : base(uiFacade)
    {

    }
    public override void EnterScene()
    {
        mUIFacade.AddPanelToDict(StringManager.MainPanel);
        mUIFacade.AddPanelToDict(StringManager.SetPanel);
        mUIFacade.AddPanelToDict(StringManager.HelpPanel);
        mUIFacade.AddPanelToDict(StringManager.GameLoadPanel);

        base.EnterScene();

    }
    public override void ExitScene()
    {
        base.ExitScene();
        //当前对象的类 的 类型
        if (mUIFacade.currentSceneState.GetType()==typeof(NormalGameOptionSceneState))
        {
            SceneManager.LoadScene(2);
        }
        else if (mUIFacade.currentSceneState.GetType() == typeof(BossOptionSceneState))
        {
            SceneManager.LoadScene(4);
        }
        else
        {
            SceneManager.LoadScene(6);
        }
    }

}
