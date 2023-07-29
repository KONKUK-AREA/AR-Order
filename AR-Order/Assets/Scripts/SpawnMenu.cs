using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Linq;
using UnityEngine.Rendering.RendererUtils;
using UnityEngine.UIElements;
using Unity.VisualScripting;


public class SpawnMenu : MonoBehaviour
{
    public GameObject tempChar;
    public GameObject tempFood;
    public GameObject Spawn;
    public GameObject plane;
    public GameObject qrFrame;
    // Start is called before the first frame update
    GameObject DetectAR;
    GameObject SpawnedObject;
    GameObject charObject;
    GameObject _plane;
    public ARSessionOrigin arSessionOrigin;

    // ���� �̵�
    private RaycastHit hitLayerDish;
    private LayerMask dishLayerMask;
    private bool isDrag = false;
    private Transform toDrag;
    private Vector3 offset;
    private Vector3 multipleAngle;
    private float dist;


    private void Start()
    {
        //GameObject gm = Instantiate(Spawn, new Vector3(0, 0, 180), Quaternion.Euler(-90, 0, 0));
    }
    // Update is called once per frame

    void Update()
    {
        if(Input.touchCount == 0)
        {
            isDrag = false;
            return;
        }
        Vector3 vec;
        Touch touch = Input.touches[0];
        Vector3 pos = touch.position;

        if (touch.phase == TouchPhase.Began)
        {
            dishLayerMask = LayerMask.GetMask("Dish");
            Ray ray = arSessionOrigin.camera.ScreenPointToRay(pos);
            if (Physics.Raycast(ray, out hitLayerDish, Mathf.Infinity, dishLayerMask))
            {
                isDrag = true;
            }
        }
        if(isDrag && touch.phase == TouchPhase.Moved)
        {
            LayerMask layerMask = LayerMask.GetMask("Plane");
            RaycastHit hitLayerMask;
            Vector3 Pos;
            Vector3 Rot;
            Ray ray = arSessionOrigin.camera.ScreenPointToRay(pos);
            if (Physics.Raycast(ray, out hitLayerMask, Mathf.Infinity, layerMask))
            {
                Pos = hitLayerMask.point;
                //Rot = hitLayerMask.transform.eulerAngles;
                hitLayerDish.transform.position = Pos; 
                //Debug.Log("��Ÿ�� ����� : " + SpawnedObject.transform.position);
            }
        }
        if(isDrag && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
        {
            isDrag = false;
        }

        
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
            LayerMask layerMask = LayerMask.GetMask("Plane");

            //RaycastHit[] hits = Physics.RaycastAll(ray);
            RaycastHit hitLayerMask;
            Vector3 Pos;
            Vector3 Rot;
            if (Physics.Raycast(ray, out hitLayerMask, Mathf.Infinity, layerMask))
            {
                Pos = hitLayerMask.point;
                Rot = hitLayerMask.transform.eulerAngles;
                SpawnedObject = Instantiate(tempFood, Pos, Quaternion.Euler(Rot));
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
            qrFrame.SetActive(false);
        }
        else
        {
            Debug.Log("��Ÿ�� ����� : QR�νľȵ�");
        }
    }
    public void SpawnCharacter()
    {
        if (IsReady())
        {
            if (charObject != null) Destroy(charObject);
            Ray ray = arSessionOrigin.camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            LayerMask layerMask = LayerMask.GetMask("Plane");

            //RaycastHit[] hits = Physics.RaycastAll(ray);
            RaycastHit hitLayerMask;
            Vector3 Pos;
            Vector3 Rot;
            if (Physics.Raycast(ray, out hitLayerMask, Mathf.Infinity, layerMask))
            {
                Pos = hitLayerMask.point;
                Rot = hitLayerMask.transform.eulerAngles;
                charObject = Instantiate(tempChar, Pos, Quaternion.Euler(Rot));
                Debug.Log("��Ÿ�� ����� : " + SpawnedObject.transform.position);
            }
        }
        else
        {
            Debug.Log("��Ÿ�� ����� : QR�ٽ� ����");
        }
    }   
}
