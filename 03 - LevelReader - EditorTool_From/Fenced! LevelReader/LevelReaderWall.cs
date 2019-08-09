using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

public class LevelReaderWall : EditorWindow
{
    //Single fields
    public TextAsset CsvFile;
    public string ScriptableObjectName;
    public bool CreateNewScriptableObject;

    //Multi Fields
    public TextAsset[] CsvFiles;
    public string ScriptableObjectBaseName;
    public int ScriptableObjectNameFirstNumber;

    //Fields
    public static string ParentFolderPath;
    private LevelDataWall LevelData;
    private PrefabSettingsWall prefabSettings;


    protected static bool UseSingle;

    [MenuItem("Assets/Create/LevelConversionWall/LevelReaderSingle")]
    protected static void InitSingle()
    {
        UseSingle = true;
        Rect posAndSize = new Rect(0, 0, 600, 130);
        EditorWindow window = GetWindowWithRect(typeof(LevelReaderWall), posAndSize);
        window.titleContent.text = "LevelConverterCSVtoScriptable";
        ParentFolderPath = AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID());
        window.Show();
    }

    [MenuItem("Assets/Create/LevelConversionWall/LevelReaderMulti")]
    protected static void InitMulti()
    {
        UseSingle = false;
        Rect posAndSize = new Rect(0, 0, 600, 130);
        EditorWindow window = GetWindowWithRect(typeof(LevelReaderWall), posAndSize);
        window.titleContent.text = "LevelConverterCSVtoScriptableMulti";
        ParentFolderPath = AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID());
        window.Show();
    }

    void OnGUI()
    {
        EditorWindow window = this;
        prefabSettings = (PrefabSettingsWall)EditorGUILayout.ObjectField(
            "Prefab Settings Wall:", prefabSettings, typeof(PrefabSettingsWall), false);


        if (UseSingle)
        {

            GUILayout.Label("Convert CSV level files to Target type");

            CsvFile = (TextAsset)EditorGUILayout.ObjectField(
                "SourceCSV: ", CsvFile, typeof(TextAsset), false);

            LevelData = (LevelDataWall)EditorGUILayout.ObjectField(
                "Existing Target:", LevelData, typeof(LevelDataWall), false);

            CreateNewScriptableObject =
                EditorGUILayout.Toggle("CreateNewTarget:", CreateNewScriptableObject);
            ScriptableObjectName = EditorGUILayout.TextField("NewSOName", ScriptableObjectName);

            if (GUILayout.Button("CONVERT"))
            {
                LevelDataWallContainer container = ParseCsv(CsvFile);

                if (CreateNewScriptableObject)
                {
                    LevelData = (LevelDataWall)ScriptableObject.CreateInstance(typeof(LevelDataWall));

                    AssetDatabase.CreateAsset(LevelData,
                                              $"{ParentFolderPath}/{ScriptableObjectName}.asset");
                }

                SetTempContainerToScriptable(LevelData, container);
                EditorUtility.SetDirty(LevelData);

                this.Close();
            }
        }
        else
        {
            GUILayout.Label("Convert list of CSV files to Target type ");
            ScriptableObjectBaseName =
                EditorGUILayout.TextField("TargetBaseName", ScriptableObjectBaseName);

            ScriptableObjectNameFirstNumber =
                EditorGUILayout.IntField("StartNumberingFrom...", ScriptableObjectNameFirstNumber);

            ScriptableObject target = this;
            SerializedObject so = new SerializedObject(target);
            SerializedProperty CsvFilesProperty = so.FindProperty("CsvFiles");
            EditorGUILayout.PropertyField(CsvFilesProperty, true);
            so.ApplyModifiedProperties();


            if (GUILayout.Button("CONVERT"))
            {
                int i = 0;
                foreach (var file in CsvFiles)
                {
                    LevelDataWallContainer container = ParseCsv(file);

                    LevelData = (LevelDataWall)ScriptableObject.CreateInstance(typeof(LevelDataWall));

                    AssetDatabase.CreateAsset(LevelData,
                                              $"{ParentFolderPath}/{ScriptableObjectBaseName}{ScriptableObjectNameFirstNumber + i}.asset");

                    SetTempContainerToScriptable(LevelData, container);
                    i++;
                }

                this.Close();
            }
        }
    }

    public void SetTempContainerToScriptable(LevelDataWall scriptable, LevelDataWallContainer container)
    {
        scriptable.Tiles = container.TileList.ToArray();
        scriptable.Walls = container.WallList.ToArray();
        scriptable.Size = container.Data1;
        scriptable.StartPositionX = container.Data2.Item1;
        scriptable.StartPositionY = container.Data2.Item2;
        EditorUtility.SetDirty(scriptable);
    }

    public LevelDataWallContainer ParseCsv(TextAsset csvFile)
    {
        char splitter = ';';
        LevelDataWallContainer cont = new LevelDataWallContainer();
        string[] lines = csvFile.text.Split('\n');
        cont.Data1 = int.Parse(lines[0].Split(splitter)[1]); // Size
        string[] line1Arr = lines[1].Split(';');
        cont.Data2 = new Tuple<int, int>(int.Parse(line1Arr[1]), int.Parse(line1Arr[2]));

        char[] levelObjectChars = prefabSettings.TileCharList;
        char[] wallChars = prefabSettings.WallCharList;

        int wallSectionStart = 2 + cont.Data1;
        int wallSectionEnd = wallSectionStart + 1 + cont.Data1 * 2;

        for (int i = 2; i < wallSectionStart; i++)
        {
            string[] aLine = lines[i].Split(splitter);
            for (int j = 0; j<cont.Data1; j++)
            {
                if (string.IsNullOrEmpty(aLine[j]))
                {
                    throw new Exception("Empty block in Tile section J="+j);
                }
                char c = aLine[j].ToCharArray()[0];
                for (int k = 0; levelObjectChars.Length > k; k++)
                {
                    if (c == levelObjectChars[k])
                    {
                        cont.TileList.Add(k);
                    }
                }
            }
        }

        for (int i = wallSectionStart; i < wallSectionEnd; i++)
        {
            string[] aLine = lines[i].Split(splitter);
            for (int j = 0; j < cont.Data1+1; j++)
            {
                if (string.IsNullOrEmpty(aLine[j]))
                {
                    cont.WallList.Add(-1);
                }
                else
                {
                    char c = aLine[j].ToCharArray()[0];
                    for (int k = 0; wallChars.Length > k; k++)
                    {
                        if (c == wallChars[k])
                        {
                            cont.WallList.Add(k);
                        }
                    }
                }
            }
        }

        return cont;
    }
}

public class LevelDataWallContainer 
{
    public int Data1;
    public Tuple<int, int> Data2;
    public List<int> TileList;
    public List<int> WallList;

    public LevelDataWallContainer()
    {
        TileList = new List<int>();
        WallList = new List<int>();
    }

    public void DebugList()
    {
        foreach (var t in TileList)
        {
            Debug.Log(t.ToString());
        }
    }
}