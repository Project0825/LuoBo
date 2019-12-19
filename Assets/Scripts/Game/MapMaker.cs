using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class MapMaker : MonoBehaviour
{
#if Tool
    public bool DrawLine;
    public GameObject GridGo;

    private static MapMaker _instance;
    public static MapMaker Instance
    {
        get
        {
            return _instance;
        }
    }
#endif

    private float mapWidth;
    private float mapHeight;
    [HideInInspector]
    public float gridWidth;
    [HideInInspector]
    public float gridHeight;
    [HideInInspector]
    public int bigLevelID;
    [HideInInspector]
    public int levelID;

    public const int yRaw = 8;
    public const int xColumn = 12;
    //全部的格子对象
    public GridPoint[,] gridPoints;
    [HideInInspector]
    public List<GridPoint.GridIndex> monsterPath;
    [HideInInspector] 
    public List<Vector3> monsterPathPos;

    private SpriteRenderer bgSR;
    private SpriteRenderer roadSR;
    [HideInInspector]
    public List<Round.RoundInfo> roundInfoList;
    [HideInInspector]
    public Carrot carrot;
    
    private void Awake()
    {
#if Tool
        _instance = this;
        InitMapMaker();
#endif
    }

    //初始化地图
    public void InitMapMaker()
    {
        CalculateSize();
        gridPoints = new GridPoint[xColumn, yRaw];
        monsterPath = new List<GridPoint.GridIndex>();
        for (int x = 0; x < xColumn; x++)
        {
            for (int y = 0; y < yRaw; y++)
            {
#if Tool
                GameObject itemGo = Instantiate(GridGo, transform.position, transform.rotation);
#endif
#if Game
                GameObject itemGo = GameController.Instance.GetGameObjectResources("Grid");
#endif
                itemGo.transform.position = CorretPostion(x * gridWidth, y * gridHeight);
                itemGo.transform.SetParent(transform);
                gridPoints[x, y] = itemGo.GetComponent<GridPoint>();
                gridPoints[x, y].gridIndex.xIndex = x;
                gridPoints[x, y].gridIndex.yIndex = y;
            }
        }
        bgSR = transform.Find("BG").GetComponent<SpriteRenderer>();
        roadSR = transform.Find("Road").GetComponent<SpriteRenderer>();

    }
#if Game
    public void LoadMap(int bigLevel,int level)
    {
        bigLevelID = bigLevel;
        levelID = level;
        LoadLevelFile(LoadLevelInfoFile("Level" + bigLevelID.ToString()+"_"+levelID.ToString()+".json"));
        monsterPathPos = new List<Vector3>();
        for (int i = 0; i < monsterPath.Count; i++)
        {
            monsterPathPos.Add(gridPoints[monsterPath[i].xIndex, monsterPath[i].yIndex].transform.position);
        }

        GameObject startPointGo = GameController.Instance.GetGameObjectResources("startPoint");
        startPointGo.transform.transform.position = monsterPathPos[0];
        startPointGo.transform.SetParent(transform);

        GameObject endPointGo = GameController.Instance.GetGameObjectResources("Carrot");
        endPointGo.transform.transform.position = monsterPathPos[monsterPathPos.Count-1];
        endPointGo.transform.SetParent(transform);
        carrot = endPointGo.GetComponent<Carrot>();
    }

#endif


    public Vector3 CorretPostion(float x,float y)
    {
        return new Vector3(x - mapWidth / 2 + gridWidth / 2,y-mapHeight/2+gridHeight/2);
    }

    private void CalculateSize()
    {
        Vector3 leftDown = new Vector3(0, 0);
        Vector3 rightUp = new Vector3(1, 1);

        Vector3 worldLeftDown = Camera.main.ViewportToWorldPoint(leftDown);
        Vector3 worldRightUp = Camera.main.ViewportToWorldPoint(rightUp);

        mapWidth = worldRightUp.x - worldLeftDown.x;
        mapHeight = worldRightUp.y - worldLeftDown.y;

        gridWidth = mapWidth / xColumn;
        gridHeight = mapHeight / yRaw;
    }
#if Tool
    private void OnDrawGizmos()
    {
        if (DrawLine)
        {
            CalculateSize();
            Gizmos.color = Color.red;

            for (int y = 0; y <= yRaw; y++)
            {
                Vector3 startPos = new Vector3(-mapWidth / 2, -mapHeight / 2 + gridHeight * y);
                Vector3 endPos = new Vector3(mapWidth / 2, -mapHeight / 2 + gridHeight * y);
                Gizmos.DrawLine(startPos, endPos);
            }

            for (int x = 0; x <= xColumn; x++)
            {
                Vector3 startPos = new Vector3(-mapWidth / 2 + gridWidth * x, mapHeight / 2);
                Vector3 endPos = new Vector3(-mapWidth / 2 + gridWidth * x, -mapHeight / 2);
                Gizmos.DrawLine(startPos, endPos);
            }
        }
    }
#endif

    /// <summary>
    /// 地图编辑器的扩展方法
    /// </summary>

    //清楚怪物路径
    public void ClearMonterPath()
    {
        monsterPath.Clear();
    }
    //地图编辑 默认状态
    public void RecoverTowerPoint()
    {
        ClearMonterPath();
        for (int x = 0; x < xColumn; x++)
        {
            for (int y = 0; y < yRaw; y++)
            {
                gridPoints[x, y].InitGrid();
            }
        }
    }
    //初始化地图
    public void InitMap()
    {
        bigLevelID = 0;
        levelID = 0;
        RecoverTowerPoint();
        roundInfoList.Clear();
        bgSR.sprite = null;
        roadSR.sprite = null;
    }
#if Tool
    //生成LevelInfo对象 保存文件
    private LevelInfo createLevelInfoGo()
    {
        LevelInfo levelInfo = new LevelInfo {
            BigLevelID = bigLevelID,
            LevelID = levelID,
        };
        levelInfo.gridPoints = new List<GridPoint.GridState>();
        for (int x = 0; x < xColumn; x++)
        {
            for (int y = 0; y < yRaw; y++)
            {
                levelInfo.gridPoints.Add(gridPoints[x, y].gridState);
            }
        }
        levelInfo.monsterPath = new List<GridPoint.GridIndex>();
        for (int i = 0; i < monsterPath.Count; i++)
        {
            levelInfo.monsterPath.Add(monsterPath[i]);
        }
        levelInfo.roundInfo = new List<Round.RoundInfo>();
        for (int i = 0; i < roundInfoList.Count; i++)
        {
            levelInfo.roundInfo.Add(roundInfoList[i]);

        }
        Debug.Log("保存成功");
        return levelInfo;
    }

    public void SaveLevelFileByJson()
    {
        LevelInfo levelInfoObj = createLevelInfoGo();
        string filePath = Application.dataPath + "/Resources/Json/Level/" + "Level" + bigLevelID.ToString() + "_" + levelID.ToString() + ".json";
        string saveJsonStr = JsonMapper.ToJson(levelInfoObj);
        StreamWriter sw = new StreamWriter(filePath);
        sw.Write(saveJsonStr); 
        sw.Close();
    }
#endif
    public LevelInfo LoadLevelInfoFile(string fileName)
    {
        LevelInfo levelInfo = new LevelInfo();
        string filePath = Application.dataPath + "/Resources/Json/Level/" + fileName;
        if (File.Exists(filePath))
        {
            StreamReader sr = new StreamReader(filePath);
            string jsonStr = sr.ReadToEnd();
            sr.Close();
            levelInfo = JsonMapper.ToObject<LevelInfo>(jsonStr);
            return levelInfo;
        }
            Debug.Log("文件加载失败");
            return null;

    }


    public void LoadLevelFile(LevelInfo levelInfo)
    {
        bigLevelID = levelInfo.bigLevelID;
        levelID = levelInfo.levelID;
        for (int x = 0; x < xColumn; x++)
        {
            for (int y = 0; y < yRaw; y++)
            {
                gridPoints[x, y].gridState = levelInfo.gridPoints[y+x*yRaw];
                gridPoints[x, y].UpdateGrid();

            }
        }
        monsterPath.Clear();
        for (int i = 0; i < levelInfo.monsterPath.Count; i++)
        {
            monsterPath.Add(levelInfo.monsterPath[i]);
        }
        roundInfoList = new List<Round.RoundInfo>();
        for (int i = 0; i < levelInfo.roundInfo.Count; i++)
        {
            roundInfoList.Add(levelInfo.roundInfo[i]);
        }
        bgSR.sprite = Resources.Load<Sprite>("Pictures/NormalMordel/Game/" + bigLevelID.ToString() + "/" + "BG" + (levelID / 3).ToString());
        roadSR.sprite = Resources.Load<Sprite>("Pictures/NormalMordel/Game/" + bigLevelID.ToString() + "/" + "Road" + levelID);

        
    }


}
 