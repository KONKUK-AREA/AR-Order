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


public class UIControlManagerForObjMenuClone : MonoBehaviour
{
    public string menuName;
    public TextMeshProUGUI TotalPriceForEachMenu; // 메뉴별 총 금액
    public static int TotalPriceInCart=0;    // 장바구니 안 총 금액
    private int menuPrice;
    private TextMeshProUGUI[] textComponents;
    private UIControlManager manager;
    

    private int currentValue = 1;
    
    
    private void Start() {
        textComponents = GetComponentsInChildren<TextMeshProUGUI>();
        manager = GameObject.Find("Canvas").GetComponent<UIControlManager>();
    }
    public void SetInit(){
        foreach (TextMeshProUGUI TXT_MenuInfo in textComponents)
        {
            if (TXT_MenuInfo.CompareTag("MenuPriceTag"))
            {
                menuPrice = int.Parse(TXT_MenuInfo.text);
                Debug.Log(menuPrice);
            }
        }
        TotalPriceForEachMenu.text = menuPrice.ToString();
    }
    public void ClickedCountUpButton()
    {

        currentValue++;

        Debug.Log("why");


        foreach (TextMeshProUGUI TXT_Cart_MenuInfo in textComponents)
        {
            if (TXT_Cart_MenuInfo.CompareTag("MenuCountTag"))
            {
                TXT_Cart_MenuInfo.text = currentValue.ToString(); 
            }
        }

        TotalPriceInCart += menuPrice;
        UpdateCart();
    }




    public void ClickedCountDownButton()
    {
        if(currentValue>1)
        {
        currentValue--;
        Debug.Log("why");

        foreach (TextMeshProUGUI TXT_Cart_MenuInfo in textComponents)
        {
            if (TXT_Cart_MenuInfo.CompareTag("MenuCountTag"))
            {
                TXT_Cart_MenuInfo.text = currentValue.ToString(); 
            }
        }
        }
        TotalPriceInCart -= menuPrice;
        UpdateCart();
    }
    

    private void UpdateCart(){
        TotalPriceForEachMenu.text= (menuPrice*currentValue).ToString();
        foreach(TextMeshProUGUI Text in manager.TextTotalPriceCart){
            Text.text = TotalPriceInCart.ToString();
        }
    }

    
    








}
