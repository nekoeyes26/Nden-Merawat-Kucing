using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public enum CatState
{
    Unnamed,
    Named
}

public enum CatPhase
{
    Baby,
    Child,
    Adult
}
[CreateAssetMenu(fileName = "New Cat", menuName = "Cat")]
public class CatScriptable : ScriptableObject
{
    public Sprite preview;
    public int id;
    [SerializeField] private string catName;
    public int xp;
    public int xpNeeded;
    public int level;
    public CatPhase phase;
    public int hungryRemaining;
    public int showerRemaining;
    public int playRemaining;
    public int photoRemaining;
    public CatState state;
    public bool isHungry = false;
    public bool isDirty = false;
    public bool isSad = false;
    public bool isSick = false;
    public string spriteFolderPath;
    public string animationFolderPath;

    public void Awake()
    {
        state = CatState.Unnamed;
        phase = CatPhase.Baby;
    }

    public new string name
    {
        get { return catName; }
        set
        {
            state = CatState.Named;
            catName = value;
        }
    }

    [System.Serializable]
    public class ScriptableObjectData
    {
        public string catName;
        public int xp;
        public int xpNeeded;
        public int level;
        public CatPhase phase;
        public int hungryRemaining;
        public int showerRemaining;
        public int playRemaining;
        public int photoRemaining;
        public CatState state;
        public bool isHungry;
        public bool isDirty;
        public bool isSad;
        public bool isSick;
    }

    private string GetSavePath()
    {
        // string path = AssetDatabase.GetAssetPath(this);
        // string fileName = System.IO.Path.GetFileName(path);
        // string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fileName);
        string fileNameWithoutExtension = "Cat_" + id.ToString();
        string catDataPath = Path.Combine(Application.persistentDataPath, "CatData");
        if (!Directory.Exists(catDataPath))
        {
            Directory.CreateDirectory(catDataPath);
        }
        return Path.Combine(catDataPath, fileNameWithoutExtension + ".json");
    }

    public void Save()
    {
        ScriptableObjectData data = new ScriptableObjectData();
        data.catName = catName;
        data.xp = xp;
        data.xpNeeded = xpNeeded;
        data.level = level;
        data.phase = phase;
        data.hungryRemaining = hungryRemaining;
        data.showerRemaining = showerRemaining;
        data.playRemaining = playRemaining;
        data.photoRemaining = photoRemaining;
        data.state = state;
        data.isHungry = isHungry;
        data.isDirty = isDirty;
        data.isSad = isSad;
        data.isSick = isSick;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(GetSavePath(), json);
        Debug.Log("Saving Scriptable");
    }

    public void Load()
    {
        string path = GetSavePath();
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            ScriptableObjectData data = JsonUtility.FromJson<ScriptableObjectData>(json);

            catName = data.catName;
            xp = data.xp;
            xpNeeded = data.xpNeeded;
            level = data.level;
            phase = data.phase;
            hungryRemaining = data.hungryRemaining;
            showerRemaining = data.showerRemaining;
            playRemaining = data.playRemaining;
            photoRemaining = data.photoRemaining;
            state = data.state;
            isHungry = data.isHungry;
            isDirty = data.isDirty;
            isSad = data.isSad;
            isSick = data.isSick;
        }
        else
        {
            Debug.LogWarning("Save file not found: " + path);
        }
    }
}
