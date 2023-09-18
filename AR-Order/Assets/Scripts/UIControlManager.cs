using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.XR.ARSubsystems;
using System.Linq;
using UnityEditor;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using System.Security.Cryptography;
using TMPro;
using UnityEngine.Video;

public class UIControlManager : MonoBehaviour
{
    public GameObject Layer_LoadingPage;
    public TextMeshProUGUI[] TotalPriceInCart;    // 장바구니 안 총 금액
    public TextMeshProUGUI TotalCount;

    private int maxMenuCount = 0;

    [SerializeField]
    private SpawnMenu _SpawnMenu;
    [SerializeField]
    private GetDataFromQR _GetDataFromQR;
    [SerializeField]
    private StoreData _StoreData;
    private Restaurant MainRestaurant=null;

    public GameObject[] UIForEachScreen;   // element 0이 시작화면이고 element 0부터 각 ui레이어의 값    
    public GameObject[] LayersARTutorial;
    public Sprite[] Characters;
    public GameObject CharacterList;
    public GameObject Coupon;
    public TextMeshProUGUI TXT_MyPage;
    private int UIForEachScreen_currentIndex = 0;
    private int LayersARTutorial_currentIndex = 0;

    public DataManager DM;
     

    public GameObject[] ARMenuForRestaurant_A;
    public Button[] MenuButtonForRestaurant_A;




    public TextMeshProUGUI TXT_ChoosedMenuName; 
    public TextMeshProUGUI TXT_ChoosedMenuPrice;






    public TextMeshProUGUI TXT_Cart_ChoosedMenuName;
    public TextMeshProUGUI TXT_Cart_ChoosedMenuPrice;





    private Button clickedButton ;
    public Button ClickedNextMenuButton;

    

    public GameObject CartMenu;


    public GameObject Layer_AROrderTutorial;
    public GameObject Layer_RestautrantMenu;
    public Transform canvasTransform;
    public Transform Layer_Cart_Transform;
    public Transform Layer_Cart_ScrollView_Transform;



    [SerializeField]
    private GameObject[] showCartCount;
    [SerializeField]
    private TextMeshProUGUI[] showCartCountText;

    public Sprite[] ARGameTutorial;
    public Image ARGameTutorialImg;

    [SerializeField]
    private TextMeshProUGUI[] MenuExplanation;

    [SerializeField]
    private TextMeshProUGUI[] MenuTypeText;
    [SerializeField]
    private TextMeshProUGUI RestaurantNameText;

    // public Image MenuImage;
    // public TextMeshProUGUI CountForEachMenu;
    // public TextMeshProUGUI TotalPriceForEachMenu;
    private int currentValue = 1;
    private int ARMenuIndex;
    private int ListIndex;
    private GameObject spawnedMenuObject = null;
    private GameObject spawnedObject = null;
    private int Layer_AROrderTutorial_Activated = 0;
    
    private int CurrentCharacter;



