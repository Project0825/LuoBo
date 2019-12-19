using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 动画控制器
/// </summary>
public class RuntimeAnimControllerFactory : IBaseResourcesFactory<RuntimeAnimatorController> {
    protected Dictionary<string, RuntimeAnimatorController> factoryDict = new Dictionary<string, RuntimeAnimatorController>();
    protected string loadPath;

    public RuntimeAnimControllerFactory()
    {
        loadPath = "Animator/";
    }

    public RuntimeAnimatorController GetSingleResources(string resourcePath)
    {
        RuntimeAnimatorController itemGo = null;
        string itemLoadPath = loadPath + resourcePath;
        if (factoryDict.ContainsKey(resourcePath))
        {
            itemGo = factoryDict[resourcePath];

        }
        else
        {
            itemGo = Resources.Load<RuntimeAnimatorController>(itemLoadPath);
            factoryDict.Add(resourcePath, itemGo);
        }
        if (itemGo == null)
        {
            Debug.Log(resourcePath + "的资源获取失败，错误路径为:" + itemLoadPath);
        }
        return itemGo;
    }
}
