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
using UnityEngine.Video;
using System.IO;

public class SpawnMenu : MonoBehaviour
{
    private GameObject character;
    public GameObject foodSet;
    public GameObject Spawn;
    public GameObject plane;
    public GameObject FoodFilter;
    //public GameObject qrFrame;
    public GameObject[] MarketCharacters;
    public Menu[][] foodPrefabs;
    public AceMenu[] AceMenus;
    [SerializeField]
    private VideoPlayer VP;
    private List<GameObject> Particles = new List<GameObject>();
    // Start is called before the first frame update
    GameObject DetectAR;
    GameObject SpawnedObject = null;
    GameObject charObject=null;
    GameObject _plane;
    public ARSessionOrigin arSessionOrigin;

    private Restaurant MainRestaurant = null;
    private GetDataFromQR _GetDataFromQR;
    private StoreData _StoreData;
    // À½½Ä ÀÌµ¿
    private RaycastHit hitLayerDish;
    private LayerMask dishLayerMask;
    private bool isDrag = false;
    private Transform toDrag;
    private Vector3 offset;
    private Vector3 multipleAngle;
    private float dist;

    //À½½Ä º¯°æ
    private int ListIndex = 0;
    private int menuIndex = 0;
    private GameObject showFood = null;

    private int FoodType=0;

