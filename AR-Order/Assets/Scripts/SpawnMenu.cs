using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Linq;

public class SpawnMenu : MonoBehaviour
{
    public GameObject KU;
    public GameObject Spawn;
    public GameObject plane;
    // Start is called before the first frame update
    GameObject DetectAR;
    GameObject SpawnedObject;
    GameObject _plane;
    public ARSessionOrigin arSessionOrigin;
    private void Start()
    {
        //GameObject gm = Instantiate(Spawn, new Vector3(0, 0, 180), Quaternion.Euler(-90, 0, 0));
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsReady()
    {
        DetectAR = GameObject.FindWithTag("Detector");
        Debug.Log("¸ÞÅ¸¸ù µð¹ö±ë : "+DetectAR);
        return GetComponent<GetDataFromQR>().isGetInfo() && (DetectAR != null);
    }
    public void SpawnItem()
    {
        if (IsReady())
        {
            if (SpawnedObject != null) Destroy(SpawnedObject);
            Ray ray = arSessionOrigin.camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit[] hits = Physics.RaycastAll(ray);
            Debug.Log(hits[0]);
            Vector3 Pos;
            Vector3 Rot;
            int idx = HitFind("MenuPlane(Clone)", hits);
            Debug.Log("¸ÞÅ¸¸ù µð¹ö±ë : " + idx);
            if (idx >= 0)
            {
                Pos = hits[idx].point;
                Rot = hits[idx].transform.eulerAngles;
                Debug.Log(Rot);
                SpawnedObject = Instantiate(KU, Pos, Quaternion.Euler(Rot));
                Debug.Log("¸ÞÅ¸¸ù µð¹ö±ë : " + SpawnedObject.transform.position);
            }
        }
        else
        {
            Debug.Log("¸ÞÅ¸¸ù µð¹ö±ë : QR´Ù½Ã Âï¾î¶ó");
        }
    }

    public int HitFind(string name, RaycastHit[] array)
    {
        if (array.Length == 0) {
            Debug.Log("ºñ¾îÀÖÀ½");
            return -1;
        }


        foreach (var hit in array)
        {
            Debug.Log("¸ÞÅ¸¸ù µð¹ö±ë : " + hit.transform.gameObject.name + " vs " + name);
        }
        foreach (var item in array.Select((value, index) => (value, index)))
        {
            var value = item.value;
            var index = item.index;
            Debug.Log("¸ÞÅ¸¸ù µð¹ö±ë : " + value + " " + index);
            if (value.transform.gameObject.name.Equals(name))
            {
                return index;
            }
        }
        return -1;
    }
    public void SetPlane()
    {
        if (IsReady())
        {
            _plane = Instantiate(plane, DetectAR.transform.position, DetectAR.transform.rotation);
        }
        else
        {
            Debug.Log("¸ÞÅ¸¸ù µð¹ö±ë : QRÀÎ½Ä¾ÈµÊ");
        }
    }
}
