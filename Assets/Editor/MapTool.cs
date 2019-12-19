using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

#if Tool
[CustomEditor(typeof(MapMaker))]
public class MapTool : Editor {
    private MapMaker mapMaker;
    //关卡文件列表
    private List<FileInfo> fileList = new List<FileInfo>();
    private string[] fileNameList;
    //当前编辑的关卡
    private int selectIndex = -1;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (Application.isPlaying)
        {
            mapMaker = MapMaker.Instance;
            EditorGUILayout.BeginHorizontal();
            fileNameList = getName(fileList);
            int currentIndex = EditorGUILayout.Popup(selectIndex, fileNameList);
            if (currentIndex !=selectIndex)
            {
                selectIndex = currentIndex;
                //实例化地图的方法
                mapMaker.InitMap();
                //加载当前选择的Leve文件
                mapMaker.LoadLevelFile(mapMaker.LoadLevelInfoFile(fileNameList[selectIndex]));
            }
            if (GUILayout.Button("读取关卡列表"))
            {
                loadLevelFiles();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("恢复默认状态"))
            {
                mapMaker.RecoverTowerPoint();
            };
            if (GUILayout.Button("清除怪物路径点"))
            {
                mapMaker.ClearMonterPath();
            };
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("SaveJson"))
            {
                mapMaker.SaveLevelFileByJson();
            };

        }
    }
    //加载关卡文件
    private void loadLevelFiles()
    {
        clearList();
        fileList = getLevelLists();
    }
    //清除文件列表
    private void clearList()
    {
        fileList.Clear();
        selectIndex = -1;
    }
    //
    private List<FileInfo> getLevelLists()
    {
       // string[] files = Directory.GetFiles(Application.dataPath + "/Resources/Json/Level/", "*.json");
        string[] files = Directory.GetFiles(Application.dataPath + "/Resources/Json/Level/", "*.json");

        List<FileInfo> list = new List<FileInfo>();
        for (int i = 0; i < files.Length; i++)
        {
            FileInfo file = new FileInfo(files[i]);
            list.Add(file);
        }
        return list;
    }

    //获取关卡文件的名字
    private string[] getName(List<FileInfo> files)
    {
        List<string> names = new List<string>();
        foreach (FileInfo file in files)
        {
            names.Add(file.Name);
        }
        return names.ToArray();
    }
}
#endif