    private void Start()
    {
        ARCameraManager aR= new ARCameraManager();
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
                offset = hitLayerDish.point - SpawnedObject.transform.position;
                isDrag = true;
            }
            else
            {
                if(FoodType == 2)
                {
                    if(!Physics.Raycast(ray,out hitLayerDish, Mathf.Infinity, dishLayerMask))
                    {
                        foreach(GameObject obj in Particles)
                        {
                            if (!obj.activeSelf)
                            {
                                obj.SetActive(true);
                            }
                        }
                    }
                }
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
                    SpawnedObject.transform.Rotate(0f, -150f * Time.deltaTime, 0f);
                }
                else
                {
                    SpawnedObject.transform.Rotate(0f, 150f * Time.deltaTime, 0f);
                }
            }
            else if(touch.position.x>secondTouch.position.x)
            {
                if (secondTouchPos.y > 0)
                {
                    SpawnedObject.transform.Rotate(0f, 150f * Time.deltaTime, 0f);
                }
                else
                {
                    SpawnedObject.transform.Rotate(0f, -150f * Time.deltaTime, 0f);
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
                SpawnedObject.transform.position = Pos - offset;
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
        if (_GetDataFromQR.isGetInfo())
        {
            if (MainRestaurant == null)
            {
                MainRestaurant = _StoreData.GetMenu(_GetDataFromQR.marketInfo().name);
                foodPrefabs = MainRestaurant.totalMenu;
                AceMenus = MainRestaurant.aceMenus;
            }
        }
        return _GetDataFromQR.isGetInfo() && (DetectAR != null);
    }
    public void SpawnItem()
    {
        if (IsReady())
        {
            FoodType = 0;
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
                SpawnedObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                InstantiateFood(menuIndex);
                Debug.Log("¸ÞÅ¸¸ù µð¹ö±ë : " + SpawnedObject.transform.position);
            }
        }
        else
        {
            Debug.Log("¸ÞÅ¸¸ù µð¹ö±ë : QR´Ù½Ã Âï¾î¶ó");
        }
    }
    public void SpawnAceItem(int idx)
    {
        if (IsReady())
        {
            FoodFilter.SetActive(false);
            if (SpawnedObject != null) Destroy(SpawnedObject);
            Ray ray = arSessionOrigin.camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            LayerMask layerMask = LayerMask.GetMask("Plane");

            //RaycastHit[] hits = Physics.RaycastAll(ray);
            RaycastHit hitLayerMask;
            Vector3 Pos;
            Vector3 Rot;
            Debug.Log("¸ÞÅ¸¸ù µð¹ö±ë : »ý¼ºÀü " + AceMenus[idx].baseMenu.menuPrefab.name);
            if (Physics.Raycast(ray, out hitLayerMask, Mathf.Infinity, layerMask))
            {
                Pos = hitLayerMask.point;
                Rot = hitLayerMask.transform.eulerAngles;
                SpawnedObject = Instantiate(foodSet, Pos, Quaternion.Euler(Rot));
                SpawnedObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                Debug.Log("¸ÞÅ¸¸ù µð¹ö±ë : ´ëÇ¥¸Þ´º SpawnedObject"+SpawnedObject.transform.position);
                InstantiateAceFood(idx);
                FoodType = AceMenus[idx].type;
                if(FoodType == 1)
                {
                    VP.clip = AceMenus[idx].filter;
                    Debug.Log("¸ÞÅ¸¸ù µð¹ö±ë : " + VP.clip);
                    FoodFilter.SetActive(true);
                    VP.Play();
                }
                else if(FoodType == 2)
                {
                    Particles.Clear();
                    for(int i = 0; i < showFood.transform.childCount; i++)
                    {
                        if (showFood.transform.GetChild(i).CompareTag("Particles"))
                        {
                            int tmp = i;
                            Particles.Add(showFood.transform.GetChild(tmp).gameObject);
                            Debug.Log("¸ÞÅ¸¸ù µð¹ö±ë : " + Particles.Count);
                        }
                    }
                }
                Debug.Log("¸ÞÅ¸¸ù µð¹ö±ë : " + SpawnedObject.transform.position);
            }
        }
        else
        {
            Debug.Log("¸ÞÅ¸¸ù µð¹ö±ë : QR´Ù½Ã Âï¾î¶ó");
        }
    }
    private void InstantiateAceFood(int index)
    {
        if (SpawnedObject != null)
        {
            if (showFood != null)
            {
                Destroy(showFood);
            }
            showFood = Instantiate(AceMenus[index].baseMenu.menuPrefab);
            Debug.Log("¸ÞÅ¸¸ù µð¹ö±ë : " + showFood.transform.position);
            Debug.Log("¸ÞÅ¸¸ù µð¹ö±ë : " + showFood);
            showFood.transform.parent = SpawnedObject.transform;
            //showFood.transform.localScale = Vector3.one * 0.7f;
            //showFood.transform.localEulerAngles = Vector3.zero;
            showFood.transform.localPosition = Vector3.zero;
            showFood.transform.localEulerAngles = Vector3.zero; 
            Debug.Log("¸ÞÅ¸¸ù µð¹ö±ë :" + showFood.transform.parent.name);
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
                charObject = Instantiate(character, Pos, Quaternion.Euler(Rot));
                Debug.Log("¸ÞÅ¸¸ù µð¹ö±ë : " + charObject.transform.position);
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
        else if(idx > foodPrefabs[listIndex].Length - 1)
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
            showFood = Instantiate(foodPrefabs[ListIndex][index].menuPrefab,SpawnedObject.transform.position,Quaternion.identity);
            Debug.Log("¸ÞÅ¸¸ù µð¹ö±ë : " + showFood.transform.position);
            Debug.Log("¸ÞÅ¸¸ù µð¹ö±ë : " + showFood);
            showFood.transform.parent = SpawnedObject.transform;
            showFood.transform.localScale = Vector3.one * 0.7f;
            showFood.transform.localEulerAngles = Vector3.zero;
            showFood.transform.localPosition = Vector3.zero;
            Debug.Log("¸ÞÅ¸¸ù µð¹ö±ë :" + showFood.transform.parent.name);
        }
    }
    public void ChangeReact()
    {
        charObject.GetComponent<MiniCharacterGame>().ChangeReact();
    }
    public void DestroyObjects()
    {
        if (showFood != null)
        {
            Destroy(showFood);
        }
        if (SpawnedObject != null)
        {
            Destroy(SpawnedObject);
        }

        if(charObject != null)
        {
            Destroy(charObject);
        }
        if (FoodFilter.activeSelf)
        {
            FoodFilter.SetActive(false);
        }
    }
    public void SetCharacter(int idx)
    {
        character = MarketCharacters[idx];
    }
}
