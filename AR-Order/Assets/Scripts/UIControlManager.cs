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
public class UIControlManager : MonoBehaviour
{
    public GameObject Layer_LoadingPage;
    public Text TXT_MenuName;


    public GameObject[] UIForEachScreen;   // element 0이 시작화면이고 element 0부터 각 ui레이어의 값    
    public GameObject[] LayersARTutorial;
    
    public GameObject[] MenuForRestaurant_A;
    public Button[] ButtonForRestaurant_A;



    public GameObject PrevBtn;
    public GameObject[] NextBtn;

    public Text MenuName;




    public int UIForEachScreen_currentIndex = 0;
    public int LayersARTutorial_currentIndex = 0;

    // Start is called before the first frame update


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
        // 현재 Layer 오브젝트를 비활성화
        LayersARTutorial[LayersARTutorial_currentIndex].SetActive(false);

        LayersARTutorial_currentIndex ++;

        LayersARTutorial[LayersARTutorial_currentIndex].SetActive(true);
        
        if(LayersARTutorial_currentIndex>3)
        {
            LayersARTutorial[2].SetActive(false);
            UIForEachScreen[UIForEachScreen_currentIndex].SetActive(true);
        }



    }






    public void ClickedMenu () 
    {
        
        for (int i = 0; i < ButtonForRestaurant_A.Length; i++)        //ButtonForRestaurant_A[]의 모든 배열 원소에 대하여
            {
                if (ButtonForRestaurant_A[i] != null)
                {
                    ButtonForRestaurant_A[i].onClick.AddListener(ChangeMenuName);            //onclick시 changemenuname함수 실행
                }
            }

    }


    public void ChangeMenuName()
    {
        

        // // 클릭된 버튼의 이름을 확인
        // string buttonName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        
        // // B 텍스트 오브젝트의 Text 컴포넌트를 가져와서 텍스트를 "coffee"로 변경
        // if (MenuName != null)
        // {
        //     MenuName.text = "coffee";
        // }


        // Button clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        // if (clickedButton != null)
        // {
        //     string buttonText = clickedButton.GetComponentInChildren<Text>().text;
           

        // }
        //  Debug.Log(resultText.text);






        GameObject ClickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        Text TextObject_ClickedMenuName = ClickedButton.transform.Find("TXT_MenuName")?.GetComponent<Text>();

        GameObject MenuNameDisplayButton = GameObject.Find("TXT_DisplayedMenuName");
        Text TextObject_MenuName = MenuNameDisplayButton.transform.Find("TXT_DisplayedMenuName")?.GetComponent<Text>();
        string TextObject_DisplayMenuName = TextObject_MenuName.text;
        
        
        if (ClickedButton != null)
        {
            string MenuName = TextObject_ClickedMenuName.text;
            Debug.Log("클릭한 메뉴의 이름: " + MenuName);
            TextObject_MenuName.text = MenuName;
            //TextObject_DisplayMenuName.text = MenuName;
        }

        if (TextObject_MenuName != null)
        {
            

            // Text 컴포넌트의 텍스트를 클릭한 메뉴의 이름으로 변경합니다.
            //TextObject_MenuName.text = MenuName;
        }





    }







    









    void Start()
    {
        // // 초기화 시 첫 번째 UI 화면만 활성화
        // for (int i = 0; i < UIForEachScreen.Length; i++)
        // {
        //     UIForEachScreen[i].SetActive(i == 0);
        // }


        StartCoroutine(StartAROrder());
        
    }

    IEnumerator StartAROrder()
    {
        yield return new WaitForSeconds(1.0f);
        Layer_LoadingPage.SetActive(false);                 // 로딩페이지
        UIForEachScreen[0].SetActive(true);                 // qr인식 페이지
                       
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }






}