    private void ClickedMenuBtn(int ARMenuIndex)
    {
        this.ARMenuIndex = ARMenuIndex;
        //3d오브젝트 생성
        if (ARMenuIndex < ARMenuForRestaurant_A.Length)
        {
            if (spawnedMenuObject !=null)
            {
                Destroy(spawnedMenuObject);
            }
            Quaternion rotation = Quaternion.identity;
            GameObject objectToSpawn = ARMenuForRestaurant_A[ARMenuIndex];

        

            spawnedMenuObject = Instantiate(objectToSpawn, canvasTransform.position , rotation , canvasTransform);
            //spawnedMenuObject.transform.position =  Vector3.zero;
        }
        else
        {
            Debug.LogError("버튼 " + ARMenuIndex + "의 오브젝트가 ARMenuForRestaurant_A 배열에 없습니다.");
        }

        ChangeMenuInfo(ARMenuIndex);
        ClickedNextBtn();



    }
    public GameObject AceMenuContent;
    public GameObject[] MenuListContent;
    public void InitMenu()
    {
        
        _SpawnMenu.SetCharacter(MainRestaurant.CharacterIdx);
        RestaurantNameText.text = MainRestaurant.Name;
        TXT_MyPage.text = MainRestaurant.Name;
        CurrentCharacter = MainRestaurant.CharacterIdx;
        for(int i = 0; i<MainRestaurant.MenuType.Length; i++)
        {
            MenuTypeText[i].text = MainRestaurant.MenuType[i];
        }
        Debug.Log("메타몽 디버깅 : 메뉴 타입 설명 설정");
        for(int i = 0; i< AceMenuContent.transform.childCount; i++)
        {
            GameObject aceMenu = AceMenuContent.transform.GetChild(i).gameObject;
            if(i >= MainRestaurant.aceMenus.Length)
            {
                aceMenu.SetActive(false);
                continue;
            }
            int temp = i;
            aceMenu.GetComponent<Button>().onClick.AddListener(() => ClickAceMenu(temp));
            aceMenu.GetComponent<Image>().sprite = MainRestaurant.aceMenus[i].aceImage;
        }
        Debug.Log("메타몽 디버깅 : 대표메뉴 설정 끝");
        for(int i = 0; i< MenuListContent.Length; i++)
        {
            if(i >= MainRestaurant.totalMenu.Length)
            {
                MenuListContent[i].SetActive(false);
                continue;
            }
            for(int j = 0; j < MenuListContent[i].transform.childCount; j++)
            {
                GameObject obj = MenuListContent[i].transform.GetChild(j).gameObject;
                if (j >= MainRestaurant.totalMenu[i].Length)
                {
                    obj.SetActive(false);
                    continue;
                }
                int temp1 = i;
                int temp2 = j;
                obj.GetComponent<Button>().onClick.AddListener(() => ClickMenuBtn(
                    temp1, temp2));
                obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = MainRestaurant.totalMenu[i][j].name;
                obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = MainRestaurant.totalMenu[i][j].price.ToString()+"원";
                obj.GetComponent<Image>().sprite = MainRestaurant.totalMenu[i][j].Img;
                Debug.Log("메타몽 디버깅 : 설정중"+MainRestaurant.totalMenu[i][j].name);
            }
        }
        int idx=0;
        for (idx = 0; idx < GameManager.instance.CharacterIndex.Count; idx++)
        {
            if (MainRestaurant.CharacterIdx == GameManager.instance.CharacterIndex[idx])
                break;
        }
        if (idx == GameManager.instance.CharacterIndex.Count)
        {
            GameManager.instance.CharacterIndex.Add(MainRestaurant.CharacterIdx);
        }
        Debug.Log("메타몽 디버깅 : InitMenu 끝");
    }

