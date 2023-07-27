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
        Debug.Log("��Ÿ�� ����� : "+DetectAR);
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
            Debug.Log("��Ÿ�� ����� : " + idx);
            if (idx >= 0)
            {
                Pos = hits[idx].point;
                Rot = hits[idx].transform.eulerAngles;
                Debug.Log(Rot);
                SpawnedObject = Instantiate(KU, Pos, Quaternion.Euler(Rot));
                Debug.Log("��Ÿ�� ����� : " + SpawnedObject.transform.position);
            }
        }
        else
        {
            Debug.Log("��Ÿ�� ����� : QR�ٽ� ����");
        }
    }

    public int HitFind(string name, RaycastHit[] array)
    {
        if (array.Length == 0) {
            Debug.Log("�������");
            return -1;
        }


        foreach (var hit in array)
        {
            Debug.Log("��Ÿ�� ����� : " + hit.transform.gameObject.name + " vs " + name);
        }
        foreach (var item in array.Select((value, index) => (value, index)))
        {
            var value = item.value;
            var index = item.index;
            Debug.Log("��Ÿ�� ����� : " + value + " " + index);
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
            Debug.Log("��Ÿ�� ����� : QR�νľȵ�");
        }
    }
}
