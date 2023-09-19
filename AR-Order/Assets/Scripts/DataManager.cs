using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public List<int> CharacterIndex = new List<int>();
    public int Coupon;
    public bool isFoodTutorial;
    public bool isGameTutorial;
}
public class DataManager : MonoBehaviour
{
    // Start is called before the first frame update
    string path;
    private void Start()
    {
        path = Path.Combine(Application.persistentDataPath, "database.json");
        JsonLoad();
    }
    public void JsonLoad()
    {
        SaveData saveData= new SaveData();
        if (!File.Exists(path))
        {
            GameManager.instance.isFoodTutorial = false;
            GameManager.instance.isGameTutorial = false;
            GameManager.instance.Coupon = 0;
        }
        else
        {
            string loadJson = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);

            if(saveData != null)
            {
                for(int i = 0; i < saveData.CharacterIndex.Count; i++)
                {
                    GameManager.instance.CharacterIndex.Add(saveData.CharacterIndex[i]);
                }
                GameManager.instance.isFoodTutorial = saveData.isFoodTutorial;
                GameManager.instance.isGameTutorial=saveData.isGameTutorial;
                GameManager.instance.Coupon = saveData.Coupon;
            }

        }
    }
    public void JsonSave()
    {
        SaveData saveData= new SaveData();
        for(int i = 0; i < GameManager.instance.CharacterIndex.Count; i++)
        {
            saveData.CharacterIndex.Add(GameManager.instance.CharacterIndex[i]);
        }
        saveData.isGameTutorial=GameManager.instance.isGameTutorial;
        saveData.isFoodTutorial=GameManager.instance.isFoodTutorial;
        saveData.Coupon = GameManager.instance.Coupon;
        string json = JsonUtility.ToJson(saveData,true);
        File.WriteAllText(path,json);
    }
}
