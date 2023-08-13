using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonLoader : MonoBehaviour
{
 
    public static JsonInfo loadJson(string jsonText)
    {
        JsonInfo data = JsonUtility.FromJson<JsonInfo>(jsonText);
        return data;
    }

}
public class JsonInfo
{
    public string name;
}
