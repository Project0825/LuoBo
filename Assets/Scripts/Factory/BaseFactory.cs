using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFactory : IBaseFactory {
    
    //这里的游戏物体包含 UI UIPanel Game 资源模板
    protected Dictionary<string, GameObject> factoryDict = new Dictionary<string, GameObject>();
    //具体生成的游戏对象
    //对象池 字典
    protected Dictionary<string, Stack<GameObject>> objPoolDict = new Dictionary<string, Stack<GameObject>>();

    //加载路径
    protected string loadPath;
    public BaseFactory()
    {
        loadPath = "Prefabs/";
    }

    public void PushItem(string itemName, GameObject item)
    {
        item.SetActive(false);
        item.transform.SetParent(GameManager.Instance.transform);
        if (objPoolDict.ContainsKey(itemName))
        {
            objPoolDict[itemName].Push(item);
        }
        else
        {
            Debug.Log("对象池字典未包含:" + itemName + "的栈");
        }
    }
    /// <summary>
    /// 取实例
    /// </summary>
    /// <param name="itemName"></param>
    /// <returns></returns>
    public GameObject GetItem(string itemName)
    {
        GameObject itemGo = null;
        if (objPoolDict.ContainsKey(itemName))
        {
            if (objPoolDict[itemName].Count==0)
            {
                GameObject go = GetResource(itemName);
                itemGo = GameManager.Instance.CreatItem(go);
            }
            else
            {
                itemGo = objPoolDict[itemName].Pop();
                itemGo.SetActive(true);
            }
        }
        else
        {
            objPoolDict.Add(itemName, new Stack<GameObject>());
            GameObject go = GetResource(itemName);
            itemGo = GameManager.Instance.CreatItem(go);
        }
        if (itemGo == null)
        {
            Debug.Log(itemName + "的实例获取失败");
        }
        return itemGo;
    }
 
    /// <summary>
    /// 获取资源
    /// </summary>
    /// <param name="itemName"></param>
    /// <returns></returns>
    private  GameObject GetResource(string itemName)
    {
        GameObject itemGo = null;
        string itemLoadPath = loadPath + itemName;
        if (factoryDict.ContainsKey(itemName))
        {
            itemGo = factoryDict[itemName];
        }
        else
        {
            itemGo = Resources.Load<GameObject>(itemLoadPath);
            factoryDict.Add(itemName, itemGo);
        }
        if (itemGo == null)
        {
            Debug.Log(itemName + "资源加载失败");
            Debug.Log("失败路径:" + itemLoadPath);
        }
        return itemGo;
    }
}
