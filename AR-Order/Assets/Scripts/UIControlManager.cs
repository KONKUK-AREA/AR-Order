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


public class UIControlManager : MonoBehaviour
{
    public GameObject Layer_LoadingPage;
    public TextMeshProUGUI TotalPriceInCart_1 ;    // 장바구니 안 총 금액






    public GameObject[] UIForEachScreen;   // element 0이 시작화면이고 element 0부터 각 ui레이어의 값    
    public GameObject[] LayersARTutorial;
    private int UIForEachScreen_currentIndex = 0;
    private int LayersARTutorial_currentIndex = 0;
   



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










    // public Image MenuImage;
    // public TextMeshProUGUI CountForEachMenu;
    // public TextMeshProUGUI TotalPriceForEachMenu;
    private int currentValue = 1;
    private int ARMenuIndex;

    private GameObject spawnedMenuObject = null;
    private GameObject spawnedObject = null;
    private int Layer_AROrderTutorial_Activated = 0;



    public TextMeshProUGUI TotalPriceInCart;

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
        ShowTutorial();

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



    private int count = 0;
    public void AddToCart()
    {   
        Debug.Log("hi");
        Quaternion rotation = Quaternion.identity;
        RectTransform CartRect = CartMenu.gameObject.GetComponent<RectTransform>();
        spawnedObject = Instantiate(CartMenu,  Layer_Cart_ScrollView_Transform.position, rotation, Layer_Cart_ScrollView_Transform);
        // spawnedObject.GetComponent<RectTransform>().anchoredPosition3D = CartRect.anchoredPosition3D;
        spawnedObject.GetComponent<RectTransform>().localPosition = CartRect.localPosition + Vector3.down * 40 + Vector3.right*100 + Vector3.down*300*count;
        count++;





        TextMeshProUGUI[] TXTs_clickedButton = clickedButton.GetComponentsInChildren<TextMeshProUGUI>();     //클릭한 메뉴의 정보
        Image[] IMGs_clickedButton = clickedButton.GetComponentsInChildren<Image>();



        TextMeshProUGUI[] TextComponent = spawnedObject.GetComponentsInChildren<TextMeshProUGUI>();   // 스폰된 메뉴 박스의 텍스트 정보
        Image[] ImageComponent = spawnedObject.GetComponentsInChildren<Image>();



        
        foreach (TextMeshProUGUI TXT_MenuInfo in TXTs_clickedButton)
        {
            if (TXT_MenuInfo.CompareTag("MenuNameTag"))
            {
                foreach (TextMeshProUGUI TXT_Cart_MenuInfo in TextComponent)
                {   
                    if (TXT_Cart_MenuInfo.CompareTag("MenuNameTag")) 
                    {
                        TXT_Cart_MenuInfo.text = TXT_MenuInfo.text;  
                    }
                }
            }

            if (TXT_MenuInfo.CompareTag("MenuPriceTag"))
            {
                foreach (TextMeshProUGUI TXT_Cart_MenuInfo in TextComponent)
                {   
                    if (TXT_Cart_MenuInfo.CompareTag("MenuPriceTag"))  // 기존 코드의 조건문 오류 수정
                    {
                        TXT_Cart_MenuInfo.text =  TXT_MenuInfo.text ;  // 스폰된 메뉴 박스의 텍스트를 클릭한 메뉴의 가격으로 설정
                    }
                }
            }
        }

        foreach (Image IMG_MenuIMG in IMGs_clickedButton)
        {


             if (IMG_MenuIMG.CompareTag("MenuImageTag"))
            {
                foreach (Image IMG_Cart_MenuIMG in ImageComponent)
                {   
                    if (IMG_Cart_MenuIMG.CompareTag("MenuImageTag")) 
                    {
                        IMG_Cart_MenuIMG.sprite = IMG_MenuIMG.sprite;  
                    }
                }
            }
        }
        
        CartMenu.GetComponent<UIControlManagerForObjMenuClone>().SetInit();

        


    }



    // public void ClickedCountUpButton()
    // {
    //     currentValue++;
    //     // CountForEachMenu.text = currentValue.ToString();

    //     Debug.Log("up");

    
    //     TextMeshProUGUI[] TextComponent = GetComponentsInParent<TextMeshProUGUI>();


    //     foreach (TextMeshProUGUI TXT_Cart_MenuInfo in TextComponent)
    //     {
    //         if (TXT_Cart_MenuInfo.CompareTag("MenuCountTag"))
    //         {
    //             TXT_Cart_MenuInfo.text = currentValue.ToString(); 
    //         }
    //     }
    // }


    // public void ClickedCountDownButton()
    // {
    //     if(currentValue >1 )
    //     {
    //     currentValue--;
    //     // CountForEachMenu.text = currentValue.ToString();

    //     Debug.Log("down");

    
    //     TextMeshProUGUI[] TextComponent = GetComponentsInParent<TextMeshProUGUI>();


