using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBuilder : IBuilder<Monster>
{
    public int m_monsterID;
    private GameObject monsterGo;


    public void GetData(Monster productClass)
    {
        productClass.MonsterID = m_monsterID;
        productClass.Hp = m_monsterID * 100;
        productClass.CurrentHp = productClass.Hp;
        productClass.MoveSpeed = m_monsterID;
        productClass.Prize = m_monsterID * 50;

    }

    public void GetOtherResource(Monster productClass)
    {
        productClass.GetMonsterProperty();
    }

    public GameObject GetProduct()
    {
        GameObject itemGo = GameController.Instance.GetGameObjectResources("MonsterPrefab");
        Monster monster = GetProductClass(itemGo);
        GetData(monster);
        GetOtherResource(monster);
        return itemGo;
    }

    public Monster GetProductClass(GameObject gameObject)
    {
        return gameObject.GetComponent<Monster>();
    }
}
