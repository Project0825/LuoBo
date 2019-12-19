using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class GridPoint : MonoBehaviour {

    private SpriteRenderer spriteRenderer;


    private Sprite gridSprite;
#if Tool
    private Sprite monsterPathSprite;
    public GameObject[] ItemPrefabs;
    public GameObject currentItem;
#endif 

    //格子状态
    public struct GridState
    {
        public bool canBuild;
        public bool isMonsterPoint;
        public bool hasItem;
        public int itemID;
    }
    //格子索引
    public struct GridIndex
    {
        public int xIndex;
        public int yIndex;
    }

    public GridState gridState;
    public GridIndex gridIndex;

    private void Awake()
    {
#if Tool
        gridSprite = Resources.Load<Sprite>("Pictures/NormalMordel/Game/Grid");
        monsterPathSprite = Resources.Load<Sprite>("Pictures/NormalMordel/Game/1/Monster/6-1");
        ItemPrefabs = new GameObject[10];
        string prefabsPath = "Prefabs/Game/"+MapMaker.Instance.bigLevelID.ToString()+"/item/";
        for (int i = 0; i < ItemPrefabs.Length; i++)
        {
            ItemPrefabs[i] = Resources.Load<GameObject>(prefabsPath + i);
            if (!ItemPrefabs[i])
            {
                Debug.Log("加载失败，失败路径;" + prefabsPath + i);
            }
        }
#endif
        spriteRenderer = GetComponent<SpriteRenderer>();
        InitGrid();

    }

    public void InitGrid()
    {
        gridState.canBuild = true;
        gridState.isMonsterPoint = false;
        gridState.hasItem = false;
        spriteRenderer.enabled = true;
        gridState.itemID = -1;
#if Tool
        spriteRenderer.sprite = gridSprite;
        Destroy(currentItem);
#endif
    }

#if Game
    public void UpdateGrid()
    {
        if (gridState.canBuild)
        {
            spriteRenderer.enabled = true;
            if (gridState.hasItem)
            {
                createItem();
            }
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }
    private void createItem()
    {
        GameObject itemGo = GameController.Instance.GetGameObjectResources(GameController.Instance.mapMaker.bigLevelID.ToString() + "/Item/" + gridState.itemID);

        itemGo.transform.SetParent(GameController.Instance.transform);

        Vector3 createPos = transform.position - new Vector3(0,0,3);
        if (gridState.itemID<=2)
        {
            createPos += new Vector3(GameController.Instance.mapMaker.gridWidth, -GameController.Instance.mapMaker.gridHeight)/2;
        }
        else if (gridState.itemID<=4)
        {
            createPos += new Vector3(GameController.Instance.mapMaker.gridWidth, 0) / 2;
        }
        itemGo.transform.position = createPos;
        itemGo.GetComponent<Item>().gridPoint = this;
    }

#endif

#if Tool
    private void OnMouseDown()
    {
        //怪物路径
        if (Input.GetKey(KeyCode.M))
        {
            gridState.canBuild = false;
            spriteRenderer.enabled = true;
            gridState.isMonsterPoint = !gridState.isMonsterPoint;
            if (gridState.isMonsterPoint)
            {
                MapMaker.Instance.monsterPath.Add(gridIndex);
                spriteRenderer.sprite = monsterPathSprite;
            }
            else
            {
                MapMaker.Instance.monsterPath.Remove(gridIndex);
                spriteRenderer.sprite = gridSprite;
                gridState.canBuild = true;
            }
        }
        //道具
        else if (Input.GetKey(KeyCode.I))
        {
            gridState.itemID++;
            if (gridState.itemID==ItemPrefabs.Length)
            {
                gridState.itemID = -1;
                Destroy(currentItem);
                gridState.hasItem = false;
                return;
            }
            if (currentItem ==null)
            {
                //
                CreateItem();
            }
            else
            {
                Destroy(currentItem);
                CreateItem();
            }
            gridState.hasItem = true;
        }
        else if(!gridState.isMonsterPoint)
        {
            gridState.isMonsterPoint = false;
            gridState.canBuild = !gridState.canBuild;
            if (gridState.canBuild)
            {
                spriteRenderer.enabled = true;
            }
            else
            {
                spriteRenderer.enabled = false;
            }
        }
    }

    //生成道具
    private void CreateItem()
    {
        Vector3 createPos = transform.position;
        if (gridState.itemID<=2)
        {
            createPos += new Vector3(MapMaker.Instance.gridWidth, -MapMaker.Instance.gridHeight)/2;
        }
        else if (gridState.itemID <= 4)
        {
            createPos += new Vector3(MapMaker.Instance.gridWidth,0) / 2;
        }
        GameObject itemGo = Instantiate(ItemPrefabs[gridState.itemID], createPos, Quaternion.identity);
        currentItem = itemGo;
    }
    //更新格子状态
    public void UpdataGrid()
    {
        if (gridState.canBuild)
        {
            spriteRenderer.sprite = gridSprite;
            spriteRenderer.enabled = true;
            if (gridState.hasItem)
            {
                CreateItem();
            }
        }
        else
        {
            if (gridState.isMonsterPoint)
            {
                spriteRenderer.sprite = monsterPathSprite;
            }
            else
            {
            spriteRenderer.enabled = false;

            }
        }
    }
#endif
}
