using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Linq;
using UnityEngine.Rendering.RendererUtils;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using System.Runtime.InteropServices;

public class SpawnMenu : MonoBehaviour
{
    public GameObject character;
    public GameObject foodSet;
    public GameObject Spawn;
    public GameObject plane;
    //public GameObject qrFrame;

    public Menu[][] foodPrefabs;
    // Start is called before the first frame update
    GameObject DetectAR;
    GameObject SpawnedObject;
    GameObject charObject;
    GameObject _plane;
    public ARSessionOrigin arSessionOrigin;

    private Restaurant MainRestaurant;
    private GetDataFromQR _GetDataFromQR;
    private StoreData _StoreData;
    // ���� �̵�
    private RaycastHit hitLayerDish;
    private LayerMask dishLayerMask;
    private bool isDrag = false;
    private Transform toDrag;
    private Vector3 offset;
    private Vector3 multipleAngle;
    private float dist;

    //���� ����
    private int ListIndex = 0;
    private int menuIndex = 0;
    private GameObject showFood;

    private void Start()
    {
        //GameObject gm = Instantiate(Spawn, new Vector3(0, 0, 180), Quaternion.Euler(-90, 0, 0));
        _GetDataFromQR = GetComponent<GetDataFromQR>();
        _StoreData= GetComponent<StoreData>();
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
        if (_GetDataFromQR.isGetInfo())
        {
            MainRestaurant = _StoreData.GetMenu(_GetDataFromQR.marketInfo().name);
            foodPrefabs = MainRestaurant.totalMenu;
        }
        return _GetDataFromQR.isGetInfo() && (DetectAR != null);
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
                SpawnedObject = Instantiate(foodSet, Pos, Quaternion.Euler(Rot));
                SpawnedObject.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                InstantiateFood(menuIndex);
                Debug.Log("��Ÿ�� ����� : " + SpawnedObject.transform.position);
            }
        }
        else
        {
            Debug.Log("��Ÿ�� ����� : QR�ٽ� ����");
        }
    }


    public void SetPlane()
    {
        if (IsReady())
        {
            _plane = Instantiate(plane, DetectAR.transform.position, DetectAR.transform.rotation);
            //qrFrame.SetActive(false);
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
                charObject = Instantiate(character, Pos, Quaternion.Euler(Rot));
                Debug.Log("��Ÿ�� ����� : " + SpawnedObject.transform.position);
            }
        }
        else
        {
            Debug.Log("��Ÿ�� ����� : QR�ٽ� ����");
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
                Instantiate(foodSet, hits[0].pose.position, hits[0].pose.rotation);
            }
        }
    }
    public void ChangeFoodIndex(int listIndex, int idx)
    {
        if (idx < 0)
        {
            idx = foodPrefabs[listIndex].Length - 1;
        }
        else if(idx > foodPrefabs.Length - 1)
        {
            idx = 0;
        }
        ListIndex = listIndex;
        menuIndex = idx;
        if (SpawnedObject != null)
        {
            InstantiateFood(idx);
        }
    }
    public void NextFoodIndex()
    {
        ChangeFoodIndex(ListIndex,menuIndex + 1);
    }
    public void PrevFoodIndex()
    {
        ChangeFoodIndex(ListIndex,menuIndex - 1);
    }

    private void InstantiateFood(int index)
    {
        if (SpawnedObject != null)
        {
            if (showFood != null)
            {
                Destroy(showFood);
            }
            showFood = Instantiate(foodPrefabs[ListIndex][index].menuPrefab);
            Debug.Log("��Ÿ�� ����� : " + showFood.transform.position);
            Debug.Log("��Ÿ�� ����� : " + showFood);
            showFood.transform.parent = SpawnedObject.transform;
            showFood.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            showFood.transform.localEulerAngles = Vector3.zero;
            showFood.transform.localPosition = Vector3.zero;
        }
    }
    
    
    public void ChangeReact()
    {
        charObject.GetComponent<MiniCharacterGame>().ChangeReact();
    }
    
}
