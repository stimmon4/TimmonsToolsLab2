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
    int ourAgility;
    int ourAttack;
    int ourDefense;
    int ourManaPool;

    float ourAttackSpeed;

    bool isMagicUser = false;
    bool nameFlag = false;
    bool nameExists = false;
    bool spriteFlag = false;

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
        ourAgility = EditorGUILayout.IntSlider("Agility", ourAgility, 1, 100);
        ourAttack = EditorGUILayout.IntSlider("Attack", ourAttack, 1, 100);
        ourDefense = EditorGUILayout.IntSlider("Defense", ourDefense, 1, 100);
        ourAttackSpeed = EditorGUILayout.Slider("Attack Speed", ourAttackSpeed, 1, 20);
        isMagicUser = EditorGUILayout.Toggle("Magic User", isMagicUser);
        if (isMagicUser)
        {
            ourManaPool = EditorGUILayout.IntSlider("Mana", ourManaPool, 1, 100);
        }
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
        if (spriteFlag)
        {
            EditorGUILayout.HelpBox("Sprite cannot be blank", MessageType.Error);
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
        ourSprite = null;
        ourAgility = 1;
        ourAttack = 1;
        ourDefense = 1;
        ourAttackSpeed = 1;
        isMagicUser = false;
        ourManaPool = 1;
        nameExists = false;
        nameFlag = false;
        spriteFlag = false;
    }

    void CurrentEmeny()
    {
        ourName = emenyList[currentChoice - 1].emname;
        ourHealth = emenyList[currentChoice - 1].health;
        ourSprite = emenyList[currentChoice - 1].mySprite;
        ourAgility = emenyList[currentChoice - 1].agi;
        ourAttack = emenyList[currentChoice - 1].atk;
        ourDefense = emenyList[currentChoice - 1].def;
        ourAttackSpeed = emenyList[currentChoice - 1].atkTime;
        isMagicUser = emenyList[currentChoice - 1].isMagic;
        ourManaPool = emenyList[currentChoice - 1].manaPool;
    }
    void SaveCurrentEmeny()
    {
        emenyList[currentChoice - 1].emname = ourName;
        emenyList[currentChoice - 1].health = ourHealth;
        emenyList[currentChoice - 1].mySprite = ourSprite;
        emenyList[currentChoice - 1].agi = ourAgility;
        emenyList[currentChoice - 1].atk = ourAttack;
        emenyList[currentChoice - 1].def = ourDefense;
        emenyList[currentChoice - 1].atkTime = ourAttackSpeed;
        emenyList[currentChoice - 1].isMagic = isMagicUser;
        emenyList[currentChoice - 1].manaPool = ourManaPool;
        EditorUtility.SetDirty(emenyList[currentChoice - 1]);
        AssetDatabase.SaveAssets();
    }

    void CreateEmeny()
    {
        if (string.IsNullOrEmpty(ourName))
        {
            nameFlag = true;
        }
        
        else if(ourSprite == null)
        {
            spriteFlag = true;
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
            ourEmeny.agi = ourAgility;
            ourEmeny.atk = ourAttack;
            ourEmeny.def = ourDefense;
            ourEmeny.atkTime = ourAttackSpeed;
            ourEmeny.isMagic = isMagicUser;
            ourEmeny.manaPool = ourManaPool;
            AssetDatabase.CreateAsset(ourEmeny, "Assets/Resources/Data/EnemyData/" + ourEmeny.emname.Replace(" ", "_") + ".asset");
            nameFlag = false;
            NewEmeny();
            GetEmenies();
        }
    }
}
