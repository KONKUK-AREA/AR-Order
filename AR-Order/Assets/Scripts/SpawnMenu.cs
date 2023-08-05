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

    // À½½Ä ÀÌµ¿
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
        Touch touch = Input.touches[0];
        Touch secondTouch = touch;
        Vector2 secondTouchPrePos = Vector2.zero;
        bool isSecond = false;
        Vector2 secondTouchPos = Vector2.zero;
        if (Input.touchCount > 1)
        {
            secondTouch = Input.touches[1];
            isSecond = true;
        }

        Vector3 pos = touch.position;

        if (touch.phase == TouchPhase.Began)
        {
            dishLayerMask = LayerMask.GetMask("Dish");
            Ray ray = arSessionOrigin.camera.ScreenPointToRay(pos);
            if (Physics.Raycast(ray, out hitLayerDish, Mathf.Infinity, dishLayerMask))
            {
                offset = hitLayerDish.point - hitLayerDish.transform.position;
                isDrag = true;
            }
        }
        if(isSecond && secondTouch.phase == TouchPhase.Began)
        {
            secondTouchPrePos = secondTouch.position;
        }
        if (isSecond && secondTouch.phase == TouchPhase.Moved)
        {
            secondTouchPos = secondTouch.deltaPosition;

            if (touch.position.x < secondTouch.position.x)
            {
                if (secondTouchPos.y > 0)
                {
                    hitLayerDish.transform.Rotate(0f, -150f * Time.deltaTime, 0f);
                }
                else
                {
                    hitLayerDish.transform.Rotate(0f, 150f * Time.deltaTime, 0f);
                }
            }
            else if(touch.position.x>secondTouch.position.x)
            {
                if (secondTouchPos.y > 0)
                {
                    hitLayerDish.transform.Rotate(0f, 150f * Time.deltaTime, 0f);
                }
                else
                {
                    hitLayerDish.transform.Rotate(0f, -150f * Time.deltaTime, 0f);
                }
            }
        }
        if (isDrag && touch.phase == TouchPhase.Moved)
        {

            LayerMask layerMask = LayerMask.GetMask("Plane");
            RaycastHit hitLayerMask;
            Vector3 Pos;
            Ray ray = arSessionOrigin.camera.ScreenPointToRay(pos);
            if (Physics.Raycast(ray, out hitLayerMask, Mathf.Infinity, layerMask))
            {
                Pos = hitLayerMask.point;
                //Rot = hitLayerMask.transform.eulerAngles;
                hitLayerDish.transform.position = Pos - offset;
                //Debug.Log("¸ÞÅ¸¸ù µð¹ö±ë : " + SpawnedObject.transform.position);
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
        Debug.Log("¸ÞÅ¸¸ù µð¹ö±ë : "+DetectAR);
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
                Debug.Log("¸ÞÅ¸¸ù µð¹ö±ë : " + SpawnedObject.transform.position);
            }
        }
        else
        {
            Debug.Log("¸ÞÅ¸¸ù µð¹ö±ë : QR´Ù½Ã Âï¾î¶ó");
        }
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
            Debug.Log("¸ÞÅ¸¸ù µð¹ö±ë : QRÀÎ½Ä¾ÈµÊ");
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
                Debug.Log("¸ÞÅ¸¸ù µð¹ö±ë : " + SpawnedObject.transform.position);
            }
        }
        else
        {
            Debug.Log("¸ÞÅ¸¸ù µð¹ö±ë : QR´Ù½Ã Âï¾î¶ó");
        }
    }

    private ARRaycastManager raycastMgr;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public void SpawnItemWithARPlane()
    {
        Vector2 screenCenterPos = arSessionOrigin.camera.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));

        if (GetComponent<GetDataFromQR>().isGetInfo())
        {
            if (raycastMgr.Raycast(screenCenterPos, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
            {
                Instantiate(tempFood, hits[0].pose.position, hits[0].pose.rotation);
            }
        }
    }

    
    public void ChangeReact()
    {
        charObject.GetComponent<MiniCharacterGame>().ChangeReact();
    }
}
