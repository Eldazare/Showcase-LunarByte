using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

public class LevelReader : EditorWindow
{
    //Single fields
    public TextAsset CsvFile;
    public string ScriptableObjectName;
    public bool CreateNewScriptableObject;

    //Multi Fields
    public  TextAsset[] CsvFiles;
    public  string      ScriptableObjectBaseName;
    public  int         ScriptableObjectNameFirstNumber;

    //Fields
    public static string ParentFolderPath;
    private LevelData LevelData;
    private PrefabSettings prefabSettings;


    protected static bool UseSingle;

    [MenuItem("Assets/Create/LevelConversion/LevelReaderSingle")]
    protected static void InitSingle()
    {
	UseSingle = true;
        Rect posAndSize = new Rect(0, 0, 600, 130);
        EditorWindow window = GetWindowWithRect(typeof(LevelReader), posAndSize);
        window.titleContent.text = "LevelConverterCSVtoScriptable";
        ParentFolderPath = AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID());
        window.Show();
    }

    [MenuItem("Assets/Create/LevelConversion/LevelReaderMulti")]
    protected static void InitMulti()
    {
	UseSingle = false;
	Rect posAndSize = new Rect(0, 0, 600, 130);
	EditorWindow window = GetWindowWithRect(typeof(LevelReader), posAndSize);
	window.titleContent.text = "LevelConverterCSVtoScriptableMulti";
	ParentFolderPath = AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID());
	window.Show();
    }

    void OnGUI()
    {
	EditorWindow window = this;
	prefabSettings = (PrefabSettings)EditorGUILayout.ObjectField
	(
		    "Prefab settings (char data):", 
		    prefabSettings, 
		    typeof(PrefabSettings), 
		    false
	);


        if (UseSingle)
	{

	    GUILayout.Label("Convert CSV level files to Target type");

	    CsvFile = (TextAsset) EditorGUILayout.ObjectField(
			    "SourceCSV: ", CsvFile, typeof(TextAsset), false);

	    LevelData = (LevelData) EditorGUILayout.ObjectField(
			    "Existing Target:", LevelData, typeof(LevelData), false);

	    CreateNewScriptableObject = EditorGUILayout.Toggle("CreateNewTarget:", CreateNewScriptableObject);
	    ScriptableObjectName = EditorGUILayout.TextField("NewSOName", ScriptableObjectName);

	    if (GUILayout.Button("CONVERT"))
	    {
		LevelDataContainerTemplate<int> container = ParseCsv(CsvFile);

		if (CreateNewScriptableObject)
		{
		    LevelData = (LevelData)ScriptableObject.CreateInstance(typeof(LevelData));

		    AssetDatabase.CreateAsset(LevelData, $"{ParentFolderPath}/{ScriptableObjectName}.asset");
		}

		LevelData.LevelObjects = container.ObjectList.ToArray();
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
		    LevelDataContainerTemplate<int> container = ParseCsv(file);

		    LevelData = (LevelData)ScriptableObject.CreateInstance(typeof(LevelData));

		    AssetDatabase.CreateAsset(LevelData,$"{ParentFolderPath}/{ScriptableObjectBaseName}{ScriptableObjectNameFirstNumber + i}.asset");

		    LevelData.LevelObjects = container.ObjectList.ToArray();
		    EditorUtility.SetDirty(LevelData);
		    i++;
		}

		this.Close();
	    }
        }
    }

    public LevelDataContainerTemplate<int> ParseCsv(TextAsset csvFile)
    {
        LevelDataContainerTemplate<int> cont = new LevelDataContainerTemplate<int>();
        string[] lines = csvFile.text.Split('\n');
        cont.Data1 = int.Parse(lines[0].Split(';')[1]);
        cont.Data2 = double.Parse(lines[1].Split(';')[1].Replace('.', ','));

	char[] chars = prefabSettings.LevelReaderCharList;

	StringBuilder singleLineBuilder = new StringBuilder();
        for (int i = lines.Length - 1; i > 1; i--)
        {
            singleLineBuilder.Append(ReplaceRulesLine(lines[i]));
        }


        foreach (char s in singleLineBuilder.ToString())
        {
	    for (int i = 0; chars.Length>i;i++)
	    {
		if (chars[i] == s)
		{
		    cont.ObjectList.Add(i);
		}
            }
        }

        return cont;
    }

    public static string ReplaceRulesLine(string line)
    {
        return line.Replace(",", "");
    }
}

public class LevelDataContainerTemplate<T> // Where T : enumType
{
    public int Data1;
    public double Data2;
    public List<T> ObjectList;

    public LevelDataContainerTemplate()
    {
        ObjectList = new List<T>();
    }

    public void DebugList()
    {
        foreach (T t in ObjectList)
        {
            Debug.Log(t.ToString());
        }
    }
}
