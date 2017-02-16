using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Sprites;

public class EnemyTool : EditorWindow
{
    public List<Enemies> emenyList = new List<Enemies>();
    public List<string> emenyNameList = new List<string>();
    public string[] emenyNameArray;

    int currentChoice = 0;
    int ourHealth;
    int lastRoyce;

    bool nameFlag = false;
    bool nameExists = false;

    string ourName = "";

    Sprite ourSprite;
    [MenuItem("Custom Tools/Enemy Tools %g")]
    static void BadThings()
    {
        EditorWindow.GetWindow<EnemyTool>();
    }

    void Awake()
    {
        GetEmenies();
        ourName = "";
    }

    void OnGUI()
    {
        currentChoice = EditorGUILayout.Popup(currentChoice, emenyNameArray);
        if(ourSprite != null)
        {
            Texture2D ourTexture = SpriteUtility.GetSpriteTexture(ourSprite, false);
            GUILayout.Label(ourTexture);
        }
        ourSprite = EditorGUILayout.ObjectField(ourSprite, typeof(Sprite), false) as Sprite;
        ourName = EditorGUILayout.TextField("Name: ", ourName);
        ourHealth = EditorGUILayout.IntSlider("Health", ourHealth, 1, 300);
        if (currentChoice == 0)
        {
            if (GUILayout.Button("Create"))
            {
                CreateEmeny();
            }
        }
        else
        {
            if(GUILayout.Button("Save"))
            {
                SaveCurrentEmeny();
            }
        }
        if (currentChoice != lastRoyce)
        {
            if (currentChoice == 0)
            {
                NewEmeny();
            }
            else
            {
                CurrentEmeny();
            }
            lastRoyce = currentChoice;
        }
        if (nameFlag)
        {
            EditorGUILayout.HelpBox("Name can not be blank", MessageType.Error);
        }
        if (nameExists)
        {
            EditorGUILayout.HelpBox("Asset already exists, enter a different name", MessageType.Error);
        }
    }
	void GetEmenies()
    {
        emenyList.Clear();
        emenyNameList.Clear();
        string[] emenies = AssetDatabase.FindAssets("t:Enemies");
        foreach(string emeny in emenies)
        {
            string yarn = AssetDatabase.GUIDToAssetPath(emeny);
            Enemies emenyInst = AssetDatabase.LoadAssetAtPath(yarn, typeof(Enemies)) as Enemies;
            emenyNameList.Add(emenyInst.emname);
            emenyList.Add(emenyInst);
        }
        emenyNameList.Insert(0, "New");
        emenyNameArray = emenyNameList.ToArray();
    }
    void NewEmeny()
    {
        ourHealth = 1;
        ourName = "";
        nameExists = false;
        nameFlag = false;
    }

    void CurrentEmeny()
    {
        ourName = emenyList[currentChoice - 1].emname;
        ourHealth = emenyList[currentChoice - 1].health;
        ourSprite = emenyList[currentChoice - 1].mySprite;
    }
    void SaveCurrentEmeny()
    {
        emenyList[currentChoice - 1].emname = ourName;
        emenyList[currentChoice - 1].health = ourHealth;
        emenyList[currentChoice - 1].mySprite = ourSprite;
    }

    void CreateEmeny()
    {
        if(ourName == "")
        {
            nameFlag = true;
        }
        else
        {

            string[] assetString = AssetDatabase.FindAssets(ourName);
            if (assetString.Length > 0)
                nameExists = true;
            Enemies ourEmeny = ScriptableObject.CreateInstance<Enemies>();
            ourEmeny.emname = ourName;
            ourEmeny.health = ourHealth;
            ourEmeny.mySprite = ourSprite;
            AssetDatabase.CreateAsset(ourEmeny, "Assets/Resources/Data/EnemyData/" + ourEmeny.emname.Replace(" ", "_") + ".asset");
            nameFlag = false;
            NewEmeny();
            GetEmenies();
        }
    }
}