    //     foreach (TextMeshProUGUI TXT_Cart_MenuInfo in TextComponent)
    //     {
    //         if (TXT_Cart_MenuInfo.CompareTag("MenuCountTag"))
    //         {
    //             TXT_Cart_MenuInfo.text = currentValue.ToString(); 
    //         }
    //     }
    //     }
    // }
    
    

    // public void ChangeTotalPriceForEachMenu()
    // {
    //     int int_PriceForEachMenu ;   // 기본값으로 초기화
    //     string MenuPrice;

    //     TextMeshProUGUI[] TXTs_clickedButton = clickedButton.GetComponentsInChildren<TextMeshProUGUI>();

    //     foreach (TextMeshProUGUI TXT_MenuInfo in TXTs_clickedButton)
    //     {
    //         if (TXT_MenuInfo.CompareTag("MenuPriceTag"))
    //         {
    //             MenuPrice = TXT_MenuInfo.text;
                
    //             if (int.TryParse(MenuPrice, out int_PriceForEachMenu))
    //             {
    //                 TotalPriceForEachMenu.text = "총 가격: " + (currentValue * int_PriceForEachMenu) + "원";
    //             }
    //             else
    //             {
    //                 TotalPriceForEachMenu.text = "가격 정보를 가져올 수 없습니다.";
    //             }
    //         }
    //     }
    // }
    









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

        if (spawnedMenuObject !=null)
        {
             Destroy(spawnedMenuObject);
        }

        ClickedNextMenuButton = MenuButtonForRestaurant_A[ARMenuIndex+1];

        TextMeshProUGUI[] TXTs_clickedButtonNextMenu = ClickedNextMenuButton.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI TXT_MenuInfo in TXTs_clickedButtonNextMenu)
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
        ARMenuIndex ++;
        clickedButton = MenuButtonForRestaurant_A[ARMenuIndex];



        Quaternion rotation = Quaternion.identity;
        GameObject objectToSpawn = ARMenuForRestaurant_A[ARMenuIndex];

        

        spawnedMenuObject = Instantiate(objectToSpawn, canvasTransform.position , rotation , canvasTransform);
    }


    
    public void ClickedPrevMenuBtn()
    {
        if (spawnedMenuObject !=null)
        {
             Destroy(spawnedMenuObject);
        }

        ClickedNextMenuButton = MenuButtonForRestaurant_A[ARMenuIndex-1];

        TextMeshProUGUI[] TXTs_clickedButtonNextMenu = ClickedNextMenuButton.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI TXT_MenuInfo in TXTs_clickedButtonNextMenu)
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
        ARMenuIndex --;
        clickedButton = MenuButtonForRestaurant_A[ARMenuIndex];

        Quaternion rotation = Quaternion.identity;
        GameObject objectToSpawn = ARMenuForRestaurant_A[ARMenuIndex];

        

        spawnedMenuObject = Instantiate(objectToSpawn, canvasTransform.position , rotation , canvasTransform);



    }








    public void ClickedCartButton()
    {
        UIForEachScreen[2].SetActive(false);
        UIForEachScreen[3].SetActive(true);    
        UIForEachScreen_currentIndex = 3;

    }

    public void ClickedPrevBtnAtCart()
    {
        UIForEachScreen[UIForEachScreen_currentIndex].SetActive(false);
        UIForEachScreen[2].SetActive(true);


    }

    public void ClickedHomeButton()
    {
        UIForEachScreen[UIForEachScreen_currentIndex].SetActive(false);
        UIForEachScreen[1].SetActive(true);  
        UIForEachScreen_currentIndex = 1;

        if (spawnedMenuObject !=null)
        {
             Destroy(spawnedMenuObject);
        }


    }




    public void ClickedNextBtn()
    {
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
       
    public void ClickedPrevBtn()
    {
        // 현재 Layer 오브젝트를 비활성화
        UIForEachScreen[UIForEachScreen_currentIndex].SetActive(false);

        // 이전 Layer 오브젝트의 인덱스로 이동
        UIForEachScreen_currentIndex--;

        // 배열 범위를 벗어나면 마지막 Layer로 이동
        if (UIForEachScreen_currentIndex < 0)
        {
            UIForEachScreen_currentIndex = UIForEachScreen.Length - 1;
        }

        // 이전 Layer 오브젝트를 활성화
        UIForEachScreen[UIForEachScreen_currentIndex].SetActive(true);


    
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
        for (int i = 0; i < MenuButtonForRestaurant_A.Length; i++)
        {
            int MenuButtonIndex = i;
            MenuButtonForRestaurant_A[i].onClick.AddListener(() => ClickedMenuBtn(MenuButtonIndex));
        }
        
    }

    public TextMeshProUGUI[] TextTotalPriceCart;
    void Update()
    {

    }






}