    public void InitMyPage()
    {
        for(int i = 0; i < GameManager.instance.CharacterIndex.Count; i++)
        {
            int tmp = GameManager.instance.CharacterIndex[i];
            CharacterList.transform.GetChild(tmp).GetChild(0).GetComponent<Image>().sprite = Characters[tmp];
            CharacterList.transform.GetChild(tmp).GetComponent<Button>().onClick.AddListener(() => ChangeCharacter(tmp));
        }
        for(int i = 0; i< GameManager.instance.Coupon; i++)
        {
            Coupon.transform.GetChild(i).GetComponent<Image>().color = new Color(255, 255, 255, 255);
        }
        CharacterList.transform.GetChild(CurrentCharacter).GetComponent<Image>().color = new Color(200, 255, 0,255);
    }
    public void ChangeCharacter(int idx)
    {
        CharacterList.transform.GetChild(CurrentCharacter).GetComponent<Image>().color = new Color(255, 255, 255, 255);
        CurrentCharacter = idx;
        _SpawnMenu.SetCharacter(CurrentCharacter);
        CharacterList.transform.GetChild(CurrentCharacter).GetComponent<Image>().color = new Color(200, 255, 0, 255);
    }
    public void ClickMenuBtn(int listIndex, int index) 
    {
        maxMenuCount = MainRestaurant.totalMenu[listIndex].Length;
        ARMenuIndex = index;
        ListIndex = listIndex;
        GetMenuInfo();
        _SpawnMenu.ChangeFoodIndex(listIndex,index);
        ChangeLayer(2);
        _SpawnMenu.SpawnItem();
        if (!GameManager.instance.isFoodTutorial)
        {
            ShowTutorial();
            GameManager.instance.isFoodTutorial = true;
            DM.JsonSave();
        }
    }
    public TextMeshProUGUI TXT_AceMenuName;
    public TextMeshProUGUI TXT_AceMenuPrice;
    public TextMeshProUGUI[] AceMenuExplanation;
    public void ClickAceMenu(int index)
    {
        ARMenuIndex = MainRestaurant.aceMenus[index].index[1];
        ListIndex = MainRestaurant.aceMenus[index].index[0];
        TXT_AceMenuName.text = MainRestaurant.aceMenus[index].baseMenu.name;
        TXT_AceMenuPrice.text = MainRestaurant.aceMenus[index].baseMenu.price.ToString() + "원";
        string[] splitDesc = MainRestaurant.aceMenus[index].baseMenu.Description.Split(new char[] { ':' });
        AceMenuExplanation[0].text = splitDesc[0];
        AceMenuExplanation[1].text = splitDesc[1];
        Debug.Log("메타몽 디버깅 : " + TXT_AceMenuName.text + " " + AceMenuExplanation[0].text);
        ChangeLayer(7); // 대표메뉴 UI로 바꾸기
        _SpawnMenu.SpawnAceItem(index);

        if (!GameManager.instance.isFoodTutorial)
        {
            ShowTutorial();
            GameManager.instance.isFoodTutorial = true;
            DM.JsonSave();
        }
    }
    public void GetMenuInfo()
    {
        TXT_ChoosedMenuName.text = MainRestaurant.totalMenu[ListIndex][ARMenuIndex].name;
        TXT_ChoosedMenuPrice.text = MainRestaurant.totalMenu[ListIndex][ARMenuIndex].price.ToString() + "원";
        string[] splitDesc = MainRestaurant.totalMenu[ListIndex][ARMenuIndex].Description.Split(new char[] {':'});
        MenuExplanation[0].text = splitDesc[0];
        MenuExplanation[1].text = splitDesc[1];
    }
    public void ChangeMenuInfo(int ARMenuIndex)
    {
        if (ARMenuIndex < MenuButtonForRestaurant_A.Length)
        {
            clickedButton = MenuButtonForRestaurant_A[ARMenuIndex];

            // 클릭된 버튼의 자식들중 텍스트메쉬오브젝트를 가지고있는 놈들의 배열 => TXTs_clickedButton
            TextMeshProUGUI[] TXTs_clickedButton = clickedButton.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI TXT_MenuInfo in TXTs_clickedButton)
            {
                if (TXT_MenuInfo.CompareTag("MenuNameTag"))
                {
                    TXT_ChoosedMenuName.text = TXT_MenuInfo.text ;
            
                }

                if (TXT_MenuInfo.CompareTag("MenuPriceTag"))
                {
                    TXT_ChoosedMenuPrice.text = TXT_MenuInfo.text +"원";
            
                }
            }
        }
        else
        {
            Debug.LogError("버튼 " + ARMenuIndex + "의 오브젝트가 MenuButtonForRestaurant_A 배열에 없습니다.");
        }

    }

    public List<GameObject> cartList = new List<GameObject>();
    private int count = 0;
    public void AddToCart()
    {
        /*
        TextMeshProUGUI[] TXTs_clickedButton = clickedButton.GetComponentsInChildren<TextMeshProUGUI>();     //클릭한 메뉴의 정보
        Image[] IMGs_clickedButton = clickedButton.GetComponentsInChildren<Image>();

        String menuName ="";
        int menuPrice=0;
        Sprite menuImg=null;
        foreach (TextMeshProUGUI TXT_MenuInfo in TXTs_clickedButton)
        {
            if (TXT_MenuInfo.CompareTag("MenuNameTag"))
            {
                menuName = TXT_MenuInfo.text ; 
            }

            if (TXT_MenuInfo.CompareTag("MenuPriceTag"))
            {
                menuPrice = int.Parse(TXT_MenuInfo.text);
            }
        }
        */
        String menuName = MainRestaurant.totalMenu[ListIndex][ARMenuIndex].name;
        int menuPrice = MainRestaurant.totalMenu[ListIndex][ARMenuIndex].price;
        Sprite menuImg = MainRestaurant.totalMenu[ListIndex][ARMenuIndex].Img;
        foreach (GameObject cart in cartList)
        {
            UIControlManagerForObjMenuClone manager = cart.GetComponent<UIControlManagerForObjMenuClone>();
            if (manager.menuName.Equals(menuName))
            {
                manager.ClickedCountUpButton();
                return;
            }
        }
        Debug.Log("hi");
        Quaternion rotation = Quaternion.identity;
        RectTransform CartRect = CartMenu.gameObject.GetComponent<RectTransform>();
        spawnedObject = Instantiate(CartMenu,  Layer_Cart_ScrollView_Transform.position, rotation, Layer_Cart_ScrollView_Transform);
        // spawnedObject.GetComponent<RectTransform>().anchoredPosition3D = CartRect.anchoredPosition3D;
        spawnedObject.GetComponent<RectTransform>().localPosition = CartRect.localPosition + Vector3.down * 40 + Vector3.right*100 + Vector3.down*300*cartList.Count;
        count++;








        TextMeshProUGUI[] TextComponent = spawnedObject.GetComponentsInChildren<TextMeshProUGUI>();   // 스폰된 메뉴 박스의 텍스트 정보
        Image[] ImageComponent = spawnedObject.GetComponentsInChildren<Image>();



        


        
        cartList.Add(spawnedObject);
        spawnedObject.GetComponent<UIControlManagerForObjMenuClone>().SetInit(menuName,menuPrice,menuImg);
        if (!showCartCount[0].activeSelf)
        {
            showCartCount[0].SetActive(true);
            showCartCount[1].SetActive(true);
            showCartCount[2].SetActive(true);
            showCartCount[3].SetActive(true);
            showCartCount[4].SetActive(true);
        }
        
    }

    public void RemoveCart(String name)
    {
        int i;
        for(i = 0; i< cartList.Count; i++)
        {
            UIControlManagerForObjMenuClone manager = cartList[i].GetComponent<UIControlManagerForObjMenuClone>();
            if (manager.menuName.Equals(name))
            {
                break;
            }
        }
        GameObject toRemove = cartList[i].gameObject;
        cartList.RemoveAt(i);
        Destroy(toRemove);
        for(int j = i; j<cartList.Count; j++)
        {
            cartList[j].GetComponent<RectTransform>().localPosition -= (Vector3.down * 300);

        }


        if (cartList.Count == 0)
        {
            showCartCount[0].SetActive(false);
            showCartCount[1].SetActive(false);
            showCartCount[2].SetActive(false);
            showCartCount[3].SetActive(false);
            showCartCount[4].SetActive(false);
        }

    }

    public void StartCartEvent()
    {
        StartCoroutine(AddCartAnim(0));
        StartCoroutine(AddCartAnim(1));
        StartCoroutine(AddCartAnim(2));
        StartCoroutine(AddCartAnim(3));
        StartCoroutine(AddCartAnim(4));
    }
    IEnumerator AddCartAnim(int idx)
    {
        showCartCount[idx].transform.localScale = Vector3.one * 1.2f;
        showCartCountText[idx].text = TotalCount.text;
        yield return new WaitForSeconds(0.5f);
        showCartCount[idx].transform.localScale = Vector3.one;
    }








    public void ShowTutorial()
    {
        if (Layer_AROrderTutorial_Activated == 0)
        {
            Layer_AROrderTutorial.SetActive(true);
            Layer_AROrderTutorial_Activated = 1;
        }
        else
        {
            Layer_AROrderTutorial.SetActive(false);
        }
    }






    public void ClickedNextMenuBtn()
    {

        _SpawnMenu.NextFoodIndex();
        if(ARMenuIndex == maxMenuCount-1)
        {
            ARMenuIndex = 0;
        }
        else
        {

            ARMenuIndex++;
        }
        GetMenuInfo();
    }


    
    public void ClickedPrevMenuBtn()
    {
        _SpawnMenu.PrevFoodIndex();
        if (ARMenuIndex == 0)
        {
            ARMenuIndex = maxMenuCount-1;
        }
        else
        {
            ARMenuIndex--;
        }
        GetMenuInfo();

    }








    public void ClickedCartButton()
    {
        ChangeLayer(3);
    }

    int previousLayer = 0;

    public void ClickedPrevBtnAtCart()
    {
        ChangeLayer(previousLayer);
    }

    public void ClickedHomeButton()
    {
        ChangeLayer(1);
        if (spawnedMenuObject !=null)
        {
             Destroy(spawnedMenuObject);
        }


    }

    public void QRSuccessful()
    {
        if (_SpawnMenu.IsReady())
        {
            MainRestaurant = _StoreData.GetMenu(_GetDataFromQR.marketInfo().name);
            InitMenu();
            ChangeLayer(1);
        }
    }
    



    public void ClickedNextBtn()
    {
        if (_SpawnMenu.IsReady())
        {
            if (MainRestaurant == null)
            {
                MainRestaurant = _StoreData.GetMenu(_GetDataFromQR.marketInfo().name);
                InitMenu();
            }
            // 현재 Layer 오브젝트를 비활성화
            UIForEachScreen[UIForEachScreen_currentIndex].SetActive(false);

            // 다음 Layer 오브젝트의 인덱스로 이동
            UIForEachScreen_currentIndex++;

            // 배열 범위를 벗어나면 첫 번째 Layer로 이동
            if (UIForEachScreen_currentIndex >= UIForEachScreen.Length)
            {
                UIForEachScreen_currentIndex = 0;
            }

            // 다음 Layer 오브젝트를 활성화
            UIForEachScreen[UIForEachScreen_currentIndex].SetActive(true);
        }
    }
       
    public void ClickedPrevBtn()
    {
        // 현재 Layer 오브젝트를 비활성화
        ChangeLayer(previousLayer);
    
    }

    


    public void ClickedOrderBtn()
    {
        while (cartList.Count > 0)
        {
            String str = cartList[0].GetComponent<UIControlManagerForObjMenuClone>().menuName;
            RemoveCart(str);
        }

        GameManager.instance.Coupon++;
        DM.JsonSave();
        UpdateTotal(0, 0);
        ChangeLayer(4);
    }
    public void ClickedOrderSuccessBtn()
    {
        ChangeLayer(5);
    }



    public void ClickedTutorialNextBtn()
    {
     if (LayersARTutorial_currentIndex < 3)
     {
         // 현재 Layer 오브젝트를 비활성화
         LayersARTutorial[LayersARTutorial_currentIndex].SetActive(false);

         LayersARTutorial_currentIndex++;   

        // 다음 튜토리얼 화면을 활성화
            if (LayersARTutorial_currentIndex < LayersARTutorial.Length)
         {
             LayersARTutorial[LayersARTutorial_currentIndex].SetActive(true);
         }
     }
     else
     {
         // 최대 인덱스 이상으로 넘어갈 때의 처리
         LayersARTutorial[LayersARTutorial_currentIndex].SetActive(false);
         if (UIForEachScreen_currentIndex < UIForEachScreen.Length)
         {
             UIForEachScreen[UIForEachScreen_currentIndex].SetActive(true);
         }
     }
    }












    IEnumerator StartAROrder()
    {
        yield return new WaitForSeconds(1.0f);
        Layer_LoadingPage.SetActive(false);                 // 로딩페이지
        UIForEachScreen[0].SetActive(true);                 // qr인식 페이지
        
    }
    // Update is called once per frame



    void Start()
    {
        StartCoroutine(StartAROrder());

        // MenuButtonIndex받아오는 과정
        /*
        for (int i = 0; i < MenuButtonForRestaurant_A.Length; i++)
        {
            if (MenuButtonForRestaurant_A[i] == null)
            {
                maxMenuCount = i - 1;
                break;
            }
            int MenuButtonIndex = i;
            MenuButtonForRestaurant_A[i].onClick.AddListener(() => ClickedMenuBtn(MenuButtonIndex));
        }
        */
        
    }
    public void UpdateTotal(int price,int count)
    {
        foreach(TextMeshProUGUI cart in TotalPriceInCart)
        {
            cart.text = price.ToString();
        }
        TotalCount.text = count.ToString();
    }

    int CurrentLayer = 0;
    public void ChangeLayer(int layer)
    {
        if(layer == 6)
        {
            _SpawnMenu.DestroyObjects();
        }
        previousLayer = CurrentLayer;
        if(layer == 2 || layer == 7)
        {
            _SpawnMenu.DestroyObjects();
            
        }
        UIForEachScreen[CurrentLayer].SetActive(false);
        UIForEachScreen[layer].SetActive(true);
        CurrentLayer = layer;
    }

    int index = 0;
    public void _ARGameTutorial()
    {
        if (GameManager.instance.isGameTutorial)
        {
            ARGameTutorialImg.gameObject.SetActive(false);
            return;
        }
        if(index == ARGameTutorial.Length)
        {
            GameManager.instance.isGameTutorial = true;
            DM.JsonSave();
            ARGameTutorialImg.gameObject.SetActive(false);
        }
        ARGameTutorialImg.sprite = ARGameTutorial[++index];
      
    }


}
